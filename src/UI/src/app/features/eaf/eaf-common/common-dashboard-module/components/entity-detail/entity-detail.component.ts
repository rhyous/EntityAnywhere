import { Component, OnInit } from '@angular/core'
import { Router, ActivatedRoute } from '@angular/router'
import { Observable } from 'rxjs'

import { EntityService } from 'src/app/core/services/entity.service'
import { EntityMetadataService } from 'src/app/core/services/entity-metadata.service'
import { EntityPropertyControlService } from '../../../common-form-module/services/entity-property-control.service'
import { SpaceTitlePipe } from 'src/app/core/pipes/spacetitle.pipe'
import { ErrorReporterDialogComponent } from 'src/app/core/dialogs/error-reporter-dialog/error-reporter-dialog.component'
import { EntityHelperService } from '../../../common-form-module/services/entity-helper.service'
import { FieldConfig } from '../../../common-form-module/models/interfaces/field-config.interface'
import { WcfFormatPipe } from 'src/app/core/pipes/wcf-format.pipe'
import { EntityMetadata } from 'src/app/core/models/concretes/entity-metadata'
import { PluralizePipe } from 'src/app/core/pipes/pluralize.pipe'
import { GlobalSnackBarService } from 'src/app/core/services/global-snack-bar.service'
import { ArraySortOrderPipe } from 'src/app/core/pipes/array-sort-order.pipe'
import { IEntityDetailParams } from '../../interfaces/entity-detail-params.interface'
import { ODataCollection } from 'src/app/core/models/interfaces/o-data-collection.interface'
import { ODataObject } from 'src/app/core/models/interfaces/o-data-entities/o-data-object.interface'
import { EntityField } from '../../../common-form-module/models/concretes/entity-field'
import { UserDataService } from 'src/app/core/services/user-data.service'
import { SingularizePipe } from 'src/app/core/pipes/singularize.pipe'
import { EntityConfigBuilder } from '../../../common-form-module/services/entity-config-builder'
import '../../../../../../core/infrastructure/linq'
import { NullEx } from 'src/app/core/infrastructure/extensions/null-ex'
import { AnyEx } from 'src/app/core/infrastructure/extensions/any-ex'
import { WellKnownProperties } from 'src/app/core/models/concretes/well-known-properties'
import { StringEx } from 'src/app/core/infrastructure/extensions/string-ex'
import { ThisReceiver } from '@angular/compiler'

@Component({
  selector: 'app-entity-detail',
  styleUrls: ['entity-detail.component.scss'],
  templateUrl: 'entity-detail.component.html',
  providers: [EntityPropertyControlService, SpaceTitlePipe, WcfFormatPipe]
})

/**
 * The Entity Detail Component. Responsible for displaying information about a specific entity
 */
export class EntityDetailComponent implements OnInit {
  entityConfigs!: FieldConfig[]
  mappingComponents: any = []
  extensionEntities: any = []
  extensionEntityData: any = []
  foreignEntities: any = []
  entityId!: any
  entityName!: string
  entityData: any
  showProgressBar = false
  entityProperties!: any[]
  mappingOrder!: 1

  title!: string

  params!: IEntityDetailParams

  permittedEntitiesForUser = [{name: '', isReadOnly: false}]

  private oDataObj!: ODataObject<any>

  constructor(
    private entityService: EntityService,
    private entityMetadataService: EntityMetadataService,
    private helper: EntityHelperService,
    private epcs: EntityPropertyControlService,
    private route: ActivatedRoute,
    private router: Router,
    public snackBar: GlobalSnackBarService,
    private errorReporter: ErrorReporterDialogComponent,
    private wcfFormatPipe: WcfFormatPipe,
    private pluralize: PluralizePipe,
    private arraySortByOrder: ArraySortOrderPipe,
    private userDataService: UserDataService,
    private singularize: SingularizePipe,
    private entityConfigBuilder: EntityConfigBuilder,
    private wellKnownProperties: WellKnownProperties
  ) { }

  //#region Page load and form creation

  ngOnInit() {
    this.route.params.subscribe((params: any) => {
      this.params = params as IEntityDetailParams
      this.init()
    })
  }

  init() {
    this.entityConfigs = []
    this.mappingComponents = []
    this.extensionEntities = []
    this.foreignEntities = []
    this.entityProperties = []


    this.entityId = this.params.id !== 'clone' ? this.params.id.toString() : this.entityId
    this.entityName = this.params.entity

    this.showProgressBar = true
    if (this.params.id === 'add') {
      this.title =  `Add ${this.entityName}`
      this.formInitialization(this.params, undefined)
    } else if (this.params.id === 'clone') {
      // So the way this works is by making the angular lifecycle work for us
      // What happens if we change the route but stay in this component is that
      // angular won't tear this down. Meaning we can re-use the data
      // If however the user has navigated here by using the url
      // there will be no data and therefore nothing to clone
      if (!this.entityId) {
        this.router.navigate([`/admin/data-administration/${this.pluralize.transform(this.entityName)}`])
      }

      this.title = `Clone ${this.entityName}`
      this.entityService.getEntityData(this.entityName, this.entityId).subscribe((response: any) => {
        if (response && response.Object) {
          this.formInitialization(this.params, response.Object)
        }
      })
      this.showProgressBar = false

    } else {
      // Need to get entity data
      this.title = `${this.entityName}`
      this.entityService.getEntityData(this.entityName, this.entityId).subscribe((response: any) => {
        if (response && response.Object) {
          this.oDataObj = response
          this.formInitialization(this.params, response.Object)
        }
      })
    }
  }

  formInitialization(params: IEntityDetailParams, entityData: any) {
    const entityMeta = this.entityMetadataService.getEntity(this.entityName)
    this.setupForm(entityMeta, params, entityData)
    this.showProgressBar = false
  }

  setupForm(entityMeta: EntityMetadata, params: IEntityDetailParams, entityData: any) {
    this.mappingComponents = []
    this.entityConfigs = []

    const filteredFields = entityMeta.Fields.filter(field => !field.isAuditable(this.wellKnownProperties.auditableProperties)
                                                          && this.wellKnownProperties.idProperty !== field.Name)
    filteredFields.forEach(field => {

      if (!field.isNavigationProperty()) {
        this.handleNonNavigationPropertyField(field, entityData, entityMeta)
        return
      }

      if (params.id !== 'add' && params.id !== 'clone' ) {

        const isFieldTypePermittedForUser = this.userDataService.canDisplayEntityForUser(this.singularize.transform(field.Name))

        if (field.isMapping() && isFieldTypePermittedForUser) {
          this.handleMappingField(field, entityData, entityMeta)
          return
        }
        if (field.isExtension) {
          this.handleExtensionField(field, isFieldTypePermittedForUser)
          return
        }
        if (field.isForeign && isFieldTypePermittedForUser) {
          // get foreign entity to see if it is a mapping type
          const foreignEntity = this.entityMetadataService.getEntity(field.Type.substr('self.'.length))
          if (!foreignEntity.isMappingEntityType) {
            this.foreignEntities.push(field)
          }
        }
      }
    })

    this.entityConfigs = this.arraySortByOrder.transform(this.entityConfigs, false)
    if (this.mappingComponents.length > 0) {
      this.mappingComponents.splice(0, 0, {name: 'Associated', label: 'Associated Records', type: 'label', inputType: 'Title', order: 0})
    }
  }

  handleNonNavigationPropertyField(field: EntityField, entityData: any, entityMetadata: EntityMetadata) {
    const navField = field.hasNavigationKey() ? entityMetadata.getField(field.NavigationKey) : undefined
    const fieldConfig = this.entityConfigBuilder.build(this.entityName, field, entityData, navField)
    this.entityConfigs.push(fieldConfig)
  }

  handleMappingField(field: EntityField, entityData: any, entityMetadata: EntityMetadata) {
    if (this.canDisplay(field, entityData, entityMetadata) &&
         this.userDataService.canDisplayEntityForUser(field.MappingEntity)) {

      this.mappingComponents.push(
        this.epcs.getMappingComponent(this.entityName, field.MappingEntity,
        field.Type.substring(5), field.Name, entityData.Id, field.Alias, this.mappingOrder++, field.EntityAlias))
    }
  }

  handleExtensionField(field: EntityField, isFieldTypePermittedForUser: boolean) {

    field.ReadOnly = !isFieldTypePermittedForUser
    this.extensionEntities.push(field)
  }

  //#endregion

  returnToList() {
    this.router.navigate([`admin/data-administration/${this.pluralize.transform(this.entityName)}`])
  }

  //#region Form Submission

  submit(eventData: any) {
    this.showProgressBar = true
    this.entityData = this.helper.removeAuditableProperties(eventData, this.wellKnownProperties.auditableProperties)

    if (this.params.id.toString().toLowerCase() === 'add' || this.params.id.toString().toLowerCase() === 'clone') {
      this.addEntity(this.entityData).subscribe((next: any) => {
        this.onSaveSuccess(next)
      })
    } else {
      this.updateEntity(this.entityData).subscribe((next: any) => this.onPatchSuccess(next))
    }
  }

  addEntity(addObject: any) {
    Object.keys(addObject).forEach(property => {
      // If the value is a Date Convert dates to WCF format.
      const conf: any = this.entityConfigs.find(x => x.name === property)
      if (conf.type === 'date') {
        addObject[property] = this.wcfFormatPipe.transform(addObject[property])
      }
    })
    return this.entityService.addEntity(this.entityName, [addObject])
  }

  updateEntity(combinedObject: any) {
    // Remove unchanged values and build patch object
    Object.keys(combinedObject).forEach(property => {
      if (combinedObject[property] === this.entityConfigs.find((x: any) => x.name === property)?.value) {
        delete combinedObject[property]
      } else {
        // If the changed value is a Date Convert dates to WCF format.
        const conf: any = this.entityConfigs.find(x => x.name === property)
        if (conf.type === 'date') {
          combinedObject[property] = this.wcfFormatPipe.transform(combinedObject[property])
        }
      }
    })

    const patchObject = { ChangedProperties: Object.keys(combinedObject), Entity: combinedObject }
    if (patchObject.ChangedProperties.length === 0) {
      this.snackBar.open('No changes detected')
      this.showProgressBar = false
      return new Observable<any>()
    }

    return this.entityService.patchEntity(this.entityName, this.entityId.toString(), patchObject)
  }

  clone() {
    this.router.navigate([`/admin/data-administration/${this.entityName}/clone`])
  }

  onSaveSuccess(entity: ODataCollection<any>) {
    if (entity && entity.Entities && entity.Entities.length > 0) {
      this.onPatchSuccess(entity.Entities.first())
    }
  }

  onPatchSuccess(entity: ODataObject<any>): void {
    this.showProgressBar = false
    if (entity) {
      if (entity.Id.toString() === this.entityId) {
        this.snackBar.open(`${this.entityName} ${this.entityId} saved successfully`)
        // Go back over the entity config to change the changed values
        // This is much cheaper than calling this.init()
        entity.Object = this.helper.removeAuditableProperties(entity.Object, this.wellKnownProperties.auditableProperties)
        Object.keys(entity.Object).forEach(property => {
          const entityConfigProperty = this.entityConfigs.find(y => y.name === property)
          // If we have an entityConfigProperty and that value has changed
          if (entityConfigProperty && entityConfigProperty.value !== entity.Object[property]) {
            entityConfigProperty.value = entity.Object[property]
          }
        })

      } else {
        this.entityId = entity.Id.toString()
        this.snackBar.open(`${this.entityName} added successfully`)
        // If we wanted to we could bounce people to the edit page and make things a bit more friendly
        this.router.navigate([`admin/data-administration/${this.entityName}/${entity.Id}`])
      }
    }
  }

  onSaveFailure(error: any): void {
    this.errorReporter.displayMessage(error.error)
  }

  foreignEntityFilterId(foreignEntity: any) {
    const referentialConstraint = this.epcs
      .getMappedReferentialConstraint(foreignEntity.Name,
                                      this.entityName,
                                      undefined)
    const result = this.oDataObj.Object[referentialConstraint]
    return result
  }

  foreignEntityProperty(foreignEntity: EntityField) {
    if (NullEx.isNullOrUndefined(foreignEntity) || StringEx.isUndefinedNullOrWhitespace(foreignEntity.Type)) {
      return null
    }
    const entityMeta = this.entityMetadataService.getEntity(foreignEntity.Type)
    return entityMeta.Fields.firstOrDefault(f => f.Name === this.entityName).ReferentialConstraint.LocalProperty
  }

  canDisplay(field: EntityField, entityData: any, ent: EntityMetadata): boolean {
    if (field.DisplayCondition) {
      const conditionDetails = field.DisplayCondition.split(' ')
      const fieldToCheck = conditionDetails[0]
      const condition = conditionDetails[1]
      let valueToCheck: any

      const fmd = ent.getField(fieldToCheck)

      if (fmd.isNumeric()) {
        valueToCheck = +conditionDetails[2]
      } else {
        valueToCheck = conditionDetails[2]
      }

      const fieldToCheckValue = entityData[fieldToCheck]
      if (condition === '!=') {
        if (fieldToCheckValue !== valueToCheck ) {
          return true
        } else {
          return false
        }
      } else {
        if (fieldToCheckValue === valueToCheck ) {
          return true
        } else {
          return false
        }
      }
    }
    return true
  }
  //#endregion
}

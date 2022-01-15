import { Component, Inject, OnInit } from '@angular/core'
import { EntityMetadata } from 'src/app/core/models/concretes/entity-metadata'
import { ArraySortOrderPipe } from 'src/app/core/pipes/array-sort-order.pipe'
import { EntityMetadataService } from 'src/app/core/services/entity-metadata.service'
import { environment } from 'src/environments/environment'
import { FieldConfig } from '../../../common-form-module/models/interfaces/field-config.interface'
import { EntityPropertyControlService } from '../../../common-form-module/services/entity-property-control.service'

import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material'
import { ConfirmDialogComponent } from 'src/app/core/dialogs/confirm-dialog/confirm-dialog.component'
import { PluralizePipe } from 'src/app/core/pipes/pluralize.pipe'
import { EntityFormComponent } from '../../../common-form-module/components/entity-form/entity-form.component'
import { ErrorReporterDialogComponent } from 'src/app/core/dialogs/error-reporter-dialog/error-reporter-dialog.component'
import { EntityService } from 'src/app/core/services/entity.service'
import { GlobalSnackBarService } from 'src/app/core/services/global-snack-bar.service'
import { PatchedEntityCollection } from 'src/app/core/models/interfaces/patch-entity-object.interface'
import { EntityField } from '../../../common-form-module/models/concretes/entity-field'

@Component({
  selector: 'app-entity-multi-update-dialog',
  templateUrl: './entity-multi-update-dialog.component.html',
  styleUrls: ['./entity-multi-update-dialog.component.scss'],
  entryComponents: [EntityFormComponent]
})
export class EntityMultiUpdateDialogComponent implements OnInit {

entityName: string
selectedRecordIds = []
entityConfig: FieldConfig[] = []

constructor(private entityMetadataService: EntityMetadataService,
    private epcs: EntityPropertyControlService,
    private arraySortByOrder: ArraySortOrderPipe,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public matDialog: MatDialog,
    private pluralizePipe: PluralizePipe,
    public dialogRef: MatDialogRef<EntityMultiUpdateDialogComponent>,
    private entityService: EntityService,
    private errorReporter: ErrorReporterDialogComponent,
    public snackBar: GlobalSnackBarService) {

    this.entityName = data.entityName
    this.selectedRecordIds = data.selectedRecordIds
  }

ngOnInit() {
    this.formInitialization()
}

formInitialization() {
    const md = this.entityMetadataService.getEntityMetaData(this.entityName)
    const entityMeta = this.entityMetadataService.getEntityFromMetaData(md)
    this.setupForm(entityMeta)
  }

setupForm(ent: EntityMetadata) {
  const auditableProperties = new EntityField().auditableProperties
  ent.Fields.filter(field => this.canDisplayField(auditableProperties, ent.Key, field))
    .forEach(field => {
       field.Required = false
       const fieldConfig = this.epcs.getEntityConfig(this.entityName, field, null,
                           field.hasNavigationKey() ? ent.getField(field.NavigationKey) : undefined)
       this.entityConfig.push(fieldConfig)
    })
  this.entityConfig = this.arraySortByOrder.transform(this.entityConfig, false)
  }

canDisplayField(auditableProperties: string[], entityKeys: string[], field: EntityField) {
    return auditableProperties.indexOf(field.Name) === -1 &&
           entityKeys.indexOf(field.Name) === -1 &&
           !field.ReadOnly &&
           !field.isNavigationProperty()
}

submit(eventData) {
     this.matDialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: { title: 'Confirm Update',
              message:  `Are you sure you want to update these fields on the selected ${this.pluralizePipe.transform(this.entityName)}?`}
    }).afterClosed().subscribe((response) => {
      if (response === 'confirmed') {
        const patchObjectCollection = this.getPatchEntityCollection(eventData)

        if (patchObjectCollection && patchObjectCollection.ChangedProperties) {
          this.updateRecords(patchObjectCollection)
        }
      }
    })
  }

getPatchEntityCollection(combinedObject) {
    // Remove unchanged values and build patch object
    Object.keys(combinedObject).forEach(property => {
      if (combinedObject[property] === this.entityConfig.find(x => x.name === property).value) {
        delete combinedObject[property]
      }
    })

    const changedProperties = Object.keys(combinedObject)

    const patchObject = { ChangedProperties: changedProperties, Entity: combinedObject }
    if (patchObject.ChangedProperties.length === 0) {
      this.snackBar.open('No changes detected')
      return
    }

    const patchObjects = []
    this.selectedRecordIds.forEach((recordId) => {
      const newPatchObj = JSON.parse(JSON.stringify(patchObject))
      newPatchObj.Entity.Id = recordId
      patchObjects.push(newPatchObj)
    })
    const requestObj: PatchedEntityCollection = { ChangedProperties: changedProperties, PatchedEntities: patchObjects }
    return requestObj
  }


updateRecords(requestObj) {
    this.entityService.patchManyEntities(this.entityName, requestObj).subscribe(
    () => {
      this.dialogRef.close('Update successful')
    },
    error => {
      this.errorReporter.displayMessage(error)
    })
  }

}

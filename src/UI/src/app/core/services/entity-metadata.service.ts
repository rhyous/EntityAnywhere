import { Injectable } from '@angular/core'
import { EntityField } from 'src/app/features/eaf/eaf-common/common-form-module/models/concretes/entity-field'
import { AnyEx } from '../infrastructure/extensions/any-ex'
import { StringEx } from '../infrastructure/extensions/string-ex'

import { EntityMetadata } from '../models/concretes/entity-metadata'
import { ReferentialConstraint } from '../models/concretes/referential-constraint'
import { AppLocalStorageService } from './local-storage.service'

@Injectable({
  providedIn: 'root'
})
export class EntityMetadataService {

  constructor(private localStorage: AppLocalStorageService) { }

  /** Gets the entire set of entity metadata from the local storage.
   * @returns An array of metadata. */
  getAllEntityMetaData(): any[] {
    const meta = JSON.parse(this.localStorage.entityMetaData)
    return meta
  }

  /** Gets a specific entity's meta data from the local storage.
   * @param string The entity name.
   * @returns any An array of metadata. */
  getEntityMetaData(entityName: string): any {
    if (entityName.startsWith('self.')) {
      entityName = entityName.substring(5)
    }
    return this.getAllEntityMetaData().firstOrDefault(x => x.key === entityName)
  }

  /** Stores the meta data within the local storage.
   * @param string A string of json metadata. */
  setMetaData(metaData: string) {
    this.localStorage.entityMetaData = metaData
  }

  /** Gets an EntityMetadata oject from the passed in metadata.
   * @param entity - The entity object.
   * @returns The EntityMetadata*/
  getEntityFromMetaData(entity: any): EntityMetadata {
    const md = new EntityMetadata()

    md.Name = entity.key
    md.Key = entity.value['$Key']
    md.UIDisplayName = this.getDisplayName(entity.value)
    md.EntityType = StringEx.isUndefinedNullOrWhitespace(entity.value['@EAF.Entity.Type'])
      ? 'Entity'
      : entity.value['@EAF.Entity.Type']
    md.FileUpload = AnyEx.getValue<boolean>(entity.value['@EAF.FileUpload'], false)

    Object.keys(entity.value).forEach(key => {
      if (!key.startsWith('@') && !key.startsWith('$')) {
        md.Fields.push(this.getFieldMetaData(entity.value, key))
      }
    })

    return md
  }

  /** Gets EntityMetadata.
   * @param entityName The name of the entity to get.
   * @returns The EntityMetadata*/
  getEntity(entityName: string): EntityMetadata {
    const meta = this.getEntityMetaData(entityName)
    return this.getEntityFromMetaData(meta)
  }

  /** Gets an EntityField from EntityMetadata.
   * @param entity The entity to get the field from.
   * @param key The name of the field to get.
   * @returns The EntityMetadata*/
  getFieldMetaData(entity: any, key: any): EntityField {
    const field = new EntityField()
    field.Name = key
    field.Type = entity[key].$Type
    if (field.Type === 'Edm.String') {
      field.StringType = AnyEx.getValue<string>(entity[key]['@StringType'], 'SingleLine')
    }
    field.MaxLength = AnyEx.getValue<number>(entity[key]['MaxLength'], 0)
    field.ReadOnly = AnyEx.getValue<boolean>(entity[key]['@UI.ReadOnly'], false)
    field.Nullable = AnyEx.getValue<boolean>(entity[key]['$Nullable'], false)
    field.Collection = AnyEx.getValue<boolean>(entity[key]['$Collection'], false)
    field.DisplayOrder = AnyEx.getValue<number>(entity[key]['@UI.DisplayOrder'], 0)
    field.Searchable = AnyEx.getValue<boolean>(entity[key]['@UI.Searchable'], false)
    field.NavigationKey = field.ReadOnly ? undefined : entity[key]['$NavigationKey']
    field.ReferentialConstraint = this.getReferentialConstraint(entity[key])
    field.RelatedEntityType = entity[key]['@EAF.RelatedEntity.Type']
    field.Kind = entity[key]['$Kind']
    field.Filter = entity[key]['@Odata.Filter']
    field.DisplayCondition = entity[key]['@Odata.DisplayCondition']
    field.Default = entity[key]['$Default']
    if (field.Type === 'Collection(Edm.Byte)') {
      field.FileType = AnyEx.getValue<string>(entity[key]['@UI.FileType'], 'file')
      field.AllowedFileExtensions = AnyEx.getValue<string[]>(entity[key]['@UI.AllowedFileExtensions'], ['*'])
    }
    if (entity[key]['@EAF.RelatedEntity.MappingEntityType']) {
      field.MappingEntity = entity[key]['@EAF.RelatedEntity.MappingEntityType'].substring(5) // remove self

      if (field.ReferentialConstraint) { // RelatedEntity
        field.MappedId = field.ReferentialConstraint.ForeignProperty
      }
    }

    if (entity[key]['@EAF.RelatedEntity.ForeignKeyProperty']) {
      field.MappedId = entity[key]['@EAF.RelatedEntity.ForeignKeyProperty']
    }

    if (entity[key]['@EAF.RelatedEntity.MappingEntityAlias']) {
      field.MappingEntityAlias = entity[key]['@EAF.RelatedEntity.MappingEntityAlias']
    }

    if (entity[key]['@EAF.Entity.Alias']) {
      field.EntityAlias = entity[key]['@EAF.Entity.Alias']
    }

    if (entity[key]['$Alias']) {
      field.Alias = entity[key]['$Alias'].substring(5) // remove .self
    }

    if (field.Kind === 'EnumType') {
      field.Options = this.getOptions(entity[key])
    }

    if (entity[key]['@UI.Required'] !== undefined) {
      field.Required = AnyEx.getValue<boolean>(entity[key]['@UI.Required'], false)
    } else {
      field.Required = field.Nullable ? false : true
    }

    if (entity[key]['@UI.VisibleOnRelations'] !== undefined) {
      field.VisibleOnRelations = AnyEx.getValue<boolean>(entity[key]['@UI.VisibleOnRelations'], false)
    }

    // Temp code while testing
    if (key === 'Quantity' || key === 'QuantityType') {
      field.VisibleOnRelations = true
    }

    return field
  }

  /** Gets enum options from an EntityField.
   * @param entityField The entity field to get options from.
   * @returns A map of enum options. */
  getOptions(entityField: any): Map<number, string> {
    const options = new Map<number, string>()
    Object.keys(entityField).forEach((key) => {
      if (!key.startsWith('@') && !key.startsWith('$')) {
        options.set(entityField[key], key)
      }
    })
    return options
  }

  /** Gets a reference constraint from the entity json. This is conditionally used in the creation
   *  of EntityMetada.
   * @param entity The entity. */
  getReferentialConstraint(entity: any): ReferentialConstraint {
    if (entity['$ReferentialConstraint']) {
      return entity['$ReferentialConstraint'] as ReferentialConstraint
    }
    return <any>undefined
  }

  getDisplayName(entity: any) {
    if (entity['@UI.DisplayName'] && entity['@UI.DisplayName']['$PropertyPath']) {
      return entity['@UI.DisplayName']['$PropertyPath']
    }

    if (Object.keys(entity).find(key => key === 'Name')) {
      return 'Name'
    }

    return 'Id'
  }

  convertMetaDataToArray(eafObject: any): any[] {
    const metaDataArray: any = []
    Object.keys(eafObject).forEach((item) => {
      if (item.indexOf('$') === -1) {
        metaDataArray.push(JSON.parse(`{"key": "${item}", "value": ${JSON.stringify(eafObject[item])}}`))
      }
    })
    return metaDataArray
  }

}

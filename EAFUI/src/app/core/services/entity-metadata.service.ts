import { Injectable } from '@angular/core'
import { filter } from 'rxjs/operators'
import { EntityField } from '../models/concretes/entity-field'

import { EntityMetadata } from '../models/concretes/entity-metadata'
import { ReferentialConstraint } from '../models/concretes/referential-constraint'
import { AppLocalStorageService } from './local-storage.service'

@Injectable({
  providedIn: 'root'
})
export class EntityMetadataService {

  constructor(private localStorage: AppLocalStorageService) { }

  /** Gets the entire set of entity metadata from the local storage */
  getAllEntityMetaData(): any[] {
    const meta = JSON.parse(this.localStorage.entityMetaData)
    return meta
  }

  /** Gets a specific entity's meta data from the local storage */
  getEntityMetaData(entityName: string): any {
    return this.getAllEntityMetaData().firstOrDefault(x => x.key === entityName)
  }

  /** Stores the meta data within the local storage */
  setMetaData(metaData: string) {
    this.localStorage.entityMetaData = metaData
  }

  getEntityFromMetaData(entity: any): EntityMetadata {
    const md = new EntityMetadata()

    md.Name = entity.key
    md.Key = entity.value['$Key']
    md.UIDisplayName = this.getDisplayName(entity.value)

    Object.keys(entity.value).forEach(key => {
      if (!key.startsWith('@') && !key.startsWith('$')) {
        md.Fields.push(this.getFieldMetaData(entity.value, key))
      }
    })

    return md
  }

  getFieldMetaData(entity, key): EntityField {
    const field = new EntityField()
    field.Name = key
    field.Type = entity[key].$Type
    field.ReadOnly = this.getBooleanAttribute(entity[key], '@UI.ReadOnly')
    field.Nullable = this.getBooleanAttribute(entity[key], '$Nullable')
    field.Collection = this.getBooleanAttribute(entity[key], '$Collection')
    field.DisplayOrder = this.getNumberAttribute(entity[key], '@UI.DisplayOrder')
    field.Searchable = this.getBooleanAttribute(entity[key], '@UI.Searchable')
    field.NavigationKey = field.ReadOnly ? undefined : entity[key]['$NavigationKey']
    field.ReferentialConstraint = this.getReferentialConstraint(entity[key])
    field.RelatedEntityType = entity[key]['@EAF.RelatedEntity.Type']
    field.Kind = entity[key]['$Kind']
    field.Filter = entity[key]['@Odata.Filter']
    field.DisplayCondition = entity[key]['@Odata.DisplayCondition']

    field.Default = entity[key]['$Default']

    if (entity[key]['@EAF.RelatedEntity.MappingEntityType']) {
      field.MappingEntity = entity[key]['@EAF.RelatedEntity.MappingEntityType'].substring(5) // remove self

      if (field.ReferentialConstraint) { // RelatedEntity and RelatedEntityForeign
        field.MappedId = field.ReferentialConstraint.ForeignProperty
      }
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
      field.Required = this.getBooleanAttribute(entity[key], '@UI.Required')
    } else {
      field.Required = field.Nullable ? false : true
    }

    if (entity[key]['@UI.VisibleOnRelations'] !== undefined) {
      field.VisibleOnRelations = this.getBooleanAttribute(entity[key], '@UI.VisibleOnRelations')
    }

    // Temp code while testing
    if (key === 'Quantity' || key === 'QuantityType') {
      field.VisibleOnRelations = true
    }

    return field
  }
  getOptions(entityField: any): Map<number, string> {
    const options = new Map<number, string>()
    Object.keys(entityField).forEach((key) => {
      if (!key.startsWith('@') && !key.startsWith('$')) {
        options.set(entityField[key], key)
      }
    })
    return options
  }
  getReferentialConstraint(entity: any): ReferentialConstraint {        
    if (entity['$ReferentialConstraint']) {
      return entity['$ReferentialConstraint'] as ReferentialConstraint
    }
    return undefined
  }

  getNumberAttribute(entity: any, key: string): number {
    if (entity[key]) {
      return entity[key]
    }
    return 0
  }

  getBooleanAttribute(entity: any, key: string): boolean {
    if (entity[key]) {
      return entity[key]
    }
    return false
  }

  getDisplayName(entity) {
    if (entity['@UI.DisplayName']) {
      return entity['@UI.DisplayName']
    }

    if (Object.keys(entity).find(key => key === 'Name')) {
      return 'Name'
    }

    return 'Id'
  }

  convertMetaDataToArray(eafObject): any[] {
    const metaDataArray = []
    Object.keys(eafObject).forEach((item) => {
      if (item.indexOf('$') === -1) {
        metaDataArray.push(JSON.parse(`{"key": "${item}", "value": ${JSON.stringify(eafObject[item])}}`))
      }
    })
    return metaDataArray
  }

}

import { Injectable } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'

import { environment } from 'src/environments/environment'
import { EntityField } from 'src/app/core/models/concretes/entity-field'
import { FieldConfig } from '../models/interfaces/field-config.interface'



@Injectable()
export class EntityPropertyControlService {

  constructor() { }

  toFormGroup(entityproperties: FieldConfig[]) {
    const group: any = {}

    entityproperties.forEach(entityproperty => {
      const validators = this.getValidators(entityproperty)

      if (validators.length > 0) {
        group[entityproperty.name] = new FormControl(entityproperty.value !== undefined ? entityproperty.value : '', validators)
      } else {
        group[entityproperty.name] = new FormControl(entityproperty.value !== undefined ? entityproperty.value : '')
      }
    })
    return new FormGroup(group)
  }

  getLabel(field: EntityField): string {
    if (field.hasNavigationKey()) {
      return field.NavigationKey
    }
    return field.Name
  }

  getValidators(entityproperty) {
    const validators = []

    if (entityproperty.required) {
      validators.push(Validators.required)
    }

    if (entityproperty.controlType === 'number' && entityproperty.min !== null) {
      validators.push(Validators.min(entityproperty.min))
    }

    return validators
  }

  getOrder(ent: any, property: string): any {
    if (ent[property]['@UI.DisplayOrder'] !== undefined) {
      return ent[property]['@UI.DisplayOrder']
    }
    return 0
  }

  getEntityConfig(entityName: string, field: EntityField, entitydata: any, navigationField: EntityField): FieldConfig {

    const returnObject = {type: this.getPropertyType(field), label: this.getLabel(field), inputType: this.getInputType(field.Type),
                          name: field.Name, entity: entityName, flex: 50, order: field.DisplayOrder}

    if (field.isEnum()) {
      returnObject['options'] = this.mapOptions(field.Options)
    }

    if (returnObject.type === 'EntitySearcher') {
      returnObject['searchEntity'] = navigationField.Type.substring(5)
      returnObject['searchEntityDefault'] = navigationField.Default
    }

    returnObject['validations'] = this.getValidator(field)
    returnObject['required'] = field.Required

    if (returnObject.type === 'checkbox') {
      // Default it to false
      returnObject['value'] = false
      returnObject['disabled'] = field.ReadOnly
    }

    if (entitydata) {
      returnObject['value'] = entitydata[field.Name]
    }

    if (field.Filter) {
      returnObject['filter'] = field.Filter
    }

    returnObject['order'] = field.DisplayOrder
    returnObject['readOnly'] = field.ReadOnly

    return returnObject
  }


  mapOptions(entityOptions: Map<number, string>): any {
    const options = []
    entityOptions.forEach((k, v) => {
      options.push({Id: v, Value: k})
    })
    return options
  }

  getInputType(fieldType: string): any {
    switch (fieldType) {
      case 'Edm.Int32':
      case 'Edm.Int64':
      case 'Edm.Double':
        return 'number'
      default:
        return 'text'
    }
  }

  getValidator(field: EntityField): any {
    const validations = []

    if (field.Required && field.Type !== 'Edm.Boolean') {
      validations.push({name: 'required', validator: Validators.required, message: `${field.Name} is required`})
    }
    return validations
  }

  getPropertyType(entityproperty: EntityField): any {

    if (entityproperty.hasNavigationKey()) {
      return 'EntitySearcher'
    }

    switch (entityproperty.Type) {
      case 'Edm.Int32':
      case 'Edm.Int64':
      case 'Edm.String':
      case 'Edm.Double':
      case 'Edm.Guid':
        return 'input'
      case 'Edm.Boolean':
        return 'checkbox'
      case 'Edm.Date':
      case 'Edm.DateTimeOffset':
        return 'date'
      default:
        if (entityproperty.isEnum()) {
          return 'select'
        }
    }
  }

  getMappingComponent(baseEntity: string, mapEntity: string, targetEntity: string,
    entityName: string, value: number, alias: string, order: number, entityAlias: string) {
    const returnObject = {type: 'Mapper', label: entityName, baseEntity: baseEntity,
                          targetEntity: targetEntity, mapEntity: mapEntity, flex: 30,
                          value: value, alias: alias, order: order, entityAlias: entityAlias}
    return returnObject
  }

  getMappedReferentialConstraint(mapEntityName, targetEntityName, alias): string {
    const mappedMeta = JSON.parse(localStorage.getItem(environment.metaDataLocalName)).find(md => md.key === mapEntityName)
    let key = alias === undefined ? targetEntityName : alias
    if (mappedMeta && mappedMeta.value) {
      let meta = mappedMeta.value[key]
      if (meta === undefined) {
        meta = mappedMeta.value[`${key}Id`]
        if (meta) {
            key = meta.$NavigationKey
            if (key) {
              meta = mappedMeta.value[key]
            }
        }
      }
      if (meta) {
        return meta.$ReferentialConstraint.LocalProperty
      }
    }
    return 'Id'
  }
}

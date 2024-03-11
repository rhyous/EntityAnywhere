import { Injectable } from '@angular/core'
import { FormControl, FormGroup, Validators } from '@angular/forms'
import { environment } from 'src/environments/environment'
import { FieldConfig } from '../models/interfaces/field-config.interface'

@Injectable()
export class EntityPropertyControlService {

  constructor() { }

  toFormGroup(entityproperties: FieldConfig[]) {

    const group: any = {}

    entityproperties.forEach(entityproperty => {
      if (entityproperty.name) {
        const validators = this.getValidators(entityproperty)

        if (validators.length > 0) {
          group[entityproperty.name] = new FormControl(entityproperty.value !== undefined ? entityproperty.value : '', validators)
        } else {
          group[entityproperty.name] = new FormControl(entityproperty.value !== undefined ? entityproperty.value : '')
        }
      }
    })
    return new FormGroup(group)
  }

  getValidators(entityproperty: FieldConfig) {
    const validators = []
    if (entityproperty.required) {
      validators.push(Validators.required)
    }
    return validators
  }

  getMappingComponent(baseEntity: string, mapEntity: string, targetEntity: string,
    entityName: string, value: number, alias: string, order: number, entityAlias: string) {
    const returnObject = {type: 'Mapper', label: entityName, baseEntity: baseEntity,
                          targetEntity: targetEntity, mapEntity: mapEntity, flex: 30,
                          value: value, alias: alias, order: order, entityAlias: entityAlias}
    return returnObject
  }

  getMappedReferentialConstraint(mapEntityName: any, targetEntityName: any, alias: any): string {
    const allMeta = localStorage.getItem(environment.metaDataLocalName)
    const mappedMeta = allMeta ? JSON.parse(allMeta).find((md: any) => md.key === mapEntityName) : undefined
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

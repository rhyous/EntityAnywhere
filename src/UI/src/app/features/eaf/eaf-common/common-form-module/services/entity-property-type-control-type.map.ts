import { Injectable } from '@angular/core'
import { NullEx } from 'src/app/core/infrastructure/extensions/null-ex'
import { EntityField } from '../models/concretes/entity-field'
import { StringTypeControlTypeMap } from './string-type-control-type.map'

@Injectable()
export class EntityPropertyTypeControlTypeMap extends Map<string, Function> {

  constructor(private stringTypeControlTypeMap: StringTypeControlTypeMap) {
    super()
    this.set('Edm.Binary',         () => 'checkbox')
    this.set('Edm.Boolean',        () => 'checkbox')
    this.set('Edm.Byte',           this.getInput)
    this.set('Edm.Date',           () => 'date')
    this.set('Edm.DateTimeOffset', () => 'date')
    this.set('Edm.Decimal',        this.getInput)
    this.set('Edm.Double',         this.getInput)
    this.set('Edm.Duration',       this.getInput)
    this.set('Edm.Enum',           () => 'select')
    this.set('Edm.Guid',           this.getInput)
    this.set('Edm.Int16',          this.getInput)
    this.set('Edm.Int32',          this.getInput)
    this.set('Edm.Int64',          this.getInput)
    this.set('Edm.SByte',          this.getInput)
    this.set('Edm.Single',         this.getInput)
    this.set('Edm.Stream',         this.getInput)
    this.set('Edm.String',         this.getStringType.bind(this))
  }

  getValueOrDefault(key: string, defaultValue: Function = this.getInput): Function {
    const func = this.get(key)
    return NullEx.isNullOrUndefined(func) ? defaultValue : func
  }

  getInput(entityField: EntityField) {
    return entityField?.hasNavigationKey() ? 'EntitySearcher' : 'input'
  }

  getStringType(entityField: EntityField) {
    const hasNavigationKey = entityField?.hasNavigationKey()
    return hasNavigationKey
         ? 'EntitySearcher'
         : this.stringTypeControlTypeMap.getValueOrDefault(entityField.StringType)(entityField)
  }
}

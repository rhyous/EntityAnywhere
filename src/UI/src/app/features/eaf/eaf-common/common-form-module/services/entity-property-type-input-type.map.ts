import { Injectable } from '@angular/core'
import { StringEx } from 'src/app/core/infrastructure/extensions/string-ex'

@Injectable()
export class EntityPropertyTypeInputTypeMap extends Map<string, string> {

  constructor() {
    super()
    this.set('Edm.Decimal', 'number')
    this.set('Edm.Double',  'number')
    this.set('Edm.Int16',   'number')
    this.set('Edm.Int32',   'number')
    this.set('Edm.Int64',   'number')
  }

  getValueOrDefault(key: string, defaultValue: string = 'text'): string {
    const val = this.get(key)
    return StringEx.isUndefinedNullOrWhitespace(val) ? defaultValue : val
  }
}

import { Injectable } from '@angular/core'
import { NullEx } from 'src/app/core/infrastructure/extensions/null-ex'

@Injectable()
export class StringTypeControlTypeMap extends Map<string, Function> {

  constructor() {
    super()
    this.set('SingleLine', () => 'input')
    this.set('MultiLine',  () => 'textarea')
  }


  getValueOrDefault(key: string, defaultValue: Function = () => 'input'): Function {
    const func = this.get(key)
    return NullEx.isNullOrUndefined(func) ? defaultValue : func
  }
}

import { Pipe, PipeTransform } from '@angular/core'
import { StringEx } from '../infrastructure/extensions/string-ex'
import { CustomPluralizationMap } from '../models/concretes/custom-pluralization-map'
import { SplitPascalCasePipe } from './split-pascal-case.pipe'

@Pipe({
  name: 'singularize'
})

/**
 * Formats a string as a singular rather than pluralised
 */
export class SingularizePipe implements PipeTransform {

  constructor(private customPluralizationMap: CustomPluralizationMap,
    private splitPascalCasePipe: SplitPascalCasePipe) {}

  esChars = ['ch', 'sh', 'ss', 'x', 'z']

  transform(value: string): string {
    if (!StringEx.isUndefinedNullOrWhitespace(value)) {
      let split = value.split(' ')
      if (split.length > 1) {
        split[split.length - 1] = this.transform(split[split.length - 1])
        return split.join(' ')
      }

      split = this.splitPascalCasePipe.transform(value).split(' ')
      if (split.length > 1) {
        split[split.length - 1] = this.transform(split[split.length - 1])
        return split.join('')
      }

      const customPlural = this.customPluralizationMap.Plural.get(value)
      if (!StringEx.isUndefinedNullOrWhitespace(customPlural)) {
        return customPlural
      }

      let returnEs = false
      if (value === 'Addenda') {
        return 'Addendum'
      }
      if (value.endsWith('ies')) {
        return value.substr(0, value.length - 3) + 'y'
      }
      this.esChars.forEach(esChar => {
        if (value.endsWith(esChar + 'es')) {
          returnEs = true
        }
      })
      if (returnEs) {
        return value.substr(0, value.length - 2)
      }
      if (value.endsWith('s')) {
        return value.substr(0, value.length - 1)
      }
      }

    return value
  }
}

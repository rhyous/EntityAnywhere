import { Pipe, PipeTransform } from '@angular/core'
import { StringEx } from '../infrastructure/extensions/string-ex'
import { CustomPluralizationMap } from '../models/concretes/custom-pluralization-map'
import { SplitPascalCasePipe } from './split-pascal-case.pipe'

@Pipe({
  name: 'pluralize'
})
/**
 * Pluralises the supplied parameter
 */
export class PluralizePipe implements PipeTransform {

  constructor(private customPluralizationMap: CustomPluralizationMap,
              private splitPascalCasePipe: SplitPascalCasePipe) {}

  esChars = ['ch', 's', 'sh', 'x', 'z']

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

      const match = value.match(/^[0-9]+/)
      if (match) { // the value is a number.
        console.log('Matching number ' + value)
        return value
      }
      if (value.endsWith('y')) {
        return `${value.substr(0, value.length - 1)}ies`
      }
      let returnEs = false
      this.esChars.forEach(esChar => {
        if (value.endsWith(esChar)) {
          returnEs = true
        }
      })

      if (returnEs) {
        return `${value}es`
      }
      return `${value}s`
    }
    return value
  }
}

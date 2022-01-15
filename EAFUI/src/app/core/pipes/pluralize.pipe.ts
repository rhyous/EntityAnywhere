import { Pipe, PipeTransform } from '@angular/core'

@Pipe({
  name: 'pluralize'
})
/**
 * Pluralises the supplied parameter
 */
export class PluralizePipe implements PipeTransform {
  esChars = ['ch', 's', 'sh', 'x', 'z']

  transform(value: string): string {
    if (value) {
      const match = value.match(/^[0-9]+/)
      if (match) { // the value is a number.
        console.log('Matching number ' + value)
        return value
      }
      let returnEs = false
      // Todo: Get from repository or something
      if (value === 'Addendum') {
        return 'Addenda'
      }
      if (value.endsWith('y')) {
        return `${value.substr(0, value.length - 1)}ies`
      }
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

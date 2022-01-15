import { Pipe, PipeTransform } from '@angular/core'

@Pipe({
  name: 'singularize'
})

/**
 * Formats a string as a singular rather than pluralised
 */
export class SingularizePipe implements PipeTransform {
  esChars = ['ch', 'sh', 'ss', 'x', 'z']

  transform(value: string): string {
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
}

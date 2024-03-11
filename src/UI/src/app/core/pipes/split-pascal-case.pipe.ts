import { Pipe, PipeTransform } from '@angular/core'

@Pipe({
  name: 'splitPascalCase'
})
export class SplitPascalCasePipe implements PipeTransform {

  transform(value: string): any {
    const regex = /($[a-z])|[A-Z][^A-Z]+/g
    const result = value.match(regex)
    if (result) {
      return result.join(' ')
    }
    return value
  }
}

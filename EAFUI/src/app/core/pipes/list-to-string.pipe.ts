import { Pipe, PipeTransform } from '@angular/core'

@Pipe({
  name: 'listToString'
})
/**
 * Turns all the supplied parameters into a comma separated string
 */
export class ListToStringPipe implements PipeTransform {

  transform(value: string[]): string {

    if (!value) {
      return ''
    }

    let returnString = ''
    let comma = ''
    value.forEach((item) => {
      returnString += `${comma}${item}`
      comma = ', '
    })
    return returnString
  }

}

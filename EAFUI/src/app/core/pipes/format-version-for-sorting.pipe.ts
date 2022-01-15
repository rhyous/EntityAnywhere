import { Pipe, PipeTransform } from '@angular/core'

@Pipe({
  name: 'formatVersionForSorting'
})
export class FormatVersionForSortingPipe implements PipeTransform {

  transform(value: string, ...args: any[]): any {
      if (value !== null) {
        return value.replace(/\d+/g, n => (+n + 100000).toString())
      } else {
          return '000000'
      }
  }
}

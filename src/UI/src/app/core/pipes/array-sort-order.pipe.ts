import { Pipe, PipeTransform } from '@angular/core'

@Pipe({
  name: 'arraySortOrder'
})

// Sorts an array of objects out by the value in the Order field

export class ArraySortOrderPipe implements PipeTransform {

  transform(array: Array<any>, decending?: boolean): Array<any> {
    array.sort((f1, f2) => {

      if (f1.order === undefined || f2.order === undefined) {
        throw new Error('Object requires Order field')
      }
      if (decending) {
        if (f1.order > f2.order) {
          return -1
        }

        if (f1.order < f2.order) {
          return 1
        }
        return 0
      } else {
        if (f1.order > f2.order) {
          return 1
        }

        if (f1.order < f2.order) {
          return -1
        }
        return 0
      }
    })

    return array
  }

}

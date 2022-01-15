import { Pipe, PipeTransform } from '@angular/core'

@Pipe({
  name: 'removeHyphens'
})
export class RemoveHyphensPipe implements PipeTransform {

  transform(value: string, ...args: any[]): any {
    return value.replace(/[-]/g, ' ')
  }
}

import { Pipe, PipeTransform } from '@angular/core'

@Pipe({
  name: 'spacetitle'
})

export class SpaceTitlePipe implements PipeTransform {
  transform(value: string): string {
    if (value) {
      value = value.replace(/([A-Z])/g, ' $1').replace(/([^\s][A-Z][a-z])/g, ' $1')
      return value.replace(/^[\s]/, '')
    }
    return value
  }
}

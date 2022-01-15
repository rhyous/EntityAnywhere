import { Pipe, PipeTransform } from '@angular/core'

@Pipe({
  name: 'wcfFormat'
})
/**
 * Formats a date to WCF format
 */
export class WcfFormatPipe implements PipeTransform {

  transform(date: Date): any {
    if (date == null) { return null }
    const d = new Date(date)
    return isNaN(d.getTime()) ?  null : `/Date(${d.getTime()}-00:00)/`
  }

}

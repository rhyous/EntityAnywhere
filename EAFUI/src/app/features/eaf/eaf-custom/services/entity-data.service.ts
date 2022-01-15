import { Injectable } from '@angular/core'
import { DecimalPipe } from '@angular/common'

@Injectable({
  providedIn: 'root'
})
export class EntityDataService {

  constructor(private decimalPipe: DecimalPipe) { }

  removeAuditableProperties(entityData, auditableProperties) {
    const returnData = entityData
    Object.keys(returnData).forEach(property => {
      if (auditableProperties.indexOf(property) >= 0) {
        delete returnData[property]
      }
    })
    return returnData
  }

  convertDatesToWCFFormat(entityObject) {
    Object.keys(entityObject).forEach(property => {
      if (property.indexOf('Date') >= 0 || property.indexOf('Updated') >= 0) {
        entityObject[property] = this.transform(entityObject[property])
        if (entityObject[property] === '' || entityObject[property] === null) {
          delete entityObject[property]
        }
      }
    })
    return entityObject
  }

  convertMetaDataToArray(eafObject): any[] {
    const metaDataArray = []
    Object.keys(eafObject).forEach((item) => {
      if (item.indexOf('$') === -1) {
        metaDataArray.push(JSON.parse(`{"key": "${item}", "value": ${JSON.stringify(eafObject[item])}}`))
      }
    })
    return metaDataArray
  }

  transform(date: Date) {
    if (date == null) { return null }
    const d = new Date(date)
    const offset = (d.getTimezoneOffset() / 60) * -1
    const offsetStr = this.decimalPipe.transform(offset, '2.0')
    return isNaN(d.getTime()) ?  null : `/Date(${d.getTime()}${offset >= 0 ? '+' : ''}${offsetStr}:00)/`
  }

  getRelatedEntityArray(relatedEntity: string, data: any, columns: string[]) {
    const returnArray = []
    if (data.RelatedEntityCollection) {
      data.RelatedEntityCollection.find(x => x.RelatedEntity === relatedEntity)
        .RelatedEntities.forEach(item => {
          const retObject = {}
          columns.forEach(element => {
            retObject[element] = item.Object[element]
          })
          returnArray.push(retObject)
      })
    }
    return returnArray
  }

  createFilterString(filterObject: any): string {
    const filters = []
    Object.keys(filterObject).select(x => {
      const filter = filterObject[x]

      if (filter.filter && filter.filter !== '') {// Ignore filters that have not been set
        if (filter.exactMatch) {
          filters.push(`${x} eq '${filter.filter}'`)
        } else {
          filters.push(`contains(${x},'${filter.filter}')`)
        }
      }
    })

    return filters.join(' and ')
  }
}

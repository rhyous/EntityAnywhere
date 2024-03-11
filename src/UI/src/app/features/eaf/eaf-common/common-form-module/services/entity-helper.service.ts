import { Injectable } from '@angular/core'
import { ApiRequestUrlHelpers } from 'src/app/core/infrastructure/api-request-url-helpers'

/**
 * The data in the expected format for calling
 * @see EntityHelperService.getOrderByString
 **/
export interface OrderByData {
  /** The name of the property to order by */
  propertyName: string
  /** The direction to sort. Must be either 'asc' or 'desc' as expected by the API */
  direction: 'asc' | 'desc'
}

@Injectable()
export class EntityHelperService {
  removeAuditableProperties(entityData: any, auditableProperties: any) {
    const returnData = entityData
    Object.keys(returnData).forEach(property => {
      if (auditableProperties.indexOf(property) >= 0) {
        delete returnData[property]
      }
    })
    return returnData
  }

  createFilterString(filterObject: any): string {
    const filters: any = []
    Object.keys(filterObject).select(x => {
      const filter = filterObject[x]

      if (filter.filter && filter.filter !== '') {// Ignore filters that have not been set
        if (filter.exactMatch) {
          filters.push(`${x} eq ${ApiRequestUrlHelpers.cleanseFilterQueryStringForApiRequest(filter.filter)}`)
        } else {
          filters.push(`contains(${x},${ApiRequestUrlHelpers.cleanseFilterQueryStringForApiRequest(filter.filter)})`)
        }
      }
    })

    return filters.join(' and ')
  }

  getRelatedEntityArray(relatedEntity: string, mappedEntity: string, data: any, columns: string[]) {
    const returnArray: any = []
    if (data.RelatedEntityCollection) {
      data.RelatedEntityCollection.find((x: any) => x.RelatedEntity === relatedEntity)
        .RelatedEntities.forEach((item: any) => {
          const retObject: any = {}
          columns.forEach(element => {
            retObject[element] = item.Object[element]
          })
          item.RelatedEntityCollection.find((x: any) => x.RelatedEntity === mappedEntity)
            .RelatedEntities.forEach((mappedItem: any) => {
              columns.forEach(element => {
                if (retObject[element] === undefined) {
                  retObject[element] = mappedItem.Object[element]
                }
              })
            })
          returnArray.push(retObject)
      })
    }
    return returnArray
  }

  /** Returns a string in the expected format for an OData request to orderBy a property */
  getOrderByString(orderByData: OrderByData) {
    return orderByData.direction === 'asc' || orderByData.direction === 'desc' ?
    `&$orderBy=${orderByData.propertyName} ${orderByData.direction}` : ''
  }
}


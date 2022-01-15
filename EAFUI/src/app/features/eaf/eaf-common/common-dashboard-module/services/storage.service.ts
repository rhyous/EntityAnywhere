import { Injectable } from '@angular/core'

import { PageFilter } from '../../../eaf-common/models/concretes/page-filters'
import { environment } from 'src/environments/environment'
import { FormGroup, FormControl, FormBuilder } from '@angular/forms'
import { ActivatedRoute } from '@angular/router'
@Injectable({
  providedIn: 'root'
})
export class StorageService {

  entityFilters: FormGroup
  private entityPlural = ''

  constructor(private fb: FormBuilder) { }

   initEntityFilters(propertyNames: string[], entityPlural: string) {
     if (this.entityPlural !== entityPlural) {
      this.entityFilters = this.fb.group({})
      propertyNames.forEach(x => {
        this.entityFilters.addControl(x, new FormGroup({
          filter: new FormControl(''),
          exactMatch: new FormControl(false)
        }))
      })
     }
     this.entityPlural = entityPlural
   }

  storeFilterSettings(entityName: string, propertyToFilter: string, filteredText: string, pageSize: number, pageIndex: number) {
    sessionStorage.setItem(`${entityName}Filter`, filteredText)
    sessionStorage.setItem(`${entityName}PageSize`, pageSize.toString())
    sessionStorage.setItem(`${entityName}PageIndex`, pageIndex === undefined ? '0' : pageIndex.toString())
    sessionStorage.setItem(`${entityName}FilteredProp`, propertyToFilter)
  }

  retrieveFilterSettings(entityName: string): PageFilter {
    const pageFilter = new PageFilter()
    // Four potential settings, covering the page size, page within data table, filtered property and filter text
    // When applying this to other pages look to put this in a service.
    let filter = sessionStorage.getItem(`${entityName}Filter`)
    let filterProp = sessionStorage.getItem(`${entityName}FilteredProp`)
    const pageSize = sessionStorage.getItem(`${entityName}PageSize`)
    const pageIndex = sessionStorage.getItem(`${entityName}PageIndex`)

    filter = !filter ? '' : filter
    filterProp = !filterProp ? '' : filterProp

    pageFilter.FilterProperty = filterProp
    pageFilter.FilterText = filter

    if (pageSize) {
      pageFilter.PageSize = +pageSize
    } else {
      pageFilter.PageSize = environment.defaultPageSize
    }

    if (pageIndex) {
      pageFilter.PageIndex = +pageIndex
    } else {
      pageFilter.PageIndex = 0
    }

    if (filter !== '') {
      pageFilter.DisplayFilter = true
    } else {
      pageFilter.DisplayFilter = false
    }
    return pageFilter
  }
}

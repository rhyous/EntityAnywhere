import { TestBed, inject, getTestBed } from '@angular/core/testing'

import { StorageService } from './storage.service'
import { PageFilter } from '../../../eaf-custom/models/concretes/page-filters'
import { environment } from 'src/environments/environment'
import { FormBuilder } from '@angular/forms'
import { HttpClientTestingModule } from '@angular/common/http/testing'


describe('StorageService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ],
      providers: [StorageService, FormBuilder]
    })
  })

  it('should be created', inject([StorageService], (service: StorageService) => {
    expect(service).toBeTruthy()
  }))

  it('should store settings passed', () => {
    const storageService = getTestBed().get(StorageService)
    storageService.storeFilterSettings('Entity1', 'Field1', 'FilterText', 10, 2)
    expect(sessionStorage.getItem('Entity1Filter')).toBe('FilterText')
    expect(sessionStorage.getItem('Entity1FilteredProp')).toBe('Field1')
    expect(sessionStorage.getItem('Entity1PageSize')).toBe('10')
    expect(sessionStorage.getItem('Entity1PageIndex')).toBe('2')
  })

  it('should default page index', () => {
    const storageService = getTestBed().get(StorageService)
    storageService.storeFilterSettings('Entity1', 'Field1', 'FilterText', 10, undefined)
    expect(sessionStorage.getItem('Entity1PageIndex')).toBe('0')
  })

  it('should return a pageFilter object', () => {
    const storageService: StorageService = getTestBed().get(StorageService)
    storageService.storeFilterSettings('Entity1', 'Field1', 'FilterText', 10, undefined)

    const returnValue = storageService.retrieveFilterSettings('Entity1')
    expect(returnValue instanceof PageFilter).toBe(true)
    expect(returnValue.FilterText).toBe('FilterText')
    expect(returnValue.FilterProperty).toBe('Field1')
    expect(returnValue.PageSize).toBe(10)
    expect(returnValue.PageIndex).toBe(0)
    expect(returnValue.DisplayFilter).toBe(true)
  })

  it('should set displayFilters', () => {
    const storageService = getTestBed().get(StorageService)
    storageService.storeFilterSettings('Entity1', 'Field1', '', 10, undefined)

    const returnValue = storageService.retrieveFilterSettings('Entity1')
    expect(returnValue.DisplayFilter).toBe(false)
  })

  it('should default filter and property', () => {
    const storageService = getTestBed().get(StorageService)
    storageService.storeFilterSettings('Entity1', 'Field1', '', 10, undefined)
    sessionStorage.removeItem('Entity1Filter')
    sessionStorage.removeItem('Entity1FilteredProp')

    const returnValue = storageService.retrieveFilterSettings('Entity1')
    expect(returnValue.FilterText).toBe('')
    expect(returnValue.FilterProperty).toBe('')
  })

  it('should default page size and index', () => {
    const storageService = getTestBed().get(StorageService)
    storageService.storeFilterSettings('Entity1', 'Field1', '', 10, undefined)
    sessionStorage.removeItem('Entity1PageSize')
    sessionStorage.removeItem('Entity1PageIndex')

    const returnValue = storageService.retrieveFilterSettings('Entity1')
    expect(returnValue.PageSize).toBe(environment.defaultPageSize)
    expect(returnValue.PageIndex).toBe(0)
  })

})

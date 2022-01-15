import { TestBed, inject, getTestBed } from '@angular/core/testing'
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing'

import { ProductEntityService } from './product-entity.service'
import { environment } from 'src/environments/environment'
import { AppLocalStorageService } from 'src/app/core/services/local-storage.service'
import { FakeAppLocalStorageService } from 'src/app/core/services/mocks/mocks'

describe('ProductEntityService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        ProductEntityService,
        { provide: AppLocalStorageService, useClass: FakeAppLocalStorageService },
      ]
    })
  })

  it('should be created', inject([ProductEntityService], (service: ProductEntityService) => {
    expect(service).toBeTruthy()
  }))

  it('should return a header', () => {
    const entityService = getTestBed().get(ProductEntityService)
    const header = entityService.getHeader()
    expect(header).toEqual({headers: {token: 'THISISMYTOKENTHEREAREMANYTOKENSLIKEITBUTTHISONEISMINE'}})
  })

  it('should call the api service',  inject([ProductEntityService, HttpTestingController],
    (entityService: ProductEntityService, httpMock: HttpTestingController) => {

    spyOn(entityService, 'getHeader').and.returnValue({headers: {token: 'ASDKASDSAKKDASJKDSAJASDKASDASKJ'}})
    entityService.getProductsBySku('GDP', 10, 0)
    .subscribe((data) => {
      expect(data.success).toBe(true)
    })

    const req = httpMock.expectOne({
      url: environment.serviceUrl +
        'ProductService.svc/Products/FilterBySku?$filter=contains(\'Name\',\'GDP\')&$expand=ProductType&$top=10&$skip=0'
    })
    req.flush({success: true})

    expect(req.request.method).toEqual('GET')
    expect(req.request.headers.has('token')).toEqual(true)

  }))

  afterEach(inject([HttpTestingController], (httpMock: HttpTestingController) => {
    httpMock.verify()
  }))
})

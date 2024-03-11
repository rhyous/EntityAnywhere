import { EntityService } from './entity.service'
import { inject, TestBed, getTestBed } from '@angular/core/testing'
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing'
import { RouterTestingModule } from '@angular/router/testing'
import { environment } from 'src/environments/environment'
import { Router } from '@angular/router'
import { AppLocalStorageService } from '../services/local-storage.service'
import { FakeAppLocalStorageService } from './mocks/mocks'
import { PluralizePipe } from '../pipes/pluralize.pipe'
import { CustomPluralizationMap } from '../models/concretes/custom-pluralization-map'
import { SplitPascalCasePipe } from '../pipes/split-pascal-case.pipe'

export class MyMockStorageService {

}

describe('Entity Service', () => {
  let appLocalStorageService: AppLocalStorageService

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        RouterTestingModule
      ],
      providers: [
        { provide: AppLocalStorageService, useClass: FakeAppLocalStorageService },
        PluralizePipe,
        CustomPluralizationMap,
        SplitPascalCasePipe
      ]
    })
  })

  beforeEach(() => {
    appLocalStorageService = TestBed.inject(AppLocalStorageService)
  })

  it('should be initialized', inject([EntityService], (entityService: EntityService) => {
    expect(entityService).toBeTruthy()
  }))

  it('should add an entity', inject([EntityService, HttpTestingController],
    (entityService: EntityService, httpMock: HttpTestingController) => {

      spyOn(entityService, 'getHeader').and.returnValue({headers: {token: 'ASDKASDSAKKDASJKDSAJASDKASDASKJ'}})

      entityService.addEntity('Entity1', [{SuiteId: 1, Entity1Id: 1215, Quantity: 2, QuantityType: 1}])
      .subscribe((data) => {
        expect(data).not.toBeNull()
      })

      const req = httpMock.expectOne({
        url: environment.serviceUrl + 'Entity1Service/Entity1s'
      })
      req.flush({success: true})

      expect(req.request.method).toEqual('POST')
      expect(req.request.headers.has('token')).toEqual(true)
  }))

  it('Should return entities', inject([HttpTestingController], (httpMock: HttpTestingController) => {
    const mockResponse = ['Addendum', 'User', 'UserGroup', 'UserGroupMembership', 'UserRole', 'UserRoleMembership',
                          'UserType', 'UserTypeMap']


    const entityService = getTestBed().get(EntityService)
    entityService.getEntities().subscribe(
      (entities: any) => {
        expect(entities.length).toBe(8)
      }
    )

    const req = httpMock.expectOne(environment.serviceUrl + 'Service/$MetaData')
    expect(req.request.method).toEqual('GET')

    req.flush(mockResponse)
  }))

  it('should return an entities metadata', inject([EntityService, HttpTestingController],
    // Arrange
    (entityService: EntityService, httpMock: HttpTestingController) => {
      const mockResponse = {'Keys': ['Id'],
                            'MediaEntity': false,
                            'Properties': [{'Name': 'Id', 'Format': 'int32', 'Type': ['integer']},
                                           {'Name': 'CreatedBy', 'Format': 'int64', 'Type': ['integer']},
                                           {'Name': 'CreateDate', 'Format': 'date', 'Type': ['string']},
                                           {'Name': 'LastUpdated', 'Format': 'date', 'Type': ['string', 'null']},
                                           {'Name': 'LastUpdatedBy', 'Format': 'int64', 'Type': ['integer', 'null']},
                                           ],
                            'Type': 'object'}

      // Act
      entityService.getEntityMetaData('Entity1').subscribe(
        properties => {
          expect(properties.Properties.length).toEqual(mockResponse.Properties.length)
        }
      )

      // Assert
      const req = httpMock.expectOne({
        url: environment.serviceUrl + 'Entity1Service/$metadata'
      })

      req.flush(mockResponse)
      expect(req.request.method).toEqual('GET')
    })
  )

  it('should return an entity', inject([EntityService, HttpTestingController],
    (entityService: EntityService, httpMock: HttpTestingController) => {
      const mockResponse = {'Id': 5, 'Object':
        {'Id': 5, 'CreateDate': '2018-05-17T18:54:38.17', 'CreatedBy': 1,
         'LastUpdated': null, 'LastUpdatedBy': null},
                            'Uri': 'http://localhost:3896/Entity1Service/Entity1s(5)'}
      spyOn(entityService, 'getHeader').and.returnValue({headers: {token: 'ASDKASDSAKKDASJKDSAJASDKASDASKJ'}})

      entityService.getEntityData('Entity1', 5).subscribe(
        data => {
          expect(data.Object.Id).toEqual(5)
        }
      )

      const req = httpMock.expectOne({
        url: environment.serviceUrl + 'Entity1Service/Entity1s(5)'
      })

      req.flush(mockResponse)
      expect(req.request.method).toEqual('GET')
      expect(req.request.headers.has('token')).toEqual(true)
    })
  )

  it('should return all data for an entity', inject([EntityService, HttpTestingController],
    (entityService: EntityService, httpMock: HttpTestingController) => {
      const mockResponse = {'Id': 5, 'Object':
        {'Id': 5, 'CreateDate': '2018-05-17T18:54:38.17', 'CreatedBy': 1,
         'LastUpdated': null, 'LastUpdatedBy': null},
                            'Uri': 'http://localhost:3896/Entity1Service/Entity1s(5)'}
      spyOn(entityService, 'getHeader').and.returnValue({headers: {token: 'ASDKASDSAKKDASJKDSAJASDKASDASKJ'}})

      entityService.getAllData('Entity1').subscribe(
        data => {
          expect(data.Object.Id).toEqual(5)
        }
      )

      const req = httpMock.expectOne({
        url: environment.serviceUrl + 'Entity1Service/Entity1s'
      })

      req.flush(mockResponse)
      expect(req.request.method).toEqual('GET')
      expect(req.request.headers.has('token')).toEqual(true)
    })
  )

  it('should put an entity', inject([EntityService, HttpTestingController],
    (entityService: EntityService, httpMock: HttpTestingController) => {
      const mockEntity = {'Id': 5, 'Object':
        {'Id': 5, 'CreateDate': '2018-05-17T18:54:38.17', 'CreatedBy': 1,
         'LastUpdated': null, 'LastUpdatedBy': null},
                          'Uri': 'http://localhost:3896/Entity1Service/Entity1s(5)'}
      spyOn(entityService, 'getHeader').and.returnValue({headers: {token: 'ASDKASDSAKKDASJKDSAJASDKASDASKJ'}})

      entityService.putEntity('Entity1', '5', mockEntity).subscribe(
        data => {
          expect(data.status).toEqual(200)
        }
      )

      const req = httpMock.expectOne({
        url: environment.serviceUrl + 'Entity1Service/Entity1s(5)'
      })

      req.flush({status: 200})
      expect(req.request.method).toEqual('PUT')
      expect(req.request.headers.has('token')).toEqual(true)
    })
  )

  it('should patch an entity', inject([EntityService, HttpTestingController],
    (entityService: EntityService, httpMock: HttpTestingController) => {
      const mockEntity = {'Id': 5, 'Object':
        {'Id': 5, 'CreateDate': '2018-05-17T18:54:38.17', 'CreatedBy': 1,
         'LastUpdated': null, 'LastUpdatedBy': null},
                          'Uri': 'http://localhost:3896/Entity1Service/Entity1s(5)'}
      spyOn(entityService, 'getHeader').and.returnValue({headers: {token: 'ASDKASDSAKKDASJKDSAJASDKASDASKJ'}})

      entityService.patchEntity('Entity1', '5', {ChangedProperties: [], Entity: mockEntity}).subscribe(
        data => {
          expect(data).not.toBeNull()
        }
      )

      const req = httpMock.expectOne({
        url: environment.serviceUrl + 'Entity1Service/Entity1s(5)'
      })

      req.flush({status: 200})
      expect(req.request.method).toEqual('PATCH')
      expect(req.request.headers.has('token')).toEqual(true)
    })
  )

  it('should patch multiple entities', inject([EntityService, HttpTestingController],
    (entityService: EntityService, httpMock: HttpTestingController) => {
      const mockEntity = {'Id': 5, 'Object':
        {'Id': 5, 'CreateDate': '2018-05-17T18:54:38.17', 'CreatedBy': 1,
         'LastUpdated': null, 'LastUpdatedBy': null},
                          'Uri': 'http://localhost:3896/Entity1Service/Entity1s(5)'}
      spyOn(entityService, 'getHeader').and.returnValue({headers: {token: 'ASDKASDSAKKDASJKDSAJASDKASDASKJ'}})

      entityService.patchManyEntities('Entity1',
        {ChangedProperties: [], PatchedEntities: [{ChangedProperties: [], Entity: mockEntity}]}
      ).subscribe(
        data => {
          expect(data).not.toBeNull()
        }
      )

      const req = httpMock.expectOne({
        url: environment.serviceUrl + 'Entity1Service/Entity1s'
      })

      req.flush({status: 200})
      expect(req.request.method).toEqual('PATCH')
      expect(req.request.headers.has('token')).toEqual(true)
    })
  )

  it('should delete an entity', inject([EntityService, HttpTestingController],
    (entityService: EntityService, httpMock: HttpTestingController) => {
      spyOn(entityService, 'getHeader').and.returnValue({headers: {token: 'ASDKASDSAKKDASJKDSAJASDKASDASKJ'}})

      entityService.deleteEntity('Entity1', 5).subscribe(
        data => {
          expect((<any>data).status).toEqual(200)
        }
      )

      const req = httpMock.expectOne({
        url: environment.serviceUrl + 'Entity1Service/Entity1s(5)'
      })

      req.flush({status: 200})
      expect(req.request.method).toEqual('DELETE')
      expect(req.request.headers.has('token')).toEqual(true)
    })
  )

  it('should put an entity list', inject([EntityService, HttpTestingController],
    (entityService: EntityService, httpMock: HttpTestingController) => {
      spyOn(entityService, 'getHeader').and.returnValue({headers: {token: 'ASDKASDSAKKDASJKDSAJASDKASDASKJ'}})

      entityService.getEntityList('Entity1', 10, 0).subscribe(
        data => {
          expect(data.status).toEqual(200)
        }
      )

      const req = httpMock.expectOne({
        url: environment.serviceUrl + 'Entity1Service/Entity1s?$top=10&$skip=0'
      })

      req.flush({status: 200})
      expect(req.request.method).toEqual('GET')
      expect(req.request.headers.has('token')).toEqual(true)
    })
  )

  it('should put an entity list with defaults', inject([EntityService, HttpTestingController],
    (entityService: EntityService, httpMock: HttpTestingController) => {
      spyOn(entityService, 'getHeader').and.returnValue({headers: {token: 'ASDKASDSAKKDASJKDSAJASDKASDASKJ'}})

      entityService.getEntityList('Entity1').subscribe(
        data => {
          expect(data.status).toEqual(200)
        }
      )

      const req = httpMock.expectOne({
        url: environment.serviceUrl + 'Entity1Service/Entity1s'
      })

      req.flush({status: 200})
      expect(req.request.method).toEqual('GET')
      expect(req.request.headers.has('token')).toEqual(true)
    })
  )

  it('should get a filtered entity list with defaults', inject([EntityService, HttpTestingController],
    (entityService: EntityService, httpMock: HttpTestingController) => {
      spyOn(entityService, 'getHeader').and.returnValue({headers: {token: 'ASDKASDSAKKDASJKDSAJASDKASDASKJ'}})

      entityService.getEntityFilteredList('User', 'user1' , ['Username']).subscribe(
        data => {
          expect(data.status).toEqual(200)
        }
      )

      const req = httpMock.expectOne({
        url: environment.serviceUrl + 'UserService/Users?$filter=contains(Username, \'user1\')'
      })

      req.flush({status: 200})
      expect(req.request.method).toEqual('GET')
      expect(req.request.headers.has('token')).toEqual(true)
    })
  )

  it('should get a filtered map list with defaults', inject([EntityService, HttpTestingController],
    (entityService: EntityService, httpMock: HttpTestingController) => {
      spyOn(entityService, 'getHeader').and.returnValue({headers: {token: 'ASDKASDSAKKDASJKDSAJASDKASDASKJ'}})

      entityService.getFilteredMapList('Entity1', 'Entity2', 'Entity3', '', 5, 10, 20).subscribe(
        data => {
          expect(data.status).toEqual(200)
        }
      )

      const req = httpMock.expectOne({
        url: environment.serviceUrl + `Entity1Service/Entity1s(5)?$expand=Entity2($Expand=Entity3)&$top=10&$skip=20`
      })

      req.flush({status: 200})
      expect(req.request.method).toEqual('GET')
      expect(req.request.headers.has('token')).toEqual(true)
    })
  )

  it('should get a filtered entity list', inject([EntityService, HttpTestingController],
    (entityService: EntityService, httpMock: HttpTestingController) => {
      spyOn(entityService, 'getHeader').and.returnValue({headers: {token: 'ASDKASDSAKKDASJKDSAJASDKASDASKJ'}})

      entityService.getEntityFilteredList('User', 'user1' , ['Username'], undefined, 10, 0).subscribe(
        data => {
          expect(data.status).toEqual(200)
        }
      )

      const req = httpMock.expectOne({
        url: environment.serviceUrl + 'UserService/Users?$filter=contains(Username, \'user1\')&$top=10&$skip=0'
      })

      req.flush({status: 200})
      expect(req.request.method).toEqual('GET')
      expect(req.request.headers.has('token')).toEqual(true)
    })
  )

  it('should get a filtered entity list by equals', inject([EntityService, HttpTestingController],
    (entityService: EntityService, httpMock: HttpTestingController) => {
      spyOn(entityService, 'getHeader').and.returnValue({headers: {token: 'ASDKASDSAKKDASJKDSAJASDKASDASKJ'}})

      entityService.getEntityFilteredList('User', 'user1' , ['Username'], undefined, 10, 0, ['Username']).subscribe(
        data => {
          expect(data.status).toEqual(200)
        }
      )

      const req = httpMock.expectOne({
        url: environment.serviceUrl + 'UserService/Users?$filter=Username eq \'user1\'&$top=10&$skip=0'
      })

      req.flush({status: 200})
      expect(req.request.method).toEqual('GET')
      expect(req.request.headers.has('token')).toEqual(true)
    })
  )

  it('should filter entities based on filter', inject([EntityService, HttpTestingController],
    (entityService: EntityService, httpMock: HttpTestingController) => {
      spyOn(entityService, 'getHeader').and.returnValue({headers: {token: 'ASDKASDSAKKDASJKDSAJASDKASDASKJ'}})

      entityService.getFilteredEntityList('User', `contains('Name', 'Test')`, 10, 0).subscribe(
        data => {
          expect((<any>data).status).toEqual(200)
        }
      )

      const req = httpMock.expectOne({
        url: environment.serviceUrl + `UserService/Users?$filter=contains('Name', 'Test')&$top=10&$skip=0`
      })

      req.flush({status: 200})
      expect(req.request.method).toEqual('GET')
      expect(req.request.headers.has('token')).toEqual(true)
    })
  )

  it('should get expanded entity list', inject([EntityService, HttpTestingController],
    (entityService: EntityService, httpMock: HttpTestingController) => {
      spyOn(entityService, 'getHeader').and.returnValue({headers: {token: 'ASDKASDSAKKDASJKDSAJASDKASDASKJ'}})

      entityService.getExpandedEntityList('Entity1', ['Entity2'], 10, 0).subscribe(
        data => {
          expect((<any>data).status).toEqual(200)
        }
      )

      const req = httpMock.expectOne({
        url: environment.serviceUrl + `Entity1Service/Entity1s?$expand=Entity2&$top=10&$skip=0`
      })

      req.flush({status: 200})
      expect(req.request.method).toEqual('GET')
      expect(req.request.headers.has('token')).toEqual(true)
    })
  )

  it('should get expanded filterd entity list', inject([EntityService, HttpTestingController],
    (entityService: EntityService, httpMock: HttpTestingController) => {
      spyOn(entityService, 'getHeader').and.returnValue({headers: {token: 'ASDKASDSAKKDASJKDSAJASDKASDASKJ'}})

      entityService.getExpandedFilteredEntityList('Entity1',  `contains('Name', 'Test')`, ['Entity2'], 10, 0).subscribe(
        data => {
          expect((<any>data).status).toEqual(200)
        }
      )

      const req = httpMock.expectOne({
        url: environment.serviceUrl + `Entity1Service/Entity1s?$filter=contains('Name', 'Test')&$expand=Entity2&$top=10&$skip=0`
      })

      req.flush({status: 200})
      expect(req.request.method).toEqual('GET')
      expect(req.request.headers.has('token')).toEqual(true)
    })
  )

  it('should get entity list by custom url', inject([EntityService, HttpTestingController],
    (entityService: EntityService, httpMock: HttpTestingController) => {
      // Arrange
      spyOn(entityService, 'getHeader').and.returnValue({headers: {token: 'ASDKASDSAKKDASJKDSAJASDKASDASKJ'}})

      entityService.getEntityListByCustomUrl('Entity1', `Some/Custom/Url`).subscribe(
        data => {
          expect((<any>data).status).toEqual(200)
        }
      )

      // Act
      const req = httpMock.expectOne({
        url: environment.serviceUrl + `Entity1Service/Entity1s/Some/Custom/Url`
      })

      // Assert
      req.flush({status: 200})
      expect(req.request.method).toEqual('GET')
      expect(req.request.headers.has('token')).toEqual(true)
    })
  )

  it('should get list of distinct extension entity properties by entity', inject([EntityService, HttpTestingController],
    (entityService: EntityService, httpMock: HttpTestingController) => {
      spyOn(entityService, 'getHeader').and.returnValue({headers: {token: 'ASDKASDSAKKDASJKDSAJASDKASDASKJ'}})

      entityService.getDistinctExtensionPropertList('Addendum', 'Entity1').subscribe(
        data => {
          expect((<any>data).status).toEqual(200)
        }
      )

      const req = httpMock.expectOne({
        url: environment.serviceUrl + `AddendumService/Addenda/Entity1/Property/Distinct`
      })

      req.flush({status: 200})
      expect(req.request.method).toEqual('GET')
      expect(req.request.headers.has('token')).toEqual(true)
    })
  )

  it('should get list of distinct values by property name', inject([EntityService, HttpTestingController],
    (entityService: EntityService, httpMock: HttpTestingController) => {
      spyOn(entityService, 'getHeader').and.returnValue({headers: {token: 'ASDKASDSAKKDASJKDSAJASDKASDASKJ'}})

      entityService.getDistinctPropertList('Entity1', 'Name').subscribe(
        data => {
          expect((<any>data).status).toEqual(200)
        }
      )

      const req = httpMock.expectOne({
        url: environment.serviceUrl + `Entity1Service/Entity1s/Name/Distinct`
      })

      req.flush({status: 200})
      expect(req.request.method).toEqual('GET')
      expect(req.request.headers.has('token')).toEqual(true)
    })
  )
  afterEach(inject([HttpTestingController], (httpMock: HttpTestingController) => {
    httpMock.verify()
  }))
})

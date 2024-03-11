import { TestBed, inject } from '@angular/core/testing'
import { HttpClient } from '@angular/common/http'
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing'

import { environment } from 'src/environments/environment'
import { ImpersonationService } from './impersonation.service'
import { AppLocalStorageService } from '../services/local-storage.service'

class FakeAppLocalStorageService {
  User = {
    OrganizationId: {
      Value: '123'
    }
  }
  activeToken!: 'THISISMYTOKENTHEREAREMANYTOKENSLIKEITBUTTHISONEISMINE'
}

describe('ImpersonationService', () => {
  let service: ImpersonationService
  let http: HttpClient

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ],
      providers: [
        { provide: AppLocalStorageService, useClass: FakeAppLocalStorageService}
      ]
    })

    service = TestBed.inject(ImpersonationService)
    http = TestBed.inject(HttpClient)
  })

  it('should be created', () => {
    expect(service).toBeTruthy()
  })

  it('should call http get on getImpersonationToken', () => {
    // Arrange
    const userRoleId = '2'
    spyOn(http, 'get').and.callThrough()

    // Act
    service.getImpersonationToken(userRoleId)

    // Assert
    expect(http.get).toHaveBeenCalled()
  })

  it('should pass the correct url when calling getImpersonationToken',
     inject([HttpTestingController], (httpMock: HttpTestingController)  => {
        // Arrange
        const userRoleId = '2'
        const url = `Service/$Impersonate/${userRoleId}`

        // Act
        service.getImpersonationToken(userRoleId).subscribe(next => expect(next).toEqual(<any>{}))

        // Assert
        const req = httpMock.expectOne(`${environment.serviceUrl}${url}`)
        expect(req.request.method).toEqual('GET')
        req.flush({})
  }))
})

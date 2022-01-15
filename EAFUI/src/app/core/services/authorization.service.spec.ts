import { HttpClient } from '@angular/common/http'
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing'
import { TestBed } from '@angular/core/testing'
import { environment } from 'src/environments/environment'
import { AuthorizationService } from './authorization.service'
import { AppLocalStorageService } from './local-storage.service'
import { FakeAppLocalStorageService } from './mocks/mocks'

describe('AuthorizationService', () => {
    let service: AuthorizationService
    let http: HttpClient
    let httpMock: HttpTestingController

    beforeEach(() => TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ],
      providers: [
        { provide: AppLocalStorageService, useClass: FakeAppLocalStorageService }
      ]
    }))

    beforeEach(() => {
      service = TestBed.get(AuthorizationService)
      http = TestBed.get(HttpClient)
      httpMock = TestBed.get(HttpTestingController)
    })

    it('should be created', () => {
        expect(service).toBeTruthy()
    })

    describe('getLoggedInUserRoleData()', () => {
        const roleData = {
            'ProductGroup': {'Entity': 'ProductGroup', 'Permissions': ['Admin'] },
            'Product': { 'Entity': 'Product', 'Permissions': ['Admin']},
            'ProductFeatureMap': {'Entity': 'ProductFeatureMap', 'Permissions': ['Admin']}
        }

        it('should call http get method for the given route', () => {

            // Act
            service.getLoggedInUserRoleData().subscribe(() => {})

            // Assert 1: Verify if correct API endpoint is called
            const req = httpMock.expectOne(`${environment.serviceUrl}AuthorizationService.svc/MyRoleData`)

            // Assert 2: Verify if the request method is 'GET'
            expect(req.request.method).toEqual('GET')

            // Assert 3: Ensures that all request are fulfilled and there are no outstanding requests
            httpMock.verify()
        })

        it('should return roledata', () => {

            // Act
            service.getLoggedInUserRoleData().subscribe((result) => {
                // Assert 1: Verify if result matches test data
                expect(result).toEqual(roleData)
            })

            // Assert 2: Verify if correct API endpoint is called
            const req = httpMock.expectOne(`${environment.serviceUrl}AuthorizationService.svc/MyRoleData`)

            // Assert 3: Ensures the correct data was returned using Subscribe callback.
            req.flush(roleData)

            // Assert 4: Ensures that all request are fulfilled and there are no outstanding requests
            httpMock.verify()
        })
    })
})

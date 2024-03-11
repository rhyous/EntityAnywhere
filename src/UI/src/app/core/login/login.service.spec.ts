import { TestBed } from '@angular/core/testing'
import { LoginService } from './login.service'

import { HttpClient } from '@angular/common/http'
import { HttpClientTestingModule } from '@angular/common/http/testing'
import { RouterTestingModule } from '@angular/router/testing'
import { Router } from '@angular/router'
import { MaterialModule } from '../material/material.module'
import { of } from 'rxjs'
import { MatDialog } from '@angular/material/dialog'
import { AppLocalStorageService } from '../services/local-storage.service'
import { EntityService } from '../services/entity.service'
import { DefaultDataService } from '../services/default-data.service'
import { EntityMetadataService } from '../services/entity-metadata.service'
import { UserDataService } from '../services/user-data.service'
import { LandingPageTypes } from '../models/enums/LandingPageTypes'
import { AuthorizationService } from '../services/authorization.service'
import { MockUserData } from './mocks/mock-user'
import { PluralizePipe } from '../pipes/pluralize.pipe'
import { CustomPluralizationMap } from '../models/concretes/custom-pluralization-map'
import { SplitPascalCasePipe } from '../pipes/split-pascal-case.pipe'

const router = {
  navigate: jasmine.createSpy('navigate')
}

export class MatDialogMock {
  open() {
    return {
      afterClosed: () => of('stub')
    }
  }
}

/**
 * Represents a Mocked version of Application Insights
 */
export class AppInsightsServiceMock {
  trackEvent() {

  }

}

describe('Login Service', () => {
  let service: LoginService
  let http: HttpClient
  let localStorageService: AppLocalStorageService
  let entityService: EntityService
  let defaultDataService: DefaultDataService
  let entityMetadataService: EntityMetadataService
  let authorizationService: AuthorizationService
  let userDataService: UserDataService

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        RouterTestingModule,
        MaterialModule
      ],
      providers: [
        LoginService,
        { provide: Router, useValue: router },
        { provide: MatDialog, useValue: MatDialogMock },
        PluralizePipe,
        CustomPluralizationMap,
        SplitPascalCasePipe
      ]
    })

    service = TestBed.inject(LoginService)
    http = TestBed.inject(HttpClient)
    localStorageService = TestBed.inject(AppLocalStorageService)
    entityService = TestBed.inject(EntityService)
    defaultDataService = TestBed.inject(DefaultDataService)
    entityMetadataService = TestBed.inject(EntityMetadataService)
    authorizationService = TestBed.inject(AuthorizationService)
    userDataService = TestBed.inject(UserDataService)
  })

  it('should call post on login', () => {
    // Arrange
    spyOn(http, 'post').and.callThrough()

    // Act
    service.login('someuser', 'somepass')

    // Assert
    expect(http.post).toHaveBeenCalled()
  })

  it('should call post on validateOAuthToken', () => {
    // Arrange
    spyOn(http, 'post').and.callThrough()

    // Act
    service.login('someuser', 'somepass')

    // Assert
    expect(http.post).toHaveBeenCalled()
  })

  it(`should parse token - Admin`, () => {
    // Arrange
    const token = MockUserData.getUserToken('admin', '3', 'Admin', LandingPageTypes.Administration)

    spyOn(service.onLoginSuccess, 'emit')
    spyOn(entityService, 'getEntities').and.returnValue(of(<any>{}))
    spyOn(authorizationService, 'getLoggedInUserRoleData').and.returnValue(of(<any>{}))
    spyOn(entityMetadataService, 'convertMetaDataToArray')

    // Act
    service.parseToken(token)

    // Assert
    expect(service.onLoginSuccess.emit).toHaveBeenCalledTimes(1)
    expect(entityService.getEntities).toHaveBeenCalledTimes(1)
    expect(authorizationService.getLoggedInUserRoleData).toHaveBeenCalledTimes(1)
    expect(entityMetadataService.convertMetaDataToArray).toHaveBeenCalled()
    expect(router.navigate).toHaveBeenCalledWith(['/admin'])
  })

  it(`should parse token - for user role with Admin landing page access`, () => {
    // Arrange
    const token = MockUserData.getUserToken('adminLandingPageAccess', '30', 'AdminLandingAccess', LandingPageTypes.Administration)

    spyOn(service.onLoginSuccess, 'emit')
    spyOn(entityService, 'getEntities').and.returnValue(of(<any>{}))
    spyOn(authorizationService, 'getLoggedInUserRoleData').and.returnValue(of(<any>{}))
    spyOn(entityMetadataService, 'convertMetaDataToArray')

    // Act
    service.parseToken(token)

    // Assert
    expect(service.onLoginSuccess.emit).toHaveBeenCalledTimes(1)
    expect(entityService.getEntities).toHaveBeenCalledTimes(1)
    expect(authorizationService.getLoggedInUserRoleData).toHaveBeenCalledTimes(1)
    expect(entityMetadataService.convertMetaDataToArray).toHaveBeenCalled()
    expect(router.navigate).toHaveBeenCalledWith(['/default'])
  })

  it(`should parse token - Default`, () => {
    // Arrange
    const token = MockUserData.getUserToken('default1', '4', 'Default', LandingPageTypes.Default)

    spyOn(service.onLoginSuccess, 'emit')
    spyOn(entityService, 'getEntities').and.returnValue(of(<any>{}))
    spyOn(authorizationService, 'getLoggedInUserRoleData').and.returnValue(of(<any>{}))
    spyOn(entityMetadataService, 'convertMetaDataToArray')

    // Act
    service.parseToken(token)

    // Assert
    expect(service.onLoginSuccess.emit).toHaveBeenCalledTimes(1)
    expect(router.navigate).toHaveBeenCalledWith(['/default'])
    expect(entityService.getEntities).toHaveBeenCalledTimes(1)
    expect(authorizationService.getLoggedInUserRoleData).toHaveBeenCalledTimes(1)
    expect(entityMetadataService.convertMetaDataToArray).toHaveBeenCalled()
  })

  it(`should parse token - - for user role with Default landing page access`, () => {
    // Arrange
    const token = MockUserData.getUserToken('defaultLandingPageAccess', '31', 'DefaultLandingAccess', LandingPageTypes.Customer)

    spyOn(userDataService, 'getLandingPageType').and.returnValue(4)
    spyOn(service.onLoginSuccess, 'emit')
    spyOn(entityService, 'getEntities').and.returnValue(of(<any>{}))
    spyOn(authorizationService, 'getLoggedInUserRoleData').and.returnValue(of(<any>{}))
    spyOn(entityMetadataService, 'convertMetaDataToArray')

    // Act
    service.parseToken(token)

    // Assert
    expect(service.onLoginSuccess.emit).toHaveBeenCalledTimes(1)
    expect(router.navigate).toHaveBeenCalledWith(['/customer'])
    expect(entityService.getEntities).toHaveBeenCalledTimes(1)
    expect(authorizationService.getLoggedInUserRoleData).toHaveBeenCalledTimes(1)
    expect(entityMetadataService.convertMetaDataToArray).toHaveBeenCalled()
  })

})

describe('Login Service Logout Impersonation Mode', () => {
  let service: LoginService
  let localStorageService: AppLocalStorageService
  let defaultDataService: DefaultDataService
  let userDataService: UserDataService

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        RouterTestingModule,
        MaterialModule
      ],
      providers: [
        LoginService,
        UserDataService,
        PluralizePipe,
        CustomPluralizationMap,
        SplitPascalCasePipe,
        { provide: Router, useValue: router }
      ]
    })

    service = TestBed.inject(LoginService)
    localStorageService = TestBed.inject(AppLocalStorageService)
    defaultDataService = TestBed.inject(DefaultDataService)
    userDataService = TestBed.inject(UserDataService)
  })

  it('should navigate to Admin page on logout', () => {
    // Arrange
    spyOn(userDataService, 'logout').and.returnValue()
    spyOn(defaultDataService, 'logout').and.returnValue()

    // Act
    service.switchUser()

    // Assert
    expect(router.navigate).toHaveBeenCalledWith(['admin/'])
    expect(userDataService.logout).toHaveBeenCalled()
    expect(defaultDataService.logout).toHaveBeenCalled()
  })
})

import { TestBed } from '@angular/core/testing'
import { LoginService } from './login.service'

import { HttpClient } from '@angular/common/http'
import { HttpClientTestingModule } from '@angular/common/http/testing'
import { RouterTestingModule } from '@angular/router/testing'
import { Router } from '@angular/router'
import { MaterialModule } from '../material/material.module'
import { of } from 'rxjs'
import { MatDialog } from '@angular/material'
import { AppLocalStorageService } from '../services/local-storage.service'
import { EntityService } from '../services/entity.service'
import { DefaultDataService } from '../services/default-data.service'
import { EntityMetadataService } from '../services/entity-metadata.service'
import { UserDataService } from '../services/user-data.service'
import { LandingPageTypes } from '../models/enums/LandingPageTypes'
import { AuthorizationService } from '../services/authorization.service'
import { UserLandingPages } from '../models/concretes/user-landing-page'
import { MockUserData } from './mocks/mock-user'

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
      ]
    })

    service = TestBed.get(LoginService)
    http = TestBed.get(HttpClient)
    localStorageService = TestBed.get(AppLocalStorageService)
    entityService = TestBed.get(EntityService)
    defaultDataService = TestBed.get(DefaultDataService)
    entityMetadataService = TestBed.get(EntityMetadataService)
    authorizationService = TestBed.get(AuthorizationService)
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
    const token = MockUserData.getUserToken('admin', '3', 'Admin', LandingPageTypes.Unknown)

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
    expect(router.navigate).toHaveBeenCalledWith([UserLandingPages.landingPages.get(LandingPageTypes.Administration).url])
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
    expect(router.navigate).toHaveBeenCalledWith([UserLandingPages.landingPages.get(LandingPageTypes.Administration).url])
  })

  it(`should parse token - Default`, () => {
    // Arrange
    const token = MockUserData.getUserToken('default1', '4', 'Default', LandingPageTypes.Unknown)

    spyOn(service.onLoginSuccess, 'emit')
    spyOn(entityService, 'getEntities').and.returnValue(of(<any>{}))
    spyOn(authorizationService, 'getLoggedInUserRoleData').and.returnValue(of(<any>{}))
    spyOn(entityMetadataService, 'convertMetaDataToArray')

    // Act
    service.parseToken(token)

    // Assert
    expect(service.onLoginSuccess.emit).toHaveBeenCalledTimes(1)
    expect(router.navigate).toHaveBeenCalledWith([UserLandingPages.landingPages.get(LandingPageTypes.Student).url])
    expect(entityService.getEntities).toHaveBeenCalledTimes(1)
    expect(authorizationService.getLoggedInUserRoleData).toHaveBeenCalledTimes(1)
    expect(entityMetadataService.convertMetaDataToArray).toHaveBeenCalled()
  })

  it(`should parse token - - for user role with Default landing page access`, () => {
    // Arrange
    const token = MockUserData.getUserToken('defaultLandingPageAccess', '31', 'DefaultLandingAccess', LandingPageTypes.Student)

    spyOn(service.onLoginSuccess, 'emit')
    spyOn(entityService, 'getEntities').and.returnValue(of(<any>{}))
    spyOn(authorizationService, 'getLoggedInUserRoleData').and.returnValue(of(<any>{}))
    spyOn(entityMetadataService, 'convertMetaDataToArray')

    // Act
    service.parseToken(token)

    // Assert
    expect(service.onLoginSuccess.emit).toHaveBeenCalledTimes(1)
    expect(router.navigate).toHaveBeenCalledWith([UserLandingPages.landingPages.get(LandingPageTypes.Student).url])
    expect(entityService.getEntities).toHaveBeenCalledTimes(1)
    expect(authorizationService.getLoggedInUserRoleData).toHaveBeenCalledTimes(1)
    expect(entityMetadataService.convertMetaDataToArray).toHaveBeenCalled()
  })

})


describe('Login Service Logout localcredentials true', () => {
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
        { provide: Router, useValue: router }
      ]
    })

    service = TestBed.get(LoginService)
    localStorageService = TestBed.get(AppLocalStorageService)
    defaultDataService = TestBed.get(DefaultDataService)
    userDataService = TestBed.get(UserDataService)
  })

  it('should navigate to login page on logout', () => {
    // Arrange
    spyOn(userDataService, 'logout').and.returnValue()
    spyOn(defaultDataService, 'logout').and.returnValue()
    localStorageService.loginMethodCookie = 'Local'

    // Act
    service.logout()

    // Assert
    expect(router.navigate).toHaveBeenCalledWith(['/'])
    expect(userDataService.logout).toHaveBeenCalled()
    expect(defaultDataService.logout).toHaveBeenCalled()
  })
})

describe('Login Service Logout localcredentials false', () => {
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
        DefaultDataService,
        { provide: Router, useValue: router }
      ]
    })

    service = TestBed.get(LoginService)
    localStorageService = TestBed.get(AppLocalStorageService)
    defaultDataService = TestBed.get(DefaultDataService)
    userDataService = TestBed.get(UserDataService)
  })

  it('should navigate to localcredentials on logout', () => {
    // Arrange
    spyOn(userDataService, 'logout').and.returnValue()
    spyOn(defaultDataService, 'logout').and.returnValue()
    localStorageService.loginMethodCookie = 'Local'

    // Act
    service.logout()

    // Assert
    expect(router.navigate).toHaveBeenCalledWith(['/localcredentials'])
    expect(userDataService.logout).toHaveBeenCalled()
    expect(defaultDataService.logout).toHaveBeenCalled()
  })

  it('should navigate to login page on logout', () => {
    // Arrange
    spyOn(userDataService, 'logout').and.returnValue()
    spyOn(defaultDataService, 'logout').and.returnValue()
    localStorageService.loginMethodCookie = 'SSO'

    // Act
    service.logout()

    // Assert
    expect(router.navigate).toHaveBeenCalledWith(['/'])
    expect(userDataService.logout).toHaveBeenCalled()
    expect(defaultDataService.logout).toHaveBeenCalled()
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
        { provide: Router, useValue: router }
      ]
    })

    service = TestBed.get(LoginService)
    localStorageService = TestBed.get(AppLocalStorageService)
    defaultDataService = TestBed.get(DefaultDataService)
    userDataService = TestBed.get(UserDataService)
  })

  it('should call swithUser on logout', () => {
    // Arrange
    spyOn(service, 'switchUser').and.returnValue()

    // Act
    service.logout()

    // Assert
    expect(service.switchUser).toHaveBeenCalled()
  })

  it('should navigate to Admin page on switchUser', () => {
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

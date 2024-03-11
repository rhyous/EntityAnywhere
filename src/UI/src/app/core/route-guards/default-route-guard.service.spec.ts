import { TestBed } from '@angular/core/testing'

import { DefaultRouteGuardService } from './default-route-guard.service'
import { RouterTestingModule } from '@angular/router/testing'
import { UserDataService } from '../services/user-data.service'
import { HttpClientTestingModule } from '@angular/common/http/testing'

describe('DefaultRouteGuardService', () => {
  let service: DefaultRouteGuardService
  let userService: UserDataService

  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      RouterTestingModule,
      HttpClientTestingModule
    ],
    providers: [
      UserDataService
    ]
  }))

  beforeEach(() => {
    service = TestBed.inject(DefaultRouteGuardService)
    userService = TestBed.inject(UserDataService)
  })

  it('should be created', () => {
    expect(service).toBeTruthy()
  })

  it('should check the token and return true if logged in user is default', () => {
    // Arrange
    spyOn(userService, 'userLandingPageIs').and.returnValue(true)

    // Act
    const canLoad = service.canLoad(<any>null, <any>null)

    // Assert
    expect(canLoad).toBeTruthy()
  })

  it('should check the token and return true if the landing page of logged in user is default', () => {
    // Arrange
    spyOn(userService, 'userLandingPageIs').and.returnValue(true)

    // Act
    const canLoad = service.canLoad(<any>null, <any>null)

    // Assert
    expect(canLoad).toBeTruthy()
  })

  it('should check the token and return false if both user role and their landing page is not default', () => {
    // Arrange
    spyOn(userService, 'userLandingPageIs').and.returnValue(false)

    // Act
    const canLoad = service.canLoad(<any>null, <any>null)

    // Assert
    expect(canLoad).toBeFalsy()
  })
})

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
    service = TestBed.get(DefaultRouteGuardService)
    userService = TestBed.get(UserDataService)
  })

  it('should be created', () => {
    expect(service).toBeTruthy()
  })

  it('should check the token and return true if logged in user is default', () => {
    // Arrange
    spyOn(userService, 'userIsDefault').and.returnValue(true)
    spyOn(userService, 'userLandingPageIsDefault').and.returnValue(false)

    // Act
    const canLoad = service.canLoad(null, null)

    // Assert
    expect(canLoad).toBeTruthy()
  })

  it('should check the token and return true if the landing page of logged in user is Default', () => {
    // Arrange
    spyOn(userService, 'userIsDefault').and.returnValue(false)
    spyOn(userService, 'userLandingPageIsDefault').and.returnValue(true)

    // Act
    const canLoad = service.canLoad(null, null)

    // Assert
    expect(canLoad).toBeTruthy()
  })

  it('should check the token and return false if both user role and their landing page is not Default', () => {
    // Arrange
    spyOn(userService, 'userIsDefault').and.returnValue(false)
    spyOn(userService, 'userLandingPageIsDefault').and.returnValue(false)

    // Act
    const canLoad = service.canLoad(null, null)

    // Assert
    expect(canLoad).toBeFalsy()
  })
})

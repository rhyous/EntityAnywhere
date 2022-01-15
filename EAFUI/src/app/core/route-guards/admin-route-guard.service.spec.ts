import { TestBed } from '@angular/core/testing'

import { AdminRouteGuardService } from './admin-route-guard.service'
import { UserDataService } from '../services/user-data.service'
import { RouterTestingModule } from '@angular/router/testing'
import { ActivatedRoute, ActivatedRouteSnapshot, convertToParamMap, Router } from '@angular/router'
import { HttpClientTestingModule } from '@angular/common/http/testing'
import { AppLocalStorageService } from '../services/local-storage.service'
import { EntityService } from '../services/entity.service'

const fakeRouter = {
  navigate: (routes: string[]) => {
  }
}

describe('AdminRouteGuardService', () => {
  let service: AdminRouteGuardService
  let userService: UserDataService
  let route: ActivatedRoute
  let localStorageService: AppLocalStorageService
  let entityService: EntityService

  beforeEach(() => TestBed.configureTestingModule({
    imports: [ RouterTestingModule, HttpClientTestingModule ],
    providers: [
      UserDataService,
      // { provide: Router, useValue: fakeRouter },
      { provide: ActivatedRouteSnapshot, useValue: {snapshot: {paramMap: convertToParamMap({id: 'one-id'})}}}
    ]
  }))



  beforeEach(() => {
    service = TestBed.get(AdminRouteGuardService)
    userService = TestBed.get(UserDataService)
    route = TestBed.get(ActivatedRoute)
    localStorageService = TestBed.get(AppLocalStorageService)
    entityService = TestBed.get(EntityService)
  })

  it('should be created', () => {
    expect(service).toBeTruthy()
  })

  it('should check the token and return true if user is admin', () => {
    // Arrange
    spyOn(userService, 'userIsAdmin').and.returnValue(true)
    spyOn(userService, 'userLandingPageIsAdmin').and.returnValue(false)

    // Act
    const canLoad = service.canLoad(null, null)

    // Assert
    expect(canLoad).toBeTruthy()
  })

  it('should check the token and return true if landing page of user is administration', () => {
    // Arrange
    spyOn(userService, 'userIsAdmin').and.returnValue(false)
    spyOn(userService, 'userLandingPageIsAdmin').and.returnValue(true)

    // Act
    const canLoad = service.canLoad(null, null)

    // Assert
    expect(canLoad).toBeTruthy()
  })


  it(`should handle canActivate`, () => {
    // Arrange
    spyOn(service, 'canLoad')

    // Act
    service.canActivate(route.snapshot, null)

    // Assert
    expect(service.canLoad).toHaveBeenCalled()
  })

  it('should navigate to the app root and return false if the admin user isnt authorised', () => {
    // Arrange
    spyOn(userService, 'userIsAdmin').and.returnValue(false)
    spyOn(userService, 'userLandingPageIsAdmin').and.returnValue(false)
    const router = TestBed.get(Router)
    spyOn(service, 'fail')

    // Act
    const canLoad = service.canLoad(null, null)

    // Assert
    expect(service.fail).toHaveBeenCalled()
    expect(canLoad).toBeFalsy()
  })
})

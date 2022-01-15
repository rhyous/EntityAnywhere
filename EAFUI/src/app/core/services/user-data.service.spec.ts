import { TestBed, getTestBed } from '@angular/core/testing'

import { UserDataService } from './user-data.service'
import { AppLocalStorageService } from './local-storage.service'
import { LandingPageTypes } from '../models/enums/LandingPageTypes'

describe('UserDataService', () => {
  let localStorageService: AppLocalStorageService
  let service: UserDataService

  beforeEach(() => TestBed.configureTestingModule({
    providers: [AppLocalStorageService]
  }))

  beforeEach(() => {
    service = getTestBed().get(UserDataService)

    localStorageService = getTestBed().get(AppLocalStorageService)
  })

  it('should be created', () => {
    expect(service).toBeTruthy()
  })

  describe('UserIsAdmin', () => {

    it('should return true if active user is admin', () => {
      // Arrange
      const userRoleClaim = {
        Name: 'Role',
        Issuer: 'LOCAL AUTHORITY',
        Subject: 'UserRole',
        Value: 'Admin',
        ValueType: 'null'
      }
      spyOnProperty(localStorageService, 'User' , 'get').and.returnValue({AdminRole: userRoleClaim})

      // Assert
      expect(service.userIsAdmin()).toBeTruthy()
    })

    it('should return false if active user not admin', () => {
      // Arrange
     spyOnProperty(localStorageService, 'User' , 'get').and.returnValue({AdminRole: null})

      // Assert
     expect(service.userIsAdmin()).toBeFalsy()
    })

  })

  describe('userIsDefaultRole', () => {

    it('should return true if active user is a default', () => {
      // Arrange
      const userRoleClaim = {
        Name: 'Role',
        Issuer: 'LOCAL AUTHORITY',
        Subject: 'UserRole',
        Value: 'Default',
        ValueType: 'null'
      }

      spyOnProperty(localStorageService, 'User' , 'get').and.returnValue({DefaultRole: userRoleClaim})

      // Assert
      expect(service.userIsDefault()).toBeTruthy()
    })

    it('should return false if active user is not a default', () => {
      // Arrange
      spyOnProperty(localStorageService, 'User' , 'get').and.returnValue({DefaultRole: null})

      // Assert
      expect(service.userIsDefault()).toBeFalsy()
    })

  })

  describe('userLandingPageIsAdmin', () => {

    it('should return true if active user landing page is Administration', () => {
      // Arrange
      const landingPageClaim = {
      Name: 'LandingPageId',
      Issuer: 'LOCAL AUTHORITY',
      Subject: 'UserRole',
      Value: LandingPageTypes.Administration.toString(),
      ValueType: 'null'
    }

      spyOnProperty(localStorageService, 'User' , 'get').and.returnValue({PortalLandingPageType: landingPageClaim})

      // Assert
      expect(service.userLandingPageIsAdmin()).toBeTruthy()
    })

    it('should return false if active user landing page is not Administration', () => {

      // Arrange
      const landingPageClaim = {
        Name: 'LandingPageId',
        Issuer: 'LOCAL AUTHORITY',
        Subject: 'UserRole',
        Value: LandingPageTypes.Default.toString(),
        ValueType: 'null'
      }

      spyOnProperty(localStorageService, 'User' , 'get').and.returnValue({PortalLandingPageType: landingPageClaim})

      // Assert
      expect(service.userLandingPageIsAdmin()).toEqual(false)
    })

  })

  describe('userLandingPageIsDefault', () => {

    it('should return true if active user landing page is Default', () => {
      // Arrange
      const landingPageClaim = {
        Name: 'LandingPageId',
        Issuer: 'LOCAL AUTHORITY',
        Subject: 'UserRole',
        Value: LandingPageTypes.Default.toString(),
        ValueType: 'null'
      }

      spyOnProperty(localStorageService, 'User' , 'get').and.returnValue({PortalLandingPageType: landingPageClaim})

      // Assert
      expect(service.userLandingPageIsDefault()).toBeTruthy()
    })

    it('should return false if active user landing page is not Default', () => {

      // Arrange
      const landingPageClaim = {
        Name: 'PortalLandingPageType',
        Issuer: 'LOCAL AUTHORITY',
        Subject: 'UserRole',
        Value: LandingPageTypes.Administration.toString(),
        ValueType: 'null'
      }

      spyOnProperty(localStorageService, 'User' , 'get').and.returnValue({PortalLandingPageType: landingPageClaim})

      // Assert
      expect(service.userLandingPageIsDefault()).toBeFalsy()
    })

  })

  describe('CanDisplayEntityForUser', () => {

    it('should return true if active user is admin', () => {
      // Arrange
      const entityName = 'Product'
      const userRoleClaim = {
        Name: 'Role',
        Issuer: 'LOCAL AUTHORITY',
        Subject: 'UserRole',
        Value: 'Admin',
        ValueType: 'null'
      }
      spyOnProperty(localStorageService, 'User' , 'get').and.returnValue({AdminRole: userRoleClaim})

      // Assert
      expect(service.canDisplayEntityForUser(entityName)).toBeTruthy()
    })

    it('should return true if entity is permitted for user role', () => {
      // Arrange
      const entityName = 'Product'
      spyOnProperty(localStorageService, 'User' , 'get').and.returnValue({AdminRole: null})
      spyOnProperty(service, 'permittedEntitiesForUser' , 'get').and.
                    returnValue([entityName])
      // Assert
      expect(service.canDisplayEntityForUser(entityName)).toBeTruthy()
    })

    it('should return false if entity is not permitted for user role', () => {
      // Arrange
      const entityName = 'Product'
      spyOnProperty(localStorageService, 'User' , 'get').and.returnValue({AdminRole: null})
      spyOnProperty(service, 'permittedEntitiesForUser' , 'get').and.returnValue(['ProductType'])

      // Assert
      expect(service.canDisplayEntityForUser(entityName)).toBeFalsy()
    })
  })
})

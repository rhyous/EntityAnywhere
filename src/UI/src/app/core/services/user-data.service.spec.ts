import { TestBed, getTestBed } from '@angular/core/testing'

import { UserDataService } from './user-data.service'
import { AppLocalStorageService } from './local-storage.service'
import { LandingPageTypes } from '../models/enums/LandingPageTypes'
import { UserDetailData } from 'src/app/features/eaf/eaf-common/common-dashboard-module/components/entity-detail/mock-user-detail-data'
import { Claim } from '../login/login-token.interface'
import { UserLoginData } from '../models/concretes/user-login-data'

describe('UserDataService', () => {
  let localStorageService: AppLocalStorageService
  let service: UserDataService
  let adminUserLoginData: UserLoginData
  let adminRoleClaim: Claim
  let customerUserLoginData: UserLoginData
  let customerRoleClaim: Claim
  let defaultUserLoginData: UserLoginData
  let defaultRoleClaim: Claim

  beforeEach(() => TestBed.configureTestingModule({
    providers: [AppLocalStorageService]
  }))

  beforeEach(() => {
    service = getTestBed().inject(UserDataService)
    localStorageService = getTestBed().inject(AppLocalStorageService)

    adminUserLoginData = new UserLoginData()
    adminRoleClaim = new Claim()
    adminRoleClaim.Name = 'Role'
    adminRoleClaim.Issuer = 'LOCAL AUTHORITY'
    adminRoleClaim.Subject = 'UserRole'
    adminRoleClaim.Value = 'Admin'
    adminRoleClaim.ValueType = 'null'
    adminUserLoginData.Roles = [ adminRoleClaim ]

    customerUserLoginData = new UserLoginData()
    customerRoleClaim = new Claim()
    customerRoleClaim.Name = 'Role'
    customerRoleClaim.Issuer = 'LOCAL AUTHORITY'
    customerRoleClaim.Subject = 'UserRole'
    customerRoleClaim.Value = 'Customer'
    customerRoleClaim.ValueType = 'null'
    customerUserLoginData.Roles = [ customerRoleClaim ]

    defaultUserLoginData = new UserLoginData()
    defaultRoleClaim = new Claim()
    defaultRoleClaim.Name = 'Role'
    defaultRoleClaim.Issuer = 'LOCAL AUTHORITY'
    defaultRoleClaim.Subject = 'UserRole'
    defaultRoleClaim.Value = 'Default'
    defaultRoleClaim.ValueType = 'null'
    defaultUserLoginData.Roles = [ defaultRoleClaim ]
  })

  it('should be created', () => {
    expect(service).toBeTruthy()
  })

  describe('UserIsAdmin', () => {

    it('should return true if active user is admin', () => {
      // Arrange
      spyOnProperty(localStorageService, 'User' , 'get').and.returnValue(adminUserLoginData)

      // act
      const actual = service.userIsAdmin()

      // Assert
      expect(actual).toBeTruthy()
    })

    it('should return false if active user not admin', () => {
      // Arrange
      spyOnProperty(localStorageService, 'User' , 'get').and.returnValue(customerUserLoginData)

      // act
      const actual = service.userIsAdmin()

      // Assert
      expect(actual).toBeFalsy()
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

      spyOnProperty(localStorageService, 'User' , 'get').and.returnValue(defaultUserLoginData)
      // Act
      const actual = service.userIsDefault()

      // Assert
      expect(actual).toBeTruthy()
    })

    it('should return false if active user is not a default', () => {
      // Arrange
      spyOnProperty(localStorageService, 'User' , 'get').and.returnValue(customerUserLoginData)

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

      spyOnProperty(localStorageService, 'User' , 'get').and.returnValue({LandingPageType: landingPageClaim})

      // Assert
      expect(service.userLandingPageIs(LandingPageTypes.Administration)).toBeTruthy()
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
      spyOnProperty(localStorageService, 'User' , 'get').and.returnValue({LandingPageType: landingPageClaim})
      spyOn(service, 'userIsAdmin').and.returnValue(false)

      // Act
      const actual = service.userLandingPageIs(LandingPageTypes.Administration)

      // Assert
      expect(actual).toEqual(false)
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

      spyOn(service, 'userIsAdmin').and.returnValue(false)
      spyOnProperty(localStorageService, 'User' , 'get').and.returnValue({LandingPageType: landingPageClaim})

      // Assert
      expect(service.userLandingPageIs(LandingPageTypes.Default)).toBeTruthy()
    })

    it('should return false if active user landing page is not Default', () => {

      // Arrange
      const landingPageClaim = {
        Name: 'LandingPageType',
        Issuer: 'LOCAL AUTHORITY',
        Subject: 'UserRole',
        Value: LandingPageTypes.Administration.toString(),
        ValueType: 'null'
      }

      spyOnProperty(localStorageService, 'User' , 'get').and.returnValue({LandingPageType: landingPageClaim})

      // Assert
      expect(service.userLandingPageIs(LandingPageTypes.Customer)).toBeFalsy()
    })

  })

  describe('userIsAllowedAdminView', () => {

    it('should return true if user is admin', () => {
          // Arrange

          spyOnProperty(localStorageService, 'User' , 'get').and.returnValue(adminUserLoginData)

          // Assert
          expect(service.userIsAdmin()).toBeTruthy()
    })

    it('should return false if user is not admin', () => {
      // Arrange
      const userLoginData = new UserLoginData()
      userLoginData.Roles = [ customerRoleClaim ]

      spyOnProperty(localStorageService, 'User' , 'get').and.returnValue(userLoginData)

        // Assert
      expect(service.userIsAdmin()).toBeFalsy()
    })

  })

  describe('CanDisplayEntityForUser', () => {

    it('should return true if active user is admin', () => {
      // Arrange
      const entityName = 'Entity1'
      spyOnProperty(localStorageService, 'User' , 'get').and.returnValue(adminUserLoginData)

      // Assert
      expect(service.canDisplayEntityForUser(entityName)).toBeTruthy()
    })

    it('should return true if entity is permitted for user role', () => {
      // Arrange
      const entityName = 'Entity1'
      spyOnProperty(localStorageService, 'User' , 'get').and.returnValue({AdminRole: null})
      spyOnProperty(service, 'permittedEntitiesForUser' , 'get').and.
                    returnValue([entityName])
      spyOn(service, 'userIsAdmin').and.returnValue(false)

      // Act
      const actual = service.canDisplayEntityForUser(entityName)

      // Assert
      expect(actual).toBeTruthy()
    })

    it('should return false if entity is not permitted for user role', () => {
      // Arrange
      const entityName = 'Entity1'

      spyOn(service, 'userIsAdmin').and.returnValue(false)
      spyOnProperty(service, 'permittedEntitiesForUser', 'get').and.returnValue(['Entity2'])

      // Act
      const actual = service.canDisplayEntityForUser(entityName)

      // Assert
      expect(actual).toBeFalsy()
    })
  })
})

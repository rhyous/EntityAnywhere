import { Injectable } from '@angular/core'
import { NullEx } from '../infrastructure/extensions/null-ex'
import { StringEx } from '../infrastructure/extensions/string-ex'
import { LandingPageTypes } from '../models/enums/LandingPageTypes'
import { AppLocalStorageService } from './local-storage.service'

@Injectable({
  providedIn: 'root'
})
export class UserDataService {

  constructor(private localStorageService: AppLocalStorageService) { }

  userIsRole(roleName: string) {
    if (NullEx.isNullOrUndefined(this.localStorageService.User)) {
      return false
    }
    return this.localStorageService.User.hasRole(roleName)
  }

  userIsAdmin() { return this.userIsRole('Admin') }

  userIsCustomer() { return this.userIsRole('Customer') }

  userIsDefault() { return this.userIsRole('Default') }

  userIsImpersonate() { return this.userIsRole('Impersonation') }

  userLandingPageIs(landingPageType: LandingPageTypes) {
    return this.getLandingPageType() === landingPageType
  }

  userIsAllowedAdminView() {
    return this.userIsAdmin() || this.permittedEntitiesForUser.length > 0
  }

  getLandingPageType(): Number {
      let userLandingPageType = Number(this.localStorageService.User?.LandingPageType?.Value ?? LandingPageTypes.Default)
      if (userLandingPageType === LandingPageTypes.Default) {
        userLandingPageType = this.userIsAdmin() ? LandingPageTypes.Administration : LandingPageTypes.Default
      }
      return userLandingPageType
    }

  get permittedEntitiesForUser(): string[] {
      const authorizedRoleDataJson: string = this.localStorageService.authorizedUserRoleData
      return Object.keys(authorizedRoleDataJson) || []
  }

  canDisplayEntityForUser(entityName: string) {
    return this.userIsAdmin() || this.permittedEntitiesForUser.some(x => x === entityName)
  }

  logout() {
    if (this.userIsImpersonate()) {
      this.localStorageService.removeImpersonationClaims()
    } else {
      this.localStorageService.removeClaims()
    }

    this.localStorageService.redirectUrl = []
  }
}

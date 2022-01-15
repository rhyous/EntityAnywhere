import { Injectable } from '@angular/core'
import { LandingPageTypes } from '../models/enums/LandingPageTypes'
import { AppLocalStorageService } from './local-storage.service'

@Injectable({
  providedIn: 'root'
})
export class UserDataService {

  constructor(private localStorageService: AppLocalStorageService) { }

  userIsAdmin() {
    const isAdmin = this.localStorageService.User.AdminRole
    return isAdmin != null ? true : false
  }

  getLandingPageType() : Number {
      let userLandingPageType = Number(this.localStorageService.User.PortalLandingPageType.Value)

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
    return !this.userIsAdmin() ? this.permittedEntitiesForUser.some(x => x === entityName) : true
  }

  logout() {
    this.localStorageService.removeClaims()
    this.localStorageService.redirectUrl = []
  }
}

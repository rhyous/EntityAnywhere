import { Injectable } from '@angular/core'
import { UserDataService } from '../services/user-data.service'
import { Router, UrlSegment, CanLoad, Route } from '@angular/router'
import { AppLocalStorageService } from '../services/local-storage.service'
import { LandingPageTypes } from '../models/enums/LandingPageTypes'

@Injectable({
  providedIn: 'root'
})
/** This service is responsible for only allowing the user to download the Default
 * Module once they have been authenticated
 */
export class DefaultRouteGuardService implements CanLoad {

  constructor(private userService: UserDataService, private router: Router, private localStorage: AppLocalStorageService) { }

  canLoad(route: Route, segments: UrlSegment[]): boolean {
    this.localStorage.redirectUrl = segments

    if (this.userService.userLandingPageIs(LandingPageTypes.Default)) {
      return true
    } else {
      this.router.navigate([''])
      return false
    }
  }
}

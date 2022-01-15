import { Injectable } from '@angular/core'
import { CanLoad, Route, UrlSegment, Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router'
import { UserDataService } from '../services/user-data.service'
import { AppLocalStorageService } from '../services/local-storage.service'
import { LandingPageTypes } from '../models/enums/LandingPageTypes'

@Injectable({
  providedIn: 'root'
})
/** This service is responsible for only allowing the user to download the Admin
 * Module once they have been authenticated
 */
export class AdminRouteGuardService implements CanLoad, CanActivate {
  constructor(private userService: UserDataService,
    private router: Router,
    private localStorageService: AppLocalStorageService) { }

  canLoad(route: Route, segments: UrlSegment[]): boolean {
    this.localStorageService.redirectUrl = segments

    const canUserAccessAdminSection = this.userService.userIsAdmin() || this.userService.getLandingPageType() === LandingPageTypes.Administration

    if (!canUserAccessAdminSection) {
      this.fail()
    }

    return canUserAccessAdminSection
  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): boolean {
    return this.canLoad(null, next.url)
  }

  fail() {
    this.localStorageService.removeClaims()
    this.router.navigate([''])
  }
}

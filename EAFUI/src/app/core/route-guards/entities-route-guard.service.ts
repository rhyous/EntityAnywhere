import { Injectable } from '@angular/core'
import { CanLoad, Route, UrlSegment, Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router'
import { UserDataService } from '../services/user-data.service'
import { AppLocalStorageService } from '../services/local-storage.service'
import { Observable } from 'rxjs'
import { EntityService } from '../services/entity.service'
import { PluralizePipe } from '../pipes/pluralize.pipe'
import { SingularizePipe } from '../pipes/singularize.pipe'
import { GlobalSnackBarService } from '../services/global-snack-bar.service'

@Injectable({
  providedIn: 'root'
})

/** This service is responsible for only allowing the user to view entities permitted for their role **/
export class EntitiesRouteGuardService implements CanActivate {
  constructor(
    private router: Router,
    private singularize: SingularizePipe,
    private userDataService: UserDataService,
    private snackBar: GlobalSnackBarService) { }

  canActivate(next: ActivatedRouteSnapshot): boolean {

      if (this.userDataService.userIsAdmin()) {
       return true
      }

      const currentEntityNamePathFromUrl = next.url[0].path
      const currentEntityName: string = this.singularize.transform(currentEntityNamePathFromUrl) || currentEntityNamePathFromUrl

      const isRouteAllowed = this.userDataService.permittedEntitiesForUser.includes(currentEntityName)

      if (!isRouteAllowed) {
        this.snackBar.open(`You do not have access to manage: ${currentEntityName}`)
        this.router.navigate(['/admin/data-management'])
      }

      return isRouteAllowed
  }

}

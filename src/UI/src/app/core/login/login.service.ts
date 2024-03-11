import { Injectable, EventEmitter } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { Observable } from 'rxjs'
import { environment } from '../../../environments/environment'
import { LoginToken } from './login-token.interface'
import { AppLocalStorageService } from '../services/local-storage.service'
import { EntityService } from '../services/entity.service'
import { DefaultDataService } from '../services/default-data.service'
import { EntityMetadataService } from '../services/entity-metadata.service'
import { Router } from '@angular/router'
import { TypedEventEmitter } from '../infrastructure/typed-event-emitter.interface'
import { UserDataService } from '../services/user-data.service'
import { LandingPageTypes } from '../models/enums/LandingPageTypes'
import { AuthorizationService } from '../services/authorization.service'
import { NullEx } from '../infrastructure/extensions/null-ex'
import { UserLandingPages } from '../models/concretes/user-landing-pages'

@Injectable()
export class LoginService {
  private loginServiceUrl = environment.serviceUrl + 'AuthenticationService'

  public onLoginSuccess: TypedEventEmitter<boolean> = new EventEmitter<boolean>()

  constructor(
    private http: HttpClient,
    private localStorageService: AppLocalStorageService,
    private entityService: EntityService,
    private defaultDataService: DefaultDataService,
    private entityMetadataService: EntityMetadataService,
    private router: Router,
    private userDataService: UserDataService,
    private authorizationService: AuthorizationService
  ) { }

  /**
   * Log in using the Authentication Service
   * @param username The user name
   * @param password The password
   */
  login(username: any, password: any): Observable<LoginToken> {
    const user = { User: username, Password: password, AuthenticationPlugin: 'Users' }
    return this.http.post<LoginToken>(`${this.loginServiceUrl}/Authenticate`, user)
  }

  /**
   * @param accessToken Validate the access token
   * @param id The SSO Token
   */
  validateOAuthToken(accessToken: string, id: string) {
    return this.http.post<LoginToken>(`${environment.serviceUrl}SingleSignOnService/ValidateToken`, {
      AccessToken: accessToken,
      IdUrl: id
    })
  }

  /**
   * Parses the provided token to determine the user's permissions
   * @param token
   */
  parseToken(token: any) {
    this.localStorageService.userClaims = token
    this.onLoginSuccess.emit(true)

    let url = ``
    if (this.localStorageService.redirectUrl && this.localStorageService.redirectUrl.length > 1) {
      for (let i = 1; i < this.localStorageService.redirectUrl.length; i++) {
        url += `/${this.localStorageService.redirectUrl[i].path}`
      }
    }

    this.entityService.getEntities().subscribe(result => {
      this.localStorageService.entityMetaData = JSON.stringify(this.entityMetadataService.convertMetaDataToArray(result.EAF))
    })

    this.authorizationService.getLoggedInUserRoleData().subscribe(result => {
      this.localStorageService.authorizedUserRoleData = JSON.stringify(result)

      const landingPageType = this.userDataService.getLandingPageType()
      if (landingPageType === LandingPageTypes.Default) {
      }
      const pages = UserLandingPages.landingPages
      const data = pages.get(landingPageType)
      let landingPageUrl = ''
      if (!NullEx.isNullOrUndefined(data)) {
        landingPageUrl = data.url
      }
      this.router.navigate([`${landingPageUrl}${url}`])
    })
  }

  logout() {
    this.userDataService.logout()
    this.defaultDataService.logout()
    this.router.navigate(['/'])
    this.localStorageService.loginMethodCookie = ''
  }

  switchUser() {
    this.userDataService.logout()
    this.defaultDataService.logout()
    this.router.navigate([`admin/`])
  }
}

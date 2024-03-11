import { Injectable } from '@angular/core'
import { environment } from '../../../environments/environment'
import { LoginToken, Claim, UserClaim } from '../login/login-token.interface'
import { UrlSegment } from '@angular/router'
import { getRtlScrollAxisType } from '@angular/cdk/platform'
import { UserLoginData } from '../models/concretes/user-login-data'

@Injectable({
  providedIn: 'root'
})
/** Wraps up the concern of getting information from localStorage */
export class AppLocalStorageService {
  localStorageService: any
  constructor() { }

  private cookieConsentName = 'cookie-consent'
  private ssoSignOutCookieName = 'SSOSignOut'
  private naurtechIntroCookieName = 'naurtechIntro'
  private readResMessageCookieName = 'readResMessage'
  private loginMethod = 'loginMethod'

  redirectUrl!: UrlSegment[]

  set userClaims(value: LoginToken) {
    const claimsArray: any = []
    value.ClaimDomains.select<Claim[]>(cd => cd.Claims).forEach(claims => claims.forEach(claim => {
      claimsArray.push(claim)
    }))

    const token = value.Text

    this.addUserClaim(claimsArray, token)
  }

  removeImpersonationClaims() {
    let _userClaims: UserClaim[] = []

    const claimsObj = localStorage.getItem(environment.claims)
    if (claimsObj) {
      _userClaims = claimsObj ? JSON.parse(claimsObj) : ''
    }

    if (_userClaims.length > 1) {
      _userClaims.pop()
    }

    localStorage.setItem(environment.claims, JSON.stringify(_userClaims))
  }

  removeClaims() {
    localStorage.removeItem(environment.claims)
    localStorage.removeItem(environment.adminTokenName)
    localStorage.removeItem(environment.adminTokenName)
  }

  addUserClaim(claimsArray: any[], token: string) {
    let _userClaims: UserClaim[] = []

    const claimsObj = localStorage.getItem(environment.claims)
    if (claimsObj) {
      _userClaims = claimsObj ? JSON.parse(claimsObj) : ''
    }

    const key = _userClaims.length + 1
    const userClaim = {
      Key: key.toString(),
      Claims: claimsArray,
      Token: token
    }

    _userClaims.push(userClaim)
    localStorage.setItem(environment.claims, JSON.stringify(_userClaims))
  }

  getActiveUserClaim(): UserClaim {
    return this.getUserClaims().lastOrDefault()
  }

  getUserClaims(): any[] {
    const claimsObj = localStorage.getItem(environment.claims)
    return claimsObj ? JSON.parse(claimsObj) : []
  }

  get activeToken(): string {
    return this.getActiveUserClaim().Token
  }

  get User(): UserLoginData | null {
    if (localStorage.getItem(environment.claims)) {
      const _userClaims: Claim[] = this.getActiveUserClaim().Claims
      if (_userClaims) {
        const user = new UserLoginData()
        user.Username = _userClaims.firstOrDefault(x => x.Subject === 'User' && x.Name === 'Username')
        user.UserId = _userClaims.firstOrDefault(x => x.Subject === 'User' && x.Name === 'Id')
        user.LastAuthenticated = _userClaims.firstOrDefault(x => x.Subject === 'User' && x.Name === 'LastAuthenticated')
        user.Roles = _userClaims.where(x => x.Subject === 'UserRole' && x.Name === 'Role')
        user.Impersonation = _userClaims.firstOrDefault(x => x.Subject === 'User' && x.Name === 'Impersonation')
        user.LandingPageType = _userClaims.firstOrDefault(x => x.Subject === 'UserRole' && x.Name === 'LandingPageId')
        return user
      }
    }
    return null
  }

  get entityMetaData(): string {
    return localStorage.getItem(environment.metaDataLocalName) ?? ''
  }
  set entityMetaData(meta: string) {
    localStorage.setItem(environment.metaDataLocalName, meta)
  }

  get cookieConsent(): Date {
    return new Date(localStorage.getItem(this.cookieConsentName) ?? '')
  }
  set cookieConsent(val: Date) {
    localStorage.setItem(this.cookieConsentName, val.toString())
  }

  get ssoSignOutCookie() {
    return localStorage.getItem(this.ssoSignOutCookieName)
  }
  set ssoSignOutCookie(value: any) {
    localStorage.setItem(this.ssoSignOutCookieName, value)
  }

  removeSsoSignOutCookie() {
    localStorage.removeItem(this.ssoSignOutCookieName)
  }

  get naurtechIntroCookie(): Date {
    return new Date(localStorage.getItem(this.naurtechIntroCookieName) ?? '')
  }
  set naurtechIntroCookie(v: Date) {
    localStorage.setItem(this.naurtechIntroCookieName, v.toString())
  }

  get readResMessage() {
    const date = new Date(localStorage.getItem(this.readResMessageCookieName) ?? '')
    if (date.toString() === 'Invalid Date') {
      return new Date()
    }
    return date
  }
  set readResMessage(val: Date) {
    localStorage.setItem(this.readResMessageCookieName, val.toString())
  }
  get loginMethodCookie() {
    return localStorage.getItem(this.loginMethod) ?? ''
  }

  set loginMethodCookie(value: string) {
    localStorage.setItem(this.loginMethod, value)
  }

  get authorizedUserRoleData() {
    return JSON.parse(localStorage.getItem(environment.authorizedUserRoleData) ?? '')
  }

  set authorizedUserRoleData(value: string) {
    localStorage.setItem(environment.authorizedUserRoleData,  value)
  }
}

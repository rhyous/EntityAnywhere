import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'

import { Observable } from 'rxjs'

import { environment } from 'src/environments/environment'
import { AppLocalStorageService } from './local-storage.service'
import { LoginToken } from '../login/login-token.interface'

@Injectable({
  providedIn: 'root'
})

export class ImpersonationService {

  private impersonationService = `${environment.serviceUrl}Service/$Impersonate`

  constructor(private http: HttpClient,
              private localStorageService: AppLocalStorageService) { }

  getImpersonationToken(userRoleId: string): Observable<LoginToken> {
    const url = `${this.impersonationService}/${userRoleId}`
    return this.http.get<LoginToken>(url, this.getToken())
  }

  getToken() {
    return {
      headers: {
        Token: this.localStorageService.activeToken
      }
    }
  }
}

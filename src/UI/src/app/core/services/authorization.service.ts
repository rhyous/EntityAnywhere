import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
import { environment } from 'src/environments/environment'
import { AppLocalStorageService } from './local-storage.service'

@Injectable({
    providedIn: 'root'
  })

  export class AuthorizationService {
      constructor(private http: HttpClient,
        private localStorageService: AppLocalStorageService) {}

      getHeader() {
        const header = {headers: {token: this.localStorageService.activeToken}}
        return header
      }

      getLoggedInUserRoleData(): Observable<any> {
        const requestUri = environment.serviceUrl + 'AuthorizationService/MyRoleData'
        return this.http.get(requestUri, this.getHeader())
      }
  }

import { Injectable } from '@angular/core'
import { AppLocalStorageService } from 'src/app/core/services/local-storage.service'
import { environment } from 'src/environments/environment'

@Injectable({
  providedIn: 'root'
})
/** The Default service is responsible for making webcalls that the Default requires */
export class DefaultService {

  private url = environment.serviceUrl

  constructor(private localStorageService: AppLocalStorageService) { }

  getToken(): any {
    return {
      headers: {
        Token: this.localStorageService.activeToken,
      }
    }
  }
}

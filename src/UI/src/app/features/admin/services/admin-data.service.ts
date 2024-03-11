import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { environment } from 'src/environments/environment'
import { AppLocalStorageService } from 'src/app/core/services/local-storage.service'

@Injectable({
  providedIn: 'root'
})
export class AdminDataService {
  private api = `${environment.serviceUrl}`
  private header = {headers: {token: this.localStorageService.activeToken}}

  constructor(private http: HttpClient, private localStorageService: AppLocalStorageService) { }

}

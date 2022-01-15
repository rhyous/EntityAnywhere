import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { Observable } from 'rxjs'

import { environment } from 'src/environments/environment'
import { AppLocalStorageService } from 'src/app/core/services/local-storage.service'

@Injectable({
  providedIn: 'root'
})
export class ProductEntityService {

  constructor(
    private http: HttpClient,
    private localStorage: AppLocalStorageService
    ) { }

  getHeader() {
      const token = this.localStorage.activeToken

      const header = {headers: {token: token}}
      return header
  }

  getProductsBySku(skuText: string, top: number, skip: number): Observable<any> {
      let requestUri = environment.serviceUrl +
        `ProductService.svc/Products/FilterBySku?$filter=contains('Name','${skuText}')&$expand=ProductType`
      requestUri += top !== null ? `&$top=${top}` : ''
      requestUri += skip !== null ? `&$skip=${skip}` : ''
      return this.http.get(requestUri, this.getHeader())
  }
}

import { Injectable } from '@angular/core'
import { Observable } from 'rxjs'
import { StringEx } from 'src/app/core/infrastructure/extensions/string-ex'
import { HttpApiHelpers } from '../infrastructure/http-api.helpers'
import { ODataCollection } from 'src/app/core/models/interfaces/o-data-collection.interface'
import { ApiFile } from '../models/interfaces/o-data-entities/file.interface'
import { HttpClient } from '@angular/common/http'
import { environment } from 'src/environments/environment'
import { AppLocalStorageService } from 'src/app/core/services/local-storage.service'

@Injectable({
  providedIn: 'root'
})
export class FileApiService {
  private get url() {
    return `${environment.serviceUrl}FileService`
  }

  get token() {
    return this.localStorageService.activeToken
  }
  private header = {headers: {Token: this.token}}

  constructor(private http: HttpClient, private localStorageService: AppLocalStorageService) { }

  /** Calls the FileService to see if a Filename already exists for an organizatio */
  checkFileExists(filename: string, organizationId: string, orderId: string, productId: string, productReleaseId: string): Observable<boolean> {
    filename = StringEx.isUndefinedNullOrEmpty(filename) ? '' : `/${filename}`

    const fileUrl = ''

    return this.http.get<boolean>(fileUrl, this.header)
  }

  /** Uploads a file to the File Service */
  postFileUpload(organizationSapId: any, orderId: any, productId: any, file: any, productReleaseId: any) {
    const fileUrl = ''
    return this.http.post(fileUrl, [file], this.header)
  }

  /** Gets all of the files for an organization */
  getFilesByOrganization(sapId: string): Observable<ODataCollection<ApiFile>> {
    return this.http.get<ODataCollection<ApiFile>>(`${this.url}/Organizations(${sapId})/Files`, this.header)
  }

  getFileDataByOrganization(sapId: string, licenseId: string): any {
    return this.http.get<Blob>(`${this.url}/Organizations(${sapId})/Files(${licenseId})/Data`,
                               HttpApiHelpers.getOctetStreamHeader(this.token))
  }

  getFilesByProduct(productId: number): Observable<ODataCollection<ApiFile>> {
    return this.http.get<ODataCollection<ApiFile>>(`${this.url}/Products(${productId})/Files`, this.header)
  }

  deleteFile(fileId: string) {
    return this.http.delete(`${this.url}/Files(${fileId})`, this.header)
  }
}

import { Injectable } from '@angular/core'
import { Observable, throwError } from 'rxjs'
import { catchError } from 'rxjs/operators'
import { environment } from 'src/environments/environment'
import { PageEvent } from '@angular/material/paginator'
import { PluralizePipe } from '../pipes/pluralize.pipe'
import { HttpClient } from '@angular/common/http'
import { Router } from '@angular/router'
import { ODataCollection } from '../models/interfaces/o-data-collection.interface'
import { EntitySchema } from '../models/interfaces/entity-schema.interface'
import { Metadata } from '../models/interfaces/metadata.interface'
import { AppLocalStorageService } from './local-storage.service'
import { ODataObject } from '../models/interfaces/o-data-entities/o-data-object.interface'
import { PatchedEntity, PatchedEntityCollection } from '../models/interfaces/patch-entity-object.interface'
import { ApiRequestUrlHelpers } from '../infrastructure/api-request-url-helpers'
import { StringEx } from '../infrastructure/extensions/string-ex'

@Injectable({
  providedIn: 'root'
})
/** The EntityService is responsible for exposing the necessary HTTP calls to the Entity Services */
export class EntityService {

  pageEvent!: PageEvent

  constructor(
    private http: HttpClient,
    private router: Router,
    private localStorage: AppLocalStorageService,
    private pluralizePipe: PluralizePipe
    ) { }

    /**
     * Get the User Token header
     */
  getHeader() {
    const token = this.localStorage.activeToken

    const header = {headers: {token: token}}
    return header
  }

  /**
   * Get all the entities
   */
  getEntities() {
    return this.http.get<Metadata>(environment.serviceUrl + 'Service/$MetaData')
      // .map((entities) => entities.json() as Entity[])
      // .map((res:Response) => res.json())
  }

  getAllData(entityName: string): Observable<any> {
    const entityPath = entityName + 'Service/' + this.pluralizePipe.transform(entityName)
    return this.http.get<Response>(environment.serviceUrl + entityPath, this.getHeader())
  }

  getEntityList(entityName: string, top: number = <any>null, skip: number = <any>null): Observable<any> {
    let requestUri = environment.serviceUrl + entityName + 'Service/' + this.pluralizePipe.transform(entityName)
    requestUri += top !== null ? `${this.queryConnector(requestUri)}$top=${top}` : ''
    requestUri += skip !== null ? `${this.queryConnector(requestUri)}$skip=${skip}` : ''
    return this.http.get(requestUri, this.getHeader())
  }

  getExpandedEntityList(entityName: string, expandedEntities: string[], top: number = <any>null, skip: number = <any>null)
  : Observable<ODataCollection<any>> {
    let requestUri = environment.serviceUrl + entityName + 'Service/' + this.pluralizePipe.transform(entityName)
    requestUri += expandedEntities.length > 0 ? `${this.queryConnector(requestUri)}$expand=${expandedEntities.join(',')}` : ''
    requestUri += top !== null ? `${this.queryConnector(requestUri)}$top=${top}` : ''
    requestUri += skip !== null ? `${this.queryConnector(requestUri)}$skip=${skip}` : ''
    return this.http.get<ODataCollection<any>>(requestUri, this.getHeader())
  }

  getEntityFilteredList(entityName: string, filterText: string, properties: string[], orderBy: string = <any>null, top: number = <any>null,
    skip: number = <any>null, nonContainsProperties: string[] = [])
    : Observable<any> {
    // This loops through entity properties and adds contains params to the filter param
    // Obviously this is ridiculous and this needs to be changed to use $search when it is implemented
    let requestUri = environment.serviceUrl + entityName + 'Service/' + this.pluralizePipe.transform(entityName)
    requestUri += '?$filter='
    properties.forEach(prop => {
      if (nonContainsProperties.indexOf(prop) === -1) {
        requestUri += `contains(${prop}, ${ApiRequestUrlHelpers.cleanseFilterQueryStringForApiRequest(filterText)}) or `
      } else {
        requestUri += `${prop} eq ${ApiRequestUrlHelpers.cleanseFilterQueryStringForApiRequest(filterText)} or `
      }
    })
    requestUri = requestUri.substring(0, requestUri.length - 4)
    requestUri += top !== null ? `${this.queryConnector(requestUri)}$top=${top}` : ''
    requestUri += skip !== null ? `${this.queryConnector(requestUri)}$skip=${skip}` : ''
    if (orderBy) {
      requestUri += `&$orderBy=${orderBy}`
    }
    return this.http.get(requestUri, this.getHeader())
  }

  getFilteredEntityList<T>(entityName: string, filter: string, top: number = <any>null, skip: number = <any>null): Observable<ODataCollection<T>> {
    let requestUri = environment.serviceUrl + entityName + 'Service/' + this.pluralizePipe.transform(entityName)
    requestUri += filter && filter !== '' ? `?$filter=${filter}` : ''
    requestUri += top !== null ? `${this.queryConnector(requestUri)}$top=${top}` : ''
    requestUri += skip !== null ? `${this.queryConnector(requestUri)}$skip=${skip}` : ''
    return this.http.get<ODataCollection<T>>(requestUri, this.getHeader())
  }

  getEntityListByCustomUrl<T>(entityName: string, customUrl: string): Observable<T> {
    const requestUri = environment.serviceUrl + entityName + 'Service/' + this.pluralizePipe.transform(entityName) + '/' + customUrl
    return this.http.get<T>(requestUri, this.getHeader())
  }

  getExpandedFilteredEntityList(entityName: string, filter: string, expandedEntities: string[], top: number, skip: number)
      : Observable<ODataCollection<any>> {
    let requestUri = environment.serviceUrl + entityName + 'Service/' + this.pluralizePipe.transform(entityName)
    requestUri += filter && filter !== '' ?  `${this.queryConnector(requestUri)}$filter=${filter}` : ''
    requestUri += expandedEntities.length > 0 ? `${this.queryConnector(requestUri)}$expand=${expandedEntities.join(',')}` : ''
    requestUri += top !== null ? `${this.queryConnector(requestUri)}$top=${top}` : ''
    requestUri += skip !== null ? `${this.queryConnector(requestUri)}$skip=${skip}` : ''
    return this.http.get<ODataCollection<any>>(requestUri, this.getHeader())
  }

  getFilteredMapList(entityName: string, targetEntity: string, mappedEntity: string, mappedFilter: string, entityId: number,
                     top: number, skip: number): Observable<any> {
    const filter = StringEx.isUndefinedNullOrWhitespace(mappedFilter) ? '' : `($filter=${mappedFilter})`
    let requestUri = environment.serviceUrl + entityName + 'Service/' + this.pluralizePipe.transform(entityName) + `(${entityId})`
    requestUri += `?$expand=${targetEntity}($Expand=${mappedEntity}${filter})`
    requestUri += `${this.queryConnector(requestUri)}$top=${top}${this.queryConnector(requestUri)}$skip=${skip}`
    return this.http.get(requestUri, this.getHeader())
  }

  getEntityMetaData(entityName: string)  {
    return this.http.get<EntitySchema>(environment.serviceUrl + entityName + 'Service/$metadata', this.getHeader())
  }

  getEntityData(entityName: string, entityId: any): Observable<any> {
    const entityPath = entityName + 'Service/' + this.pluralizePipe.transform(entityName) + `(${entityId})`
    return this.http.get<Response>(environment.serviceUrl + entityPath, this.getHeader())
  }

  addEntity(entityName: string, entities: any[]): Observable<ODataCollection<any>> {
    const servicePath = `${entityName}Service/${this.pluralizePipe.transform(entityName)}`
    return this.http.post<ODataCollection<any>>(environment.serviceUrl + servicePath, entities, this.getHeader())
  }

  addTypedEntity<T>(entityName: string, entities: T[]): Observable<ODataCollection<T>> {
    const servicePath = `${entityName}Service/${this.pluralizePipe.transform(entityName)}`
    return this.http.post<ODataCollection<T>>(environment.serviceUrl + servicePath, entities, this.getHeader())
  }

  putEntity(entityName: string, entityId: string, entityObject: any): Observable<any> {
    const servicePath = `${entityName}Service/${this.pluralizePipe.transform(entityName)}(${entityId})`
    return this.http.put(environment.serviceUrl + servicePath, entityObject, this.getHeader())
      // .do(data => console.log('putEntity: ' + JSON.stringify(data)))
      .pipe(catchError(this.handleError))
  }

  patchEntity(entityName: string, entityId: string, patchedEntityObject: PatchedEntity): Observable<ODataObject<any>> {
    const servicePath = `${entityName}Service/${this.pluralizePipe.transform(entityName)}(${entityId})`
    return this.http.patch<ODataObject<any>>(environment.serviceUrl + servicePath, patchedEntityObject, this.getHeader())
  }

  patchManyEntities(entityName: string, patchedEntityCollection: PatchedEntityCollection): Observable<ODataObject<any>> {
    const servicePath = `${entityName}Service/${this.pluralizePipe.transform(entityName)}`
    return this.http.patch<ODataObject<any>>(environment.serviceUrl + servicePath, patchedEntityCollection, this.getHeader())
  }

  deleteEntity(entityName: string, entityId: number): Observable<boolean> {
    const servicePath = `${entityName}Service/${this.pluralizePipe.transform(entityName)}(${entityId})`
    return this.http.delete<boolean>(environment.serviceUrl + servicePath, this.getHeader())
  }

  bulkDeleteEntities(entityName: string, entityIds: string[]): Observable<string> {
    const servicePath = `${entityName}Service/${this.pluralizePipe.transform(entityName)}/Bulk`
    // const obj = { Ids: entityIds }
    return this.http.request<string>('delete', environment.serviceUrl + servicePath, { body: entityIds, headers: <any>this.getHeader().headers })
  }

  queryConnector(uri: string): string {
    return uri.indexOf('?') > 0 ? '&' : '?'
  }

  checkToken() {
    const iuToken = this.localStorage.activeToken
    if (iuToken === undefined || iuToken === null) {
      this.router.navigate(['/expired'])
      return
    }
  }

  private handleError(error: Response): Observable<any> {
    console.error(error)
    return throwError(error)
  }

  getDistinctExtensionPropertList(entityName: string, byEntity: string): Observable<any> {
    const entityPath = `${entityName}Service/${this.pluralizePipe.transform(entityName)}/${byEntity}/Property/Distinct`
    return this.http.get(environment.serviceUrl + entityPath, this.getHeader())
  }

  getDistinctPropertList(entityName: string, propertyName: string): Observable<any> {
    const entityPath = `${entityName}Service/${this.pluralizePipe.transform(entityName)}/${propertyName}/Distinct`
    return this.http.get(environment.serviceUrl + entityPath, this.getHeader())
  }

}


import { Component, ViewChild, OnInit, AfterViewInit } from '@angular/core'
import { Router, ActivatedRoute } from '@angular/router'
import { DatePipe } from '@angular/common'
import { SelectionModel } from '@angular/cdk/collections'
import { MatDialog } from '@angular/material/dialog'
import { PageEvent } from '@angular/material/paginator'
import { MatSnackBar } from '@angular/material/snack-bar'
import { MatSort } from '@angular/material/sort'
import { MatTableDataSource } from '@angular/material/table'
import { FormGroup, FormBuilder } from '@angular/forms'
import { forkJoin, Subject } from 'rxjs'
import { debounceTime, map } from 'rxjs/operators'

import { EntityService } from 'src/app/core/services/entity.service'
import { EntityMetadataService } from 'src/app/core/services/entity-metadata.service'
import { ConfirmDialogComponent } from 'src/app/core/dialogs/confirm-dialog/confirm-dialog.component'
import { ArraySortOrderPipe } from 'src/app/core/pipes/array-sort-order.pipe'
import { SingularizePipe } from 'src/app/core/pipes/singularize.pipe'
import { environment } from 'src/environments/environment'
import { PageFilter } from '../../../models/concretes/page-filters'
import { StorageService } from '../../services/storage.service'
import { EntityMetadata } from 'src/app/core/models/concretes/entity-metadata'
import { EntityHelperService } from '../../../common-form-module/services/entity-helper.service'
import { ODataCollection } from 'src/app/core/models/interfaces/o-data-collection.interface'
// tslint:disable-next-line: max-line-length
import { EntityMultiUpdateDialogComponent } from '../../../common-dialogs-module/components/entity-multi-update-dialog/entity-multi-update-dialog.component'
import { WellKnownProperties } from 'src/app/core/models/concretes/well-known-properties'

@Component({
  selector: 'app-entity-list',
  styleUrls: ['entity-list.component.scss'],
  templateUrl: 'entity-list.component.html',
  providers: [ArraySortOrderPipe]
})

/**
 * The Entity List Component. Responsible for listing all related entities under a sub banner
 */
export class EntityListComponent implements OnInit, AfterViewInit {
  @ViewChild(MatSort, {static: true}) sort!: MatSort

  dataSource: any = new MatTableDataSource([])

  count!: number
  entityName!: string
  entityNamePlural!: string
  entityProperties!: any[]
  nonContainsProperties: any = []
  filterSettings!: PageFilter
  pageSizeOptions = environment.pageSizeOptions
  pageEvent!: PageEvent
  selection = new SelectionModel<number>(true, [])
  displayedColumns: any = []
  showProgressBar = true
  subject: Subject<string> = new Subject()
  filteredList = false
  displayTable = false
  metaDataObject!: EntityMetadata

  get entityFilters(): FormGroup {
    return this.storageService.entityFilters ? this.storageService.entityFilters : this.fb.group({})
  }

  latestEntityListResponse!: ODataCollection<any>
  lastSearch: any

  constructor(
    private entityService: EntityService,
    private entityMetadataService: EntityMetadataService,
    private route: ActivatedRoute,
    private router: Router,
    private datePipe: DatePipe,
    public matDialog: MatDialog,
    public snackBar: MatSnackBar,
    private storageService: StorageService,
    private arraySortByOrder: ArraySortOrderPipe,
    private fb: FormBuilder,
    private helper: EntityHelperService,
    private activatedRoute: ActivatedRoute,
    private singularizePipe: SingularizePipe,
    private wellKnownProperties: WellKnownProperties
  ) { }

  ngOnInit() {
    this.activatedRoute.params.subscribe(() => {
      this.initialise()
    })
  }

  initialise() {
    let param = ''
    this.route.params.pipe(map(e => e['entityPlural'])).subscribe(s => param = s)
    this.entityName = this.singularizePipe.transform(param)
    this.entityNamePlural = param
    this.filterSettings = this.storageService.retrieveFilterSettings(this.entityName)

    this.getEntityPropertiesFromMetadata()
    this.storageService.initEntityFilters(this.entityProperties.select<string>(x => x.name), this.entityNamePlural)
    this.displayedColumns = ['select'].concat(this.arraySortByOrder.transform(this.entityProperties).map((x) => x.name))
    this.updateGridData(true)

    this.entityFilters.valueChanges.pipe(debounceTime(environment.debounceTimeInMs)).subscribe(filter => this.filter(filter))

    // Every time the sort changes we want to refilter again
    // so we can hit the API again to refetch the data
    this.sort.sortChange.subscribe(next => {
      this.filter(this.entityFilters.value)
    })
  }

  filter(filter: any) {
    this.showProgressBar = true

    // If the user has previously searched
    if (this.lastSearch) {
        let searchCriteriaIsSmaller = false
        // Have I been given a smaller set of filter data? If so then I need to hit the server for more data
        Object.keys(filter).forEach(x => {
          if (filter[x].filter.length < this.lastSearch[x].filter.length) {
            searchCriteriaIsSmaller = true
          }
          if (filter[x].exactMatch !== this.lastSearch[x].exactMatch) {
            // Has the "Exact Match" changed? If so hit the server again
            searchCriteriaIsSmaller = true
          }
        })
        if (searchCriteriaIsSmaller) {
          // Search criteria is smaller. Filtering with Server
          this.filterWithServer(filter)
        } else {
          if (this.latestEntityListResponse.Count < this.filterSettings.PageSize) {
            // I have enough data to filter again. Filtering in memory
            this.filterInMemory(filter)
          } else {
            // I dont have enough data to filter again. Filtering with Server
            this.filterWithServer(filter)
          }
        }
    } else {
      // User has not search before. Filtering with server
      this.filterWithServer(filter)
    }

    this.lastSearch = filter // store the latest search
  }

  returnToMenu() {
    this.router.navigate(['./admin/data-administration'])
  }

  filterWithServer(filter: any) {
    this.showProgressBar = true
    let filterString = this.helper.createFilterString(filter)
    const orderBy = this.sort.active ? this.sort.active : 'Id'
    // this.sort.direction will always equate to asc | desc which is as consequential as you might decide
    // if ever there is a problem however then maybe you should check this value
    filterString += this.helper.getOrderByString({direction: <'asc' | 'desc'>this.sort.direction, propertyName: orderBy})
    const fields = this.metaDataObject.Fields.where(x => x.hasNavigationKey()).select<string>(y => y.NavigationKey)

    this.entityService.getExpandedFilteredEntityList(this.entityName,
                                                     filterString,
                                                     fields,
                                                     this.filterSettings.PageSize,
                                                     this.filterSettings.PageSize * this.filterSettings.PageIndex)
      .subscribe(response => {
        // We didn't get any data because the user is too many pages in front
        if (response.Count === 0 && this.filterSettings.PageIndex > 0) {

          this.filterSettings.PageIndex = 0
          this.filterWithServer(filter)
          return
        }

        this.latestEntityListResponse = response

        this.updateDatasource(this.latestEntityListResponse)
        this.showProgressBar = false
      },
                 () => { this.showProgressBar = false })
  }

  filterInMemory(filter: any) {
    if (this.latestEntityListResponse && this.latestEntityListResponse.Entities) {
      const result = this.latestEntityListResponse.Entities.odataFacetedSearch(filter)
      if (result.length === 0) {
        // No results from in Memory. Need to check the server again
        this.filterWithServer(filter)
      } else {
        this.latestEntityListResponse = {
          Count: result.length,
          Entities: result,
          TotalCount: result.length,
          Entity: this.entityName
        }
        this.updateDatasource(this.latestEntityListResponse)
        this.showProgressBar = false
      }
    } else {
      // Latest entity response or latest entity response entities are undefined. Filtering with server
      this.filterWithServer(filter)
    }
  }

  get userIsFiltering(): boolean {
    if (this.lastSearch) {
      let _userIsFiltering = false
      Object.keys(this.lastSearch).forEach(x => {
        if (this.lastSearch[x].filter.length > 0) {
          _userIsFiltering = true
        }
      })
      return _userIsFiltering
    } else {
      return false
    }
  }

  ngAfterViewInit() {
      setTimeout(() => this.dataSource.sort = this.sort, 3000)
  }

  /**
   *  When the user clicks on a row I want them to navigate to the entity details
   * @param entityRow
   */
  rowClick(entityRow: any) {
    if (!window.getSelection()?.toString()) {
      this.router.navigate([`./admin/data-administration/${this.entityName}/${entityRow.Id}`])
    }
  }

  updateGridData(refreshData: boolean) {
    this.storageService.storeFilterSettings(this.entityName, this.filterSettings.FilterProperty, this.filterSettings.FilterText,
                                            this.filterSettings.PageSize, this.filterSettings.PageIndex)

    if (refreshData) {
        this.selection.clear()
        this.getEntityList()
    }
  }

  updateDatasource(response: ODataCollection<any>) {
    this.dataSource = new MatTableDataSource(this.formatEntities(response))
    this.count = response.TotalCount
    this.displayTable = true
    this.dataSource.sort = this.sort
    if (response.TotalCount <= this.filterSettings.PageSize) { // reset the page index
      this.filterSettings.PageIndex = 0
    }
  }

  // Applies formatting to returned entities
  formatEntities(response: ODataCollection<any>) {
    const formattedData: any = []
    if (response.Entities !== undefined) {
      response.Entities.forEach(entity => {
        if (entity.Object !== undefined) {
          this.metaDataObject
              .Fields
              .where(x => !x.isAuditable(this.wellKnownProperties.auditableProperties))
              .select<string>(x => x.Name).forEach((prop) => {
            const metadataField = this.metaDataObject.getField(prop)
            if (metadataField.isDate() && entity.Object[prop]) {
              entity.Object[prop] = new Date(entity.Object[prop])
            }
            if (metadataField.isEnum()) {
              entity.Object[prop] = metadataField.Options.get(entity.Object[prop])
            }
            if (metadataField
                && metadataField.hasNavigationKey()
                && entity.RelatedEntityCollection) {
              const metaField = metadataField
              const meta = JSON.parse(localStorage.getItem(environment.metaDataLocalName) ?? '')
              const metaObj = meta.firstOrDefault((x: any) => x.key === metaField.NavigationKey)

              if (metaObj) {
                const displayField = metaObj.value['@UI.DisplayName']
                  ? metaObj.value['@UI.DisplayName']['$PropertyPath']
                  : 'Name'

                const relatedEntity = entity.RelatedEntityCollection.firstOrDefault(x => x.RelatedEntity === metaField.NavigationKey)
                if (relatedEntity != null && relatedEntity.RelatedEntities) {
                  const relatedEntityObject = relatedEntity.RelatedEntities.firstOrDefault(
                      x => x.Object.Id.toString() === entity.Object[prop].toString())
                  if (relatedEntityObject) {
                    const name = `${metaField.NavigationKey}${displayField}`
                    entity.Object[name] = `${relatedEntityObject.Object[displayField]}`
                    if (this.entityProperties.firstOrDefault(x => x.name === name) === null) {
                      const neighbour = this.entityProperties.firstOrDefault(x => x.name === metaField.Name)
                      this.entityProperties.push({name: name, order: neighbour.order, searchable: false})
                    }
                  }
                }
              }
            }
          })
          formattedData.push(entity.Object)
        }
      })
    }

    this.entityProperties = this.entityProperties.orderBy(x => x.order)
    this.displayedColumns = ['select']
    this.entityProperties.select(x => x.name).forEach(y => this.displayedColumns.push(y))

    return formattedData
  }

  formatCell(input: any) {
    if (typeof input !== typeof new Date() ) {
      return input
    }
    return this.datePipe.transform(input, 'MMM dd, yyyy')
  }

  getEntityList() {
    this.filteredList = false
    this.showProgressBar = true
    this.filterWithServer(this.entityFilters.value)
  }

  getNonContainsProperties(schema: EntityMetadata): string[] {
    const propertyNames: any = []
    schema.Fields.forEach(field => {
      if (field.isNonContains()) {
        propertyNames.push(field.Name)
      }
    })
    return propertyNames
  }

  getEntityPropertiesFromMetadata() {
    // Okay, so at this point the metadata should be cached so if we can find the information
    // in the cache then we retrieve it from there, otherwise call the server
    const md = JSON.parse(localStorage.getItem(environment.metaDataLocalName) ?? '')
    const entity = md.find((x: any) => x.key === this.entityName)
    if (entity === undefined) {
      this.router.navigate(['/admin/dashboard'])
      throw new Error(`"${this.entityNamePlural}" is not a known entity. Please check the URL`)
    }
    this.metaDataObject = this.entityMetadataService.getEntityFromMetaData(entity)
    this.entityProperties = this.processMetaData(this.metaDataObject)
    this.nonContainsProperties = this.getNonContainsProperties(this.metaDataObject)
    this.displayedColumns = ['select'].concat(this.arraySortByOrder.transform(this.entityProperties).map((x) => x))
  }

  // Take an entity schema and obtain the properties
  // populate the entityProperties from this.
  processMetaData(schema: EntityMetadata): any[] {
    const propertyNames: any = []

    schema.Fields.forEach(field => {
      if (!field.isAuditable(this.wellKnownProperties.auditableProperties) && !field.isNavigationProperty() && !field.isMapping()) {
        propertyNames.push({name: field.Name, order: field.DisplayOrder, searchable: true})
      }
    })
    return propertyNames
  }

  pageChange(event: any) {
    this.pageEvent = event
    this.filterSettings.PageSize = this.pageEvent.pageSize
    this.filterSettings.PageIndex = this.pageEvent.pageIndex
    if (this.userIsFiltering) {
      this.filterWithServer(this.lastSearch)
    } else {
      this.updateGridData(true)
    }
  }

  deleteEntities(): void {
    // Show dialog confirming number of selected entities
    // If yes, delete selected entities
    if (this.selection.selected.length === 0) { return }
    const entName = this.selection.selected.length === 1 ? this.entityName : this.selection.selected.length + ' ' + this.entityNamePlural
    const message = `Are you sure you wish to delete the selected ${entName}?`
    const title = 'Confirm delete'
    const confirmationDialog = this.matDialog.open(ConfirmDialogComponent,
                                                   {
      width: '400px',
      data : {
        title: title,
        message: message
      }
    })

    confirmationDialog.afterClosed().subscribe(result => {
      if (result !== 'confirmed') { return }
      this.processDeletes(this.selection.selected)
    })
  }

  processDeletes(entityIds: number[]) {
    const deleteCalls: any = []
    entityIds.forEach(entityId => {
      deleteCalls.push(this.entityService.deleteEntity(this.entityName, entityId))
    })
    forkJoin(deleteCalls).subscribe(() => {
      // Individual call returns
    },                              () => {
      this.snackBar.open(`There was an issue deleting the selected ${this.entityNamePlural.toLowerCase()}`,
                         <any>null, { duration: environment.snackBarDuration })
    },                              () => {
      this.snackBar.open(`The ${this.entityNamePlural.toLowerCase()} were successfully deleted`,
                         <any>null, { duration: 10000 })
      this.selection.clear()
      this.filterSettings.PageIndex = 0
      this.updateGridData(true)
    })
  }

  createNew() {
    this.router.navigate([`./admin/data-administration/${this.entityName}/add`])
  }

  multiUpdate() {
    this.matDialog.open(EntityMultiUpdateDialogComponent,
    {
      width: '800px',
      data : { entityName: this.entityName, selectedRecordIds: this.selection.selected}
    }).afterClosed().subscribe((response) => {
      if (response === 'Update successful') {
        this.snackBar.open(this.entityNamePlural + ' have been updated successfully', <any>null, {duration: environment.snackBarDuration})
        this.updateGridData(true)
      }
    })
  }
}


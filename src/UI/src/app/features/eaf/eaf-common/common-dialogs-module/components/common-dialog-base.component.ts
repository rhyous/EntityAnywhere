import { OnInit, Directive, Component, Inject } from '@angular/core'
import { FormBuilder, FormControl, FormGroup } from '@angular/forms'
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'
import { MatTableDataSource } from '@angular/material/table'
import { Subject } from 'rxjs'
import { debounceTime } from 'rxjs/operators'
import { ErrorReporterDialogComponent } from 'src/app/core/dialogs/error-reporter-dialog/error-reporter-dialog.component'
import { ApiRequestUrlHelpers } from 'src/app/core/infrastructure/api-request-url-helpers'
import { EntityMetadata } from 'src/app/core/models/concretes/entity-metadata'
import { ODataObject } from 'src/app/core/models/interfaces/o-data-entities/o-data-object.interface'
import { ArraySortOrderPipe } from 'src/app/core/pipes/array-sort-order.pipe'
import { EntityMetadataService } from 'src/app/core/services/entity-metadata.service'
import { EntityService } from 'src/app/core/services/entity.service'
import { environment } from 'src/environments/environment'

@Component({
  template: ``
})
export class CommonDialogBaseComponent implements OnInit {

  form!: FormGroup
  datasource = new MatTableDataSource()
  subject: Subject<string> = new Subject()
  entityName: string
  metaData!: EntityMetadata
  auditableFields = ['CreateDate', 'LastUpdated', 'CreatedBy', 'LastUpdatedBy']
  filterableTypes = [ 'Edm.String', 'Edm.Int32', 'Edm.Int64']
  displayProgress = false
  displayedColumns: any = []
  filterFields: any = []
  count = 0
  pageIndex = 0
  pageSize = 10
  filterText = ''
  defaultEntity?: {Name: string, Value: number}
  hasDefaultFilter = false

  entityService: EntityService
  entityMetadataService: EntityMetadataService
  errorReporterDialog: ErrorReporterDialogComponent
  arraySortByOrder: ArraySortOrderPipe

  constructor(public dialogRef: MatDialogRef<any>,
    private formBuilder: FormBuilder,
    entityMetadataService: EntityMetadataService,
    entityService: EntityService,
    errorReporterDialog: ErrorReporterDialogComponent,
    arraySortByOrder: ArraySortOrderPipe,
    @Inject(String) entityName: string,
    @Inject({String, Number}) defaultEntity: {Name: string, Value: number}) {
      this.entityService = entityService,
      this.entityMetadataService = entityMetadataService,
      this.errorReporterDialog = errorReporterDialog,
      this.arraySortByOrder = arraySortByOrder,
      this.entityName = entityName,
      this.defaultEntity = defaultEntity
     }

  ngOnInit() {
    this.loadMetaData()
    this.setupFilterFields(this.metaData)
    this.createForm(this.filterFields)
    this.subject.pipe(debounceTime(environment.debounceTimeInMs)).subscribe(filterText => {this.onFilterChanged()})
    this.filterData()
  }

  loadMetaData(): any {
    const entityMetaData = this.entityMetadataService.getEntityMetaData(this.entityName)
    this.metaData = this.entityMetadataService.getEntityFromMetaData(entityMetaData)
  }

  setupFilterFields(md: EntityMetadata) {

    this.filterFields = md.getSearchFields()
      .filter(field => this.auditableFields.indexOf(field.Name) === -1)
      .map(field => ({Name: field.Name, Type: field.Type, Flex: 20, order: field.DisplayOrder}))

    this.filterFields = this.arraySortByOrder.transform(this.filterFields)
    this.displayedColumns = this.displayedColumns.concat(this.filterFields.map((x: any) => x.Name))
  }

  createForm(fields: any) {
    const group = this.formBuilder.group({})
    fields.forEach((field: any) => {
      group.addControl(field.Name, this.formBuilder.control(''))
    })
    this.form = group
  }

  onFilterChanged(): any {
    this.hasDefaultFilter = false
    this.filterText = ''
    let andClause = ''

    Object.keys(this.form.controls).forEach(key => {
      const control = this.form.controls[key]
      if (this.metaData.getField(key).isString()) {
        const cleansedFilterValue = ApiRequestUrlHelpers.cleanseFilterQueryStringForApiRequest(control.value)
        this.filterText += control.value ? `${andClause}contains(${key}, ${cleansedFilterValue})` : ''
      } else {
        this.filterText += control.value ? `${andClause}${key} eq '${control.value}'` : ''
      }
      this.checkDefaultValueFilter(key, control.value)
      andClause = this.filterText ? ' and ' : ''
    })

    this.pageIndex = 0
    this.filterData()
  }

  checkDefaultValueFilter(key: string, value: string) {
    if (this.defaultEntity) {
      if (key === 'Id') {
        if (value === this.defaultEntity.Value.toString()) {
          this.hasDefaultFilter = true
        }
      }
      if (key === 'Name') {
        if (value.toLowerCase() === this.defaultEntity.Name.toLowerCase()) {
          this.hasDefaultFilter = true
        }
      }
    }
  }

  filterData() {
    this.displayProgress = true
    const service = this.entityService.getFilteredEntityList(this.entityName, this.filterText, this.pageSize, this.pageIndex * this.pageSize)
    this.resolveFilterService(service)
  }

  resolveFilterService(service: any) {
    service.subscribe((results: any) => {
        let entities = []
        if (results.Entities !== undefined) {
          this.count = results.TotalCount
          entities = results.Entities
          if (this.defaultEntity && this.pageIndex === 0) {
            const defaultEntity = this.createDefaultEntity()
            entities.push(defaultEntity)
            entities = entities.orderBy((x: any) => x.Id)
            this.count = results.TotalCount + 1
          }
          this.datasource.data = entities
        } else if (this.hasDefaultFilter) {
          const defaultEntity = this.createDefaultEntity()
          entities.push(defaultEntity)
          this.datasource.data = entities
          this.count = 1

        } else {
          this.datasource.data = []
          this.count = 0
        }
        this.displayProgress = false
      },
      (error: any) => {
        this.displayProgress = false
        this.errorReporterDialog.displayMessage(error.error)
      }
    )
  }

  createDefaultEntity(): ODataObject<any> {
    const oDataObject = {
      Id: this.defaultEntity?.Value,
      Object: {
        Id: this.defaultEntity?.Value,
        Name: this.defaultEntity?.Name,
      },
    }
    return oDataObject
  }

  pageChange(event: any) {
    this.pageIndex = event.pageIndex
    this.filterData()
  }

  cancel() {
    this.dialogRef.close('Cancel')
  }

  onKeyUp(event: any) {
    this.subject.next(this.form.get(event.target.attributes['data-placeholder'].value)?.value)
  }

  getPropValue(row: any, column: any) {
    return row.Object[column]
  }

}

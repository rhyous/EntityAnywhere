import { Component, OnInit, ViewChild } from '@angular/core'
import { FormGroup } from '@angular/forms'
import { SelectionModel } from '@angular/cdk/collections'
import { MatDialog } from '@angular/material/dialog'
import { MatPaginator } from '@angular/material/paginator'
import { MatSnackBar } from '@angular/material/snack-bar'
import { MatSort } from '@angular/material/sort'
import { MatTableDataSource } from '@angular/material/table'
import { forkJoin } from 'rxjs'

import { ArraySortOrderPipe } from 'src/app/core/pipes/array-sort-order.pipe'
import { FieldConfig } from '../../models/interfaces/field-config.interface'
import { EntityService } from 'src/app/core/services/entity.service'
import { EntityPropertyControlService } from '../../services/entity-property-control.service'
import { ConfirmDialogComponent } from 'src/app/core/dialogs/confirm-dialog/confirm-dialog.component'
import { environment } from 'src/environments/environment'
import { ErrorReporterDialogComponent } from 'src/app/core/dialogs/error-reporter-dialog/error-reporter-dialog.component'
import { MapperFactoryService } from '../../services/mapper-factory.service'
import { EntityMetadataService } from 'src/app/core/services/entity-metadata.service'
import { EntityHelperService } from '../../services/entity-helper.service'
import { Router } from '@angular/router'
import { EntityMetadata } from 'src/app/core/models/concretes/entity-metadata'
import { StringEx } from 'src/app/core/infrastructure/extensions/string-ex'

// import { EntityDataService } from 'app/eaf/entity-data.service'
// import { MapperFactoryService } from '../mapper-factory.service'

@Component({
  selector: 'app-entity-mapper',
  templateUrl: './entity-mapper.component.html',
  styleUrls: ['./entity-mapper.component.scss'],
  providers: [ArraySortOrderPipe]
})
export class EntityMapperComponent implements OnInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator
  @ViewChild(MatSort) matSort!: MatSort

  field!: FieldConfig
  group!: FormGroup

  top = 10
  skip = 0
  tableData: any = new MatTableDataSource([])
  displayedColumns = ['select', 'Id']
  entityColumns = ['Id']
  pageIndex = 0
  selection = new SelectionModel<number>(true, [])
  displayProgress = false
  entityProperties!: any[]
  targetEntityMetaData!: EntityMetadata
  mapEntityMetaData!: EntityMetadata

  get count() {
    return this.tableData.data.length
  }

  get pageSizeOptions() {
    return environment.pageSizeOptions
  }

  targetEntity!: string
  mapEntity!: string

  constructor(private entityService: EntityService,
              private entityMetadataService: EntityMetadataService,
              private entityDataService: EntityHelperService,
              private epcs: EntityPropertyControlService,
              private mapperFactory: MapperFactoryService,
              private matDialog: MatDialog,
              public snackBar: MatSnackBar,
              public errorReporter: ErrorReporterDialogComponent,
              private arraySortByOrder: ArraySortOrderPipe,
              private router: Router) { }

  ngOnInit() {
    this.loadTable()
  }

  loadTable(): any {
    this.displayProgress = true

    this.targetEntity = this.getTarget(this.field?.targetEntity, this.field?.alias)
    this.mapEntity = this.field.entityAlias === undefined ? this.field?.mapEntity ?? '' : this.getTarget(this.field?.targetEntity, this.field?.alias)

    const mapEntityFilter: any = this.field.entityAlias !== undefined ? `${this.field.entityAlias}Id eq ${this.field.value}` : undefined

    this.entityService.getFilteredMapList(this.field?.baseEntity ?? '',
                                          this.mapEntity,
                                          this.targetEntity,
                                          mapEntityFilter,
                                          this.field.value,
                                          this.top,
                                          this.skip).subscribe(results => {

      this.processMetadata()

      const data = this.formatData(results)
      this.tableData = new MatTableDataSource(data)
      setTimeout(() => {
        this.tableData.paginator = this.paginator
        this.tableData.sort = this.matSort
      },         100)
      this.displayProgress = false
    },
                                     error => {
      this.errorReporter.displayMessage(error.error)
      this.displayProgress = false
    })
  }

  processMetadata() {

    const targetEntityMD = this.entityMetadataService.getEntityMetaData(this.field.targetEntity ?? '')
    const mapEntityMD = this.entityMetadataService.getEntityMetaData(this.field.mapEntity ?? '')

    this.targetEntityMetaData = this.entityMetadataService.getEntityFromMetaData({key: this.field.targetEntity, value: targetEntityMD.value})
    this.mapEntityMetaData = this.entityMetadataService.getEntityFromMetaData({key: this.field.mapEntity, value: mapEntityMD.value})

    let columns = this.targetEntityMetaData.Fields.filter(x => x.isString()).map((field) => ({name: field.Name, order: field.DisplayOrder}))
    const mColumns = this.mapEntityMetaData.Fields.filter(x => x.VisibleOnRelations)
                                                  .map((field) => ({name: field.Name, order: field.DisplayOrder}))


    this.arraySortByOrder.transform(columns)
    this.arraySortByOrder.transform(mColumns)
    columns = columns.concat(mColumns)
    this.entityColumns = this.entityColumns.concat(columns.map(x => x.name))
    this.displayedColumns = this.displayedColumns.concat(columns.map(x => x.name))
  }

  formatData(data: any) {
    const returnArray: any = []
    let retObject = {}

    const firstRelatedEntity = this.mapEntity
    const secondRelatedEntity = this.targetEntity

    if (data.RelatedEntityCollection) {
      data.RelatedEntityCollection.find((x: any) => x.RelatedEntity === firstRelatedEntity)
        .RelatedEntities.forEach((item: any) => {
          retObject = {}
          if (this.field.entityAlias !== undefined) {
            this.formatEntityData(item, retObject)
            if (item.RelatedEntityCollection) {
              item.RelatedEntityCollection.find((x: any) => x.RelatedEntity === secondRelatedEntity)
              .RelatedEntities.forEach((mappedItem: any) => {
                    this.formatMappedEntityData(mappedItem, retObject)
              })
            }
          } else {
            if (item.RelatedEntityCollection) {
              item.RelatedEntityCollection.find((x: any) => x.RelatedEntity === secondRelatedEntity)
              .RelatedEntities.forEach((mappedItem: any) => {
                this.formatEntityData(mappedItem, retObject)
              })
            }
            this.formatMappedEntityData(item, retObject)
          }
          returnArray.push(retObject)
      })
    }
    return returnArray
  }

  formatEntityData(item: any, retObject: any) {
    this.entityColumns.forEach(element => {
      retObject[element] = item.Object[element]
    })
  }

  formatMappedEntityData(item: any, retObject: any) {
    this.entityColumns.forEach(element => {
      if (retObject[element] === undefined) {

        if (this.mapEntityMetaData.hasField(element)) {
          const metadataField = this.mapEntityMetaData.getField(element)

          if (metadataField.isDate()) {
            retObject[element] = new Date(item.Object[element])
          } else if (metadataField.isEnum()) {
            retObject[element] = metadataField.Options.get(item.Object[element])
          } else {
            retObject[element] = item.Object[element]
          }
        } else {
          retObject[element] = ''
        }
      }
    })
  }

  checkIfMappedDataNeeded(object: any): boolean {
    let mappedDataNeededCount = 0
    this.entityColumns.forEach(column => {
      if (object[column] === undefined) {
        mappedDataNeededCount = + 1
      }
    })
    if (mappedDataNeededCount > 0) {
      return true
    }
    return false
  }

  getCount(results: any) {
    if (results.RelatedEntityCollection) {
      if (results.RelatedEntityCollection.find((x: any) => x.RelatedEntity === this.targetEntity)) {
        return results.RelatedEntityCollection.find((x: any) => x.RelatedEntity === this.targetEntity).Count
      }
    }
    return 0
  }

  rowClick(row: any) {
    this.router.navigate([`/admin/data-administration/${this.field.targetEntity}/${row.Id}`])
  }

  createNew() {
    const dialog = this.mapperFactory.getMapperDialog(this.field.label)
    this.matDialog.open(dialog, {
      width: '800px',
      data: { MappingEntityName: this.field.mapEntity,
              FromEntityName: this.field.baseEntity,
              ToEntityName: this.field.targetEntity,
              Id: this.field.value,
              Alias: this.field.alias,
              default: this.mapEntityMetaData.getField(this.targetEntity).Default
        }
    }).afterClosed().subscribe((response) => {
      if (response === 'Added') {
        this.refreshTable()
      } else {
        if (response !== undefined && response.error !== undefined) {
          this.errorReporter.displayMessage(response.error)
        }
      }
    })

    return
  }

  deleteEntities() {
    if (this.selection.selected.length === 0) { return }
    const entName = this.selection.selected.length === 1 ?
                    this.field.mapEntity :
                    this.selection.selected.length + ' ' + this.field.mapEntity
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
    this.displayProgress = true
    const getMapCalls: any = []

    entityIds.forEach(relatedEntityId => {
      const entity = this.field.baseEntity
      const entityAlias = this.field.entityAlias !== undefined ? this.field.entityAlias : this.field.baseEntity
      const relatedEntity = this.field.targetEntity
      const relatedEntityAlias = this.field.alias
      const mappingEntity = this.field.mapEntity

      const leftFilter = `${this.epcs.getMappedReferentialConstraint(mappingEntity, entity, entityAlias)} eq ${this.field.value} and `
      const rightFilter = `${this.epcs.getMappedReferentialConstraint(mappingEntity,
                                                                      this.getTarget(relatedEntity ?? '', relatedEntityAlias ?? ''),
                                                                      relatedEntityAlias)} eq ${relatedEntityId}`

      const filter = `${leftFilter} ${rightFilter}`
      const filteredEntityList = this.entityService.getFilteredEntityList(this.field.mapEntity ?? '', filter, 1, 0)

      getMapCalls.push(filteredEntityList)
    })

    forkJoin(getMapCalls).subscribe((results: any) => {
      if (results.length > 1) {
        const ids: any = []
        results.forEach((result: any) => {
          ids.push(result.Entities[0].Id)
        })

        this.deleteMultipleRecords(ids)

      } else {
        // Individual call returns
        this.deleteRecord(results[0].Entities[0].Id)
      }
    },                              error => {
      this.displayProgress = false
      this.snackBar.open(`There was an issue deleting the selected ${this.field.label}`,
                         <any>null, { duration: environment.snackBarDuration })
    },                              () => {
      this.displayProgress = false
      this.snackBar.open(`The ${this.field.label} were successfully deleted`,
                         <any>null, { duration: environment.snackBarDuration }).afterDismissed().subscribe(() => {
         this.selection.clear()
         this.refreshTable()
       })
    })
  }

  deleteRecord(id: number) {
    this.entityService.deleteEntity(this.field.mapEntity ?? '', id).subscribe(
      (response) => {

      },
      (error) => {
        this.errorReporter.displayMessage(error.error)
      }
    )
  }

  deleteMultipleRecords(ids: any[]) {
    this.entityService.bulkDeleteEntities(this.field.mapEntity ?? '', ids).subscribe(
      (response) => {

      },
      (error) => {
        this.errorReporter.displayMessage(error.error)
      }
    )
  }

  refreshTable() {
    this.displayProgress = true
    this.top = 10
    this.skip = 0
    const mapEntityFilter: any = this.field.entityAlias !== undefined ? `${this.field.entityAlias}Id eq ${this.field.value}` : undefined

    this.entityService.getFilteredMapList(this.field.baseEntity ?? '',
                                          this.mapEntity,
                                          this.targetEntity,
                                          mapEntityFilter,
                                          this.field.value,
                                          this.top,
                                          this.skip).subscribe(
        (results) => {
          this.tableData.data = this.formatData(results)
          setTimeout(() => {
              this.tableData.paginator = this.paginator
              this.tableData.sort = this.matSort
            },       100)
          this.displayProgress = false
        },
        (error) => {
          this.errorReporter.displayMessage(error.error)
          this.displayProgress = false
         }
      )
  }

  getTarget(target: string | undefined, alias: string | undefined): string {
    if (!StringEx.isUndefinedNullOrWhitespace(alias)) {
      return alias
    }
    if (!StringEx.isUndefinedNullOrWhitespace(target)) {
      return target
    }
    throw new Error('The target or alias must be configured')
  }
}

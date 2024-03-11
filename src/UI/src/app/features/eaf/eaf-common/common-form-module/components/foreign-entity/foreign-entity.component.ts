import { Component, OnInit, Input, ViewChild } from '@angular/core'
import { SelectionModel } from '@angular/cdk/collections'
import { MatDialog } from '@angular/material/dialog'
import { MatPaginator } from '@angular/material/paginator'
import { MatSnackBar } from '@angular/material/snack-bar'
import { MatSort } from '@angular/material/sort'
import { MatTableDataSource } from '@angular/material/table'

import { ArraySortOrderPipe } from 'src/app/core/pipes/array-sort-order.pipe'
import { SpaceTitlePipe } from 'src/app/core/pipes/spacetitle.pipe'
import { EntityField } from '../../models/concretes/entity-field'
import { FieldConfig } from '../../models/interfaces/field-config.interface'
import { EntityService } from 'src/app/core/services/entity.service'
import { EntityPropertyControlService } from '../../services/entity-property-control.service'
import { environment } from 'src/environments/environment'
import { EntityFormDialogComponent } from '../../../common-dialogs-module/components/entity-form-dialog/entity-form-dialog.component'
import { ConfirmDialogComponent } from 'src/app/core/dialogs/confirm-dialog/confirm-dialog.component'
import { EntityMetadataService } from 'src/app/core/services/entity-metadata.service'
import { Router } from '@angular/router'
import { DatePipe } from '@angular/common'
import { EntityConfigBuilder } from '../../services/entity-config-builder'

@Component({
  selector: 'app-foreign-entity',
  templateUrl: './foreign-entity.component.html',
  styleUrls: ['./foreign-entity.component.scss'],
  providers: [ArraySortOrderPipe, SpaceTitlePipe]
})
export class ForeignEntityComponent implements OnInit {

  @Input() field!: EntityField
  @Input() parentEntityId!: number
  @Input() relatedEntityProperty!: string | null
  @Input() parentEntityName!: string

  @ViewChild(MatPaginator) paginator!: MatPaginator
  @ViewChild(MatSort) matSort!: MatSort

  top = 10
  skip = 0
  tableData: any = new MatTableDataSource([])
  displayedColumns = ['select']
  entityColumns: any = []
  pageIndex = 0
  selection = new SelectionModel<number>(true, [])
  displayProgress = false

  /** The name of the entity that has been stripped of .self */
  entityName!: string

  /** A friendly spaced entity name for showing to the user */
  private spacedEntityName!: string

  private entityConfig!: FieldConfig[]

  get count() {
    return this.tableData.data.length
  }

  get pageSizeOptions() {
    return environment.pageSizeOptions
  }

  constructor(private entityService: EntityService,
              private entityMetadataService: EntityMetadataService,
              private spaceTitlePipe: SpaceTitlePipe,
              private datePipe: DatePipe,
              private arraySortByOrder: ArraySortOrderPipe,
              private entityPropertyControlService: EntityPropertyControlService,
              private matSnackBar: MatSnackBar,
              private matDialog: MatDialog,
              private router: Router,
              private entityConfigBuilder: EntityConfigBuilder) { }

  ngOnInit() {
    this.initialiseComponent()
  }

  initialiseComponent() {
    this.entityName = this.field.Type.substring(5, this.field.Type.length)
    this.spacedEntityName = this.spaceTitlePipe.transform(this.entityName)

    this.displayProgress = true

    // Get the configuration for the entity
    this.getEntityConfiguration()

    // Now configure the table
    this.getTableConfiguration()

    // Now get the data for the table
    this.refreshTable()
  }

  /** Gets the entity configuration so we can pass it to the dialog component when a user wants to create a new Foreign row */
  getEntityConfiguration() {
    this.entityConfig = this.getEntityFieldConfigs(this.entityName)
    // We want to overwrite these as otherwise the don't take up enough room and it looks weird
    this.entityConfig.forEach(x => {
      x.flex = 100
    })

    // Sort the fields by their order
    this.entityConfig.orderBy(x => x.order)
  }

  /**
  * Get the entity field configuration which includes metadata for the form to be created
  * @param entityName The name of the entity
  */
  getEntityFieldConfigs(entityName: string): FieldConfig[] {
   const fieldConfigs: FieldConfig[] = []

   // Create the metadata for fields to be injected into the dialog to the app-entity-form
   const md = JSON.parse(localStorage.getItem(environment.metaDataLocalName) ?? '')
   const auditableProperties = ['CreateDate', 'CreatedBy', 'LastUpdated', 'LastUpdatedBy', 'Id']

   const entity = md.find((x: any) => x.key === entityName)
   const entityMeta = this.entityMetadataService.getEntityFromMetaData(entity)

   const filteredFields = entityMeta.Fields.filter(field => auditableProperties.indexOf(field.Name) === -1)
   filteredFields.forEach(field => {
     if (!field.isNavigationProperty()) {
       const navField = field.hasNavigationKey() ? entityMeta.getField(field.NavigationKey) : undefined
       const fieldConfig = this.entityConfigBuilder.build(entityName, field, null, navField)
       if (fieldConfig.name === this.relatedEntityProperty) {
         fieldConfig.value = this.parentEntityId
       }
       fieldConfigs.push(fieldConfig)
     }
   })

   return fieldConfigs
 }

  /** Setup the table with the necessary columns and data */
  getTableConfiguration() {
    const md = JSON.parse(localStorage.getItem(environment.metaDataLocalName) ?? '')
    const meta = md.find((x: any) => x.key === this.entityName)

    // Get all the keys in the meta.value so
    // I can order them as defined by the meta data
    const keys = Object.keys(meta.value)
    const columns = keys.select<any>((key: any) => {
      const data = meta.value[key]
      // tslint:disable-next-line: max-line-length
      if (data !== null && typeof data === 'object' && !Array.isArray(data) && data['$Kind'] !== 'NavigationProperty'
       && data['@UI.ReadOnly'] !== true && key !== '@UI.DisplayName') {
        const order = meta.value[key]['@UI.DisplayOrder'] ? meta.value[key]['@UI.DisplayOrder'] : 0
        return {name: key, order: order}
      }
      return
    })

    this.entityColumns = this.entityColumns.concat(this.arraySortByOrder.transform(columns).map((x) => x.name))
    this.displayedColumns = this.displayedColumns.concat(columns.map(x => x.name))
  }

  getForeignKeyProperty(): String {
    let foreignKeyProperty = this.field.MappedId
    if (!foreignKeyProperty) {
      foreignKeyProperty = this.parentEntityName + 'Id'
    }
    return foreignKeyProperty
  }

  /** Gets all of the data for this entity and populates the table */
  refreshTable() {
    this.displayProgress = true
    const foreignKeyProperty = this.getForeignKeyProperty()
    this.entityService.getFilteredEntityList(this.entityName, `${foreignKeyProperty} eq ${this.parentEntityId}`)
      .subscribe((results) => {
        if (results && results.Entities) {
          const data = results.Entities.select<any>(element => element.Object)
          this.tableData = new MatTableDataSource(this.formatEntities(data))
        } else {
          this.tableData = new MatTableDataSource([])
        }
          // Need to give the UI a second to update so it knows that data exists before setting the paginator
        setTimeout(() => {
            this.tableData.paginator = this.paginator
            this.tableData.sort = this.matSort
          }, 100 )
      },
      (error: any) => {
        this.matSnackBar.open(`There was an error getting the Foreign entity ${this.spacedEntityName}: ${error}`,
                              <any>null,
                              { duration: 1000 })
      },
      () => {
        this.displayProgress = false
      })
  }

  formatEntities(data: any[]) {
    const formattedData: any = []
    if (data !== undefined) {
      data.forEach(entity => {
        Object.keys(entity).forEach((prop, value) => {
          if (typeof entity[prop] === 'string' && (prop.indexOf('Date') >= 0 || prop.indexOf('Updated') >= 0)) {
            entity[prop] = new Date(entity[prop])
          }
        })

        formattedData.push(entity)
      })
    }
    return formattedData
  }

  formatCell(input: any) {
    if (typeof input !== typeof new Date() ) {
      return input
    }
    return this.datePipe.transform(input, 'MMM dd, yyyy')
  }

  rowClick(row: any) {
    this.router.navigate([`admin/data-administration/${this.entityName}/${row.Id}`])
  }

  /** Create a new Entity. Passes this on to the MatDialog */
  createNew() {
    const data = Object.assign([], this.entityConfig)

    // push an 'add' button to the array so the user can submit the form
    /*entityConfigCopy.push({
      'type': 'button',
      'label': 'Add',
      'flex': 60,
      'entity': this.entityName,
      'order': 99
    })*/
    const entityDialog = this.matDialog.open(EntityFormDialogComponent, {
        width: '800px',
        data: {
          entityConfig: data,
          title: `Add ${this.entityName}`
        }
      }
    )

    entityDialog.afterClosed().subscribe(result => {
      if (result) {
        this.displayProgress = true
        this.entityService.addEntity(this.entityName, [result])
          .subscribe((success) => {
            this.matSnackBar.open(`The ${this.spacedEntityName} was created`, <any>null, { duration: 1000 })
            this.refreshTable()
          },
                     (error) => {
              this.displayProgress = false
              this.matSnackBar.open(`Error creating the ${this.spacedEntityName}: ${error.error.Message}`, <any>null, { duration: 5000 })
            },
                     () => {
              this.displayProgress = false
            })
      } else {
        this.matSnackBar.open(`User cancelled creating the ${this.spacedEntityName}`, <any>null, { duration: 1000 })
      }
    })
  }

  /** Delete the selected entities */
  deleteEntities() {
    this.displayProgress = true
    if (this.selection.selected.length === 0) { return }

    const message = `Are you sure you wish to delete the selected ${this.spacedEntityName}?`
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

  /** Take an array of selected ids and send them to the entityService one by one to delete them */
  processDeletes(selected: number[]) {
    selected.forEach(item => {
      this.entityService.deleteEntity(this.entityName, item)
      .subscribe(() => {
        // reset the selection on success or the page will think there is still a selection and
        // show the delete button
        this.selection.deselect(item)
        this.matSnackBar.open(`The selected ${this.spacedEntityName} were deleted`, <any>null, { duration: 1000 })
        },
        (error) => {
          this.matSnackBar.open(`Error deleting the selected ${this.spacedEntityName}: ${error}`, <any>null, { duration: 1000 })
        },
        () => {
          this.selection = new SelectionModel<number>(true, [])
          this.refreshTable()
      })
    })
  }
}

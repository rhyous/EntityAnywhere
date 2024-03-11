import { Component, Input, OnInit, ViewChild } from '@angular/core'
import { MatDialog } from '@angular/material/dialog'
import { MatPaginator } from '@angular/material/paginator'
import { MatSnackBar } from '@angular/material/snack-bar'
import { MatSort } from '@angular/material/sort'
import { MatTableDataSource } from '@angular/material/table'
import { SelectionModel } from '@angular/cdk/collections'

import { ExtensionEntity } from '../../models/interfaces/extension-entity'
// tslint:disable-next-line: max-line-length
import { ExtensionEntityDialogComponentData } from '../../../common-dialogs-module/components/extension-entity-dialog/extension-entity-dialog-component-data'
// tslint:disable-next-line: max-line-length
import { ExtensionEntityDialogComponent } from '../../../common-dialogs-module/components/extension-entity-dialog/extension-entity-dialog.component'
import { ConfirmDialogComponent } from 'src/app/core/dialogs/confirm-dialog/confirm-dialog.component'
import { EntityService } from 'src/app/core/services/entity.service'
import { Router } from '@angular/router'
import { SingularizePipe } from 'src/app/core/pipes/singularize.pipe'
import { environment } from 'src/environments/environment'

@Component({
  selector: 'app-extension-entity',
  templateUrl: './extension-entity.component.html',
  styleUrls: ['./extension-entity.component.scss']
})
export class ExtensionEntityComponent implements OnInit {
  @Input() extensionEntityData!: ExtensionEntity
  @Input() parentEntityId!: number
  @Input() parentEntityName!: string

  @ViewChild(MatPaginator) paginator!: MatPaginator
  @ViewChild(MatSort) matSort!: MatSort

  extensionEntityName!: string
  tableData: any = new MatTableDataSource([])
  displayProgress = false
  entityColumns = ['Id', 'Property', 'Value']
  displayedColumns = ['select', 'Id', 'Property', 'Value']
  selection = new SelectionModel<number>(true, [])
  top = 100
  skip = 0

  get count() {
    return this.tableData.data.length
  }

  get pageSizeOptions() {
    return environment.pageSizeOptions
  }

  get isReadOnlyMode() {
    return this.extensionEntityData.ReadOnly
  }

  constructor(private entityService: EntityService,
              private snackBar: MatSnackBar,
              private matDialog: MatDialog,
              private router: Router,
              private sigularizePipe: SingularizePipe) { }

  ngOnInit() {
    this.extensionEntityName = this.sigularizePipe.transform(this.extensionEntityData.Name)
    this.loadTable()
  }

  loadTable() {
    this.displayProgress = true
    this.entityService.getExpandedFilteredEntityList(this.parentEntityName,
      `Id eq ${this.parentEntityId}`, [this.extensionEntityName], this.top, this.skip)
      .subscribe((success) => {
        this.tableData = new MatTableDataSource([])
        if (success.Entities && success.Entities[0].RelatedEntityCollection) {
          const extensionEntities: any = success.Entities[0].RelatedEntityCollection[0].RelatedEntities
          extensionEntities.forEach((element: any) => {
              this.tableData.data.push(
                { Id: element.Id,
                  Property: element.Object.Property,
                  Value: element.Object.Value
                })
            })

          setTimeout(() => {
            this.tableData.paginator = this.paginator
            this.tableData.sort = this.matSort
          },
                     100)
        }
      },         (error) => {
        this.snackBar.open(error)
      },
                 () => {
        this.displayProgress = false
        })
  }

  createNew() {
    const data: ExtensionEntityDialogComponentData = {
      extensionEntityData: this.extensionEntityData,
      parentEntityId: this.parentEntityId,
      parentEntityName: this.parentEntityName
    }

    const confirmDialog = this.matDialog.open(ExtensionEntityDialogComponent, {
      width: '800px',
      data: data
    })
    confirmDialog.afterClosed().subscribe((response) => {
      this.loadTable()
    },
                                          (error) => this.snackBar.open(error))

  }

  rowClick(row: any) {
    const name = this.sigularizePipe.transform(this.extensionEntityData.Name)
    this.router.navigate([`admin/data-administration/${name}/${row.Id}`])
   }

  deleteEntities() {
    if (this.selection.selected.length === 0) { return }
    const message = `Are you sure you wish to delete the selected ${this.extensionEntityData.Name}`
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
      this.displayProgress = true
      this.selection.selected.forEach(x => {
        this.entityService.deleteEntity(this.extensionEntityData.Type.substring(5), x)
        .subscribe((response) => {
          // reset the selection on success or the page will think there is still a selection and
          // show the delete button
          this.selection.deselect(x)
          this.snackBar.open(`The selected ${this.extensionEntityData.Name} were deleted`, <any>null, { duration: 1000 })
        },
                   (error) => {
          this.snackBar.open(error, <any>null, { duration: 1000})
        },
                   () => {
          this.loadTable()
        })
      })
    })
  }
}

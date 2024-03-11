import { Component, OnInit, Inject, AfterViewInit } from '@angular/core'
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog'
import { MatTableDataSource } from '@angular/material/table'
import { FormBuilder, FormGroup } from '@angular/forms'
import { Subject } from 'rxjs'

import { ArraySortOrderPipe } from 'src/app/core/pipes/array-sort-order.pipe'
import { EntityService } from 'src/app/core/services/entity.service'
import { EntityMetadataService } from 'src/app/core/services/entity-metadata.service'
import { EntityPropertyControlService } from '../../../common-form-module/services/entity-property-control.service'
import { ConfirmDialogComponent } from 'src/app/core/dialogs/confirm-dialog/confirm-dialog.component'
import { ErrorReporterDialogComponent } from 'src/app/core/dialogs/error-reporter-dialog/error-reporter-dialog.component'
import { CommonDialogBaseComponent } from '../common-dialog-base.component'
import { SelectionModel } from '@angular/cdk/collections'
import { PluralizePipe } from 'src/app/core/pipes/pluralize.pipe'
import { PageRowSelection } from '../../interfaces/page-row-selection.interface'


@Component({
  selector: 'app-mapper-dialog',
  templateUrl: './mapper-dialog.component.html',
  styleUrls: ['./mapper-dialog.component.scss'],
  providers: [ArraySortOrderPipe, EntityPropertyControlService]
})

export class MapperDialogComponent extends CommonDialogBaseComponent implements OnInit {

  override form!: FormGroup
  override datasource = new MatTableDataSource()
  override subject: Subject<string> = new Subject()
  override displayedColumns = ['select']

  selection = new SelectionModel<number>(true, [])
  selectedOnThisPage: PageRowSelection[] = []

  // data expects the following information
  // 1. MappingEntityName - the link table
  // 2. FromEntityName - the name of the entity that the dialog has been accessed from
  // 3. ToEntityName - the name of the entity that the dialog needs to list to select from
  // 4. Id - the id of the from entity record that the dialog has been accessed from
  // 5. default - the default value for all entities

  constructor(public override dialogRef: MatDialogRef<MapperDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    formBuilder: FormBuilder,
    public matDialog: MatDialog,
    private pluralizePipe: PluralizePipe,
    entityService: EntityService,
    entityMetadataService: EntityMetadataService,
    private epcs: EntityPropertyControlService,
    errorReporterDialog: ErrorReporterDialogComponent,
    arraySortByOrder: ArraySortOrderPipe) {
      super(dialogRef, formBuilder, entityMetadataService, entityService, errorReporterDialog, arraySortByOrder, data.ToEntityName, data.default)
  }

  override ngOnInit() {
    super.ngOnInit()
  }

  rowClick(row: any) {
    // Confirm
    this.confirmAdd([row.Id])
  }

  addMultipleRecords() {
    if (this.selection.selected.length === 0) { return }
    // Confirm
    this.confirmAdd(this.selection.selected)
  }

  confirmAdd(ids: any[]) {
    // Confirm
    const entityName = ids.length > 1 ? this.pluralizePipe.transform(this.data.ToEntityName) : this.data.ToEntityName
    const message = `Are you sure you wish to add the selected ${entityName} to the ${this.data.FromEntityName}?`
    const title = 'Confirm Mapping'
    const confirmationDialog = this.matDialog.open(ConfirmDialogComponent,
    {
      width: '400px',
      data : {
        title: title,
        message: message
      }
    })

    confirmationDialog.afterClosed().subscribe(result => {
      // Add the mapping record
      if (result !== 'confirmed') { return }
      this.addRecords(ids)
    })
  }

  addRecords(ids: any[]) {
    if (ids.length === 0) { return }
    this.displayProgress = true
    const addEntities: any = []
    ids.forEach(selectedEntityId  => {
      const addObject: any = {}
      addObject[this.epcs.getMappedReferentialConstraint(this.data.MappingEntityName, this.data.FromEntityName, undefined)] = this.data.Id
      addObject[this.epcs.getMappedReferentialConstraint(this.data.MappingEntityName, this.data.ToEntityName, undefined)] = selectedEntityId
      addEntities.push(addObject)
    })
    this.entityService.addEntity(this.data.MappingEntityName, addEntities).subscribe(
      (response) => {
        this.displayProgress = false
        this.dialogRef.close('Added')
      },
      (error) => {
        this.errorReporterDialog.displayMessage(error.error)
        this.displayProgress = false
      }
    )
  }

  rowToggle(rowId: number) {
    const pageRowSelector: PageRowSelection = {PageIndex: this.pageIndex, RowId: rowId }
    this.selection.toggle(rowId)

    if (this.isRowSelected(rowId)) {
      this.selectedOnThisPage.push(pageRowSelector)
    } else {
      this.selectedOnThisPage = this.selectedOnThisPage.where( i => i.RowId !== rowId )
    }
  }

  isRowSelected(rowId: number): boolean {
    return this.selection.isSelected(rowId)
  }

  masterToggle() {
    if (this.isAllSelected()) {
      this.selectedOnThisPage = this.selectedOnThisPage.where( i => i.PageIndex !== this.pageIndex)
      this.datasource.data.forEach((row: any) => {
        this.selection.deselect(row.Id)
      })
    } else {
      this.selectedOnThisPage = this.selectedOnThisPage.where( i => i.PageIndex !== this.pageIndex)
      this.datasource.data.forEach((row: any) => {
        const pageRowSelector: PageRowSelection = {PageIndex: this.pageIndex, RowId: row.Id}
        this.selection.select(row.Id)
        this.selectedOnThisPage.push(pageRowSelector)
      })
    }
  }

  isAllSelected() {
    const numSelected = this.selectedOnThisPage.select( i => i.PageIndex === this.pageIndex).length
    const numRows = this.datasource.data.length
    return numSelected === numRows
  }
}

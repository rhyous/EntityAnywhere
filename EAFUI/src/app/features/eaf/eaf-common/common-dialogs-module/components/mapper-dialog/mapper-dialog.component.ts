import { Component, OnInit, Inject } from '@angular/core'
import { MAT_DIALOG_DATA, MatDialogRef, MatTableDataSource, MatDialog } from '@angular/material'
import { FormBuilder, FormGroup } from '@angular/forms'
import { Subject } from 'rxjs'

import { ArraySortOrderPipe } from 'src/app/core/pipes/array-sort-order.pipe'
import { EntityService } from 'src/app/core/services/entity.service'
import { EntityMetadataService } from 'src/app/core/services/entity-metadata.service'
import { EntityPropertyControlService } from '../../../common-form-module/services/entity-property-control.service'
import { ConfirmDialogComponent } from 'src/app/core/dialogs/confirm-dialog/confirm-dialog.component'
import { ErrorReporterDialogComponent } from 'src/app/core/dialogs/error-reporter-dialog/error-reporter-dialog.component'
import { CommonDialogBaseComponent } from '../common-dialog-base.component'

@Component({
  selector: 'app-mapper-dialog',
  templateUrl: './mapper-dialog.component.html',
  styleUrls: ['./mapper-dialog.component.scss'],
  providers: [ArraySortOrderPipe, EntityPropertyControlService]
})
export class MapperDialogComponent extends CommonDialogBaseComponent implements OnInit {

  form: FormGroup
  datasource = new MatTableDataSource()
  subject: Subject<string> = new Subject()

  // data expects the following information
  // 1. MappingEntityName - the link table
  // 2. FromEntityName - the name of the entity that the dialog has been accessed from
  // 3. ToEntityName - the name of the entity that the dialog needs to list to select from
  // 4. Id - the id of the from entity record that the dialog has been accessed from
  // 5. default - the default value for all entities

  constructor(public dialogRef: MatDialogRef<MapperDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    formBuilder: FormBuilder,
    public matDialog: MatDialog,
    entityService: EntityService,
    entityMetadataService: EntityMetadataService,
    private epcs: EntityPropertyControlService,
    errorReporterDialog: ErrorReporterDialogComponent,
    arraySortByOrder: ArraySortOrderPipe) {
      super(dialogRef, formBuilder, entityMetadataService, entityService, errorReporterDialog, arraySortByOrder, data.ToEntityName, data.default)
  }

  ngOnInit() {
    super.ngOnInit()
  }

  rowClick(row) {
    // Confirm
    const message = `Are you sure you wish to add the selected ${this.data.ToEntityName} to the ${this.data.FromEntityName}?`
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
      if (result === 'confirmed') {
        this.addRecord(row)
      }
    })
  }

  addRecord(row) {
    this.displayProgress = true
    const addObject = {}
    addObject[this.epcs.getMappedReferentialConstraint(this.data.MappingEntityName, this.data.FromEntityName, undefined)] = this.data.Id
    addObject[this.epcs.getMappedReferentialConstraint(this.data.MappingEntityName, this.data.ToEntityName, undefined)] = row.Id

    this.entityService.addEntity(this.data.MappingEntityName, [addObject]).subscribe(
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
}

import { Component, OnInit, Inject } from '@angular/core'
import { FormGroup, FormBuilder } from '@angular/forms'
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'
import { MatTableDataSource } from '@angular/material/table'
import { Subject } from 'rxjs'

import { ArraySortOrderPipe } from 'src/app/core/pipes/array-sort-order.pipe'
import { EntityService } from 'src/app/core/services/entity.service'
import { EntityPropertyControlService } from '../../../common-form-module/services/entity-property-control.service'
import { ErrorReporterDialogComponent } from 'src/app/core/dialogs/error-reporter-dialog/error-reporter-dialog.component'
import { EntityMetadataService } from 'src/app/core/services/entity-metadata.service'
import { CommonDialogBaseComponent } from '../common-dialog-base.component'



@Component({
  selector: 'app-entity-searcher-dialog',
  templateUrl: './entity-searcher-dialog.component.html',
  styleUrls: ['./entity-searcher-dialog.component.scss'],
  providers: [ArraySortOrderPipe, EntityPropertyControlService]
})

/**
 * Entity Searcher Dialog Component. Responsible for searching for an Entity and displaying the Dialog
 */
export class EntitySearcherDialogComponent extends CommonDialogBaseComponent implements OnInit {

  override form!: FormGroup
  override datasource = new MatTableDataSource()
  override subject: Subject<string> = new Subject()

  // data expects the following information
  // 1. entityName - the name of the entity that the dialog needs to list to select from
  // 2. default - the default value for all entities

  constructor(public override dialogRef: MatDialogRef<EntitySearcherDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    formBuilder: FormBuilder,
    entityService: EntityService,
    entityMetadataService: EntityMetadataService,
    errorReporterDialog: ErrorReporterDialogComponent,
    epcs: EntityPropertyControlService,
    arraySortByOrder: ArraySortOrderPipe) {
      super(dialogRef, formBuilder, entityMetadataService, entityService, errorReporterDialog, arraySortByOrder, data.entityName, data.default)
  }

  override ngOnInit() {
    super.ngOnInit()
    this.onFilterChanged()
  }

  rowClick(row: any) {
    const displayName: any = (<any>this.metaData).UIDisplayName['$PropertyPath']
      ? (<any>this.metaData).UIDisplayName['$PropertyPath']
      : this.metaData.UIDisplayName

    this.dialogRef.close({Id: row.Object.Id, Name: row.Object[displayName]})
  }

}

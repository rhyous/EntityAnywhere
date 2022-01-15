import { Component, OnInit, Inject } from '@angular/core'
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material'
import { FieldConfig } from '../../../common-form-module/models/interfaces/field-config.interface'


@Component({
  selector: 'app-entity-form-dialog',
  templateUrl: './entity-form-dialog.component.html',
  styleUrls: ['./entity-form-dialog.component.scss']
})
export class EntityFormDialogComponent implements OnInit {

  entityConfig: FieldConfig[]
  title: string

  constructor(public dialogRef: MatDialogRef<EntityFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
     }

  ngOnInit() {
    this.entityConfig = this.data.entityConfig
    this.title = this.data.title
  }

  submit(event) {
    this.dialogRef.close(event)
  }

}

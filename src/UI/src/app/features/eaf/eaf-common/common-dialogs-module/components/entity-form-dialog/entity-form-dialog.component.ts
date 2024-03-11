import { Component, OnInit, Inject } from '@angular/core'
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'
import { FieldConfig } from '../../../common-form-module/models/interfaces/field-config.interface'


@Component({
  selector: 'app-entity-form-dialog',
  templateUrl: './entity-form-dialog.component.html',
  styleUrls: ['./entity-form-dialog.component.scss']
})
export class EntityFormDialogComponent implements OnInit {

  fieldConfigs!: FieldConfig[]
  title!: string

  constructor(public dialogRef: MatDialogRef<EntityFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
     }

  ngOnInit() {
    this.fieldConfigs = this.data.entityConfig
    this.title = this.data.title
  }

  submit(event: any) {
    this.dialogRef.close(event)
  }

}

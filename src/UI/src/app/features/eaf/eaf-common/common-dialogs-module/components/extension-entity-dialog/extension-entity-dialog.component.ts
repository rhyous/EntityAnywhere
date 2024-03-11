import { Component, OnInit, Inject } from '@angular/core'
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'
import { MatSnackBar } from '@angular/material/snack-bar'
import { Validators } from '@angular/forms'

import { EntityService } from 'src/app/core/services/entity.service'
import { FieldConfig } from '../../../common-form-module/models/interfaces/field-config.interface'
import { ExtensionEntityDialogComponentData } from './extension-entity-dialog-component-data'


@Component({
  selector: 'app-extension-entity-dialog',
  templateUrl: './extension-entity-dialog.component.html',
  styleUrls: ['./extension-entity-dialog.component.scss']
})
export class ExtensionEntityDialogComponent implements OnInit {
  showProgressBar = false
  entityConfig: FieldConfig[] = [
    {
      type: 'input',
      label: 'Entity',
      name: 'Entity',
      inputType: 'text',
      flex: 90,
      disabled: true,
      value: this.data.parentEntityName,
      order: 0
    },
    {
      type: 'input',
      label: 'Entity Id',
      name: 'EntityId',
      inputType: 'text',
      flex: 90,
      disabled: true,
      value: this.data.parentEntityId,
      order: 1
    },
    {
      type: 'defaultproperty',
      label: 'Property',
      name: 'Property',
      inputType: 'text',
      flex: 90,
      disabled: false,
      validations: [{name: 'required', message: 'Property is required', validator: Validators.required}],
      order: 2,
      searchEntity: this.data.parentEntityName
    },
    {
      type: 'input',
      label: 'Value',
      name: 'Value',
      inputType: 'text',
      flex: 90,
      validations: [{name: 'required', message: 'Value is required', validator: Validators.required}],
      order: 3
    },
  ]
  entityName!: string

  constructor(public dialogRef: MatDialogRef<ExtensionEntityDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: ExtensionEntityDialogComponentData,
              private entityService: EntityService,
              private snackBar: MatSnackBar) { }

  ngOnInit() {
    this.entityName = this.data.extensionEntityData.Type.startsWith('self.') ? // If the data starts with .self
                      this.data.extensionEntityData.Type.substring(5) : // Strip it out
                      this.data.extensionEntityData.Type // Otherwise just pass the provided value

    this.entityConfig.forEach(x => x.entity = this.entityName)
  }

  submit(event: any) {
    this.showProgressBar = true

    this.entityService.addEntity(this.entityName, [{
      Entity: this.data.parentEntityName,
      EntityId: this.data.parentEntityId,
      Property: event.Property,
      Value: event.Value
    }]).subscribe((results: any) => {
      this.snackBar.open(`${this.data.extensionEntityData.Name} record has been added`, <any>null, { duration: 1000 })
      this.dialogRef.close('Added')
    },
    (error) => {this.snackBar.open(`There was an error creating the ${this.data.extensionEntityData.Name}. ${error}`)},
    () => this.showProgressBar = false)
  }
}

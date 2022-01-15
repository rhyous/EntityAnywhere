import { Component, OnInit, Input } from '@angular/core'
import { FormGroup } from '@angular/forms'
import { MatDialog } from '@angular/material'

import { FieldConfig } from '../../models/interfaces/field-config.interface'
import { EntityService } from 'src/app/core/services/entity.service'
import { environment } from 'src/environments/environment'
import { ErrorReporterDialogComponent } from 'src/app/core/dialogs/error-reporter-dialog/error-reporter-dialog.component'
// tslint:disable-next-line: max-line-length
import { EntitySearcherDialogComponent } from '../../../common-dialogs-module/components/entity-searcher-dialog/entity-searcher-dialog.component'
import { Router } from '@angular/router'

@Component({
  selector: 'app-entity-searcher',
  templateUrl: './entity-searcher.component.html',
  styleUrls: ['./entity-searcher.component.scss']
})
export class EntitySearcherComponent implements OnInit {
  @Input() field: FieldConfig
  @Input() group: FormGroup

  dialogWidth = '800px' // TODO: allow this to be injected
  entityValue = ''

  canGoTo = false

  constructor(public matDialog: MatDialog,
    private entityService: EntityService,
    public errorReporter: ErrorReporterDialogComponent,
    public router: Router) { }

  ngOnInit() {

    if (this.field && this.field.value !== undefined) {
      this.canGoTo = true
      this.entityService.getFilteredEntityList(this.field.searchEntity, `Id eq ${this.field.value}`, 1, 0)
      .subscribe((response) => {this.getResponseObject(response)},
                  (error) => {
                    this.errorReporter.displayMessage(error.error)})
    }
  }

  getResponseObject(response) {
    if (response !== undefined && response.Entities !== undefined && response.Entities[0].Object !== undefined) {
      const metaData = <any[]>JSON.parse(localStorage.getItem(environment.metaDataLocalName))
      const entityMetaData = metaData.firstOrDefault(x => x.key === this.field.searchEntity)

      const displayProperty = entityMetaData && entityMetaData.value['@UI.DisplayName']
        ? entityMetaData.value['@UI.DisplayName']['$PropertyPath']
        : 'Name'

      this.entityValue = response.Entities[0].Object[displayProperty]

      if (this.entityValue === undefined || this.entityValue === '') {
        // Log error
      }
      this.field.value = +response.Entities[0].Id
    } else if (this.field.searchEntityDefault) {
      this.canGoTo = false
      this.getDefaultData()
    } else {
      // Log error
    }
  }

  getDefaultData() {
    // Gets the value to display if entity has a search entity default value.
    if (this.field.value === this.field.searchEntityDefault.Value) {
      this.entityValue = this.field.searchEntityDefault.Name
    }
  }

  getEntity() {
    this.matDialog.open(EntitySearcherDialogComponent,
    {
      width: this.dialogWidth,
      data : { entityName: this.field.searchEntity, default: this.field.searchEntityDefault }
    }).afterClosed().subscribe((response) => {
      if (response && response !== 'Cancel') {
        this.canGoTo = false
        this.entityValue = response.Name
        this.field.validations = response.Id
        this.group.controls[this.field.name].setErrors(null)
        this.group.controls[this.field.name].setValue(response.Id)
        this.group.get(this.field.name).markAsDirty()
      }
    })
  }

  goto() {
    this.router.navigate([`./admin/data-management/${this.field.searchEntity}/${this.field.value}/`])
  }

}

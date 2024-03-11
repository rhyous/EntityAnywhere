import { Component, OnInit, Input } from '@angular/core'
import { FormGroup } from '@angular/forms'

import { FieldConfig } from '../../models/interfaces/field-config.interface'

@Component({
  selector: 'app-entity-checkbox',
  templateUrl: './entity-checkbox.component.html',
  styleUrls: ['./entity-checkbox.component.scss']
})
/**
 * Checkbox Component. Responsible for rendering a checkbox
 */
export class EntityCheckboxComponent implements OnInit {

  @Input() field!: FieldConfig
  @Input() group!: FormGroup

  constructor() { }

  ngOnInit() {
  }

}

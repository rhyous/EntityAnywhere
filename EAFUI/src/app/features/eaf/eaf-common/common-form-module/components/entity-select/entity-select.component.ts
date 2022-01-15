import { Component, OnInit, Input } from '@angular/core'
import { FormGroup } from '@angular/forms'

import { FieldConfig } from '../../models/interfaces/field-config.interface'

@Component({
  selector: 'app-entity-select',
  templateUrl: './entity-select.component.html',
  styleUrls: ['./entity-select.component.scss']
})
export class EntitySelectComponent implements OnInit {

  @Input() field: FieldConfig
  @Input() group: FormGroup

  constructor() { }

  ngOnInit() {
  }
}

import { Component, OnInit, Input } from '@angular/core'
import { FormGroup } from '@angular/forms'

import { FieldConfig } from '../../models/interfaces/field-config.interface'

@Component({
  selector: 'app-entity-date',
  templateUrl: './entity-date.component.html',
  styleUrls: ['./entity-date.component.scss']
})
export class EntityDateComponent implements OnInit {
  @Input() field: FieldConfig
  @Input() group: FormGroup

  constructor() { }

  ngOnInit() {
  }

}

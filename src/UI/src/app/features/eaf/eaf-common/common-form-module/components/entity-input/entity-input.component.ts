import { Component, OnInit, Input } from '@angular/core'
import { FormGroup } from '@angular/forms'

import { FieldConfig } from '../../models/interfaces/field-config.interface'

@Component({
  selector: 'app-entity-input',
  templateUrl: './entity-input.component.html',
  styleUrls: ['./entity-input.component.scss']
})
export class EntityInputComponent implements OnInit {

  @Input() field!: FieldConfig
  @Input() group!: FormGroup
  constructor() { }

  ngOnInit() {
  }

}


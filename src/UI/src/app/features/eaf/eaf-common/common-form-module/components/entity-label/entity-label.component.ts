import { Component, OnInit } from '@angular/core'

import { FieldConfig } from '../../models/interfaces/field-config.interface'

@Component({
  selector: 'app-entity-label',
  templateUrl: './entity-label.component.html',
  styleUrls: ['./entity-label.component.scss']
})
export class EntityLabelComponent implements OnInit {

  field!: FieldConfig
  constructor() { }

  ngOnInit() {
  }

}

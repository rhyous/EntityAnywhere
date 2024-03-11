import { Component, OnInit } from '@angular/core'
import { FormGroup } from '@angular/forms'
import { FieldConfig } from '../../models/interfaces/field-config.interface'

@Component({
  selector: 'app-entity-button',
  templateUrl: './entity-button.component.html',
  styleUrls: ['./entity-button.component.scss']
})

export class EntityButtonComponent implements OnInit {

  field!: FieldConfig
  group!: FormGroup

  constructor() {}
  ngOnInit() {}

}

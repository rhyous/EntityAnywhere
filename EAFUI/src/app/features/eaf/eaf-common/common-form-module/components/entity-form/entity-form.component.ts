import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core'
import { FormGroup, FormBuilder, Validators } from '@angular/forms'
import { ActivatedRoute } from '@angular/router'

import { ArraySortOrderPipe } from 'src/app/core/pipes/array-sort-order.pipe'
import { FieldConfig } from '../../models/interfaces/field-config.interface'
import { EntitySearcherComponent } from '../entity-searcher/entity-searcher.component'


@Component({
  exportAs: 'entityForm',
  selector: 'app-entity-form',
  templateUrl: './entity-form.component.html',
  styleUrls: ['./entity-form.component.scss'],
  providers: [ArraySortOrderPipe, EntitySearcherComponent]
})
export class EntityFormComponent implements OnInit {
  @Input() fields: FieldConfig[] = []
  @Output() submit: EventEmitter<any> = new EventEmitter<any>()
  @Output() clone: EventEmitter<any> = new EventEmitter<any>()
  form: FormGroup

  canClone = false
  canSave = false
  addLabel = ''
  constructor(private fb: FormBuilder,
              private arraySortByOrder: ArraySortOrderPipe,
              private activatedRoute: ActivatedRoute) {}

  ngOnInit() {
    this.form = this.createControl()
    this.activatedRoute.params.subscribe(params => {
      this.canClone = this.fields.where(x => x.type === 'Mapper').length === 0 && (params['id'] !== 'add' && !isNaN(params['id']))
      this.canSave = this.fields.where(x => x.type === 'Mapper').length === 0 // && params['id'] !== undefined
      this.addLabel = params['id'] === 'add' || params['id'] === 'clone' ? 'Add' : 'Update'
    })
  }

  get value() {
    return this.form.value
  }

  createControl() {
    const group = this.fb.group({})
    this.fields = this.arraySortByOrder.transform(this.fields, false)
    this.fields.forEach(field => {
      if (field.type === 'button') {
        return
      }
      const control = this.fb.control(
        {value: field.value, disabled: field.disabled === undefined ? false : field.disabled},
        this.bindValidations(field.validations || [])
      )
      group.addControl(field.name, control)
    })
    return group
  }

  bindValidations(validations: any) {
    if (validations.length > 0) {
      const validList = []
      validations.forEach(valid => {
        validList.push(valid.validator)
      })
      return Validators.compose(validList)
    }
    return null
  }

  onSubmit(event: Event) {
    event.preventDefault()
    event.stopPropagation()
    if (this.form.valid) {
      this.submit.emit(this.form.value)
    } else {
      this.validateAllFormFields(this.form)
    }
  }

  cloneRecord() {
    event.preventDefault()
    event.stopPropagation()
    const submitButton = this.fields.firstOrDefault(x => x.type === 'button')
    if (submitButton) {
      submitButton.label = 'Clone'
    }

    this.clone.emit()
  }

  validateAllFormFields(formGroup: FormGroup) {
    Object.keys(formGroup.controls).forEach(field => {
      const control = formGroup.get(field)
      control.markAsTouched({ onlySelf: true })
    })
  }

}

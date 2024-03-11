import { Injectable } from '@angular/core'
import { Validators } from '@angular/forms'
import { EntityField } from '../models/concretes/entity-field'

@Injectable()
export class EntityFieldValidatorProvider {

  constructor() {}

  getValidator(field: EntityField): any {
    const validations = []

    if (field.Required && field.Type !== 'Edm.Boolean') {
      validations.push({name: 'required', validator: Validators.required, message: `${field.Name} is required`})
    }
    return validations
  }
}

import { TestBed } from '@angular/core/testing'
import { Validators } from '@angular/forms'
import { EntityField } from '../models/concretes/entity-field'
import { EntityFieldValidatorProvider } from './entity-field-validator-provider'

describe('EntityFieldValidatorProvider', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [EntityFieldValidatorProvider]
    })
  })

  it ('Should get validators', () => {
    // Arrange
    const entityFieldValidatorProvider = TestBed.inject(EntityFieldValidatorProvider)
    const field = new EntityField()
    field.Nullable = false
    field.Type = 'Edm.String'
    field.Name = 'Password'
    field.Required = true

    // Act
    const validators = entityFieldValidatorProvider.getValidator(field)

    // Assert
    expect(validators).toEqual([{name: 'required', validator: Validators.required, message: 'Password is required'}])
  })

  it ('Should get validators', () => {
    // Arrange
    const entityFieldValidatorProvider = TestBed.inject(EntityFieldValidatorProvider)
    const field = new EntityField()
    field.Nullable = false
    field.Type = 'Edm.Boolean'
    field.Name = 'SomeFlag'
    field.Required = true

    // Act
    const validators = entityFieldValidatorProvider.getValidator(field)

    // Assert
    expect(validators).toEqual([])
  })
})

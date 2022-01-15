import { TestBed, inject } from '@angular/core/testing'
import { Validators } from '@angular/forms'

import { EntityPropertyControlService } from './entity-property-control.service'
import { FieldConfig } from '../models/interfaces/field-config.interface'
import { EntityField } from 'src/app/core/models/concretes/entity-field'


describe('EntitytDataServiceService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [EntityPropertyControlService]
    })
  })

  it('Should be created', inject([EntityPropertyControlService], (service: EntityPropertyControlService) => {
    expect(service).toBeTruthy()
  }))

  it ('Should create a form group', () => {
    // Arrange
    const entityProperties: FieldConfig[] = [
      {
        name: 'OrganizationId',
        label: 'Organization',
        value: 1,
        required: true,
        order: 1,
        flex: 50,
        type: 'number'
      },
      {
        name: 'OrderId',
        label: 'Order Id',
        value: '992929',
        required: false,
        order: 2,
        flex: 15,
        type: 'text'
      }]

    const epcs = TestBed.get(EntityPropertyControlService)

    // Act
    const formGroup = epcs.toFormGroup(entityProperties)

    // Assert
    expect(formGroup.controls['OrganizationId']).toBeTruthy()
    expect(formGroup.controls['OrderId']).toBeTruthy()
    expect(formGroup.controls['OrganizationId'].value).toEqual(1)
    expect(formGroup.controls['OrganizationId'].validator.length).toEqual(1)

  })

  it ('Should get an entity config from metadata', () => {
    // Arrange
    const epcs = TestBed.get(EntityPropertyControlService)
    spyOn(epcs, 'getPropertyType').and.returnValue('input')
    spyOn(epcs, 'getInputType').and.returnValue('text')
    spyOn(epcs, 'getValidator').and.returnValue([])
    const field = new EntityField()
    field.Name = 'Name'
    field.Type = 'Edm.String'
    field.DisplayOrder = 1
    field.Required = true
    field.ReadOnly = false

    // Act
    const entityConfig = epcs.getEntityConfig('Product', field, null, null)

    // Assert
    expect(entityConfig).toEqual({ type: 'input',
                                   label: 'Name',
                                   inputType: 'text',
                                   name: 'Name',
                                   entity: 'Product',
                                   flex: 50,
                                   validations: [],
                                   required: true,
                                   order: 1,
                                   readOnly: false })
    expect(epcs.getPropertyType).toHaveBeenCalledTimes(1)
    expect(epcs.getInputType).toHaveBeenCalledTimes(1)
    expect(epcs.getValidator).toHaveBeenCalledTimes(1)
  })

  it ('Should get an entity config with a value from metadata', () => {
    // Arrange
    const epcs = TestBed.get(EntityPropertyControlService)
    spyOn(epcs, 'getPropertyType').and.returnValue('input')
    spyOn(epcs, 'getInputType').and.returnValue('text')
    spyOn(epcs, 'getValidator').and.returnValue([])
    const field = new EntityField()
    field.Name = 'Name'
    field.Type = 'Edm.String'
    field.DisplayOrder = 1
    field.Required = true
    field.ReadOnly = false

    // Act
    const entityConfig = epcs.getEntityConfig('Product', field, {Name: 'Test'}, null)

    // Assert
    expect(entityConfig).toEqual({ type: 'input',
                                   label: 'Name',
                                   inputType: 'text',
                                   name: 'Name',
                                   entity: 'Product',
                                   flex: 50,
                                   validations: [],
                                   required: true,
                                   value: 'Test',
                                   order: 1,
                                   readOnly: false })
    expect(epcs.getPropertyType).toHaveBeenCalledTimes(1)
    expect(epcs.getInputType).toHaveBeenCalledTimes(1)
    expect(epcs.getValidator).toHaveBeenCalledTimes(1)
  })

  it ('Should get an entity config with validations from metadata', () => {
    // Arrange
    const epcs = TestBed.get(EntityPropertyControlService)
    spyOn(epcs, 'getPropertyType').and.returnValue('input')
    spyOn(epcs, 'getInputType').and.returnValue('text')
    spyOn(epcs, 'getValidator').and.returnValue([{Validation: 1}])

    const field = new EntityField()
    field.Name = 'Name'
    field.Type = 'Edm.String'
    field.DisplayOrder = 1
    field.Required = true
    field.ReadOnly = false

    // Act
    const entityConfig = epcs.getEntityConfig('Product', field, {Name: 'Test'}, undefined)

    // Assert
    expect(entityConfig).toEqual({ type: 'input',
                                   label: 'Name',
                                   inputType: 'text',
                                   name: 'Name',
                                   entity: 'Product',
                                   flex: 50,
                                   required: true,
                                   validations: [{Validation: 1}],
                                   value: 'Test',
                                   order: 1,
                                   readOnly: false })
    expect(epcs.getPropertyType).toHaveBeenCalledTimes(1)
    expect(epcs.getInputType).toHaveBeenCalledTimes(1)
    expect(epcs.getValidator).toHaveBeenCalledTimes(1)
  })

  it ('Should get an entity config for EntitySearcher', () => {
    // Arrange
    const epcs = TestBed.get(EntityPropertyControlService)
    spyOn(epcs, 'getPropertyType').and.returnValue('EntitySearcher')
    spyOn(epcs, 'getInputType').and.returnValue('text')
    spyOn(epcs, 'getValidator').and.returnValue([{Validation: 1}])

    const field = new EntityField()
    field.Name = 'Name'
    field.Type = 'Edm.String'
    field.DisplayOrder = 2
    field.EntityAlias = ''
    field.Required = true
    field.ReadOnly = false

    const navField = new EntityField()
    navField.Type = 'self.ProductType'
    navField.Default = null
    navField.NavigationKey = 'EntitySearcher'

    // Act
    const entityConfig = epcs.getEntityConfig('Product', field, {Name: 'Test'}, navField)
    console.log(entityConfig)

    // Assert
    expect(entityConfig).toEqual({ type: 'EntitySearcher',
                                   label: 'Name',
                                   inputType: 'text',
                                   name: 'Name',
                                   entity: 'Product',
                                   flex: 50,
                                   required: true,
                                   searchEntity: 'ProductType',
                                   validations: [{Validation: 1}],
                                   value: 'Test',
                                   order: 2,
                                   readOnly: false,
                                   searchEntityDefault: null })
    expect(epcs.getPropertyType).toHaveBeenCalledTimes(1)
    expect(epcs.getInputType).toHaveBeenCalledTimes(1)
    expect(epcs.getValidator).toHaveBeenCalledTimes(1)
  })

  it ('Should get an entity config for select with options', () => {
    // Arrange
    const epcs = TestBed.get(EntityPropertyControlService)
    spyOn(epcs, 'getPropertyType').and.returnValue('select')
    spyOn(epcs, 'getInputType').and.returnValue('text')
    spyOn(epcs, 'getValidator').and.returnValue([{Validation: 1}])

    const field = new EntityField()
    field.Name = 'Name'
    field.Type = 'Edm.String'
    field.DisplayOrder = 1
    field.Kind = 'EnumType'
    field.Options = new Map<number, string>([[1, 'Test']])
    field.Required = true
    field.ReadOnly = false

    // Act
    const entityConfig = epcs.getEntityConfig('Product', field, {Name: 'Test'}, undefined)

    // Assert
    expect(entityConfig).toEqual({ type: 'select',
                                   label: 'Name',
                                   inputType: 'text',
                                   name: 'Name',
                                   entity: 'Product',
                                   flex: 50,
                                   options: [{Id: 1, Value: 'Test'}],
                                   validations: [{Validation: 1}],
                                   required: true,
                                   value: 'Test',
                                   order: 1,
                                   readOnly: false })
    expect(epcs.getPropertyType).toHaveBeenCalledTimes(1)
    expect(epcs.getInputType).toHaveBeenCalledTimes(1)
    expect(epcs.getValidator).toHaveBeenCalledTimes(1)
  })

  it ('Should get a number input type', () => {
    // Arrange
    const epcs = TestBed.get(EntityPropertyControlService)

    // Act
    const returnType = epcs.getInputType('Edm.Int64')

    // Assert
    expect(returnType).toEqual('number')
  })

  it ('Should get a string input type', () => {
    // Arrange
    const epcs = TestBed.get(EntityPropertyControlService)

    // Act
    const returnType = epcs.getInputType('Edm.String')

    // Assert
    expect(returnType).toEqual('text')
  })

  it ('Should return a propertype of EntitySearcher for propertys with a Navigation Key', () => {
    // Arrange
    const epcs = TestBed.get(EntityPropertyControlService)
    const field = new EntityField()
    field.NavigationKey = 'Product'

    // Act
    const returnType = epcs.getPropertyType(field)

    // Assert
    expect(returnType).toEqual('EntitySearcher')
  })

  it ('Should return a property type from the $type', () => {
    // Arrange
    const epcs = TestBed.get(EntityPropertyControlService)
    const field = new EntityField()
    field.Type = 'Edm.Int64'

    // Act
    const returnType = epcs.getPropertyType(field)

    // Assert
    expect(returnType).toEqual('input')
  })

  it ('Should return a date property type from the $type', () => {
    // Arrange
    const epcs = TestBed.get(EntityPropertyControlService)
    const field = new EntityField()
    field.Type = 'Edm.Date'

    // Act
    const returnType = epcs.getPropertyType(field)

    // Assert
    expect(returnType).toEqual('date')
  })

  it ('Should return a checkbox property type from the $type', () => {
    // Arrange
    const epcs = TestBed.get(EntityPropertyControlService)
    const field = new EntityField()
    field.Type = 'Edm.Boolean'

    // Act
    const returnType = epcs.getPropertyType(field)

    // Assert
    expect(returnType).toEqual('checkbox')
  })

  it ('Should return a select property type from the $type', () => {
    // Arrange
    const epcs = TestBed.get(EntityPropertyControlService)
    const field = new EntityField()
    field.Kind = 'EnumType'

    // Act
    const returnType = epcs.getPropertyType(field)

    // Assert
    expect(returnType).toEqual('select')
  })


  it ('Should get validators', () => {
    // Arrange
    const epcs = TestBed.get(EntityPropertyControlService)
    const field = new EntityField()
    field.Nullable = false
    field.Type = 'Edm.String'
    field.Name = 'Password'
    field.Required = true

    // Act
    const validators = epcs.getValidator(field)

    // Assert
    expect(validators).toEqual([{name: 'required', validator: Validators.required, message: 'Password is required'}])
  })
})

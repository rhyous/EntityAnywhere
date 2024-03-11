import { TestBed, inject } from '@angular/core/testing'
import { Validators } from '@angular/forms'

import { FieldConfig } from '../models/interfaces/field-config.interface'
import { EntityField } from '../models/concretes/entity-field'
import { EntityPropertyTypeControlTypeMap } from './entity-property-type-control-type.map'
import { EntityConfigBuilder } from './entity-config-builder'
import { EntityPropertyTypeInputTypeMap } from './entity-property-type-input-type.map'
import { EntityFieldValidatorProvider } from './entity-field-validator-provider'
import { StringTypeControlTypeMap } from './string-type-control-type.map'
import { EnumOptionMapper } from './enum-option-mapper'
import { SpaceTitlePipe } from 'src/app/core/pipes/spacetitle.pipe'


describe('EntityConfigBuilder', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [EntityConfigBuilder,
                  EntityPropertyTypeControlTypeMap,
                  EntityPropertyTypeInputTypeMap,
                  EntityFieldValidatorProvider,
                  EnumOptionMapper,
                  StringTypeControlTypeMap,
                  SpaceTitlePipe]
    })
  })

  it ('Should get an entity config from metadata', () => {
    // Arrange
    const entityConfigBuilder = TestBed.inject(EntityConfigBuilder)
    const entityPropertyTypeControlTypeMap = TestBed.inject(EntityPropertyTypeControlTypeMap)
    spyOn(entityPropertyTypeControlTypeMap, 'getValueOrDefault').and.returnValue((entityField: EntityField) => 'input')
    const entityPropertyTypeInputTypeMap = TestBed.inject(EntityPropertyTypeInputTypeMap)
    spyOn(entityPropertyTypeInputTypeMap, 'getValueOrDefault').and.returnValue('text')
    const entityFieldValidatorProvider = TestBed.inject(EntityFieldValidatorProvider)
    spyOn(entityFieldValidatorProvider, 'getValidator').and.returnValue([])
    const field = new EntityField()
    field.Name = 'Name'
    field.Type = 'Edm.String'
    field.DisplayOrder = 1
    field.Required = true
    field.ReadOnly = false

    // Act
    const entityConfig = entityConfigBuilder.build('Product', field, null, <any>null)

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
                                   readOnly: false,
                                   options: undefined,
                                   filter: undefined })
    expect(entityPropertyTypeControlTypeMap.getValueOrDefault).toHaveBeenCalledTimes(1)
    expect(entityPropertyTypeInputTypeMap.getValueOrDefault).toHaveBeenCalledTimes(1)
    expect(entityFieldValidatorProvider.getValidator).toHaveBeenCalledTimes(1)
  })

  it ('Should build config', () => {
    // Arrange
    const entityConfigBuilder = TestBed.inject(EntityConfigBuilder)
    const entityPropertyTypeControlTypeMap = TestBed.inject(EntityPropertyTypeControlTypeMap)
    spyOn(entityPropertyTypeControlTypeMap, 'getValueOrDefault').and.returnValue((entityField: EntityField) => 'input')
    const entityPropertyTypeInputTypeMap = TestBed.inject(EntityPropertyTypeInputTypeMap)
    spyOn(entityPropertyTypeInputTypeMap, 'getValueOrDefault').and.returnValue('text')
    const entityFieldValidatorProvider = TestBed.inject(EntityFieldValidatorProvider)
    spyOn(entityFieldValidatorProvider, 'getValidator').and.returnValue([])
    const field = new EntityField()
    field.Name = 'Name'
    field.Type = 'Edm.String'
    field.DisplayOrder = 1
    field.Required = true
    field.ReadOnly = false

    // Act
    const entityConfig = entityConfigBuilder.build('Product', field, {Name: 'Test'}, <any>null)

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
                                   readOnly: false,
                                   options: undefined,
                                   filter: undefined })
    expect(entityPropertyTypeControlTypeMap.getValueOrDefault).toHaveBeenCalledTimes(1)
    expect(entityPropertyTypeInputTypeMap.getValueOrDefault).toHaveBeenCalledTimes(1)
    expect(entityFieldValidatorProvider.getValidator).toHaveBeenCalledTimes(1)
  })

  it ('Should get an entity config with validations from metadata', () => {
    // Arrange
    const entityConfigBuilder = TestBed.inject(EntityConfigBuilder)
    const entityPropertyTypeControlTypeMap = TestBed.inject(EntityPropertyTypeControlTypeMap)
    spyOn(entityPropertyTypeControlTypeMap, 'getValueOrDefault').and.returnValue((entityField: EntityField) => 'input')
    const entityPropertyTypeInputTypeMap = TestBed.inject(EntityPropertyTypeInputTypeMap)
    spyOn(entityPropertyTypeInputTypeMap, 'getValueOrDefault').and.returnValue('text')
    const entityFieldValidatorProvider = TestBed.inject(EntityFieldValidatorProvider)
    spyOn(entityFieldValidatorProvider, 'getValidator').and.returnValue([{name: 'required', validator: Validators.required, message: ''}])

    const field = new EntityField()
    field.Name = 'Name'
    field.Type = 'Edm.String'
    field.DisplayOrder = 1
    field.Required = true
    field.ReadOnly = false

    // Act
    const entityConfig = entityConfigBuilder.build('Product', field, {Name: 'Test'}, <any>undefined)

    // Assert
    expect(entityConfig).toEqual({ type: 'input',
                                   label: 'Name',
                                   inputType: 'text',
                                   name: 'Name',
                                   entity: 'Product',
                                   flex: 50,
                                   required: true,
                                   validations: [{name: 'required', validator: Validators.required, message: ''}],
                                   value: 'Test',
                                   order: 1,
                                   readOnly: false,
                                   options: undefined,
                                   filter: undefined
                                 })
    expect(entityPropertyTypeControlTypeMap.getValueOrDefault).toHaveBeenCalledTimes(1)
    expect(entityPropertyTypeInputTypeMap.getValueOrDefault).toHaveBeenCalledTimes(1)
    expect(entityFieldValidatorProvider.getValidator).toHaveBeenCalledTimes(1)
  })

  it ('Should get an entity config for EntitySearcher', () => {
    // Arrange
    const entityConfigBuilder = TestBed.inject(EntityConfigBuilder)
    const entityPropertyTypeControlTypeMap = TestBed.inject(EntityPropertyTypeControlTypeMap)
    spyOn(entityPropertyTypeControlTypeMap, 'getValueOrDefault').and.returnValue((entityField: EntityField) => 'EntitySearcher')
    const entityPropertyTypeInputTypeMap = TestBed.inject(EntityPropertyTypeInputTypeMap)
    spyOn(entityPropertyTypeInputTypeMap, 'getValueOrDefault').and.returnValue('text')
    const entityFieldValidatorProvider = TestBed.inject(EntityFieldValidatorProvider)
    spyOn(entityFieldValidatorProvider, 'getValidator').and.returnValue([{name: 'required', validator: Validators.required, message: ''}])

    const field = new EntityField()
    field.Name = 'Name'
    field.Type = 'Edm.String'
    field.DisplayOrder = 2
    field.EntityAlias = ''
    field.Required = true
    field.ReadOnly = false

    const navField = new EntityField()
    navField.Type = 'self.ProductType'
    navField.Default = <any>null
    navField.NavigationKey = 'EntitySearcher'

    // Act
    const entityConfig = entityConfigBuilder.build('Product', field, {Name: 'Test'}, navField)
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
                                   validations: [{name: 'required', validator: Validators.required, message: ''}],
                                   value: 'Test',
                                   order: 2,
                                   readOnly: false,
                                   searchEntityDefault: <any>null,
                                   options: undefined,
                                   filter: undefined })
    expect(entityPropertyTypeControlTypeMap.getValueOrDefault).toHaveBeenCalledTimes(1)
    expect(entityPropertyTypeInputTypeMap.getValueOrDefault).toHaveBeenCalledTimes(1)
    expect(entityFieldValidatorProvider.getValidator).toHaveBeenCalledTimes(1)
  })

  it ('Should get an entity config for select with options', () => {
    // Arrange
    const entityConfigBuilder = TestBed.inject(EntityConfigBuilder)
    const entityPropertyTypeControlTypeMap = TestBed.inject(EntityPropertyTypeControlTypeMap)
    spyOn(entityPropertyTypeControlTypeMap, 'getValueOrDefault').and.returnValue((entityField: EntityField) => 'select')
    const entityPropertyTypeInputTypeMap = TestBed.inject(EntityPropertyTypeInputTypeMap)
    spyOn(entityPropertyTypeInputTypeMap, 'getValueOrDefault').and.returnValue('text')
    const entityFieldValidatorProvider = TestBed.inject(EntityFieldValidatorProvider)
    spyOn(entityFieldValidatorProvider, 'getValidator').and.returnValue([{name: 'required', validator: Validators.required, message: ''}])

    const field = new EntityField()
    field.Name = 'Name'
    field.Type = 'Edm.String'
    field.DisplayOrder = 1
    field.Kind = 'EnumType'
    field.Options = new Map<number, string>([[1, 'Test']])
    field.Required = true
    field.ReadOnly = false


    const options = [{Id: 1, Value: 'Test'}]
    // Act
    const entityConfig = entityConfigBuilder.build('Product', field, {Name: 'Test'}, <any>undefined)

    // Assert
    expect(entityConfig).toEqual({ type: 'select',
                                   label: 'Name',
                                   inputType: 'text',
                                   name: 'Name',
                                   entity: 'Product',
                                   flex: 50,
                                   options: options,
                                   validations: [{name: 'required', validator: Validators.required, message: ''}],
                                   required: true,
                                   value: 'Test',
                                   order: 1,
                                   readOnly: false,
                                   filter: undefined })
    expect(entityPropertyTypeControlTypeMap.getValueOrDefault).toHaveBeenCalledTimes(1)
    expect(entityPropertyTypeInputTypeMap.getValueOrDefault).toHaveBeenCalledTimes(1)
    expect(entityFieldValidatorProvider.getValidator).toHaveBeenCalledTimes(1)
  })
})

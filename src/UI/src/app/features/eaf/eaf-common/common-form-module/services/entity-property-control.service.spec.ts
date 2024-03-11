import { TestBed, inject } from '@angular/core/testing'
import { EntityPropertyControlService } from './entity-property-control.service'
import { FieldConfig } from '../models/interfaces/field-config.interface'

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

    const epcs = TestBed.inject(EntityPropertyControlService)

    // Act
    const formGroup = epcs.toFormGroup(entityProperties)

    // Assert
    expect(formGroup.controls['OrganizationId']).toBeTruthy()
    expect(formGroup.controls['OrderId']).toBeTruthy()
    expect(formGroup.controls['OrganizationId'].value).toEqual(1)
    expect(formGroup.controls['OrganizationId'].validator?.length).toEqual(1)

  })
})

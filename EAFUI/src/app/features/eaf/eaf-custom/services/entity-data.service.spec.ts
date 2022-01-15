import { TestBed, inject, getTestBed } from '@angular/core/testing'
import { DecimalPipe } from '@angular/common'

import { EntityDataService } from './entity-data.service'

describe('EntitytDataServiceService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [EntityDataService, DecimalPipe]
    })
  })

  it('should be created', inject([EntityDataService], (service: EntityDataService) => {
    expect(service).toBeTruthy()
  }))

  it('should format dates correctly', () => {
    const entityService = getTestBed().get(EntityDataService)
    const entityObject = entityService.convertDatesToWCFFormat({StartDate: '2018-09-14', LastUpdated: '2018-09-14', EndDate: ''})
    expect(entityObject['EndDate']).toBeUndefined()
    expect(entityObject['StartDate']).toMatch(/\d{13}[\+-]\d{2}:\d{2}/)
    expect(entityObject['LastUpdated']).toMatch(/\d{13}[\+-]\d{2}:\d{2}/)
  })


  it('should remove auditable properties from an entity', () => {
    const entityService = getTestBed().get(EntityDataService)
    const entityData = {'Id': 343756, 'Username': "Allen1", 'Firstname': 'Allen', 'CreateDate': '2018-09-20T10:57:19.6373979', 'CreatedBy': 2, 'Enabled': true, 
    'LastUpdated': null, 'LastUpdatedBy': null}

    const auditableProperties = ['CreateDate', 'CreatedBy', 'LastUpdated', 'LastUpdatedBy', 'Id']

    const returnData = entityService.removeAuditableProperties(entityData, auditableProperties)

    expect(returnData.CreateDate).toBeUndefined()
    expect(returnData.Id).toBeUndefined()
    expect(returnData.Firstname).toEqual('Allen')

  })

  it('should get the related entity', () => {
    const entityService = getTestBed().get(EntityDataService)
    const data = {
      'Id': 9,
      'Object': {
        'Id': 9,
        'Name': 'LANDesk AntiVirus Termed',
        'CreateDate': '2018-08-02T09:51:23.683',
        'CreatedBy': 1,
        'Description': 'LANDesk AntiVirus Termed',
        'Enabled': true,
        'LastUpdated': null,
        'LastUpdatedBy': null,
        'TypeId': 1,
        'Version': '9.50'
      },
      'RelatedEntityCollection': [{
        'Count': 1,
        'RelatedEntity': 'ProductGroup',
        'RelatedEntities': [{
          'Id': '1',
          'Object': {
            'Id': 1,
            'Name': 'Management Suite',
            'CreateDate': '2016-08-10T12:27:50.7890592',
            'CreatedBy': 1,
            'Description': '',
            'LastUpdated': '2018-08-02T09:51:23.793',
            'LastUpdatedBy': 1
          },
          'Uri': 'http://localhost:3896/ProductGroupService.svc/ProductGroups/Ids(1)'
        }]
      }],
      'Uri': 'http://localhost:3896/ProductService.svc/Products(9)'
    }

    const retValue = entityService.getRelatedEntityArray('ProductGroup', data, ['Id', 'Name'] )
    expect(retValue).toEqual([{Id: 1, Name: 'Management Suite'}])
  })

  it('createFilterSting should be able to distinguish between exact and partial match', () => {
    // Arrange
    const entityService = getTestBed().get(EntityDataService)

    const obj = {
      myProp: {
        filter: 'My Filter',
        exactMatch: false
      },
      myOtherProp: {
        filter: 'My Other Filter',
        exactMatch: true
      }
    }

    // Act
    const result = entityService.createFilterString(obj)

    // Assert
    expect(result).toEqual(`contains(myProp,'My Filter') and myOtherProp eq 'My Other Filter'`)
  })

  it('createFilterSting shouldnt add filters that have not been set', () => {
    // Arrange
    const entityService = getTestBed().get(EntityDataService)

    const obj = {
      myProp: {
        filter: 'My Filter',
        exactMatch: false
      },
      myOtherProp: {
        filter: 'My Other Filter',
        exactMatch: true
      },
      myUnsetFilter: {
        filter: null,
        exactMatch: null
      },
      myOtherUnsetFilter: {
        filter: '',
        exactMatch: false
      }
    }

    // Act
    const result = entityService.createFilterString(obj)

    // Assert
    expect(result).toEqual(`contains(myProp,'My Filter') and myOtherProp eq 'My Other Filter'`)
  })
})

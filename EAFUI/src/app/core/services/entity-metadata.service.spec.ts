import { TestBed } from '@angular/core/testing'
import { ReferentialConstraint } from '../models/concretes/referential-constraint'
import {Fake} from './entity-metadata-fake'

import { EntityMetadataService } from './entity-metadata.service'

const entityObj =   {
  '$Key': [
      'Id'
  ],
  '$Kind': 'EntityType',
  'Brand': {
      '$Nullable': true,
      '$Type': 'Edm.String',
      '@UI.DisplayOrder': 2,
      '@UI.Searchable': true
  },
  'CreateDate': {
      '$Type': 'Edm.DateTimeOffset',
      '@UI.DisplayOrder': 11,
      '@UI.Searchable': false,
      '@UI.ReadOnly': true
  },
  'CreatedBy': {
      '$Type': 'Edm.Int64',
      '@UI.DisplayOrder': 12,
      '@UI.Searchable': false,
      '@UI.ReadOnly': true
  },
  'Description': {
      '$Nullable': true,
      '$Type': 'Edm.String',
      '@UI.DisplayOrder': 3,
      '@UI.Searchable': false
  },
  'EarlyPurchaseDate': {
      '$Nullable': true,
      '$Type': 'Edm.DateTimeOffset',
      '@UI.DisplayOrder': 4,
      '@UI.Searchable': false
  },
  'EndOfLife': {
      '$Nullable': true,
      '$Type': 'Edm.DateTimeOffset',
      '@UI.DisplayOrder': 5,
      '@UI.Searchable': false
  },
  'Id': {
      '$Type': 'Edm.Int32',
      '@UI.DisplayOrder': 1,
      '@UI.Searchable': true
  },
  'LastUpdated': {
      '$Nullable': true,
      '$Type': 'Edm.DateTimeOffset',
      '@UI.DisplayOrder': 13,
      '@UI.Searchable': false,
      '@UI.ReadOnly': true
  },
  'LastUpdatedBy': {
      '$Nullable': true,
      '$Type': 'Edm.Int64',
      '@UI.DisplayOrder': 14,
      '@UI.Searchable': false,
      '@UI.ReadOnly': true
  },
  'LicenseModelId': {
      '$Type': 'Edm.Int32',
      '@UI.DisplayOrder': 15,
      '@UI.Searchable': false,
      '$NavigationKey': 'LicenseModel'
  },
  'LicenseModel': {
      '$Kind': 'NavigationProperty',
      '$ReferentialConstraint': {
          'LicenseModelId': 'Id'
      },
      '$Type': 'self.LicenseModel',
      '@EAF.RelatedEntity.Type': 'Local'
  },
  'N': {
      '$Type': 'Edm.Int32',
      '@UI.DisplayOrder': 6,
      '@UI.Searchable': false
  },
  'ProductId': {
      '$Type': 'Edm.Int32',
      '@UI.DisplayOrder': 7,
      '@UI.Searchable': true,
      '$NavigationKey': 'Product'
  },
  'Product': {
      '$Kind': 'NavigationProperty',
      '$ReferentialConstraint': {
          'ProductId': 'Id'
      },
      '$Type': 'self.Product',
      '@EAF.RelatedEntity.Type': 'Local'
  },
  'ReleaseDate': {
      '$Type': 'Edm.DateTimeOffset',
      '@UI.DisplayOrder': 8,
      '@UI.Searchable': false
  },
  'Version': {
      '$Type': 'Edm.String',
      '@UI.DisplayOrder': 9,
      '@UI.Searchable': true
  },
  'VersionBrand': {
      '$Nullable': true,
      '$Type': 'Edm.String',
      '@UI.DisplayOrder': 10,
      '@UI.Searchable': true
  },
  '@EAF.EntityGroup': 'Product Packaging Management',
  '@UI.DisplayName': {
      '$PropertyPath': 'Brand'
  },
  'ProductReleaseFeatureMaps': {
      '$Collection': true,
      '$Kind': 'NavigationProperty',
      '$Nullable': true,
      '$Type': 'self.ProductReleaseFeatureMap',
      '@EAF.RelatedEntity.Type': 'Foreign'
  },
  'Features': {
      '$Collection': true,
      '$Kind': 'NavigationProperty',
      '$Nullable': true,
      '$Type': 'self.Feature',
      '@EAF.RelatedEntity.Type': 'Mapping',
      '@EAF.RelatedEntity.MappingEntityType': 'self.ProductReleaseFeatureMap'
  },
  'Addenda': {
      '$Collection': true,
      '$Kind': 'NavigationProperty',
      '$Nullable': true,
      '$Type': 'self.Addendum',
      '@EAF.RelatedEntity.Type': 'Extension'
  },
  'AlternateIds': {
      '$Collection': true,
      '$Kind': 'NavigationProperty',
      '$Nullable': true,
      '$Type': 'self.AlternateId',
      '@EAF.RelatedEntity.Type': 'Extension'
  },
  'Test1': {
    '$Nullable': true,
    '$Type': 'Edm.String',
    '@UI.DisplayOrder': 3,
    '@UI.Searchable': false,
    '@EAF.RelatedEntity.MappingEntityType': 'self.ProductReleaseFeatureMap'
  },
  'Test2': {
    '$Nullable': true,
    '$Type': 'Edm.String',
    '@UI.DisplayOrder': 3,
    '@UI.Searchable': false,
  }
}

describe('EntityMetadataService', () => {
  let service: EntityMetadataService

  beforeEach(() => TestBed.configureTestingModule({}))

  beforeEach(() => {
    service = TestBed.get(EntityMetadataService)
    spyOn(localStorage, 'getItem').and.returnValue(JSON.stringify(Fake.FakeMeta))
  })

  it('should be created', () => {
    expect(service).toBeTruthy()
  })

  it('should parse the metadata from localstorage and return an object', () => {
    // Arrange

    // Act
    const response = service.getAllEntityMetaData()

    // Assert
    expect(response).not.toEqual(null)
    expect(localStorage.getItem).toHaveBeenCalled()
  })

  it('should get a single table from local storage and supply an object', () => {
    // Arrange
    spyOn(service, 'getAllEntityMetaData').and.callThrough()

    // Act
    const response = service.getEntityMetaData('ActivationAttempt')

    // Assert
    expect(response).not.toBeNull()
    expect(localStorage.getItem).toHaveBeenCalled()
    expect(service.getAllEntityMetaData).toHaveBeenCalled()
    expect(response.value).not.toBeNull()
    expect(response.value.$Kind).toBe('EntityType')
  })

  it('should store the entity metadata in local storage', () => {
    // Arrange
    spyOn(localStorage, 'setItem').and.returnValue()

    // Act
    service.setMetaData('This is my metadata')


    // Assert
    expect(localStorage.setItem).toHaveBeenCalledWith('metadata', 'This is my metadata')
  })

  it('should return entity metadata', () => {
    // Arrange
    const entity = {key: 'ActivationAttempt', value: Fake.FakeMeta}
    spyOn(service, 'getDisplayName')
    spyOn(service, 'getFieldMetaData')

    // Act
    const md = service.getEntityFromMetaData(entity)

    // Assert
    expect(service.getDisplayName).toHaveBeenCalled()
    expect(service.getFieldMetaData).toHaveBeenCalled()
    expect(md.Name).toEqual('ActivationAttempt')
  })

  it('should return options', () => {
    // Arrange
    const field = {
      $Kind: 'EnumType',
      $UnderlyingType: 'Edm.Int32',
      Fixed: 2,
      Inherited: 1,
      Percentage: 3,
    }

    // Act
    const options = service.getOptions(field)

    // Assert
    expect(options)
  })

  it('should return the ReferentialConstraint', () => {
    // Arrange
    const entity = {
      '$Kind': 'NavigationProperty',
      '$ReferentialConstraint': {
         'LocalProperty': 'OrganizationId',
         'ForeignProperty': 'Id'
      },
      '$Type': 'self.Organization',
      '@EAF.RelatedEntity.Type': 'Local'
    }

    const rc = new ReferentialConstraint()
    rc.LocalProperty = 'OrganizationId'
    rc.ForeignProperty = 'Id'

    // Act
    const result = service.getReferentialConstraint(entity)

    // Assert
    expect(result.LocalProperty).toEqual(rc.LocalProperty)
    expect(result.ForeignProperty).toEqual(rc.ForeignProperty)
  })

  it('should return undefined when no ReferentialConstraint', () => {
    // Arrange
    const entity = {
      '$Kind': 'NavigationProperty',
      '$Type': 'self.Organization',
      '@EAF.RelatedEntity.Type': 'Local'
    }

    // Act
    const result = service.getReferentialConstraint(entity)

    // Assert
    expect(result).toEqual(undefined)
  })

  it('should return the correct number value', () => {
    // Arrange
    const entity = {
      '$Nullable': true,
      '$Type': 'Edm.String',
      '@UI.DisplayOrder': 2,
      '@UI.Searchable': false
    }

    // Act
    const result = service.getNumberAttribute(entity, '@UI.DisplayOrder')

    // Assert
    expect(result).toEqual(2)
  })

  it('should return 0 as default ', () => {
    // Arrange
    const entity = {
      '$Nullable': true,
      '$Type': 'Edm.String',
      '@UI.Searchable': false
    }

    // Act
    const result = service.getNumberAttribute(entity, '@UI.DisplayOrder')

    // Assert
    expect(result).toEqual(0)
  })

  it('should return the correct bool value', () => {
    // Arrange
    const entity = {
      '$Nullable': true,
      '$Type': 'Edm.String',
      '@UI.DisplayOrder': 2,
      '@UI.Searchable': false,
      '@UI.ReadOnly': true,
    }

    // Act
    const result = service.getBooleanAttribute(entity, '@UI.ReadOnly')
    const result2 = service.getBooleanAttribute(entity, '@UI.Searchable')

    // Assert
    expect(result).toBeTruthy()
    expect(result2).toBeFalsy()
  })

  it('should return false as default ', () => {
    // Arrange
    const entity = {
      '$Nullable': true,
      '$Type': 'Edm.String',
      '@UI.DisplayOrder': 2,
    }

    // Act
    const result = service.getBooleanAttribute(entity, '@UI.ReadOnly')

    // Assert
    expect(result).toBeFalsy()
  })

  it('should return the correct DisplayName value', () => {
    // Arrange
    const entity = {
      '$Nullable': true,
      '$Type': 'Edm.String',
      '@UI.DisplayOrder': 2,
      '@UI.Searchable': false,
      '@UI.DisplayName': 'Fred',
    }

    // Act
    const result = service.getDisplayName(entity)

    // Assert
    expect(result).toEqual('Fred')
  })

  it('should return the correct DisplayName value', () => {
    // Arrange
    const entity = {
      '$Nullable': true,
      '$Type': 'Edm.String',
      '@UI.DisplayOrder': 2,
      '@UI.Searchable': false,
      'Name': 'Phil',
    }

    // Act
    const result = service.getDisplayName(entity)

    // Assert
    expect(result).toEqual('Name')
  })

  it('should return id as default ', () => {
    // Arrange
    const entity = {
      '$Nullable': true,
      '$Type': 'Edm.String',
      '@UI.DisplayOrder': 2,
    }

    // Act
    const result = service.getDisplayName(entity)

    // Assert
    expect(result).toEqual('Id')
  })

  it('should return array', () => {
    // Arrange

    // Act
    const result = service.convertMetaDataToArray(Fake.FakeMeta)

    // Assert
    expect(result.length).toEqual(51)
  })

  it('Should return an EntityField', () => {
    // Arrange

    // Act
    const result = service.getFieldMetaData(entityObj, 'AlternateIds')

    // Assert
    expect(result.Name).toEqual('AlternateIds')
  })

  it('Should return an EntityField', () => {
    // Arrange

    // Act
    const result = service.getFieldMetaData(entityObj, 'Test1')

    // Assert
    expect(result.Name).toEqual('Test1')
    expect(result.MappingEntity).toEqual('ProductReleaseFeatureMap')
  })

  // fit('Should return an EntityField', () => {
  //   // Arrange

  //   // Act
  //   const result = service.getFieldMetaData(entityObj, 'LicenseModel')
  //   console.log(result)
  //   // Assert
  //   expect(result.Name).toEqual('LicenseModel')
  //   expect(result.MappedId).toEqual('LicenseModelId')
  // })

})

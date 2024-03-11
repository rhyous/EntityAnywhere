import { TestBed } from '@angular/core/testing'
import { EntityField } from '../models/concretes/entity-field'
import { EntityPropertyTypeControlTypeMap } from './entity-property-type-control-type.map'
import { StringTypeControlTypeMap } from './string-type-control-type.map'

describe('EntityPropertyTypeControlTypeMap', () => {
    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [EntityPropertyTypeControlTypeMap,
                        StringTypeControlTypeMap]
        })
    })

    it('Default should be provided of key does not exist', () => {
        // Arrange
        const entityPropertyTypeControlTypeMap = TestBed.inject(EntityPropertyTypeControlTypeMap)

        // Act
        const func = entityPropertyTypeControlTypeMap.getValueOrDefault('NonExistent')
        const actual = func()

        // Assert
        expect(actual).toEqual('input')
    })

    it('If EntityField.hasNavigationKey is true, it should be an EntitySearcher', () => {
        // Arrange
        const entityPropertyTypeControlTypeMap = TestBed.inject(EntityPropertyTypeControlTypeMap)
        const entityField = new EntityField()
        entityField.NavigationKey = 'SomeRelatedEntity'

        // Act
        const func = entityPropertyTypeControlTypeMap.getValueOrDefault('Edm.Int32')
        const actual = func(entityField)

        // Assert
        expect(actual).toEqual('EntitySearcher')
    })

    it('Edm.String should call StringTypeControlTypeMap', () => {
        // Arrange
        const entityPropertyTypeControlTypeMap = TestBed.inject(EntityPropertyTypeControlTypeMap)
        const stringTypeControlTypeMap = TestBed.inject(StringTypeControlTypeMap)
        spyOn(stringTypeControlTypeMap, 'getValueOrDefault').and.returnValue(() => 'input')
        const entityField = new EntityField()
        entityField.StringType = 'SingleLine'

        // Act
        const func = entityPropertyTypeControlTypeMap.getValueOrDefault('Edm.String')
        const actual = func(entityField)

        // Assert
        expect(actual).toEqual('input')
    })
})

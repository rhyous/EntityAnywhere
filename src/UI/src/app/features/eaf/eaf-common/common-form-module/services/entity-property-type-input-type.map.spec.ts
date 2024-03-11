import { TestBed } from '@angular/core/testing'
import { EntityField } from '../models/concretes/entity-field'
import { EntityPropertyTypeControlTypeMap } from './entity-property-type-control-type.map'
import { EntityPropertyTypeInputTypeMap } from './entity-property-type-input-type.map'
import { StringTypeControlTypeMap } from './string-type-control-type.map'

describe('EntityPropertyTypeInputTypeMap', () => {
    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [EntityPropertyTypeInputTypeMap]
        })
    })

    it('Default should be text', () => {
        // Arrange
        const entityPropertyTypeInputTypeMap = TestBed.inject(EntityPropertyTypeInputTypeMap)

        // Act
        const actual = entityPropertyTypeInputTypeMap.getValueOrDefault('NonExistent')

        // Assert
        expect(actual).toEqual('text')
    })

    it('If it is found, it returns the found value', () => {
        // Arrange
        const entityPropertyTypeInputTypeMap = TestBed.inject(EntityPropertyTypeInputTypeMap)

        // Act
        const actual = entityPropertyTypeInputTypeMap.getValueOrDefault('Edm.Int32')

        // Assert
        expect(actual).toEqual('number')
    })
})

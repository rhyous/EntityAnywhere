import { NullEx } from './null-ex'

describe('NullEx', () => {
    // isZeroOrNegative
    it('isNullOrUndefined(val) for undefined should return true', () => {
        // Arrange
        // Act
        const actual = NullEx.isNullOrUndefined(undefined)
        // Assert
        expect(actual).toBeTrue()
    })

    it('isNullOrUndefined(val) for nullable with value should force type to be any after check', () => {
        // Arrange
        interface X { id?: number }
        const x: X = { id: 5 }
        const num = x.id
        let a
        // Act
        if (!NullEx.isNullOrUndefined(num)) {
            a = num >= 5 // This doesn't work without 'val is null | undefined' in isNullOrUndefined method declaration
        }

        // Assert
        expect(a).toBeTrue()
    })

    it('isNullOrUndefined(val) for null should return true', () => {
        // Arrange
        // Act
        const actual = NullEx.isNullOrUndefined(null)
        // Assert
        expect(actual).toBeTrue()
    })

    it('isNullOrUndefined(val) for any valid number should return false', ()  => {
        // Arrange
        const val = 127
        // Act
        const actual = NullEx.isNullOrUndefined(val)
        // Assert
        expect(actual).toBeFalse()
    })

    it('isNullOrUndefined(val) for any valid string should return false', ()  => {
        // Arrange
        const val = 'some string'
        // Act
        const actual = NullEx.isNullOrUndefined(val)
        // Assert
        expect(actual).toBeFalse()
    })
})


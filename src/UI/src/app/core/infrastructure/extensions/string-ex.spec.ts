import { StringEx } from './string-ex'

describe('StringExtensions', () => {
    // isUndefinedNullOrEmpty
    it('isUndefinedNullOrEmpty(val) for undefined should return true', () => {
        // Arrange
        // Act
        const actual = StringEx.isUndefinedNullOrEmpty(undefined)
        // Assert
        expect(actual).toBeTrue()
    })

    it('isUndefinedNullOrEmpty(val) for null should return true', () => {
        // Arrange
        // Act
        const actual = StringEx.isUndefinedNullOrEmpty(undefined)
        // Assert
        expect(actual).toBeTrue()
    })

    it('isUndefinedNullOrEmpty(val) for whitespace should return true', () => {
        // Arrange
        const str = ' '
        // Act
        const actual = StringEx.isUndefinedNullOrEmpty(str)
        // Assert
        expect(actual).toBeFalse()
    })

    it('isUndefinedNullOrEmpty(val) for valid string should return false', () => {
        // Arrange
        const str = 'abc'
        // Act
        const actual = StringEx.isUndefinedNullOrEmpty(str)
        // Assert
        expect(actual).toBeFalse()
    })

    // isUndefinedNullOrWhitespace
    it('isUndefinedNullOrEmpty(val) for undefined should return false', () => {
        // Arrange
        // Act
        const actual = StringEx.isUndefinedNullOrWhitespace(undefined)
        // Assert
        expect(actual).toBeTrue()
    })

    it('isUndefinedNullOrEmpty(val) for null should return false', () => {
        // Arrange
        // Act
        const actual = StringEx.isUndefinedNullOrWhitespace(undefined)
        // Assert
        expect(actual).toBeTrue()
    })

    it('isUndefinedNullOrWhitespace(val) for whitespace (any and all whitespace) should return true', () => {
        // Arrange
        const str = '\r\n\t '
        // Act
        const actual = StringEx.isUndefinedNullOrWhitespace(str)
        // Assert
        expect(actual).toBeTrue()
    })

    it('isUndefinedNullOrWhitespace(val) for valid string should return false', () => {
        // Arrange
        const str = 'abc'
        // Act
        const actual = StringEx.isUndefinedNullOrWhitespace(str)
        // Assert
        expect(actual).toBeFalse()
    })
})

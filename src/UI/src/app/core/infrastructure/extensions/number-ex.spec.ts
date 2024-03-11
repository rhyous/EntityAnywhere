import { NumberEx } from './number-ex'

describe('NumberEx', () => {
    // isZeroOrNegative
    it('isPositive(num) for undefined should return false', () => {
        // Arrange
        // Act
        const actual = NumberEx.isPositive(undefined)
        // Assert
        expect(actual).toBeFalse()
    })

    it('isPositive(num) for null should return false', () => {
        // Arrange
        // Act
        const actual = NumberEx.isPositive(null)
        // Assert
        expect(actual).toBeFalse()
    })

    it('isPositive(num) for negative number should return false', ()  => {
        // Arrange
        const num = -127
        // Act
        const actual = NumberEx.isPositive(num)
        // Assert
        expect(actual).toBeFalse()
    })

    it('NumberEx.isPositive(num) for zero should return false', ()  => {
        // Arrange
        const num = 0
        // Act
        const actual = NumberEx.isPositive(num)
        // Assert
        expect(actual).toBeFalse()
    })

    it('isPositive(num) for positive number should return true', ()  => {
        // Arrange
        const num = 5
        // Act
        const actual = NumberEx.isPositive(num)
        // Assert
        expect(actual).toBeTrue()
    })

    it('isPositive(num) for not set nullable number should return false', ()  => {
        // Arrange
        interface X { id?: number }
        const x: X = {}
        // Act
        const actual = NumberEx.isPositive(x.id)
        // Assert
        expect(actual).toBeFalse()
    })

    it('isPositive(num) for positive nullable number should return true', ()  => {
        // Arrange
        interface X { id?: number }
        const x: X = { id: 5 }
        // Act
        const actual = NumberEx.isPositive(x.id)
        // Assert
        expect(actual).toBeTrue()
    })

    // isZeroOrNegative
    it('isZeroOrNegative(num) for undefined should return false', () => {
        // Arrange
        // Act
        const actual = NumberEx.isZeroOrNegative(undefined)
        // Assert
        expect(actual).toBeFalse()
    })

    it('isZeroOrNegative(num) for null should return false', () => {
        // Arrange
        // Act
        const actual = NumberEx.isZeroOrNegative(null)
        // Assert
        expect(actual).toBeFalse()
    })

    it('isZeroOrNegative(num) for negative number should return true', ()  => {
        // Arrange
        const num = -127
        // Act
        const actual = NumberEx.isZeroOrNegative(num)
        // Assert
        expect(actual).toBeTrue()
    })

    it('isZeroOrNegative(num) for zero should return true', ()  => {
        // Arrange
        const num = 0
        // Act
        const actual = NumberEx.isZeroOrNegative(num)
        // Assert
        expect(actual).toBeTrue()
    })

    it('isZeroOrNegative(num) for positive number should return false', ()  => {
        // Arrange
        const num = 5
        // Act
        const actual = NumberEx.isZeroOrNegative(num)
        // Assert
        expect(actual).toBeFalse()
    })

})

import { AnyEx } from './any-ex'
describe('AnyEx', () => {
    // getBoolean
    it('getType<boolean>(val) for undefined should return the default value of false', () => {
        // Arrange
        // Act
        const actual = AnyEx.getValue<boolean>(undefined, false)
        // Assert
        expect(actual).toBeFalse()
    })

    it('getType<boolean>(val) for null should return  the default value of false', () => {
        // Arrange
        // Act
        const actual = AnyEx.getValue<boolean>(null, false)
        // Assert
        expect(actual).toBeFalse()
    })

    it('getType<boolean>(val) should return the value of b which is true', () => {
        // Arrange
        const b = true
        // Act
        const actual = AnyEx.getValue<boolean>(b, false)
        // Assert
        expect(actual).toBeTrue()
    })

    // getNumber
    it('getType<number>(val) for undefined should return the default value of 0', () => {
        // Arrange
        const expected = 0
        // Act
        const actual = AnyEx.getValue<number>(undefined, 0)
        // Assert
        expect(actual).toEqual(expected)
    })

    it('getType<number>(val) for null should return the default value of 0', () => {
        // Arrange
        const expected = 0
        // Act
        const actual = AnyEx.getValue<number>(null, 0)
        // Assert
        expect(actual).toEqual(expected)
    })

    it('getType<number>(val) should return the correct number', ()  => {
        // Arrange
        const num = 5
        // Act
        const actual = AnyEx.getValue<number>(num, 0)
        // Assert
        expect(actual).toEqual(num)
    })

})

import { SplitPascalCasePipe } from './split-pascal-case.pipe'

describe('SplitPascalCasePipe', () => {
  it('create an instance', () => {
    const pipe = new SplitPascalCasePipe()
    expect(pipe).toBeTruthy()
  })

  it('should split PascalCased string into separate words', () => {
    // Arrange
    const pipe = new SplitPascalCasePipe()

    // Act
    const result = pipe.transform('DealTypes')

    // Assert
    expect(result).toBe('Deal Types')
  })

  it('should split ComplexPascalCased string into separate words', () => {
    // Arrange
    const pipe = new SplitPascalCasePipe()

    // Act
    const result = pipe.transform('ComplexPascalCased')

    // Assert
    expect(result).toBe('Complex Pascal Cased')
  })

  it('Should do nothing if the phrase is already formatted', () => {
    // Arrange
    const pipe = new SplitPascalCasePipe()

    // Act
    const result = pipe.transform('This is a sentence')

    // Assert
    expect(result).toBe('This is a sentence')
  })
})

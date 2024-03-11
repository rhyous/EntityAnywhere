import { RemoveHyphensPipe } from './remove-hyphens.pipe'

describe('RemoveHyphensPipe', () => {
  it('create an instance', () => {
    const pipe = new RemoveHyphensPipe()
    expect(pipe).toBeTruthy()
  })

  it('should remove the hyphens', () => {
    // Arrange
    const pipe = new RemoveHyphensPipe()

    // Act
    const result = pipe.transform('Data-Administration')

    // Assert
    expect(result).toBe('Data Administration')
  })

  it('should replace all the hyphens', () => {
    // Arrange
    const pipe = new RemoveHyphensPipe()

    // Act
    const result = pipe.transform('my-hyphenated-string')

    // Assert
    expect(result).toBe('my hyphenated string')
  })

  it('should do nothing if there are no hyphens', () => {
    // Arrange
    const pipe = new RemoveHyphensPipe()

    // Act
    const result = pipe.transform('admin')

    // Assert
    expect(result).toBe('admin')
  })
})

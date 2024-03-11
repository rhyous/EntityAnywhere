import { FormatVersionForSortingPipe } from './format-version-for-sorting.pipe'

describe('RemoveHyphensPipe', () => {
  it('create an instance', () => {
    const pipe = new FormatVersionForSortingPipe()
    expect(pipe).toBeTruthy()
  })

  it('should format the version ready for sorting', () => {
    // Arrange
    const pipe = new FormatVersionForSortingPipe()

    // Act
    const result = pipe.transform('11.0.3')

    // Assert
    expect(result).toBe('100011.100000.100003')
  })

  it('should return 000000 if value is null', () => {
    // Arrange
    const pipe = new FormatVersionForSortingPipe()

    // Act
    const result = pipe.transform(<any>null)

    // Assert
    expect(result).toBe('000000')
  })
})

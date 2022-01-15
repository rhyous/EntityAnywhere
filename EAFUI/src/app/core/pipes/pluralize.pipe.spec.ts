import { PluralizePipe } from './pluralize.pipe'

describe('Singularize Pipe', () => {
  it('create an instance', () => {
    const pipe = new PluralizePipe()
    expect(pipe).toBeTruthy()
  })

  it('should handle products', () => {
    const pipe = new PluralizePipe()
    const transformValue = pipe.transform('Product')

    expect(transformValue).toEqual('Products')
  })

  it('should handle addenda', () => {
    const pipe = new PluralizePipe()
    const transformValue = pipe.transform('Addendum')

    expect(transformValue).toEqual('Addenda')
  })

  it('should handle entities', () => {
    const pipe = new PluralizePipe()
    const transformValue = pipe.transform('Entity')

    expect(transformValue).toEqual('Entities')
  })

  it('should handle churches', () => {
    const pipe = new PluralizePipe()
    const transformValue = pipe.transform('Church')

    expect(transformValue).toEqual('Churches')

  })

  it('should handle a word ending with s', () => {
    // Arrange
    const pipe = new PluralizePipe()

    // Act
    const transform = pipe.transform('Miss')

    // Assert
    expect(transform).toEqual('Misses')
  })

  it('should handle a word ending with sh', () => {
    // Arrange
    const pipe = new PluralizePipe()

    // Act
    const transform = pipe.transform('Push')

    // Assert
    expect(transform).toEqual('Pushes')
  })

  it('should handle a word ending with x', () => {
    // Arrange
    const pipe = new PluralizePipe()

    // Act
    const transform = pipe.transform('Mix')

    // Assert
    expect(transform).toEqual('Mixes')
  })

  it('should handle a word ending with z', () => {
    // Arrange
    const pipe = new PluralizePipe()

    // Act
    const transform = pipe.transform('Buzz')

    // Assert
    expect(transform).toEqual('Buzzes')
  })
})

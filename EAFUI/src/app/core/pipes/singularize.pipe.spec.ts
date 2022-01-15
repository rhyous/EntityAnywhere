import { SingularizePipe } from './singularize.pipe'

describe('Singularize Pipe', () => {
  it('create an instance', () => {
    const pipe = new SingularizePipe()
    expect(pipe).toBeTruthy()
  })

  it('should handle products', () => {
    // Arange
    const pipe = new SingularizePipe()

    // Act
    const transformValue = pipe.transform('Products')

    // Assert
    expect(transformValue).toEqual('Product')
  })

  it('should handle addenda', () => {
    // Arrange
    const pipe = new SingularizePipe()

    // Act
    const transformValue = pipe.transform('Addenda')

    // Assert
    expect(transformValue).toEqual('Addendum')
  })

  it('should handle entities', () => {
    // Arrange
    const pipe = new SingularizePipe()

    // Act
    const transformValue = pipe.transform('Entities')

    // Assert
    expect(transformValue).toEqual('Entity')
  })

  it('should handle puppies', () => {
    // Arrange
    const pipe = new SingularizePipe()

    // Act
    const transformValue = pipe.transform('Puppies')

    // Assert
    expect(transformValue).toEqual('Puppy')
  })

  it('should handle churches', () => {
    // Arrange
    const pipe = new SingularizePipe()

    // Act
    const transformValue = pipe.transform('Churches')

    // Assert
    expect(transformValue).toEqual('Church')

  })

  it('should handle misses', () => {
    // Arrange
    const pipe = new SingularizePipe()

    // Act
    const transform = pipe.transform('Misses')

    // Assert
    expect(transform).toEqual('Miss')
  })

  it('should handle buzzes', () => {
    // Arrange
    const pipe = new SingularizePipe()

    // Act
    const transform = pipe.transform('Buzzes')

    // Assert
    expect(transform).toEqual('Buzz')
  })

  it('should handle mixes', () => {
    // Arrange
    const pipe = new SingularizePipe()

    // Act
    const transform = pipe.transform('Mixes')

    // Assert
    expect(transform).toEqual('Mix')
  })

})

import { CustomPluralizationMap } from '../models/concretes/custom-pluralization-map'
import { SingularizePipe } from './singularize.pipe'
import { SplitPascalCasePipe } from './split-pascal-case.pipe'

describe('Singularize Pipe', () => {
  // Arange
  const customPluralizationMap = new CustomPluralizationMap()
  const splitPascalCasePipe = new SplitPascalCasePipe()
  const pipe = new SingularizePipe(customPluralizationMap, splitPascalCasePipe)

  it('create an instance', () => {
    expect(pipe).toBeTruthy()
  })

  it('should handle products', () => {
    // Act
    const transformValue = pipe.transform('Products')

    // Assert
    expect(transformValue).toEqual('Product')
  })

  it('should handle addenda', () => {
    // Act
    const transformValue = pipe.transform('Addenda')

    // Assert
    expect(transformValue).toEqual('Addendum')
  })

  it('should handle entities', () => {
    // Act
    const transformValue = pipe.transform('Entities')

    // Assert
    expect(transformValue).toEqual('Entity')
  })

  it('should handle puppies', () => {
    // Act
    const transformValue = pipe.transform('Puppies')

    // Assert
    expect(transformValue).toEqual('Puppy')
  })

  it('should handle churches', () => {
    // Act
    const transformValue = pipe.transform('Churches')

    // Assert
    expect(transformValue).toEqual('Church')

  })

  it('should handle misses', () => {
    // Act
    const transform = pipe.transform('Misses')

    // Assert
    expect(transform).toEqual('Miss')
  })

  it('should handle buzzes', () => {
    // Act
    const transform = pipe.transform('Buzzes')

    // Assert
    expect(transform).toEqual('Buzz')
  })

  it('should handle mixes', () => {
    // Act
    const transform = pipe.transform('Mixes')

    // Assert
    expect(transform).toEqual('Mix')
  })

})

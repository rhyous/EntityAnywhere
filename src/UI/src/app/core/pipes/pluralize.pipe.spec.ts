import { CustomPluralizationMap } from '../models/concretes/custom-pluralization-map'
import { PluralizePipe } from './pluralize.pipe'
import { SplitPascalCasePipe } from './split-pascal-case.pipe'

describe('Singularize Pipe', () => {
  // Arrange
  const customPluralizationMap = new CustomPluralizationMap()
  const splitPascalCasePipe = new SplitPascalCasePipe()
  const pipe = new PluralizePipe(customPluralizationMap, splitPascalCasePipe)

  it('create an instance', () => {
    expect(pipe).toBeTruthy()
  })

  it('should handle products', () => {
    // Act
    const transformValue = pipe.transform('Product')

    // Assert
    expect(transformValue).toEqual('Products')
  })

  it('should handle addenda', () => {
    // Act
    const transformValue = pipe.transform('Addendum')

    // Act
    expect(transformValue).toEqual('Addenda')
  })

  it('should handle entities', () => {
    // Act
    const transformValue = pipe.transform('Entity')

    expect(transformValue).toEqual('Entities')
  })

  it('should handle churches', () => {
    // Act
    const transformValue = pipe.transform('Church')

    expect(transformValue).toEqual('Churches')

  })

  it('should handle a word ending with s', () => {
    // Act
    const transform = pipe.transform('Miss')

    // Assert
    expect(transform).toEqual('Misses')
  })

  it('should handle a word ending with sh', () => {
    // Act
    const transform = pipe.transform('Push')

    // Assert
    expect(transform).toEqual('Pushes')
  })

  it('should handle a word ending with x', () => {
    // Act
    const transform = pipe.transform('Mix')

    // Assert
    expect(transform).toEqual('Mixes')
  })

  it('should handle a word ending with z', () => {
    // Act
    const transform = pipe.transform('Buzz')

    // Assert
    expect(transform).toEqual('Buzzes')
  })
})

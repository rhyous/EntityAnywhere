import { Component } from '@angular/core'
import { EntityHelperService } from './entity-helper.service'

describe('Helper', () => {
  const service = new EntityHelperService()
  it('should create an instance', () => {
    expect(service).toBeTruthy()
  })

  it('getOrderByString should be able to create the correct filter string', () => {
    // Arrange

    // Act
    const result = service.getOrderByString({
      direction: 'asc',
      propertyName: 'MyProperty'
    })

    // Assert
    expect(result).toBe('&$orderBy=MyProperty asc')
  })

  it('getOrderByString should handle if the direction isnt what we think it will be', () => {
    // Arrange

    // Act
    const result = service.getOrderByString({
      direction: <'asc' | 'desc'>'dorigible',
      propertyName: 'MyProperty'
    })

    // Assert
    expect(result).toBe('')
  })

})

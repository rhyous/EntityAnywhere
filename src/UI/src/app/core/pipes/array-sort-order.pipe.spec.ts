import { ArraySortOrderPipe } from './array-sort-order.pipe'

describe('ArraySortOrderPipe', () => {
  it('create an instance', () => {
    const pipe = new ArraySortOrderPipe()
    expect(pipe).toBeTruthy()
  })

  it('Should sort an array', () => {
    const pipe = new ArraySortOrderPipe()
    let array = [{name: 'Test 1', order: 3},
                  {name: 'Test 2', order: 2},
                  {name: 'Test 3', order: 6},
                  {name: 'Test 4', order: 1},
                  {name: 'Test 5', order: 1}]
    array = pipe.transform(array)
    expect(array[0].order).toEqual(1)
    expect(array[1].name).toEqual('Test 5')
  })

  it ('Should throw an error if no order field present', () => {
    const pipe = new ArraySortOrderPipe()
    const array = [{name: 'Test 1'}, {name: 'Test2'}]
    expect(() => {pipe.transform(array)}).toThrowError('Object requires Order field')
  })
})

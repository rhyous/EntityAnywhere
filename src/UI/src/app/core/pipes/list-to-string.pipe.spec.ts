import { ListToStringPipe } from './list-to-string.pipe'

describe('ListToStringPipe', () => {
  it('create an instance', () => {
    const pipe = new ListToStringPipe()
    expect(pipe).toBeTruthy()
  })

  it('should create a string from a list', () => {
    const pipe = new ListToStringPipe()
    const result = pipe.transform(['Test', 'Test1'])
    expect(result).toEqual('Test, Test1')
  })

  it('should return empty string if value is null', () => {
    const pipe = new ListToStringPipe()
    const result = pipe.transform(<any>null)
    expect(result).toEqual('')
  })

  it('should handle empty array', () => {
    const pipe = new ListToStringPipe()
    const result = pipe.transform([])
    expect(result).toEqual('')
  })
})

import { WcfFormatPipe } from './wcf-format.pipe'

describe('WcfFormatPipe', () => {
  it('create an instance', () => {
    const pipe = new WcfFormatPipe()
    expect(pipe).toBeTruthy()
  })

  it ('should handle null values', () => {
    const pipe = new WcfFormatPipe()

    const transformed = pipe.transform(null)
    expect(transformed).toEqual(null)
  })

  it ('should pass the value through', () => {
    const pipe = new WcfFormatPipe()

    const td = new Date('28-Feb-2018')
    const transformed = pipe.transform(td)
    expect(transformed).toEqual(`/Date(${td.getTime()}-00:00)/`)
  })

})

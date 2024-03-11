import { EntityCommonPipe } from './common.pipe'
import { PropertyTypePipe } from './property.type.pipe'
import { EntityPropertyPipe } from './entityproperty.pipe'

export class MockPipe extends PropertyTypePipe {
  public type: string

  constructor() {
    super()
    this.type = 'User'
    EntityCommonPipe.registerTypePipe(this)
  }

  override transform(value: any, ...args: any[]) {
    return 'Pipe Called'
  }

}

export class MockEntityPipe extends EntityPropertyPipe {
  public entity: string
  public property: string

  constructor() {
    super()
    this.entity = 'User'
    this.property = 'Username'
    EntityCommonPipe.registerEntityPipe(this)
  }

  override transform(value: any, ...args: any[]) {
    return 'EPipe Called'
  }

}

describe('Singularize Pipe', () => {

  const pipe = new EntityCommonPipe()

  it('create an instance', () => {
    expect(pipe).toBeTruthy()
    expect(pipe.myInstance).toEqual(1)
  })

  it('should register a property pipe correctly', () => {
    const mockPipe = new MockPipe()

    const transformed = pipe.transform('', 'User', '', 'User')
    expect(transformed).toEqual('Pipe Called')
  })

  it ('should pass the value through', () => {
    const mockPipe = new MockPipe()

    const transformed = pipe.transform('User')
    expect(transformed).toEqual('User')
  })

  it('should register an entity pipe correctly', () => {
    const mockPipe = new MockEntityPipe()

    const transformed = pipe.transform('', 'User', 'Username')
    expect(transformed).toEqual('EPipe Called')
  })

})


import { Subject } from 'rxjs'

export class ActivatedRouteStub {
  private _params = new Subject<any>()

  get params() {
      return this._params.asObservable()
  }

  public push(value: any) {
      this._params.next(value)
  }
}

import { Injectable } from '@angular/core'

@Injectable()
export class EnumOptionMapper {

  constructor() {}

  mapOptions(entityOptions: Map<number, string>): any {
    const options: any = []
    entityOptions.forEach((k, v) => {
      options.push({Id: v, Value: k})
    })
    return options
  }
}

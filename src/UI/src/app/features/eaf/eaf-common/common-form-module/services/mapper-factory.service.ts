import { Injectable, ComponentFactoryResolver } from '@angular/core'

import { MapperDialogComponent } from '../../common-dialogs-module/components/mapper-dialog/mapper-dialog.component'
// tslint:disable-next-line: max-line-length

@Injectable({
  providedIn: 'root'
})
export class MapperFactoryService {

  constructor() { }

  public getMapperDialog(mappingEntity: any): any {
    return MapperDialogComponent
  }
}

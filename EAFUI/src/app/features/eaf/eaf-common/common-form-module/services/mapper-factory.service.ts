import { Injectable, ComponentFactoryResolver } from '@angular/core'

import { MapperDialogComponent } from '../../common-dialogs-module/components/mapper-dialog/mapper-dialog.component'
// tslint:disable-next-line: max-line-length

@Injectable({
  providedIn: 'root'
})
export class MapperFactoryService {

  constructor() { }

  public getMapperDialog(mappingEntity): any {
    const mapperDialogs = []

    const dialog = mapperDialogs.find(x => x.Entity === mappingEntity)

    if (dialog) {
      return dialog.Class
    }

    return MapperDialogComponent
  }
}

import { TestBed, inject } from '@angular/core/testing'

import { MapperFactoryService } from './mapper-factory.service'
import { MapperDialogComponent } from '../../common-dialogs-module/components/mapper-dialog/mapper-dialog.component'
// tslint:disable-next-line: max-line-length

describe('MapperFactoryService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [MapperFactoryService]
    })
  })

  it('should be created', inject([MapperFactoryService], (service: MapperFactoryService) => {
    expect(service).toBeTruthy()
  }))

  it('should return a mapper dialog by default', () => {
    const mfs = new MapperFactoryService()
    const md = mfs.getMapperDialog('ProductMapper')
    expect(typeof(md)).toBe(typeof(MapperDialogComponent))
  })
})

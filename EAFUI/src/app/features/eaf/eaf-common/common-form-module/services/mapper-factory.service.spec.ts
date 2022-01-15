import { TestBed, inject } from '@angular/core/testing'

import { MapperFactoryService } from './mapper-factory.service'
import { MapperDialogComponent } from '../../common-dialogs-module/components/mapper-dialog/mapper-dialog.component'
import { SuiteMapperDialogComponent } from '../../../eaf-custom/dialogs/suite-mapper-dialog/suite-mapper-dialog.component'
// tslint:disable-next-line: max-line-length
import { UpgradeProductMapperDialogComponent } from '../../../eaf-custom/dialogs/upgrade-product-mapper-dialog/upgrade-product-mapper-dialog.component'

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

  it('should return a suite mapper dialog', () => {
    const mfs = new MapperFactoryService()
    expect(typeof(mfs.getMapperDialog('ProductsInSuite'))).toBe(typeof(SuiteMapperDialogComponent))
  })

  it('should return a suite mapper dialog2', () => {
    const mfs = new MapperFactoryService()
    expect(typeof(mfs.getMapperDialog('Suites'))).toBe(typeof(SuiteMapperDialogComponent))
  })

  it('should return a upgrade product mapper dialog', () => {
    const mfs = new MapperFactoryService()
    expect(typeof(mfs.getMapperDialog('UpgradeProducts'))).toBe(typeof(UpgradeProductMapperDialogComponent))
  })

  it('should return a upgrade product mapper dialog', () => {
    const mfs = new MapperFactoryService()
    expect(typeof(mfs.getMapperDialog('ProductsToUpgrade'))).toBe(typeof(UpgradeProductMapperDialogComponent))
  })
})

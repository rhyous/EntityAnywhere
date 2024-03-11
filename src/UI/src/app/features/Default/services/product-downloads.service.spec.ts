import { TestBed } from '@angular/core/testing'

import { ProductDownloadsService } from './product-downloads.service'

describe('ProductDownloadsService', () => {
  let service: ProductDownloadsService

  beforeEach(() => {
    TestBed.configureTestingModule({})
    service = TestBed.inject(ProductDownloadsService)
  });

  it('should be created', () => {
    expect(service).toBeTruthy()
  })
})

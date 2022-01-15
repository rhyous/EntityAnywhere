import { TestBed } from '@angular/core/testing'

import { FeatureFlagsService } from './feature-flags.service'

let service: FeatureFlagsService

describe('FeatureFlagsService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [
      FeatureFlagsService
    ]
  }))

  beforeEach(() => {
    service = TestBed.get(FeatureFlagsService)
  })

  it('should be created', () => {
    expect(service).toBeTruthy()
  })
})

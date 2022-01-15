import { TestBed } from '@angular/core/testing'

import { AppLocalStorageService } from './local-storage.service'

describe('LocalStorageService', () => {
  beforeEach(() => TestBed.configureTestingModule({}))

  it('should be created', () => {
    const service: AppLocalStorageService = TestBed.get(AppLocalStorageService)
    expect(service).toBeTruthy()
  })
})

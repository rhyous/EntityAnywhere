import { TestBed, flush, inject } from '@angular/core/testing'

import { DefaultService } from './default.service'
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing'
import { AppLocalStorageService } from './local-storage.service'

class FakeAppLocalStorageService {
  User = {
  }
  activeToken: 'THISISMYTOKENTHEREAREMANYTOKENSLIKEITBUTTHISONEISMINE'
}

describe('DefaultService', () => {

  let service: DefaultService

  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      HttpClientTestingModule
    ],
    providers: [
      { provide: AppLocalStorageService, useClass: FakeAppLocalStorageService }
    ]
  }))

  beforeEach(() => {
    service = TestBed.get(DefaultService)
  })

  it('should be created', () => {
    expect(service).toBeTruthy()
  })
})

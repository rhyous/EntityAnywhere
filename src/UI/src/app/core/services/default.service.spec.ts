import { TestBed } from '@angular/core/testing'

import { DefaultService } from './default.service'
import { HttpClientTestingModule } from '@angular/common/http/testing'
import { AppLocalStorageService } from './local-storage.service'
import { HttpClient } from '@angular/common/http'

class FakeAppLocalStorageService {
  User = {
  }
  activeToken!: 'THISISMYTOKENTHEREAREMANYTOKENSLIKEITBUTTHISONEISMINE'
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
    service = TestBed.inject(DefaultService)
  })

  it('should be created', () => {
    expect(service).toBeTruthy()
  })
})

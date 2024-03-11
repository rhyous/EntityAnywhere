import { TestBed } from '@angular/core/testing'

import { AdminDataService } from './admin-data.service'
import { HttpClientTestingModule } from '@angular/common/http/testing'
import { HttpClient } from '@angular/common/http'
import { of } from 'rxjs'
import { environment } from 'src/environments/environment'
import { AppLocalStorageService } from 'src/app/core/services/local-storage.service'
import { FakeAppLocalStorageService } from 'src/app/core/services/mocks/mocks'

describe('AdminDataService', () => {
  const token = {headers: {token: 'THISISMYTOKENTHEREAREMANYTOKENSLIKEITBUTTHISONEISMINE'}}

  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      HttpClientTestingModule
    ],
    providers: [
      { provide: AppLocalStorageService, useClass: FakeAppLocalStorageService },
    ]
  }))

  it('should be created', () => {
    const service: AdminDataService = TestBed.inject(AdminDataService)
    expect(service).toBeTruthy()
  })
})


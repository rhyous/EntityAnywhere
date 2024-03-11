import { TestBed } from '@angular/core/testing'

import { DefaultDataService } from './default-data.service'
import { GlobalMatDialogService } from './global-mat-dialog.service'
import { RouterTestingModule } from '@angular/router/testing'
import { MatDialogModule } from '@angular/material/dialog'

describe('DefaultDataService', () => {
  let service: DefaultDataService
  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      MatDialogModule,
      RouterTestingModule
    ],
    providers: [
      GlobalMatDialogService,
    ]
  }))

  beforeEach(()  => {
    service = TestBed.inject(DefaultDataService)
  })

  it('should be created', () => {
    expect(service).toBeTruthy()
  })

  it('should clear all data on logout', () => {
    // Arrange
    // Act
    service.logout()

    // Assert
  })
})

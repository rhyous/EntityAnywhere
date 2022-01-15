import { TestBed } from '@angular/core/testing'

import { DefaultDataService } from './default-data.service'
import { GlobalMatDialogService } from './global-mat-dialog.service'
import { RouterTestingModule } from '@angular/router/testing'
import { DefaultDataServiceFakeData } from './default-data.service.specdata'
import { MatDialogModule } from '@angular/material'
import { ODataObject } from '../models/interfaces/o-data-entities/o-data-object.interface'
import { ODataCollection } from '../models/interfaces/o-data-collection.interface'

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
    service = TestBed.get(DefaultDataService)
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

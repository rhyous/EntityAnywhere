import { TestBed } from '@angular/core/testing'

import { GlobalSnackBarService } from './global-snack-bar.service'
import { MatSnackBar } from '@angular/material'
import { Overlay } from '@angular/cdk/overlay'
import { environment } from 'src/environments/environment'

describe('GlobalSnackBarService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [MatSnackBar, Overlay]
  }))

  it('should be created', () => {
    const service: GlobalSnackBarService = TestBed.get(GlobalSnackBarService)
    expect(service).toBeTruthy()
  })

  it('should call the underlying snackbar with the default data', () => {
    // Arrange
    const service: GlobalSnackBarService = TestBed.get(GlobalSnackBarService)
    const snackbar: MatSnackBar = TestBed.get(MatSnackBar)
    spyOn(snackbar, 'open')

    // Act
    service.open('This is my message to you')

    // Assert
    expect(snackbar.open).toHaveBeenCalledWith('This is my message to you',
    null,
    {duration: environment.snackBarDuration})
  })
})

import { TestBed } from '@angular/core/testing'

import { GlobalSnackBarService } from './global-snack-bar.service'
import { MatSnackBar } from '@angular/material/snack-bar'
import { Overlay } from '@angular/cdk/overlay'
import { environment } from 'src/environments/environment'

describe('GlobalSnackBarService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    providers: [MatSnackBar, Overlay]
  }))

  it('should be created', () => {
    const service: GlobalSnackBarService = TestBed.inject(GlobalSnackBarService)
    expect(service).toBeTruthy()
  })

  it('should call the underlying snackbar with the default data', () => {
    // Arrange
    const service: GlobalSnackBarService = TestBed.inject(GlobalSnackBarService)
    const snackbar: MatSnackBar = TestBed.inject(MatSnackBar)
    spyOn(snackbar, 'open')

    // Act
    service.open('This is my message to you')

    // Assert
    expect(snackbar.open).toHaveBeenCalledWith('This is my message to you',
    <any>null,
    {duration: environment.snackBarDuration})
  })
})

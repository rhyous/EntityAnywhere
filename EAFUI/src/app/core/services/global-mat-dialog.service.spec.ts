import { TestBed } from '@angular/core/testing'

import { GlobalMatDialogService } from './global-mat-dialog.service'
import { ConfirmDialogComponent } from '../dialogs/confirm-dialog/confirm-dialog.component'
import { LicenseInformationDialogData } from '../dialogs/license-information-dialog/license-information-dialog-data.interface'
import { LicenseInformationDialogComponent } from '../dialogs/license-information-dialog/license-information-dialog.component'
import { MatDialogModule } from '@angular/material'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { BrowserDynamicTestingModule } from '@angular/platform-browser-dynamic/testing'

describe('GlobalMatDialogService', () => {
  let service: GlobalMatDialogService
  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      MatDialogModule,
      BrowserAnimationsModule
    ],
    declarations: [
      ConfirmDialogComponent
    ]
  })
  .overrideModule(BrowserDynamicTestingModule, {set: {entryComponents: [ConfirmDialogComponent]} }))

  beforeEach(() => {
    service = TestBed.get(GlobalMatDialogService)
    spyOn(service, 'open')
    spyOn(service, 'openTyped')
  })

  it('should be created', () => {
    expect(service).toBeTruthy()
  })

  it('should open the confirm dialog', () => {
    // Arrange
    const data = {
      confirm: true,
      message: 'My message',
      title: 'My title'
    }
    // Act
    service.openConfirmDialog(data)

    // Assert
    expect(service.openTyped).toHaveBeenCalledWith(ConfirmDialogComponent, data)
  })

  it('should open the license info dialog', () => {
    // Arrange
    const data: LicenseInformationDialogData =  {
      ActivationCode: '123',
      LicenseKey: 'LicenseKey',
      LicenseType: 'TypeA',
      ProductName: 'My product',
      Version: '1.0',
      FileExt: '.lic'
    }

    // Act
    service.openLicenseInformationDialog(data)


    // Assert
    expect(service.openTyped).toHaveBeenCalledWith(LicenseInformationDialogComponent, data)
  })
})

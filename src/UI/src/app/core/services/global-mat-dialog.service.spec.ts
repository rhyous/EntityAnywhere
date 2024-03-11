import { TestBed } from '@angular/core/testing'

import { GlobalMatDialogService } from './global-mat-dialog.service'
import { ConfirmDialogComponent } from '../dialogs/confirm-dialog/confirm-dialog.component'
import { MatDialogModule } from '@angular/material/dialog'
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
    service = TestBed.inject(GlobalMatDialogService)
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
})

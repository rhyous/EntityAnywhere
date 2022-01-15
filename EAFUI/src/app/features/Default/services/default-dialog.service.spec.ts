import { TestBed } from '@angular/core/testing'

import { DefaultDialogService } from './default-dialog.service'
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material'
import { of } from 'rxjs'
import { MaterialModule } from 'src/app/core/material/material.module'

class DialogMock {
  close(returnValue: any) {
    return {
      afterClosed: () => of(returnValue)
    }
  }
}
describe('DefaultDialogService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      MaterialModule
    ],
    providers: [
      {provide: MatDialogRef, useClass: DialogMock},
      {provide: MAT_DIALOG_DATA, useValue:  {data: {title: 'Confirm Test', message: 'Please confirm this test', confirm: true}} },
    ]
  }))

  it('should be created', () => {
    const service: DefaultDialogService = TestBed.get(DefaultDialogService)
    expect(service).toBeTruthy()
  })
})

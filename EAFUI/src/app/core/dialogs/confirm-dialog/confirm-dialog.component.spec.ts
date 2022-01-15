import { async, TestBed, ComponentFixture } from '@angular/core/testing'
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material'
import { By } from '@angular/platform-browser'
import { ConfirmDialogComponent } from './confirm-dialog.component'
import { of } from 'rxjs'
import { ErrorReporterDialogComponent } from '../error-reporter-dialog/error-reporter-dialog.component'

class DialogMock {
  close(returnValue: any) {
    return {
      afterClosed: () => of(returnValue)
    }
  }
}

describe('ConfirmDialogComponent no confirm tests', () => {
  let component: ConfirmDialogComponent
  let fixture: ComponentFixture<ConfirmDialogComponent>

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ ],
      providers: [
       {provide: MatDialogRef, useClass: DialogMock},
       {provide: MAT_DIALOG_DATA, useValue:  {data: {title: 'Confirm Test', message: 'Please confirm this test', confirm: true}} },
       ErrorReporterDialogComponent,
      ],
      declarations: [ ConfirmDialogComponent ]
    })
    .compileComponents()
  }))

  beforeEach(() => {

    fixture = TestBed.createComponent(ConfirmDialogComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it ('Should create', () => {
    const de = fixture.debugElement.queryAll(By.css('button'))
    expect(de.length).toEqual(2) // There should be two buttons form confirm
    expect(component).toBeTruthy()
  })

  it ('Should close with a confirm', () => {
    spyOn(component.dialogRef, 'close')
    component.onConfirm()
    expect(component.dialogRef.close).toHaveBeenCalledWith('confirmed')
  })

  it ('Should close with no message on no click', () => {
    spyOn(component.dialogRef, 'close')
    component.onNoClick()
    expect(component.dialogRef.close).toHaveBeenCalledWith('cancelled')
  })

})

describe('ConfirmDialogComponent Tests', () => {
  let component: ConfirmDialogComponent
  let fixture: ComponentFixture<ConfirmDialogComponent>

  const fakeData = {
    data: {
      title: 'Confirm Test',
      message: 'You dont need to click confirm',
      confirm: false
    }
  }

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ ],
      providers: [
        { provide: MatDialogRef, useClass: DialogMock},
        { provide: MAT_DIALOG_DATA, useValue: fakeData },
        ErrorReporterDialogComponent,
      ],
      declarations: [ ConfirmDialogComponent ]
    })
    .compileComponents()
  }))

  beforeEach(() => {

    fixture = TestBed.createComponent(ConfirmDialogComponent)
    component = fixture.componentInstance
    component.confirmation = false
    fixture.detectChanges()
  })

  it ('Should create', () => {
    const de = fixture.debugElement.queryAll(By.css('button'))

    expect(de.length).toEqual(1) // There should be one buttons for noconfirm
    expect(component.confirmation).toEqual(false)
    expect(component).toBeTruthy()
  })

})


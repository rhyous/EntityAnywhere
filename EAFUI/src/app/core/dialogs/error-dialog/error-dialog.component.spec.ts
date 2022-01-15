import { async, TestBed, ComponentFixture } from '@angular/core/testing'
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material'
import { By } from '@angular/platform-browser'
import { ErrorDialogComponent } from './error-dialog.component'
import { of } from 'rxjs'

class DialogMock {
  close(returnValue: any) {
    return {
      afterClosed: () => of(returnValue)
    }
  }
}

describe('ErrorDialogComponent no confirm tests', () => {
  let component: ErrorDialogComponent
  let fixture: ComponentFixture<ErrorDialogComponent>

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ ],
      providers: [
       {provide: MatDialogRef, useClass: DialogMock},
       {provide: MAT_DIALOG_DATA, useValue:  {data: {title: 'Confirm Test', message: 'Please confirm this test'}} }],
      declarations: [ ErrorDialogComponent ]
    })
    .compileComponents()
  }))

  beforeEach(() => {

    fixture = TestBed.createComponent(ErrorDialogComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it ('Should create', () => {
    const de = fixture.debugElement.queryAll(By.css('button'))
    expect(de.length).toEqual(1) // There should be two buttons form confirm
    expect(component).toBeTruthy()
  })

  it ('Should close with no message on no click', () => {
    spyOn(component.dialogRef, 'close').and.returnValue()
    component.onNoClick()
    expect(component.dialogRef.close).toHaveBeenCalledWith()
  })

})



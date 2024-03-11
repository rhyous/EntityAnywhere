import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing'

import { MAT_DIALOG_DATA, MatDialogRef, MatDialogConfig } from '@angular/material/dialog'
import { ErrorReporterDialogComponent } from './error-reporter-dialog.component'
import { ErrorResponse } from '../../models/interfaces/error-response.interface'
import { environment } from 'src/environments/environment'
import { MessageDialogComponent } from '../message-dialog/message-dialog.component'
import { MaterialModule } from '../../material/material.module'

describe('ErrorReporterComponent', () => {
  let component: ErrorReporterDialogComponent
  let fixture: ComponentFixture<ErrorReporterDialogComponent>

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [ MaterialModule ],
      providers: [
        { provide: MatDialogRef, useValue: [] },
        { provide: MAT_DIALOG_DATA, useValue: [] } ],
      declarations: [ ErrorReporterDialogComponent ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(ErrorReporterDialogComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })

  it('should call a snackbar', () => {
    const errorResponse: ErrorResponse = {
      Acknowledgeable: false,
      Message: 'This is a message',
      Data: [],
      Source: '',
      Type: ''
    }
    spyOn(component.snackBar, 'open')
    component.displayMessage(errorResponse)

    expect(component.snackBar.open).toHaveBeenCalledWith(`This is a message`, <any>undefined, { duration: environment.snackBarDuration })
  })

  it('should call a dialog', () => {
    const errorResponse: ErrorResponse = {
      Acknowledgeable: true,
      Message: 'This is a message',
      Data: [ {
        Key: 'Organization must have licenses',
        Value: {_type: 'BusinessRulesResult', FailedObjects: ['Requested:6', 'Available:5'],
        Result: false}
      }],
      Source: '',
      Type: ''
    }

    const config = new MatDialogConfig()
    config.width = '800px'
    config.disableClose = true
    config.autoFocus = true
    config.data = errorResponse

    spyOn(component.dialog, 'open')
    component.displayMessage(errorResponse)
    expect(component.dialog.open).toHaveBeenCalledWith(MessageDialogComponent, config)
  })
})

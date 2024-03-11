import { Component, OnInit } from '@angular/core'
import { MatDialog, MatDialogConfig } from '@angular/material/dialog'
import { MatSnackBar } from '@angular/material/snack-bar'
import { ErrorResponse } from '../../models/interfaces/error-response.interface'
import { environment } from 'src/environments/environment'
import { MessageDialogComponent } from '../message-dialog/message-dialog.component'

@Component({
  selector: 'app-error-reporter-dialog',
  templateUrl: './error-reporter-dialog.component.html'
})
export class ErrorReporterDialogComponent implements OnInit {

  constructor(public snackBar: MatSnackBar,
              public dialog: MatDialog) {
  }

  ngOnInit() {
  }

  displayMessage(errorResponse: ErrorResponse | Blob) {
    if (errorResponse && errorResponse instanceof Blob === false && (<ErrorResponse>errorResponse).Acknowledgeable) {
      this.displayDialog(<ErrorResponse>errorResponse)
    } else {
      this.displaySnackbar(errorResponse)
    }
  }

  displayDialog(errorResponse: ErrorResponse) {
    const dialogConfig = new MatDialogConfig()

    dialogConfig.disableClose = true
    dialogConfig.autoFocus = true
    dialogConfig.data = errorResponse
    dialogConfig.width = '800px'

    this.dialog.open(MessageDialogComponent, dialogConfig)
  }

  displaySnackbar(errorResponse: ErrorResponse | Blob) {
    // So if an error happens when making a call for a Blob (file) then Angular decides to be really helpful
    // and not decide that the error response will be a normal error response but will be a blob
    // For some strange reason an Error response will be a Blob.
    // This will then read from the Blob and turn it back into an object for us
    if (errorResponse instanceof Blob) {
      const fr = new FileReader()
      fr.onload = () => {
        const result: ErrorResponse = JSON.parse(<string>fr.result)
        this.snackBar.open(result.Message, undefined, {duration: environment.snackBarDuration})
      }

      fr.readAsText(errorResponse)
    } else {
      this.snackBar.open(errorResponse.Message, undefined, {duration: environment.snackBarDuration})
    }
  }

}

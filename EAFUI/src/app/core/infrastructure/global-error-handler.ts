import { ErrorHandler, Injectable, Injector, NgZone } from '@angular/core'
import { HttpErrorResponse } from '@angular/common/http'
import { ErrorReporterDialogComponent } from '../dialogs/error-reporter-dialog/error-reporter-dialog.component'
import { GlobalSnackBarService } from '../services/global-snack-bar.service'

@Injectable()
/** A global error handler. Responsible for handling errors relating to the application */
export class GlobalErrorHandler extends ErrorHandler {
  private snackBar: GlobalSnackBarService
  private errorReporter: ErrorReporterDialogComponent

  constructor(private injector: Injector, private ngZone: NgZone) {
    super()
    // Because this is instantiated as a provider it happens out of the ngZone, we have to get the services like this
    this.snackBar = this.injector.get(GlobalSnackBarService)
    this.errorReporter = this.injector.get(ErrorReporterDialogComponent)
  }

  handleError(error: any): void {
    this.ngZone.run(() => {
      if (error instanceof HttpErrorResponse) {
        // do nothing. this should be handled by the HttpInterceptor
      } else {
        // Log everything to the console to make it easy to bug fix
        console.log(error)
        if (error.error) {
          // send it to the common control
          this.errorReporter.displayMessage(error.error)
        } else {
          // Send it to the common snackbar
          this.snackBar.open(`Application error: ${error.message}`)
        }
      }
    })
  }
}

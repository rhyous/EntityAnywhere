import { Injectable, Injector, NgZone } from '@angular/core'
import { HttpRequest, HttpHandler, HttpEvent, HttpResponse } from '@angular/common/http'
import { Observable, throwError } from 'rxjs'
import { catchError, tap, finalize } from 'rxjs/operators'
import { ErrorReporterDialogComponent } from '../dialogs/error-reporter-dialog/error-reporter-dialog.component'
import { Router } from '@angular/router'
import { GlobalSnackBarService } from '../services/global-snack-bar.service'
import { AppLocalStorageService } from '../services/local-storage.service'

@Injectable()
/** Http Interceptor. Responsible for logging http errors and kicking the user out if they are unauthorized */
export class HttpInterceptor implements HttpInterceptor {
    private snackBar: GlobalSnackBarService

    private errorReporter: ErrorReporterDialogComponent

    private router: Router

    private localStorage: AppLocalStorageService

    constructor(private inject: Injector, private ngZone: NgZone) {
        // Because this is instantiated as a provider it happens out of the ngZone, we have to get the services like this
        this.snackBar = this.inject.get(GlobalSnackBarService)
        this.errorReporter = this.inject.get(ErrorReporterDialogComponent)
        this.router = this.inject.get(Router)
        this.localStorage = this.inject.get(AppLocalStorageService)
    }

    /** Intercept http responses */
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const started = Date.now()
        let message: string
        let status = 200

        return next.handle(req).pipe(
            tap(
                // Tap into the event so we know what to send to the Application Monitoring Service
                event => {
                    if (event instanceof HttpResponse) {
                        message = 'succeeded'
                        status = event.status
                    }
                },
                error => {
                    if (error && error.error && error.error.Message) {
                        message = `${error.message}: ${error.error.Message}`
                    } else {
                        message = error.message
                    }
                    status = error.status
                }),
            finalize(() => {
                const elapsed = Date.now() - started
                 // Please dont change this as it is tied to the AI event
                let msg = `ApiRequestDuration`
                const obj = {
                    url: req.url,
                    method: req.method,
                    durationms: elapsed,
                    message: message,
                    status: status,
                    date: new Date().toISOString()
                }

                // Track excessive requests with a separate event so we can analyse that user's performance
                if (elapsed > 5000) {
                    msg = 'ExcessiveRequestDuration'
                    const id = this.localStorage.User ? this.localStorage.User.UserId.Value : 0
                    const excessiveReqObj = {
                        url: req.url,
                        method: req.method,
                        durationms: elapsed,
                        message: message,
                        status: status,
                        date: new Date().toISOString(),
                        id: id
                    }
                }
            }),
            catchError(httpError => {
                this.ngZone.run(() => {
                    if (httpError.status > 0) { // Error status of 0 indicates that the API cannot be reached.
                        if (httpError.status === 400 || httpError.status === 401 || httpError.status === 403) {
                            this.localStorage.removeClaims()
                            this.router.navigate([''])
                            if (httpError && httpError.error) {
                                this.snackBar.open(httpError.error.Message)
                            }
                        } else if (httpError.error !== null || httpError.error instanceof Blob || httpError.error.Message !== undefined) {
                            this.errorReporter.displayMessage(httpError.error)
                        } else { // Handle it with a generic snackbar
                         this.snackBar.open(httpError.message)
                        }
                        if (httpError.status >= 400 && httpError.status < 500) {
                            // Log error
                        }
                        if (httpError.status >= 500) {
                            // Log error
                        }
                    }
                })
                // Let the error perpetuate up the chain so that
                // other services can make use of it
                return throwError(httpError)
            })
        )
    }
}

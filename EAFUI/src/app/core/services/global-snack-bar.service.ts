import { Injectable } from '@angular/core'
import { MatSnackBar, SimpleSnackBar, MatSnackBarRef } from '@angular/material'
import { environment } from 'src/environments/environment'
import { Queue } from '../infrastructure/queue'

@Injectable({
  providedIn: 'root'
})
/** Exposes Common Snackbar functionality for this entire application */
export class GlobalSnackBarService {

  constructor(private snackBar: MatSnackBar) { }

  private snackBarQueue: Queue<string> = new Queue<string>()

  /** Open a snackbar with a message, no actions and the default sitewide snackbar duration */
  open(message: string, duration: number = null): MatSnackBarRef<SimpleSnackBar> {
    const snackBarDuration = duration ? duration : environment.snackBarDuration
    return this.snackBar.open(message, null, {duration: snackBarDuration})
  }
}

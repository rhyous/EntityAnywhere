import { Injectable, TemplateRef } from '@angular/core'
import { MatDialog, MatDialogRef } from '@angular/material'
import { ConfirmDialogData } from '../dialogs/confirm-dialog/confirm-dialog-data.interface'
import { ConfirmDialogComponent } from '../dialogs/confirm-dialog/confirm-dialog.component'
// tslint:disable-next-line: max-line-length

@Injectable({
  providedIn: 'root'
})
export class GlobalMatDialogService {

  constructor(private matDialog: MatDialog) { }

  open<T>(component, data: T) {
    return this.matDialog.open(component, {
      width: '50%',
      height: 'auto',
      data: data
    })
  }

  openTyped<TData, TComponent, TReturn>(component, data: TData): MatDialogRef<TComponent, TReturn> {
    return this.matDialog.open(component, {
      width: '50%',
      height: 'auto',
      data: data
    })
  }

  openConfirmDialog(data: ConfirmDialogData) {
    return this.openTyped<ConfirmDialogData, ConfirmDialogComponent, 'confirmed' | 'cancelled'>(ConfirmDialogComponent, data)
  }
}

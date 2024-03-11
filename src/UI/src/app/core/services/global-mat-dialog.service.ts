import { Injectable, TemplateRef } from '@angular/core'
import { MatDialog, MatDialogRef } from '@angular/material/dialog'
import { ConfirmDialogData } from '../dialogs/confirm-dialog/confirm-dialog-data.interface'
import { ConfirmDialogComponent } from '../dialogs/confirm-dialog/confirm-dialog.component'

@Injectable({
  providedIn: 'root'
})
export class GlobalMatDialogService {

  constructor(private matDialog: MatDialog) { }

  open<T>(component: any, data: T) {
    return this.matDialog.open(component, {
      width: '50%',
      height: 'auto',
      data: data
    })
  }

  openTyped<TData, TComponent, TReturn>(component: any, data: TData): MatDialogRef<TComponent, TReturn> {
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

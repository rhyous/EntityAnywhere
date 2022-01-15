import { Component, Inject } from '@angular/core'
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material'
import { ConfirmDialogData } from './confirm-dialog-data.interface'

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: 'confirm-dialog.component.html',
})
export class ConfirmDialogComponent {

  confirmation = true

  constructor(
    public dialogRef: MatDialogRef<ConfirmDialogComponent, 'confirmed' | 'cancelled'>,
    @Inject(MAT_DIALOG_DATA) public data: ConfirmDialogData) {
      if (data.confirm !== undefined) {
        this.confirmation = data.confirm
      }
     }

  onConfirm(): void {
    this.dialogRef.close('confirmed')
  }

  onNoClick(): void {
    this.dialogRef.close('cancelled')
  }
}

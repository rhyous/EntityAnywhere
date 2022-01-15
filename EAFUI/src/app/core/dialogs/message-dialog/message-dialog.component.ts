import { Component, OnInit, Inject } from '@angular/core'
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material'

@Component({
  selector: 'app-message-dialog',
  templateUrl: './message-dialog.component.html',
  styleUrls: ['./message-dialog.component.scss']
})
export class MessageDialogComponent implements OnInit {
  messageHeader: string
  messageData: []
  tabId: number

  constructor(public dialogRef: MatDialogRef<MessageDialogComponent>,
              @Inject(MAT_DIALOG_DATA) data) {
      this.messageHeader = data.Message
      this.messageData = data.Data
      this.tabId = 0
    }

  ngOnInit() {
  }

  getTabLabel(index) {
    return `Message ${index + 1}`
  }
  closeDialog() {
    this.dialogRef.close()
  }
}

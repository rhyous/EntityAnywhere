import { Injectable } from '@angular/core'
import { GlobalMatDialogService } from 'src/app/core/services/global-mat-dialog.service'

@Injectable({
  providedIn: 'root'
})
export class DefaultDialogService {

  constructor(private matDialog: GlobalMatDialogService) { }


}

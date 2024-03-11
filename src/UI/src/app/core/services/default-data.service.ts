import { Injectable, EventEmitter, ɵConsole } from '@angular/core'
import { Router } from '@angular/router'
// tslint:disable-next-line: max-line-length
import { GlobalMatDialogService } from './global-mat-dialog.service'
import { AppLocalStorageService } from './local-storage.service'


@Injectable({
  providedIn: 'root'
})
/** This service is responsible for storing data about the Default */
export class DefaultDataService {
  constructor(private matDialog: GlobalMatDialogService,
              private router: Router,
              private localStorageService: AppLocalStorageService) {
  }

  onLogout: EventEmitter<boolean> = new EventEmitter<boolean>()

  logout() {
    this.onLogout.emit(true)
  }
}

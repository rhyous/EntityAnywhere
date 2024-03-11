import { Component, OnInit } from '@angular/core'
import { AppLocalStorageService } from '../services/local-storage.service'

@Component({
  selector: 'app-cookie-banner',
  templateUrl: './cookie-banner.component.html',
  styleUrls: ['./cookie-banner.component.scss']
})
export class CookieBannerComponent implements OnInit {
  constructor(private appStorage: AppLocalStorageService) { }
  ngOnInit(): void {
    const today = new Date()
    const aMonthAgo = new Date(today.getFullYear(), today.getMonth() - 1, today.getDate())
    const consent = this.appStorage.cookieConsent
    if (consent) {
      this.hasConsent = consent > aMonthAgo
    }
  }

  hasConsent = false
  consent() {
    this.appStorage.cookieConsent = new Date()
    this.hasConsent = true
  }
}

import { Component, Input, OnInit } from '@angular/core'
import { LoginService } from '../login/login.service'

@Component({
  selector: 'app-impersonate-customer-banner',
  templateUrl: './impersonate-customer-banner.component.html',
  styleUrls: ['./impersonate-customer-banner.component.scss']
})
export class ImpersonateCustomerBannerComponent implements OnInit {
  @Input() username!: string

  constructor(
    private loginService: LoginService
  ) { }

  ngOnInit() {
  }

  switchBack() {
    this.loginService.logout()
    return
  }

}

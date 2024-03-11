import { Component, OnInit } from '@angular/core'
import { Router } from '@angular/router'
import { UserDataService } from '../services/user-data.service'
import { AppLocalStorageService } from '../services/local-storage.service'
import { LoginService } from '../login/login.service'
import { LandingPageTypes } from '../models/enums/LandingPageTypes'

@Component({
  selector: 'app-sidebar-menu',
  templateUrl: './sidebar-menu.component.html',
  styleUrls: ['./sidebar-menu.component.scss']
})
export class SidebarMenuComponent implements OnInit {

  constructor(private router: Router,
              private localStorage: AppLocalStorageService,
              private userDataService: UserDataService,
              private loginService: LoginService) {
  }

  ngOnInit() {
    if (this.localStorage.User === null) {
      this.router.navigate([''])
    }
  }

  get username() {
    return this.localStorage.User && this.localStorage.User.Username ? this.localStorage.User.Username.Value : ''
  }

  get showAdminMenu() {
    return this.userDataService.userIsAdmin() || this.userDataService.userLandingPageIs(LandingPageTypes.Administration)
  }

  get showDefaultMenu() {
    return this.userDataService.getLandingPageType() === LandingPageTypes.Default
  }

  get isImpersonate() {
    return this.userDataService.userIsImpersonate()
  }

  logout() {
    this.loginService.logout()
    return
  }

  navigateToDashboard() {
    this.router.navigate(['/admin/dashboard'])
  }

  refresh() {
  }
}

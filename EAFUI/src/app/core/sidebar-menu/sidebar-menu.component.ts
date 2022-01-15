import { Component, OnInit } from '@angular/core'
import { Router } from '@angular/router'
import { UserDataService } from '../services/user-data.service'
import { AppLocalStorageService } from '../services/local-storage.service'
import { LoginService } from '../login/login.service'
import { LandingPageTypes } from '../models/enums/LandingPageTypes'
import { DefaultDataService } from '../services/default-data.service'

@Component({
  selector: 'app-sidebar-menu',
  templateUrl: './sidebar-menu.component.html',
  styleUrls: ['./sidebar-menu.component.scss']
})
export class SidebarMenuComponent implements OnInit {

  get username() {
    return this.localStorage.User && this.localStorage.User.Username ? this.localStorage.User.Username.Value : null
  }

  get showAdminMenu() {
    return this.userDataService.userIsAdmin() || this.userDataService.getLandingPageType() == LandingPageTypes.Administration
  }

  get showDefaultMenu() {
    return this.userDataService.getLandingPageType() == LandingPageTypes.Default
  }

  constructor(private router: Router,
              private localStorage: AppLocalStorageService,
              private userDataService: UserDataService,
              private DefaultDataService: DefaultDataService,
              private loginService: LoginService) {
    }


  ngOnInit() {
    if (this.localStorage.User === null) {
      this.router.navigate([''])
    }
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
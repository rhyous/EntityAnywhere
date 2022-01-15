import { Component, OnInit } from '@angular/core'
import { LoginService } from './login.service'
import { ActivatedRoute } from '@angular/router'
import { FormControl, Validators, FormGroup } from '@angular/forms'
import { environment } from 'src/environments/environment'
import { AppLocalStorageService } from '../services/local-storage.service'

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

/**
 * The Login Component. Responsible for allowing the user to log in
 */
export class LoginComponent implements OnInit {
  //logo = `${environment.host}/assets/yourlogo.svg`
  loading = false
  form: FormGroup = new FormGroup({})

  showError = false

  constructor(private loginService: LoginService,
    private route: ActivatedRoute,
    private localStorageService: AppLocalStorageService
  ) { }

  ngOnInit() {
    this.form = new FormGroup({ username: new FormControl('', [Validators.required]), password: new FormControl('', [Validators.required]) })
    this.route.fragment.subscribe(fragment => {
      if (fragment) {
        const urlPieces = fragment.split('&')
        const obj = {}
        urlPieces.forEach(item => {
          const paramsSplit = item.split('=')
          obj[paramsSplit[0]] = paramsSplit[1]
        })
        this.parseParamsAndVerifyToken(obj)
      }
    })
  }

  login() {
    if (this.form.valid) {
      this.localStorageService.removeClaims()
      this.localStorageService.loginMethodCookie = 'Local'
      this.loading = true
      this.showError = false
      this.loginService.login(this.form.value.username, this.form.value.password)
        .subscribe(token => {
          this.loginService.parseToken(token)
        }, (errorResponse) => { // failure. display the error
          if (errorResponse.status === 0) {
            this.showError = true
          }
          this.loading = false
          this.loginService.logout()
        })
    }
  }

  /** Pull out the parameters returned by SSO and verify them. If ok then parse the token and move onto the next step */
  parseParamsAndVerifyToken(params) {
    if (params['access_token'] && params['id']) {
      this.loading = true
      this.loginService.validateOAuthToken(params['access_token'], params['id'])
        .subscribe(token => {
          this.loading = false
          if (token) {
            this.loginService.parseToken(token)
          }
        }, () => {
          this.loading = false
          setTimeout(() => window.location.replace(''), 2000)
        })
    }
  }
}

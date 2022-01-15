import { async, ComponentFixture, TestBed } from '@angular/core/testing'

import { RouterTestingModule } from '@angular/router/testing'
import { MaterialModule } from 'src/app/core/material/material.module'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { Router, ActivatedRoute } from '@angular/router'
import { of } from 'rxjs'
import { LoginService } from 'src/app/core/login/login.service'
import { DefaultDataService } from 'src/app/core/services/default-data.service'
import { UserDataService } from 'src/app/core/services/user-data.service'
import { SidebarMenuComponent } from 'src/app/core/sidebar-menu/sidebar-menu.component'
import { AppLocalStorageService } from 'src/app/core/services/local-storage.service'
import { FakeAppLocalStorageService } from 'src/app/core/services/mocks/mocks'
import { DefaultComponent } from './default.component'


class FakeActivatedRoute {
  get params() { return of('') }
}

class FakeRouter {
  parseUrl() {
    return {
      root: {
        children: {
          primary: {
            segments: []
          }
        }
      }
    }
  }
  url = 'http://localhost:4200/default'
  navigate() { }
}

export class FakeLoginService {
  logout() {}
}

describe('DefaultComponent', () => {
  let component: DefaultComponent
  let fixture: ComponentFixture<DefaultComponent>

  let router: Router
  let activatedRoute: ActivatedRoute
  let userDataService: UserDataService

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        BrowserAnimationsModule,
        RouterTestingModule,
        MaterialModule
      ],
      declarations: [DefaultComponent, SidebarMenuComponent],
      providers: [
        { provide: Router, useClass: FakeRouter },
        { provide: ActivatedRoute, useClass: FakeActivatedRoute },
        { provide: LoginService, useClass: FakeLoginService },
        DefaultDataService,
        UserDataService,
        { provide: AppLocalStorageService, useClass: FakeAppLocalStorageService },
      ]
    })
      .compileComponents()
  }))

  beforeEach(() => {
    router =  TestBed.get(Router)
    activatedRoute = TestBed.get(ActivatedRoute)
    fixture = TestBed.createComponent(DefaultComponent)
    router = TestBed.get(Router)
    activatedRoute = TestBed.get(ActivatedRoute)
    userDataService = TestBed.get(UserDataService)

    spyOn(userDataService, 'userIsAdmin').and.returnValue(false)

    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})

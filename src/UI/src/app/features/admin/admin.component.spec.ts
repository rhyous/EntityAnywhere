import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing'

import { RouterTestingModule } from '@angular/router/testing'
import { MaterialModule } from 'src/app/core/material/material.module'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { Router, ActivatedRoute } from '@angular/router'
import { of } from 'rxjs'
import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component'
import { SplitPascalCasePipe } from 'src/app/core/pipes/split-pascal-case.pipe'
import { RemoveHyphensPipe } from 'src/app/core/pipes/remove-hyphens.pipe'
import { LoginService } from 'src/app/core/login/login.service'
import { DefaultDataService } from 'src/app/core/services/default-data.service'
import { UserDataService } from 'src/app/core/services/user-data.service'
import { SidebarMenuComponent } from 'src/app/core/sidebar-menu/sidebar-menu.component'
import { AppLocalStorageService } from 'src/app/core/services/local-storage.service'
import { FakeAppLocalStorageService } from 'src/app/core/services/mocks/mocks'
import { AdminComponent } from './admin.component'
import { ImpersonateCustomerBannerComponent } from 'src/app/core/impersonate-customer-banner/impersonate-customer-banner.component'
import { PluralizePipe } from 'src/app/core/pipes/pluralize.pipe'
import { CustomPluralizationMap } from 'src/app/core/models/concretes/custom-pluralization-map'

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
  url = 'http://localhost:4200/admin'
  navigate() {}
}

export class FakeLoginService {
  logout() {}
}

describe('AdminComponent', () => {
  let component: AdminComponent
  let fixture: ComponentFixture<AdminComponent>

  let router: Router
  let activatedRoute: ActivatedRoute
  let userDataService: UserDataService

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [
        BrowserAnimationsModule,
        RouterTestingModule,
        MaterialModule
      ],
      declarations: [AdminComponent, SidebarMenuComponent, ImpersonateCustomerBannerComponent],
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
    router =  TestBed.inject(Router)
    activatedRoute = TestBed.inject(ActivatedRoute)
    userDataService = TestBed.inject(UserDataService)

    fixture = TestBed.createComponent(AdminComponent)

    spyOn(userDataService, 'userIsAdmin').and.returnValue(false)
    spyOn(userDataService, 'userIsDefault').and.returnValue(true)
    spyOn(userDataService, 'userLandingPageIs').and.returnValue(false)

    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})

import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing'

import { SidebarMenuComponent } from './sidebar-menu.component'
import { RouterTestingModule } from '@angular/router/testing'
import { UserDataService } from '../services/user-data.service'
import { Router } from '@angular/router'
import { MaterialModule } from '../material/material.module'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { DefaultDataService } from '../services/default-data.service'
import { LoginService } from '../login/login.service'
import { AppLocalStorageService } from '../services/local-storage.service'
import { FakeAppLocalStorageService } from '../services/mocks/mocks'
import { ImpersonateCustomerBannerComponent } from '../impersonate-customer-banner/impersonate-customer-banner.component'
import { By } from '@angular/platform-browser'

export class FakeLoginService {
  logout() {}
}

describe('SidebarMenuComponent', () => {
  let component: SidebarMenuComponent
  let fixture: ComponentFixture<SidebarMenuComponent>
  let defaultDataService: DefaultDataService
  let router: Router
  let userDataService: UserDataService
  let loginService: LoginService

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ SidebarMenuComponent, ImpersonateCustomerBannerComponent ],
      imports: [
        RouterTestingModule.withRoutes([]),
        MaterialModule,
        BrowserAnimationsModule
      ],
      providers: [
        DefaultDataService,
        UserDataService,
        { provide: LoginService, useClass: FakeLoginService },
        { provide: AppLocalStorageService, useClass: FakeAppLocalStorageService }
      ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(SidebarMenuComponent)
    router = TestBed.inject(Router)
    userDataService = TestBed.inject(UserDataService)
    defaultDataService = TestBed.inject(DefaultDataService)
    loginService = TestBed.inject(LoginService)

    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    // Arrange
    spyOn(userDataService, 'userIsAllowedAdminView').and.returnValue(true)
    spyOn(userDataService, 'userIsCustomer').and.returnValue(false)
    spyOn(userDataService, 'userLandingPageIs').and.returnValue(false)

    // Assert
    expect(component).toBeTruthy()
  })

  it('should display Administration menu item if user is allowed admin view', () => {
    // Arrange
    spyOn(userDataService, 'userIsRole').and.returnValue(true)
    spyOn(userDataService, 'userIsImpersonate').and.returnValue(false)

    // Act
    fixture.detectChanges()

    // Assert
    const adminLink = fixture.debugElement.query(By.css('#admin-link'))
    expect(adminLink.nativeElement.textContent).toContain('Administration')

    const customerLink = fixture.debugElement.query(By.css('#default-menu'))
    expect(customerLink).toBeNull()
  })

  it('should display Default menu if user has default role', () => {
    // Arrange
    spyOn(userDataService, 'userIsRole').withArgs('Default').and.returnValue(true)
                                        .withArgs('Admin').and.returnValue(false)
    spyOn(userDataService, 'userIsImpersonate').and.returnValue(false)

    // Act
    fixture.detectChanges()

    // Assert
    const adminLink = fixture.debugElement.query(By.css('#admin-link'))
    expect(adminLink).toBeNull()

    const defaultLink = fixture.debugElement.query(By.css('#default-menu'))
    expect(defaultLink.children.length).toBeGreaterThan(0)
  })

  it('should display Default menu if user role landing page type is Default', () => {
    // Arrange
    spyOn(userDataService, 'userIsRole').withArgs('Default').and.returnValue(true)
                                        .withArgs('Admin').and.returnValue(false)
    spyOn(userDataService, 'userIsImpersonate').and.returnValue(false)

    // Act
    fixture.detectChanges()

    // Assert
    const adminLink = fixture.debugElement.query(By.css('#admin-link'))
    expect(adminLink).toBeNull()

    const defaultLink = fixture.debugElement.query(By.css('#default-menu'))
    expect(defaultLink.children.length).toBeGreaterThan(0)
  })

  it('should navigate to the login page on click', () => {
    // Arrange
    spyOn(loginService, 'logout')

    // Act
    component.logout()

    // Assert
    expect(loginService.logout).toHaveBeenCalled()
  })

  it('should navigate to the admin page on click', () => {
    // Arrange
    spyOn(userDataService, 'userIsRole').and.returnValue(true)
    spyOn(router, 'navigate')

    // Act
    component.navigateToDashboard()

    // Assert
    expect(router.navigate).toHaveBeenCalledWith(['/admin/dashboard'])
  })
})

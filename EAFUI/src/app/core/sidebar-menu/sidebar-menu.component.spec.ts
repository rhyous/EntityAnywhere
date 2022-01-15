import { async, ComponentFixture, TestBed } from '@angular/core/testing'

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

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SidebarMenuComponent ],
      imports: [
        RouterTestingModule.withRoutes([]),
        MaterialModule,
        BrowserAnimationsModule
      ],
      providers: [
        DefaultDataService,
        UserDataService,
        { provide: FeatureFlagsService, useValue: { init: () => {} } },
        { provide: LoginService, useClass: FakeLoginService },
        { provide: AppLocalStorageService, useClass: FakeAppLocalStorageService },
        DefaultDataService
      ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(SidebarMenuComponent)
    router = TestBed.get(Router)
    userDataService = TestBed.get(UserDataService)
    defaultDataService = TestBed.get(DefaultDataService)
    loginService = TestBed.get(LoginService)


    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    spyOn(userDataService, 'userIsAdmin').and.returnValue(true)
    spyOn(userDataService, 'userIsDefault').and.returnValue(false)
    spyOn(userDataService, 'userLandingPageIsAdmin').and.returnValue(false)
    spyOn(userDataService, 'userLandingPageIsDefault').and.returnValue(false)
    expect(component).toBeTruthy()
  })

  it('should display Administration menu item if user has admin role', () => {
    // Arrange
    spyOn(userDataService, 'userIsAdmin').and.returnValue(true)
    spyOn(userDataService, 'userIsDefault').and.returnValue(false)
    spyOn(userDataService, 'userLandingPageIsAdmin').and.returnValue(false)
    spyOn(userDataService, 'userLandingPageIsDefault').and.returnValue(false)

    // Act
    fixture.detectChanges()

    // Assert
    const adminLink = fixture.debugElement.query(By.css('#admin-link'))
    expect(adminLink.nativeElement.textContent).toContain('Administration')
  })

  it('should display Administration menu item if user has admin role', () => {
    // Arrange
    spyOn(userDataService, 'userIsAdmin').and.returnValue(true)
    spyOn(userDataService, 'userIsDefault').and.returnValue(false)
    spyOn(userDataService, 'userLandingPageIsAdmin').and.returnValue(false)
    spyOn(userDataService, 'userLandingPageIsDefault').and.returnValue(false)

    // Act
    fixture.detectChanges()

    // Assert
    const adminLink = fixture.debugElement.query(By.css('#admin-link'))
    expect(adminLink.nativeElement.textContent).toContain('Administration')

    const defaultLink = fixture.debugElement.query(By.css('#default-menu'))
    expect(defaultLink).toBeNull()
  })

  it('should display Administration menu item if user role landing page type is Administration', () => {
    // Arrange
    spyOn(userDataService, 'userIsAdmin').and.returnValue(false)
    spyOn(userDataService, 'userIsDefault').and.returnValue(false)
    spyOn(userDataService, 'userLandingPageIsAdmin').and.returnValue(true)
    spyOn(userDataService, 'userLandingPageIsDefault').and.returnValue(false)

    // Act
    fixture.detectChanges()

    // Assert
    const adminLink = fixture.debugElement.query(By.css('#admin-link'))
    expect(adminLink.nativeElement.textContent).toContain('Administration')

    const defaultLink = fixture.debugElement.query(By.css('#default-menu'))
    expect(defaultLink).toBeNull()

  })

  it('should display Default menu if user has default role', () => {
    // Arrange
    spyOn(userDataService, 'userIsAdmin').and.returnValue(false)
    spyOn(userDataService, 'userIsDefault').and.returnValue(true)
    spyOn(userDataService, 'userLandingPageIsAdmin').and.returnValue(false)
    spyOn(userDataService, 'userLandingPageIsDefault').and.returnValue(false)

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
    spyOn(userDataService, 'userIsAdmin').and.returnValue(false)
    spyOn(userDataService, 'userIsDefault').and.returnValue(false)
    spyOn(userDataService, 'userLandingPageIsAdmin').and.returnValue(false)
    spyOn(userDataService, 'userLandingPageIsDefault').and.returnValue(true)

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
    spyOn(router, 'navigate')

    // Act
    component.navigateToDashboard()

    // Assert
    expect(router.navigate).toHaveBeenCalledWith(['/admin/dashboard'])
  })

})

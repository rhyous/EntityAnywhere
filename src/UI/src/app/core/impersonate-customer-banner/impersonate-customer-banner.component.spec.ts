import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { RouterTestingModule } from '@angular/router/testing'
import { LoginService } from '../login/login.service'
import { MaterialModule } from '../material/material.module'

import { ImpersonateCustomerBannerComponent } from './impersonate-customer-banner.component'

export class FakeLoginService {
  logout() {}
}

describe('ImpersonateCustomerBannerComponent', () => {
  let component: ImpersonateCustomerBannerComponent
  let fixture: ComponentFixture<ImpersonateCustomerBannerComponent>
  let loginService: LoginService

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ ImpersonateCustomerBannerComponent ],
      imports: [
        RouterTestingModule,
        MaterialModule,
        BrowserAnimationsModule
      ],
      providers: [ { provide: LoginService, useClass: FakeLoginService} ],
    })
    .compileComponents()
  }))

  beforeEach(() => {
    loginService = TestBed.inject(LoginService)
    fixture = TestBed.createComponent(ImpersonateCustomerBannerComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })

  it('should call logout on LoginService when SwitchBack', () => {
    // Arrange
    spyOn(loginService, 'logout').and.callThrough()

    // Act
    component.switchBack()

    // Assert
    expect(loginService.logout).toHaveBeenCalled()
  })
})

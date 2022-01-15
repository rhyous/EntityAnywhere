import { async, ComponentFixture, TestBed } from '@angular/core/testing'

import { CookieBannerComponent } from './cookie-banner.component'

describe('CookieBannerComponent', () => {
  let component: CookieBannerComponent
  let fixture: ComponentFixture<CookieBannerComponent>

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      providers: [
      ],
      declarations: [ CookieBannerComponent ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(CookieBannerComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })

  it('should default to false', () => {
    expect(component.hasConsent).toBeFalsy()
  })

  it('hasConsent should be true if the service returns a date greater than 30 days ago', () => {
    // Arrange
    const today = new Date()
    const aMonthAgo = new Date(today.getFullYear(), today.getMonth() - 1, today.getDate() + 1)
    spyOn(localStorage, 'getItem').and.returnValue(aMonthAgo.toString())

    // Act
    component.ngOnInit()

    // Assert
    expect(component.hasConsent).toBeTruthy()
  })

  it('hasConsent should be false if the service returns a date less than 30 days ago', () => {
    // Arrange
    const today = new Date()
    const aMonthAgo = new Date(today.getFullYear(), today.getMonth() - 1, today.getDate())
    spyOn(localStorage, 'getItem').and.returnValue(aMonthAgo.toString())

    // Act
    component.ngOnInit()

    // Assert
    expect(component.hasConsent).toBeFalsy()
  })
})

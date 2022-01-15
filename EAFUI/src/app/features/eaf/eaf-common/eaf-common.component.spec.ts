import { async, ComponentFixture, TestBed } from '@angular/core/testing'

import { EafCommonComponent } from './eaf-common.component'

describe('EafCommonComponent', () => {
  let component: EafCommonComponent
  let fixture: ComponentFixture<EafCommonComponent>

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EafCommonComponent ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(EafCommonComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})

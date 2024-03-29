import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing'

import { EafCustomComponent } from './eaf-custom.component'

describe('EafCustomComponent', () => {
  let component: EafCustomComponent
  let fixture: ComponentFixture<EafCustomComponent>

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ EafCustomComponent ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(EafCustomComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})

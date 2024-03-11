import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing'

import { EntityLabelComponent } from './entity-label.component'

describe('LabelComponent', () => {
  let component: EntityLabelComponent
  let fixture: ComponentFixture<EntityLabelComponent>

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ EntityLabelComponent ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(EntityLabelComponent)
    component = fixture.componentInstance
    component.field = {inputType: 'label', type: 'input'}
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})

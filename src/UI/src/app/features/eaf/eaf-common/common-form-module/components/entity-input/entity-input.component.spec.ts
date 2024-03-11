import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing'
import { By } from '@angular/platform-browser'
import { FormControlName, FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms'

import { EntityInputComponent } from './entity-input.component'
import { MaterialModule } from 'src/app/core/material/material.module'
import { FlexModule } from '@angular/flex-layout'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'

describe('InputComponent', () => {
  let component: EntityInputComponent
  let fixture: ComponentFixture<EntityInputComponent>

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        FlexModule,
        BrowserAnimationsModule,
        MaterialModule,
       ],
      declarations: [ EntityInputComponent ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(EntityInputComponent)
    component = fixture.componentInstance
    component.group = new FormGroup({Description: new FormControl('')})
    component.field = {name: 'Description', label: 'Type', type: 'input'}

    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })

  it ('Should display a input', () => {
    const de = fixture.debugElement.queryAll(By.directive(FormControlName))
    expect(de[0].name.toLocaleLowerCase()).toEqual('input')
    expect(de[0].attributes['ng-reflect-name']).toEqual('Description')
  })
})

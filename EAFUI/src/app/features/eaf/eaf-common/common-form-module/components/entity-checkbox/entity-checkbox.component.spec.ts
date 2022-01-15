import { async, ComponentFixture, TestBed } from '@angular/core/testing'
import { By } from '@angular/platform-browser'
import { FormControlName, FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms'

import { EntityCheckboxComponent } from './entity-checkbox.component'
import { RouterTestingModule } from '@angular/router/testing'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { MaterialModule } from 'src/app/core/material/material.module'

describe('CheckboxComponent', () => {
  let component: EntityCheckboxComponent
  let fixture: ComponentFixture<EntityCheckboxComponent>

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        BrowserAnimationsModule,
        MaterialModule,
        ReactiveFormsModule
      ],
      declarations: [
        EntityCheckboxComponent
      ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(EntityCheckboxComponent)
    component = fixture.componentInstance
    component.group = new FormGroup({enabled: new FormControl('')})
    component.field = {name: 'enabled', label: 'enabled', type: 'checkbox'}
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })

  it('should create a checkbox on the form', () => {
    const de = fixture.debugElement.queryAll(By.directive(FormControlName))
    expect(de[0].name.toLocaleLowerCase()).toEqual('mat-checkbox')
    expect(de[0].attributes['ng-reflect-name']).toEqual('enabled')
  })
})

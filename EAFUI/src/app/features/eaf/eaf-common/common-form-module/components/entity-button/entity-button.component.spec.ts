import { async, ComponentFixture, TestBed } from '@angular/core/testing'
import { By } from '@angular/platform-browser'
import { FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms'

import { EntityButtonComponent } from './entity-button.component'
import { EafModule } from 'src/app/features/eaf/eaf.module'
import { RouterTestingModule } from '@angular/router/testing'
import { FlexModule } from '@angular/flex-layout'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { MaterialModule } from 'src/app/core/material/material.module'

describe('ButtonComponent', () => {
  let component: EntityButtonComponent
  let fixture: ComponentFixture<EntityButtonComponent>

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        FlexModule,
        BrowserAnimationsModule,
        MaterialModule
      ],
      declarations: [ EntityButtonComponent ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(EntityButtonComponent)
    component = fixture.componentInstance
    component.group = new FormGroup({enabled: new FormControl('')})
    component.field = {name: 'add', label: 'Add', type: 'button'}
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })


  it('should create a button on the form', () => {
    const de = fixture.debugElement.queryAll(By.css('button'))
    expect(de[0].name.toLocaleLowerCase()).toEqual('button')
})
})

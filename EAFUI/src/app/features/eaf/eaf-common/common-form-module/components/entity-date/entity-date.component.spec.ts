import { async, ComponentFixture, TestBed } from '@angular/core/testing'
import { FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms'

import { EntityDateComponent } from './entity-date.component'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { FlexModule } from '@angular/flex-layout'
import { MaterialModule } from 'src/app/core/material/material.module'

describe('EntityDateComponent', () => {
  let component: EntityDateComponent
  let fixture: ComponentFixture<EntityDateComponent>

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        FlexModule,
        BrowserAnimationsModule,
        MaterialModule
      ],
      declarations: [ EntityDateComponent ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(EntityDateComponent)
    component = fixture.componentInstance
    component.group = new FormGroup({Description: new FormControl('')})
    component.field = {name: 'Description', label: 'Type', type: 'input'}
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})

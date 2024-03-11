import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing'
import { By } from '@angular/platform-browser'
import { FormControlName, FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms'

import { EntityTextAreaComponent } from './entity-text-area.component'
import { MaterialModule } from 'src/app/core/material/material.module'
import { FlexModule } from '@angular/flex-layout'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'


describe('EntityTextareaComponent', () => {
  let component: EntityTextAreaComponent
  let fixture: ComponentFixture<EntityTextAreaComponent>

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        FlexModule,
        BrowserAnimationsModule,
        MaterialModule,
       ],
      declarations: [ EntityTextAreaComponent ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(EntityTextAreaComponent)
    component = fixture.componentInstance
    component.group = new FormGroup({Description: new FormControl('')})
    component.field = {name: 'Description', label: 'Type', type: 'textarea'}
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })

  it ('Should display a textarea', () => {
    fixture.whenStable().then((de: any) => {
      de.debugElement.queryAll(By.directive(FormControlName))
      expect(de[0].name.toLocaleLowerCase()).toEqual('textarea')
      expect(de[0].attributes['ng-reflect-name']).toEqual('Description')
    })
  })
})

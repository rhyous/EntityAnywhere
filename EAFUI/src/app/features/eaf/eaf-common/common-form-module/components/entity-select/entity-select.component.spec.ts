import { async, ComponentFixture, TestBed } from '@angular/core/testing'
import { By } from '@angular/platform-browser'
import { FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms'

import { EntitySelectComponent } from './entity-select.component'
import { EafModule } from 'src/app/features/eaf/eaf.module'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { FlexModule } from '@angular/flex-layout'
import { MaterialModule } from 'src/app/core/material/material.module'

describe('SelectComponent', () => {
  let component: EntitySelectComponent
  let fixture: ComponentFixture<EntitySelectComponent>

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        FlexModule,
        BrowserAnimationsModule,
        MaterialModule,
       ],
      declarations: [ EntitySelectComponent ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(EntitySelectComponent)
    component = fixture.componentInstance
    component.group = new FormGroup({TypeId: new FormControl('')})
    component.field = {name: 'TypeId', label: 'Type', type: 'select', options: ['{Id: 1, Value: \'Test\'}', '{Id: 2, Value: \'Test 2\'}',
                                                                                '{Id: 3, Value: \'Test 2\'}']}
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })

  it ('Should display a select', () => {
    const de = fixture.debugElement.queryAll(By.css('mat-select'))
    expect(de[0].name.toLocaleLowerCase()).toEqual('mat-select')
    expect(de[0].attributes['ng-reflect-name']).toEqual('TypeId')
  })
})

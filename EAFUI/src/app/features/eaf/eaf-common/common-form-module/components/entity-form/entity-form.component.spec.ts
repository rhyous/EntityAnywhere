import { async, ComponentFixture, TestBed } from '@angular/core/testing'
import { Validators, FormControl, FormBuilder } from '@angular/forms'
import { Params, ActivatedRoute } from '@angular/router'

import { EntityFormComponent } from './entity-form.component'
import { EafModule } from 'src/app/features/eaf/eaf.module'
import { Observable, of } from 'rxjs'
import { RouterTestingModule } from '@angular/router/testing'


export class MockEvent implements Event {
  bubbles: boolean;  cancelBubble: boolean
  cancelable: boolean
  composed: boolean
  currentTarget: EventTarget
  defaultPrevented: boolean
  eventPhase: number
  isTrusted: boolean
  returnValue: boolean
  srcElement: Element
  target: EventTarget
  timeStamp: number
  type: string
  scoped: boolean
  deepPath: any

  AT_TARGET: number
  BUBBLING_PHASE: number
  CAPTURING_PHASE: number
  NONE: number

  composedPath(): EventTarget[] {
    throw new Error('Method not implemented.')
  }
  initEvent(type: string, bubbles?: boolean, cancelable?: boolean): void {
    throw new Error('Method not implemented.')
  }
  preventDefault(): void {
    return
  }
  stopImmediatePropagation(): void {
    throw new Error('Method not implemented.')
  }
  stopPropagation(): void {
    return
  }
}

describe('EntityFormComponent', () => {
  let component: EntityFormComponent
  let fixture: ComponentFixture<EntityFormComponent>
  let mockParams: any
  let mockActivatedRoute: any

  beforeEach(async(() => {
    mockParams = of<Params>({entityPlural: 'SuiteMemberships'})
    mockActivatedRoute = {params: mockParams}

    TestBed.configureTestingModule({
      imports: [ EafModule, RouterTestingModule ],
      declarations: [ ],
      providers: [
        { provide: ActivatedRoute, useValue: mockActivatedRoute }
      ]

    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(EntityFormComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })

  it('should bind validators', () => {
    const validations = [
      {name: 'required', validator: Validators.required, message: 'Name Required'},
      {name: 'pattern', validator: Validators.pattern('^[a-zA-Z]+$'), message: 'Accept only text'}
    ]
    const result = component.bindValidations(validations)
    expect(result).toBeDefined()
  })

  it('should return null for empty validators', () => {
    const validations = []
    const result = component.bindValidations(validations)
    expect(result).toEqual(null)
  })


  it ('Should create the control', () => {
    component.fields =  [
      {
        type: 'input',
        label: 'Username',
        inputType: 'text',
        name: 'name',
        validations: [
          {name: 'required', validator: Validators.required, message: 'Name Required'},
          {name: 'pattern', validator: Validators.pattern('^[a-zA-Z]+$'), message: 'Accept only text'}
        ],
        order: 0
      },
      {
        type: 'input',
        label: 'Email Address',
        inputType: 'email',
        name: 'email',
        validations: [
          {name: 'required', validator: Validators.required, message: 'Email Required'},
          {name: 'pattern', validator: Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+.[a-z]{2,4}$'), message: 'Invalid email'}
        ],
        order: 1
      },
      {
        type: 'button',
        label: 'Save',
        order: 2
      }]

    spyOn(component, 'bindValidations').and.returnValue(null)
    const group = component.createControl()
    expect(group.controls['email']).toBeDefined()
    expect(group.controls['name']).toBeDefined()
    expect(component.bindValidations).toHaveBeenCalledTimes(2)
  })

  it ('Should submit a form', () => {
    const mockEvent = new MockEvent()
    spyOn(mockEvent, 'stopPropagation').and.returnValue()
    spyOn(mockEvent, 'preventDefault').and.returnValue()
    spyOn(component.submit, 'emit').and.returnValue()
    component.onSubmit(mockEvent)
    expect(mockEvent.stopPropagation).toHaveBeenCalled()
    expect(mockEvent.preventDefault).toHaveBeenCalled()
    expect(component.submit.emit).toHaveBeenCalledWith({})
  })

  it ('Should validate fields if form not valid', () => {
    const mockEvent = new MockEvent()
    spyOn(mockEvent, 'stopPropagation').and.returnValue()
    spyOn(mockEvent, 'preventDefault').and.returnValue()
    spyOn(component, 'validateAllFormFields').and.returnValue()
    component.form.addControl('Test', new FormControl('', Validators.required))
    component.onSubmit(mockEvent)
    expect(component.validateAllFormFields).toHaveBeenCalled()
  })

  it ('Should validate form fields', () => {
    component.form.addControl('Test', new FormControl('', Validators.required))
    component.form.controls['Test'].markAsUntouched()
    component.validateAllFormFields(component.form)
    expect(component.form.controls['Test'].touched).toEqual(true)
  })
})

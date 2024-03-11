import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing'

import { FilterComponent } from './filter.component'
import { ReactiveFormsModule } from '@angular/forms'
import { MatCheckboxModule } from '@angular/material/checkbox'
import { MatFormFieldModule } from '@angular/material/form-field'
import { MatIconModule } from '@angular/material/icon'


describe('FilterComponent', () => {
  let component: FilterComponent
  let fixture: ComponentFixture<FilterComponent>

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        MatIconModule,
        MatFormFieldModule,
        MatCheckboxModule
      ],
      declarations: [ FilterComponent ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(FilterComponent)
    component = fixture.componentInstance
    component.filters = [
      {HeaderName: 'Field 1', PropName: 'Field1'},
      {HeaderName: 'Field 2', PropName: 'Field2'},
      {HeaderName: 'Field 3', PropName: 'Field3'}]
    component.displayFilters = false
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })

  it('Should toggle the display filter', () => {
    component.filterToggle()
    expect(component.displayFilters).toBe(true)
    component.filterToggle()
    expect(component.displayFilters).toBe(false)
  })

  it ('Should toggle FilterProperty', () => {
    spyOn(component.filterChanged, 'emit')
    spyOn(component.filteredPropertyChange, 'emit')
    component.toggleFilterProperty({HeaderName: 'Field 1', PropName: 'Field1'})
    expect(component.propertiesToFilter.length).toBe(1)
    expect(component.filteredProperty).toBe('Field1')
    expect(component.filteredPropertyChange.emit).toHaveBeenCalledWith('Field1')
    component.form.get('Field 1')?.setValue(true)
    component.toggleFilterProperty({HeaderName: 'Field 1', PropName: 'Field1'})
    expect(component.filteredProperty).toBe('')
    expect(component.filterChanged.emit).toHaveBeenCalledTimes(2)
  })

  it ('should trigger a filter text event', () => {
    spyOn(component.filterTextChange, 'emit')
    spyOn(component.filterChanged, 'emit')

    component.filterTextChanged('Test Filter')

    expect(component.filterTextChange.emit).toHaveBeenCalledWith('Test Filter')
    expect(component.filterChanged.emit).toHaveBeenCalled()

  })
})

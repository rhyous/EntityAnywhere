import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core'
import { FormGroup, FormControl } from '@angular/forms'
import { Subject } from 'rxjs'
import { debounceTime } from 'rxjs/operators'
import { FilterProperty } from './filter-property.class'
import { environment } from 'src/environments/environment'

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.scss']
})
export class FilterComponent implements OnInit {
  @Input () displayFilters!: boolean
  @Input () filters!: FilterProperty[]
  @Input () filterText!: string
  @Input () filteredProperty!: string
  @Output() filterTextChange = new EventEmitter<string>()
  @Output() filteredPropertyChange = new EventEmitter<string>()
  @Output() filterChanged = new EventEmitter()

  form!: FormGroup
  propertiesToFilter: any = []
  subject: Subject<string> = new Subject()

  constructor() { }

  ngOnInit() {
    const group: any = {}
    group['filterText'] = new FormControl(`${this.filterText}`)
    this.filters.forEach(prop => {
      if (this.filteredProperty === '') {
        group[prop.HeaderName] = new FormControl('')
      } else {
        if (prop.PropName === this.filteredProperty) {
          group[prop.HeaderName] = new FormControl(true)
        } else {
          group[prop.HeaderName] = new FormControl(false)
        }
      }
    })
    this.form = new FormGroup(group)
    this.subject.pipe(debounceTime(environment.debounceTimeInMs)).subscribe(filterText => {
      this.filterTextChanged(filterText)
    })

  }

  filterToggle() {
    this.displayFilters = !this.displayFilters
  }

  toggleFilterProperty(property: FilterProperty) {
    const currentPropertyValue = !(this.form.get(property.HeaderName)?.value || false)
    if (currentPropertyValue) {
      // add to array, clear other checkboxes
      this.propertiesToFilter = []
      this.propertiesToFilter.push(property.PropName)
      this.filters.forEach(filter => {
        if (filter.HeaderName !== property.PropName) {
          this.form.get(filter.HeaderName)?.setValue(false)
        }
      })
    } else {
      // remove property from array
      const propertyFilterIndex = this.propertiesToFilter.indexOf(property.PropName)
      if (propertyFilterIndex >= 0) {
        this.propertiesToFilter.splice(propertyFilterIndex, 1)
      }
    }
    this.filteredProperty = this.propertiesToFilter.length > 0 ? this.propertiesToFilter[0] : ''
    this.filteredPropertyChange.emit(this.filteredProperty)
    this.filterChanged.emit()
  }

  onKeyUp() {
    this.subject.next(this.form.get('filterText')?.value)
  }

  filterTextChanged(fText: string) {
    this.filterText = fText
    this.filterTextChange.emit(this.filterText)
    this.filterChanged.emit()
  }

}


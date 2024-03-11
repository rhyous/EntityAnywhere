import { Component, Input, OnInit } from '@angular/core'
import { FormGroup } from '@angular/forms'
import { Observable } from 'rxjs'
import { debounceTime, map, startWith } from 'rxjs/operators'
import { EntityService } from 'src/app/core/services/entity.service'
import { FieldConfig } from '../../../common-form-module/models/interfaces/field-config.interface'
import { environment } from 'src/environments/environment'

@Component({
  selector: 'app-extension-entity-property-auto-complete',
  templateUrl: './extension-entity-property-auto-complete.component.html',
  styleUrls: ['./extension-entity-property-auto-complete.component.scss']
})
export class ExtensionEntityPropertyAutoCompleteComponent implements OnInit {
  @Input() field!: FieldConfig
  @Input() group!: FormGroup
  @Input() onChangeCallback!: Function

  filteredOptions!: Observable<any[]>
  options: string[] = []

  constructor(private entityService: EntityService) { }

  ngOnInit() {
    this.getDistinctProperties().subscribe(resp => {
      resp.forEach((property: any) => {
        this.options.push(property)
      })
    })

    const fieldInGroup =  this.field.name ? this.group.get(this.field.name) : null

    if (fieldInGroup) {
    this.filteredOptions  = fieldInGroup
      .valueChanges.pipe(debounceTime(environment.debounceTimeInMs),
        startWith<string | any>(''),
        map(value => typeof value === 'string' ? value : value),
        map(key => key ? this.filter(key) : this.options.slice())
      )
    }
  }

  filter(value: string): string[] {
    const filterValue = value.toLowerCase()
    return this.options.filter(option => option.toLowerCase().includes(filterValue))
  }

  getDistinctProperties() {
    return this.entityService.getDistinctExtensionPropertList(this.field.entity ?? '', this.field.searchEntity ?? '')
  }

  displayFn(option: any): string {
    return option ? option : undefined
  }

}

import { Component, OnInit, Input } from '@angular/core'
import { EntityService } from '../services/entity.service'
import { map, debounceTime, startWith } from 'rxjs/operators'
import { FormGroup } from '@angular/forms'
import { Observable } from 'rxjs'
import { EntityMetadataService } from '../services/entity-metadata.service'
import { DisplayFnData } from './auto-complete-display-function.interface'

@Component({
  selector: 'app-auto-complete',
  templateUrl: './auto-complete.component.html',
  styleUrls: ['./auto-complete.component.scss']
})
export class AutoCompleteComponent implements OnInit {
  @Input() form: FormGroup
  @Input() field: any // FieldConfig
  @Input() entityName: string
  @Input() displayFunction: (option: DisplayFnData<any>) => string | undefined

  filteredData: Observable<any[]>
  options: { key: string; value: string }[] = []

  entityMetaData: any

  showProgressBar = false

  @Input() filterCriteria: string[]

  entityDisplay: string

  constructor(private entityService: EntityService, private metaDataService: EntityMetadataService) {}

  ngOnInit() {
    this.init()
  }

  init() {
    this.entityMetaData = this.metaDataService.getEntityMetaData(this.entityName)
    if (this.field && this.field.value) {
      const entities = []
      this.getData(+this.field.value).subscribe(entity => {
        entities.push({ key: entity.Object.Name, value: entity.Object.Id })
        this.form.get(this.field.name).setValue(entities[0])
      })
    }

    this.getDataList().subscribe(resp => {
      resp.forEach(entity => {
        const obj = {}
        this.filterCriteria.forEach(x => {
          obj[x] = entity.Object[x]
        })

        this.options.push({ key: entity.Object.Name, value: entity.Object.Id })
      })

      this.options = this.options.orderBy(x => x.key)
    })

    this.filteredData = this.form
      .get(this.field.name)
      .valueChanges.pipe(debounceTime(800),
        startWith<string | any>(''),
        map(value => typeof value === 'string' ? value : value.key),
        map(key => key ? this.filter(key) : this.options.slice())
      )

      this.entityDisplay = this.entityMetaData['@UI.DisplayName'] ?
                            this.entityMetaData['@UI.DisplayName'].$PropertyPath :
                            'Name'

      this.filterCriteria = <string[]>this.entityMetaData.value.$Key
      this.filterCriteria.push(this.entityDisplay)
      this.filterCriteria = this.filterCriteria.distinctOnly()
  }

  filter(key: string): { key: string; value: string }[] {
    this.showProgressBar = true
    const filteredOptions = []
    this.getFilteredDataList(key).subscribe(response => {
      if (response.Entities) {
        response.Entities.forEach(entity => {
          const obj = { key: entity.Object.Name, value: entity.Object.Id, Entity: entity }
          this.filterCriteria.forEach(x => {
            obj[x] = entity.Object[x]
          })

          filteredOptions.push(obj)
        })
      }
      this.showProgressBar = false
    })
    return filteredOptions
  }

  getDataList() {
    return this.entityService.getEntityList(this.entityName , 10).pipe(map(resp => resp.Entities))
  }

  getFilteredDataList(filterText: string) {
    return this.entityService.getEntityFilteredList(this.entityName , filterText, this.filterCriteria, this.entityDisplay, 10)
  }

  getData(id: number) {
    return this.entityService.getEntityData(this.entityName , id.toString())
  }

  displayFn(option?): string | undefined {
    return this.displayFunction ? this.displayFunction(option) : option ? option.key : undefined
  }
}

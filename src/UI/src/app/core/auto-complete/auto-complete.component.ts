import { Component, OnInit, Input } from '@angular/core'
import { EntityService } from '../services/entity.service'
import { map, debounceTime, startWith } from 'rxjs/operators'
import { FormGroup } from '@angular/forms'
import { Observable } from 'rxjs'
import { EntityMetadataService } from '../services/entity-metadata.service'
import { DisplayFnData } from './auto-complete-display-function.interface'
import { environment } from 'src/environments/environment'

@Component({
  selector: 'app-auto-complete',
  templateUrl: './auto-complete.component.html',
  styleUrls: ['./auto-complete.component.scss']
})
export class AutoCompleteComponent implements OnInit {
  @Input() form!: FormGroup
  @Input() field: any // FieldConfig
  @Input() entityName!: string
  @Input() displayFunction!: (option: DisplayFnData<any>) => string

  filteredData!: Observable<any[]>
  options: { key: string; value: string }[] = []

  entityMetaData: any

  showProgressBar = false

  @Input() filterCriteria!: string[]

  entityDisplay!: string

  constructor(private entityService: EntityService,
              private metaDataService: EntityMetadataService) {}

  ngOnInit() {
    this.init()
  }

  init() {
    this.entityMetaData = this.metaDataService.getEntityMetaData(this.entityName)
    if (this.field && this.field.value) {
      const entities: any = []
      this.getData(+this.field.value).subscribe((entity: any) => {
        entities.push({ key: entity.Object.Name, value: entity.Object.Id })
        this.form.get(this.field.name)?.setValue(entities[0])
      })
    }

    this.getDataList().subscribe((resp: any) => {
      resp.forEach((entity: any) => {
        const obj: any = {}
        this.filterCriteria.forEach(x => {
          obj[x] = entity.Object[x]
        })

        this.options.push({ key: entity.Object.Name, value: entity.Object.Id })
      })

      this.options = this.options.orderBy(x => x.key)
    })

    const formField = this.form.get(this.field.name)
    this.filteredData =  formField ?
      formField.valueChanges.pipe(debounceTime(environment.debounceTimeInMs),
        startWith<string | any>(''),
        map((value: any) => typeof value === 'string' ? value : value.key),
        map((key: any) => key ? this.filter(key) : this.options.slice())
      ) : <any>null

    this.entityDisplay = this.entityMetaData['@UI.DisplayName']
                       ? this.entityMetaData['@UI.DisplayName'].$PropertyPath
                       : 'Name'

    this.filterCriteria = <string[]>this.entityMetaData.value.$Key
    this.filterCriteria.push(this.entityDisplay)
    this.filterCriteria = this.filterCriteria.distinctOnly()
  }

  filter(key: string): { key: string; value: string }[] {
    this.showProgressBar = true
    const filteredOptions: any = []
    this.getFilteredDataList(key).subscribe((response: any) => {
      if (response.Entities) {
        response.Entities.forEach((entity: any) => {
          const obj: any = { key: entity.Object.Name, value: entity.Object.Id, Entity: entity }
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
    return this.entityService.getEntityList(this.entityName , 10).pipe(map((resp: any) => resp.Entities))
  }

  getFilteredDataList(filterText: string) {
    return this.entityService.getEntityFilteredList(this.entityName , filterText, this.filterCriteria, this.entityDisplay, 10)
  }

  getData(id: number) {
    return this.entityService.getEntityData(this.entityName , id.toString())
  }

  displayFn(option?: any): string | undefined {
    return this.displayFunction ? this.displayFunction(option) : option ? option.key : undefined
  }
}

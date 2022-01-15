import { async, ComponentFixture, TestBed } from '@angular/core/testing'

import { AutoCompleteComponent } from './auto-complete.component'
import { EntityService } from '../services/entity.service'
import { EntityMetadataService } from '../services/entity-metadata.service'
import { ReactiveFormsModule, FormGroup, FormControl } from '@angular/forms'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { MaterialModule } from '../material/material.module'
import { HttpClientTestingModule } from '@angular/common/http/testing'
import { RouterTestingModule } from '@angular/router/testing'
import { Fake } from '../services/entity-metadata-fake'
import { of } from 'rxjs'

describe('AutoCompleteComponent', () => {
  let component: AutoCompleteComponent
  let fixture: ComponentFixture<AutoCompleteComponent>

  let entityService: EntityService
  let entityMetadataService: EntityMetadataService

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AutoCompleteComponent ],
      providers: [
        EntityService,
        EntityMetadataService,
        { provide: EntityMetadataService, useValue: { getEntityMetaData: (entityName) => Fake.FakeMeta.firstOrDefault(x => x.key === entityName) }},
      ],
      imports: [
        ReactiveFormsModule,
        BrowserAnimationsModule,
        MaterialModule,
        HttpClientTestingModule,
        RouterTestingModule

      ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    entityService = TestBed.get(EntityService)
    entityMetadataService = TestBed.get(EntityMetadataService)

    spyOn(entityService, 'getEntityList').and.returnValue(of({ Entities: [] }))

    fixture = TestBed.createComponent(AutoCompleteComponent)

    component = fixture.componentInstance

    component.entityName = 'Organization'
    component.form = new FormGroup({Organization: new FormControl('')})
    component.field = {
      name: 'Organization'
    }
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })

  it('Should filter', () => {
    spyOn(component, 'getFilteredDataList').and.returnValue(of({Entities: [{Object: {Id: '1', Name: 'Warehouse1'}}]}))
    const fo = component.filter('ware')
    expect(fo.length).toEqual(1)
    expect(fo[0].key).toBe('Warehouse1')
    expect(fo[0].value).toBe('1')
  })

  it('Should get a filtered list', () => {
    spyOn(entityService, 'getEntityFilteredList').and.returnValue(of())
    component.getFilteredDataList('Ware')
    expect(entityService.getEntityFilteredList).toHaveBeenCalledWith('Organization', 'Ware', ['Id', 'SapId', 'Name'], 'Name', 10)
  })

  it ('Should get an organization', () => {
    spyOn(entityService, 'getEntityData').and.returnValue(of())
    component.getData(10)
    expect(entityService.getEntityData).toHaveBeenCalledWith('Organization', '10')
  })


  it('should handle undefined', () => {
    // Arrange

    // Act
    const response = component.displayFn(undefined)

    // Assert
    expect(response).toEqual(undefined)
  })

  it('should handle null', () => {
    // Arrange

    // Act
    const response = component.displayFn(null)

    // Assert
    expect(response).toEqual(undefined)
  })

  it('should handle an object with a property called key', () => {
    // Arrange
    const obj = {
      prop: 'MyVal'
    }

    // Act
    const response = component.displayFn(obj)

    // Assert
    expect(response).toEqual(undefined)
  })

  it('should handle an object', () => {
    // Arrange
    const obj = {
      key: 'MyKey'
    }

    // Act
    const response = component.displayFn(obj)


    // Assert
    expect(response).toEqual('MyKey')
  })
})


describe('AutoCompleteComponent update', () => {
  let component: AutoCompleteComponent
  let fixture: ComponentFixture<AutoCompleteComponent>

  let entityService: EntityService
  let entityMetadataService: EntityMetadataService

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AutoCompleteComponent ],
      providers: [
        EntityService,
        EntityMetadataService,
        { provide: EntityMetadataService, useValue: { getEntityMetaData: (entityName) => Fake.FakeMeta.firstOrDefault(x => x.key === entityName) }},
      ],
      imports: [
        ReactiveFormsModule,
        BrowserAnimationsModule,
        MaterialModule,
        HttpClientTestingModule,
        RouterTestingModule

      ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    entityService = TestBed.get(EntityService)
    entityMetadataService = TestBed.get(EntityMetadataService)

    spyOn(entityService, 'getEntityList').and.returnValue(of({ Entities: [] }))

    fixture = TestBed.createComponent(AutoCompleteComponent)

    component = fixture.componentInstance

    component.entityName = 'Organization'
    component.form = new FormGroup({Organization: new FormControl('')})
    component.field = {
      name: 'Organization'
    }
    fixture.detectChanges()


    it ('Should create with the correct data', () => {
      expect(component.getData).toHaveBeenCalledWith(20)
      expect(component.form.controls['OrganizationId'].value).toEqual({key: 'A Test Org', value: 20})
    })
  })
})

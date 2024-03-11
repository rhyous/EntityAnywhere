import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing'
import { RouterTestingModule } from '@angular/router/testing'
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'
import { FormGroup, FormControl } from '@angular/forms'
import { Observable, of } from 'rxjs'

import { MapperDialogComponent } from './mapper-dialog.component'
import { EafModule } from '../../../../eaf.module'
import { EntityField } from '../../../common-form-module/models/concretes/entity-field'
import { EntityService } from 'src/app/core/services/entity.service'
import { EntityPropertyControlService } from '../../../common-form-module/services/entity-property-control.service'
import { environment } from 'src/environments/environment'
import { EntityMetadata } from 'src/app/core/models/concretes/entity-metadata'
import { EntityMetadataService } from 'src/app/core/services/entity-metadata.service'
import { Fake } from 'src/app/core/services/entity-metadata-fake'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { PluralizePipe } from 'src/app/core/pipes/pluralize.pipe'
import { CustomPluralizationMap } from 'src/app/core/models/concretes/custom-pluralization-map'
import { SplitPascalCasePipe } from 'src/app/core/pipes/split-pascal-case.pipe'


export class MdDialogMock {
  close(returnValue: any) {
    return {
      afterClosed: () => of(returnValue)
    }
  }
}

export class MdAddDialogMock {
  afterClosed() {
    return of('confirmed')
  }
  apply() {
  }
}

export class MockData {
  static getFieldObject(ftype: string, name: string, readOnly: boolean,
    nullable: boolean, collection: boolean, displayOrder: number,
    searchable: boolean): EntityField {

    const ef = new EntityField()
    ef.Type = ftype
    ef.Name = name
    ef.ReadOnly = readOnly
    ef.Nullable = nullable
    ef.Collection = collection
    ef.DisplayOrder = displayOrder
    ef.Searchable = searchable

    return ef
  }
}

describe('MapperDialogComponent', () => {
  let component: MapperDialogComponent
  let fixture: ComponentFixture<MapperDialogComponent>
  let addDialog: MdAddDialogMock
  let entityService: EntityService
  let epcs: EntityPropertyControlService
  let entityMetadataService: EntityMetadataService

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [
        EafModule,
        RouterTestingModule,
        BrowserAnimationsModule
      ],
      declarations: [ ],
      providers: [
        { provide: MatDialogRef, useClass: MdDialogMock },
        { provide: MAT_DIALOG_DATA, useValue:  {entityName: 'Feature', ToEntityName: 'Product', FromEntityName: 'ProductFeatureMap'} },
        EntityPropertyControlService,
        PluralizePipe,
        CustomPluralizationMap,
        SplitPascalCasePipe,
      ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(MapperDialogComponent)
    entityService = TestBed.inject(EntityService)
    epcs = TestBed.inject(EntityPropertyControlService)
    entityMetadataService = TestBed.inject(EntityMetadataService)
    component = fixture.componentInstance
    component.form = new FormGroup({Description: new FormControl('')})
    addDialog = new MdAddDialogMock()
  })

  it('should create', () => {
    // Arrange
    spyOn(component, 'loadMetaData').and.returnValue(null)
    spyOn(component, 'setupFilterFields').and.returnValue(<any>null)
    spyOn(component, 'createForm').and.returnValue(<any>null)

    // Act
    fixture.detectChanges()

    // Assert
    expect(component).toBeTruthy()
  })

  it('should create the form', () => {
    // Act
    component.createForm([{Name: 'Description'}])

    // Assert
    expect(component.form.controls['Description']).toBeDefined()
  })

  it('should change the page', () => {
    // Arrange
    spyOn(component, 'filterData').and.returnValue(<any>null)

    // Act
    component.pageChange({pageIndex: 2})

    // Assert
    expect(component.filterData).toHaveBeenCalledTimes(1)
  })

  it('should close when cancelled', () => {
    // Arrange
    spyOn(component.dialogRef, 'close').and.returnValue(<any>null)

    // Act
    component.cancel()

    // Assert
    expect(component.dialogRef.close).toHaveBeenCalledTimes(1)
  })

  it('should add the mapping on row click', () => {
    // Arrange
    spyOn(component, 'confirmAdd').and.callThrough()
    spyOn(component, 'addRecords').and.returnValue(<any>null)
    const dialogRefSpyObj = jasmine.createSpyObj({ afterClosed : of('confirmed')})
    spyOn(component.matDialog, 'open').and.returnValue(dialogRefSpyObj)

    component.data = {ToEntityName: 'Product', FromEntityName: 'Feature'}

    // Act
    component.rowClick({Id: 4})

    // Assert
    expect(component.matDialog.open).toHaveBeenCalled()
    expect(component.confirmAdd).toHaveBeenCalledWith([4])
    expect(component.addRecords).toHaveBeenCalledWith([4])
  })

  it('should add multiple mappings on Add click', () => {
    // Arrange
    spyOn(component, 'confirmAdd').and.callThrough()
    spyOn(component, 'addRecords').and.returnValue(<any>null)
    const dialogRefSpyObj = jasmine.createSpyObj({ afterClosed : of('confirmed')})
    spyOn(component.matDialog, 'open').and.returnValue(dialogRefSpyObj)

    component.selection.select(2, 3, 4, 5)

    component.data = {ToEntityName: 'Product', FromEntityName: 'Feature'}

    // Act
    component.addMultipleRecords()

    // Assert
    expect(component.matDialog.open).toHaveBeenCalled()
    expect(component.confirmAdd).toHaveBeenCalledWith([2, 3, 4, 5])
    expect(component.addRecords).toHaveBeenCalledWith([2, 3, 4, 5])
  })

  it('should filter the table', () => {
    // Arrange
    spyOn(entityService, 'getFilteredEntityList').and.returnValue(of(<any>{TotalCount: 24, Entities: [{Id: 1}]}))
    spyOn(component, 'createDefaultEntity').and.callThrough()
    component.data = {ToEntityName: 'Product', FromEntityName: 'Feature'}
    component.filterText = 'Fred'
    component.defaultEntity = {Name: 'All', Value: 1}

    // Act
    component.filterData()

    // Assert
    expect(component.displayProgress).toBeFalsy()
    expect(component.count).toEqual(25)
    expect(entityService.getFilteredEntityList).toHaveBeenCalledWith('Product', 'Fred', 10, 0)
  })

  it('should filter the table with defaultEntity', () => {
    // Arrange
    spyOn(entityService, 'getFilteredEntityList').and.returnValue(of(<any>{TotalCount: 24, Entities: [{Id: 1}]}))
    component.data = {ToEntityName: 'Product', FromEntityName: 'Feature'}
    component.filterText = 'Fred'

    // Act
    component.filterData()

    // Assert
    expect(component.displayProgress).toBeFalsy()
    expect(component.count).toEqual(24)
    expect(entityService.getFilteredEntityList).toHaveBeenCalledWith('Product', 'Fred', 10, 0)
  })

  it('should add a row', () => {
    // Arrange
    spyOn(localStorage, 'getItem').and.returnValue(JSON.stringify([]))
    spyOn(TestBed.inject(EntityPropertyControlService), 'getMappedReferentialConstraint').and.callFake((m, t, a) => 'Something')
    spyOn(entityService, 'addEntity').and.returnValue(of(<any>{}))
    spyOn(component.dialogRef, 'close').and.returnValue(<any>null)
    component.data = {
      MappingEntityName: 'Something',
      FromEntityName: 'Somewhere',
      ToEntityName: 'That place'
    }

    // Act
    component.addRecords([{Id: 4}])

    // Assert
    expect(component.dialogRef.close).toHaveBeenCalledWith('Added')
    expect(component.displayProgress).toBeFalsy()
  })

  it ('should filter records on a filter change', () => {
    // Arrange
    component.form = new FormGroup({Id: new FormControl(2), Description: new FormControl('Test')})
    const metaData = new EntityMetadata()
    metaData.Name = 'Feature'
    metaData.Fields.push(MockData.getFieldObject('Edm.String', 'Description', false, true, false, 0, true))
    metaData.Fields.push(MockData.getFieldObject('Edm.Int32', 'Id', false, true, false, 0, true))
    component.metaData = metaData

    spyOn(component, 'filterData').and.returnValue()

    // Act
    component.onFilterChanged()

    // Assert
    expect(component.filterText).toEqual(`Id eq '2' and contains(Description, 'Test')`)
    expect(component.pageIndex).toEqual(0)
  })

  it('Should setup filter fields correctly', () => {
    // Arrange
    const metaData = new EntityMetadata()
    metaData.Name = 'Feature'
    metaData.Fields.push(MockData.getFieldObject('Edm.Date', 'CreateDate', true, true, false, 0, false))
    metaData.Fields.push(MockData.getFieldObject('Edm.Int64', 'CreatedBy', true, true, false, 0, false))
    metaData.Fields.push(MockData.getFieldObject('Edm.String', 'Description', false, true, false, 0, true))
    metaData.Fields.push(MockData.getFieldObject('Edm.Int32', 'Id', false, true, false, 0, true))

    // Act
    component.setupFilterFields(metaData)

    // Assert
    expect(component.filterFields.length).toEqual(2)
    // DisplayedColumns will 3 becuse of the SelectAll Coulmn
    expect(component.displayedColumns.length).toEqual(3)
  })

  it('should get the metadata from local storage', () => {
    // #region Test metadata
    const metaData = [{
      'key': 'Addendum',
      'value': {
            '$Key': ['Id'],
            '$Kind': 'EntityType',
            'CreateDate': {
                '$Type': 'Edm.DateTimeOffset',
                '@UI.DisplayOrder': 6,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'CreatedBy': {
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 7,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'Entity': {
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 2,
                '@UI.Searchable': false
            },
            'EntityId': {
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 3,
                '@UI.Searchable': false
            },
            'Id': {
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 1,
                '@UI.Searchable': true
            },
            'LastUpdated': {
                '$Nullable': true,
                '$Type': 'Edm.DateTimeOffset',
                '@UI.DisplayOrder': 8,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'LastUpdatedBy': {
                '$Nullable': true,
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 9,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'Property': {
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 4,
                '@UI.Searchable': false
            },
            'Value': {
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 5,
                '@UI.Searchable': false
            },
            '@EAF.EntityGroup': 'Extension Entities'
        }
    }]
    //#endregion

    localStorage.setItem(environment.metaDataLocalName, JSON.stringify(metaData))
    component.data.ToEntityName = 'Addendum'
    spyOn(entityMetadataService, 'getEntityMetaData').and.returnValue(Fake.FakeMeta.firstOrDefault(x => x.key === 'Addendum'))
    spyOn(entityMetadataService, 'getEntityFromMetaData').and.callThrough()

    // Act
    component.loadMetaData()

    // Assert
    expect(component.metaData.Name).toEqual('Addendum')
    expect(entityMetadataService.getEntityMetaData).toHaveBeenCalled()
    expect(entityMetadataService.getEntityFromMetaData).toHaveBeenCalled()
  })

  it ('Should set hasDefaultFilter to true - Key = Id', () => {
    // Arrange
    component.defaultEntity = {Name: 'All', Value: 0}

    // Act
    component.checkDefaultValueFilter('Id', '0')

    // Assert
    expect(component.hasDefaultFilter).toBeTruthy()
  })

  it ('Should set hasDefaultFilter to false - Key = Id', () => {
    // Arrange
    component.defaultEntity = {Name: 'All', Value: 1}

    // Act
    component.checkDefaultValueFilter('Id', '0')

    // Assert
    expect(component.hasDefaultFilter).toBeFalsy()
  })

  it ('Should set hasDefaultFilter to true - Key = Name', () => {
    // Arrange
    component.defaultEntity = {Name: 'All', Value: 0}

    // Act
    component.checkDefaultValueFilter('Name', 'All')

    // Assert
    expect(component.hasDefaultFilter).toBeTruthy()
  })

  it ('Should set hasDefaultFilter to false - Key = Name', () => {
    // Arrange
    component.defaultEntity = {Name: 'All', Value: 0}

    // Act
    component.checkDefaultValueFilter('Name', 'All Entities')

    // Assert
    expect(component.hasDefaultFilter).toBeFalsy()
  })

  it ('Should create a default entity', () => {
    // Arrange
    component.defaultEntity = {Name: 'All', Value: 0}

    // Act
    const defaultEntity = component.createDefaultEntity()

    // Assert
    expect(defaultEntity.Id).toEqual(0)
    expect(defaultEntity.Object).toEqual({Id: 0, Name: 'All'})
  })

  it('should update the selection and selectedOnThisPage on rowToggle', () => {
    // Arrange
    component.pageIndex = 1
    component.selection.select(1)
    component.selectedOnThisPage.push({PageIndex: component.pageIndex, RowId: 1 })
    spyOn(component, 'isRowSelected').and.returnValue(false)

    // Act
    component.rowToggle(1)

    // Assert
    expect(component.selection.selected.length).toEqual(0)
    expect(component.selectedOnThisPage.length).toEqual(0)
  })

  it('should check if a row is selected', () => {
    // Arrange
    component.selection.select(1)

    // Act
    const isSelected = component.isRowSelected(1)

    // Assert
    expect(isSelected).toBeTruthy()
  })

  it('isAllSelected should be true', () => {
    // Arrange
    component.pageIndex = 1
    component.selectedOnThisPage.push({PageIndex: component.pageIndex, RowId: 1 })
    component.selectedOnThisPage.push({PageIndex: component.pageIndex, RowId: 2 })
    component.selectedOnThisPage.push({PageIndex: component.pageIndex, RowId: 3 })
    component.selectedOnThisPage.push({PageIndex: 2, RowId: 4 })
    component.selectedOnThisPage.push({PageIndex: 2, RowId: 5 })
    component.selectedOnThisPage.push({PageIndex: 2, RowId: 6 })

    const rows: any = []
    rows.push('Row1')
    rows.push('Row2')
    rows.push('Row3')

    component.datasource.data = rows

    // Assert
    expect(component.isAllSelected()).toBeTruthy()
  })

  it('isAllSelected should be false', () => {
    // Arrange
    component.pageIndex = 1
    component.selectedOnThisPage.push({PageIndex: component.pageIndex, RowId: 1 })
    component.selectedOnThisPage.push({PageIndex: component.pageIndex, RowId: 2 })
    component.selectedOnThisPage.push({PageIndex: 2, RowId: 4 })
    component.selectedOnThisPage.push({PageIndex: 2, RowId: 5 })
    component.selectedOnThisPage.push({PageIndex: 2, RowId: 6 })

    const rows: any = []
    rows.push('Row1')
    rows.push('Row2')
    rows.push('Row3')

    component.datasource.data = rows

    // Assert
    expect(component.isAllSelected()).toBeFalsy()
  })

  it('masterToggle should clear selection', () => {
    // Arrange
    spyOn(component, 'isAllSelected').and.returnValue(true)
    component.pageIndex = 1
    component.selectedOnThisPage.push({PageIndex: component.pageIndex, RowId: 1 })
    component.selectedOnThisPage.push({PageIndex: component.pageIndex, RowId: 2 })
    component.selectedOnThisPage.push({PageIndex: component.pageIndex, RowId: 3 })
    component.selectedOnThisPage.push({PageIndex: 2, RowId: 4 })
    component.selectedOnThisPage.push({PageIndex: 2, RowId: 5 })
    component.selectedOnThisPage.push({PageIndex: 2, RowId: 6 })

    const rows: any = []
    rows.push({Id: 1})
    rows.push({Id: 2})
    rows.push({Id: 3})

    component.datasource.data = rows

    // Act
    component.masterToggle()

    // Assert
    expect(component.selection.selected.length).toEqual(0)
    expect(component.selectedOnThisPage.length).toEqual(3)
  })

  it('masterToggle should add to selection', () => {
    // Arrange
    spyOn(component, 'isAllSelected').and.returnValue(false)
    component.pageIndex = 1
    component.selectedOnThisPage.push({PageIndex: component.pageIndex, RowId: 1 })
    component.selectedOnThisPage.push({PageIndex: component.pageIndex, RowId: 2 })
    component.selectedOnThisPage.push({PageIndex: component.pageIndex, RowId: 3 })
    component.selectedOnThisPage.push({PageIndex: 2, RowId: 4 })
    component.selectedOnThisPage.push({PageIndex: 2, RowId: 5 })
    component.selectedOnThisPage.push({PageIndex: 2, RowId: 6 })

    const rows: any = []
    rows.push({Id: 1})
    rows.push({Id: 2})
    rows.push({Id: 3})

    component.datasource.data = rows

    // Act
    component.masterToggle()

    // Assert
    expect(component.selection.selected.length).toEqual(3)
    expect(component.selectedOnThisPage.length).toEqual(6)
  })



})

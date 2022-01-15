import { async, ComponentFixture, TestBed } from '@angular/core/testing'
import { RouterTestingModule } from '@angular/router/testing'
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material'
import { FormGroup, FormControl } from '@angular/forms'
import { Observable, of } from 'rxjs'

import { MapperDialogComponent } from './mapper-dialog.component'
import { EafModule } from '../../../../eaf.module'
import { ApplicationMonitoringService } from 'src/app/core/services/application-monitoring.service'
import { EntityField } from 'src/app/core/models/concretes/entity-field'
import { EntityService } from 'src/app/core/services/entity.service'
import { EntityPropertyControlService } from '../../../common-form-module/services/entity-property-control.service'
import { environment } from 'src/environments/environment'
import { EntityMetadata } from 'src/app/core/models/concretes/entity-metadata'
import { EntityMetadataService } from 'src/app/core/services/entity-metadata.service'
import { Fake } from 'src/app/core/services/entity-metadata-fake'


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

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        EafModule,
        RouterTestingModule
      ],
      declarations: [ ],
      providers: [{ provide: MatDialogRef, useClass: MdDialogMock },
                  { provide: MAT_DIALOG_DATA, useValue:  {entityName: 'Feature', ToEntityName: 'Product', FromEntityName: 'ProductFeatureMap'} },
                  ApplicationMonitoringService,
                  EntityPropertyControlService
      ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(MapperDialogComponent)
    entityService = TestBed.get(EntityService)
    epcs = TestBed.get(EntityPropertyControlService)
    entityMetadataService = TestBed.get(EntityMetadataService)
    component = fixture.componentInstance
    component.form = new FormGroup({Description: new FormControl('')})
    addDialog = new MdAddDialogMock()
  })

  it('should create', () => {
    spyOn(component, 'loadMetaData').and.returnValue(null)
    spyOn(component, 'setupFilterFields').and.returnValue(null)
    spyOn(component, 'createForm').and.returnValue(null)
    fixture.detectChanges()
    expect(component).toBeTruthy()
  })

  it('should create the form', () => {
    component.createForm([{Name: 'Description'}])
    expect(component.form.controls['Description']).toBeDefined()
  })

  it('should change the page', () => {
    spyOn(component, 'filterData').and.returnValue(null)
    component.pageChange({pageIndex: 2})
    expect(component.filterData).toHaveBeenCalledTimes(1)
  })

  it('should close when cancelled', () => {
    spyOn(component.dialogRef, 'close').and.returnValue(null)
    component.cancel()
    expect(component.dialogRef.close).toHaveBeenCalledTimes(1)
  })

  it('should add the mapping on row click', () => {
    spyOn(component, 'addRecord').and.returnValue(null)
    const dialogRefSpyObj = jasmine.createSpyObj({ afterClosed : of('confirmed')})
    spyOn(component.matDialog, 'open').and.returnValue(dialogRefSpyObj)

    component.data = {ToEntityName: 'Product', FromEntityName: 'Feature'}
    component.rowClick({Id: 4})

    expect(component.matDialog.open).toHaveBeenCalled()
    expect(component.addRecord).toHaveBeenCalledWith({Id: 4})
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
    spyOn(entityService, 'getFilteredEntityList').and.returnValue(of(<any>{TotalCount: 24, Entities: [{Id: 1}]}))
    component.data = {ToEntityName: 'Product', FromEntityName: 'Feature'}
    component.filterText = 'Fred'

    component.filterData()
    expect(component.displayProgress).toBeFalsy()
    expect(component.count).toEqual(24)
    expect(entityService.getFilteredEntityList).toHaveBeenCalledWith('Product', 'Fred', 10, 0)
  })

  it('should add a row', () => {
    spyOn(localStorage, 'getItem').and.returnValue(JSON.stringify([]))
    spyOn(TestBed.get(EntityPropertyControlService), 'getMappedReferentialConstraint').and.callFake((m, t, a) => 'Something')
    spyOn(entityService, 'addEntity').and.returnValue(of(<any>{}))
    spyOn(component.dialogRef, 'close').and.returnValue(null)
    component.data = {
      MappingEntityName: 'Something',
      FromEntityName: 'Somewhere',
      ToEntityName: 'That place'
    }
    component.addRecord({Id: 4, Name: 'Test'})
    expect(component.dialogRef.close).toHaveBeenCalledWith('Added')
    expect(component.displayProgress).toBeFalsy()
  })

  it ('should filter records on a filter change', () => {
    component.form = new FormGroup({Id: new FormControl(2), Description: new FormControl('Test')})
    const metaData = new EntityMetadata()
    metaData.Name = 'Feature'
    metaData.Fields.push(MockData.getFieldObject('Edm.String', 'Description', false, true, false, 0, true))
    metaData.Fields.push(MockData.getFieldObject('Edm.Int32', 'Id', false, true, false, 0, true))
    component.metaData = metaData

    spyOn(component, 'filterData').and.returnValue()
    component.onFilterChanged()

    expect(component.filterText).toEqual(`Id eq '2' and contains(Description, 'Test')`)
    expect(component.pageIndex).toEqual(0)
  })

  it('Should setup filter fields correctly', () => {
    //#region Test Data
    const metaData = new EntityMetadata()
    metaData.Name = 'Feature'
    metaData.Fields.push(MockData.getFieldObject('Edm.Date', 'CreateDate', true, true, false, 0, false))
    metaData.Fields.push(MockData.getFieldObject('Edm.Int64', 'CreatedBy', true, true, false, 0, false))
    metaData.Fields.push(MockData.getFieldObject('Edm.String', 'Description', false, true, false, 0, true))
    metaData.Fields.push(MockData.getFieldObject('Edm.Int32', 'Id', false, true, false, 0, true))

    //#endregion
    component.setupFilterFields(metaData)

    expect(component.filterFields.length).toEqual(2)
    expect(component.displayedColumns.length).toEqual(2)
  })

  it('should get the metadata from local storage', () => {
    // #region Test metadata
    const metaData = [{
      'key': 'User',
      'value': {
        "$Key": [
            "Id",
            "Username"
        ],
        "$Kind": "EntityType",
        "CreateDate": {
            "$Type": "Edm.DateTimeOffset",
            "@UI.DisplayOrder": 10,
            "@UI.Searchable": false,
            "@UI.ReadOnly": true
        },
        "CreatedBy": {
            "$Type": "Edm.Int64",
            "@UI.DisplayOrder": 11,
            "@UI.Searchable": false,
            "@UI.ReadOnly": true
        },
        "Enabled": {
            "$Type": "Edm.Boolean",
            "@UI.DisplayOrder": 2,
            "@UI.Searchable": false
        },
        "ExternalAuth": {
            "$Type": "Edm.Boolean",
            "@UI.DisplayOrder": 3,
            "@UI.Searchable": false
        },
        "Firstname": {
            "$Nullable": true,
            "$Type": "Edm.String",
            "@UI.DisplayOrder": 4,
            "@UI.Searchable": false
        },
        "Id": {
            "$Type": "Edm.Int64",
            "@UI.DisplayOrder": 1,
            "@UI.Searchable": false
        },
        "IsHashed": {
            "$Type": "Edm.Boolean",
            "@UI.DisplayOrder": 5,
            "@UI.Searchable": false
        },
        "Lastname": {
            "$Nullable": true,
            "$Type": "Edm.String",
            "@UI.DisplayOrder": 6,
            "@UI.Searchable": false
        },
        "LastUpdated": {
            "$Nullable": true,
            "$Type": "Edm.DateTimeOffset",
            "@UI.DisplayOrder": 12,
            "@UI.Searchable": false,
            "@UI.ReadOnly": true
        },
        "LastUpdatedBy": {
            "$Nullable": true,
            "$Type": "Edm.Int64",
            "@UI.DisplayOrder": 13,
            "@UI.Searchable": false,
            "@UI.ReadOnly": true
        },
        "Password": {
            "$Nullable": true,
            "$Type": "Edm.String",
            "@UI.DisplayOrder": 7,
            "@UI.Searchable": false
        },
        "Salt": {
            "$Nullable": true,
            "$Type": "Edm.String",
            "@UI.DisplayOrder": 8,
            "@UI.Searchable": false
        },
        "Username": {
            "$Type": "Edm.String",
            "@UI.DisplayOrder": 9,
            "@UI.Searchable": false,
            "@UI.MinLength": 1,
            "@UI.MaxLength": 450
        },
        "@EAF.EntityGroup": "Miscellaneous",
        "@UI.DisplayName": {
            "$PropertyPath": "Username"
        },
        "UserGroupMemberships": {
            "$Collection": true,
            "$Kind": "NavigationProperty",
            "$Nullable": true,
            "$Type": "self.UserGroupMembership",
            "@EAF.RelatedEntity.Type": "Foreign"
        },
        "UserRoleMemberships": {
            "$Collection": true,
            "$Kind": "NavigationProperty",
            "$Nullable": true,
            "$Type": "self.UserRoleMembership",
            "@EAF.RelatedEntity.Type": "Foreign"
        },
        "UserTypeMaps": {
            "$Collection": true,
            "$Kind": "NavigationProperty",
            "$Nullable": true,
            "$Type": "self.UserTypeMap",
            "@EAF.RelatedEntity.Type": "Foreign"
        },
        "UserGroups": {
            "$Collection": true,
            "$Kind": "NavigationProperty",
            "$Nullable": true,
            "$Type": "self.UserGroup",
            "@EAF.RelatedEntity.Type": "Mapping",
            "@EAF.RelatedEntity.MappingEntityType": "self.UserGroupMembership"
        },
        "UserRoles": {
            "$Collection": true,
            "$Kind": "NavigationProperty",
            "$Nullable": true,
            "$Type": "self.UserRole",
            "@EAF.RelatedEntity.Type": "Mapping",
            "@EAF.RelatedEntity.MappingEntityType": "self.UserRoleMembership"
        },
        "UserTypes": {
            "$Collection": true,
            "$Kind": "NavigationProperty",
            "$Nullable": true,
            "$Type": "self.UserType",
            "@EAF.RelatedEntity.Type": "Mapping",
            "@EAF.RelatedEntity.MappingEntityType": "self.UserTypeMap"
        },
        "Addenda": {
            "$Collection": true,
            "$Kind": "NavigationProperty",
            "$Nullable": true,
            "$Type": "self.Addendum",
            "@EAF.RelatedEntity.Type": "Extension"
        },
        "AlternateIds": {
            "$Collection": true,
            "$Kind": "NavigationProperty",
            "$Nullable": true,
            "$Type": "self.AlternateId",
            "@EAF.RelatedEntity.Type": "Extension"
        }
      }
    }]
    //#endregion

    localStorage.setItem(environment.metaDataLocalName, JSON.stringify(metaData))
    component.data.ToEntityName = 'ProductGroup'
    spyOn(entityMetadataService, 'getEntityMetaData').and.returnValue(Fake.FakeMeta.firstOrDefault(x => x.key === 'ProductGroup'))
    spyOn(entityMetadataService, 'getEntityFromMetaData').and.callThrough()

    // Act
    component.loadMetaData()

    // Assert
    expect(component.metaData.Name).toEqual('ProductGroup')
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


})

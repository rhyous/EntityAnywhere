import { async, ComponentFixture, TestBed } from '@angular/core/testing'
import { RouterTestingModule } from '@angular/router/testing'
import { of, throwError } from 'rxjs'

import { EntityMapperComponent } from './entity-mapper.component'
import { EntityService } from 'src/app/core/services/entity.service'
import { EntityPropertyControlService } from '../../services/entity-property-control.service'
import { EntityHelperService } from '../../services/entity-helper.service'
import { FlexModule } from '@angular/flex-layout'
import { MaterialModule } from 'src/app/core/material/material.module'
import { SpaceTitlePipe } from 'src/app/core/pipes/spacetitle.pipe'
import { HttpClientTestingModule } from '@angular/common/http/testing'
import { ErrorReporterDialogComponent } from 'src/app/core/dialogs/error-reporter-dialog/error-reporter-dialog.component'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { MatDialog, MatSnackBar } from '@angular/material'
import { FieldConfig } from '../../models/interfaces/field-config.interface'
import { EntityMetadata } from 'src/app/core/models/concretes/entity-metadata'
import { EntityField } from 'src/app/core/models/concretes/entity-field'
import { EntitySchema } from 'src/app/core/models/interfaces/entity-schema.interface'
import { EntityMetadataService } from 'src/app/core/services/entity-metadata.service'
import { ArraySortOrderPipe } from 'src/app/core/pipes/array-sort-order.pipe'
import { AppLocalStorageService } from 'src/app/core/services/local-storage.service'
import { FakeAppLocalStorageService } from 'src/app/core/services/mocks/mocks'
import { Fake } from 'src/app/core/services/entity-metadata-fake'


export class MdDialogMock {
  afterClosed() {
    return of('Added')
  }
}

export class MdConfirmDialogMock {
  afterClosed() {
    return of('confirmed')
  }
}

export class SnackbarMock {
  afterDismissed() {
    return of('')
  }
}

describe('EntityMapperComponent', () => {
  let component: EntityMapperComponent
  let fixture: ComponentFixture<EntityMapperComponent>
  let entityService: EntityService
  let entityDataService: EntityHelperService
  let dialogMock: MdDialogMock
  let confirmMock: MdConfirmDialogMock
  let snackbarMock: SnackbarMock
  let epcs: EntityPropertyControlService
  let entityMetadataService: EntityMetadataService
  let arraySortOrderPipe: ArraySortOrderPipe

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        FlexModule,
        MaterialModule,
        HttpClientTestingModule,
        BrowserAnimationsModule,
      ],
      declarations: [
        EntityMapperComponent,
        SpaceTitlePipe
      ],
      providers: [
        EntityHelperService,
        EntityPropertyControlService,
        ErrorReporterDialogComponent,
        MatDialog,
        ArraySortOrderPipe,
        { provide: AppLocalStorageService, useClass: FakeAppLocalStorageService },
      ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(EntityMapperComponent)
    entityService = TestBed.get(EntityService)
    entityDataService = TestBed.get(EntityHelperService)
    epcs = TestBed.get(EntityPropertyControlService)
    entityMetadataService = TestBed.get(EntityMetadataService)
    arraySortOrderPipe = TestBed.get(ArraySortOrderPipe)

    component = fixture.componentInstance
    component.field = { targetEntity: 'Product', type: 'select', baseEntity: 'Product',
                        label: 'Product', value: 4, mapEntity: 'ProductFeatureMap'}
    component.top = 10
    dialogMock = new MdDialogMock()
    confirmMock = new MdConfirmDialogMock()
    snackbarMock = new SnackbarMock()
  })

  it('should create', () => {
    spyOn(component, 'loadTable')
    fixture.detectChanges()
    expect(component).toBeTruthy()
  })

  it('Should refresh a table', () => {
    // Arrange
    spyOn(entityService, 'getFilteredMapList').and.returnValue(of(''))

    // Act
    component.targetEntity = 'ProductFeatureMap'
    component.mapEntity = 'Product'
    component.refreshTable()

    // Assert
    expect(entityService.getFilteredMapList).toHaveBeenCalledWith('Product', 'Product', 'ProductFeatureMap', undefined, 4, 10, 0)
    expect(component.displayProgress).toBe(false)
  })

  it('should throw an error on refresh table', () => {
    spyOn(entityService, 'getFilteredMapList').and.returnValue(throwError({error: {Message: 'An error', Acknowledgable: false}}))
    spyOn(component.errorReporter, 'displayMessage').and.returnValue()
    component.refreshTable()
    expect(component.errorReporter.displayMessage).toHaveBeenCalledWith({Message: 'An error', Acknowledgable: false})
    expect(component.displayProgress).toBe(false)
  })

  it('should delete a record', () => {
    spyOn(entityService, 'deleteEntity').and.returnValue(of(null))
    component.deleteRecord(4)
    expect(entityService.deleteEntity).toHaveBeenCalledWith('ProductFeatureMap', 4)
  })

  it('should handle an error on delete', () => {
    spyOn(component.errorReporter, 'displayMessage').and.returnValue(null)
    spyOn(entityService, 'deleteEntity').and.returnValue(throwError({error: {Message: 'An error', Acknowledgable: false}}))
    component.deleteRecord(4)
    expect(entityService.deleteEntity).toHaveBeenCalledWith('ProductFeatureMap', 4)
    expect(component.errorReporter.displayMessage).toHaveBeenCalledWith({Message: 'An error', Acknowledgable: false})
  })

  it('should get the count of entities', () => {
    // Arrange
    component.targetEntity = 'Product'
    // Act
    const count = component.getCount({RelatedEntityCollection: [{RelatedEntity: 'Product', Count: 6}]})

    // Assert
    expect(count).toEqual(6)
  })

  it('should return 0 if it cannot find the RelatedEntity', () => {
    const count = component.getCount({RelatedEntityCollection: [{RelatedEntity: 'Feature', Count: 6}]})
    expect(count).toEqual(0)
  })

  it('Should process the metadata', () => {
    // Arrange
    const entityMetadata = Fake.FakeMeta.firstOrDefault(x => x.key === component.field.targetEntity)
    const mappedEntityMetadata = Fake.FakeMeta.firstOrDefault(x => x.key === component.field.mapEntity)

    spyOn(entityMetadataService, 'getEntityMetaData').and.returnValues(entityMetadata, mappedEntityMetadata)
    spyOn(entityMetadataService, 'getEntityFromMetaData').and.callThrough()
    fixture.detectChanges()

    // Act
    console.log(component.field)
    component.processMetadata()
    console.log(component.displayedColumns)
    console.log(component.entityColumns)

    // Assert
    expect(component.displayedColumns).toEqual(['select', 'Id', 'Name', 'Version', 'Description'])
    expect(component.entityColumns).toEqual(['Id', 'Name', 'Version', 'Description'])
    expect(entityMetadataService.getEntityFromMetaData).toHaveBeenCalledTimes(2)
    expect(entityMetadataService.getEntityMetaData).toHaveBeenCalledTimes(2)
  })

  it('Should create a new mapping record', () => {
    // Arrange
    spyOn(component, 'refreshTable').and.returnValue(null)

    const md = new EntityMetadata()
    md.Name = 'SuiteMembership'
    md.Kind = 'EntityType'
    const Field1 = new EntityField()
    Field1.Type = 'Edm.Int32'
    Field1.Name = 'Id'
    Field1.Default = null
    const Field2 = new EntityField()
    Field2.Type = 'Edm.Double'
    Field2.Name = 'Quantity'
    Field2.Default = null
    const Field3 = new EntityField()
    Field3.Type = 'EnumType'
    Field3.Name = 'QuantityType'
    Field3.Default = null

    md.Fields.push(<any>Field1, <any>Field2, <any>Field3)
    component.targetEntity = 'QuantityType'
    component.mapEntityMetaData = md
    spyOn(component.mapEntityMetaData, 'getField').and.returnValue(Field3)
    const matDialog = TestBed.get(MatDialog)
    spyOn(matDialog, 'open').and.returnValue({
      afterClosed: () => of('Added')
    })

    // Act
    component.createNew()

    // Assert
    expect(matDialog.open).toHaveBeenCalledTimes(1)
    expect(component.refreshTable).toHaveBeenCalledTimes(1)
  })

  it('Should delete the selected entities', () => {
    spyOn(component, 'processDeletes')

    const matDialog = TestBed.get(MatDialog)
    spyOn(matDialog, 'open').and.returnValue({
      afterClosed: () => of('confirmed')
    })

    component.selection.select(1)
    component.deleteEntities()
    expect(component.processDeletes).toHaveBeenCalledWith([1])
  })

  it('should process delete requests', () => {
    spyOn(epcs, 'getMappedReferentialConstraint').and.returnValue('')
    spyOn(entityService, 'getFilteredEntityList').and.returnValue(of(<any>{Entities: [{Id: 2}]}))
    spyOn(component, 'deleteRecord').and.returnValue(null)
    spyOn(component, 'refreshTable').and.returnValue(null)
    const matDialog = TestBed.get(MatDialog)
    spyOn(matDialog, 'open').and.returnValue({
      afterClosed: () => of('confirmed')
    })
    const snackBar = TestBed.get(MatSnackBar)
    spyOn(snackBar, 'open').and.returnValue({
      afterDismissed: () => of('')
    })

    component.selection.select(2)
    component.processDeletes([2])

    expect(component.deleteRecord).toHaveBeenCalledWith(2)
    expect(component.selection.selected.length).toEqual(0)
    expect(entityService.getFilteredEntityList).toHaveBeenCalledTimes(1)

  })

  it('should get the data and populate the table', () => {
    // Arrange
    const entitySchema = {Properties: [], Keys: []}
    // const entityMetadata = {'$Key': ['Id'], '$Kind': 'EntityType',
    //                         'Description': {'$Nullable': true, '$Type': 'Edm.String', '@UI.DisplayOrder': 6, '@UI.Searchable': true, },
    //                         'Enabled': { '$Type': 'Edm.Boolean', '@UI.DisplayOrder': 5, '@UI.Searchable': false, }}

    const mappedEntityMetadata = {'$Key': ['Id'], '$Kind': 'EntityType', 'Description': {'$Nullable': true, '$Type': 'Edm.String'},
                                  'Enabled': {'$Type': 'Edm.Boolean'}}

    // spyOn(entityMetadataService, 'getEntityMetaData').and.returnValue(of(entitySchema))
    spyOn(entityService, 'getFilteredMapList').and.returnValue(of(mappedEntityMetadata))
    spyOn(component, 'processMetadata')

    // Act
    component.loadTable()

    // Assert
    expect(entityService.getFilteredMapList).toHaveBeenCalled()
  })

  it('should format the related entity data', () => {
    // Arrange
    const data = {
      Id: 911,
      Object: {
        Id: 911,
        Name: 'Service Desk Enterprise Server',
        Description: 'Service Desk Enterprise Server',
        Enabled: true,
        IsHidden: false,
        TypeId: 3,
        Version: '10.0',
        CreateDate: '2018-08-02T09:51:23.683+00:00',
        CreatedBy: 1,
        LastUpdated: null,
        LastUpdatedBy: null,
      },
      RelatedEntityCollection: [
        {
          Count: 43,
          RelatedEntity: 'ProductInSuite',
          RelatedEntities: [
            {
              Id: '887',
              Object: {
                Id: 887,
                Name: 'Service Desk External Connections',
                Description: 'Service Desk External Connections',
                Enabled: true,
                IsHidden: false,
                TypeId: 1,
                Version: '10.0',
                CreateDate: '2018-08-02T10:51:23.683+01:00',
                CreatedBy: 1,
                LastUpdated: null,
                LastUpdatedBy: null,
              },
              RelatedEntityCollection: [
                {
                  Count: 1,
                  RelatedEntity: 'SuiteMembership',
                  RelatedEntities: [
                    {
                      Id: '407',
                      Object: {
                        Id: 407,
                        OneTime: false,
                        ProductId: 887,
                        Quantity: 1,
                        QuantityType: 1,
                        SuiteId: 911,
                        CreateDate: '2018-05-17T19:54:38.047+01:00',
                        CreatedBy: 1,
                        LastUpdated: null,
                        LastUpdatedBy: null,
                      },
                      Uri: 'test1',
                    },
                  ],
                },
              ],
              Uri: 'test2',
            },
          ],
        },
      ],
      Uri: 'test3',
    }

    const field: FieldConfig = {
        name: 'Product',
        label: 'Organization',
        value: 1,
        required: true,
        order: 1,
        flex: 30,
        type: 'number',
        targetEntity: 'ProductInSuite',
        mapEntity: 'SuiteMembership',
        entityAlias: 'Suite'
      }

    const md = new EntityMetadata()
    md.Name = 'SuiteMembership'
    md.Kind = 'EntityType'
    const Field1 = new EntityField()
    Field1.Type = 'Edm.Int32'
    Field1.Name = 'Id'
    const Field2 = new EntityField()
    Field2.Type = 'Edm.Double'
    Field2.Name = 'Quantity'
    const Field3 = new EntityField()
    Field3.Type = 'EnumType'
    Field3.Name = 'QuantityType'
    const f3Options = new Map<number, string>()
    f3Options.set(1, 'Inherited')
    f3Options.set(2, 'Fixed')
    f3Options.set(3, 'Percentage')
    Field3.Options = f3Options

    md.Fields.push(<any>Field1, <any>Field2, <any>Field3)

    component.entityColumns = ['Id', 'Name', 'Description', 'Version', 'Quantity', 'QuantityType']
    component.field = field
    component.mapEntityMetaData = md
    component.targetEntity = 'SuiteMembership'
    component.mapEntity = 'ProductInSuite'

    spyOn(Field3, 'isEnum').and.returnValue(true)
    spyOn(Field3.Options, 'get').and.returnValue('Inherited')

    // Act
    const formatteddata = component.formatData(data)

    // Assert
    expect(formatteddata.length).toEqual(1)
    expect(formatteddata[0]['Id']).toEqual(887)
    expect(formatteddata[0]['Name']).toEqual('Service Desk External Connections')
    expect(formatteddata[0]['Description']).toEqual('Service Desk External Connections')
    expect(formatteddata[0]['Version']).toEqual('10.0')
    expect(formatteddata[0]['Quantity']).toEqual(1)
    expect(formatteddata[0]['QuantityType']).toEqual('Inherited')
  })

  it('should format the related entity data - no related entity', () => {
    // Arrange
    const data = {
      Id: 911,
      Object: {
        Id: 911,
        Name: 'Service Desk Enterprise Server',
        Description: 'Service Desk Enterprise Server',
        Enabled: true,
        IsHidden: false,
        TypeId: 3,
        Version: '10.0',
        CreateDate: '2018-08-02T09:51:23.683+00:00',
        CreatedBy: 1,
        LastUpdated: null,
        LastUpdatedBy: null,
      },
      RelatedEntityCollection: [
        {
          Count: 43,
          RelatedEntity: 'ProductInSuite',
          RelatedEntities: [
            {
              Id: '887',
              Object: {
                Id: 887,
                Name: 'Service Desk External Connections',
                Description: 'Service Desk External Connections',
                Enabled: true,
                IsHidden: false,
                TypeId: 1,
                Version: '10.0',
                CreateDate: '2018-08-02T10:51:23.683+01:00',
                CreatedBy: 1,
                LastUpdated: null,
                LastUpdatedBy: null,
              },
              Uri: 'test2',
            },
          ],
        },
      ],
      Uri: 'test3',
    }

    const field: FieldConfig = {
        name: 'Product',
        label: 'Organization',
        value: 1,
        required: true,
        order: 1,
        flex: 30,
        type: 'number',
        targetEntity: 'ProductInSuite',
        mapEntity: 'SuiteMembership',
        entityAlias: 'Suite'
      }

    const md = new EntityMetadata()
    md.Name = 'SuiteMembership'
    md.Kind = 'EntityType'
    const Field1 = new EntityField()
    Field1.Type = 'Edm.Int32'
    Field1.Name = 'Id'
    const Field2 = new EntityField()
    Field2.Type = 'Edm.Double'
    Field2.Name = 'Quantity'
    const Field3 = new EntityField()
    Field3.Type = 'EnumType'
    Field3.Name = 'QuantityType'
    const f3Options = new Map<number, string>()
    f3Options.set(1, 'Inherited')
    f3Options.set(2, 'Fixed')
    f3Options.set(3, 'Percentage')
    Field3.Options = f3Options

    md.Fields.push(<any>Field1, <any>Field2, <any>Field3)

    component.entityColumns = ['Id', 'Name', 'Description', 'Version']
    component.field = field
    component.mapEntityMetaData = md
    component.targetEntity = 'SuiteMembership'
    component.mapEntity = 'ProductInSuite'

    spyOn(Field3, 'isEnum').and.returnValue(true)
    spyOn(Field3.Options, 'get').and.returnValue('Inherited')

    // Act
    const formatteddata = component.formatData(data)

    // Assert
    expect(formatteddata.length).toEqual(1)
    expect(formatteddata[0]['Id']).toEqual(887)
    expect(formatteddata[0]['Name']).toEqual('Service Desk External Connections')
    expect(formatteddata[0]['Description']).toEqual('Service Desk External Connections')
    expect(formatteddata[0]['Version']).toEqual('10.0')
  })
})

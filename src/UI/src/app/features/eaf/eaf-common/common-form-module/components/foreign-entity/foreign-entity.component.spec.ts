import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing'
import { RouterTestingModule } from '@angular/router/testing'
import { MatDialog } from '@angular/material/dialog'
import { MatSnackBar } from '@angular/material/snack-bar'
import { MatTableDataSource } from '@angular/material/table'

import { ForeignEntityComponent } from './foreign-entity.component'
import { EafModule } from 'src/app/features/eaf/eaf.module'
import { EntityService } from 'src/app/core/services/entity.service'
import { ArraySortOrderPipe } from 'src/app/core/pipes/array-sort-order.pipe'
import { SpaceTitlePipe } from 'src/app/core/pipes/spacetitle.pipe'
import { EntityPropertyControlService } from '../../services/entity-property-control.service'
import { EntityField } from '../../models/concretes/entity-field'
import { environment } from 'src/environments/environment'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { Observable, of } from 'rxjs'
import { GlobalMatDialogService } from 'src/app/core/services/global-mat-dialog.service'
import { EntityMetadataService } from 'src/app/core/services/entity-metadata.service'
import { Fake } from 'src/app/core/services/entity-metadata-fake'
import { Router } from '@angular/router'
import { By } from '@angular/platform-browser'
import { SplitPascalCasePipe } from 'src/app/core/pipes/split-pascal-case.pipe'
import { CustomPluralizationMap } from 'src/app/core/models/concretes/custom-pluralization-map'
import { PluralizePipe } from 'src/app/core/pipes/pluralize.pipe'

class FakeDialogRef {
  constructor() {}

  afterClosed(): Observable<any> {
    return of('confirmed')
  }
}

class MockDialog {
  open() {
    return new FakeDialogRef()
  }
}

/**
 * Represents a Mocked version of Application Insights
 */
class AppInsightsServiceMock {
  trackEvent() {  }
}

describe('ForeignEntityComponent', () => {
  let component: ForeignEntityComponent
  let fixture: ComponentFixture<ForeignEntityComponent>
  let entityService: EntityService
  let entityMetadataService: EntityMetadataService
  let dialog: MatDialog
  let entityPropertyControlService: EntityPropertyControlService
  const mockRouter = {
    navigate: jasmine.createSpy('navigate')
  }

  const fieldData: any = {
    Name: 'Skus',
    Type: 'self.Sku',
    ReadOnly: false,
    Nullable: true,
    Collection: true,
    DisplayOrder: 0,
    Searchable: false,
    RelatedEntityType: 'Foreign',
    Kind: 'NavigationProperty'
  }

  const md: any = [
    {
      'key': 'Sku',
      'value': {
        '$Key': ['Id', 'Name'],
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
        'Description': {
          '$Nullable': true,
          '$Type': 'Edm.String',
          '@UI.DisplayOrder': 3,
          '@UI.Searchable': true
        },
        'Enabled': {
          '$Type': 'Edm.Boolean',
          '@UI.DisplayOrder': 4,
          '@UI.Searchable': false
        },
        'Id': {
          '$Type': 'Edm.Int32',
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
        'Name': {
          '$Type': 'Edm.String',
          '@UI.DisplayOrder': 2,
          '@UI.Searchable': true
        },
        'ProductId': {
          '$Type': 'Edm.Int32',
          '@UI.DisplayOrder': 5,
          '@UI.Searchable': false,
          '$NavigationKey': 'Product'
        },
        'Product': {
          '$Kind': 'NavigationProperty',
          '$ReferentialConstraint': {
            'LocalProperty': 'ProductId',
            'ForeignProperty': 'Id'
          },
          '$Type': 'self.Product',
          '@EAF.RelatedEntity.Type': 'Local'
        },
        '@EAF.EntityGroup': 'Miscellaneous',
        'Addenda': {
          '$Collection': true,
          '$Kind': 'NavigationProperty',
          '$Nullable': true,
          '$Type': 'self.Addendum',
          '@EAF.RelatedEntity.Type': 'Extension'
        },
        'AlternateIds': {
          '$Collection': true,
          '$Kind': 'NavigationProperty',
          '$Nullable': true,
          '$Type': 'self.AlternateId',
          '@EAF.RelatedEntity.Type': 'Extension'
        }
      }
    }
  ]

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [EafModule, RouterTestingModule, BrowserAnimationsModule],
      providers: [
        EntityMetadataService,
        EntityService,
        ArraySortOrderPipe,
        SpaceTitlePipe,
        EntityPropertyControlService,
        MatSnackBar,
        PluralizePipe,
        CustomPluralizationMap,
        SplitPascalCasePipe,
        { provide: MatDialog, useClass: MockDialog },
        { provide: Router, useValue: mockRouter },
      ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    entityMetadataService = TestBed.inject(EntityMetadataService)
    entityService = TestBed.inject(EntityService)
    fixture = TestBed.createComponent(ForeignEntityComponent)
    dialog = TestBed.inject(MatDialog)
    entityPropertyControlService = TestBed.inject(EntityPropertyControlService)
    component = fixture.componentInstance

    const field: any = new EntityField()
    Object.keys(fieldData).forEach((x: any) => {
      field[x] = fieldData[x]
    })

    component.field = field

    component.parentEntityId = 2703
    component.parentEntityName = 'Product'

    localStorage.setItem(environment.metaDataLocalName, JSON.stringify(md))

    // fixture.detectChanges()
  })


  it('should create', () => {
    // Arrange
    spyOn(component, 'getEntityConfiguration').and.returnValue(<any>null)
    spyOn(component, 'getTableConfiguration').and.returnValue(<any>null)
    spyOn(component, 'refreshTable').and.returnValue(<any>null)

    // Act
    fixture.detectChanges()

    // Assert
    expect(component).toBeTruthy()
  })

  it('should call the mat dialog on create', () => {
    // Arrange
    const dialogRefSpyObj = jasmine.createSpyObj({ afterClosed : of({}), close: null })
    spyOn(dialog, 'open').and.returnValue(dialogRefSpyObj)
    spyOn(entityService, 'addEntity').and.returnValue(of(<any>null))
    spyOn(component, 'refreshTable').and.returnValue(<any>null)

    // Act
    component.createNew()

    // Assert
    expect(dialog.open).toHaveBeenCalled()
    expect(entityService.addEntity).toHaveBeenCalled()
    expect(component.refreshTable).toHaveBeenCalled()
  })

  it('should call the mat dialog on delete', () => {
    // Arrange
    spyOn(dialog, 'open').and.returnValue(<any>{
      afterClosed: () => of('confirmed')
    })
    spyOn(component, 'processDeletes')
    spyOn(entityService, 'deleteEntity').and.returnValue(of(<any>true))
    spyOn(component, 'refreshTable').and.returnValue(<any>null)

    component.selection.select(1)

    // Act
    fixture.detectChanges()
    component.deleteEntities()

    // Assert
    expect(dialog.open).toHaveBeenCalled()
    expect(component.processDeletes).toHaveBeenCalled()
    expect(component.refreshTable).toHaveBeenCalled()
  })

  it('should remove record from mat table after successful delete operation', () => {
    // Arrange
    spyOn(dialog, 'open').and.returnValue(<any>{
      afterClosed: () => of('confirmed')
    })
    spyOn(entityService, 'deleteEntity').and.returnValue(of(<any>true))
    spyOn(entityService, 'getFilteredEntityList').and.returnValue(of(<any>null))
    component.tableData = new MatTableDataSource([
      {
          'Id': 368123,
          'DealTypeId': 3,
          'EndDate': '2019-12-31T00:00:00.000Z',
          'EntitlementStateId': 1,
          'IsPerpetual': false,
          'LineNumber': '000090',
          'OrderId': '0000561190',
          'OrganizationId': 16014,
          'ProductId': 2476,
          'Quantity': 28,
          'Sku': 'LDMSPMA-M',
          'StartDate': '2019-01-01T00:00:00.000Z',
          'TypeId': 1,
          'CreateDate': '2018-12-14T03:08:03.300Z',
          'CreatedBy': 41429,
          'LastUpdated': null,
          'LastUpdatedBy': null
      }
  ])
    component.selection.select(368123)

    // Act
    fixture.detectChanges()
    component.deleteEntities()

    // Assert
    expect(component.selection.selected).toEqual([])
    expect(component.count).toEqual(0)
  })

  it('should throw error on delete operation failure', () => {
    // Arrange
    spyOn(dialog, 'open').and.returnValue(<any>{
      afterClosed: () => of('confirmed')
    })

    const response: any = {
      name: 'Error',
      size: 'string',
      type: 'string',
      statusCode: 'string',
      actions: null
    }

    spyOn(entityService, 'deleteEntity').and.returnValue(of(response))
    spyOn(entityService, 'getFilteredEntityList').and.returnValue(of(<any>null))

    component.tableData = new MatTableDataSource([
      {
          'Id': 368123,
          'DealTypeId': 3,
          'EndDate': '2019-12-31T00:00:00.000Z',
          'EntitlementStateId': 1,
          'IsPerpetual': false,
          'LineNumber': '000090',
          'OrderId': '0000561190',
          'OrganizationId': 16014,
          'ProductId': 2476,
          'Quantity': 28,
          'Sku': 'LDMSPMA-M',
          'StartDate': '2019-01-01T00:00:00.000Z',
          'TypeId': 1,
          'CreateDate': '2018-12-14T03:08:03.300Z',
          'CreatedBy': 41429,
          'LastUpdated': null,
          'LastUpdatedBy': null
      }
  ])
    component.selection.select(368123)

    // Act
    fixture.detectChanges()
    component.deleteEntities()

    // Assert
    expect(fixture.debugElement.query(By.css('#foreignEntity_Skus_Delete'))).toBeTruthy()
  })

  it('should get the count', () => {
    // Arrange
    component.tableData = new MatTableDataSource<any>([{Id: 1, Name: 'Foreign Entity 1'}])

    // Act
    const result = component.count

    // Assert
    expect(result).toEqual(1)
  })

  it('should get the page size options', () => {
    // Arrange

    // Act
    const pageSizeOps = component.pageSizeOptions

    // Assert
    expect(pageSizeOps).toEqual(environment.pageSizeOptions)
  })

  it('should get the table data', () => {
    // Arrange
    spyOn(entityService, 'getFilteredEntityList').and.returnValue(of(<any>{TotalCount: 1, Entities: [{Id: 1, Object: {Id: 1}}]}))
    spyOn(component, 'formatEntities').and.callThrough()

    // Act
    component.refreshTable()

    // Assert
    expect(component.displayProgress).toBeFalsy()
    expect(component.count).toEqual(1)
    expect(entityService.getFilteredEntityList).toHaveBeenCalled()
    expect(component.formatEntities).toHaveBeenCalled()
  })

  it('should format the entity data', () => {
    // Arrange
    const data = [{
      Object: {
        Id: 1,
        Username: 'Tester'
      }
    }]

    // Act
    const response = component.formatEntities(data)

    // Assert
    expect(response.length).toEqual(1)
    expect(response[0].Object.Id).toEqual(1)
    expect(response[0].Object.Username).toEqual('Tester')
  })

  it ('Should navigate to entity details page', () => {
    // Arrange
    component.entityName = 'Product'

    // Act
    component.rowClick({Id: 4})

    // Assert
    expect(mockRouter.navigate).toHaveBeenCalledWith(['admin/data-administration/Product/4'])
  })
})

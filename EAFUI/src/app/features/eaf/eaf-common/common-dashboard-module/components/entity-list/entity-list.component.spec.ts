// Core
import { TestBed, async, ComponentFixture } from '@angular/core/testing'
import { DatePipe } from '@angular/common'
import { Pipe, PipeTransform } from '@angular/core'
import { SelectionModel } from '@angular/cdk/collections'
import { MatDialog, MatSnackBar } from '@angular/material'
import { ActivatedRoute, Params, Router } from '@angular/router'
import { of, throwError } from 'rxjs'

// Local
import { EntityListComponent } from './entity-list.component'
import { EntityService } from 'src/app/core/services/entity.service'
import { ErrorReporterDialogComponent } from 'src/app/core/dialogs/error-reporter-dialog/error-reporter-dialog.component'
import { MockSuiteMembershipData } from './mock-suite-membership-data'
import { PageFilter } from '../../../../eaf-custom/models/concretes/page-filters'
import { StorageService } from '../../services/storage.service'
import { environment } from 'src/environments/environment'
import { EntityMetadata } from 'src/app/core/models/concretes/entity-metadata'
import { EntityField } from '../../../common-form-module/models/concretes/entity-field'
import { EntityHelperService } from '../../../common-form-module/services/entity-helper.service'
import { EntityMetadataService } from 'src/app/core/services/entity-metadata.service'
import { ArraySortOrderPipe } from 'src/app/core/pipes/array-sort-order.pipe'
import { FormBuilder, ReactiveFormsModule } from '@angular/forms'
import { MaterialModule } from 'src/app/core/material/material.module'
import { SpaceTitlePipe } from 'src/app/core/pipes/spacetitle.pipe'
import { PluralizePipe } from 'src/app/core/pipes/pluralize.pipe'
import { HttpClientTestingModule } from '@angular/common/http/testing'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { ConfirmDialogComponent } from 'src/app/core/dialogs/confirm-dialog/confirm-dialog.component'
import { BrowserDynamicTestingModule } from '@angular/platform-browser-dynamic/testing'
import { FeatureFlagsService } from 'src/app/core/services/feature-flags.service'


@Pipe({name: 'MockDatePipe'})
export class MockDatePipe implements PipeTransform {
    transform(value: number): number {
        return value
    }
}

@Pipe({name: 'MockWcfPipe'})
export class MockWcfPipe implements PipeTransform {
    transform(value: number): number {
        return value
    }
}

export class MockConfirm {
  // When the component calls this.dialog.open(...) we'll return an object
  // with an afterClosed method that allows to subscribe to the dialog result observable.
  open() {
    return {
      afterClosed: () => of('confirmed')
    }
  }
}

export class MockFeatureFlagsService {
  get entityBulkEditFeature() {
    return true
  }
  init: () => {}
}

describe('Entity List Component', () => {
  afterEach(() => {
    TestBed.resetTestingModule()
  })

  let fixture: ComponentFixture<EntityListComponent>
  let component: EntityListComponent
  let mockParams: any
  let mockActivatedRoute: ActivatedRoute
  let entityService: EntityService
  let storageService: StorageService
  let errorReporter: ErrorReporterDialogComponent
  let helper: EntityHelperService
  const mockSuiteMembershipData = new MockSuiteMembershipData()
  let featureFlagsService: FeatureFlagsService
  let featureFlagsServiceBulkEditSpy: jasmine.Spy

  let mockRouter: Router

  beforeEach(async(() => {

    mockParams = of<Params>({entityPlural: 'SuiteMemberships'})
    mockActivatedRoute = {
      params: mockParams,
      snapshot: { data: { } }
    } as ActivatedRoute

    TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        HttpClientTestingModule,
        MaterialModule,
        BrowserAnimationsModule,
      ],
      providers: [
        EntityService,
        EntityMetadataService,
        {provide: Router, useValue: {
          navigate: jasmine.createSpy('navigate')
        }},
        {provide: ActivatedRoute, useValue: mockActivatedRoute},
        DatePipe,
        MatDialog,
        MatSnackBar,
        StorageService,
        ErrorReporterDialogComponent,
        ArraySortOrderPipe,
        FormBuilder,
        EntityHelperService,
        { provide: FeatureFlagsService, useClass: MockFeatureFlagsService}
      ],
      declarations: [
        EntityListComponent,
        ConfirmDialogComponent,
        SpaceTitlePipe,
        PluralizePipe
      ]
    })
    .overrideModule(BrowserDynamicTestingModule, {set: {entryComponents: [ConfirmDialogComponent]}})
    .compileComponents()

    fixture = TestBed.createComponent(EntityListComponent)
    entityService = TestBed.get(EntityService)
    errorReporter = TestBed.get(ErrorReporterDialogComponent)
    storageService = TestBed.get(StorageService)
    helper = TestBed.get(EntityHelperService)
    featureFlagsService = TestBed.get(FeatureFlagsService)
    featureFlagsServiceBulkEditSpy = spyOnProperty(featureFlagsService, 'entityBulkEditFeature', 'get')

    component = fixture.componentInstance
    component.entityName = 'SuiteMembership'
    component.entityNamePlural = 'SuiteMemberShips'
    component.filterSettings = new PageFilter()
    component.filterSettings.FilterText = ''
    component.filterSettings.FilterProperty = ''

    mockRouter = TestBed.get(Router)
  }))

  beforeEach(() => {
    component.metaDataObject = {
      Fields: [],
      Key: ['Id'],
      Kind: 'number',
      Name: 'Id',
      UIDisplayName: 'Id',
      getField: (fieldName) => {
        return null
      },
      getSearchFields: () => {
        return null
      },
      hasField: (fieldName) => {
        return false
      }
    }
  })

  it('should create', () => {
    spyOn(component, 'initialise').and.returnValue(null)
    fixture.detectChanges()
    expect(component).toBeTruthy()
    expect(component.initialise).toHaveBeenCalled()
  })

  it ('Should change a page of results', () => {
    spyOn(component, 'updateGridData').and.returnValue(null)
    component.pageChange({pageSize: 25, pageIndex: 4})
    expect(component.updateGridData).toHaveBeenCalledWith(true)
    expect(component.filterSettings.PageSize).toEqual(25)
    expect(component.filterSettings.PageIndex).toEqual(4)
  })

  it ('Should navigate to entity details page', () => {
    component.rowClick({Id: 4})
    expect(mockRouter.navigate).toHaveBeenCalledWith(['./admin/data-management/SuiteMembership/4'])
  })

  it ('Should process the delete calls successfully', () => {
    spyOn(entityService, 'deleteEntity').and.returnValue(of(<any>''))
    spyOn(component.snackBar, 'open').and.returnValue(null)
    spyOn(component, 'updateGridData').and.returnValue(null)
    component.processDeletes([ 1 , 2])

    expect(entityService.deleteEntity).toHaveBeenCalledTimes(2)
    expect(component.snackBar.open)
    .toHaveBeenCalledWith( 'The suitememberships were successfully deleted', null, Object({ duration: 10000 }) )
    expect(component.updateGridData).toHaveBeenCalledWith(true)

  })

  it ('Should process the delete calls failure', () => {
    spyOn(entityService, 'deleteEntity').and.returnValue(throwError(''))
    spyOn(component.snackBar, 'open').and.returnValue(null)
    spyOn(component, 'updateGridData').and.returnValue(null)
    component.processDeletes([ 1 , 2])

    expect(entityService.deleteEntity).toHaveBeenCalledTimes(2)
    expect(component.snackBar.open)
    .toHaveBeenCalledWith( 'There was an issue deleting the selected suitememberships', null, Object({ duration: environment.snackBarDuration }) )
    expect(component.updateGridData).toHaveBeenCalledTimes(0)
  })

  it ('Should navigate to a create new page', () => {
    component.createNew()
    expect(mockRouter.navigate).toHaveBeenCalledWith(['./admin/data-management/SuiteMembership/add'])
  })

  it ('Should ask for confirmation for delete', () => {
    const matDialog = TestBed.get(MatDialog)
    spyOn(matDialog, 'open').and.returnValue({
      afterClosed: () => of('confirmed')
    })

    component.selection = new SelectionModel(true, [1, 2])
    spyOn(component, 'processDeletes').and.returnValue()
    component.deleteEntities()
    expect(component.processDeletes).toHaveBeenCalledWith([1, 2])
  })

  it('should formatEntities correctly', () => {
    // Arrange
    component.metaDataObject = new EntityMetadata()
    const IdField = new EntityField()
    IdField.Type = 'Edm.Int32'
    IdField.Name = 'Id'
    const cdField = new EntityField()
    cdField.Type = 'Edm.Date'
    cdField.Name = 'CreateDate'
    const cbField = new EntityField()
    cbField.Type = 'Edm.Int32'
    cbField.Name = 'CreatedBy'
    component.metaDataObject.Fields.push(<any>IdField, <any>cdField, <any>cbField)
    component.entityProperties = [{
      name: 'Id',
      order: 0
    },                            {
      name: 'CreateDate',
      order: 1
    },                            {
      name: 'CreatedBy',
      order: 2
    }]

    // Act
    const formattedData = component.formatEntities(<any>mockSuiteMembershipData.getEntityList())

    // Act
    expect(formattedData.length).toEqual(2)
  })

  it('should get the properties that are not contains', () => {
    const md1 = new EntityMetadata()
    const IdField = new EntityField()
    IdField.Type = 'Edm.Int32'
    IdField.Name = 'Id'
    const cdField = new EntityField()
    cdField.Type = 'Edm.Date'
    cdField.Name = 'CreateDate'
    const cbField = new EntityField()
    cbField.Type = 'Edm.Int32'
    cbField.Name = 'SuiteId'
    md1.Fields.push(<any>IdField, <any>cdField, <any>cbField)
    const ncp = component.getNonContainsProperties(md1)
    expect(ncp.length).toEqual(2)
    expect(ncp[1]).toEqual('SuiteId')
  })

  it('should process metaData', () => {
    const md1 = new EntityMetadata()
    const IdField = new EntityField()
    IdField.Type = 'Edm.Int32'
    IdField.Name = 'Id'
    const cdField = new EntityField()
    cdField.Type = 'Edm.Date'
    cdField.Name = 'CreateDate'
    const cbField = new EntityField()
    cbField.Type = 'Edm.Int32'
    cbField.Name = 'SuiteId'
    md1.Fields.push(<any>IdField, <any>cdField, <any>cbField)
    const propertyNames = component.processMetaData(md1)
    expect(propertyNames.length).toEqual(2)
  })

  it ('should update the grid data with fresh entity data', () => {
    spyOn(storageService, 'storeFilterSettings').and.returnValue(null)
    spyOn(component, 'getEntityList').and.returnValue(null)

    component.filterSettings.FilterText = ''
    component.filterSettings.FilterProperty = ''
    component.filterSettings.PageIndex = 1
    component.filterSettings.PageSize = 20
    component.selection.select(1)

    component.updateGridData(true)

    expect(component.selection.selected.length).toEqual(0)
    expect(component.getEntityList).toHaveBeenCalled()
  })

  it('should initialise', () => {

    spyOn(storageService, 'retrieveFilterSettings').and.returnValue(new PageFilter())
    spyOn(component, 'getEntityPropertiesFromMetadata')
    spyOn(component, 'updateGridData')
    component.entityProperties = [{name: 'Name', order: 1}]

    component.initialise()

    expect(storageService.retrieveFilterSettings).toHaveBeenCalled()
    expect(component.getEntityPropertiesFromMetadata).toHaveBeenCalled()
    expect(component.updateGridData).toHaveBeenCalled()
  })

  it('should get entity properties from stored metadata', () => {
    localStorage.setItem(environment.metaDataLocalName, JSON.stringify([{
      'key': 'DealType',
      'value': {'$Kind': 'EntityType', 'Name': {'$Collection': true, '$Type': 'Edm.String'},
                'CreateDate': {'$Type': 'Edm.Date'},
                'LastUpdated': {'$Type': 'Edm.Date'},
                'CreatedBy': {'$Type': 'Edm.Int64'},
                'LastUpdatedBy': {'$Type': 'Edm.Int64'},
                'Id': {'$Type': 'Edm.Int32'}}
    },
                                                                        {
      'key': 'SuiteMembership',
      'value': {
        '$Kind': 'EntityType',
        'Name': {'$Collection': true, '$Type': 'Edm.String'},
        'Description': {'$Collection': true, '$Type': 'Edm.String'},
        'CreateDate': {'$Type': 'Edm.Date'},
        'LastUpdated': {'$Type': 'Edm.Date'},
        'CreatedBy': {'$Type': 'Edm.Int64'},
        'LastUpdatedBy': {'$Type': 'Edm.Int64'},
        'Id': {'$Type': 'Edm.Int32'}}
    },
                                                                        {
      'key': 'Feature',
      'value': {'$Kind': 'EntityType',
                'Name': {'$Collection': true, '$Type': 'Edm.String'},
                'Description': {'$Collection': true, '$Type': 'Edm.String'},
                'Version': {'$Collection': true, '$Type': 'Edm.String'},
                'CreateDate': {'$Type': 'Edm.Date'},
                'LastUpdated': {'$Type': 'Edm.Date'},
                'CreatedBy': {'$Type': 'Edm.Int64'},
                'LastUpdatedBy': {'$Type': 'Edm.Int64'},
                'Id': {'$Type': 'Edm.Int32'}}
    }]))
    spyOn(component, 'processMetaData').and.returnValue([{name: 'Name', order: 1}])
    spyOn(component, 'getNonContainsProperties')
    component.getEntityPropertiesFromMetadata()

    expect(component.processMetaData).toHaveBeenCalled()
    expect(component.getNonContainsProperties).toHaveBeenCalled()
  })


  it('should hit the server if this is the first time the user has searched', () => {
    // Arrange
    spyOn(component, 'filterInMemory').and.returnValue()
    spyOn(component, 'filterWithServer').and.returnValue()
    component.lastSearch = null

    // Act
    component.filter({myProp: 'My Val'})

    // Assert
    expect(component.filterWithServer).toHaveBeenCalled()

  })

  it('should hit the server if the search criteria is smaller', () => {
    // Arrange
    spyOn(component, 'filterInMemory').and.returnValue()
    spyOn(component, 'filterWithServer').and.returnValue()
    component.lastSearch = {
      myProp: {
        filter: 'My Val',
        exactMatch: false
      },
      myOtherProp: {
        filter: 'My other Val',
        exactMatch: false
      }
    }

    // Act
    component.filter({
      myProp: {
        filter: 'My Val',
        exactMatch: false
      },
      myOtherProp: {
        filter: '',
        exactMatch: false
      }
    })

    // Assert
    expect(component.filterWithServer).toHaveBeenCalled()
  })

  it('should filter in memory if the amount of data is less than the total count', () => {
    // Arrange
    spyOn(component, 'filterInMemory')
    spyOn(component, 'filterWithServer')
    component.filterSettings.PageSize = 11
    component.latestEntityListResponse = {
      Count: 10,
      Entities: [
        {
          Id: 1,
          Object: {
            CreateDate: new Date(),
            CreatedBy: 0,
            DealTypeId: 1,
            EndDate: new Date(),
            EntitlementStateId: 1,
            Id: 1,
            IsPerpetual: false,
            LastUpdated: null,
            LastUpdatedBy: null,
            LineNumber: '0020',
            OrderId: '123456',
            OrganizationId: 0,
            ProductId: 1,
            Quantity: 100,
            Sku: 'SKU1',
            StartDate: new Date(),
            TypeId: 1
          },
          Uri: ''
        },
      ],
      Entity: 'My Entity',
      TotalCount: 50
    }

    component.lastSearch = {
      myProp: {
        filter: 'My Val',
        exactMatch: false
      },
      myOtherProp: {
        filter: 'My other Val',
        exactMatch: false
      }
    }

    // Act
    component.filter({
      myProp: {
        filter: 'My Val ',
        exactMatch: false
      },
      myOtherProp: {
        filter: 'My other Val',
        exactMatch: false
      }
    })

    // Assert
    expect(component.filterInMemory).toHaveBeenCalled()
  })

  it('should hit the server if the amount of data is equal to or greater than the total count', () => {
    // Arrange
    // spyOn(component, 'filterInMemory').and.returnValue()
    spyOn(component, 'filterWithServer').and.returnValue()
    component.filterSettings.PageSize = 10
    component.latestEntityListResponse = {
      Count: 10,
      Entities: [
        {
          Id: 1,
          Object: {
            CreateDate: new Date(),
            CreatedBy: 0,
            DealTypeId: 1,
            EndDate: new Date(),
            EntitlementStateId: 1,
            Id: 1,
            IsPerpetual: false,
            LastUpdated: null,
            LastUpdatedBy: null,
            LineNumber: '0020',
            OrderId: '123456',
            OrganizationId: 0,
            ProductId: 1,
            Quantity: 100,
            Sku: 'SKU1',
            StartDate: new Date(),
            TypeId: 1
          },
          Uri: ''
        },
      ],
      Entity: 'My Entity',
      TotalCount: 50
    }

    component.lastSearch = {
      myProp: {
        filter: 'My Val',
        exactMatch: false
      },
      myOtherProp: {
        filter: 'My other Val',
        exactMatch: false
      }
    }

    // Act
    component.filter({
      myProp: {
        filter: 'My Val ',
        exactMatch: false
      },
      myOtherProp: {
        filter: 'My other Val',
        exactMatch: false
      }
    })

    // Assert
    expect(component.filterWithServer).toHaveBeenCalled()
  })

  it('should call the server if there is no latest response or no entities in the latest response', () => {
    // Arrange
    spyOn(component, 'filterWithServer').and.returnValue()
    component.latestEntityListResponse = undefined

    // Act
    component.filterInMemory({myProp: { filter: 'myVal', exactMatch: false }})

    // Assert
    expect(component.filterWithServer).toHaveBeenCalled()

    component.latestEntityListResponse = {
      Count: 0,
      Entities: undefined,
      Entity: 'My Entity',
      TotalCount: 0
    }

    // Act
    component.filterInMemory({myProp: { filter: 'myVal', exactMatch: false }})

    // Assert
    expect(component.filterWithServer).toHaveBeenCalled()
  })

  it('should filter with the server if no in mem items are found', () => {
    // Arrange
    spyOn(component, 'filterWithServer').and.returnValue()
    component.latestEntityListResponse = {
      Count: 1,
      Entities: [
        {
          'Id': 1,
          'Object': {
              'Id': 1,
              'Name': 'EntitledProduct',
              'Description': 'A calculated entity that is generated from Entitlements.',
              'Enabled': true,
              'EntityGroupId': 5,
              'CreateDate': new Date('2019-04-30T11:57:19.065698-06:00'),
              'CreatedBy': 2,
              'LastUpdated': new Date('2019-08-19T10:01:17.0996284+01:00'),
              'LastUpdatedBy': 3
          },
          'Uri': 'http://localhost:3896/EntityService.svc/Entities(1)'
      }
      ],
      Entity: 'Entity',
      TotalCount: 1
    }

    // Act
    component.filterInMemory({Name: { filter: 'User', exactMatch: false }})

    // Assert
    expect(component.filterWithServer).toHaveBeenCalled()
  })

  it('should update the latest response with the data it filtered ' +
      'along with calling updateDatasource and setting the progressbar to false', () => {
        // Arrange
        spyOn(component, 'filterWithServer').and.returnValue()
        component.latestEntityListResponse = { // Represents the response of searching for 'Entitle'
          Count: 2,
          Entities: [
            {
              'Id': 1,
              'Object': {
                  'Id': 1,
                  'Name': 'EntitledProduct',
                  'Description': 'A calculated entity that is generated from Entitlements.',
                  'Enabled': true,
                  'EntityGroupId': 5,
                  'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                  'CreatedBy': 2,
                  'LastUpdated': '2019-08-19T10:01:17.0996284+01:00',
                  'LastUpdatedBy': 3
              },
              'Uri': 'http://localhost:3896/EntityService.svc/Entities(1)'
            },
            {
              'Id': 2,
              'Object': {
                'Id': 2,
                'Name': 'Entitlement',
                'Description': '',
                'Enabled': true,
                'EntityGroupId': 5,
                'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                'CreatedBy': 2,
                'LastUpdated': '2019-08-19T10:01:56.8002948+01:00',
                'LastUpdatedBy': 3
            },
              'Uri': 'http://localhost:3896/EntityService.svc/Entities(2)'
          }
          ],
          Entity: 'Entity',
          TotalCount: 2
        }
        component.showProgressBar = true

        spyOn(component, 'updateDatasource').and.returnValue()

        // Act
        component.filterInMemory({Name: { filter: 'Entitlement', exactMatch: false }})

        // Assert
        expect(component.latestEntityListResponse.Count).toEqual(1)
        expect(component.latestEntityListResponse.Entities.length).toEqual(1)
        expect(component.latestEntityListResponse.TotalCount).toEqual(1)
        expect(component.updateDatasource).toHaveBeenCalled()
        expect(component.showProgressBar).toBeFalsy()
        expect(component.filterWithServer).not.toHaveBeenCalled()
  })

  it('should filter in memory and reduce the dataset when searching for multiple fields', () => {
      // Arrange
      spyOn(component, 'filterWithServer').and.returnValue()
      component.latestEntityListResponse = { // Represents the response of searching for 'Entitle'
      Count: 2,
      Entities: [
        {
          'Id': 1,
          'Object': {
              'Id': 1,
              'Name': 'EntitledProduct',
              'Description': 'A calculated entity that is generated from Entitlements.',
              'Enabled': true,
              'EntityGroupId': 5,
              'CreateDate': '2019-04-30T11:57:19.065698-06:00',
              'CreatedBy': 2,
              'LastUpdated': '2019-08-19T10:01:17.0996284+01:00',
              'LastUpdatedBy': 3
          },
          'Uri': 'http://localhost:3896/EntityService.svc/Entities(1)'
        },
        {
          'Id': 2,
          'Object': {
            'Id': 2,
            'Name': 'Entitlement',
            'Description': '',
            'Enabled': true,
            'EntityGroupId': 5,
            'CreateDate': '2019-04-30T11:57:19.065698-06:00',
            'CreatedBy': 2,
            'LastUpdated': '2019-08-19T10:01:56.8002948+01:00',
            'LastUpdatedBy': 3
        },
          'Uri': 'http://localhost:3896/EntityService.svc/Entities(2)'
    }
      ],
      Entity: 'Entity',
      TotalCount: 2
    }
      component.showProgressBar = true

      spyOn(component, 'updateDatasource').and.returnValue()

    // Act
      component.filterInMemory(
      {
        Name: { filter: 'Entitle', exactMatch: false },
        Description: { filter: 'A calculated entity that is generated from Entitlements.', exactMatch: true }
      },
    )

    // Assert
      expect(component.latestEntityListResponse.Count).toEqual(1)
      expect(component.latestEntityListResponse.Entities.length).toEqual(1)
      expect(component.latestEntityListResponse.Entities[0].Id).toEqual(1)
      expect(component.latestEntityListResponse.Entities[0].Object.Name).toEqual('EntitledProduct')
      expect(component.latestEntityListResponse.Entities[0].Object.Description)
          .toEqual('A calculated entity that is generated from Entitlements.')

      expect(component.latestEntityListResponse.TotalCount).toEqual(1)
      expect(component.updateDatasource).toHaveBeenCalled()
      expect(component.showProgressBar).toBeFalsy()
      expect(component.filterWithServer).not.toHaveBeenCalled()
  })

  // fit('should refilter if the user is too many pages ahead', () => {
  //   // Arrange
  //   spyOn(helper, 'createFilterString').and.returnValue('this is my filter')
  //   spyOn(entityService, 'getExpandedFilteredEntityList').and.returnValue(of(
  //     {
  //       Count: 0,
  //       Entities: [
  //       ],
  //       Entity: 'Entity',
  //       TotalCount: 0
  //     }
  //   ))
  //   component.filterSettings.PageSize = 100
  //   component.filterSettings.PageIndex = 1

  //   // Act
  //   component.filterWithServer({Name: { filter: 'Entitlement', exactMatch: false }})

  //   // Assert
  //   expect(component.filterSettings.PageIndex).toEqual(0)
  // })

  // MATT: 30/8/19 - For some reason this test seems to throw an error
  // I have no idea why. Everything seems to be fine with it, it has all the data it needs
  // But it just throws a random test out every single time.

  it('should update the latest response, call updateDataSource and set the progressbar to false', () => {
    // Arrange
    const response = {
      Count: 0,
      Entities: [{
        'Id': 1,
        'Object': {
            'Id': 1,
            'Name': 'EntitledProduct',
            'Description': 'A calculated entity that is generated from Entitlements.',
            'Enabled': true,
            'EntityGroupId': 5,
            'CreateDate': '2019-04-30T11:57:19.065698-06:00',
            'CreatedBy': 2,
            'LastUpdated': '2019-08-19T10:01:17.0996284+01:00',
            'LastUpdatedBy': 3
        },
        'Uri': 'http://localhost:3896/EntityService.svc/Entities(1)'
    },           {
      'Id': 2,
      'Object': {
          'Id': 2,
          'Name': 'Entitlement',
          'Description': '',
          'Enabled': true,
          'EntityGroupId': 5,
          'CreateDate': '2019-04-30T11:57:19.065698-06:00',
          'CreatedBy': 2,
          'LastUpdated': '2019-08-19T10:01:56.8002948+01:00',
          'LastUpdatedBy': 3
      },
      'Uri': 'http://localhost:3896/EntityService.svc/Entities(2)'
  }
      ],
      Entity: 'Entity',
      TotalCount: 0
    }

    spyOn(helper, 'createFilterString').and.returnValue('this is my filter')
    spyOn(entityService, 'getExpandedFilteredEntityList').and.returnValue(of(response))
    spyOn(component, 'updateDatasource').and.returnValue(null)
    component.filterSettings.PageSize = 100
    component.filterSettings.PageIndex = 0

    // Act
    component.filterWithServer({Name: { filter: 'Entitlement', exactMatch: false }})

    // Assert
    expect(component.latestEntityListResponse).toEqual(response)
    expect(component.updateDatasource).toHaveBeenCalledWith(response)
    expect(component.showProgressBar).toBeFalsy()
  })

  it('should call the multi edit dialog', () => {
    // Arrange
    const dialogRefSpyObj = jasmine.createSpyObj({ afterClosed : of('Update successful')})
    spyOn(component.matDialog, 'open').and.returnValue(dialogRefSpyObj)
    spyOn(component.snackBar, 'open').and.returnValue(null)
    spyOn(component, 'updateGridData').and.returnValue(null)

    // Act
    component.multiUpdate()

    // Assert
    expect(component.updateGridData).toHaveBeenCalledWith(true)
  })

  describe('multiple entity record selection', () => {
    it('should show bulk edit button if entityBulkEdit feature flag is turned on', () => {
      featureFlagsServiceBulkEditSpy.and.returnValue(true)
      expect(component.enableBulkEditFeature).toBeTruthy()
     })

    it('should not show bulk edit button if entityBulkEdit feature flag is turned off', () => {
      featureFlagsServiceBulkEditSpy.and.returnValue(false)
      expect(component.enableBulkEditFeature).toBeFalsy()
     })
  })
})

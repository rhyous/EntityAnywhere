import { async, ComponentFixture, TestBed } from '@angular/core/testing'
import { RouterTestingModule } from '@angular/router/testing'
import { MatSnackBar, MatSnackBarConfig, MatDialogConfig, MatDialogRef, MatDialog } from '@angular/material'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'

import { ExtensionEntityComponent } from './extension-entity.component'
import { EntityService } from 'src/app/core/services/entity.service'
import { Observable, of } from 'rxjs'
import { MaterialModule } from 'src/app/core/material/material.module'
import { SpaceTitlePipe } from 'src/app/core/pipes/spacetitle.pipe'
import { HttpClientTestingModule } from '@angular/common/http/testing'
import { SingularizePipe } from 'src/app/core/pipes/singularize.pipe'
import { AppLocalStorageService } from 'src/app/core/services/local-storage.service'
import { FakeAppLocalStorageService } from 'src/app/core/services/mocks/mocks'

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

class SnackBarMock {
  open(message: string, action?: string, config?: MatSnackBarConfig<any>) {
    return this
  }
}

describe('ExtensionEntityComponent', () => {
  let component: ExtensionEntityComponent
  let fixture: ComponentFixture<ExtensionEntityComponent>

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        BrowserAnimationsModule,
        MaterialModule,
        HttpClientTestingModule
      ],
      providers: [
        EntityService,
        { provide: MatSnackBar, useClass: SnackBarMock },
        { provide: MatDialog, useClass: MockDialog },
        { provide: AppLocalStorageService, useClass: FakeAppLocalStorageService },
        SpaceTitlePipe,
        SingularizePipe
      ],
      declarations: [
        ExtensionEntityComponent,
        SpaceTitlePipe
      ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(ExtensionEntityComponent)
    component = fixture.componentInstance
    component.extensionEntityData = {
      Type: 'self.Addendum',
      Name: 'Addenda'
    }
    component.parentEntityId = 2703
    component.parentEntityName = 'Product'

    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })


  it('should populate the table with entries', () => {
    // Arrange
    const entityService = fixture.debugElement.injector.get(EntityService)

    spyOn(entityService, 'getExpandedFilteredEntityList').and.returnValue(of(
      {
        'Count': 1,
        'Entities': [
            {
                'Id': 16,
                'Object': {
                    'Id': 16,
                    'Name': 'Acorda Therapeutics',
                    'Category': 0,
                    'Description': null,
                    'SapId': '0000046267',
                    'CreateDate': '2018-08-02T10:04:59.853+00:00',
                    'CreatedBy': 1,
                    'LastUpdated': '2018-08-02T10:04:59.853+00:00',
                    'LastUpdatedBy': 1
                },
                'RelatedEntityCollection': [
                    {
                        'Count': 2,
                        'RelatedEntity': 'Addendum',
                        'RelatedEntities': [
                            {
                                'Id': '14',
                                'Object': {
                                    'Id': 14,
                                    'Entity': 'Organization',
                                    'EntityId': '16013',
                                    'Property': 'LicencedMachineName',
                                    'Value': 'dfdsf',
                                    'CreateDate': '2020-06-19T12:42:18.419-06:00',
                                    'CreatedBy': 2,
                                    'LastUpdated': null,
                                    'LastUpdatedBy': null
                                },
                                'Uri': 'https://site.domain.tld/Api/AddendumService.svc/Addenda/Organization/Ids(12345)'
                            },
                            {
                                'Id': '25',
                                'Object': {
                                    'Id': 25,
                                    'Entity': 'Organization',
                                    'EntityId': '16013',
                                    'Property': 'ITSM.Staging.MacAddress',
                                    'Value': '00-50-56-98-62-34',
                                    'CreateDate': '2021-01-14T13:41:12.006-07:00',
                                    'CreatedBy': 7,
                                    'LastUpdated': null,
                                    'LastUpdatedBy': null
                                },
                                'Uri': 'https://site.domain.tld/Api/AddendumService.svc/Addenda/Organization/Ids(1234)'
                            }
                        ]
                    }
                ],
                'Uri': 'https://site.domain.tld/api/OrganizationService.svc/Organizations(16)'
            }
        ],
        'Entity': 'Organization',
        'TotalCount': 1
    }
    ))

    // Act
    component.loadTable()
    fixture.detectChanges()

    // Assert
    expect(component.tableData.data.length).toEqual(2)
  })

  it('should load a mat dialog on create', () => {
    // Arrange
    const dialog = fixture.debugElement.injector.get(MatDialog)
    const dialogRefSpyObj = jasmine.createSpyObj({ afterClosed : of({}), close: null })
    spyOn(dialog, 'open').and.returnValue(dialogRefSpyObj)

    // Act
    fixture.detectChanges()
    component.createNew()

    // Assert
    expect(dialog.open).toHaveBeenCalled()
  })

  it('should call the entity service with the substring of the table name', () => {
    // Arrange
    const entityService = fixture.debugElement.injector.get(EntityService)
    const dialog = fixture.debugElement.injector.get(MatDialog)
    spyOn(entityService, 'deleteEntity').and.returnValue(of(<any>'confirmed'))
    spyOn(dialog, 'open').and.returnValue(<any>{
      afterClosed: () => of('confirmed')
    })

    component.selection.select(1)

    // Act
    component.deleteEntities()

    // Assert
    expect(entityService.deleteEntity).toHaveBeenCalledWith('Addendum', 1)
  })

  it('should call the entityService to get all permitted extension entities', () => {
    // Arrange
    component.parentEntityName = 'User'
    component.parentEntityId = 123
    component.extensionEntityName = 'Addendum'

    const entityService = fixture.debugElement.injector.get(EntityService)
    spyOn(entityService, 'getExpandedFilteredEntityList').and.returnValue(of())

    // Act
    component.loadTable()

   // Assert
    expect(entityService.getExpandedFilteredEntityList).toHaveBeenCalledWith(`${component.parentEntityName}`,
        `Id eq ${component.parentEntityId}`,
        [component.extensionEntityName],
        100,
        0)
  })
})

import { ComponentFixture, async, TestBed } from '@angular/core/testing'
import { MatProgressBarModule, MatIconModule, MatCheckboxModule,
  MatFormFieldModule, MatTableModule, MatPaginatorModule, MatSnackBarModule, MatDialogModule } from '@angular/material'
import { Pipe, PipeTransform } from '@angular/core'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { ReactiveFormsModule } from '@angular/forms'
import { RouterTestingModule } from '@angular/router/testing'
import { Params, Router, ActivatedRoute } from '@angular/router'
import { By } from '@angular/platform-browser'
import { DecimalPipe } from '@angular/common'

import { EntityService } from 'src/app/core/services/entity.service'
import { EntitiesListComponent } from './entities-list.component'
import { ErrorReporterDialogComponent } from 'src/app/core/dialogs/error-reporter-dialog/error-reporter-dialog.component'
import { SpaceTitlePipe } from 'src/app/core/pipes/spacetitle.pipe'
import { PluralizePipe } from 'src/app/core/pipes/pluralize.pipe'
import { EntityMetadataService } from 'src/app/core/services/entity-metadata.service'
import { of, Observable, throwError } from 'rxjs'

import '../../../../../../core/infrastructure/linq'
import { HttpClientTestingModule } from '@angular/common/http/testing'
import { UserDataService } from 'src/app/core/services/user-data.service'
import { AppLocalStorageService } from 'src/app/core/services/local-storage.service'
import { UserEntityService } from 'src/app/features/eaf/eaf-custom/services/user-entity.service'



@Pipe({name: 'MockTitlePipe'})
export class MockTitlePipe implements PipeTransform {
    transform(value: number): number {
        return value
    }
}

@Pipe({name: 'MockPluralPipe'})
export class MockPluralPipe implements PipeTransform {
    transform(value: number): number {
        return value
    }
}

const mockAppLocalStorageServiceAdminUser = {
  User: {
    AdminRole: {
        Name: 'Role',
        Issuer: 'LOCAL AUTHORITY',
        Subject: 'UserRole',
        Value: 'Admin',
        ValueType: null
    }
  }
}

describe('EntitiesList', () => {
  let component: EntitiesListComponent
  let fixture: ComponentFixture<EntitiesListComponent>
  let mockParams, mockActivatedRoute
  let entityService: EntityService
  let entityMetadataService: EntityMetadataService
  let errorReporter: ErrorReporterDialogComponent
  let userDataService: UserDataService
  let userDataServicePermittedEntitiesForUserPropertySpy: jasmine.Spy
  let localStorageService: AppLocalStorageService
  let localStorageUserPropertySpy: jasmine.Spy

  const mockRouter = {
    navigate: jasmine.createSpy('navigate')
  }

  beforeEach(async(() => {
    mockParams = of<Params>({id: 'add'})
    mockActivatedRoute = {params: mockParams}

    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        BrowserAnimationsModule,
        MatProgressBarModule,
        MatIconModule,
        MatCheckboxModule,
        MatTableModule,
        MatPaginatorModule,
        MatSnackBarModule,
        MatDialogModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        HttpClientTestingModule
      ],
      providers: [
        {provide: SpaceTitlePipe, useClass: MockTitlePipe},
        {provide: PluralizePipe, useClass: MockPluralPipe},
        {provide: ActivatedRoute, useValue: mockActivatedRoute},
        {provide: Router, useValue: mockRouter},
        AppLocalStorageService,
        EntityService,
        EntityMetadataService,
        ErrorReporterDialogComponent,
        DecimalPipe,
        UserDataService
      ],
      declarations: [
        SpaceTitlePipe,
        PluralizePipe,
        EntitiesListComponent
      ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(EntitiesListComponent)
    component = fixture.componentInstance
    entityService = TestBed.get(EntityService)
    entityMetadataService = TestBed.get(EntityMetadataService)
    errorReporter = TestBed.get(ErrorReporterDialogComponent)
    localStorageService = TestBed.get(AppLocalStorageService)
    userDataService = TestBed.get(UserDataService)

    localStorageUserPropertySpy = spyOnProperty(localStorageService, 'User', 'get').and.returnValue(mockAppLocalStorageServiceAdminUser.User)

    const emptyStringArray: string[] = []

    // tslint:disable-next-line: max-line-length
    userDataServicePermittedEntitiesForUserPropertySpy = spyOnProperty(userDataService, 'permittedEntitiesForUser', 'get').and.returnValue(emptyStringArray)

    spyOn(entityService, 'getEntities').and.returnValue(of({
          '$EntityContainer': 'EAF',
          '$Version': '4.01',
          'EAF': {
            'ActivationAttempt': {
              '$Kind': 'EntityType',
              'Id': {'$Type': 'Edm.Int32'},
              '@EAF.EntityGroup': 'Management',
              'HostName': {'$Collection': true, '$Type': 'Edm.String'},
              'Username': {'$Collection': true, '$Type': 'Edm.String'},
              'Password': {'$Collection': true, '$Type': 'Edm.String'},
              'CreateDate': {'$Type': 'Edm.Date'}
            },
            'Addendum': {
              '$Kind': 'EntityType',
              '@EAF.EntityGroup': 'Data Management',
              'Entity': {'$Collection': true, '$Type': 'Edm.String'},
              'EntityId': {'$Collection': true, '$Type': 'Edm.String'},
              'Property': {'$Collection': true, '$Type': 'Edm.String'},
              'Value': {'$Collection': true, '$Type': 'Edm.String'},
              'CreateDate': {'$Type': 'Edm.Date'},
              'LastUpdated': {'$Type': 'Edm.Date'},
              'CreatedBy': {'$Type': 'Edm.Int64'},
              'LastUpdatedBy': {'$Type': 'Edm.Int64'},
              'Id': {'$Type': 'Edm.Int64'}
            },
            'AlternateIds': {
              '$Kind': 'EntityType',
              '@EAF.EntityGroup': 'Management',
              'Entity': {'$Collection': true, '$Type': 'Edm.String'},
              'EntityId': {'$Collection': true, '$Type': 'Edm.String'},
              'Property': {'$Collection': true, '$Type': 'Edm.String'},
              'Value': {'$Collection': true, '$Type': 'Edm.String'},
              'CreateDate': {'$Type': 'Edm.Date'},
              'LastUpdated': {'$Type': 'Edm.Date'},
              'CreatedBy': {'$Type': 'Edm.Int64'},
              'LastUpdatedBy': {'$Type': 'Edm.Int64'},
              'Id': {'$Type': 'Edm.Int64'}
            }}}))
    fixture.detectChanges()
  })

  it('should create', () => {
    // Arrange

    // Act

    // Assert
    expect(component).toBeTruthy()
    expect(component.showProgressBar).toEqual(false)
  })

  describe('for user with admin role', () => {

    it('should set the entities array', () => {
      // Arrange

      // Act
      component.ngOnInit()
      fixture.detectChanges()

      // Assert
      expect(component.entitiesArray.length).toEqual(2) // There should be 2 groups

      expect(component.entitiesArray[0].data.length).toEqual(1) // The first group should have one item in it
      expect(component.entitiesArray[0].group).toBe('Data Management') // It should be related to data management
      expect(component.entitiesArray[0].data[0].entityGroup).toBe('Data Management')
      expect(component.entitiesArray[0].data[0].entityName).toBe('Addendum') // It should be the Addendum Entity

      expect(component.entitiesArray[1].data.length).toEqual(2) // The second group should have 2 items in it
      expect(component.entitiesArray[1].group).toBe('Management') // It should be related to Management
      expect(component.entitiesArray[1].data[0].entityGroup).toBe('Management')
      expect(component.entitiesArray[1].data[0].entityName).toBe('ActivationAttempt') // The first item should be ActivationAttempt
      expect(component.entitiesArray[1].data[1].entityGroup).toBe('Management')
      expect(component.entitiesArray[1].data[1].entityName).toBe('AlternateIds') // The second item should be Alternate Ids
    })

    it('should create the menu buttons and headers for the groups', () => {
      // Arrange

      // Act
      component.ngOnInit()
      fixture.detectChanges()

      const buttons = fixture.debugElement.queryAll(By.css('button'))
      const headers = fixture.debugElement.queryAll(By.css('h3'))

      // Assert
      expect(buttons.length).toEqual(3) // There should be 3 buttons
      expect(headers.length).toEqual(2) // There should be 2 groups
      expect(headers[0].nativeElement.textContent).toContain('Data Management') // The first header should be Data Management
      expect(headers[1].nativeElement.textContent).toContain('Management') // The second header should be Management
    })

    it('should navigate to the correct entity', () => {
      component.toEntityList('Product')
      expect(mockRouter.navigate).toHaveBeenCalledWith(['/admin/data-management/Products'])
    })
  })

  describe('for user with limited entities access', () => {
    const permittedEntities = [
      'ActivationAttempt',
      'AlternateIds'
  ]
    beforeEach(() => {
      userDataServicePermittedEntitiesForUserPropertySpy.and.returnValue(permittedEntities)
      localStorageUserPropertySpy.and.returnValue({AdminUser: null})

    })

    it('should set the entities array with permitted list', () => {
      // Arrange

      // Act
      component.ngOnInit()
      fixture.detectChanges()

      // Assert
      expect(component.entitiesArray.length).toEqual(1) // There should be 2 groups

      expect(component.entitiesArray[0].data.length).toEqual(2) // The first group should have 2 items in it
      expect(component.entitiesArray[0].group).toBe('Management') // It should be related to Management
      expect(component.entitiesArray[0].data[0].entityGroup).toBe('Management')
      expect(component.entitiesArray[0].data[0].entityName).toBe('ActivationAttempt') // The first item should be ActivationAttempt
      expect(component.entitiesArray[0].data[1].entityGroup).toBe('Management')
      expect(component.entitiesArray[0].data[1].entityName).toBe('AlternateIds') // The second item should be Alternate Ids
    })

    it('should create the menu buttons and headers for only permitted entity groups', () => {
      // Arrange


      // Act
      component.ngOnInit()
      fixture.detectChanges()
      const buttons = fixture.debugElement.queryAll(By.css('button'))
      const headers = fixture.debugElement.queryAll(By.css('h3'))

      // Assert
      expect(buttons.length).toEqual(2) // There should be 2 buttons
      expect(headers.length).toEqual(1) // There should be 1 group
      expect(headers[0].nativeElement.textContent).toContain('Management') // The first header should be Data Management
    })

  })

})

describe('EntitiesList Error Conditions', () => {
  let component: EntitiesListComponent
  let fixture: ComponentFixture<EntitiesListComponent>
  let mockParams, mockActivatedRoute
  let entityService: EntityService
  let entityMetadataService: EntityMetadataService
  let errorReporter: ErrorReporterDialogComponent
  const mockRouter = {
    navigate: jasmine.createSpy('navigate')
  }

  beforeEach(async(() => {
    mockParams = of<Params>({id: 'add'})
    mockActivatedRoute = {params: mockParams}

    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        BrowserAnimationsModule,
        MatProgressBarModule,
        MatIconModule,
        MatCheckboxModule,
        MatTableModule,
        MatPaginatorModule,
        MatSnackBarModule,
        MatDialogModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        HttpClientTestingModule
      ],
      providers: [
        {provide: SpaceTitlePipe, useClass: MockTitlePipe},
        {provide: PluralizePipe, useClass: MockPluralPipe},
        {provide: ActivatedRoute, useValue: mockActivatedRoute},
        {provide: Router, useValue: mockRouter},
        {provide: AppLocalStorageService, useValue: mockAppLocalStorageServiceAdminUser},
        EntityMetadataService,
        ErrorReporterDialogComponent,
        DecimalPipe,
        UserEntityService
      ],
      declarations: [
        SpaceTitlePipe,
        PluralizePipe,
        EntitiesListComponent
      ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(EntitiesListComponent)
    component = fixture.componentInstance
    entityService = TestBed.get(EntityService)
    entityMetadataService = TestBed.get(EntityMetadataService)
    errorReporter = TestBed.get(ErrorReporterDialogComponent)

    const errorMessage = {status: 500, Acknowledgable: false, error: 'There was an error'}
    spyOn(entityService, 'getEntities').and.returnValue(throwError(errorMessage))
    spyOn(errorReporter, 'displayMessage').and.returnValue()
    fixture.detectChanges()
  })

  it('Should throw an error', () => {
    expect(errorReporter.displayMessage).toHaveBeenCalledTimes(1)
    expect(errorReporter.displayMessage).toHaveBeenCalledWith('There was an error')
    expect(component.showProgressBar).toEqual(false)
  })
})

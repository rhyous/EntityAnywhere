// Core
import { TestBed, ComponentFixture, waitForAsync } from '@angular/core/testing'
import { DatePipe, CommonModule } from '@angular/common'
import { RouterTestingModule } from '@angular/router/testing'
import { Pipe, PipeTransform, Component } from '@angular/core'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { Params, ActivatedRoute, Router } from '@angular/router'
import { ReactiveFormsModule } from '@angular/forms'
import { FlexLayoutModule } from '@angular/flex-layout'
import { of } from 'rxjs'

// Local
import { EntityService } from 'src/app/core/services/entity.service'
import { UserDetailData } from './mock-user-detail-data'
import { EntityDetailComponent } from './entity-detail.component'
import { ErrorReporterDialogComponent } from 'src/app/core/dialogs/error-reporter-dialog/error-reporter-dialog.component'
import { WcfFormatPipe } from 'src/app/core/pipes/wcf-format.pipe'
import { EntityHelperService } from '../../../common-form-module/services/entity-helper.service'
import { EntityPropertyControlService } from '../../../common-form-module/services/entity-property-control.service'
import { environment } from 'src/environments/environment'
import { EntityMetadata } from 'src/app/core/models/concretes/entity-metadata'
import { EntityField } from '../../../common-form-module/models/concretes/entity-field'
import { HttpClientTestingModule } from '@angular/common/http/testing'
import { BrowserDynamicTestingModule } from '@angular/platform-browser-dynamic/testing'
import { MaterialModule } from 'src/app/core/material/material.module'
import { SpaceTitlePipe } from 'src/app/core/pipes/spacetitle.pipe'
import { EntityFormComponent } from '../../../common-form-module/components/entity-form/entity-form.component'
import { ExtensionEntityComponent } from '../../../common-form-module/components/extension-entity/extension-entity.component'
import { ForeignEntityComponent } from '../../../common-form-module/components/foreign-entity/foreign-entity.component'
import { EntityFieldDirective } from 'src/app/features/eaf/eaf-custom/directives/entity-field.directive'
// tslint:disable-next-line: max-line-length
import { ExtensionEntityDialogComponent } from '../../../common-dialogs-module/components/extension-entity-dialog/extension-entity-dialog.component'
import { PluralizePipe } from 'src/app/core/pipes/pluralize.pipe'
import { ArraySortOrderPipe } from 'src/app/core/pipes/array-sort-order.pipe'
import { AppLocalStorageService } from 'src/app/core/services/local-storage.service'
import { FakeAppLocalStorageService } from 'src/app/core/services/mocks/mocks'
import { SingularizePipe } from 'src/app/core/pipes/singularize.pipe'
import { UserDataService } from 'src/app/core/services/user-data.service'
import { EntityMetadataService } from 'src/app/core/services/entity-metadata.service'
import { EntityConfigBuilder } from '../../../common-form-module/services/entity-config-builder'
import { EntityPropertyTypeControlTypeMap } from '../../../common-form-module/services/entity-property-type-control-type.map'
import { EntityPropertyTypeInputTypeMap } from '../../../common-form-module/services/entity-property-type-input-type.map'
import { EntityFieldValidatorProvider } from '../../../common-form-module/services/entity-field-validator-provider'
import { EnumOptionMapper } from '../../../common-form-module/services/enum-option-mapper'
import { StringTypeControlTypeMap } from '../../../common-form-module/services/string-type-control-type.map'
import { CustomPluralizationMap } from 'src/app/core/models/concretes/custom-pluralization-map'
import { SplitPascalCasePipe } from 'src/app/core/pipes/split-pascal-case.pipe'
import { WellKnownProperties } from 'src/app/core/models/concretes/well-known-properties'

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

@Pipe({name: 'spacetitle'})
export class MockSpaceTitlePipe implements PipeTransform {
    transform(value: string): string {
        return value
    }
}

describe('Entity Detail Component Add', () => {
  let fixture: ComponentFixture<EntityDetailComponent>
  let component: EntityDetailComponent
  let mockParams, mockActivatedRoute
  let entityService: EntityService
  let helper: EntityHelperService
  let epcs: EntityPropertyControlService
  let entityConfigBuilder: EntityConfigBuilder
  const userDetailData = new UserDetailData()
  let errorReporterDialog: ErrorReporterDialogComponent
  const mockRouter = {
    navigate: jasmine.createSpy('navigate')
  }
  let entityMetadataService: EntityMetadataService

  beforeEach(waitForAsync(() => {

    mockParams = of<Params>({entityPlural: 'Users', id: 'add', entity: 'User'})
    mockActivatedRoute = {params: mockParams}

    TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        HttpClientTestingModule,
        FlexLayoutModule,
        RouterTestingModule,
        BrowserAnimationsModule,
        MaterialModule,
        CommonModule,
      ],
      providers: [
        {provide: DatePipe, useClass: MockDatePipe},
        {provide: ActivatedRoute, useValue: mockActivatedRoute},
        {provide: Router, useValue: mockRouter},
        {provide: WcfFormatPipe, useClass: MockWcfPipe},
        { provide: AppLocalStorageService, useClass: FakeAppLocalStorageService },
        SpaceTitlePipe,
        PluralizePipe,
        SingularizePipe,
        CustomPluralizationMap,
        SplitPascalCasePipe,
        EntityService,
        ErrorReporterDialogComponent,
        EntityHelperService,
        EntityPropertyControlService,
        ArraySortOrderPipe,
        EntityConfigBuilder,
        EntityPropertyTypeControlTypeMap,
        EntityPropertyTypeInputTypeMap,
        EntityFieldValidatorProvider,
        EnumOptionMapper,
        StringTypeControlTypeMap,
        SpaceTitlePipe,
        WellKnownProperties
      ],
      declarations: [
        ExtensionEntityDialogComponent,
        ErrorReporterDialogComponent,
        EntityDetailComponent,
        EntityFormComponent,
        ExtensionEntityComponent,
        ForeignEntityComponent,
        EntityFieldDirective,
        SpaceTitlePipe,
        PluralizePipe,
        SingularizePipe
      ]
    })
    .overrideModule(BrowserDynamicTestingModule, {set: {entryComponents: [ErrorReporterDialogComponent, ExtensionEntityDialogComponent]}})
    .compileComponents()
    fixture = TestBed.createComponent(EntityDetailComponent)
    entityService = TestBed.inject(EntityService)
    helper = TestBed.inject(EntityHelperService)
    errorReporterDialog = TestBed.inject(ErrorReporterDialogComponent)
    epcs = TestBed.inject(EntityPropertyControlService)
    entityConfigBuilder = TestBed.inject(EntityConfigBuilder)
    entityMetadataService = TestBed.inject(EntityMetadataService)

    component = fixture.componentInstance
  }))

  it('should create', () => {
    expect(component).toBeTruthy()
  })

  it('should setup initialise correctly', () => {
    spyOn(component, 'formInitialization').and.returnValue()
    component.ngOnInit()
    expect(component.formInitialization).toHaveBeenCalledTimes(1)
  })

  it('should setup the form correctly', () => {

    const md = new EntityMetadata()
    md.Name = 'Feature'
    md.Kind = 'EntityType'
    const field1 = new EntityField()
    field1.Name = 'CreateDate'
    field1.Type = 'Edm.Date'
    field1.DisplayOrder = 0
    const field2 = new EntityField()
    field2.Name = 'Description'
    field2.Type = 'Edm.String'
    field2.DisplayOrder = 1
    const field3 = new EntityField()
    field3.Name = 'Name'
    field3.Type = 'Edm.String'
    field3.DisplayOrder = 2

    md.Fields.push(<any>field1, <any>field2, <any>field3)

    spyOn(entityConfigBuilder, 'build').and.callThrough()
    component.entityConfigs = []
    component.setupForm(md, <any>{id: null, entity: 'User'}, undefined)

    expect(component.entityConfigs.length).toEqual(2)
  })

  it('should initialize the form', () => {
    const featureMetadata = new EntityMetadata()
    featureMetadata.Key = ['Id']
    const idField = new EntityField()
    idField.Name = 'Id'
    idField.Type = 'Edm.Int32'
    featureMetadata.Fields.push()
    spyOn(entityMetadataService, 'getEntity').and.returnValue(featureMetadata)
    component.entityName = 'Feature'
    spyOn(component, 'setupForm').and.returnValue()
    component.formInitialization( <any>{id: null, entity: 'User'}, undefined)

    expect(component.setupForm).toHaveBeenCalled()
    expect(component.showProgressBar).toEqual(false)
  })

  it('should call the entity service to add', () => {
    // Arrange
    spyOn(entityService, 'addEntity').and.returnValue(of(<any>{}))
    const entity = {Id: 4, Name: 'Test', Description: 'This is a test'}
    component.entityName = 'User'

    const ent =  [{ name: 'Name', value: 'Test', type: 'input' },
                  { name: 'Description', value: 'Testing', type: 'input' },
                  { name: 'Id', value: '4', type: 'input' },
                  { name: 'StartDate', value: '2021-02-13T14:11:36.153', type: 'date'}]

    component.entityConfigs = ent

    // Act
    component.addEntity(entity)

    // Assert
    expect(entityService.addEntity).toHaveBeenCalledWith('User', [entity])
  })

  it('should display a message on success add', () => {
    component.entityName = 'User'
    spyOn(component.snackBar, 'open').and.returnValue(<any>null)
    component.onPatchSuccess(
      {Id: 4, Object: { Name: 'Test', Description: 'This is a description'}})
    expect(component.snackBar.open).toHaveBeenCalledWith('User added successfully')
    expect(mockRouter.navigate).toHaveBeenCalledWith(['admin/data-administration/User/4'])
  })

  it('should submit an add request', () => {
    component.params = {
      entity: 'User',
      id: 'add'
    }
    spyOn(component, 'addEntity').and.returnValue(of(<any>{}))
    spyOn(component, 'onSaveSuccess').and.returnValue()
    spyOn(helper, 'removeAuditableProperties').and.returnValue(userDetailData.getFormValue())

    component.submit('')

    expect(component.addEntity).toHaveBeenCalledTimes(1)
    expect(component.addEntity).toHaveBeenCalledWith(userDetailData.getFormValue())
  })

  it('should handle a save failure', () => {
    spyOn(errorReporterDialog, 'displayMessage').and.returnValue()
    const error = {error: {Acknowledgeable: false, Message: 'This is an error'}}
    component.onSaveFailure(error)
    expect(errorReporterDialog.displayMessage).toHaveBeenCalledWith(<any>{Message: 'This is an error', Acknowledgeable: false})
  })

})

describe('Entity Detail Component Update', () => {
  let fixture: ComponentFixture<EntityDetailComponent>
  let component: EntityDetailComponent
  let mockParams, mockActivatedRoute
  let entityService: EntityService
  let helper: EntityHelperService
  const userDetailData = new UserDetailData()

  beforeEach(waitForAsync(() => {

    mockParams = of<Params>({entityPlural: 'Users', id: '4', entity: 'User'})
    mockActivatedRoute = {params: mockParams}

    TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        HttpClientTestingModule,
        FlexLayoutModule,
        RouterTestingModule,
        BrowserAnimationsModule,
        MaterialModule,
        CommonModule,
      ],
      providers: [
        {provide: DatePipe, useClass: MockDatePipe},
        {provide: ActivatedRoute, useValue: mockActivatedRoute},
        {provide: WcfFormatPipe, useClass: MockWcfPipe},
        { provide: AppLocalStorageService, useClass: FakeAppLocalStorageService },
        SpaceTitlePipe,
        PluralizePipe,
        EntityService,
        ErrorReporterDialogComponent,
        EntityHelperService,
        EntityPropertyControlService,
        ArraySortOrderPipe,
        SingularizePipe,
        CustomPluralizationMap,
        SplitPascalCasePipe,
        EntityConfigBuilder,
        EntityPropertyTypeControlTypeMap,
        EntityPropertyTypeInputTypeMap,
        EntityFieldValidatorProvider,
        EnumOptionMapper,
        StringTypeControlTypeMap,
        SpaceTitlePipe,
        WellKnownProperties
      ],
      declarations: [
        ExtensionEntityDialogComponent,
        ErrorReporterDialogComponent,
        EntityDetailComponent,
        EntityFormComponent,
        ExtensionEntityComponent,
        ForeignEntityComponent,
        EntityFieldDirective,
        SpaceTitlePipe,
        PluralizePipe,
        SingularizePipe
      ]
    })
    .overrideModule(BrowserDynamicTestingModule, {set: {entryComponents: [ErrorReporterDialogComponent, ExtensionEntityDialogComponent]}})
    .compileComponents()
    fixture = TestBed.createComponent(EntityDetailComponent)
    entityService = TestBed.inject(EntityService)
    helper = TestBed.inject(EntityHelperService)
    spyOn(entityService, 'getEntityData').and.returnValue(of(userDetailData.getUserDetailData()))

    component = fixture.componentInstance
  }))

  it('should create', () => {
    expect(component).toBeTruthy()
  })


  it('should submit an update request', () => {
    spyOn(component, 'updateEntity').and.returnValue(of(''))
    spyOn(helper, 'removeAuditableProperties').and.returnValue(userDetailData.getFormValue())
    spyOn(component, 'onSaveSuccess').and.returnValue()
    component.params = {
      entity: 'User',
      id: 4
    }

    const ent =  [{type: 'input', label: 'Username', inputType: 'text', name: 'Username', entity: 'User'},
                  {type: 'input', label: 'Password', inputType: 'text', name: 'Password', entity: 'User'},
                  {type: 'input', label: 'Salt', inputType: 'text', name: 'Salt', entity: 'User'},
                  {type: 'checkbox', label: 'Enabled', inputType: 'text', name: 'Enabled', entity: 'User'},
                  {type: 'checkbox', label: 'ExternalAuth', inputType: 'text', name: 'ExternalAuth', entity: 'User'},
                  {type: 'checkbox', label: 'IsHashed', inputType: 'text', name: 'IsHashed', entity: 'User'},
                  {type: 'EntitySearcher', label: 'Organization', inputType: 'number', name: 'OrganizationId', entity: 'User'},
                  {type: 'button', label: 'Update', entity: 'User', flex: 30, order: 9}]

    component.entityConfigs = ent
    component.entityId = '4'
    component.submit(userDetailData.getFormValue())

    expect(component.updateEntity).toHaveBeenCalledTimes(1)
    expect(helper.removeAuditableProperties).toHaveBeenCalledTimes(1)
    expect(component.updateEntity).toHaveBeenCalledWith(userDetailData.getFormValue())
  })

  it('should update a record', () => {
    // Arrange
    const ent =  [{ name: 'Name', value: 'Test', type: 'input' },
                  { name: 'Description', value: 'Testing', type: 'input' },
                  { name: 'Id', value: '4', type: 'input' },
                  { name: 'StartDate', value: '2021-02-13T14:11:36.153-00:00', type: 'date'}]

    component.entityConfigs = ent
    component.entityId = '4'
    component.entityName = 'User'

    spyOn(entityService, 'patchEntity').and.returnValue(of(<any>{}))
    const entity = {Id: '4', Name: 'Test1', Description: 'This is a test', StartDate: '2021-02-14T14:11:36.153-00:00'}
    const patchEntity = {ChangedProperties: ['Name', 'Description', 'StartDate'],
                         Entity: {Name: 'Test1', Description: 'This is a test', StartDate: '/Date(1613311896153-00:00)/'}}

    // Act
    component.updateEntity(entity)

    // Assert
    expect(entityService.patchEntity).toHaveBeenCalledWith('User', '4', patchEntity)
  })

  it('shouldnt update the entity if nothing has changed', () => {
    // Arrange
    const ent =  [{ name: 'Name', value: 'Test', type: 'input' },
                  { name: 'Description', value: 'Testing', type: 'input' },
                  { name: 'Id', value: '4', type: 'input' }]

    component.entityConfigs = ent
    spyOn(component.snackBar, 'open').and.returnValue(<any>null)
    spyOn(entityService, 'patchEntity').and.returnValue(<any>null)

    // Act
    component.updateEntity({Name: 'Test', Description: 'Testing', Id: '4'})

    // Assert
    expect(component.snackBar.open).toHaveBeenCalledWith('No changes detected')
    expect(entityService.patchEntity).not.toHaveBeenCalled()
  })

  it('should display a message on success update', () => {
    spyOn(component.snackBar, 'open').and.returnValue(<any>null)
    const ent =  [{ name: 'Name', value: 'Test', type: 'input' },
                  { name: 'Description', value: 'Testing', type: 'input' },
                  { name: 'Id', value: '4', type: 'input' }]

    component.entityConfigs = ent
    component.entityId = '4'
    component.entityName = 'User'

    component.onPatchSuccess(
      {Id: 4, Object: { Name: 'Test', Description: 'This is a description'}})
    expect(component.snackBar.open).toHaveBeenCalledWith('User 4 saved successfully')
  })

  it('should update the entityConfig on success update', () => {
    // Arrange
    const ent =  [{ name: 'Name', value: 'Test', type: 'input' },
                  { name: 'Description', value: 'Testing', type: 'input' },
                  { name: 'Id', value: '4', type: 'input' }]
    component.entityConfigs = ent

    component.entityId = '4'
    component.entityName = 'Test'
    spyOn(component.snackBar, 'open').and.returnValue(<any>null)

    // Act
    component.onPatchSuccess(
      {Id: 4, Object: { Name: 'Test', Description: 'Updated'}})

    // Assert
    expect(component.entityConfigs.find(x => x.name === 'Description')?.value).toEqual('Updated')
    expect(component.snackBar.open).toHaveBeenCalledWith('Test 4 saved successfully')
  })

  it ('should setup a form', () => {
    const md = new EntityMetadata()
    md.Name = 'Product'
    const field1 = new EntityField()
    field1.Name = 'Name'
    field1.Type = 'Edm.String'

    const field2 = new EntityField()
    field2.Name = 'Features'
    field2.Collection = true
    field2.Kind = 'NavigationProperty'
    field2.Nullable = true
    field2.Type = 'self.Feature'
    field2.RelatedEntityType = 'Mapping'
    field2.MappingEntity = 'ProductFeatureMap'
    md.Fields.push(<any>field1, <any>field2)

    component.setupForm(md, {entity: 'User', id: 3}, {Id: 3, Name: 'Fred'})
    expect(component.entityConfigs.length).toEqual(1) // Name
    expect(component.mappingComponents.length).toEqual(2) // Title and mapper
  })
})



describe('entity detail component clone url', () => {
  let fixture: ComponentFixture<EntityDetailComponent>
  let component: EntityDetailComponent
  let mockActivatedRoute
  let mockParams
  let epcs: EntityPropertyControlService
  const router: any = {
    navigate: () => {}
  }

  beforeEach(waitForAsync(() => {

    mockParams = of<Params>({entityPlural: 'Users', id: 'clone', entity: 'User'})
    mockActivatedRoute = {params: mockParams}

    TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        HttpClientTestingModule,
        FlexLayoutModule,
        RouterTestingModule,
        BrowserAnimationsModule,
        MaterialModule,
        CommonModule,
      ],
      providers: [
        {provide: DatePipe, useClass: MockDatePipe},
        {provide: ActivatedRoute, useValue: mockActivatedRoute},
        {provide: WcfFormatPipe, useClass: MockWcfPipe},
        { provide: AppLocalStorageService, useClass: FakeAppLocalStorageService },
        SpaceTitlePipe,
        PluralizePipe,
        EntityService,
        CustomPluralizationMap,
        SplitPascalCasePipe,
        ErrorReporterDialogComponent,
        EntityHelperService,
        EntityPropertyControlService,
        ArraySortOrderPipe,
        SingularizePipe,
        EntityConfigBuilder,
        EntityPropertyTypeControlTypeMap,
        EntityPropertyTypeInputTypeMap,
        EntityFieldValidatorProvider,
        EnumOptionMapper,
        StringTypeControlTypeMap,
        SpaceTitlePipe,
        WellKnownProperties
      ],
      declarations: [
        ExtensionEntityDialogComponent,
        ErrorReporterDialogComponent,
        EntityDetailComponent,
        EntityFormComponent,
        ExtensionEntityComponent,
        ForeignEntityComponent,
        EntityFieldDirective,
        SpaceTitlePipe,
        PluralizePipe,
        SingularizePipe,
        SplitPascalCasePipe
       ]
    })
    .overrideModule(BrowserDynamicTestingModule, {set: {entryComponents: [ErrorReporterDialogComponent, ExtensionEntityDialogComponent]}})
    .compileComponents()

    fixture = TestBed.createComponent(EntityDetailComponent)
    epcs = TestBed.inject(EntityPropertyControlService)

    component = fixture.componentInstance
  }))

  it('should reset the title if the user is on a clone url', () => {
    // Arrange
    spyOn(router, 'navigate').and.returnValue('')
    component.entityConfigs = [{
      type: 'number'
    }]
    component.entityId = 'Clone'
    component.entityName = 'User'

    // Act
    component.ngOnInit()

    // Assert
    expect(router.navigate).not.toHaveBeenCalled()
    expect(component.title).toEqual('Clone User')
  })

})

describe ('entity detail component permitted entities of user', () => {
  let fixture: ComponentFixture<EntityDetailComponent>
  let component: EntityDetailComponent
  let mockActivatedRoute
  let mockParams
  let mockUserDataService
  let mockEntityService
  let epcs: EntityPropertyControlService
  let entityMetadataService: EntityMetadataService
  const router: any = {
    navigate: () => {}
  }

  beforeEach(waitForAsync(() => {
    mockParams = of<Params>({entityPlural: 'Users', id: '1321', entity: 'User'})
    mockActivatedRoute = {params: mockParams}
    mockUserDataService = {
      get permittedEntitiesForUser() {
        return ['Addendum', 'User' , 'Organization']
      },
      userIsAdmin() {
        return false
      },
      canDisplayEntityForUser(entityName: string) {
        return !this.userIsAdmin() ? this.permittedEntitiesForUser.some((x: string) => x === entityName) : true
      }
    }

    mockEntityService = {
      getExpandedFilteredEntityList() {return of()}
    }

    TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        HttpClientTestingModule,
        FlexLayoutModule,
        RouterTestingModule,
        BrowserAnimationsModule,
        MaterialModule,
        CommonModule,
      ],
      providers: [
        {provide: DatePipe, useClass: MockDatePipe},
        {provide: ActivatedRoute, useValue: mockActivatedRoute},
        {provide: WcfFormatPipe, useClass: MockWcfPipe},
        {provide: AppLocalStorageService, useClass: FakeAppLocalStorageService },
        {provide: UserDataService, useValue: mockUserDataService},
        SpaceTitlePipe,
        PluralizePipe,
        CustomPluralizationMap,
        SplitPascalCasePipe,
        {provide: EntityService, useValue: mockEntityService },
        ErrorReporterDialogComponent,
        EntityHelperService,
        EntityPropertyControlService,
        ArraySortOrderPipe,
        SingularizePipe,
        EntityConfigBuilder,
        EntityPropertyTypeControlTypeMap,
        EntityPropertyTypeInputTypeMap,
        EntityFieldValidatorProvider,
        EnumOptionMapper,
        StringTypeControlTypeMap,
        SpaceTitlePipe,
        WellKnownProperties
      ],
      declarations: [
        ExtensionEntityDialogComponent,
        ErrorReporterDialogComponent,
        EntityDetailComponent,
        EntityFormComponent,
        ExtensionEntityComponent,
        ForeignEntityComponent,
        EntityFieldDirective,
        SpaceTitlePipe,
        PluralizePipe,
        SingularizePipe,
        SplitPascalCasePipe,
       ]
    })
    .overrideModule(BrowserDynamicTestingModule, {set: {entryComponents: [ErrorReporterDialogComponent, ExtensionEntityDialogComponent]}})
    .compileComponents()


    fixture = TestBed.createComponent(EntityDetailComponent)
    epcs = TestBed.inject(EntityPropertyControlService)
    entityMetadataService = TestBed.inject(EntityMetadataService)

    component = fixture.componentInstance
  }))


  it ('should display all extension entities of user', () => {
    // Arrange
    const md = new EntityMetadata()
    md.Name = 'User'
    md.Kind = 'EntityType'
    const field1 = new EntityField()
    field1.Name = 'Addenda'
    field1.Type = 'Extension'
    field1.DisplayOrder = 3
    field1.RelatedEntityType = 'Extension'
    const field2 = new EntityField()
    field2.Name = 'AlternateIds'
    field2.Type = 'Extension'
    field2.DisplayOrder = 2
    field2.RelatedEntityType = 'Extension'
    const field3 = new EntityField()
    field3.Name = 'UserRoles'
    field3.Type = 'Extension'
    field3.DisplayOrder = 3
    field3.RelatedEntityType = 'Extension'

    md.Fields.push(<any>field1, <any>field2, <any>field3)

    // Act
    component.setupForm(md, {id: <any>null, entity: 'User'}, undefined)

    // Assert
    expect(component.extensionEntities.length).toEqual(3)

  })

  it ('should only display permitted mapping entities of user', () => {
    // Arrange
    component.mappingComponents = []
    const md = new EntityMetadata()
    md.Name = 'User'
    md.Kind = 'EntityType'
    const field1 = new EntityField()
    field1.Name = 'UserRoles'
    field1.Type = 'Mapping'
    field1.DisplayOrder = 1
    field1.Kind = 'Mapping'
    field1.RelatedEntityType = 'Mapping'
    field1.MappingEntity = 'UserRole'
    const field2 = new EntityField()
    field2.Name = 'Organizations'
    field2.Type = 'Mapping'
    field2.DisplayOrder = 2
    field1.Kind = 'Mapping'
    field2.RelatedEntityType = 'Mapping'
    field2.MappingEntity = 'Organization'

    md.Fields.push(<any>field1, <any>field2)

    // Act
    component.setupForm(md, {id: <any>null, entity: 'User'}, {Id: 123})

    // Assert
    expect(component.mappingComponents.some((x: any) => x.type === 'Mapper' && x.label === 'Organizations')).toBeTruthy()

  })

  it ('should only display permitted foreign entities of user', () => {
    // Arrange
    const md = new EntityMetadata()
    md.Name = 'User'
    md.Kind = 'EntityType'
    const field1 = new EntityField()
    field1.Name = 'UserRoles'
    field1.Type = 'self.UserRole'
    field1.DisplayOrder = 1
    field1.Kind = 'Foreign'
    field1.RelatedEntityType = 'Foreign'
    const field2 = new EntityField()
    field2.Name = 'Organizations'
    field2.Type = 'self.Organization'
    field2.DisplayOrder = 2
    field1.Kind = 'Foreign'
    field2.RelatedEntityType = 'Foreign'

    md.Fields.push(<any>field1, <any>field2)
    const orgEntitMetadata = new EntityMetadata()
    spyOn(entityMetadataService, 'getEntity').and.returnValue(orgEntitMetadata)

    // Act
    component.setupForm(md, {id: <any>null, entity: 'User'}, undefined)

    // Assert
    expect(component.foreignEntities.length).toEqual(1)
    expect(component.foreignEntities[0].Name).toEqual('Organizations')

  })

})

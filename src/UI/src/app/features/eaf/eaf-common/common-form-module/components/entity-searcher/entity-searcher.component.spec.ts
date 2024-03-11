import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing'
import { ReactiveFormsModule, FormGroup, FormControl } from '@angular/forms'
import { FlexModule } from '@angular/flex-layout'
import { MatDialog } from '@angular/material/dialog'
import { RouterTestingModule } from '@angular/router/testing'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'

import { of, Observable } from 'rxjs'
import { EntitySearcherComponent } from './entity-searcher.component'
import { EntityService } from 'src/app/core/services/entity.service'
import { EafModule } from 'src/app/features/eaf/eaf.module'
import { ErrorReporterDialogComponent } from 'src/app/core/dialogs/error-reporter-dialog/error-reporter-dialog.component'

import '../../../../../../core/infrastructure/linq'
import { HttpClientTestingModule } from '@angular/common/http/testing'
import { Router } from '@angular/router'
import { MaterialModule } from 'src/app/core/material/material.module'
import { PluralizePipe } from 'src/app/core/pipes/pluralize.pipe'
import { CustomPluralizationMap } from 'src/app/core/models/concretes/custom-pluralization-map'
import { SplitPascalCasePipe } from 'src/app/core/pipes/split-pascal-case.pipe'

export class MdDialogMock {
  open() {
    return {
      afterClosed: () => of({Id: 1066, Name: 'Hastings', Version: '1.0', ProductType: 'Suite'})
    }
  }
}

/**
 * Represents a Mocked version of Application Insights
 */
export class AppInsightsServiceMock {
  trackError() {

  }
}

describe('EntitySearcherComponent', () => {
  let component: EntitySearcherComponent
  let fixture: ComponentFixture<EntitySearcherComponent>
  let entityService: EntityService
  let errorReporter: ErrorReporterDialogComponent

  const mockRouter = {
    navigate: jasmine.createSpy('navigate')
  }

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        FlexModule,
        HttpClientTestingModule,
        RouterTestingModule,
        BrowserAnimationsModule,
        EafModule,
        MaterialModule
      ],
      declarations: [  ],
      providers: [
        EntityService,
        ErrorReporterDialogComponent,
        { provide: MatDialog, useClass: MdDialogMock },
        { provide: Router, useValue: mockRouter },
        PluralizePipe,
        CustomPluralizationMap,
        SplitPascalCasePipe
      ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(EntitySearcherComponent)
    entityService =  TestBed.inject(EntityService)
    errorReporter = TestBed.inject(ErrorReporterDialogComponent)
    component = fixture.componentInstance
    component.group = new FormGroup({Id: new FormControl('Id')})
    component.field = {
      label: 'Product',
      name: 'Id',
      inputType: 'input',
      type: 'number',
      value: 0,
      entity: 'Product'
    }

    spyOn(entityService, 'getFilteredEntityList').and.returnValue(of(<any>'Response'))
    fixture.detectChanges()
  })

  it('should create', () => {
    spyOn(component, 'getResponseObject').and.returnValue()

    expect(component).toBeTruthy()
  })

  it ('should raise an exception for an empty response', () => {
    spyOn(localStorage, 'getItem').and.returnValue('[{"key":"Product"}]')

    component.getResponseObject('')
  })

  it ('should raise an exception if the name was not found', () => {
    spyOn(localStorage, 'getItem').and.returnValue('[{"key":"Product"}]')

    component.getResponseObject({Entities: [{Id: 1, Object: {Id: 1, Name: ''}}]})
  })

  it ('should get an entity', () => {
    component.getEntity()
    expect(component.entityValue).toEqual('Hastings')
    expect(component.group.controls['Id'].value).toEqual(1066)
    expect(component.group.controls['Id'].errors).toBeNull()
  })

  it('should goto related entity', () => {
    // Arrange
    component.field = {
      value: 123,
      searchEntity: 'SomeEntity',
      type: ''
    }

    // Act
    component.goto()

    // Assert
    expect(component.router.navigate).toHaveBeenCalledWith(['./admin/data-administration/SomeEntity/123/'])
  })

})

describe('EntitySearcherComponent getResponseObject', () => {
  let component: EntitySearcherComponent
  let fixture: ComponentFixture<EntitySearcherComponent>

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        FlexModule,
        HttpClientTestingModule,
        RouterTestingModule,
        BrowserAnimationsModule,
        EafModule,
        MaterialModule
      ],
      declarations: [  ],
      providers: [
        EntityService,
        ErrorReporterDialogComponent,
        { provide: MatDialog, useClass: MdDialogMock },
        PluralizePipe,
        CustomPluralizationMap,
        SplitPascalCasePipe
      ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(EntitySearcherComponent)
    component = fixture.componentInstance

    const metaDataObj = [{
      'key': 'Feature',
      'value': {
      }
    },                   {
      'key': 'User',
      'value': {
        '@UI.DisplayName': {
          '$PropertyPath': 'Username'
        }
      }
    }
  ]
    spyOn(localStorage, 'getItem').and.returnValue(JSON.stringify(metaDataObj))
  })

  it('should choose UI.DisplayName.$PropertyPath before Name', () => {
    // Arrange
    component.field = {
      label: 'User',
      name: 'Id',
      inputType: 'input',
      type: 'number',
      value: 0,
      entity: 'User',
      searchEntity: 'User'
    }

    // Act
    component.getResponseObject({Id: 1, Entities: [{Id: 1, Object: {Id: 1, Username: 'MyUserName'}}]})


    // Assert
    expect(component.entityValue).toEqual('MyUserName')
  })

  it('should choose Name if @UI.DisplayName.$PropertyPath does not exist on the metadata object', () => {
    // Arrange
    component.field = {
      label: 'Feature',
      name: 'Id',
      inputType: 'input',
      type: 'number',
      value: 1,
      entity: 'Feature',
      searchEntity: 'Feature'
    }

    // Act
    component.getResponseObject({Entities: [{Id: 1, Object: {Id: 1, Name: 'MyFeature'}}]})


    // Assert
    expect(component.entityValue).toEqual('MyFeature')
    expect(component.field.value).toBe(1)
  })

  it('should get the default value', () => {
    // Arrange
    component.field = {
      label: 'Feature',
      name: 'Id',
      inputType: 'input',
      type: 'number',
      value: 0,
      entity: 'Feature',
      searchEntity: 'Feature',
      searchEntityDefault: {Name: 'All', Value: 0}
    }

    // Act
    component.getDefaultData()

    // Assert
    expect(component.entityValue).toEqual('All')
  })
})

describe('EntitySearcherComponent Update', () => {
  let component: EntitySearcherComponent
  let fixture: ComponentFixture<EntitySearcherComponent>
  let entityService: EntityService

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [
        ReactiveFormsModule,
        FlexModule,
        HttpClientTestingModule,
        RouterTestingModule,
        BrowserAnimationsModule,
        EafModule
      ],
      declarations: [  ],
      providers: [
        EntityService,
        ErrorReporterDialogComponent,
        PluralizePipe,
        CustomPluralizationMap,
        SplitPascalCasePipe,
        { provide: MatDialog, useClass: MdDialogMock }
      ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(EntitySearcherComponent)
    entityService =  TestBed.inject(EntityService)
    component = fixture.componentInstance
    component.group = new FormGroup({Id: new FormControl('Id')})
    component.field = {
      label: 'Product',
      name: 'Id',
      inputType: 'input',
      type: 'number',
      value: 1,
      entity: 'Product'
    }

    spyOn(entityService, 'getFilteredEntityList').and.returnValue(of(<any>''))
    spyOn(component, 'getResponseObject')
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
    expect(entityService.getFilteredEntityList).toHaveBeenCalledTimes(1)
    expect(component.getResponseObject).toHaveBeenCalledTimes(1)
  })

})

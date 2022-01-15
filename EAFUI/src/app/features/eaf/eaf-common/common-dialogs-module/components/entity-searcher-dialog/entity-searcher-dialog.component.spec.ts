import { async, ComponentFixture, TestBed } from '@angular/core/testing'
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA, MatFormFieldModule,
  MatProgressBarModule, MatTableModule, MatPaginatorModule, MatSnackBarModule, MatInputModule } from '@angular/material'
import { ReactiveFormsModule } from '@angular/forms'
import { FlexLayoutModule } from '@angular/flex-layout'
import { HttpClientTestingModule } from '@angular/common/http/testing'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { RouterTestingModule } from '@angular/router/testing'
import { of, Observable } from 'rxjs'

import { EntitySearcherDialogComponent } from './entity-searcher-dialog.component'
import { EntityService } from 'src/app/core/services/entity.service'
import { EntityPropertyControlService } from '../../../common-form-module/services/entity-property-control.service'
import { ErrorReporterDialogComponent } from 'src/app/core/dialogs/error-reporter-dialog/error-reporter-dialog.component'
import { environment } from 'src/environments/environment'
import { ApplicationMonitoringService } from 'src/app/core/services/application-monitoring.service'
import { SpaceTitlePipe } from 'src/app/core/pipes/spacetitle.pipe'
import { PluralizePipe } from 'src/app/core/pipes/pluralize.pipe'
import { defaultIfEmpty } from 'rxjs/operators'



class MdDialogMock {
  close(returnValue: any) {
    return {
      afterClosed: () => of(returnValue)
    }
  }
}

describe('EntitySearcherDialogComponent', () => {
  let component: EntitySearcherDialogComponent
  let fixture: ComponentFixture<EntitySearcherDialogComponent>
  let entityService: EntityService

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        MatDialogModule,
        MatSnackBarModule,
        MatFormFieldModule,
        MatInputModule,
        ReactiveFormsModule,
        FlexLayoutModule,
        MatProgressBarModule,
        MatTableModule,
        MatPaginatorModule,
        RouterTestingModule,
        BrowserAnimationsModule,
        HttpClientTestingModule
      ],
      providers: [
       { provide: MatDialogRef, useClass: MdDialogMock },
       { provide: MAT_DIALOG_DATA, useValue:  {entityName: 'Feature'} },
       EntityService,
       HttpClientTestingModule,
       ApplicationMonitoringService,
       EntityPropertyControlService,
       ErrorReporterDialogComponent
      ],
      declarations: [
        EntitySearcherDialogComponent,
        SpaceTitlePipe,
        PluralizePipe,
      ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
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
      'key': 'EntitlementType',
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
                'Name': {'$Collection': true, '$Type': 'Edm.String', '@UI.Searchable': true},
                'Description': {'$Collection': true, '$Type': 'Edm.String', '@UI.Searchable': true},
                'Version': {'$Collection': true, '$Type': 'Edm.String', '@UI.Searchable': true},
                'CreateDate': {'$Type': 'Edm.Date'},
                'LastUpdated': {'$Type': 'Edm.Date'},
                'CreatedBy': {'$Type': 'Edm.Int64'},
                'LastUpdatedBy': {'$Type': 'Edm.Int64'},
                'Id': {'$Type': 'Edm.Int32', '@UI.Searchable': true}}
    }]))

    entityService = TestBed.get(EntityService)
    spyOn(entityService, 'getFilteredEntityList').and.returnValue(of(<any>{TotalCount: 23, Entities : [{Id: 23, Name: 'Test 1'}]}))
    fixture = TestBed.createComponent(EntitySearcherDialogComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })

  it ('should return a row on click', () => {
    // Setup the mock dialog
    spyOn(component.dialogRef, 'close')
    component.rowClick({Id: 1, Object: {Id: 23, Name: 'Feature 1'}})
    expect(component.dialogRef.close).toHaveBeenCalledWith({Id: 23, Name: 'Feature 1'})
  })

  it ('Should return cancel on cancel button', () => {
    spyOn(component.dialogRef, 'close')
    component.cancel()
    expect(component.dialogRef.close).toHaveBeenCalledWith('Cancel')
  })

  it ('Should filter the table', () => {
    component.filterData()
    expect(component.datasource.data.length).toEqual(1)
  })

  it ('Should filter the table with defaultEntity', () => {
    // Arange
    component.defaultEntity = {Name: 'All', Value: 1}

    spyOn(component, 'createDefaultEntity').and.callThrough()

    // Act
    component.filterData()

    // Assert
    expect(component.createDefaultEntity).toHaveBeenCalled()
    expect(component.datasource.data.length).toEqual(2)
  })

  it ('Should change a page', () => {
    spyOn(component, 'filterData').and.returnValue()
    component.pageChange({pageIndex: 2})
    expect(component.filterData).toHaveBeenCalledTimes(1)
    expect(component.pageIndex).toEqual(2)
  })

  it ('Should get a property value', () => {
    const prop = component.getPropValue({Object: {Id: 23, Name: 'Feature1'}}, 'Name')
    expect(prop).toEqual('Feature1')
  })

  it ('Should update the grid on filterchange', () => {
    spyOn(component, 'filterData').and.returnValue()
    component.form.get('Name').setValue('Test')
    component.onFilterChanged()

    expect(component.filterData).toHaveBeenCalledTimes(1)
    expect(component.filterText).toEqual('contains(Name, \'Test\')')

  })

  it ('Should update multiple fields on filterchange', () => {
    spyOn(component, 'filterData').and.returnValue()
    component.form.get('Name').setValue('Test')
    component.form.get('Id').setValue('23')
    component.onFilterChanged()

    expect(component.filterData).toHaveBeenCalledTimes(1)
    expect(component.filterText).toEqual('contains(Name, \'Test\') and Id eq \'23\'')

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

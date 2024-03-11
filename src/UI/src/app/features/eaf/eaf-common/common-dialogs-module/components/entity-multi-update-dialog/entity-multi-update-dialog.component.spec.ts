import { HttpClient } from '@angular/common/http'
import { HttpClientTestingModule } from '@angular/common/http/testing'
import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing'
import { FlexModule } from '@angular/flex-layout'
import { ReactiveFormsModule } from '@angular/forms'
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { RouterTestingModule } from '@angular/router/testing'
import { of, throwError } from 'rxjs'
import { ErrorReporterDialogComponent } from 'src/app/core/dialogs/error-reporter-dialog/error-reporter-dialog.component'
import { MaterialModule } from 'src/app/core/material/material.module'
import { CustomPluralizationMap } from 'src/app/core/models/concretes/custom-pluralization-map'
import { WellKnownProperties } from 'src/app/core/models/concretes/well-known-properties'
import { ArraySortOrderPipe } from 'src/app/core/pipes/array-sort-order.pipe'
import { PluralizePipe } from 'src/app/core/pipes/pluralize.pipe'
import { SpaceTitlePipe } from 'src/app/core/pipes/spacetitle.pipe'
import { SplitPascalCasePipe } from 'src/app/core/pipes/split-pascal-case.pipe'
import { EntityMetadataService } from 'src/app/core/services/entity-metadata.service'
import { EntityService } from 'src/app/core/services/entity.service'
import { GlobalSnackBarService } from 'src/app/core/services/global-snack-bar.service'
import { AppLocalStorageService } from 'src/app/core/services/local-storage.service'
import { FakeAppLocalStorageService } from 'src/app/core/services/mocks/mocks'
import { CommonFormModule } from '../../../common-form-module/common-form.module'
import { EntityField } from '../../../common-form-module/models/concretes/entity-field'
import { EntityConfigBuilder } from '../../../common-form-module/services/entity-config-builder'
import { EntityFieldValidatorProvider } from '../../../common-form-module/services/entity-field-validator-provider'
import { EntityPropertyControlService } from '../../../common-form-module/services/entity-property-control.service'
import { EntityPropertyTypeControlTypeMap } from '../../../common-form-module/services/entity-property-type-control-type.map'
import { EntityPropertyTypeInputTypeMap } from '../../../common-form-module/services/entity-property-type-input-type.map'
import { EnumOptionMapper } from '../../../common-form-module/services/enum-option-mapper'
import { StringTypeControlTypeMap } from '../../../common-form-module/services/string-type-control-type.map'

import { EntityMultiUpdateDialogComponent } from './entity-multi-update-dialog.component'

export class MockConfirm {
  afterClosed() {
    return of('confirmed')
  }
  apply() {
  }
}

class MdDialogMock {
  close(returnValue: any) {
    return {
      afterClosed: () => of(returnValue)
    }
  }
}

const featureEntityMetadata = {
  'key': 'Feature',
  'value': {
    '$Key': [
      'Id'
    ],
    '$Kind': 'EntityType',
    'CreateDate': {
      '$Type': 'Edm.DateTimeOffset',
      '@UI.DisplayOrder': 5,
      '@UI.Searchable': false,
      '@UI.ReadOnly': true
    },
    'CreatedBy': {
      '$Type': 'Edm.Int64',
      '@UI.DisplayOrder': 6,
      '@UI.Searchable': false,
      '@UI.ReadOnly': true
    },
    'Description': {
      '$Nullable': true,
      '$Type': 'Edm.String',
      '@UI.DisplayOrder': 3,
      '@UI.Searchable': true
    },
    'Id': {
      '$Type': 'Edm.Int32',
      '@UI.DisplayOrder': 1,
      '@UI.Searchable': true
    },
    'LastUpdated': {
      '$Nullable': true,
      '$Type': 'Edm.DateTimeOffset',
      '@UI.DisplayOrder': 7,
      '@UI.Searchable': false,
      '@UI.ReadOnly': true
    },
    'LastUpdatedBy': {
      '$Nullable': true,
      '$Type': 'Edm.Int64',
      '@UI.DisplayOrder': 8,
      '@UI.Searchable': false,
      '@UI.ReadOnly': true
    },
    'Name': {
      '$Type': 'Edm.String',
      '@UI.DisplayOrder': 2,
      '@UI.Searchable': true
    },
    'Version': {
      '$Nullable': true,
      '$Type': 'Edm.String',
      '@UI.DisplayOrder': 4,
      '@UI.Searchable': true
    },
    '@EAF.EntityGroup': 'Product Packaging Management',
    'ProductFeatureMaps': {
      '$Collection': true,
      '$Kind': 'NavigationProperty',
      '$Nullable': true,
      '$Type': 'self.ProductFeatureMap',
      '@EAF.RelatedEntity.Type': 'Foreign'
    },
    'ProductReleaseFeatureMaps': {
      '$Collection': true,
      '$Kind': 'NavigationProperty',
      '$Nullable': true,
      '$Type': 'self.ProductReleaseFeatureMap',
      '@EAF.RelatedEntity.Type': 'Foreign'
    },
    'Products': {
      '$Collection': true,
      '$Kind': 'NavigationProperty',
      '$Nullable': true,
      '$Type': 'self.Product',
      '@EAF.RelatedEntity.Type': 'Mapping',
      '@EAF.RelatedEntity.MappingEntityType': 'self.ProductFeatureMap'
    },
    'ProductReleases': {
      '$Collection': true,
      '$Kind': 'NavigationProperty',
      '$Nullable': true,
      '$Type': 'self.ProductRelease',
      '@EAF.RelatedEntity.Type': 'Mapping',
      '@EAF.RelatedEntity.MappingEntityType': 'self.ProductReleaseFeatureMap'
    },
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

describe('EntityMultiUpdateDialogComponent', () => {
  let component: EntityMultiUpdateDialogComponent
  let fixture: ComponentFixture<EntityMultiUpdateDialogComponent>
  let entityService: EntityService
  let errorReporter: ErrorReporterDialogComponent
  let entityMetadataService: EntityMetadataService
  let wellKnownProperties: WellKnownProperties

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [
        FlexModule,
        ReactiveFormsModule,
        BrowserAnimationsModule,
        HttpClientTestingModule,
        RouterTestingModule,
        ReactiveFormsModule,
        MaterialModule,
        CommonFormModule
      ],
      providers: [
        {provide: MatDialogRef,  useClass: MdDialogMock},
        { provide: MAT_DIALOG_DATA, useValue: [] },
        { provide: AppLocalStorageService, useClass: FakeAppLocalStorageService },
        EntityService,
        HttpClient,
        ErrorReporterDialogComponent,
        EntityMetadataService,
        ArraySortOrderPipe,
        PluralizePipe,
        CustomPluralizationMap,
        SplitPascalCasePipe,
        GlobalSnackBarService,
        EntityPropertyControlService,
        EntityConfigBuilder,
        EntityPropertyTypeControlTypeMap,
        EntityPropertyTypeInputTypeMap,
        EntityFieldValidatorProvider,
        EnumOptionMapper,
        StringTypeControlTypeMap,
        SpaceTitlePipe,
        WellKnownProperties
      ],
      declarations: [ EntityMultiUpdateDialogComponent ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(EntityMultiUpdateDialogComponent)
    component = fixture.componentInstance
    entityService = TestBed.inject(EntityService)
    errorReporter = TestBed.inject(ErrorReporterDialogComponent)
    entityMetadataService = TestBed.inject(EntityMetadataService)
    wellKnownProperties = TestBed.inject(WellKnownProperties)
    spyOn(entityMetadataService, 'getEntityMetaData').and.returnValue(featureEntityMetadata)
    component.entityName = 'Feature'
    fixture.detectChanges()
  })

  it('EntityMultiUpdateDialogComponent should create', () => {
    expect(component).toBeTruthy()
  })

  it('should not display auditable properties', () => {
    const auditableProperties = wellKnownProperties.auditableProperties

    auditableProperties.each(function(p) {
      expect(component.entityConfig.some(x => x.name === p)).toBeFalsy()
    })
  })

  it('should not display key field', () => {
      expect(component.entityConfig.some(x => x.name === 'Id')).toBeFalsy()
  })

  it('should not display readonly field', () => {
    expect(component.entityConfig.some(x => x.readOnly)).toBeFalsy()
  })

  it('should not display navigation field', () => {
    expect(component.entityConfig.some(x => x.name === 'ProductFeatureMaps')).toBeFalsy()
    expect(component.entityConfig.some(x => x.name === 'ProductReleaseFeatureMaps')).toBeFalsy()
    expect(component.entityConfig.some(x => x.name === 'Products')).toBeFalsy()
    expect(component.entityConfig.some(x => x.name === 'ProductReleases')).toBeFalsy()
    expect(component.entityConfig.some(x => x.name === 'Addenda')).toBeFalsy()
    expect(component.entityConfig.some(x => x.name === 'AlternateIds')).toBeFalsy()
  })

  it('should confirm update dialog on submit', () => {
    spyOn(component, 'getPatchEntityCollection').and.returnValue(<any>null)
    const dialog = TestBed.inject(MatDialog)
    spyOn(dialog, 'open').and.returnValue(
      {
        afterClosed: () => of('confirmed')
      } as MatDialogRef<typeof component>
    )

    component.submit(undefined)

    expect(component.matDialog.open).toHaveBeenCalled()
    expect(component.getPatchEntityCollection).toHaveBeenCalled()
  })

  it('should return updated fields to patch', () => {

    const formFields = {
      'Name': 'Test feature2',
      'Description': 'Test desc',
      'Version': '1.8'
    }

    component.entityConfig = [{
      name: 'Name',
      type: 'Edm.String',
      value: 'Test feature'
    },                        {
      name: 'Description',
      type: 'Edm.String',
      value: 'Test desc'
    },                        {
      name: 'Version',
      type: 'Edm.String',
      value: '1.7'
    }  ]

    component.selectedRecordIds = [1, 2]
    const requestObj: any = component.getPatchEntityCollection(formFields)

    expect(requestObj.ChangedProperties.length).toEqual(2)
    expect(requestObj.PatchedEntities[0].Entity['Id']).toEqual(component.selectedRecordIds[0])
    expect(requestObj.PatchedEntities[0].Entity['Name']).toEqual(formFields['Name'])
    expect(requestObj.PatchedEntities[1].Entity['Id']).toEqual(component.selectedRecordIds[1])
    expect(requestObj.PatchedEntities[1].Entity['Name']).toEqual(formFields['Name'])
})

  it('should display success message on successful update', () => {
    spyOn(entityService, 'patchManyEntities').and.returnValue(of(<any>null))
    spyOn(component.dialogRef, 'close').and.returnValue()
    component.updateRecords(null)
    expect(component.dialogRef.close).toHaveBeenCalledWith('Update successful')
})

  it('should handle an error on update', () => {
    const errorMessage = {status: 500, Acknowledgeable: false}
    spyOn(entityService, 'patchManyEntities').and.returnValue(throwError(errorMessage))
    spyOn(errorReporter, 'displayMessage').and.returnValue()
    component.updateRecords(null)
    expect(errorReporter.displayMessage).toHaveBeenCalledWith(<any>errorMessage)
  })

})

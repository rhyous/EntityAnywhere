import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing'
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar'
import { RouterTestingModule } from '@angular/router/testing'
import { of } from 'rxjs'

import { ExtensionEntityDialogComponent } from './extension-entity-dialog.component'
import { ExtensionEntityDialogComponentData } from './extension-entity-dialog-component-data'
import { EntityService } from 'src/app/core/services/entity.service'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { EntityFieldDirective } from 'src/app/features/eaf/eaf-custom/directives/entity-field.directive'
import { HttpClientTestingModule } from '@angular/common/http/testing'
import { FlexModule } from '@angular/flex-layout'
import { ReactiveFormsModule } from '@angular/forms'
import { MaterialModule } from 'src/app/core/material/material.module'
import { ErrorReporterDialogComponent } from 'src/app/core/dialogs/error-reporter-dialog/error-reporter-dialog.component'
import { EntityButtonComponent } from '../../../common-form-module/components/entity-button/entity-button.component'
import { EntityFormComponent } from '../../../common-form-module/components/entity-form/entity-form.component'
import { EntityInputComponent } from '../../../common-form-module/components/entity-input/entity-input.component'
import { EntitySelectComponent } from '../../../common-form-module/components/entity-select/entity-select.component'
// tslint:disable-next-line: max-line-length
import { ExtensionEntityPropertyAutoCompleteComponent } from 'src/app/features/eaf/eaf-common/common-dashboard-module/components/extension-entity-property-auto-complete/extension-entity-property-auto-complete.component'
import { CommonModule } from '@angular/common'
import { BrowserDynamicTestingModule } from '@angular/platform-browser-dynamic/testing'
import { SpaceTitlePipe } from 'src/app/core/pipes/spacetitle.pipe'
import { SplitPascalCasePipe } from 'src/app/core/pipes/split-pascal-case.pipe'
import { CustomPluralizationMap } from 'src/app/core/models/concretes/custom-pluralization-map'
import { PluralizePipe } from 'src/app/core/pipes/pluralize.pipe'

class DialogMock {
  close(returnValue: any) {
    return {
      afterClosed: () => of(returnValue)
    }
  }
}

class SnackBarMock {
  open() {
    return this
  }
}

const distinctPropertyList: any[] =  ['Test1', 'Test2', 'Test3']

describe('ExtensionEntityDialogComponent', () => {
  let component: ExtensionEntityDialogComponent
  let fixture: ComponentFixture<ExtensionEntityDialogComponent>
  let entityService: EntityService

  const matData: ExtensionEntityDialogComponentData = {
    extensionEntityData: {
      Type: 'self.Addendum',
      Name: 'Addenda'
    },
    parentEntityId: 2703,
    parentEntityName: 'Feature'
  }

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [
        CommonModule,
        RouterTestingModule,
        MaterialModule,
        ReactiveFormsModule,
        FlexModule,
        HttpClientTestingModule,
        BrowserAnimationsModule
      ],
      providers: [
        { provide: MatDialogRef, useClass: DialogMock },
        { provide: MAT_DIALOG_DATA, useValue: matData },
        { provide: EntityService, useClass: EntityService },
        { provide: MatSnackBar, useClass: SnackBarMock},
        PluralizePipe,
        CustomPluralizationMap,
        SplitPascalCasePipe
      ],
      declarations: [
        ExtensionEntityDialogComponent,
        EntityFormComponent,
        EntityFieldDirective,
        ErrorReporterDialogComponent,
        EntitySelectComponent,
        EntityInputComponent,
        EntityButtonComponent,
        ExtensionEntityPropertyAutoCompleteComponent,
        SpaceTitlePipe
      ]
    })
    .overrideModule(BrowserDynamicTestingModule, {set:
      {
        entryComponents:
        [
          EntitySelectComponent,
          EntityInputComponent,
          EntityButtonComponent,
          ExtensionEntityPropertyAutoCompleteComponent,
        ]}})
    .compileComponents()
  }))

  beforeEach(() => {
    entityService = TestBed.get(EntityService)
    spyOn(entityService, 'getDistinctExtensionPropertList').and.returnValue(of(distinctPropertyList))
    fixture = TestBed.createComponent(ExtensionEntityDialogComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    // Arrange

    // Act

    // Assert
    expect(component).toBeTruthy()
  })

  /*
  it('should set the progressBar to true', () => {
    component.submit({Property: 'MyProperty', Value: 'MyValue'})

    expect(component.showProgressBar).toBeTruthy()
  })*/

  it('Should pass Addendum to entity service', () => {
    // Arrange
    entityService = fixture.debugElement.injector.get(EntityService)
    spyOn(component.dialogRef, 'close').and.returnValue()
    spyOn(entityService, 'addEntity').and.returnValue(of())

    // Act
    component.submit({Property: 'MyProperty', Value: 'MyValue'})

    // Assert
    expect(entityService.addEntity).toHaveBeenCalledWith('Addendum',
                                      [ Object({ Entity: 'Feature', EntityId: 2703, Property: 'MyProperty', Value: 'MyValue' }) ])
  })
})

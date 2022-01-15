import { async, ComponentFixture, TestBed } from '@angular/core/testing'
import { MatDialogRef, MatSnackBar, MAT_DIALOG_DATA, MatSnackBarConfig } from '@angular/material'
import { RouterTestingModule } from '@angular/router/testing'
import { Observable, of } from 'rxjs'

import { ExtensionEntityDialogComponent } from './extension-entity-dialog.component'
import { ExtensionEntityDialogComponentData } from './extension-entity-dialog-component-data'
import { EntityService } from 'src/app/core/services/entity.service'
import { EafModule } from 'src/app/features/eaf/eaf.module'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'

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
    parentEntityName: 'Product'
  }

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [EafModule, RouterTestingModule, BrowserAnimationsModule],
    })
    .overrideComponent(ExtensionEntityDialogComponent, {
      set: {
        providers: [
          { provide: MatDialogRef, useClass: DialogMock },
          { provide: MAT_DIALOG_DATA, useValue: matData },
          { provide: EntityService, useClass: EntityService },
          { provide: MatSnackBar, useClass: SnackBarMock}
        ]
      }
    })
    .compileComponents()
  }))

  beforeEach(() => {
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
                                      [ Object({ Entity: 'Product', EntityId: 2703, Property: 'MyProperty', Value: 'MyValue' }) ])
  })
})

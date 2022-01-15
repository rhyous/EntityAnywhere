import { async, ComponentFixture, TestBed } from '@angular/core/testing'
import { RouterTestingModule } from '@angular/router/testing'
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material'
import { Observable, of } from 'rxjs'

import { EntityFormDialogComponent } from './entity-form-dialog.component'
import { EafModule } from '../../../../eaf.module'

class DialogMock {
  close(returnValue: any) {
    return {
      afterClosed: () => of(returnValue)
    }
  }
}
describe('EntityFormDialogComponent', () => {
  let component: EntityFormDialogComponent
  let fixture: ComponentFixture<EntityFormDialogComponent>

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [EafModule, RouterTestingModule],
    })
    .overrideComponent(EntityFormDialogComponent, {
      set: {
        providers: [
          { provide: MatDialogRef, useClass: DialogMock },
          { provide: MAT_DIALOG_DATA, useValue: { title: 'Title', entityConfig: {} } }
      ]
      }
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(EntityFormDialogComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })


  it('should create', () => {
    expect(component).toBeTruthy()
  })

  it('should call close on submit and pass on the data', () => {
    const dialogRef = fixture.debugElement.injector.get(MatDialogRef)
    spyOn(dialogRef, 'close').and.returnValue()

    const data = {
      Id: 1,
      Name: 'Matt Speakman',
      Age: ' 35'
    }
    component.submit(data)

    expect(dialogRef.close).toHaveBeenCalledWith(data)
  })
})

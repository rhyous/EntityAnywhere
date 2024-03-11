import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing'
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'
import { MessageDialogComponent } from './message-dialog.component'
import { MaterialModule } from '../../material/material.module'

describe('MessageDialogComponent', () => {
  let component: MessageDialogComponent
  let fixture: ComponentFixture<MessageDialogComponent>

  const dialogMock = {
    close: () => { }
  }

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [ MaterialModule ],
      providers: [
        { provide: MatDialogRef, useValue: dialogMock },
        { provide: MAT_DIALOG_DATA, useValue: [] } ],
      declarations: [ MessageDialogComponent ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(MessageDialogComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })

  it('should return a message based on the index', () => {
    expect(component.getTabLabel(0)).toEqual('Message 1')
  })
})

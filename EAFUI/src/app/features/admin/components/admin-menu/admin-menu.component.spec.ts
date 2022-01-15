import { async, ComponentFixture, TestBed } from '@angular/core/testing'

import { AdminMenuComponent } from './admin-menu.component'
import { MaterialModule } from 'src/app/core/material/material.module'

describe('AdminMenuComponent', () => {
  let component: AdminMenuComponent
  let fixture: ComponentFixture<AdminMenuComponent>

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        MaterialModule
      ],
      declarations: [ AdminMenuComponent ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminMenuComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})

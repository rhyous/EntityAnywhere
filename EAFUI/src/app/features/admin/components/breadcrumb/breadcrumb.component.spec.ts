import { async, ComponentFixture, TestBed } from '@angular/core/testing'

import { BreadcrumbComponent } from './breadcrumb.component'
import { RemoveHyphensPipe } from 'src/app/core/pipes/remove-hyphens.pipe'
import { SplitPascalCasePipe } from 'src/app/core/pipes/split-pascal-case.pipe'
import { MaterialModule } from 'src/app/core/material/material.module'
import { RouterTestingModule } from '@angular/router/testing'
import { PluralizePipe } from 'src/app/core/pipes/pluralize.pipe'

describe('BreadcrumbComponent', () => {
  let component: BreadcrumbComponent
  let fixture: ComponentFixture<BreadcrumbComponent>

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        MaterialModule
      ],
      declarations: [
        BreadcrumbComponent,
        RemoveHyphensPipe,
        SplitPascalCasePipe
      ],
      providers: [
        PluralizePipe,
        RemoveHyphensPipe,
        SplitPascalCasePipe
      ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(BreadcrumbComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})

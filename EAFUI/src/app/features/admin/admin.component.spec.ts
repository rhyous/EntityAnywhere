import { async, ComponentFixture, TestBed } from '@angular/core/testing'

import { AdminComponent } from './admin.component'
import { RouterTestingModule } from '@angular/router/testing'
import { MaterialModule } from 'src/app/core/material/material.module'
import { SidebarMenuComponent } from 'src/app/core/sidebar-menu/sidebar-menu.component'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { Router, ActivatedRoute } from '@angular/router'
import { of } from 'rxjs'
import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component'
import { SplitPascalCasePipe } from 'src/app/core/pipes/split-pascal-case.pipe'
import { RemoveHyphensPipe } from 'src/app/core/pipes/remove-hyphens.pipe'
import { LoginService } from 'src/app/core/login/login.service'
import { AppLocalStorageService } from 'src/app/core/services/local-storage.service'
import { FakeAppLocalStorageService } from 'src/app/core/services/mocks/mocks'

class FakeActivatedRoute {
  get params() { return of('') }
}

class FakeRouter {
  parseUrl() {
    return {
      root: {
        children: {
          primary: {
            segments: []
          }
        }
      }
    }
  }
  url = 'http://localhost:4200/admin'
  navigate() {}
}

export class FakeLoginService {
  logout() {}
}

describe('AdminComponent', () => {
  let component: AdminComponent
  let fixture: ComponentFixture<AdminComponent>

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        AdminComponent,
        SidebarMenuComponent,
        BreadcrumbComponent,
        SplitPascalCasePipe,
        RemoveHyphensPipe,
      ],
      imports: [
        RouterTestingModule,
        MaterialModule,
        BrowserAnimationsModule
      ],
      providers: [
        { provide: Router, useClass: FakeRouter },
        { provide: ActivatedRoute, useClass: FakeActivatedRoute },
        { provide: LoginService, useClass: FakeLoginService },
        { provide: FeatureFlagsService, useValue: { init: () => {} } },
        { provide: AppLocalStorageService, useClass: FakeAppLocalStorageService },
        SplitPascalCasePipe,
        RemoveHyphensPipe
      ],
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })
})

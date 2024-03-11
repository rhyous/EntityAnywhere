import { HttpClientTestingModule } from '@angular/common/http/testing'
import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing'
import { FormBuilder, ReactiveFormsModule } from '@angular/forms'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { RouterTestingModule } from '@angular/router/testing'
import { of } from 'rxjs'
import { AutoCompleteComponent } from 'src/app/core/auto-complete/auto-complete.component'
import { LoginService } from 'src/app/core/login/login.service'
import { MaterialModule } from 'src/app/core/material/material.module'
import { CustomPluralizationMap } from 'src/app/core/models/concretes/custom-pluralization-map'
import { PluralizePipe } from 'src/app/core/pipes/pluralize.pipe'
import { SplitPascalCasePipe } from 'src/app/core/pipes/split-pascal-case.pipe'
import { Fake } from 'src/app/core/services/entity-metadata-fake'
import { EntityMetadataService } from 'src/app/core/services/entity-metadata.service'
import { EntityService } from 'src/app/core/services/entity.service'
import { GlobalSnackBarService } from 'src/app/core/services/global-snack-bar.service'
import { ImpersonationService } from 'src/app/core/services/impersonation.service'
import { AppLocalStorageService } from 'src/app/core/services/local-storage.service'
import { FakeAppLocalStorageService } from 'src/app/core/services/mocks/mocks'

import { ImpersonateCustomerComponent } from './impersonate-customer.component'

export class FakeLoginService {
  parseToken() {}
}

describe('ImpersonateCustomerComponent', () => {
  let component: ImpersonateCustomerComponent
  let fixture: ComponentFixture<ImpersonateCustomerComponent>
  let entityService: EntityService
  let impersonationService: ImpersonationService

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [
        BrowserAnimationsModule,
        ReactiveFormsModule,
        HttpClientTestingModule,
        RouterTestingModule,
        MaterialModule,
      ],
      declarations: [ ImpersonateCustomerComponent, AutoCompleteComponent ],
      providers: [
        { provide: LoginService, useClass: FakeLoginService},
        { provide: EntityMetadataService, useValue: { getEntityMetaData: (entityName: any) =>
           Fake.FakeMeta.firstOrDefault(x => x.key === entityName) }},
        { provide: AppLocalStorageService, useClass: FakeAppLocalStorageService },
        EntityService,
        ImpersonationService,
        FormBuilder,
        GlobalSnackBarService,
        PluralizePipe,
        CustomPluralizationMap,
        SplitPascalCasePipe
      ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    entityService = TestBed.inject(EntityService)
    spyOn(entityService, 'getEntityList').and.returnValue(of({ Entities: [] }))

    impersonationService = TestBed.inject(ImpersonationService)

    fixture = TestBed.createComponent(ImpersonateCustomerComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })

  it('Should call EntityService', () => {
    // Arrange

    // Act
    component.ngOnInit()

    // Assert
    expect(entityService.getEntityList).toHaveBeenCalledWith('UserRole')
  })

  it('Should call ImpersonationService', () => {
    // Arrange
    spyOn(impersonationService, 'getImpersonationToken').and.callThrough()

    component.form.patchValue({
      UserRole: {Id: '2'}
    })

    // Act
    component.impersonateCustomer()

    // Assert
    expect(impersonationService.getImpersonationToken).toHaveBeenCalledWith('2')
  })

})

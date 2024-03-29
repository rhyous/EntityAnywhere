import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing'

import { FormsModule, ReactiveFormsModule, FormControl, Validators, FormGroup } from '@angular/forms'
import { MatDialog } from '@angular/material/dialog'
import { MatFormFieldModule } from '@angular/material/form-field'
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner'

import { ErrorDialogComponent } from '../dialogs/error-dialog/error-dialog.component'
import { LoginComponent } from './login.component'
import { LoginService } from './login.service'
import { AppLocalStorageService } from '../services/local-storage.service'

import { HttpResponse } from '@angular/common/http'
import { ActivatedRoute } from '@angular/router'
import { RouterTestingModule } from '@angular/router/testing'
import { of, throwError } from 'rxjs'
import { HttpClientTestingModule } from '@angular/common/http/testing'
import { MaterialModule } from '../material/material.module'

require('../infrastructure/linq')

export class MatDialogMock {
  open() {
    return {
      afterClosed: () => of('stub')
    }
  }
}

/**
 * Represents a Mocked version of Application Insights
 */
export class AppInsightsServiceMock {
  trackEvent() {

  }

}

export class FakeLoginService {
  login() { return {token} }
  validateOAuthToken() { return {token} }
  parseToken() {}
  logout() {}
}

const token: any = {
  'Id': 0,
  'Text': 'TOKEN',
  'ClaimDomains': [{
      'Claims': [{
          'Name': 'Username',
          'Issuer': 'LOCAL AUTHORITY',
          'Subject': 'User',
          'Value': 'admin',
          'ValueType': null
        },       {
          'Name': 'Id',
          'Issuer': 'LOCAL AUTHORITY',
          'Subject': 'User',
          'Value': '3',
          'ValueType': null
        },       {
          'Name': 'LastAuthenticated',
          'Issuer': 'LOCAL AUTHORITY',
          'Subject': 'User',
          'Value': 'Thu, 02 Jan 2020 11:29:10 GMT',
          'ValueType': null
        }
      ],
      'Issuer': 'LOCAL AUTHORITY',
      'OriginalIssuer': null,
      'Subject': 'User'
    },             {
      'Claims': [{
          'Name': 'Id',
          'Issuer': 'LOCAL AUTHORITY',
          'Subject': 'Organization',
          'Value': '1',
          'ValueType': null
        },       {
          'Name': 'Name',
          'Issuer': 'LOCAL AUTHORITY',
          'Subject': 'Organization',
          'Value': 'Internal',
          'ValueType': null
        },       {
          'Name': 'SapId',
          'Issuer': 'LOCAL AUTHORITY',
          'Subject': 'Organization',
          'Value': '0000036948',
          'ValueType': null
        }
      ],
      'Issuer': 'LOCAL AUTHORITY',
      'OriginalIssuer': null,
      'Subject': 'Organization'
    },             {
      'Claims': [{
          'Name': 'Role',
          'Issuer': 'LOCAL AUTHORITY',
          'Subject': 'UserRole',
          'Value': 'Admin',
          'ValueType': null
        }
      ],
      'Issuer': 'LOCAL AUTHORITY',
      'OriginalIssuer': null,
      'Subject': 'UserRole'
    }
  ],
  'UserId': 3,
  'CreateDate': new Date('0001-01-01T00:00:00+00:00'),
  'CreatedBy': 0,
  'LastUpdated': null,
  'LastUpdatedBy': null
}

describe('LoginComponent', () => {
  let component: LoginComponent
  let fixture: ComponentFixture<LoginComponent>
  let loginService: LoginService
  let dialog: MatDialog
  let mockActivatedRoute
  let localStorageService: AppLocalStorageService


  beforeEach(waitForAsync(() => {
    // mockActivatedRoute = new MockActivatedRoute()
    // mockActivatedRoute.parent.params = Observable.of({token:"testId"})
    // const token = {token: 'myToken'}
    mockActivatedRoute = {queryParams: of(token) , params: of(token), fragment: of(null)}

    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        FormsModule,
        ReactiveFormsModule,
        MatProgressSpinnerModule,
        MatFormFieldModule,
        MaterialModule,
        HttpClientTestingModule
      ],

      declarations: [ LoginComponent, ErrorDialogComponent ],
      providers: [
        { provide: MatDialog, useClass: MatDialogMock},
        { provide: ActivatedRoute, useValue: mockActivatedRoute},
        { provide: LoginService, useClass: FakeLoginService},
        HttpClientTestingModule
      ]
    })

    mockActivatedRoute = TestBed.inject(ActivatedRoute)
    loginService = TestBed.inject(LoginService)
    dialog = TestBed.inject(MatDialog)
    localStorageService = TestBed.inject(AppLocalStorageService)

    fixture = TestBed.createComponent(LoginComponent)
    component = fixture.componentInstance

  }))

  it('should be created', () => {
    expect(component).toBeTruthy()
  })

  it(`should call login service login - Admin`, () => {
    // Arrange
    component.form =  new FormGroup({username: new FormControl('warehouseOne', [Validators.required]),
                                     password: new FormControl('passWO', [Validators.required])})

    // tslint:disable-next-line: max-line-length
    spyOn(localStorage, 'getItem').and.returnValue(JSON.stringify([{'Name': 'Username', 'Issuer': 'LOCAL AUTHORITY', 'Subject': 'User', 'Value': 'admin', 'ValueType': null}, {'Name': 'Id', 'Issuer': 'LOCAL AUTHORITY', 'Subject': 'User', 'Value': '3', 'ValueType': null}, {'Name': 'LastAuthenticated', 'Issuer': 'LOCAL AUTHORITY', 'Subject': 'User', 'Value': 'Thu, 02 Jan 2020 11:29:10 GMT', 'ValueType': null}, {'Name': 'Id', 'Issuer': 'LOCAL AUTHORITY', 'Subject': 'Organization', 'Value': '1', 'ValueType': null}, {'Name': 'Name', 'Issuer': 'LOCAL AUTHORITY', 'Subject': 'Organization', 'Value': 'Internal', 'ValueType': null}, {'Name': 'SapId', 'Issuer': 'LOCAL AUTHORITY', 'Subject': 'Organization', 'Value': '0000036948', 'ValueType': null}, {'Name': 'Role', 'Issuer': 'LOCAL AUTHORITY', 'Subject': 'UserRole', 'Value': 'Admin', 'ValueType': null}]))

    spyOn(loginService, 'login').and.returnValue(of(token))
    spyOn(loginService, 'parseToken')

    // Act
    component.login()

    // Assert
    expect(loginService.login).toHaveBeenCalled()
    expect(loginService.parseToken).toHaveBeenCalled()
  })

  it(`should call login service login - Default`, () => {
  // Arrange
    component.form =  new FormGroup( {
      username: new FormControl('warehouseOne', [Validators.required]),
      password: new FormControl('passWO', [Validators.required])}
    )

    spyOn(loginService, 'login').and.returnValue(of(<any>{
      Id: 1,
      Text: 'mbyICGY8cqJ7ZIheCu4Xg5Rjhz7EFfrVntBcICneP4AoIgYzowIXLiaULjm7M4gbe9YHqiqJp6iegZQXxsj4UoDQM2N3IJDwjZfz',
      UserId: 1,
      CreateDate: new Date('2018-02-16'),
      CreatedBy: 1,
      LastUpdated: new Date('2018-02-16'),
      LastUpdatedBy: 1,
      ClaimDomains: [
        {
          Claims: [
            {
              Issuer: '',
              Name: '',
              Subject: 'UserRole',
              Value: 'Default',
              ValueType: null
            }
          ]
        }
      ]
    }
    ))
    const claims = [{
      'Name': 'Username',
      'Issuer': 'LOCAL AUTHORITY',
      'Subject': 'User',
      'Value': 'admin',
      'ValueType': null
    },              {
      'Name': 'Id',
      'Issuer': 'LOCAL AUTHORITY',
      'Subject': 'User',
      'Value': '3',
      'ValueType': null
    },              {
      'Name': 'LastAuthenticated',
      'Issuer': 'LOCAL AUTHORITY',
      'Subject': 'User',
      'Value': 'Thu, 02 Jan 2020 11:29:10 GMT',
      'ValueType': null
    },              {
      'Name': 'Id',
      'Issuer': 'LOCAL AUTHORITY',
      'Subject': 'Organization',
      'Value': '1',
      'ValueType': null
    },              {
      'Name': 'Name',
      'Issuer': 'LOCAL AUTHORITY',
      'Subject': 'Organization',
      'Value': 'Internal',
      'ValueType': null
    },              {
      'Name': 'SapId',
      'Issuer': 'LOCAL AUTHORITY',
      'Subject': 'Organization',
      'Value': '0000036948',
      'ValueType': null
    },              {
      'Name': 'Role',
      'Issuer': 'LOCAL AUTHORITY',
      'Subject': 'UserRole',
      'Value': 'Default',
      'ValueType': null
    }
  ]
    spyOn(localStorage, 'getItem').and.returnValue(JSON.stringify(claims))
    spyOn(loginService, 'parseToken')

    // Act
    component.login()

    // Assert
    expect(loginService.login).toHaveBeenCalled()
    expect(loginService.parseToken).toHaveBeenCalled()
  })

  it(`should handle login service error`, () => {
    // Arrange
    component.form =  new FormGroup({username: new FormControl('testUser', [Validators.required]),
                                     password: new FormControl('testPW', [Validators.required])})
    const login = spyOn(loginService, 'login').and.returnValue(throwError(new HttpResponse({status: 500, body: 'nope'})))
    const dialogRefSpyObj = jasmine.createSpyObj({ afterClosed : of({}), close: null })
    spyOn(dialog, 'open').and.returnValue(dialogRefSpyObj)

    // Act
    component.login()

    // Assert
    expect(login).toHaveBeenCalled()
  })

  it(`should handle login service 401 error`, () => {
    // Arrange
    component.form =  new FormGroup({username: new FormControl('testUser', [Validators.required]),
                                     password: new FormControl('testPW', [Validators.required])})
    const login = spyOn(loginService, 'login').and.returnValue(throwError(new HttpResponse({status: 401, body: 'nope'})))
    spyOn(loginService, 'logout').and.callThrough()
    const dialogRefSpyObj = jasmine.createSpyObj({ afterClosed : of({}), close: null })
    spyOn(dialog, 'open').and.returnValue(dialogRefSpyObj)

    // Act
    component.login()

    // Assert
    expect(login).toHaveBeenCalled()
    expect(loginService.logout).toHaveBeenCalled()
  })

  it(`should handle login service error 0`, () => {
    // Arrange
    component.form =  new FormGroup({username: new FormControl('testUser', [Validators.required]),
                                     password: new FormControl('testPW', [Validators.required])})
    const login = spyOn(loginService, 'login').and.returnValue(throwError(new HttpResponse({status: 0, body: 'nope'})))
    spyOn(loginService, 'logout').and.callThrough()
    const dialogRefSpyObj = jasmine.createSpyObj({ afterClosed : of({}), close: null })
    spyOn(dialog, 'open').and.returnValue(dialogRefSpyObj)

    // Act
    component.login()

    // Assert
    expect(login).toHaveBeenCalled()
    expect(loginService.logout).toHaveBeenCalled()
    expect(component.showError).toBeTruthy()
  })

  it('should setup initialise correctly', () => {
    // Arrange
    spyOn(component, 'parseParamsAndVerifyToken').and.returnValue()

    // Act
    component.ngOnInit()

    // Assert
    expect(component.parseParamsAndVerifyToken).toHaveBeenCalledTimes(0)
  })

})



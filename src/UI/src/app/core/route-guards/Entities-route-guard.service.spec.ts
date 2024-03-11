import { TestBed } from '@angular/core/testing'
import { RouterTestingModule } from '@angular/router/testing'
import { UserDataService } from '../services/user-data.service'
import { HttpClientTestingModule } from '@angular/common/http/testing'
import { EntitiesRouteGuardService } from './Entities-route-guard.service'
import { ActivatedRouteSnapshot, ParamMap, Router } from '@angular/router'
import { GlobalSnackBarService } from '../services/global-snack-bar.service'
import { SingularizePipe } from '../pipes/singularize.pipe'
import { MatSnackBar } from '@angular/material/snack-bar'
import { Overlay } from '@angular/cdk/overlay'
import { PluralizePipe } from '../pipes/pluralize.pipe'
import { CustomPluralizationMap } from '../models/concretes/custom-pluralization-map'
import { SplitPascalCasePipe } from '../pipes/split-pascal-case.pipe'

describe('EntitiesRouteGuardService', () => {
  let service: EntitiesRouteGuardService
  let userDataService: UserDataService

  const mockSnackbarService = {
    open: () => {}
  }

  const mockRouter = {
    navigate: jasmine.createSpy('navigate')
  }


  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      RouterTestingModule,
      HttpClientTestingModule
    ],
    providers: [
      UserDataService,
      {provide: GlobalSnackBarService, useValue: mockSnackbarService},
      {provide: Router, useValue: mockRouter},
      SingularizePipe,
      PluralizePipe,
      CustomPluralizationMap,
      SplitPascalCasePipe,
      MatSnackBar,
      Overlay
    ]
  }))


  beforeEach(() => {
    service = TestBed.inject(EntitiesRouteGuardService)
    userDataService = TestBed.inject(UserDataService)
  })

  it('should be created', () => expect(service).toBeTruthy())

  describe('CanActivate()', () => {
    const mock = <T, P extends keyof T>(obj: Pick<T, P>): T => obj as T

    it('should return true if user is admin', () => {
        spyOn(userDataService, 'userIsAdmin').and.returnValue(true)

        const route = mock<ActivatedRouteSnapshot, 'url'>({
            url: [
                {
                path: 'entity',
                parameters: {},
                parameterMap: <any> null
                }
            ]
        })

        const result = service.canActivate(route)

        expect(result).toEqual(true)
    })

    it('should return true if user role is allowed to access entity', () => {
      // Arrange
      const permittedEntities = [ 'AlternateId' ]

      spyOn(userDataService, 'userIsAdmin').and.returnValue(false)
      spyOnProperty(userDataService, 'permittedEntitiesForUser', 'get').and.returnValue(permittedEntities)

      const route = mock<ActivatedRouteSnapshot, 'url'>({
            url: [
                {
                path: 'AlternateId',
                parameters: {},
                parameterMap: <any>null
                }
            ]
        })

      // act
      const result = service.canActivate(route)

      // Assert
      expect(result).toEqual(true)
    })

    it('should return false if user role is allowed to access entity', () => {
        const permittedEntities = [
            'AlternateIds'
        ]

        spyOn(userDataService, 'userIsAdmin').and.returnValue(false)
        spyOnProperty(userDataService, 'permittedEntitiesForUser', 'get').and.returnValue(permittedEntities)

        const route = mock<ActivatedRouteSnapshot, 'url'>({
            url: [
                {
                path: 'AlternateIds',
                parameters: {},
                parameterMap: <any>undefined
                }
            ]
        })

        const result = service.canActivate(route)

        expect(result).toEqual(false)
        expect(mockRouter.navigate).toHaveBeenCalledWith(['/admin/data-administration'])
    })

  })
})


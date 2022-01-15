import { TestBed } from '@angular/core/testing'
import { RouterTestingModule } from '@angular/router/testing'
import { UserDataService } from '../services/user-data.service'
import { HttpClientTestingModule } from '@angular/common/http/testing'
import { EntitiesRouteGuardService } from './entities-route-guard.service'
import { ActivatedRouteSnapshot } from '@angular/router'
import { GlobalSnackBarService } from '../services/global-snack-bar.service'
import { SingularizePipe } from '../pipes/singularize.pipe'
import { MatSnackBar } from '@angular/material'
import { Overlay } from '@angular/cdk/overlay'

describe('EntitiesRouteGuardService', () => {
  let service: EntitiesRouteGuardService
  let userDataService: UserDataService

  const mockSnackbarService = {
    open: () => {}
  }

  beforeEach(() => TestBed.configureTestingModule({
    imports: [
      RouterTestingModule,
      HttpClientTestingModule
    ],
    providers: [
      UserDataService,
      {provide: GlobalSnackBarService, useValue: mockSnackbarService},
      SingularizePipe,
      MatSnackBar,
      Overlay
    ]
  }))


  beforeEach(() => {
    service = TestBed.get(EntitiesRouteGuardService)
    userDataService = TestBed.get(UserDataService)
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
                parameterMap: null
                }
            ]
        })

        const result = service.canActivate(route)

        expect(result).toEqual(true)
    })

    it('should return true if user role is allowed admin access to any entity', () => {
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
                parameterMap: null
                }
            ]
        })

        const result = service.canActivate(route)

        expect(result).toEqual(true)
    })

    it('should return false if user role is not allowed admin access to any entity', () => {
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
                parameterMap: null
                }
            ]
        })

        const result = service.canActivate(route)

        expect(result).toEqual(false)
    })

  })
})


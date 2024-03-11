import { HttpClientTestingModule } from '@angular/common/http/testing'
import { waitForAsync, ComponentFixture, TestBed } from '@angular/core/testing'
import { FlexLayoutModule } from '@angular/flex-layout'
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { RouterTestingModule } from '@angular/router/testing'
import { assert } from 'console'
import { of } from 'rxjs'
import { distinct } from 'rxjs/operators'
import { MaterialModule } from 'src/app/core/material/material.module'
import { CustomPluralizationMap } from 'src/app/core/models/concretes/custom-pluralization-map'
import { PluralizePipe } from 'src/app/core/pipes/pluralize.pipe'
import { SplitPascalCasePipe } from 'src/app/core/pipes/split-pascal-case.pipe'
import { EntityService } from 'src/app/core/services/entity.service'

import { ExtensionEntityPropertyAutoCompleteComponent } from './extension-entity-property-auto-complete.component'

const distinctPropertyList: any[] =  ['Test1', 'Test2', 'Test3']

describe('ExtensionEntityPropertyAutoCompleteComponent', () => {
  let component: ExtensionEntityPropertyAutoCompleteComponent

  let fixture: ComponentFixture<ExtensionEntityPropertyAutoCompleteComponent>
  let entityService: EntityService

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [
        MaterialModule,
        ReactiveFormsModule,
        HttpClientTestingModule,
        BrowserAnimationsModule,
        RouterTestingModule,
        FlexLayoutModule
      ],
      providers: [
        EntityService,
        PluralizePipe,
        CustomPluralizationMap,
        SplitPascalCasePipe,
      ],
      declarations: [ ExtensionEntityPropertyAutoCompleteComponent ]
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(ExtensionEntityPropertyAutoCompleteComponent)
    component = fixture.componentInstance
    entityService = TestBed.get(EntityService)
    spyOn(entityService, 'getDistinctExtensionPropertList').and.returnValue(of(distinctPropertyList))
    component.field = {name: 'Property', label: 'Property', type: 'defaultproperty', value: null}
    component.group = new FormGroup({Property: new FormControl('Property')})
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })

  it('Should filter the options list', () => {
    // Arrange
    component.options = distinctPropertyList

    // Act
    const results =  component.filter('2')

    // Assert
    expect(results.length).toEqual(1)
    expect(results[0]).toEqual('Test2')
  })

  it('Should call the EntityService on getDistinctProperties', () => {
    // Arrange
    component.field = { name: 'ALternateId', entity: 'User', searchEntity: 'Property', type: 'Test' }

    // Act
    component.getDistinctProperties()

    // Assert
    expect(entityService.getDistinctExtensionPropertList).toHaveBeenCalledWith('User', 'Property')
  })

  it('displayFn should handle undefined', () => {
    // Arrange

    // Act
    const result = component.displayFn(undefined)

    // Assert
    expect(result).toBeUndefined()
  })

  it('displayFn should handle null', () => {
    // Arrange

    // Act
    const result = component.displayFn(null)

    // Assert
    expect(result).toBeUndefined()
  })

  it('displayFn should return true', () => {
    // Arrange

    // Act
    const result = component.displayFn('Test1')

    // Assert
    expect(result).toBeTruthy()
  })

})

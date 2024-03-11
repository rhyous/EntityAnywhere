import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing'
import { Component, NgModule } from '@angular/core'
import { FormsModule, FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms'
import { FlexModule } from '@angular/flex-layout'
import { EntityFieldDirective } from './entity-field.directive'

@Component({
  selector: 'app-test-component',
  template: '<div appEntityField [field]="field" [group]="form"> </div>'
})
class TestComponent {
  field: any = {SearchEntity: 'Product'}
  form = new FormGroup({enabled: new FormControl('')})
}

@NgModule({
  declarations: [TestComponent, EntityFieldDirective],
})
class TestModule {}


describe('EntityFieldDirective', () => {
  let component: TestComponent
  let fixture: ComponentFixture<TestComponent>

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [ FormsModule, ReactiveFormsModule, TestModule, FlexModule],
      providers: [],
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(TestComponent)
    component = fixture.componentInstance
    component.field.SearchEntity = 'User'
    component.field = 'button'
    fixture.detectChanges()
  })

  // it('should create', () => {
  //   expect(component).toBeTruthy()
  // })
})

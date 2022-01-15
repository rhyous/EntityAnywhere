import { ComponentFactoryResolver, Directive, Input, OnInit, ViewContainerRef } from '@angular/core'
import { FormGroup } from '@angular/forms'

import { FieldConfig } from '../../eaf-common/common-form-module/models/interfaces/field-config.interface'
import { EntitySearcherComponent } from '../../eaf-common/common-form-module/components/entity-searcher/entity-searcher.component'
import { EntityMapperComponent } from '../../eaf-common/common-form-module/components/entity-mapper/entity-mapper.component'
import { EntityInputComponent } from '../../eaf-common/common-form-module/components/entity-input/entity-input.component'
import { EntityCheckboxComponent } from '../../eaf-common/common-form-module/components/entity-checkbox/entity-checkbox.component'
import { EntityButtonComponent } from '../../eaf-common/common-form-module/components/entity-button/entity-button.component'
import { EntitySelectComponent } from '../../eaf-common/common-form-module/components/entity-select/entity-select.component'
import { EntityDateComponent } from '../../eaf-common/common-form-module/components/entity-date/entity-date.component'
import { EntityLabelComponent } from '../../eaf-common/common-form-module/components/entity-label/entity-label.component'

@Directive({
  selector: '[appEntityField]'
})

/**
 * The Entity Field Directive. Responsible for defining the control that should be injected into the form
 */
export class EntityFieldDirective implements OnInit {

  @Input() field: FieldConfig
  @Input() group: FormGroup
  componentRef: any


  constructor(private resolver: ComponentFactoryResolver,
              private container: ViewContainerRef) { }

  ngOnInit(): void {
    const factory = this.resolver.resolveComponentFactory(
      this.getMapper(this.field.searchEntity)[this.field.type])
    this.componentRef = this.container.createComponent(factory)
    this.componentRef.instance.field = this.field
    this.componentRef.instance.group = this.group
  }

  getMapper(entity: string) {
    const componentMapper = {
      input: EntityInputComponent,
      checkbox: EntityCheckboxComponent,
      button: EntityButtonComponent,
      select: EntitySelectComponent,
      date: EntityDateComponent,
      EntitySearcher: EntitySearcherComponent,
      Mapper: EntityMapperComponent,
      label: EntityLabelComponent
    }

    return componentMapper
  }

}


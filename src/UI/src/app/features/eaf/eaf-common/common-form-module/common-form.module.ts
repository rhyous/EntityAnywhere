import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'

import { ExtensionEntityComponent } from './components/extension-entity/extension-entity.component'
import { EntityFormComponent } from './components/entity-form/entity-form.component'
import { EntityMapperComponent } from './components/entity-mapper/entity-mapper.component'
import { EntitySearcherComponent } from './components/entity-searcher/entity-searcher.component'
import { EntityFieldDirective } from '../../eaf-custom/directives/entity-field.directive'
import { EntityButtonComponent } from './components/entity-button/entity-button.component'
import { EntityCheckboxComponent } from './components/entity-checkbox/entity-checkbox.component'
import { EntityDateComponent } from './components/entity-date/entity-date.component'
import { EntityInputComponent } from './components/entity-input/entity-input.component'
import { EntityTextAreaComponent } from './components/entity-text-area/entity-text-area.component'
import { EntityLabelComponent } from './components/entity-label/entity-label.component'
import { EntitySelectComponent } from './components/entity-select/entity-select.component'
import { MaterialModule } from 'src/app/core/material/material.module'
import { CoreModule } from 'src/app/core/core.module'
import { FlexLayoutModule } from '@angular/flex-layout'
import { ForeignEntityComponent } from './components/foreign-entity/foreign-entity.component'
// tslint:disable-next-line: max-line-length
import { ExtensionEntityPropertyAutoCompleteComponent } from '../common-dashboard-module/components/extension-entity-property-auto-complete/extension-entity-property-auto-complete.component'


@NgModule({
  declarations: [
    ExtensionEntityComponent,
    EntityFormComponent,
    EntityMapperComponent,
    EntitySearcherComponent,
    EntityFieldDirective,
    EntityButtonComponent,
    EntityCheckboxComponent,
    EntityDateComponent,
    EntityInputComponent,
    EntityTextAreaComponent,
    EntityLabelComponent,
    EntitySelectComponent,
    ForeignEntityComponent,
    ExtensionEntityPropertyAutoCompleteComponent,
  ],
  imports: [
    CommonModule,
    MaterialModule,
    CoreModule,
    FlexLayoutModule,
  ],
  exports: [
    ExtensionEntityComponent,
    EntityFormComponent,
    EntityMapperComponent,
    EntitySearcherComponent,
    EntityFieldDirective,
    EntityButtonComponent,
    EntityCheckboxComponent,
    EntityDateComponent,
    EntityInputComponent,
    EntityTextAreaComponent,
    EntityLabelComponent,
    EntitySelectComponent,
    ForeignEntityComponent,
    ExtensionEntityPropertyAutoCompleteComponent,
  ],
  entryComponents: [
    EntityInputComponent,
    EntityTextAreaComponent,
    EntityButtonComponent,
    EntitySearcherComponent,
    EntityCheckboxComponent,
    EntitySelectComponent,
    EntityLabelComponent,
    EntityMapperComponent,
    EntityDateComponent,
    ExtensionEntityPropertyAutoCompleteComponent,
  ]

})
export class CommonFormModule { }

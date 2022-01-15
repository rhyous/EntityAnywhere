import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'

import { ExtensionEntityComponent } from './components/extension-entity/extension-entity.component'
import { EntityFormComponent } from './components/entity-form/entity-form.component'
import { EntityMapperComponent } from './components/entity-mapper/entity-mapper.component'
import { EntitySearcherComponent } from './components/entity-searcher/entity-searcher.component'
import { EntityFieldDirective } from '../../eaf-common/directives/entity-field.directive'
import { EntityButtonComponent } from './components/entity-button/entity-button.component'
import { EntityCheckboxComponent } from './components/entity-checkbox/entity-checkbox.component'
import { EntityDateComponent } from './components/entity-date/entity-date.component'
import { EntityInputComponent } from './components/entity-input/entity-input.component'
import { EntityLabelComponent } from './components/entity-label/entity-label.component'
import { EntitySelectComponent } from './components/entity-select/entity-select.component'
import { MaterialModule } from 'src/app/core/material/material.module'
import { CoreModule } from 'src/app/core/core.module'
import { FlexLayoutModule } from '@angular/flex-layout'
import { ForeignEntityComponent } from './components/foreign-entity/foreign-entity.component'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { EntityPropertyControlService } from './services/entity-property-control.service'


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
    EntityLabelComponent,
    EntitySelectComponent,
    ForeignEntityComponent,
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
    EntityLabelComponent,
    EntitySelectComponent,
    ForeignEntityComponent,
  ],
  entryComponents: [
    EntityInputComponent,
    EntityButtonComponent,
    EntitySearcherComponent,
    EntityCheckboxComponent,
    EntitySelectComponent,
    EntityLabelComponent,
    EntityMapperComponent,
    EntityDateComponent,
  ]

})
export class CommonFormModule { }

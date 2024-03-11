import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { FlexLayoutModule } from '@angular/flex-layout'

import { EntitiesListComponent } from './components/entities-list/entities-list.component'
import { EntityListComponent } from './components/entity-list/entity-list.component'
import { EntityDetailComponent } from './components/entity-detail/entity-detail.component'
import { MaterialModule } from 'src/app/core/material/material.module'
import { CoreModule } from 'src/app/core/core.module'
import { CommonFormModule } from '../common-form-module/common-form.module'
import { CommonDialogsModule } from '../common-dialogs-module/common-dialogs.module'
import { EntityPropertyControlService } from '../common-form-module/services/entity-property-control.service'
import { EntityConfigBuilder } from '../common-form-module/services/entity-config-builder'
import { EntityFieldValidatorProvider } from '../common-form-module/services/entity-field-validator-provider'
import { EntityPropertyTypeControlTypeMap } from '../common-form-module/services/entity-property-type-control-type.map'
import { EntityPropertyTypeInputTypeMap } from '../common-form-module/services/entity-property-type-input-type.map'
import { EnumOptionMapper } from '../common-form-module/services/enum-option-mapper'
import { StringTypeControlTypeMap } from '../common-form-module/services/string-type-control-type.map'
import { SpaceTitlePipe } from 'src/app/core/pipes/spacetitle.pipe'

@NgModule({
  declarations: [
    EntitiesListComponent,
    EntityListComponent,
    EntityDetailComponent,
  ],
  imports: [
    CommonModule,
    MaterialModule,
    CoreModule,
    CommonFormModule,
    CommonDialogsModule,
    FlexLayoutModule
  ],
  exports: [
    CommonFormModule,
    CommonDialogsModule,
    FlexLayoutModule,
    EntityListComponent,
    EntityDetailComponent,
  ],
  providers: [
    EntityConfigBuilder,
    EntityFieldValidatorProvider,
    EntityPropertyControlService,
    EntityPropertyTypeControlTypeMap,
    EntityPropertyTypeInputTypeMap,
    EnumOptionMapper,
    StringTypeControlTypeMap,
    SpaceTitlePipe
  ]
})
export class CommonDashboardModule { }

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
    EntityPropertyControlService
  ]
})
export class CommonDashboardModule { }

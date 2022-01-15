import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'

import { CommonDashboardModule } from './common-dashboard-module/common-dashboard.module'
import { CommonDialogsModule } from './common-dialogs-module/common-dialogs.module'
import { CommonFormModule } from './common-form-module/common-form.module'
import { CoreModule } from 'src/app/core/core.module'
import { MaterialModule } from 'src/app/core/material/material.module'
import { FlexLayoutModule } from '@angular/flex-layout'
import { EafCustomModule } from '../eaf-custom/eaf-custom.module'
import { EafCommonComponent } from './eaf-common.component'



@NgModule({
  declarations: [
    EafCommonComponent
  ],
  imports: [
    CommonModule,
    CommonDashboardModule,
    CommonDialogsModule,
    CommonFormModule,
    MaterialModule,
    CoreModule,
    FlexLayoutModule,
    EafCustomModule
  ],
  exports: [
    CommonDashboardModule,
    CommonDialogsModule,
    CommonFormModule,
    FlexLayoutModule
  ]
})
export class EafCommonModule { }

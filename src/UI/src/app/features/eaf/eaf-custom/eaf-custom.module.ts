import { NgModule } from '@angular/core'
import { FilterComponent } from './components/filter/filter.component'
import { MaterialModule } from 'src/app/core/material/material.module'
import { FlexLayoutModule } from '@angular/flex-layout'
import { CoreModule } from 'src/app/core/core.module'
import { CommonFormModule } from '../eaf-common/common-form-module/common-form.module'
import { CommonDialogsModule } from '../eaf-common/common-dialogs-module/common-dialogs.module'
import { EafCustomComponent } from './eaf-custom.component'

@NgModule({
  declarations: [
    EafCustomComponent,
    FilterComponent,
  ],
  imports: [
    MaterialModule,
    CoreModule,
    FlexLayoutModule,
    CommonFormModule,
    CommonDialogsModule
  ],
  exports: [
    FlexLayoutModule
  ],
  entryComponents: [
  ]
})
export class EafCustomModule { }

import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'

import { EntityFormDialogComponent } from './components/entity-form-dialog/entity-form-dialog.component'
import { ExtensionEntityDialogComponent } from './components/extension-entity-dialog/extension-entity-dialog.component'
import { MapperDialogComponent } from './components/mapper-dialog/mapper-dialog.component'
import { EntitySearcherDialogComponent } from './components/entity-searcher-dialog/entity-searcher-dialog.component'
import { MaterialModule } from 'src/app/core/material/material.module'
import { CoreModule } from 'src/app/core/core.module'
import { FlexLayoutModule } from '@angular/flex-layout'
import { CommonFormModule } from '../common-form-module/common-form.module'
import { EntityMultiUpdateDialogComponent } from './components/entity-multi-update-dialog/entity-multi-update-dialog.component'

@NgModule({
  declarations: [
    EntityFormDialogComponent,
    ExtensionEntityDialogComponent,
    MapperDialogComponent,
    EntitySearcherDialogComponent,
    EntityMultiUpdateDialogComponent
  ],
  imports: [
    CommonModule,
    MaterialModule,
    CoreModule,
    FlexLayoutModule,
    CommonFormModule
  ],
  exports: [
    EntityFormDialogComponent,
    ExtensionEntityDialogComponent,
    MapperDialogComponent,
    EntitySearcherDialogComponent,
    EntityMultiUpdateDialogComponent
  ],
  entryComponents: [
    ExtensionEntityDialogComponent,
    EntitySearcherDialogComponent,
    MapperDialogComponent,
    EntityFormDialogComponent,
    EntityMultiUpdateDialogComponent
  ]
})
export class CommonDialogsModule { }

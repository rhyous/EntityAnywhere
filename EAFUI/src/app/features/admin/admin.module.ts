import { NgModule } from '@angular/core'
import { AdminComponent } from './admin.component'
import { CoreModule } from 'src/app/core/core.module'
import { AdminRoutingModule } from './admin-routing.module'
import { AdminMenuComponent } from './components/admin-menu/admin-menu.component'
import { AdminDataService } from './services/admin-data.service'
import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component';

@NgModule({
  declarations: [
    AdminComponent,
    AdminMenuComponent,
    BreadcrumbComponent
  ],
  imports: [
    CoreModule,
    AdminRoutingModule,
  ],
  exports: [
    AdminComponent,
  ],
  entryComponents: [
  ],
  providers: [
    AdminDataService
  ]
})
export class AdminModule { }

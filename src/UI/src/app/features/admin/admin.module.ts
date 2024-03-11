import { NgModule } from '@angular/core'
import { AdminComponent } from './admin.component'
import { CoreModule } from 'src/app/core/core.module'
import { AdminRoutingModule } from './admin-routing.module'
import { AdminMenuComponent } from './components/admin-menu/admin-menu.component'
import { AdminDataService } from './services/admin-data.service'
import { ProductFileUploadComponent } from './components/product-file-upload/product-file-upload.component'
import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component'
import { ImpersonateCustomerComponent } from './components/impersonate-customer/impersonate-customer.component'
import { PluralizePipe } from 'src/app/core/pipes/pluralize.pipe'
import { CustomPluralizationMap } from 'src/app/core/models/concretes/custom-pluralization-map'
import { SplitPascalCasePipe } from 'src/app/core/pipes/split-pascal-case.pipe'

@NgModule({
    declarations: [
        AdminComponent,
        AdminMenuComponent,
        ProductFileUploadComponent,
        BreadcrumbComponent,
        ImpersonateCustomerComponent
    ],
    imports: [
        CoreModule,
        AdminRoutingModule,
    ],
    exports: [
        AdminComponent,
    ],
    providers: [
        AdminDataService,
        PluralizePipe,
        CustomPluralizationMap,
        SplitPascalCasePipe
    ]
})
export class AdminModule { }

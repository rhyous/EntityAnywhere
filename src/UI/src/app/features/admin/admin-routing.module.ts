import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { AdminComponent } from './admin.component'
import { AdminMenuComponent } from './components/admin-menu/admin-menu.component'
import { ProductFileUploadComponent } from './components/product-file-upload/product-file-upload.component'
import { AdminRouteGuardService } from 'src/app/core/route-guards/admin-route-guard.service'
import { ImpersonateCustomerComponent } from './components/impersonate-customer/impersonate-customer.component'

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    canActivate: [AdminRouteGuardService],
    children: [
      { path: 'dashboard', component: AdminMenuComponent },
      { path: 'product-file-upload', component: ProductFileUploadComponent },
      { path: 'impersonate-customer', component: ImpersonateCustomerComponent },
      {
        path: 'data-administration',
        loadChildren: () => import('../eaf/eaf.module').then(x => x.EafModule),
      }
    ]

  }
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }

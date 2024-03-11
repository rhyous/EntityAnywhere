import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { LoginComponent } from './core/login/login.component'
import { AdminRouteGuardService } from './core/route-guards/admin-route-guard.service'
import { DefaultRouteGuardService } from './core/route-guards/default-route-guard.service'

const routes: Routes = [
  { path: '', component: LoginComponent },
  {
    path: 'admin',
    loadChildren: () => import('./features/admin/admin.module').then(x => x.AdminModule),
    canLoad: [AdminRouteGuardService]
  },
  {
    path: 'default',
    loadChildren: () => import('./features/default/default.module').then(x => x.DefaultModule),
    canLoad: [DefaultRouteGuardService]
  }
]

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }

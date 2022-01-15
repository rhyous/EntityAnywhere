import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { AdminComponent } from './admin.component'
import { AdminMenuComponent } from './components/admin-menu/admin-menu.component'
import { AdminRouteGuardService } from 'src/app/core/route-guards/admin-route-guard.service'

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    canActivate: [AdminRouteGuardService],
    children: [
      { path: 'dashboard', component: AdminMenuComponent },
      {
        path: 'data-management',
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

import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { EntitiesListComponent } from './eaf-common/common-dashboard-module/components/entities-list/entities-list.component'
import { EntityListComponent } from './eaf-common/common-dashboard-module/components/entity-list/entity-list.component'
import { EntityDetailComponent } from './eaf-common/common-dashboard-module/components/entity-detail/entity-detail.component'
import { EntitiesRouteGuardService } from 'src/app/core/route-guards/entities-route-guard.service'


const eafRoutes: Routes = [
  {
    path: '',
    children: [
      // The user is on the Admin section. Show all the entities
      { path: '', component: EntitiesListComponent },

      // The Entity is pluralised. Show the List Component
      { path: ':entityPlural', component: EntityListComponent, canActivate: [EntitiesRouteGuardService]},

      // Display a specific entity
      { path: ':entity/:id', component: EntityDetailComponent, canActivate: [EntitiesRouteGuardService]},

      // Can't match any route. Redirect the user back to the admin section
      { path: '**', redirectTo: '/admin/dashboard', pathMatch: 'full'  }
    ]
  }
]

@NgModule({
  imports: [RouterModule.forChild(eafRoutes)],
  exports: [RouterModule]
})
export class EafRoutingModule { }

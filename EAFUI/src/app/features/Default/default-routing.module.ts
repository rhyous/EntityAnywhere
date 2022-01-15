import { NgModule } from '@angular/core'
import { Routes, RouterModule } from '@angular/router'
import { DefaultComponent } from './default.component'
import { MatTab } from '@angular/material'
import { DefaultLandingPageComponent } from './components/default-landing-page/default-landing-page.component'

const routes: Routes = [
  {
    path: '',
    component: DefaultComponent,

    children: [
      // Landing Pages
      { path: 'default', component: DefaultLandingPageComponent },

      // Custom
    ]
  }
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class DefaultRoutingModule { }

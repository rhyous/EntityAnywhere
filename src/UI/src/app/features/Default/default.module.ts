// tslint:disable: max-line-length
import { NgModule } from '@angular/core'
import { DefaultComponent } from './default.component'
import { DefaultRoutingModule } from './default-routing.module'
import { CoreModule } from 'src/app/core/core.module'
import { DefaultLandingPageComponent } from './components/default-landing-page/default-landing-page.component'

@NgModule({
    declarations: [
    DefaultComponent,
    DefaultLandingPageComponent
    ],
    imports: [
        CoreModule,
        DefaultRoutingModule,
    ],
    providers: [
    ]
})
export class DefaultModule { }

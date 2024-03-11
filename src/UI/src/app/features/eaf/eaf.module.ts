import { NgModule} from '@angular/core'
import { CommonModule, DatePipe, DecimalPipe } from '@angular/common'
import { FlexLayoutModule, BREAKPOINTS } from '@angular/flex-layout'
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { HttpClientModule } from '@angular/common/http'
import { EafCommonModule } from './eaf-common/eaf-common.module'
import { CoreModule } from 'src/app/core/core.module'
import { MaterialModule } from 'src/app/core/material/material.module'
import { EafRoutingModule } from './eaf-routing.module'
import { EntityHelperService } from './eaf-common/common-form-module/services/entity-helper.service'
import { EafCustomModule } from './eaf-custom/eaf-custom.module'
import { EafComponent } from './eaf.component'


@NgModule({
    imports: [
        CommonModule,
        FlexLayoutModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
        EafCommonModule,
        EafCustomModule,
        CoreModule,
        MaterialModule,
        EafRoutingModule,
    ],
    declarations: [
        EafComponent
    ],
    exports: [
        EafCommonModule,
        CoreModule,
        FlexLayoutModule
    ],
    providers: [DatePipe, DecimalPipe, EntityHelperService]
})
export class EafModule { }

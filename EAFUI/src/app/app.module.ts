import { NgModule } from '@angular/core'

import { AppRoutingModule } from './app-routing.module'
import { AppComponent } from './app.component'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { ReactiveFormsModule } from '@angular/forms'
import { LoginService } from './core/login/login.service'
import { MaterialModule } from './core/material/material.module'
import { HttpClientModule } from '@angular/common/http'

import '../app/core/infrastructure/linq'
import { CoreModule } from './core/core.module'
import { DefaultDataService } from './core/services/default-data.service'
import { DecimalPipe } from '@angular/common'

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    MaterialModule,
    CoreModule
  ],
  exports: [
    AppRoutingModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    HttpClientModule,
    MaterialModule,
    CoreModule
  ],
  providers: [
    LoginService,
    DefaultDataService,
    DecimalPipe
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

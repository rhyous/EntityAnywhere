// Angular Modules
import { NgModule, ErrorHandler } from '@angular/core'
import { CommonModule } from '@angular/common'
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'
import { ReactiveFormsModule, FormsModule } from '@angular/forms'
import { ClipboardModule } from 'ngx-clipboard'

// Local Modules
import { MaterialModule } from './material/material.module'

// Local Components
import { SidebarMenuComponent } from './sidebar-menu/sidebar-menu.component'

// Local dialogs
import { ErrorDialogComponent } from './dialogs/error-dialog/error-dialog.component'
import { ConfirmDialogComponent } from './dialogs/confirm-dialog/confirm-dialog.component'
import { ErrorReporterDialogComponent } from './dialogs/error-reporter-dialog/error-reporter-dialog.component'
import { MessageDialogComponent } from './dialogs/message-dialog/message-dialog.component'

// Local Components, pipes and services
import { EntityCommonPipe } from './pipes/common.pipe'
import { PluralizePipe } from './pipes/pluralize.pipe'
import { SingularizePipe } from './pipes/singularize.pipe'
import { SpaceTitlePipe } from './pipes/spacetitle.pipe'
import { EntityMetadataService } from './services/entity-metadata.service'

// Local services
import { EntityService } from './services/entity.service'
import { AppLocalStorageService } from './services/local-storage.service'

// Other imports
import { CustomBreakPointProvider } from './infrastructure/custom-breakpoint'
import { MAT_DATE_LOCALE } from '@angular/material/core'
import { GlobalErrorHandler } from './infrastructure/global-error-handler'
import { HttpInterceptor } from './infrastructure/http-interceptor'
import { LoginComponent } from './login/login.component'
import { UserDataService } from './services/user-data.service'
import { AutoCompleteComponent } from './auto-complete/auto-complete.component'
import { UploadFileComponent } from './upload-file/upload-file.component'
import { DragDropDirective } from './directives/drag-drop.directive'
import { GlobalSnackBarService } from './services/global-snack-bar.service'
import { FileService } from './services/file.service'
import { GlobalMatDialogService } from './services/global-mat-dialog.service'
import { DefaultService } from './services/default.service'
import { RouterModule } from '@angular/router'
import { FileApiService } from './services/file-api.service'
import { ArraySortOrderPipe } from './pipes/array-sort-order.pipe'
import { WcfFormatPipe } from './pipes/wcf-format.pipe'
import { ListToStringPipe } from './pipes/list-to-string.pipe'
import { CookieBannerComponent } from './cookie-banner/cookie-banner.component'
import { RemoveHyphensPipe } from './pipes/remove-hyphens.pipe'
import { SplitPascalCasePipe } from './pipes/split-pascal-case.pipe'
import { FormatVersionForSortingPipe } from './pipes/format-version-for-sorting.pipe'
import { ImpersonateCustomerBannerComponent } from './impersonate-customer-banner/impersonate-customer-banner.component'
import { AuthorizationService } from './services/authorization.service'
import { FooterComponent } from './footer/footer.component'
import { WellKnownProperties } from './models/concretes/well-known-properties'

@NgModule({
    declarations: [
        // Directives
        DragDropDirective,
        // Components
        SidebarMenuComponent,
        FooterComponent,
        LoginComponent,
        // Dialogs
        ErrorDialogComponent,
        ErrorReporterDialogComponent,
        ConfirmDialogComponent,
        MessageDialogComponent,
        ConfirmDialogComponent,
        // Pipes
        SplitPascalCasePipe,
        SingularizePipe,
        PluralizePipe,
        SpaceTitlePipe,
        EntityCommonPipe,
        ArraySortOrderPipe,
        WcfFormatPipe,
        ListToStringPipe,
        AutoCompleteComponent,
        UploadFileComponent,
        CookieBannerComponent,
        RemoveHyphensPipe,
        SplitPascalCasePipe,
        FormatVersionForSortingPipe,
        ImpersonateCustomerBannerComponent
    ],
    imports: [
        CommonModule,
        HttpClientModule,
        FormsModule,
        ReactiveFormsModule,
        MaterialModule,
        ClipboardModule,
        RouterModule
    ],
    exports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule,
        MaterialModule,
        ClipboardModule,
        LoginComponent,
        // Directives
        DragDropDirective,
        // Components
        ErrorDialogComponent,
        ErrorReporterDialogComponent,
        ConfirmDialogComponent,
        MessageDialogComponent,
        ConfirmDialogComponent,
        SidebarMenuComponent,
        FooterComponent,
        AutoCompleteComponent,
        UploadFileComponent,
        CookieBannerComponent,
        // Pipes
        SplitPascalCasePipe,
        SingularizePipe,
        PluralizePipe,
        SpaceTitlePipe,
        EntityCommonPipe,
        ArraySortOrderPipe,
        WcfFormatPipe,
        RemoveHyphensPipe,
        SplitPascalCasePipe
    ],
    providers: [
        { provide: MAT_DATE_LOCALE, useValue: 'en-US' },
        { provide: ErrorHandler, useClass: GlobalErrorHandler },
        { provide: HTTP_INTERCEPTORS, useClass: HttpInterceptor, multi: true },
        EntityMetadataService,
        EntityService,
        CustomBreakPointProvider,
        SplitPascalCasePipe,
        PluralizePipe,
        SingularizePipe,
        ArraySortOrderPipe,
        WcfFormatPipe,
        ErrorReporterDialogComponent,
        UserDataService,
        GlobalSnackBarService,
        GlobalMatDialogService,
        FileService,
        DefaultService,
        FileApiService,
        AuthorizationService,
        WellKnownProperties
    ]
})
/** The core module. Responsible for importing and exporting Components and Services that are used throughout every Feature module */
export class CoreModule {

}

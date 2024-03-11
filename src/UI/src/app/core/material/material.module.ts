import { NgModule } from '@angular/core'
import { MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter'
import { MatAutocompleteModule } from '@angular/material/autocomplete'
import { MatButtonModule } from '@angular/material/button'
import { MatCardModule } from '@angular/material/card'
import { MatCheckboxModule } from '@angular/material/checkbox'
import { MatNativeDateModule } from '@angular/material/core'
import { MatDatepickerModule } from '@angular/material/datepicker'
import { MatDialogModule } from '@angular/material/dialog'
import { MatExpansionModule } from '@angular/material/expansion'
import { MatFormFieldModule } from '@angular/material/form-field'
import { MatIconModule } from '@angular/material/icon'
import { MatInputModule } from '@angular/material/input'
import { MatPaginatorModule } from '@angular/material/paginator'
import { MatProgressBarModule } from '@angular/material/progress-bar'
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner'
import { MatSelectModule } from '@angular/material/select'
import { MatSidenavModule } from '@angular/material/sidenav'
import { MatSnackBarModule } from '@angular/material/snack-bar'
import { MatSortModule } from '@angular/material/sort'
import { MatTableModule } from '@angular/material/table'
import { MatTabsModule } from '@angular/material/tabs'
import { MatTooltipModule } from '@angular/material/tooltip'
import { MatToolbarModule } from '@angular/material/toolbar'

@NgModule({
  imports: [
    MatTableModule,
    MatButtonModule,
    MatCardModule,
    MatInputModule,
    MatProgressSpinnerModule,
    MatDialogModule,
    MatDatepickerModule,
    MatPaginatorModule,
    MatCheckboxModule,
    MatSnackBarModule,
    MatSortModule,
    MatProgressBarModule,
    MatIconModule,
    MatAutocompleteModule,
    MatSelectModule,
    MatTabsModule,
    MatTooltipModule,
    MatToolbarModule,
    MatFormFieldModule,
    MatNativeDateModule,
    MatExpansionModule,
    MatSidenavModule
  ],
  declarations: [],
  exports: [
    MatTableModule,
    MatButtonModule,
    MatCardModule,
    MatInputModule,
    MatProgressSpinnerModule,
    MatDialogModule,
    MatDatepickerModule,
    MatPaginatorModule,
    MatCheckboxModule,
    MatSnackBarModule,
    MatSortModule,
    MatProgressBarModule,
    MatIconModule,
    MatAutocompleteModule,
    MatSelectModule,
    MatTabsModule,
    MatTooltipModule,
    MatToolbarModule,
    MatFormFieldModule,
    MatNativeDateModule,
    MatExpansionModule,
    MatSidenavModule,
  ],
  providers: [
    { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: true } }
  ]
})
/**Responsible for importing and exporting the necessary Material modules */
export class MaterialModule {}

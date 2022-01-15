import { NgModule } from '@angular/core'
import { MAT_MOMENT_DATE_ADAPTER_OPTIONS } from '@angular/material-moment-adapter'
import {
  MatCardModule,
  MatButtonModule,
  MatTableModule,
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
  MatFormFieldModule,
  MatNativeDateModule,
  MatExpansionModule,
  MatSidenavModule
} from '@angular/material'

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

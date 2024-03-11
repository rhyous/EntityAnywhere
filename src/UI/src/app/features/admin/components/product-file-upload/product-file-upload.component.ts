import { Component, OnInit, ViewChild } from '@angular/core'
import { IComponentProgressBar } from 'src/app/core/infrastructure/core-interfaces'
import { AutocompleteHelper } from 'src/app/core/infrastructure/autocomplete-helper'
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms'
import { IAutoCompleteDisplayFunction, DisplayFnData } from 'src/app/core/auto-complete/auto-complete-display-function.interface'
import { Product } from '../../../../core/models/interfaces/o-data-entities/product.interface'
import { FileApiService } from '../../../../core/services/file-api.service'
import { ApiFile } from '../../../../core/models/interfaces/o-data-entities/file.interface'
import { ODataCollection } from 'src/app/core/models/interfaces/o-data-collection.interface'
import { GlobalMatDialogService } from 'src/app/core/services/global-mat-dialog.service'
import { GlobalSnackBarService } from 'src/app/core/services/global-snack-bar.service'
import { ODataObject } from 'src/app/core/models/interfaces/o-data-entities/o-data-object.interface'
import { UploadFileComponent } from 'src/app/core/upload-file/upload-file.component'

@Component({
  selector: 'app-product-file-upload',
  templateUrl: './product-file-upload.component.html',
  styleUrls: ['./product-file-upload.component.scss']
})
export class ProductFileUploadComponent implements OnInit, IComponentProgressBar, IAutoCompleteDisplayFunction<Product> {
  displayProgressBar = false
  lookingUpFiles = false

  productIdField = AutocompleteHelper.createAutocompleteField('ProductId', 'Product *')
  form: FormGroup = new FormGroup({})

  currentFiles!: ODataCollection<ApiFile>

  @ViewChild(UploadFileComponent, {static: true}) uploadFileComponent!: UploadFileComponent

  constructor(private fb: FormBuilder,
              private fileApiService: FileApiService,
              private matDialog: GlobalMatDialogService,
              private matSnackBar: GlobalSnackBarService) { }

  ngOnInit() {
    this.form = this.fb.group({
      ProductId: new FormControl('', Validators.required)
    })
    this.subscribeToProductIdChange()
  }

  displayFn(option: DisplayFnData<Product>): any {
    return option?.key ? `${option.key} (Version: ${option.Entity?.Object?.Version})` : undefined
  }

  subscribeToProductIdChange() {
    this.form.valueChanges.subscribe((next: any) => {
      if (next.ProductId instanceof Object) {
        // Check the api for any files
        this.getfilesByProduct(next.ProductId.Entity)
      }
    })
  }

  getfilesByProduct(product: ODataObject<Product>) {
    if (product) {
      this.currentFiles = <any>null
      this.displayProgressBar = true
      this.lookingUpFiles = true
      this.fileApiService.getFilesByProduct(product.Id).subscribe((result: any) => {
        this.currentFiles = result
        this.displayProgressBar = false
        this.lookingUpFiles = false
      })
    }
  }

  get showUpload(): boolean {
    return this.form.valid &&
           this.form.value.ProductId instanceof Object &&
           this.uploadFileComponent.file !== null
  }

  upload() {
    this.displayProgressBar = true
    this.fileApiService.checkFileExists(this.uploadFileComponent.file.name,
                                        <any>null,
                                        <any>null,
                                        (<ODataObject<Product>>this.form.value.ProductId).Id, <any>null).subscribe((next: any) => {
        if (next === false) {
          const reader = new FileReader()
          reader.onload = (ev) => {
            const buffer = reader.result
            const byteArray = new Uint8Array(<ArrayBuffer>buffer)
            const request = { Name: this.uploadFileComponent.file.name, Data: [].slice.call(byteArray) }
            this.uploadFile(request, this.form.value.ProductId)
          }

          reader.readAsArrayBuffer(this.uploadFileComponent.file)
        } else {
          this.matSnackBar.open('That file already exists. Please update the filename and reupload')
        }
      })
  }

  deleteFile(file: ApiFile) {
    const dialogRef = this.matDialog.openConfirmDialog({
      confirm: true,
      message: `Are you sure you wish to delete ${file.Name}`,
      title: 'Confirm Delete'
    })

    dialogRef.afterClosed().subscribe((next: any) => {
      if (next === 'confirmed') {
        this.displayProgressBar = true
        this.fileApiService.deleteFile(file.Id).subscribe((result: any) => {
          this.matSnackBar.open('File deleted successfully')
          this.displayProgressBar = false
          this.getfilesByProduct(this.form.value.ProductId)
        })
      }
    })
  }

  uploadFile(request: any, product: ODataObject<Product>) {
    this.displayProgressBar = true
    this.fileApiService.postFileUpload(null, null, product.Id, request, null)
      .subscribe((next: any) => {
        this.matSnackBar.open('File uploaded successfully')
        this.displayProgressBar = false
        this.getfilesByProduct(this.form.value.ProductId.Entity)
        this.uploadFileComponent.clearFiles()
    },           (error: any) => {
      this.matSnackBar.open('There was an error uploading the file')
      this.displayProgressBar = false
    })
  }

}

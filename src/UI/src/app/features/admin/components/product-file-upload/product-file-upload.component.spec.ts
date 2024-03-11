// import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing'

// import { ProductFileUploadComponent } from './product-file-upload.component'
// import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
// import { ReactiveFormsModule, FormBuilder } from '@angular/forms'
// import { HttpClientTestingModule } from '@angular/common/http/testing'
// import { RouterTestingModule } from '@angular/router/testing'
// import { MaterialModule } from 'src/app/core/material/material.module'
// import { EntityMetadataService } from 'src/app/core/services/entity-metadata.service'
// import { Fake } from 'src/app/core/services/entity-metadata-fake'
// import { FileApiService } from '../../../../core/services/file-api.service'
// import { GlobalMatDialogService } from 'src/app/core/services/global-mat-dialog.service'
// import { GlobalSnackBarService } from 'src/app/core/services/global-snack-bar.service'
// import { AutoCompleteComponent } from 'src/app/core/auto-complete/auto-complete.component'
// import { UploadFileComponent } from 'src/app/core/upload-file/upload-file.component'
// import { of } from 'rxjs'
// import { AppLocalStorageService } from 'src/app/core/services/local-storage.service'
// import { FakeAppLocalStorageService } from 'src/app/core/services/mocks/mocks'
// import { PluralizePipe } from 'src/app/core/pipes/pluralize.pipe'
// import { CustomPluralizationMap } from 'src/app/core/models/concretes/custom-pluralization-map'
// import { SplitPascalCasePipe } from 'src/app/core/pipes/split-pascal-case.pipe'

// const fakeFile = {
//   lastModified: new Date().getDate(),
//   name: 'filename.txt',
//   size: 16,
//   type: 'txt',
//   slice: (x: any, y: any, z: any) => null
// }

// fdescribe('ProductFileUploadComponent', () => {
//   let component: ProductFileUploadComponent
//   let fixture: ComponentFixture<ProductFileUploadComponent>
//   let matSnackBar: GlobalSnackBarService
//   let matDialog: GlobalMatDialogService

//   beforeEach(waitForAsync(() => {
//     TestBed.configureTestingModule({
//       imports: [
//         BrowserAnimationsModule,
//         ReactiveFormsModule,
//         HttpClientTestingModule,
//         RouterTestingModule,
//         MaterialModule,
//       ],
//       declarations: [ ProductFileUploadComponent, AutoCompleteComponent, UploadFileComponent ],
//       providers: [
//         {
//           provide: EntityMetadataService,
//           useValue: { getEntityMetaData: (entityName: any) => Fake.FakeMeta.firstOrDefault(x => x.key === entityName) }
//         },
//         { provide: AppLocalStorageService, useClass: FakeAppLocalStorageService },
//         FormBuilder,
//         FileApiService,
//         GlobalMatDialogService,
//         GlobalSnackBarService,
//         PluralizePipe,
//         CustomPluralizationMap,
//         SplitPascalCasePipe
//       ]
//     })
//     .compileComponents()
//   }))

//   beforeEach(() => {
//     fixture = TestBed.createComponent(ProductFileUploadComponent)
//     component = fixture.componentInstance
//     fixture.detectChanges()
//   })

//   beforeEach(() => {
//     matDialog = TestBed.inject(GlobalMatDialogService)
//     matSnackBar = TestBed.inject(GlobalSnackBarService)
//   })

//   it('should create', () => {
//     expect(component).toBeTruthy()
//   })

//   describe('displayFn', () => {
//     it('should handle undefined', () => {
//       // Arrange

//       // Act
//       const result = component.displayFn(<any>undefined)

//       // Assert
//       expect(result).toBeUndefined()
//     })

//     it('should handle null', () => {
//       // Arrange

//       // Act
//       const result = component.displayFn(<any>null)

//       // Assert
//       expect(result).toBeUndefined()
//     })

//     it('should amalgamate the product name and the version', () => {
//       // Arrange


//       // Act
//       const result = component.displayFn({
//         key: 'My Product',
//         value: 1234,
//         Entity: {
//           Id: 1234,
//           Object: {
//             CreateDate: new Date(),
//             Description: 'A fake product',
//             Enabled: true,
//             Id: 1234,
//             LastUpdated: new Date(),
//             Name: 'My Product',
//             TypeId: 1,
//             Version: '1.0',
//             IsHidden: false
//           }
//         }
//       })

//       // Assert
//       expect(result).toEqual('My Product (Version: 1.0)')
//     })
//   })

//   describe('getFilesByProduct', () => {
//     let fileApiService: FileApiService

//     beforeEach(() => {
//       fileApiService = TestBed.inject(FileApiService)
//     })

//     it('should call the api service with the id and store the result', () => {
//       // Arrange
//       const response = {
//         Count: 1,
//         Entities: [<any>{
//           Id: 1,
//           Object: {
//             Id: 1,
//             FileName: 'file.txt'
//           }
//         }],
//         Entity: 'File',
//         TotalCount: 1
//       }
//       spyOn(fileApiService, 'getFilesByProduct').and.returnValue(of(response))

//       // Act
//       component.getfilesByProduct(<any>{
//         Id: 1,
//         Object: {
//           Id: 1
//         },
//       })

//       // Assert
//       expect(fileApiService.getFilesByProduct).toHaveBeenCalledWith(1)
//       expect(component.currentFiles).toEqual(response)
//     })
//   })

//   describe('showUpload', () => {
//     it('should be false if the form is invalid', () => {
//       // Arrange

//       // Act


//       // Assert
//       expect(component.form.valid).toBeFalsy()
//       expect(component.showUpload).toBeFalsy()
//     })

//     it('should be false if the file is null or undefined', () => {
//       // Arrange
//       component.form.patchValue({
//         ProductId: {Id: 1, Value: 'Value'}
//       })

//       // Act


//       // Assert
//       expect(component.form.valid).toBeTruthy()
//       expect(component.form.value.ProductId instanceof Object).toBeTruthy()
//       expect(component.showUpload).toBeFalsy()
//     })
//   })

//   describe('deleteFile', () => {
//     let fileApiService: FileApiService
//     beforeEach(() => {
//       fileApiService = TestBed.inject(FileApiService)
//       spyOn(fileApiService, 'deleteFile').and.returnValue(of(''))
//     })

//     it('should call the fileservice if the user confirms', () => {
//       // Arrange
//       spyOn(matDialog, 'openConfirmDialog').and.returnValue(<any>{
//         afterClosed: () => of('confirmed')
//       })
//       spyOn(matSnackBar, 'open')

//       // Act
//       component.deleteFile({
//         CreateDate: new Date(),
//         Id: 'MYGUID',
//         Name: 'FileName.txt',
//         Order: '0',
//         Path: 'path',
//         Product: '1',
//         Type: '1',
//         Version: '1.1'
//       })

//       // Assert
//       expect(matDialog.openConfirmDialog).toHaveBeenCalledWith({
//         confirm: true,
//         message: `Are you sure you wish to delete FileName.txt`,
//         title: 'Confirm Delete'
//       })
//       expect(fileApiService.deleteFile).toHaveBeenCalledWith('MYGUID')
//       expect(matSnackBar.open).toHaveBeenCalledWith('File deleted successfully')
//     })

//     it('shouldnt call the fileservice if the user doesnt confirm', () => {
//       // Arrange
//       spyOn(matDialog, 'openConfirmDialog').and.returnValue(<any>{
//         afterClosed: () => of('cancelled')
//       })
//       spyOn(matSnackBar, 'open')

//       // Act
//       component.deleteFile({
//         CreateDate: new Date(),
//         Id: 'MYGUID',
//         Name: 'FileName.txt',
//         Order: '0',
//         Path: 'path',
//         Product: '1',
//         Type: '1',
//         Version: '1.1'
//       })

//       // Assert
//       expect(matDialog.openConfirmDialog).toHaveBeenCalledWith({
//         confirm: true,
//         message: `Are you sure you wish to delete FileName.txt`,
//         title: 'Confirm Delete'
//       })
//       expect(fileApiService.deleteFile).not.toHaveBeenCalled()
//       expect(matSnackBar.open).not.toHaveBeenCalled()
//     })

//   })
// })

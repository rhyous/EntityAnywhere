import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing'

import { UploadFileComponent } from './upload-file.component'
import { CommonModule } from '@angular/common'
import { MaterialModule } from '../material/material.module'
import { HttpClientTestingModule } from '@angular/common/http/testing'
import { ReactiveFormsModule } from '@angular/forms'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'

describe('UploadFileComponent', () => {
  let component: UploadFileComponent
  let fixture: ComponentFixture<UploadFileComponent>

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ UploadFileComponent ],
      imports: [
        CommonModule,
        MaterialModule,
        HttpClientTestingModule,
        ReactiveFormsModule,
        BrowserAnimationsModule
      ],
    })
    .compileComponents()
  }))

  beforeEach(() => {
    fixture = TestBed.createComponent(UploadFileComponent)
    component = fixture.componentInstance
    fixture.detectChanges()
  })

  it('should create', () => {
    expect(component).toBeTruthy()
  })

  it('should set the file to null on delete', () => {
    // Arrange
    const blob: any = new Blob([''], { type: 'text/html' })
    blob['lastModifiedDate'] = ''
    blob['name'] = 'filename'

    const fakeF = <File>blob
    component['_file'] = fakeF
    // Act
    component.deleteFile()

    // Assert
    expect(component.file).toBeNull()
  })

  it('should set the file to null on clear', () => {
    // Arrange
    const blob: any = new Blob([''], { type: 'text/html' })
    blob['lastModifiedDate'] = ''
    blob['name'] = 'filename'

    const fakeF = <File>blob
    component['_file'] = fakeF
    // Act
    component.clearFiles()

    // Assert
    expect(component.file).toBeNull()
  })

  it('should use event emitter on file upload', () => {
    // Arrange
    const blob: any = new Blob([''], { type: 'text/html' })
    blob['lastModifiedDate'] = ''
    blob['name'] = 'filename'

    const fakeF = <File>blob
    const fakefileList = {
      0: fakeF,
      1: fakeF,
      length: 2,
      item: (index: number) => fakeF
    }

    component.isButton = true
    spyOn(component.getFile, 'emit')

    // Act
    component.uploadFile(fakefileList)

    // Assert
    expect(component.getFile.emit).toHaveBeenCalled()
  })

})

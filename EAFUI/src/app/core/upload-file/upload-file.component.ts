import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core'
import { FormControl } from '@angular/forms'
import { debounceTime } from 'rxjs/operators'

@Component({
  selector: 'app-upload-file',
  templateUrl: './upload-file.component.html',
  styleUrls: ['./upload-file.component.scss']
})
export class UploadFileComponent implements OnInit {

  @Input() isButton = false
  @Output() getFile: EventEmitter<any> = new EventEmitter<any>()

  filename: FormControl = new FormControl('')

  private _file: File = null
  get file(): File {
    return this._file
  }

  constructor() { }

  ngOnInit() {
    this.filename.valueChanges.pipe(debounceTime(800)).subscribe(next => {
      const blob = this._file.slice(0, this._file.size, this.file.type)
      this._file = new File([blob], next, { type: this._file.type })
    })
  }

  clearFiles() {
    this._file = null
  }

  uploadFile(event: FileList) {
    for (let index = 0; index < event.length; index++) {
      const element = event[index]
      this._file = element
      if (this.isButton) {
        this.getFile.emit(this._file)
      } else {
        this.filename.patchValue(element.name)
      }
      if (element) {
        const reader = new FileReader()
        reader.readAsDataURL(element)
      }
    }
  }

  deleteFile() {
    this._file = null
  }
}

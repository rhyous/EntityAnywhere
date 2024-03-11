import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core'
import { AbstractControl, FormControl, FormGroup } from '@angular/forms'
import { debounceTime } from 'rxjs/internal/operators/debounceTime'
import { NullEx } from 'src/app/core/infrastructure/extensions/null-ex'
import { environment } from 'src/environments/environment'
import { FieldConfig } from '../../models/interfaces/field-config.interface'

@Component({
  selector: 'app-entity-file',
  templateUrl: './entity-file.component.html',
  styleUrls: ['./entity-file.component.scss']
})
export class EntityFileComponent implements OnInit {

  filename: FormControl = new FormControl('')
  private _file: File = <any>null
  @Input() field!: FieldConfig
  @Input() group!: FormGroup

  ngOnInit() {
    this.filename.valueChanges.pipe(debounceTime(environment.debounceTimeInMs)).subscribe(next => {
      if (NullEx.isNullOrUndefined(this._file)) { return }
      const blob = this._file.slice(0, this._file.size, this._file.type)
      this._file = new File([blob], next, { type: this._file.type })
    })
  }

  @ViewChild('fileInput') fileInput?: ElementRef
  fileAttr = 'Choose File'
  uploadFileEvent(file: any) {
    if (file.target.files && file.target.files[0]) {
      this.fileAttr = ''
      Array.from(file.target.files).forEach((f: any) => {
        this.fileAttr += f.name
      })
      // HTML5 FileReader API
      const reader = new FileReader()
      reader.onload = (e: any) => {
        if (!NullEx.isNullOrUndefined(this.field)) {
          this.field.value = reader.result
        }
      }
      reader.readAsArrayBuffer(file.target.files[0])
      if (!NullEx.isNullOrUndefined(this.fileInput)) {
        this.fileInput.nativeElement.value = ''
      }
    } else {
      this.fileAttr = 'Choose File'
    }
  }
}
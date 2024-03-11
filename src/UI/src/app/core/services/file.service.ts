import { Injectable } from '@angular/core'
import { saveAs } from 'file-saver'
import { Observable } from 'rxjs'


@Injectable({
  providedIn: 'root'
})
/** Common Application service for dealing with files */
export class FileService {

  constructor() { }

  /** Push a Blob as a file to the user */
  saveFile(blob: Blob, filename: string) {
    const file = new File([blob], filename)
    saveAs(file)
  }

  base64ToBlob(blob: Blob, filename: string, contentType: string = '', sliceSize = 512) {
    this.readBlob(blob).subscribe(data => {
      const result = data
      const byteArrays = []

      for (let offset = 0; offset < result.length; offset += sliceSize) {
        const slice = result.slice(offset, offset + sliceSize)

        const byteNumbers = new Array(slice.length)
        for (let i = 0; i < slice.length; i++) {
          byteNumbers[i] = slice.charCodeAt(i)
        }

        const byteArray = new Uint8Array(byteNumbers)

        byteArrays.push(byteArray)
      }

      const newBlob = new Blob(byteArrays, { type: contentType })
      this.saveFile(newBlob, filename)
    })
  }

  readBlob(blob: Blob): Observable<string> {
    return new Observable((observer) => {

      const reader = new FileReader()
      reader.onload = (ev) => {
        const result = <string>reader.result
        observer.next(result)
        observer.complete()
      }
      reader.readAsText(blob)
    })
  }

  readBlobAsObject<T>(blob: Blob): Observable<T> {
    return new Observable((observer) => {
      this.readBlob(blob).subscribe(text => {
        const result = JSON.parse(text)
        observer.next(result as T)
        observer.complete()
      })
    })
  }
}

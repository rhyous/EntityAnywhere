import { Directive, Output, Input, EventEmitter, HostBinding, HostListener, ViewChild, ElementRef } from '@angular/core'

@Directive({
  selector: '[appDragDrop]'
})
export class DragDropDirective {

    private ivantiRed = '#DA291C'
    private grey = 'rgba(0, 0, 0, 0.38)'

    @Output() onFileDropped = new EventEmitter<any>()

    @HostBinding('style.border-color') public background = this.grey
    @HostBinding('style.opacity') public opacity = '1'
    @HostBinding('style.color') public colour = this.grey

  // Dragover listener
    @HostListener('dragover', ['$event']) onDragOver(evt) {
      evt.preventDefault()
      evt.stopPropagation()
      this.background = this.ivantiRed
      this.colour = this.ivantiRed
      this.opacity = '0.8'
    }

  // Dragleave listener
    @HostListener('dragleave', ['$event']) public onDragLeave(evt) {
      evt.preventDefault()
      evt.stopPropagation()
      this.background = this.grey
      this.colour = this.grey
      this.opacity = '1'
    }

  // Drop listener
    @HostListener('drop', ['$event']) public ondrop(evt) {
      this.background = this.grey
      this.colour = this.grey
      this.opacity = '1'
      const files = evt.dataTransfer.files
      if (files.length > 0) {
        this.onFileDropped.emit(files)
      }
      evt.preventDefault()
      evt.stopPropagation()

    }
}

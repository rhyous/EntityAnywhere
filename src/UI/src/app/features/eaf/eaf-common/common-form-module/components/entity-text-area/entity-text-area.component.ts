import { Component, Input, OnInit, ViewChild, NgZone } from '@angular/core'
import { FormGroup } from '@angular/forms'
import { FieldConfig } from '../../models/interfaces/field-config.interface'
import { CdkTextareaAutosize } from '@angular/cdk/text-field'
import { take } from 'rxjs/operators'

@Component({
  selector: 'app-entity-text-area',
  templateUrl: './entity-text-area.component.html',
  styleUrls: ['./entity-text-area.component.scss']
})
export class EntityTextAreaComponent implements OnInit {

  constructor(private ngZone: NgZone) {
   }

  @Input() field!: FieldConfig
  @Input() group!: FormGroup

  @ViewChild('autosize') autosize!: CdkTextareaAutosize

  triggerResize() {
    // Wait for changes to be applied, then trigger textarea resize.
    this.ngZone.onStable.pipe(take(1)).subscribe(() => this.autosize.resizeToFitContent(true))
  }

  ngOnInit(): void {
  }
}

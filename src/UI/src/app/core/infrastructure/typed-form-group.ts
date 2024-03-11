import { FormGroup } from '@angular/forms'

/** Extends FormGroup to strongly type value */
export class TypedFormGroup<TValue> extends FormGroup {
    override readonly value!: TValue
}

import { AbstractControl, ValidatorFn } from '@angular/forms'
import { StringEx } from './extensions/string-ex'

/** Represents custom validators */

/** Returns { requiredIfOtherControlIsNotEmpty: true }
 *  if the other control's value is not empty and this control is empty or null otherwise */
export function requiredIfOtherControlIsNotEmpty(otherControl: AbstractControl): ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
        if (otherControl && otherControl.value !== '' && control.value === '') {
            return { requiredIfOtherControlIsNotEmpty:  true }
        }

        return null
    }
}

export function eitherThisOrThat(otherControl: AbstractControl): ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
        if (StringEx.isUndefinedNullOrEmpty(control.value) && StringEx.isUndefinedNullOrEmpty(otherControl.value)) {
            return { eitherThisOrThat: true }
        }
        return null
    }
}

export function allAreValid(validators: Validator[]): ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
        let obj: any = null
        validators.forEach(x => {
            const result = x.validation(control)

            if (result) {
                if (obj === null) {
                    obj = {}
                }
                obj[x.property] = true
            }
        })
        return obj
    }
}

export interface Validator {
    validation: (control: AbstractControl) => boolean
    property: string
}


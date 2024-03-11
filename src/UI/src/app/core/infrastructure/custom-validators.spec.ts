import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing'
import { FormControl, Validators, FormGroup, FormBuilder } from '@angular/forms'
import { StringEx } from './extensions/string-ex'
import { requiredIfOtherControlIsNotEmpty, allAreValid } from './custom-validators'


describe('CustomValidators', () => {

    describe('requiredIfOtherControlIsNotEmpty', () => {


        it('should return false if the other control is not empty and this control is empty', () => {
            // Arrange
            const control1 = new FormControl('C')

            const form = new FormGroup({
                control1: control1,
                control2: new FormControl('', requiredIfOtherControlIsNotEmpty(control1))
            })

            // Act
            form.patchValue({control2: ''})

            // Assert
            expect(form.valid).toBeFalsy()
        })

        it('should return true if the other control is not empty and this control is not empty', () => {
            // Arrange
            const control1 = new FormControl('C')

            const form = new FormGroup({
                control1: control1,
                control2: new FormControl('', requiredIfOtherControlIsNotEmpty(control1))
            })

            // Act
            form.patchValue({control2: 'This is my value'})

            // Assert
            expect(form.valid).toBeTruthy()

        })

        it('should return true if the other control is empty', () => {
            // Arrange
            const control1 = new FormControl('')

            const form = new FormGroup({
                control1: control1,
                control2: new FormControl('', requiredIfOtherControlIsNotEmpty(control1))
            })

            // Act

            // Assert
            expect(form.valid).toBeTruthy()

        })

    })

    describe('allAreValid', () => {
        it('should be able to take an array of functions', () => {
            // Arrange
            const form = new FormGroup({
                control: new FormControl('', allAreValid([{
                    validation: (x) => false,
                    property: 'IsFalse'
                }, {
                    validation: (x) => true,
                    property: 'IsTrue'
                }
                ]))
            })

            // Act

            // Assert
            expect(form.valid).toBeFalsy()
            expect(form.controls['control'].errors).toEqual({ IsTrue: true })
        })

        it('should be able to work on more than one control', () => {
            // Arrange
            const fb = new FormBuilder()
            let form: FormGroup = new FormGroup({})

            form = fb.group({
                control1: new FormControl('This is my form'),
                control2: new FormControl('', allAreValid([{
                    validation: (x) => form.value.control1 !== 'This is my form',
                    property: 'MustBeSpecific'
                }
                ]))
            })

            // Act

            // Assert
            expect(form.valid).toBeFalsy()
            expect(form.controls['control2'].errors).toEqual({ MustBeSpecific: true })
        })

        it('should be able to work on more than one control and with more than one validator', () => {
            // Arrange
            const fb = new FormBuilder()
            let form: FormGroup = new FormGroup({})

            form = fb.group({
                control1: new FormControl(''),
                control2: new FormControl('', allAreValid([{
                    validation: (x) => form.value.control1 !== '',
                    property: 'NotUndefinedNullOrEmpty'
                },
                {
                    validation: (x) => form.value.control1 !== 'I am an evil genius',
                    property: 'MustBeAnEvilGenius'
                }
                ]))
            })

            // Act

            // Assert
            expect(form.valid).toBeFalsy()
            expect(form.controls['control2'].errors).toEqual({ NotUndefinedNullOrEmpty: true, MustBeAnEvilGenius: true })

            form.patchValue({control1: 'I am an evil genius'})

            form.controls['control2'].updateValueAndValidity()
            form.updateValueAndValidity()
/*
            expect(form.valid).toBeTruthy()
            expect(form.controls.control2.errors).toBeNull()
*/

        })
    })

})

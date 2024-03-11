import { NullEx } from './null-ex'

/** Static methods to help with dealing with number types. */
export class NumberEx {

    /** Checks if a number is positive.
     * @param val The value, which could a number, null, or undefined.
     * @returns Returns true if greater than zero, false if undefined, null, zero, or negative */
    static isPositive(val: number | null | undefined): boolean {
        return !NullEx.isNullOrUndefined(val) && val > 0
    }

    /** Checks if a number is zero or negative.
     * @param val The value, which could a number, null, or undefined.
     * @returns Returns true if zero or negative, false if undefined, null, or positive */
    static isZeroOrNegative(val: number | null | undefined): boolean {
        return !NullEx.isNullOrUndefined(val) && val < 1
    }

    /** Returns a number from a type that could be undefined, null, or a number.
     * @param val The value, which could a number, null, or undefined.
     * @param defaultNumber Optional. Default is 0.
     * @returns Returns the number in val unless undefined or null, then it returns the default number. */
    static getNumber(val: number | null | undefined, defaultNumber: number = 0): number {
        return NullEx.isNullOrUndefined(val) ? defaultNumber : val
    }
}

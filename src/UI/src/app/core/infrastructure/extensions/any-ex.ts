import { NullEx } from './null-ex'

/** Static methods to help with dealing with Any or Generic types. */
export class AnyEx {
    /** Returns an valid instance of T from an instance that could be undefined, null, or a T.
     * @param val The value, which could an instance of Type, null, or undefined.
     * @param defaultValue Optional. Default must be provided.
     * @returns Returns the instance of a type T assigned to val unless undefined or null, then it returns the defaultValue. */
    static getValue<T>(val: T | null | undefined, defaultValue: T): T {
        return NullEx.isNullOrUndefined(val) ? defaultValue : val
    }
}

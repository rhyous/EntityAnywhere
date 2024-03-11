import { NullEx } from './null-ex'

/** Static methods to help with dealing with string types. */
export class StringEx {
    /** Checks if a string is undefined, null, or empty. Does not check for whitespace.
     * @param string | null | undefined The string to check.
     * @returns True if the string is undefined, null, or empty, false otherwise. */
    static isUndefinedNullOrEmpty(val: string | null | undefined): val is null | undefined {
        return NullEx.isNullOrUndefined(val) || val === ''
    }

    /** Checks if a string is undefined, null, empty, or whitespace.
     * @param string | null | undefined The string to check.
     * @returns True if the string is undefined, null, empty, or whitespace, false otherwise. */
    static isUndefinedNullOrWhitespace(val: string | null | undefined): val is null | undefined {
        return NullEx.isNullOrUndefined(val) || val === '' || val.trim() === ''
    }

    /** Replaces all occurances of the findString found in the originalString.
     *  This solution is adapted from JavaScript function: String.prototype.replaceAll()
     * @param string The original string.
     * @param string The string to find and replace.
     * @param string The replacement value. */
    static replaceAll(originalString: string, findString: string, replaceWithString: string) {
        return originalString.split(findString).join(replaceWithString)
    }
}

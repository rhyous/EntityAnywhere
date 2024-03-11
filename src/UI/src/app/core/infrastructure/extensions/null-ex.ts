/** A static tool to check if types are undefined or null. */
export class NullEx {
    /** Checks if the parameter is undefined or null.
     * @param val The parameter to check.
     * @returns True if undefined or null, false otherwise. */
    static isNullOrUndefined(val: any | null | undefined): val is null | undefined {
        return val === undefined || val === null
    }
}

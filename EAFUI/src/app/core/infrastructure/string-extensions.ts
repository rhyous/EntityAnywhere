export class StringExtensions {
    static isUndefinedNullOrEmpty(val: string) {
        if (val === undefined || val === null) {
            return true
        }
        return val === ''
    }

    // This solution is adapted from JavaScript function: String.prototype.replaceAll()
    static replaceAll(originalString: string, findString: string, replaceWithString: string) {
        return originalString.split(findString).join(replaceWithString)
    }
}

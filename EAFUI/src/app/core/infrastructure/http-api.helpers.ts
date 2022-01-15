/** Helper class for extracting out functionality for the AdminDataService that is not to do with HTTP */
export class HttpApiHelpers {

    /** Converts JSON objects or strings into a byte array */
    static jsonToByteArray(json: Object | string): number[] {
        const jsonString = json instanceof Object ? JSON.stringify(json) : json
        const utf8 = unescape(encodeURIComponent(jsonString))
        const byteArray: number[] = []
        for (let i = 0; i < jsonString.length; i++) {
            byteArray.push(utf8.charCodeAt(i))
        }
        return byteArray
    }

    /** Gets an object with the token and sets the response type to a blob so an octet-stream can be returned */
    static getOctetStreamHeader(token: string) {
        const obj = {headers: {Token : token}}
        obj['responseType'] = 'blob'
        return obj
    }
}

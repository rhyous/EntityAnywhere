import { ApiRequestUrlHelpers } from './api-request-url-helpers'

describe('ApiRequestUrlHelpers', () => {
    it('should cleanse one single quote in filter query and return two single quotes in query string', () => {
        const originalString = `test'SingleQuote`
        const expectedCleansedString =  '\'' + encodeURIComponent( `test''SingleQuote`) + '\''

        const returnedCleansedString = ApiRequestUrlHelpers.cleanseFilterQueryStringForApiRequest(originalString)

        expect(returnedCleansedString).toEqual(expectedCleansedString)
    })

    it('should cleanse one double quote in filter query and return two double quotes in query string', () => {
        const originalString = `test"DoubleQuote`
        const expectedCleansedString =  '\'' + encodeURIComponent(`test""DoubleQuote`) + '\''

        const returnedCleansedString = ApiRequestUrlHelpers.cleanseFilterQueryStringForApiRequest(originalString)

        expect(returnedCleansedString).toEqual(expectedCleansedString)
    })

    it('should cleanse two single quotes in filter query and return four single quotes in query string', () => {
        const originalString = `test''SingleQuote`
        const expectedCleansedString =  '\'' + encodeURIComponent(`test''''SingleQuote`) + '\''

        const returnedCleansedString = ApiRequestUrlHelpers.cleanseFilterQueryStringForApiRequest(originalString)

        expect(returnedCleansedString).toEqual(expectedCleansedString)
    })

    it('should cleanse two double quote in filter query and return four double quotes in query string', () => {
        const originalString = `test""DoubleQuote`
        const expectedCleansedString =  '\'' + encodeURIComponent(`test""""DoubleQuote`) + '\''

        const returnedCleansedString = ApiRequestUrlHelpers.cleanseFilterQueryStringForApiRequest(originalString)

        expect(returnedCleansedString).toEqual(expectedCleansedString)
    })

    it('should cleanse one single and one double quote in filter query and return two single and two double quotes in query string', () => {
        const originalString = `test'Single"DoubleQuote`
        const expectedCleansedString =  '\'' + encodeURIComponent(`test''Single""DoubleQuote`) + '\''

        const returnedCleansedString = ApiRequestUrlHelpers.cleanseFilterQueryStringForApiRequest(originalString)

        expect(returnedCleansedString).toEqual(expectedCleansedString)
    })

})


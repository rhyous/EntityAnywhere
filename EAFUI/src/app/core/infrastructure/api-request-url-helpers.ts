import { StringExtensions } from './string-extensions'

export class ApiRequestUrlHelpers {
  static cleanseFilterQueryStringForApiRequest(val: string) {
    return val ? '\'' + encodeURIComponent(ApiRequestUrlHelpers.escapeQuotesInFilterQueryString(val)) + '\'' : ''
  }

  private static escapeQuotesInFilterQueryString(val: string) {
    const singleQuoteEscapedString = StringExtensions.replaceAll(val, '\'', '\'\'')
    const singleAndDoubleQuoteEscapedString = StringExtensions.replaceAll(singleQuoteEscapedString, '"', '""')
    return singleAndDoubleQuoteEscapedString
  }
}


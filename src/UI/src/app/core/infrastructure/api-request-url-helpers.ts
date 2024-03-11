import { StringEx } from './extensions/string-ex'

export class ApiRequestUrlHelpers {
  static cleanseFilterQueryStringForApiRequest(val: string) {
    return val ? '\'' + encodeURIComponent(ApiRequestUrlHelpers.escapeQuotesInFilterQueryString(val)) + '\'' : ''
  }

  private static escapeQuotesInFilterQueryString(val: string) {
    const singleQuoteEscapedString = StringEx.replaceAll(val, '\'', '\'\'')
    const singleAndDoubleQuoteEscapedString = StringEx.replaceAll(singleQuoteEscapedString, '"', '""')
    return singleAndDoubleQuoteEscapedString
  }
}


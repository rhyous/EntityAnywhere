import { LandingPageTypes } from '../enums/LandingPageTypes'

export class UserLandingPages {

  static landingPages = new Map([
    [LandingPageTypes.Default as Number, { name: 'default', url: '/default' }],
    [LandingPageTypes.Administration as Number, { name: 'Administration', url: '/admin' }],
    [LandingPageTypes.Author as Number, { name: 'Author', url: '/author' }],
    [LandingPageTypes.Customer as Number, { name: 'Customer', url: '/customer' }],
    [LandingPageTypes.Publisher as Number, { name: 'Publisher', url: '/publisher' }]
  ])

  /** Checks if the parameter is undefined or null.
   * @param val The parameter to check.
   * @returns True if undefined or null, false otherwise. */
  static isNullOrUndefined(val: any | null | undefined): val is null | undefined {
      return val === undefined || val === null
  }
}

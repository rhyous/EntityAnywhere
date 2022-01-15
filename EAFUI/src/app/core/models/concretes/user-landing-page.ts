import { LandingPageTypes } from '../enums/LandingPageTypes'

export class UserLandingPages {

 public static landingPages = new Map([
       [ LandingPageTypes.Administration as Number , {name: 'Administration', url: '/admin'}],
       [ LandingPageTypes.Default as Number  , {name: 'default', url: '/default'}]
    ])
  }

import { LandingPageTypes } from '../enums/LandingPageTypes'

export class CustomPluralizationMap {

  public Plural = new Map([
    ['Addendum', 'Addenda'],
    ['Series', 'Series']
  ])
  public Singular = new Map([
    ['Addenda', 'Addendum'],
    ['Series', 'Series']
  ])
}

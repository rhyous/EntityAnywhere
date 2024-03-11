import {NgModule} from '@angular/core'
import { BREAKPOINT } from '@angular/flex-layout'

/**
 * For mobile and tablet, reset ranges
 */
const MOBILE_BREAKPOINTS = [{
  alias: 'lt-lg',
  suffix: 'LtLg',
  mediaQuery: '(max-width: 1300px)'
}]

export const CustomBreakPointProvider = {
  provide: BREAKPOINT,
  useValue: MOBILE_BREAKPOINTS,
  multi: true
}

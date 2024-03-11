import { AngularComponentBaseModel } from './common/angular-component-base.model'

export class SidebarMenuComponentModel extends AngularComponentBaseModel {
  get logoutLink(): Selector {return this.component.find('#logout-link')}
  get adminLink(): Selector { return this.component.find('#admin-link')}
  get username(): Selector { return this.component.find('#username')}
  get allEntitlements() { return this.component.find('#all-entitlements')}
  get customerMenu(): Selector { return this.component.find('#customer-menu')}
  get refresh() { return this.component.find('#refresh')}

  constructor() {
    super('app-sidebar-menu')
  }

}

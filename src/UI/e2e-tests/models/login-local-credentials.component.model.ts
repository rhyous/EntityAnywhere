import { t } from 'testcafe'
import { AngularComponentBaseModel } from './common/angular-component-base.model'
import { CookieBannerComponentModel } from './cookie-banner.component.model'

export class LoginLocalCredentialsComponentModel extends AngularComponentBaseModel {
  get username(): Selector { return this.component.find('#username') }
  get password(): Selector { return this.component.find('#password') }
  get signInBtn(): Selector { return this.component.find('#sign-in') }
  get cookieBannerComponentPageModel(): CookieBannerComponentModel { return new CookieBannerComponentModel() }

  constructor() {
    super('app-local-credentials')
  }

  async login(username: string, password: string) {
    if (await this.cookieBannerComponentPageModel.okBtn.exists) {
      await t.click(this.cookieBannerComponentPageModel.okBtn)
    }

    await t.typeText(this.username, username)
            .typeText(this.password, password)
            .click(this.signInBtn)
  }
}

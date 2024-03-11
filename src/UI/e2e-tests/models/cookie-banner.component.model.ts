import { Selector, t } from 'testcafe'
import { AngularComponentBaseModel } from './common/angular-component-base.model'

export class CookieBannerComponentModel extends AngularComponentBaseModel {
  get okBtn() { return this.component.find('#ok') }
  get privacyPolicyLink() { return this.component.find('#privacy-policy-link') }

  constructor() {
    super('app-cookie-banner')
  }

  async confirmPolicyLink() {
    const attrs = await this.privacyPolicyLink.attributes
    await t.expect(attrs['href']).eql('<link-to-policy-here>')
    await t.expect(attrs['target']).eql('_blank')
    await t.click(this.privacyPolicyLink)
    await t.expect(Selector('h1').withExactText('<Your Company> Privacy Policy').exists).ok()
  }

  async confirm() {
    await t.click(this.okBtn)
    await t.expect(this.okBtn.exists).notOk()
  }
}

import { LoginCredentials } from '../data/login-credentials.data'
import { PageUrls } from '../data/page-urls.data'
import { TestGroupNames } from '../data/test-group-names.data'
import { FixtureMetaData } from '../models/common/fixture-metadata.model'
import { TESTFILTER_PROD_METADATA } from '../models/common/test-filter-metadata.model'
import { LoginLocalCredentialsComponentModel } from '../models/login-local-credentials.component.model'
import { SidebarMenuComponentModel } from '../models/sidebar-menu.component.model'

let loginComponent: LoginLocalCredentialsComponentModel
let sidebarMenuComponent: SidebarMenuComponentModel

const fixtureMetaData: FixtureMetaData = {testGroup: TestGroupNames.Login}

fixture(`Login-LocalCredentials-Component`)
  .meta(fixtureMetaData)
  .page(`${PageUrls.LoginLocalCredentialsPage}`)
  .beforeEach(async () => {
    loginComponent = new LoginLocalCredentialsComponentModel()
    sidebarMenuComponent = new SidebarMenuComponentModel()
  })
  .afterEach(async t => {
    await t.click(sidebarMenuComponent.logoutLink)
  })

test
  .meta(TESTFILTER_PROD_METADATA)
  ('should be able to login as admin and see admin sidebar', async t => {
    await loginComponent.login(LoginCredentials.AdminUsername,  LoginCredentials.AdminUserPassword)

    await t.expect(sidebarMenuComponent.username.innerText).eql('Internal')
    await t.expect(sidebarMenuComponent.logoutLink.exists).ok()
    await t.expect(sidebarMenuComponent.adminLink.exists).ok()

    await t.expect(sidebarMenuComponent.allEntitlements.exists).notOk()
    await t.expect(sidebarMenuComponent.refresh.exists).notOk()
  })

test
  .meta(TESTFILTER_PROD_METADATA)
  ('should be able to login as customer and see customer sidebar', async t => {
    await loginComponent.login(LoginCredentials.CustomerWarehouseOneUsername,  LoginCredentials.CustomerWarehouseOneUserPassword)

    await t.expect(sidebarMenuComponent.username.innerText).eql('Warehouse One')
    await t.expect(sidebarMenuComponent.logoutLink.exists).ok()
    await t.expect(sidebarMenuComponent.customerMenu.exists).ok()
    await t.expect(sidebarMenuComponent.allEntitlements.exists).ok()
    await t.expect(sidebarMenuComponent.refresh.exists).ok()

    await t.expect(sidebarMenuComponent.adminLink.exists).notOk()
  })

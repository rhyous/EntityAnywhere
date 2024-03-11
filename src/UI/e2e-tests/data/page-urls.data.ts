import { userVariables } from 'testcafe'

export class PageUrls {
    static BaseUrl = <string>userVariables['e2eTest-PortalBaseUrl']
    static HomePage = `${PageUrls.BaseUrl}`
}

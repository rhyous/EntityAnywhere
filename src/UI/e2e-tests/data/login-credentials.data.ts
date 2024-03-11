import 'dotenv/config'
import { userVariables } from 'testcafe'

export class LoginCredentials {
    static AdminUsername = 'admin'
    static AdminUserPassword = <string>userVariables['AdminUserPassword'] !== '' ?
                                <string>userVariables['AdminUserPassword'] :
                                <string>process.env['ADMIN_USER_PASSWORD']

    static CustomerWarehouseOneUsername = 'warehouseone'
    static CustomerWarehouseOneUserPassword = <string>userVariables['CustomerWarehouseoneUserPassword'] !== '' ?
                                <string>userVariables['CustomerWarehouseoneUserPassword'] :
                                <string>process.env['CUSTOMER_WAREHOUSEONE_USER_PASSWORD']
}

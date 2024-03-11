import { LandingPageTypes } from '../../models/enums/LandingPageTypes'
import { UserLoginData } from '../../models/concretes/user-login-data'
import { Claim } from '../../login/login-token.interface'

export class FakeAppLocalStorageService {
    get User(): UserLoginData {
        const user = new UserLoginData()

        const userIdClaim = new Claim()
        userIdClaim.Value = '1'
        user.UserId = userIdClaim

        const userNameClaim = new Claim()
        userNameClaim.Value = 'Username'
        user.Username = userNameClaim

        const adminRoleClaim = new Claim()
        adminRoleClaim.Name = 'Role'
        adminRoleClaim.Subject = 'UserRole'
        adminRoleClaim.Value = 'Admin'
        adminRoleClaim.ValueType = 'null'
        user.Roles = [ adminRoleClaim ]

        const landingPageClaim = new Claim()
        landingPageClaim.Value = LandingPageTypes.Default.toString()
        user.LandingPageType = landingPageClaim

        return user
    }

    get activeToken(): string {
        return 'THISISMYTOKENTHEREAREMANYTOKENSLIKEITBUTTHISONEISMINE'
    }

    set activeToken(value: string) {
        this.activeToken = value
    }

    userRoleData = `{
        "EntitledProduct": {
            "Entity": "EntitledProduct",
            "Permissions": [
                "Admin"
            ]
        },
        "EntitledProductUsage": {
            "Entity": "EntitledProductUsage",
            "Permissions": [
                "Admin"
            ]
        }
    }`

    get authorizedUserRoleData() {
        return JSON.parse(this.userRoleData)
    }

    set authorizedUserRoleData(value: string) {
        this.userRoleData = value
    }
}

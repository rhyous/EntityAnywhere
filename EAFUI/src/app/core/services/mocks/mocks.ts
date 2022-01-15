import { TypedEventEmitter } from '../../infrastructure/typed-event-emitter.interface'
import { EventEmitter } from '@angular/core'
import { LandingPageTypes } from '../../models/enums/LandingPageTypes'

export class FakeAppLocalStorageService {
    User = {
      UserId: {
          Value: 1,
      },
      Username: {
          Value: 'Username'
      },
      UserRole: {
          Value: 'Admin'
      },
      AdminRole: {
          Value: ''
      },
      DefaultRole: {
          Value: ''
      },
      PortalLandingPageType: {
          Value: LandingPageTypes.Default.toString()
        }

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
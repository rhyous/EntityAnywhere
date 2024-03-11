import { NullEx } from '../../infrastructure/extensions/null-ex'
import { Claim } from '../../login/login-token.interface'

export class UserLoginData {
  Username?: Claim
  UserId?: Claim
  LastAuthenticated?: Claim
  Roles?: Claim[]
  Impersonation?: Claim
  LandingPageType?: Claim

  getRole(roleName: string): Claim | null {
    if (NullEx.isNullOrUndefined(this.Roles)) {
      return null
    }
    return this.Roles.firstOrDefault(r => r.Value === roleName)
  }

  hasRole(roleName: string): boolean {
    return !NullEx.isNullOrUndefined(this.getRole(roleName))
  }
}

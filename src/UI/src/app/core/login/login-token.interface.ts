/**
 * Represents the response from the server upon logging in
 */
export interface LoginToken {
  Id: number
  /** The Token */
  Text: string
  UserId: number

  ClaimDomains: ClaimDomains[]

  CreateDate: Date
  CreatedBy: number
  LastUpdated: Date
  LastUpdatedBy: number
}

export class ClaimDomains {
  Claims!: Claim[]
}

export class Claim {
  Name!: string
  Issuer!: string
  Subject!: string
  Value!: string
  ValueType!: string
}

export class UserClaim {
  Key!: string
  Claims!: Claim[]
  Token!: string
}

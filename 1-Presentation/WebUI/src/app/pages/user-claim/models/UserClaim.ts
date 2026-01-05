import { ClaimTypeValues } from "./ClaimTypeValues"

export interface UserClaim {
  claimType: ClaimTypeValues
  value: string
  isFixed: boolean
}

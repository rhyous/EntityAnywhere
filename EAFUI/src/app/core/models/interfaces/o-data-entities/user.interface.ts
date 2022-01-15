import { AuditableEntity } from './common/auditable-entity.interface'

export interface User extends AuditableEntity {
    Id: number
    Username: string
    Enabled: boolean
    ExternalAuth: boolean
    IsHashed: boolean
    OrganizationId: number
    Password: string
    Salt: string
}

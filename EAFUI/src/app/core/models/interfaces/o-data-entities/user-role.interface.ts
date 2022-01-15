import { AuditableEntity } from './common/auditable-entity.interface'

export interface UserRole extends AuditableEntity {
    Name: string
    Description: string
    enabled: boolean
}

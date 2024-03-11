import { AuditableEntity } from './common/auditable-entity.interface'

export interface AlternateId extends AuditableEntity {
    Id: number
    Entity: string
    EntityId: any
    Property: string
    Value: string
}

import { AuditableEntity } from './common/auditable-entity.interface'

export interface Addenda extends AuditableEntity {
    Id: number
    Entity: string
    EntityId: string
    Property: string
    Value: string
}

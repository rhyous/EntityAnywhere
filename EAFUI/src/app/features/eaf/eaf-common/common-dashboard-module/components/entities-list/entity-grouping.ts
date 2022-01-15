/** Represents the grouping together of an Entity and the Group the Entity belongs to */
export class EntityGrouping {
    /** The name of this entity */
    entityName: string

    /** The group that this entity belongs to */
    entityGroup: string

    constructor(entityName: string, entityGroup: string) {
        this.entityGroup = entityGroup
        this.entityName = entityName
    }
}

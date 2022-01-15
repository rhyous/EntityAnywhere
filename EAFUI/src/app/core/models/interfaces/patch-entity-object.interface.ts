/** Represents the data needed for a patch to be performed */
export interface PatchedEntity {
    ChangedProperties: string[]
    Entity: {[key: string]: any}
}

export interface PatchedEntityCollection {
    ChangedProperties: string[]
    PatchedEntities: PatchedEntity[]
}

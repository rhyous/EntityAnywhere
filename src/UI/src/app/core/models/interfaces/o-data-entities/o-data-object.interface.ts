import { ODataCollection } from "../o-data-collection.interface"

/** Represents Related Entity Data */
export interface RelatedEntityData {
    /** The amount of entities returned through this query */
    Count: number

    /** The name of the related entity. Also the Entity's type */
    RelatedEntity: string

    /** The related entities */
    RelatedEntities?: ODataObject<any>[]
}

    /** Convert to ODataCollection */
export function AsOdataCollection<T>(relatedEntities: RelatedEntityData): ODataCollection<T> {
        return {
            'Count': relatedEntities.Count,
            'Entities': relatedEntities.RelatedEntities ?? [],
            'Entity': relatedEntities.RelatedEntity,
            'TotalCount': relatedEntities.Count,
        }
    }

/** Represents the data returned by the API */
export interface ODataObject<T> {
    /** The URI to get this object */
    Uri?: string

    /** The Primary identifier for this object */
    Id: any,

    /** The Object itself */
    Object: T,

    /** Related Entities for this object */
    RelatedEntityCollection?: RelatedEntityData[]
}

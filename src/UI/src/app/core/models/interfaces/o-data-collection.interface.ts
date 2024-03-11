import { ODataObject } from './o-data-entities/o-data-object.interface'

/** Represents a set of data returned from requesting a list of entities */
export interface ODataCollection<T> {
    /**The amount of entities in this set of data */
    Count: number,

    /** The entities */
    Entities: ODataObject<T>[],

    /** The name of this entity */
    Entity: string,

    /** The total count of entities */
    TotalCount: number
}

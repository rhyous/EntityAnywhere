import { ODataObject } from '../models/interfaces/o-data-entities/o-data-object.interface'
import { Addenda } from '../models/interfaces/o-data-entities/addenda.interface'

/**
 * Singleton helper class to make things easier for OData
 */
export class ODataObjectHelpers {

    private constructor() {}

    private static _instance: ODataObjectHelpers
    static get Instance(): ODataObjectHelpers {
        if (!this._instance) {
            this._instance = new ODataObjectHelpers()
        }
        return this._instance
    }

    /** Gets the related entities as specified by the RelatedEntityName */
    getRelatedEntityFromODataObject<TOut>(oDataObject: ODataObject<any>, relatedEntityName: string): ODataObject<TOut>[] {
      if (oDataObject && oDataObject.RelatedEntityCollection) {
       const relatedEntityCollection =  oDataObject.RelatedEntityCollection.firstOrDefault(x => x.RelatedEntity === relatedEntityName)
       if (relatedEntityCollection && relatedEntityCollection.RelatedEntities) {
          return relatedEntityCollection.RelatedEntities
        }
      }
      return []
    }
}

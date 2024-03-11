import { EntityField } from 'src/app/features/eaf/eaf-common/common-form-module/models/concretes/entity-field'
import { NullEx } from '../../infrastructure/extensions/null-ex'

export class EntityMetadata {
  /** The Entity name */
  Name!: string

  /** The Entity display name. */
  UIDisplayName!: string

  /** The Kind, which is almost always EntityType.*/
  Kind!: string

  /** The key fields in an array. Usually there is one key, Id, but an alternate key can be added. */
  Key: string[] = []

  /** The fields in this entity. */
  Fields: EntityField[] = []

  /* If type of field not a primitive type, it is a RelatedEntity, what is the type of the RelatedEntity? Entity, Lookup, Mapping. */
  EntityType!: string

  /* If type of field not a primitive type, it is a RelatedEntity, what is the type of the RelatedEntity? Entity, Lookup, Mapping. */
  FileUpload!: boolean

  /** Is this field a mapping entity type. This is a RelatedEntity foreign field where the foreign fields
   *  is a mapping field to a mapped entity.
   * @returns {boolean} */
  get isMappingEntityType(): boolean {
    return this.isOfEntityType('Mapping')
  }

  /** Is this entity the the provided type.
   * EntityType options are:
   * - Undefined or null - This Field is likely a primitive without an entity type
   * - Entity - A field representing an Entity.
   * - Lookup - An entity that is a lookup entity
   * - Mapping - An entity that maps two entities together.
   * Returns true if it the typ matches, false otherwise. */
  isOfEntityType(entityType: string): boolean {
    return (!NullEx.isNullOrUndefined(this.EntityType)
      && this.EntityType === entityType)
  }

  /** Returns the list of fields that are searchable.
   * @returns EntityField[] */
  getSearchFields(): EntityField[] {
    return this.Fields.filter(x => x.Searchable)
  }

  /** Gets a field that matches the input fieldName.
   * @param fieldname string the name of the field.
   * @returns EntityField The found field.
   * @throws Error If the field is not found. */
  getField(fieldName: string): EntityField {
    const field = this.Fields.find(x => x.Name === fieldName)
    if (field === undefined) {
      throw new Error(`${fieldName} does not exist in the ${this.Name} entity metadata`)
    }
    return field
  }

  /** Checks if this Entity has a field with the given fieldName.
   * @param string The field name.
   * @returns boolean True if this Entity has a field with the given fieldName, false otherwise. */
  hasField(fieldName: string): boolean {
    const field = this.Fields.find(x => x.Name === fieldName)
    if (field === undefined) {
      return false
    }
    return true
  }
}

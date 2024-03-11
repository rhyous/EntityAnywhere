import { EntityMetadata } from 'src/app/core/models/concretes/entity-metadata'
import { Validator } from 'src/app/core/models/interfaces/validator.interface'

/**
 * Represents the Metadata that needs to be be passed in order to initialise this component
 */
export interface FieldConfig {
  /** A label to be displayed to the user */
  label?: string
  name?: string
  inputType?: string
  options?: any
  collections?: any
  /** The type of metadata */
  type: string
  /** The current value */
  value?: any
  validations?: Validator[]
  entity?: string,
  searchEntity?: string,
  searchEntityDisplayName?: string,
  /** The search entity default value */
  searchEntityDefault?: { Name: string, Value: number }
  order?: number,
  required?: boolean,
  /** The amount to flex */
  flex?: number,
  filter?: string,
  /** The base entity to map from */
  baseEntity?: string,
  /** The entity that will map between the objects */
  mapEntity?: string,
  /** The target entity to map to */
  targetEntity?: string,
  /** An alias - usually the RelatedEntityAlias */
  alias?: string

  /** Is this field readonly? */
  disabled?: boolean

  /** An alias for the current entity that this Field is a member of */
  entityAlias?: string

  /** Is this field readonly? */
  readOnly?: boolean
  /** If this is a byte array for a file, what is the file type? */
  fileType?: string
  /** If this is a byte array for a file, what are the allowed file extensions? */
  allowedFileExtensions?: string[]
  /** Is Auditable field */
  isAuditable?: boolean
}

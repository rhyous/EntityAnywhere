import { Validator } from 'src/app/core/models/interfaces/validator.interface'

/**
 * Represents the Metadata that needs to be be passed in order to initialise this component
 */
export interface FieldConfig {
  /** A label to be displayed to the user */
  label?: string
  name?: string
  inputType?: string
  options?: string[]
  collections?: any
  /** The type of metadata */
  type: string
  /** The primary key identifier of the base entity */
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
  /** An alias */
  alias?: string

  disabled?: boolean

  /** An alias for the entity */
  entityAlias?: string
  readOnly?: boolean
}

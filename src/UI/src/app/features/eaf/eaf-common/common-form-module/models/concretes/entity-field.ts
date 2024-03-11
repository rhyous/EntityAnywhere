import { NullEx } from 'src/app/core/infrastructure/extensions/null-ex'
import { StringEx } from 'src/app/core/infrastructure/extensions/string-ex'
import { ReferentialConstraint } from 'src/app/core/models/concretes/referential-constraint'

export class EntityField {
  /** The name of this field. */
  Name!: string

  /** The edm type of this field. */
  Type!: string

  /** If this type is a string, what type of string is it? SingleLine, MultiLine, Href, etc... */
  StringType!: string

  /** If the type is edm.string, there might be a max length. */
  MaxLength!: number

  /** Whether this field is read only or not. */
  ReadOnly!: boolean

  /** Whether this field is nullab only or not. */
  Nullable!: boolean

  /** If this is a RelatedEntity Id field, it will have a NavigationKey with the value being the name of the RelatedEntity field. */
  NavigationKey!: string

  /** If this is a RelatedEntity field, it will have a ReferentialConstraint, which has the local RelatedEntity Id field and the foreign id field. */
  ReferentialConstraint!: ReferentialConstraint

  /** If this is a RelatedEntity field, it will have a RelatedEntity type of: Local (RelatedEntity), Foreign, Mapping, or Extension. */
  RelatedEntityType!: string

  /** If this is a RelatedEntity foreign or mapping field, it will be a collection and this will be true, otherwise this will be false. */
  Collection!: boolean

  /** If this is a primitive field and this field is configured to be searchable, this will be true.
   *  When this is true, this field will be available in the Entity Searcher. */
  Searchable!: boolean

  /** The order to display this field. */
  DisplayOrder!: number

  /** If this field is an a RelatedEntity field, this might be set. It is the alias of the Related Entity (not the alias of the field's parent). */
  Alias!: string

  /** If this field is an enum, Kind will be 'EnumType'
   * If this field is an a RelatedEntity field, Kind will be 'NavigationProperty'. */
  Kind!: string

  /** If this field is an enum, then this will be a map of the enum options. */
  Options!: Map<number, string>

  /** If this field is an a RelatedEntity field, there might be a filter to determine if the parent entity shows this
   * RelatedEntity field or not in associated records.
   * Question: How is this different than DisplayCondition? */
  Filter!: string

  /** If this field is an a RelatedEntityMapping field (there is a mapping entity between this field the parent), there
   * then this must be the value of the mapping entity. */
  MappingEntity!: string

  /** If this field is an a RelatedEntityMapping field (there is a mapping entity between this field the parent), there
   * then this might exist as an alias name of the mapping entity. */
  MappingEntityAlias!: string

  /** If this field is an a RelatedEntity field, this might be set. It is the alias of the fields parent entity (not the related entity). */
  EntityAlias!: string

  /** If this field is a RelatedEntity foreign, this field must be set to the foreign entity's field.
  * If this field is a RelatedEntity mapping, this field must be set to the mapping entity's field. */
  MappedId!: string

  /** Whether this field is required on create or edit forms. */
  Required!: boolean

  /** If this field is an a RelatedEntity field, there might be a filter to determine if the parent entity shows this
   * RelatedEntity field or not in associated records.
   * Question: How is this different than Filter? */
  DisplayCondition!: string

  /** Whether this field is visible in the Associated Records screen's list view for this RelatedEntity. */
  VisibleOnRelations!: boolean

  /** A default value for this field. */
  Default?: { Name: string, Value: number }

  /** If this is a byte array or file, what type of file? */
  FileType!: string

  /** If this is a byte array or file, what type of file? */
  AllowedFileExtensions!: string[]

  /** Checks if this field is an Enum
   * @returns {boolean} */
  isEnum(): boolean {
    return this.Kind === 'EnumType'
  }

  /** Checks if this field is a File
   * @returns {boolean} */
  isFile(): boolean {
    return !StringEx.isUndefinedNullOrWhitespace(this.FileType)
  }

  /** Checks if this field has a NagivationKey
   * @returns {boolean} */
  hasNavigationKey(): boolean {
    return !StringEx.isUndefinedNullOrWhitespace(this.NavigationKey)
  }

  /** Checks if this field is a NavigationProperty
   * @returns {boolean} */
  isNavigationProperty(): boolean {
    return this.Kind === 'NavigationProperty'
  }

  /** Checks if this field is a RelatedEntityMapping field (not the mapping field in between)
   * @returns {boolean} */
  isMapping(): boolean {
    return (!NullEx.isNullOrUndefined(this.RelatedEntityType) && this.RelatedEntityType === 'Mapping')
  }

  /** Checks if this field is a RelatedEntity extension
   * @returns {boolean} */
  get isExtension(): boolean {
    return (!NullEx.isNullOrUndefined(this.RelatedEntityType) && this.RelatedEntityType === 'Extension')
  }

  /** Is this field a RelatedEntity foreign field.
   * @returns {boolean} */
  get isForeign(): boolean {
    return (!NullEx.isNullOrUndefined(this.RelatedEntityType)
      && this.RelatedEntityType === 'Foreign')
  }

  /** Is this field a single line string or text with a max length
   * @returns {boolean} */
  isString(): boolean {
    return !NullEx.isNullOrUndefined(this.Type)
      && this.Type === 'Edm.String'
  }

  /** Is this field a one of the auditables
   * @returns {boolean} */
  isAuditable(auditableProperties: string[]): boolean {
    return !(auditableProperties.indexOf(this.Name) === -1)
  }

  /** Is this field a ???
   * @returns {boolean} */
  isNonContains(): boolean {
    return (!this.Type || this.isNumeric())
  }

  /** Is this field a number type
   * @returns {boolean} */
  isNumeric(): boolean {
    const numericHashSet = new Set<string>(['Edm.Int64', 'Edm.Int32', 'Edm.Int16', 'Edm.Byte', 'Edm.SByte', 'Edm.Double', 'Edm.Decimal'])
    return numericHashSet.has(this.Type)
  }

  /** Is this field a Date type
   * @returns {boolean} */
  isDate(): boolean {
    return (this.Type === 'Edm.Date' || this.Type === 'Edm.DateTimeOffset')
  }

  /** Gets the label for a field
   * @returns {string} The Navigation Key if it exists, otherwise, just the name. */
  getLabel(): string {
    return this.hasNavigationKey() ? this.NavigationKey : this.Name
  }
}

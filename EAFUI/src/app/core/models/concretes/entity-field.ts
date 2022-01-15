import { ReferentialConstraint } from "./referential-constraint"

export class EntityField {
    Name: string
    Type: string
    ReadOnly: boolean
    Nullable: boolean
    NavigationKey: string
    ReferentialConstraint: ReferentialConstraint
    RelatedEntityType: string
    Collection: boolean
    Searchable: boolean
    DisplayOrder: number
    Alias: string
    Kind: string
    Options: Map<number, string>
    Filter: string
    MappingEntity: string
    MappingEntityAlias: string
    EntityAlias: string
    MappedId: string
    Required: boolean
    DisplayCondition: string
    VisibleOnRelations: boolean
    Default?: { Name: string, Value: number }

    auditableProperties = ['CreateDate', 'CreatedBy', 'LastUpdated', 'LastUpdatedBy']

    isEnum(): boolean {
      if (this.Kind === 'EnumType') {
        return true
      }
      return false
    }

    hasNavigationKey(): boolean {
      if (this.NavigationKey) {
        return true
      }
      return false
    }

    isNavigationProperty(): boolean {
      return this.Kind === 'NavigationProperty'
    }

    /** Is it a Many to Many relationship? */
    isMapping(): boolean {
      return (this.RelatedEntityType && this.RelatedEntityType === 'Mapping')
    }

    /** Does it have an extension entity attached to it? */
    get isExtension(): boolean {
      return (this.RelatedEntityType && this.RelatedEntityType === 'Extension')
    }

    /** Is it a one to many relationship? */
    get isForeign(): boolean {
      if (this.Type && (this.Type.includes('Map') || this.Type.includes('Membership'))) {
        return false
      }
      return (this.RelatedEntityType
         && this.RelatedEntityType === 'Foreign')
    }

    isString(): boolean {
      return (this.Type && this.Type === 'Edm.String')
    }

    isSearchable(): boolean {
      return this.Searchable
    }

    isAuditable(): boolean {
      return !(this.auditableProperties.indexOf(this.Name) === -1)
    }

    isNonContains(): boolean {
      return (!this.Type || this.isNumeric())
    }

    isNumeric(): boolean {
      return (this.Type === 'Edm.Int64' || this.Type === 'Edm.Int32' || this.Type === 'Edm.Double')
    }

    isDate(): boolean {
      return (this.Type === 'Edm.Date' || this.Type === 'Edm.DateTimeOffset')
    }

    isVisibleOnRelationsMapper(): boolean {
      return this.VisibleOnRelations
    }
  }

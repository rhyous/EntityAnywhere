/** Represents data about an extension entity */
export interface ExtensionEntity {
    auditableProperties?: string[]
    Name: string
    Type: string
    ReadOnly?: boolean
    Nullable?: boolean
    Collection?: boolean
    DisplayOrder?: number
    RelatedEntityType?: string
    Kind?: string
}

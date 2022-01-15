import { EntityField } from './entity-field'

export class EntityMetadata {
  Name: string
  UIDisplayName: string
  Kind: string
  Key: string[] = []
  Fields: EntityField[] = []

  getSearchFields(): EntityField[] {
    return this.Fields.filter(x => x.Searchable)
  }

  getField(fieldName: string): EntityField {
    const field = this.Fields.find(x => x.Name === fieldName)
    if (field === undefined) {
      throw new Error(`${fieldName} does not exist in the ${this.Name} entity metadata`)
    }
    return field
  }

  hasField(fieldName: string): boolean {
    const field = this.Fields.find(x => x.Name === fieldName)
    if (field === undefined) {
      return false
    }
    return true
  }
}

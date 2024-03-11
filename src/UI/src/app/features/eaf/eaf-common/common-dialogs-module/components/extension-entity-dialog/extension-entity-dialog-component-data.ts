import { ExtensionEntity } from '../../../common-form-module/models/interfaces/extension-entity'

/** Represents the data that must be passed to the ExtensionEntityDialogComponent */
export interface ExtensionEntityDialogComponentData {
    extensionEntityData: ExtensionEntity
    parentEntityName: string
    parentEntityId: number
}

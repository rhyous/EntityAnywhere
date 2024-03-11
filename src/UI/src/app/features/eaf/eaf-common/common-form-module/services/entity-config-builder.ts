import { Injectable } from '@angular/core'
import { AnyEx } from 'src/app/core/infrastructure/extensions/any-ex'
import { NullEx } from 'src/app/core/infrastructure/extensions/null-ex'
import { WellKnownProperties } from 'src/app/core/models/concretes/well-known-properties'
import { SpaceTitlePipe } from 'src/app/core/pipes/spacetitle.pipe'
import { EntityField } from '../models/concretes/entity-field'
import { FieldConfig } from '../models/interfaces/field-config.interface'
import { EntityFieldValidatorProvider } from './entity-field-validator-provider'
import { EntityPropertyTypeControlTypeMap } from './entity-property-type-control-type.map'
import { EntityPropertyTypeInputTypeMap } from './entity-property-type-input-type.map'
import { EnumOptionMapper } from './enum-option-mapper'

/** Builds a FieldConfig from an EntityField instance. */
@Injectable()
export class EntityConfigBuilder {

  constructor(private entityPropertyTypeControlTypeMap: EntityPropertyTypeControlTypeMap,
              private entityPropertyTypeInputTypeMap: EntityPropertyTypeInputTypeMap,
              private enumOptionMapper: EnumOptionMapper,
              private entityFieldValidationProvider: EntityFieldValidatorProvider,
              private spaceTitlePipe: SpaceTitlePipe,
              private wellKnownProperties: WellKnownProperties) { }

  /** Builds a FieldConfig from an EntityField instance.
   * @param entityName The name of the entity
   * @param field The EntityField
   * @param entitydata The entityData
   * @param navigationField The EntityField of the navigation field */
  build(entityName: string, field: EntityField, entitydata: any, navigationField: EntityField | null | undefined): FieldConfig {
    const fieldConfig: FieldConfig = {
      type: this.entityPropertyTypeControlTypeMap.getValueOrDefault(field.Type)(field),
      label:  this.spaceTitlePipe.transform(field.hasNavigationKey() ? field.NavigationKey : field.Name),
      inputType: this.entityPropertyTypeInputTypeMap.getValueOrDefault(field.Type),
      name: field.Name,
      entity: entityName,
      flex: 50,
      order: AnyEx.getValue(field.DisplayOrder, 0),
      options: field.isEnum() ? this.enumOptionMapper.mapOptions(field.Options) : undefined,
      validations: this.entityFieldValidationProvider.getValidator(field),
      required: AnyEx.getValue(field.Required, false),
      filter: field.Filter,
      readOnly: AnyEx.getValue(field.ReadOnly, false),
      isAuditable: field.isAuditable(this.wellKnownProperties.auditableProperties)
    }

    if (fieldConfig.type === 'EntitySearcher' && !NullEx.isNullOrUndefined(navigationField)) {
      fieldConfig.searchEntity = navigationField.Type.substring(5)
      fieldConfig.searchEntityDefault = navigationField.Default
    }

    if (fieldConfig.type === 'checkbox') {
      // Default it to false
      fieldConfig.value = false
      fieldConfig.disabled = field.ReadOnly
    }

    if (entitydata) {
      fieldConfig.value = entitydata[field.Name]
    }
    return fieldConfig
  }
}

import { EntityField } from './entity-field'

describe('Entity Field class', () => {

  let ef: EntityField
  beforeEach(() => {
    ef = new EntityField()
  })

  it ('Should be flagged as an enum', () => {
    ef.Kind = 'EnumType'
    expect(ef.isEnum()).toBeTruthy()
  })

  it('Should not be flagged as enum if kind is undefined', () => {
    expect(ef.isEnum()).toBeFalsy()
  })

  it('Should not be flagged as enum if kind is not enum', () => {
    ef.Kind = 'NavigationProperty'
    expect(ef.isEnum()).toBeFalsy()
  })

  it('Should show as a navigationkey if present', () => {
    ef.NavigationKey = 'Feature'
    expect(ef.hasNavigationKey()).toBeTruthy()
  })

  it('Should not show as having a Navigation key as a default', () => {
    expect(ef.hasNavigationKey()).toBeFalsy()
  })

  it ('Should show as a Navigation property', () => {
    ef.Kind = 'NavigationProperty'
    expect(ef.isNavigationProperty()).toBeTruthy()
  })

  it ('Should show as a mapping property', () => {
    ef.RelatedEntityType = 'Mapping'
    expect(ef.isMapping()).toBeTruthy()
  })

  it ('Should show as not a mapping property', () => {
    ef.RelatedEntityType = 'Local'
    expect(ef.isMapping()).toBeFalsy()
  })

  it ('Should show as not a mapping property as default', () => {
    expect(ef.isMapping()).toBeFalsy()
  })

  it ('Should show as a string', () => {
    ef.Type = 'Edm.String'
    expect(ef.isString()).toBeTruthy()
    expect(ef.isDate()).toBeFalsy()
    expect(ef.isNumeric()).toBeFalsy()
  })

  it ('Should show as a number when int32', () => {
    ef.Type = 'Edm.Int32'
    expect(ef.isString()).toBeFalsy()
    expect(ef.isDate()).toBeFalsy()
    expect(ef.isNumeric()).toBeTruthy()
  })

  it ('Should show as a number when int64', () => {
    ef.Type = 'Edm.Int64'
    expect(ef.isString()).toBeFalsy()
    expect(ef.isDate()).toBeFalsy()
    expect(ef.isNumeric()).toBeTruthy()
  })

  it ('Should show as a number when double', () => {
    ef.Type = 'Edm.Double'
    expect(ef.isString()).toBeFalsy()
    expect(ef.isDate()).toBeFalsy()
    expect(ef.isNumeric()).toBeTruthy()
  })

  it ('Should show as a date', () => {
    ef.Type = 'Edm.Date'
    expect(ef.isString()).toBeFalsy()
    expect(ef.isDate()).toBeTruthy()
    expect(ef.isNumeric()).toBeFalsy()
  })

  it ('Should show as a date when datetimeoffset', () => {
    ef.Type = 'Edm.DateTimeOffset'
    expect(ef.isString()).toBeFalsy()
    expect(ef.isDate()).toBeTruthy()
    expect(ef.isNumeric()).toBeFalsy()
  })

  it('Should createdate be classed as an auditable field', () => {
    ef.Name = 'CreateDate'
    expect(ef.isAuditable()).toBeTruthy()
  })

  it('Should createdby be classed as an auditable field', () => {
    ef.Name = 'CreatedBy'
    expect(ef.isAuditable()).toBeTruthy()
  })

  it('Should LastUpdated be classed as an auditable field', () => {
    ef.Name = 'LastUpdated'
    expect(ef.isAuditable()).toBeTruthy()
  })

  it('Should LastUpdatedBy be classed as an auditable field', () => {
    ef.Name = 'LastUpdatedBy'
    expect(ef.isAuditable()).toBeTruthy()
  })

  it('Should class other names as not auditable field', () => {
    ef.Name = 'Id'
    expect(ef.isAuditable()).toBeFalsy()
  })

  it('Should not be searchable as a default', () => {
    expect(ef.isSearchable()).toBeFalsy()
  })

  it('Should be searchable if marked as such', () => {
    ef.Searchable = true
    expect(ef.isSearchable()).toBeTruthy()
  })

  it('Should be non contains if numeric', () => {
    ef.Type = 'Edm.Int32'
    spyOn(ef, 'isNumeric').and.callThrough()
    expect(ef.isNonContains()).toBeTruthy()
    expect(ef.isNumeric).toHaveBeenCalledTimes(1)
  })

  it('Should be contains if not numeric', () => {
    ef.Type = 'Edm.String'
    spyOn(ef, 'isNumeric').and.callThrough()
    expect(ef.isNonContains()).toBeFalsy()
    expect(ef.isNumeric).toHaveBeenCalledTimes(1)
  })

  it ('Should show as a Extension property', () => {
    ef.RelatedEntityType = 'Extension'
    expect(ef.isExtension).toBeTruthy()
  })

  it ('Should show as not a Extension property', () => {
    ef.RelatedEntityType = 'Local'
    expect(ef.isExtension).toBeFalsy()
  })

  it ('Should show as not a Extension property as default', () => {
    expect(ef.isExtension).toBeFalsy()
  })

  it ('Should show as a Foreign property', () => {
    ef.Type = 'Edm.Double'
    ef.RelatedEntityType = 'Foreign'
    expect(ef.isForeign).toBeTruthy()
  })

  it ('Should show as not a Foreign property', () => {
    ef.Type = 'Edm.Double'
    ef.RelatedEntityType = 'Local'
    expect(ef.isForeign).toBeFalsy()
  })

  it ('Should show as not a Foreign property as default', () => {
    ef.Type = 'Edm.Double'
    expect(ef.isForeign).toBeFalsy()
  })

  it ('Should show as not a Foreign property if Map or Membership type', () => {
    ef.RelatedEntityType = 'Foreign'
    ef.Type = 'Mapping'
    expect(ef.isForeign).toBeFalsy()
  })

  it('Should not be Visible On Relations as a default', () => {
    expect(ef.isVisibleOnRelationsMapper()).toBeFalsy()
  })

  it('Should be Visible On Relations if marked as such', () => {
    ef.VisibleOnRelations = true
    expect(ef.isVisibleOnRelationsMapper()).toBeTruthy()
  })

})

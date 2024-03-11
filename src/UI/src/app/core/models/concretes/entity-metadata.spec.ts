import { EntityMetadata } from './entity-metadata'

let em: EntityMetadata
beforeEach(() => {
    em = new EntityMetadata()
})

describe('Entity Field class', () => {

    it('Should show as as a mapping entity type', () => {
        em.EntityType = 'Mapping'
        expect(em.isMappingEntityType).toBeTruthy()
    })

    it('Should NOT show as a mapping entity type', () => {
        em.EntityType = 'Lookup'
        expect(em.isMappingEntityType).toBeFalsy()
    })
})

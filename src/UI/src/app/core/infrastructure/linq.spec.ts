import './linq'

describe('linq', () => {
    describe('whereMatches', () => {
        it('should be able to take an object', () => {
            // Arrange
            const arr = [
                {
                        myProp: 'My Property',
                        myVal: 'My Val'
                },
                {
                        myProp: 'Hello',
                        myVal: 'World'
                }
            ]

            // Act
            const result = arr.whereMatches({myProp: 'My Property'})

            // Assert
            expect(result.length).toBe(1)
            expect(result[0].myProp).toEqual('My Property')
            expect(result[0].myVal).toEqual('My Val')
        })
    })

    describe('facetedSearch', () => {
        //#region Entity Data

        const entityData = {
            'Count': 49,
            'Entities': [{
                    'Id': 1,
                    'Object': {
                        'Id': 1,
                        'Name': 'EntitledProduct',
                        'Description': 'A calculated entity that is generated from Entitlements.',
                        'Enabled': true,
                        'EntityGroupId': 5,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T10:01:17.0996284+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(1)'
                }, {
                    'Id': 2,
                    'Object': {
                        'Id': 2,
                        'Name': 'Entitlement',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 5,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T10:01:56.8002948+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(2)'
                }, {
                    'Id': 3,
                    'Object': {
                        'Id': 3,
                        'Name': 'File',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 6,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T10:02:30.7468129+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(3)'
                }, {
                    'Id': 4,
                    'Object': {
                        'Id': 4,
                        'Name': 'Product',
                        'Description': 'An entitleable product.',
                        'Enabled': true,
                        'EntityGroupId': 8,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T10:02:55.0858364+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(4)'
                }, {
                    'Id': 5,
                    'Object': {
                        'Id': 5,
                        'Name': 'User',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 4,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T10:03:21.1147968+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(5)'
                }, {
                    'Id': 6,
                    'Object': {
                        'Id': 6,
                        'Name': 'ActivationAttempt',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 6,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T10:04:44.2351867+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(6)'
                }, {
                    'Id': 7,
                    'Object': {
                        'Id': 7,
                        'Name': 'Addendum',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 3,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T10:05:14.6240219+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(7)'
                }, {
                    'Id': 8,
                    'Object': {
                        'Id': 8,
                        'Name': 'AlternateId',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 3,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T10:05:44.8136695+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(8)'
                }, {
                    'Id': 9,
                    'Object': {
                        'Id': 9,
                        'Name': 'AuthenticationAttempt',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 10,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T10:06:09.9830582+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(9)'
                }, {
                    'Id': 10,
                    'Object': {
                        'Id': 10,
                        'Name': 'ClaimConfiguration',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 10,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T10:06:39.2627738+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(10)'
                }, {
                    'Id': 11,
                    'Object': {
                        'Id': 11,
                        'Name': 'CoreServer',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 1,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': null,
                        'LastUpdatedBy': null
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(11)'
                }, {
                    'Id': 12,
                    'Object': {
                        'Id': 12,
                        'Name': 'Country',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 1,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': null,
                        'LastUpdatedBy': null
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(12)'
                }, {
                    'Id': 13,
                    'Object': {
                        'Id': 13,
                        'Name': 'DealType',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 5,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T10:08:19.2109313+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(13)'
                }, {
                    'Id': 14,
                    'Object': {
                        'Id': 14,
                        'Name': 'EntitlementGroup',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 1,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T10:10:05.7618139+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(14)'
                }, {
                    'Id': 15,
                    'Object': {
                        'Id': 15,
                        'Name': 'EntitlementGroupMembership',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 1,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': null,
                        'LastUpdatedBy': null
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(15)'
                }, {
                    'Id': 16,
                    'Object': {
                        'Id': 16,
                        'Name': 'EntitlementType',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 5,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T10:10:21.2721381+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(16)'
                }, {
                    'Id': 17,
                    'Object': {
                        'Id': 17,
                        'Name': 'Entity',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 2,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T10:10:49.7619679+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(17)'
                }, {
                    'Id': 18,
                    'Object': {
                        'Id': 18,
                        'Name': 'EntityGroup',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 2,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T10:11:07.6017981+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(18)'
                }, {
                    'Id': 19,
                    'Object': {
                        'Id': 19,
                        'Name': 'EntityHandlerError',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 1,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': null,
                        'LastUpdatedBy': null
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(19)'
                }, {
                    'Id': 20,
                    'Object': {
                        'Id': 20,
                        'Name': 'EntityProperty',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 2,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T10:12:01.8862907+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(20)'
                }, {
                    'Id': 21,
                    'Object': {
                        'Id': 21,
                        'Name': 'FailedEntitledProductCalculation',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 5,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-20T09:36:03.7030631-06:00',
                        'LastUpdatedBy': 41892
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(21)'
                }, {
                    'Id': 22,
                    'Object': {
                        'Id': 22,
                        'Name': 'Feature',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 8,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T10:13:11.1999413+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(22)'
                }, {
                    'Id': 26,
                    'Object': {
                        'Id': 26,
                        'Name': 'LicenseDownloadAttempt',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 6,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T10:14:52.9053361+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(26)'
                }, {
                    'Id': 27,
                    'Object': {
                        'Id': 27,
                        'Name': 'Organization',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 9,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T10:15:14.2551771+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(27)'
                }, {
                    'Id': 28,
                    'Object': {
                        'Id': 28,
                        'Name': 'OrganizationGroup',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 9,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T10:15:43.9154321+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(28)'
                }, {
                    'Id': 29,
                    'Object': {
                        'Id': 29,
                        'Name': 'OrganizationGroupEarlyRelease',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 9,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T10:16:13.7850277+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(29)'
                }, {
                    'Id': 30,
                    'Object': {
                        'Id': 30,
                        'Name': 'OrganizationGroupMembership',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 9,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T10:16:47.6490803+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(30)'
                }, {
                    'Id': 31,
                    'Object': {
                        'Id': 31,
                        'Name': 'ProductFeatureMap',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 8,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T10:17:24.3886237+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(31)'
                }, {
                    'Id': 32,
                    'Object': {
                        'Id': 32,
                        'Name': 'ProductGroup',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 8,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T10:18:00.2185679+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(32)'
                }, {
                    'Id': 43,
                    'Object': {
                        'Id': 43,
                        'Name': 'UserGroup',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 4,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T11:07:43.8599664+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(43)'
                }, {
                    'Id': 44,
                    'Object': {
                        'Id': 44,
                        'Name': 'UserGroupMembership',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 4,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T11:08:01.0392278+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(44)'
                }, {
                    'Id': 45,
                    'Object': {
                        'Id': 45,
                        'Name': 'UserRole',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 4,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T11:08:56.7480694+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(45)'
                }, {
                    'Id': 46,
                    'Object': {
                        'Id': 46,
                        'Name': 'UserRoleMembership',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 4,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T11:09:15.735233+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(46)'
                }, {
                    'Id': 47,
                    'Object': {
                        'Id': 47,
                        'Name': 'UserType',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 4,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T11:09:31.4241538+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(47)'
                }, {
                    'Id': 48,
                    'Object': {
                        'Id': 48,
                        'Name': 'UserTypeMap',
                        'Description': '',
                        'Enabled': true,
                        'EntityGroupId': 4,
                        'CreateDate': '2019-04-30T11:57:19.065698-06:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T11:09:48.0536536+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(48)'
                }, {
                    'Id': 49,
                    'Object': {
                        'Id': 49,
                        'Name': 'EntitlementState',
                        'Description': 'The state of the entitlement',
                        'Enabled': true,
                        'EntityGroupId': 5,
                        'CreateDate': '2019-05-14T14:03:34.1394959+01:00',
                        'CreatedBy': 2,
                        'LastUpdated': '2019-08-19T11:10:09.6935233+01:00',
                        'LastUpdatedBy': 3
                    },
                    'Uri': 'http://localhost:3896/EntityService/Entities(49)'
                }
            ],
            'Entity': 'Entity',
            'TotalCount': 49
        }

        //#endregion

        it('should be able to filter', () => {
            // Arrange
            const filter = {
                Id: {
                    filter: '',
                    exactMatch: false
                },
                Name: {
                    filter: 'Entitle',
                    exactMatch: false
                },
                Description: {
                    filter: '',
                    exactMatch: false
                },
                Enabled: {
                    filter: '',
                    exactMatch: false
                },
                EntityGroupId: {
                    filter: '',
                    exactMatch: false
                }
            }

            // Act
            const result = entityData.Entities.odataFacetedSearch(filter)

            // Assert
            expect(result.length).toEqual(7)
        })

        it('should be able to filter on 2 properties', () => {
            // Arrange
            const filter = {
                Id: {
                    filter: '',
                    exactMatch: false
                },
                Name: {
                    filter: 'Entitle',
                    exactMatch: false
                },
                Description: {
                    filter: 'state',
                    exactMatch: false
                },
                Enabled: {
                    filter: '',
                    exactMatch: false
                },
                EntityGroupId: {
                    filter: '',
                    exactMatch: false
                }
            }

            // Act
            const result = entityData.Entities.odataFacetedSearch(filter)

            // Assert
            expect(result.length).toEqual(1)
        })

        it('should be able to differentiate between exact and contains', () => {
            // Arrange
            const filter = {
                Id: {
                    filter: '',
                    exactMatch: false
                },
                Name: {
                    filter: 'Entitlement',
                    exactMatch: false
                },
                Description: {
                    filter: '',
                    exactMatch: false
                },
                Enabled: {
                    filter: '',
                    exactMatch: false
                },
                EntityGroupId: {
                    filter: '',
                    exactMatch: false
                }
            }

            // Act
            const result = entityData.Entities.odataFacetedSearch(filter)

            // Assert
            expect(result.length).toEqual(5)

            // Arrange
            filter.Name.exactMatch = true

            // Act
            const secondResults = entityData.Entities.odataFacetedSearch(filter)

            // Assert
            expect(secondResults.length).toEqual(1)
        })

    })
})



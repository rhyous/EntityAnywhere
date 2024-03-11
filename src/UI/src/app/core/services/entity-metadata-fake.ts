export class Fake {
    static FakeEntityMeta: any = {
            '$Kind': 'EntityType',
            'OrderId': {
              '$Collection': true,
              '$Type': 'Edm.String'
            },
            'OrganizationId': {
              '$Type': 'Edm.Int32'
            },
            'LineNumber': {
              '$Collection': true,
              '$Type': 'Edm.String'
            },
            'ProductId': {
              '$Type': 'Edm.Int32'
            },
            'Sku': {
              '$Collection': true,
              '$Type': 'Edm.String'
            },
            'StartDate': {
              '$Type': 'Edm.Date'
            },
            'EndDate': {
              '$Type': 'Edm.Date'
            },
            'Quantity': {
              '$Type': 'Edm.Int32'
            },
            'Enabled': {
              '$Type': 'Edm.Boolean'
            },
            'TypeId': {
              '$Type': 'Edm.Int32'
            },
            'IsPerpetual': {
              '$Type': 'Edm.Boolean'
            },
            'DealTypeId': {
              '$Kind': 'EnumType',
              '$UnderlyingType': 'Edm.Int32',
              'New': 1,
              'Volume': 2,
              'Renewal': 3
            },
            'CreateDate': {
              '$Type': 'Edm.Date'
            },
            'LastUpdated': {
              '$Type': 'Edm.Date'
            },
            'CreatedBy': {
              '$Type': 'Edm.Int64'
            },
            'LastUpdatedBy': {
              '$Type': 'Edm.Int64'
            },
            'Id': {
              '$Type': 'Edm.Int64'
            }
          }

    static FakeMeta = [{
        'key': 'Addendum',
        'value': {
            '$Key': ['Id'],
            '$Kind': 'EntityType',
            'CreateDate': {
                '$Type': 'Edm.DateTimeOffset',
                '@UI.DisplayOrder': 6,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'CreatedBy': {
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 7,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'Entity': {
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 2,
                '@UI.Searchable': false
            },
            'EntityId': {
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 3,
                '@UI.Searchable': false
            },
            'Id': {
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 1,
                '@UI.Searchable': true
            },
            'LastUpdated': {
                '$Nullable': true,
                '$Type': 'Edm.DateTimeOffset',
                '@UI.DisplayOrder': 8,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'LastUpdatedBy': {
                '$Nullable': true,
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 9,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'Property': {
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 4,
                '@UI.Searchable': false
            },
            'Value': {
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 5,
                '@UI.Searchable': false
            },
            '@EAF.EntityGroup': 'Extension Entities'
        }
    },                 {
        'key': 'AlternateId',
        'value': {
            '$Key': ['Id'],
            '$Kind': 'EntityType',
            'CreateDate': {
                '$Type': 'Edm.DateTimeOffset',
                '@UI.DisplayOrder': 6,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'CreatedBy': {
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 7,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'Entity': {
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 2,
                '@UI.Searchable': false
            },
            'EntityId': {
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 3,
                '@UI.Searchable': false
            },
            'Id': {
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 1,
                '@UI.Searchable': true
            },
            'LastUpdated': {
                '$Nullable': true,
                '$Type': 'Edm.DateTimeOffset',
                '@UI.DisplayOrder': 8,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'LastUpdatedBy': {
                '$Nullable': true,
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 9,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'Property': {
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 4,
                '@UI.Searchable': false
            },
            'Value': {
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 5,
                '@UI.Searchable': false
            },
            '@EAF.EntityGroup': 'Extension Entities'
        }
    },                 {
        'key': 'AuthenticationAttempt',
        'value': {
            '$Key': ['Id'],
            '$Kind': 'EntityType',
            'CreateDate': {
                '$Type': 'Edm.DateTimeOffset',
                '@UI.DisplayOrder': 7,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'CreatedBy': {
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 8,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'Id': {
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 1,
                '@UI.Searchable': true
            },
            'Ignore': {
                '$Type': 'Edm.Boolean',
                '@UI.DisplayOrder': 2,
                '@UI.Searchable': false
            },
            'IpAddress': {
                '$Nullable': true,
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 3,
                '@UI.Searchable': false
            },
            'LastUpdated': {
                '$Nullable': true,
                '$Type': 'Edm.DateTimeOffset',
                '@UI.DisplayOrder': 9,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'LastUpdatedBy': {
                '$Nullable': true,
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 10,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'Message': {
                '$Nullable': true,
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 4,
                '@UI.Searchable': false
            },
            'Result': {
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 5,
                '@UI.Searchable': false
            },
            'Username': {
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 6,
                '@UI.Searchable': true
            },
            '@EAF.EntityGroup': 'System Configuration',
            '@UI.DisplayName': {
                '$PropertyPath': 'Username'
            }
        }
    },                 {
        'key': 'ClaimConfiguration',
        'value': {
            '$Key': ['Id'],
            '$Kind': 'EntityType',
            'CreateDate': {
                '$Type': 'Edm.DateTimeOffset',
                '@UI.DisplayOrder': 8,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'CreatedBy': {
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 9,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'Domain': {
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 3,
                '@UI.Searchable': false
            },
            'Entity': {
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 4,
                '@UI.Searchable': false
            },
            'EntityIdProperty': {
                '$Nullable': true,
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 5,
                '@UI.Searchable': false
            },
            'EntityProperty': {
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 6,
                '@UI.Searchable': false
            },
            'Id': {
                '$Type': 'Edm.Int32',
                '@UI.DisplayOrder': 1,
                '@UI.Searchable': true
            },
            'LastUpdated': {
                '$Nullable': true,
                '$Type': 'Edm.DateTimeOffset',
                '@UI.DisplayOrder': 10,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'LastUpdatedBy': {
                '$Nullable': true,
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 11,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'Name': {
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 2,
                '@UI.Searchable': true
            },
            'RelatedEntityIdProperty': {
                '$Nullable': true,
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 7,
                '@UI.Searchable': false
            },
            '@EAF.EntityGroup': 'System Configuration',
            'Addenda': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.Addendum',
                '@EAF.RelatedEntity.Type': 'Extension'
            },
            'AlternateIds': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.AlternateId',
                '@EAF.RelatedEntity.Type': 'Extension'
            }
        }
    },                 {
        'key': 'Country',
        'value': {
            '$Key': ['Id'],
            '$Kind': 'EntityType',
            'Id': {
                '$Type': 'Edm.Int32',
                '@UI.DisplayOrder': 1,
                '@UI.Searchable': true
            },
            'Name': {
                '$Nullable': true,
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 2,
                '@UI.Searchable': true
            },
            'ThreeLetterIsoCode': {
                '$Nullable': true,
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 3,
                '@UI.Searchable': false
            },
            'TwoLetterIsoCode': {
                '$Nullable': true,
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 4,
                '@UI.Searchable': false
            },
            '@EAF.EntityGroup': 'Miscellaneous',
            'Addenda': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.Addendum',
                '@EAF.RelatedEntity.Type': 'Extension'
            },
            'AlternateIds': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.AlternateId',
                '@EAF.RelatedEntity.Type': 'Extension'
            }
        }
    },                 {
        'key': 'Entity',
        'value': {
            '$Key': ['Id', 'Name'],
            '$Kind': 'EntityType',
            'CreateDate': {
                '$Type': 'Edm.DateTimeOffset',
                '@UI.DisplayOrder': 6,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'CreatedBy': {
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 7,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'Description': {
                '$Nullable': true,
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 3,
                '@UI.Searchable': true
            },
            'Enabled': {
                '$Type': 'Edm.Boolean',
                '@UI.DisplayOrder': 4,
                '@UI.Searchable': false
            },
            'EntityGroupId': {
                '$Type': 'Edm.Int32',
                '@UI.DisplayOrder': 5,
                '@UI.Searchable': false,
                '$NavigationKey': 'EntityGroup'
            },
            'EntityGroup': {
                '$Kind': 'NavigationProperty',
                '$ReferentialConstraint': {
                    'LocalProperty': 'EntityGroupId',
                    'ForeignProperty': 'Id'
                },
                '$Type': 'self.EntityGroup',
                '@EAF.RelatedEntity.Type': 'Local'
            },
            'Id': {
                '$Type': 'Edm.Int32',
                '@UI.DisplayOrder': 1,
                '@UI.Searchable': true
            },
            'LastUpdated': {
                '$Nullable': true,
                '$Type': 'Edm.DateTimeOffset',
                '@UI.DisplayOrder': 8,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'LastUpdatedBy': {
                '$Nullable': true,
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 9,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'Name': {
                '$Nullable': true,
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 2,
                '@UI.Searchable': true
            },
            '@EAF.EntityGroup': 'Entity Management',
            'EntityProperties': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.EntityProperty',
                '@EAF.RelatedEntity.Type': 'Foreign'
            },
            'Addenda': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.Addendum',
                '@EAF.RelatedEntity.Type': 'Extension'
            },
            'AlternateIds': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.AlternateId',
                '@EAF.RelatedEntity.Type': 'Extension'
            }
        }
    },                 {
        'key': 'EntityGroup',
        'value': {
            '$Key': ['Id', 'Name'],
            '$Kind': 'EntityType',
            'CreateDate': {
                '$Type': 'Edm.DateTimeOffset',
                '@UI.DisplayOrder': 4,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'CreatedBy': {
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 5,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'Description': {
                '$Nullable': true,
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 3,
                '@UI.Searchable': true
            },
            'Id': {
                '$Type': 'Edm.Int32',
                '@UI.DisplayOrder': 1,
                '@UI.Searchable': true
            },
            'LastUpdated': {
                '$Nullable': true,
                '$Type': 'Edm.DateTimeOffset',
                '@UI.DisplayOrder': 6,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'LastUpdatedBy': {
                '$Nullable': true,
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 7,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'Name': {
                '$Nullable': true,
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 2,
                '@UI.Searchable': true
            },
            '@EAF.EntityGroup': 'Entity Management',
            'Addenda': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.Addendum',
                '@EAF.RelatedEntity.Type': 'Extension'
            },
            'AlternateIds': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.AlternateId',
                '@EAF.RelatedEntity.Type': 'Extension'
            }
        }
    },                 {
        'key': 'EntityProperty',
        'value': {
            '$Key': ['Id'],
            '$Kind': 'EntityType',
            'CreateDate': {
                '$Type': 'Edm.DateTimeOffset',
                '@UI.DisplayOrder': 9,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'CreatedBy': {
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 10,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'Description': {
                '$Nullable': true,
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 4,
                '@UI.Searchable': true
            },
            'EntityId': {
                '$Type': 'Edm.Int32',
                '@UI.DisplayOrder': 2,
                '@UI.Searchable': false,
                '$NavigationKey': 'Entity'
            },
            'Entity': {
                '$Kind': 'NavigationProperty',
                '$ReferentialConstraint': {
                    'LocalProperty': 'EntityId',
                    'ForeignProperty': 'Id'
                },
                '$Type': 'self.Entity',
                '@EAF.RelatedEntity.Type': 'Local'
            },
            'Id': {
                '$Type': 'Edm.Int32',
                '@UI.DisplayOrder': 1,
                '@UI.Searchable': true
            },
            'LastUpdated': {
                '$Nullable': true,
                '$Type': 'Edm.DateTimeOffset',
                '@UI.DisplayOrder': 11,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'LastUpdatedBy': {
                '$Nullable': true,
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 12,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'Name': {
                '$Nullable': true,
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 3,
                '@UI.Searchable': true
            },
            'Nullable': {
                '$Type': 'Edm.Boolean',
                '@UI.DisplayOrder': 5,
                '@UI.Searchable': false
            },
            'Order': {
                '$Type': 'Edm.Int32',
                '@UI.DisplayOrder': 6,
                '@UI.Searchable': false
            },
            'Searchable': {
                '$Type': 'Edm.Boolean',
                '@UI.DisplayOrder': 7,
                '@UI.Searchable': false
            },
            'Type': {
                '$Nullable': true,
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 8,
                '@UI.Searchable': false
            },
            '@EAF.EntityGroup': 'Entity Management',
            'Addenda': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.Addendum',
                '@EAF.RelatedEntity.Type': 'Extension'
            },
            'AlternateIds': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.AlternateId',
                '@EAF.RelatedEntity.Type': 'Extension'
            }
        }
    },                 {
        'key': 'User',
        'value': {
            '$Key': ['Id', 'Username'],
            '$Kind': 'EntityType',
            'CreateDate': {
                '$Type': 'Edm.DateTimeOffset',
                '@UI.DisplayOrder': 9,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'CreatedBy': {
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 10,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'Enabled': {
                '$Type': 'Edm.Boolean',
                '@UI.DisplayOrder': 5,
                '@UI.Searchable': false
            },
            'ExternalAuth': {
                '$Type': 'Edm.Boolean',
                '@UI.DisplayOrder': 6,
                '@UI.Searchable': false
            },
            'Id': {
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 1,
                '@UI.Searchable': true
            },
            'IsHashed': {
                '$Type': 'Edm.Boolean',
                '@UI.DisplayOrder': 7,
                '@UI.Searchable': false
            },
            'LastUpdated': {
                '$Nullable': true,
                '$Type': 'Edm.DateTimeOffset',
                '@UI.DisplayOrder': 11,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'LastUpdatedBy': {
                '$Nullable': true,
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 12,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'OrganizationId': {
                '$Type': 'Edm.Int32',
                '@UI.DisplayOrder': 8,
                '@UI.Searchable': false,
                '$NavigationKey': 'Organization'
            },
            'Organization': {
                '$Kind': 'NavigationProperty',
                '$ReferentialConstraint': {
                    'LocalProperty': 'OrganizationId',
                    'ForeignProperty': 'Id'
                },
                '$Type': 'self.Organization',
                '@EAF.RelatedEntity.Type': 'Local'
            },
            'Password': {
                '$Nullable': true,
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 3,
                '@UI.Searchable': true
            },
            'Salt': {
                '$Nullable': true,
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 4,
                '@UI.Searchable': false
            },
            'Username': {
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 2,
                '@UI.Searchable': true
            },
            '@EAF.EntityGroup': 'User Management',
            '@UI.DisplayName': {
                '$PropertyPath': 'Username'
            },
            'UserGroupMemberships': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.UserGroupMembership',
                '@EAF.RelatedEntity.Type': 'Foreign'
            },
            'UserRoleMemberships': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.UserRoleMembership',
                '@EAF.RelatedEntity.Type': 'Foreign'
            },
            'UserTypeMaps': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.UserTypeMap',
                '@EAF.RelatedEntity.Type': 'Foreign'
            },
            'UserGroups': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.UserGroup',
                '@EAF.RelatedEntity.Type': 'Mapping',
                '@EAF.RelatedEntity.MappingEntityType': 'self.UserGroupMembership'
            },
            'UserRoles': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.UserRole',
                '@EAF.RelatedEntity.Type': 'Mapping',
                '@EAF.RelatedEntity.MappingEntityType': 'self.UserRoleMembership'
            },
            'UserTypes': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.UserType',
                '@EAF.RelatedEntity.Type': 'Mapping',
                '@EAF.RelatedEntity.MappingEntityType': 'self.UserTypeMap'
            },
            'Addenda': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.Addendum',
                '@EAF.RelatedEntity.Type': 'Extension'
            },
            'AlternateIds': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.AlternateId',
                '@EAF.RelatedEntity.Type': 'Extension'
            }
        }
    },                 {
        'key': 'UserGroup',
        'value': {
            '$Key': ['Id', 'Name'],
            '$Kind': 'EntityType',
            'CreateDate': {
                '$Type': 'Edm.DateTimeOffset',
                '@UI.DisplayOrder': 4,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'CreatedBy': {
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 5,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'Description': {
                '$Nullable': true,
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 3,
                '@UI.Searchable': true
            },
            'Id': {
                '$Type': 'Edm.Int32',
                '@UI.DisplayOrder': 1,
                '@UI.Searchable': true
            },
            'LastUpdated': {
                '$Nullable': true,
                '$Type': 'Edm.DateTimeOffset',
                '@UI.DisplayOrder': 6,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'LastUpdatedBy': {
                '$Nullable': true,
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 7,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'Name': {
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 2,
                '@UI.Searchable': true
            },
            '@EAF.EntityGroup': 'User Management',
            'Addenda': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.Addendum',
                '@EAF.RelatedEntity.Type': 'Extension'
            },
            'AlternateIds': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.AlternateId',
                '@EAF.RelatedEntity.Type': 'Extension'
            }
        }
    },                 {
        'key': 'UserGroupMembership',
        'value': {
            '$Key': ['Id'],
            '$Kind': 'EntityType',
            'Id': {
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 1,
                '@UI.Searchable': true
            },
            'UserGroupId': {
                '$Type': 'Edm.Int32',
                '@UI.DisplayOrder': 2,
                '@UI.Searchable': false,
                '$NavigationKey': 'UserGroup'
            },
            'UserGroup': {
                '$Kind': 'NavigationProperty',
                '$ReferentialConstraint': {
                    'LocalProperty': 'UserGroupId',
                    'ForeignProperty': 'Id'
                },
                '$Type': 'self.UserGroup',
                '@EAF.RelatedEntity.Type': 'Local'
            },
            'UserId': {
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 3,
                '@UI.Searchable': false,
                '$NavigationKey': 'User'
            },
            'User': {
                '$Kind': 'NavigationProperty',
                '$ReferentialConstraint': {
                    'LocalProperty': 'UserId',
                    'ForeignProperty': 'Id'
                },
                '$Type': 'self.User',
                '@EAF.RelatedEntity.Type': 'Local'
            },
            '@EAF.EntityGroup': 'User Management',
            'Addenda': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.Addendum',
                '@EAF.RelatedEntity.Type': 'Extension'
            },
            'AlternateIds': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.AlternateId',
                '@EAF.RelatedEntity.Type': 'Extension'
            }
        }
    },                 {
        'key': 'UserRole',
        'value': {
            '$Key': ['Id', 'Name'],
            '$Kind': 'EntityType',
            'CreateDate': {
                '$Type': 'Edm.DateTimeOffset',
                '@UI.DisplayOrder': 4,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'CreatedBy': {
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 5,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'Description': {
                '$Nullable': true,
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 3,
                '@UI.Searchable': true
            },
            'Enabled': {
                '$Type': 'Edm.Boolean',
                '@UI.DisplayOrder': 1,
                '@UI.Searchable': false
            },
            'Id': {
                '$Type': 'Edm.Int32',
                '@UI.DisplayOrder': 1,
                '@UI.Searchable': true
            },
            'LastUpdated': {
                '$Nullable': true,
                '$Type': 'Edm.DateTimeOffset',
                '@UI.DisplayOrder': 6,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'LastUpdatedBy': {
                '$Nullable': true,
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 7,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'Name': {
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 2,
                '@UI.Searchable': true
            },
            '@EAF.EntityGroup': 'User Management',
            'Addenda': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.Addendum',
                '@EAF.RelatedEntity.Type': 'Extension'
            },
            'AlternateIds': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.AlternateId',
                '@EAF.RelatedEntity.Type': 'Extension'
            }
        }
    },                 {
        'key': 'UserRoleMembership',
        'value': {
            '$Key': ['Id'],
            '$Kind': 'EntityType',
            'Id': {
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 1,
                '@UI.Searchable': true
            },
            'UserId': {
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 2,
                '@UI.Searchable': false,
                '$NavigationKey': 'User'
            },
            'User': {
                '$Kind': 'NavigationProperty',
                '$ReferentialConstraint': {
                    'LocalProperty': 'UserId',
                    'ForeignProperty': 'Id'
                },
                '$Type': 'self.User',
                '@EAF.RelatedEntity.Type': 'Local'
            },
            'UserRoleId': {
                '$Type': 'Edm.Int32',
                '@UI.DisplayOrder': 3,
                '@UI.Searchable': false,
                '$NavigationKey': 'UserRole'
            },
            'UserRole': {
                '$Kind': 'NavigationProperty',
                '$ReferentialConstraint': {
                    'LocalProperty': 'UserRoleId',
                    'ForeignProperty': 'Id'
                },
                '$Type': 'self.UserRole',
                '@EAF.RelatedEntity.Type': 'Local'
            },
            '@EAF.EntityGroup': 'User Management',
            'Addenda': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.Addendum',
                '@EAF.RelatedEntity.Type': 'Extension'
            },
            'AlternateIds': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.AlternateId',
                '@EAF.RelatedEntity.Type': 'Extension'
            }
        }
    },                 {
        'key': 'UserType',
        'value': {
            '$Key': ['Id', 'Type'],
            '$Kind': 'EntityType',
            'CreateDate': {
                '$Type': 'Edm.DateTimeOffset',
                '@UI.DisplayOrder': 3,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'CreatedBy': {
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 4,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'Id': {
                '$Type': 'Edm.Int32',
                '@UI.DisplayOrder': 1,
                '@UI.Searchable': true
            },
            'LastUpdated': {
                '$Nullable': true,
                '$Type': 'Edm.DateTimeOffset',
                '@UI.DisplayOrder': 5,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'LastUpdatedBy': {
                '$Nullable': true,
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 6,
                '@UI.Searchable': false,
                '@UI.ReadOnly': true
            },
            'Type': {
                '$Type': 'Edm.String',
                '@UI.DisplayOrder': 2,
                '@UI.Searchable': false
            },
            '@EAF.EntityGroup': 'User Management',
            '@UI.DisplayName': {
                '$PropertyPath': 'Type'
            },
            'Addenda': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.Addendum',
                '@EAF.RelatedEntity.Type': 'Extension'
            },
            'AlternateIds': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.AlternateId',
                '@EAF.RelatedEntity.Type': 'Extension'
            }
        }
    },                 {
        'key': 'UserTypeMap',
        'value': {
            '$Key': ['Id'],
            '$Kind': 'EntityType',
            'Id': {
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 1,
                '@UI.Searchable': true
            },
            'UserId': {
                '$Type': 'Edm.Int64',
                '@UI.DisplayOrder': 2,
                '@UI.Searchable': false,
                '$NavigationKey': 'User'
            },
            'User': {
                '$Kind': 'NavigationProperty',
                '$ReferentialConstraint': {
                    'LocalProperty': 'UserId',
                    'ForeignProperty': 'Id'
                },
                '$Type': 'self.User',
                '@EAF.RelatedEntity.Type': 'Local'
            },
            'UserTypeId': {
                '$Type': 'Edm.Int32',
                '@UI.DisplayOrder': 3,
                '@UI.Searchable': false,
                '$NavigationKey': 'UserType'
            },
            'UserType': {
                '$Kind': 'NavigationProperty',
                '$ReferentialConstraint': {
                    'LocalProperty': 'UserTypeId',
                    'ForeignProperty': 'Id'
                },
                '$Type': 'self.UserType',
                '@EAF.RelatedEntity.Type': 'Local'
            },
            '@EAF.EntityGroup': 'User Management',
            'Addenda': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.Addendum',
                '@EAF.RelatedEntity.Type': 'Extension'
            },
            'AlternateIds': {
                '$Collection': true,
                '$Kind': 'NavigationProperty',
                '$Nullable': true,
                '$Type': 'self.AlternateId',
                '@EAF.RelatedEntity.Type': 'Extension'
            }
        }
    }
]


    static FakeDefaultClaims = [{
        'Name': 'Username',
        'Issuer': 'LOCAL AUTHORITY',
        'Subject': 'User',
        'Value': 'MattSpeakman',
        'ValueType': null
      },                        {
        'Name': 'Id',
        'Issuer': 'LOCAL AUTHORITY',
        'Subject': 'User',
        'Value': '58181',
        'ValueType': null
      },                        {
        'Name': 'LastAuthenticated',
        'Issuer': 'LOCAL AUTHORITY',
        'Subject': 'User',
        'Value': 'Mon, 04 Nov 2019 08:36:15 GMT',
        'ValueType': null
      },                        {
        'Name': 'Id',
        'Issuer': 'LOCAL AUTHORITY',
        'Subject': 'Organization',
        'Value': '272405',
        'ValueType': null
      },                        {
        'Name': 'Name',
        'Issuer': 'LOCAL AUTHORITY',
        'Subject': 'Organization',
        'Value': 'Matt Speakman',
        'ValueType': null
      },                        {
        'Name': 'SapId',
        'Issuer': 'LOCAL AUTHORITY',
        'Subject': 'Organization',
        'Value': 'MATTSORG',
        'ValueType': null
      },                        {
        'Name': 'Role',
        'Issuer': 'LOCAL AUTHORITY',
        'Subject': 'UserRole',
        'Value': 'Default',
        'ValueType': null
      }
    ]


}

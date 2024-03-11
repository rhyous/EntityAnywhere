export class MockSuiteMembershipData {
    public getEntityList() {
      const response = {
        'Count': 2,
        'Entities': [{
          'Id': 1,
          'Object': {
            'Id': 1,
            'CreateDate': '2017-03-16T20:34:57.1',
            'CreatedBy': 1
          },
          'Uri': 'http://localhost:3896/SuiteMembershipService/SuiteMemberships(1)'
        },
        {
          'Id': 2,
          'Object': {
            'Id': 2,
            'CreateDate': '2017-03-16T20:34:57.1',
            'CreatedBy': 1
          },
          'Uri': 'http://localhost:3896/SuiteMembershipService/SuiteMemberships(2)'
        }],
        'Entity': 'SuiteMembership',
        'TotalCount': 869
      }

      return response
    }

    public getMetaData() {
      const response = {
        '$Kind': 'EntityType',
        'SuiteId': {'$Type': 'Edm.Int32'},
        'ProductId': {'$Type': 'Edm.Int32'},
        'Quantity': {'$Type': 'Edm.Double'},
        'QuantityType': {'$Kind': 'EnumType',
          '$UnderlyingType': 'Edm.Int32',
          'Inherited': 1,
          'Fixed': 2,
          'Percentage': 3
        },
        'CreateDate': {'$Type': 'Edm.Date'},
        'LastUpdated': {'$Type': 'Edm.Date'},
        'CreatedBy': {'$Type': 'Edm.Int64'},
        'LastUpdatedBy': {'$Type': 'Edm.Int64'},
        'Id': {'$Type': 'Edm.Int32'}
      }

      return response
    }
  }

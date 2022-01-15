export class UserDetailData {
    public getUserMetaData() {
      const response = {
        '$Kind': 'EntityType',
        'Username': {
          '$Collection': true,
          '$Type': 'Edm.String'
        },
        'Password': {
          '$Collection': true,
          '$Type': 'Edm.String'
        },
        'Salt': {
          '$Collection': true,
          '$Type': 'Edm.String'
        },
        'IsHashed': {
          '$Type': 'Edm.Boolean'
        },
        'Enabled': {
          '$Type': 'Edm.Boolean'
        },
        'ExternalAuth': {
          '$Type': 'Edm.Boolean'
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
      return response
    }

    public getEntityData() {
      const ed = {
        'Username': null,
        'Password': null,
        'Salt': null,
        'IsHashed': false,
        'Enabled': false,
        'ExternalAuth': false,
        'CreateDate': null,
        'LastUpdated': null,
        'CreatedBy': null,
        'LastUpdatedBy': null,
        'Id': null
      }
      return ed
    }

    public getEntityDataWithoutAuditables() {
      return { Username: null,
        Password: null,
        Salt: null,
        IsHashed: false,
        Enabled: false,
        ExternalAuth: false}
    }

    public getFormValue() {
      return {
        'Username': 'test',
        'Password': 'test1',
        'Salt': null,
        'IsHashed': false,
        'Enabled': true,
        'ExternalAuth': false
      }
    }

    public getUserDetailData() {
      const response = {
        'Id': 41694,
        'Object': {
          'Id': 41694,
          'Username': 'SteffenTestAc',
          'CreateDate': '2018-08-10T15:26:11.5087697',
          'CreatedBy': 41429,
          'Enabled': true,
          'ExternalAuth': false,
          'IsHashed': false,
          'LastUpdated': null,
          'LastUpdatedBy': null,
          'Password': 's1neEfEB',
          'Salt': null
        },
        'Uri': 'https://your.domain.tld/api/UserService.svc/Users(41694)'
      }
      return response
    }
  }

import { LandingPageTypes } from '../../models/enums/LandingPageTypes'

export class MockUserData {
    static getUserToken(username: string, userId: string, userRole: string, landingPageType: LandingPageTypes) {
        const token = {
            'Id': 0,
            'Text': 'TOKEN',
            'ClaimDomains': [{
              'Claims': [{
                'Name': 'Username',
                'Issuer': 'LOCAL AUTHORITY',
                'Subject': 'User',
                'Value': username,
                'ValueType': null
              },         {
                'Name': 'Id',
                'Issuer': 'LOCAL AUTHORITY',
                'Subject': 'User',
                'Value': userId,
                'ValueType': null
              },         {
                'Name': 'LastAuthenticated',
                'Issuer': 'LOCAL AUTHORITY',
                'Subject': 'User',
                'Value': 'Thu, 02 Jan 2020 11:29:10 GMT',
                'ValueType': null
              }
              ],
              'Issuer': 'LOCAL AUTHORITY',
              'OriginalIssuer': null,
              'Subject': 'User'
            },               {
              'Claims': [{
                'Name': 'Role',
                'Issuer': 'LOCAL AUTHORITY',
                'Subject': 'UserRole',
                'Value': userRole,
                'ValueType': null
              },         {
                'Name': 'LandingPageType',
                'Issuer': 'LOCAL AUTHORITY',
                'Subject': 'UserRole',
                'Value': landingPageType,
                'ValueType': null
              }],
              'Issuer': 'LOCAL AUTHORITY',
              'OriginalIssuer': null,
              'Subject': 'UserRole'
            }
            ],
            'UserId': 3,
            'CreateDate': new Date('0001-01-01T00:00:00+00:00'),
            'CreatedBy': 0,
            'LastUpdated': null,
            'LastUpdatedBy': null
          }

        return token
    }
}

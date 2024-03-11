export const environment = {
  production: true,
  host: window.location.origin,
  serviceUrl: `${window.location.origin}/api/`,
  snackBarDuration: 2000,
  pageSizeOptions: [10, 20, 50, 100, 500],
  defaultPageSize: 50,
  metaDataLocalName: 'metadata',
  userTokenName: 'UserToken',
  adminTokenName: 'AdminToken',
  claims: 'UserClaims',
  authorizedUserRoleData: 'authorizedUserRoleData',
  debounceTimeInMs: 400
}

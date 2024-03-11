// !!The content of this file will be overwritten during build.
// If the command `ng build --env=production` is used for building portal,
// then the content of `environment.prod.ts` will overwrite this file.
// Otherwise, the build system defaults to the dev environment and the content of this file will not be overwritten.
// The list of which env maps to which file can be found in `angular.json`.

export const environment = {
  production: false,
  host: 'http://localhost:4200',
  serviceUrl: 'https://localhost:7090/api/',
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

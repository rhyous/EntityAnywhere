// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
  production: false,
  host: 'http://localhost:4200',
  serviceUrl: 'https://localhost:44355/',
  snackBarDuration: 2000,
  pageSizeOptions: [10, 20, 50, 100, 500],
  defaultPageSize: 50,
  metaDataLocalName: 'metadata',
  userTokenName: 'UserToken',
  adminTokenName: 'AdminToken',
  claims: 'UserClaims',
  authorizedUserRoleData: 'authorizedUserRoleData',
}

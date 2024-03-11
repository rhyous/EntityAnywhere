# EAFUI

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 13.1.2 and [node](https://nodejs.org/en/) version 16.13.1.

## Initial Scripts
Install node if it isn't installed already: https://nodejs.org/en/.
Run `npm install` to get the necessary node dependencies
Run `npm install --g @angular/cli@13.1.2` to install the Angular CLI Globally. You should double check the version of the @angular/cli package located in package.json

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files. Passing the `-o` flag to this command will open a browser window for you and navigate to `http://localhost:4200/`.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. Use the `--prod` flag for a production build.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).
Run `npm run test` to execute the unit tests via [Karma](https://karma-runner.github.io) with code coverage.
Run `npm run test-debug` to execute the unit tests via [Karma](https://karma-runner.github.io) with debugging in chrome.
Run `npm run test-debug-edge` to execute the unit tests via [Karma](https://karma-runner.github.io) with debugging in edge.

### To Test a single test:
1. Locate the test and find the "it( . . . )" method. Add the letter 'f' for focus so it is now "fit( . . . )"
Note: To test a whole file of tests, change "describe(. . . )" to "fdescribe(. . . )"
2. Run the same test commands as above
3. Once the test is tested and working, remove the "f" so it is no longer focused.

## end-to-end (e2e) tests (folder location: /e2e-tests)
<b>IMPORANT:</b> Use /e2e-tests as working directory. This is the root for e2e tests and therefore all commands relating to e2e tests must be run from this location.

### Installing npm modules for e2e test
Install npm modules by running the command `npm install`. 

### Configuration: Environment variables
Add a file named '.env' the e2e-tests folder.

Add the following lines to the .env file:

    ADMIN_USER_PASSWORD = "<ADMIN_PASSWORD_GOES_HERE>"
    CUSTOMER_WAREHOUSEONE_USER_PASSWORD = "<WAREHOUSEONE_PASSWORD_GOES_HERE>"

Replace the password placeholders with real admin and WarehouseOne customer passwords.

<b>NOTE:</b> This file should not be checked-in.

### Configuration: Portal URL for e2e tests

Portal base URL to be used for e2e tests is configured in .testcaferc.json file. To point e2e tests at different environment, update the value of `e2eTest-PortalBaseUrl` in this file.

    "userVariables": {
        "e2eTest-PortalBaseUrl": "http://localhost:4200"
    }

<b>NOTE: </b> user variables in this file will be substituted with environment specific values during the release.

### Running all e2e tests
Run `npx testcafe` to execute the end-to-end tests via [testcafe](https://testcafe.io/).
You can also use `npm run e2e-debug` , `npm run e2e-prod` or `npm run e2e` to run testcafe tests.

To get a nice Html output, run `npx testcafe -r html:c:\path\to\results.html`

You can also run the following commands in series to test the different parts of the site, with or without the -r parameter.

### Running tests in a specific fixture
Run `npx testcafe -f "<fixture_name>"` to run all tests in a specific fixture.
Example: `npx testcafe -f "Login-LocalCredentials-Component"`

### Running test group
Each fixture can be configured with `testGroup` metadata as below:

    fixture('Login-LocalCredentials-Component')
    .meta({testGroup: 'login'})


To run all tests in testgroup, run:
`npx testcafe --fixture-meta testGroup=<test_group_name>`

Example: `npx testcafe --fixture-meta test_group=login`

<b>NOTE:</b>Make sure to add this metadata to every fixture.

### Exclude tests from running in prod

All tests all excluded from running in production by default.
To allow test to run in production, add meta({runInProd: 'true'}) to the test.
Refer login-local-credentials.component.ispec.ts for further details.

To run all tests in testgroup which also have runInProd flag set to true as mentioned above, run:
`npx testcafe --test-meta runInProd=true --fixture-meta testGroup=<test_group_name>`

Example: `npx testcafe --test-meta runInProd=true --fixture-meta testGroup=login`


## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI README](https://github.com/angular/angular-cli/blob/master/README.md).

## Build and Deployment

### Angular Environment
The environment files are located in src/environments folder.
The content of environment.ts file will be overwritten during build.
If the command `npm run build` is used for building portal, then the content of `environment.prod.ts` will overwrite this file. This command is equal to using `ng build --env=production`.
Using `npm run build-dev` or `ng build` to build portal in dev environment, does not overwrite environment.ts file.
The list of which env maps to which file is configured in `angular.json`.

### Azure DevOps Pipeline
The yaml files located in /pipeline folder are used for building and deploying portal via Azure DevOps pipeline.
Both PR and CI pipelines use the templates in 'pipeline > templates' folder.

#### Variable groups in Azure DevOps library
Common variables whose values remain the same across environments are added to `EAFUI-Common` variable group.
Environment specific variable groups are named in the format `EAFUI-<Env_name>`.

#### Pull request pipeline:
The pipeline `it-EAFUI.PullRequest` runs when pull request is created. It builds and deploys portal to DEV1 and DEV3 environments.

#### CI pipeline:
The pipeline `it-EAFUI.CI` will be automatically triggered after a PR is merged into master. This will automatically deploy to QA and ENG, then ask for approval to go to Prod and Prod2.

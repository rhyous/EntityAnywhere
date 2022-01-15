# EAFUI

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 8.3.5.

## Initial Scripts
Run `npm install` to get the necessary node dependencies
Run `npm install --g @angular/cli@8.3.5` to install the Angular CLI Globally. You should double check the version of the @angular/cli package located in package.json

Run `npm install --g testcafe` to install the Testcafe command line globally.

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files. Passing the `-o` flag to this command will open a browser window for you and navigate to `http://localhost:4200/`.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. Use the `--prod` flag for a production build.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `testcafe` to execute the end-to-end tests via [testcafe].

To get a nice Html output, run `testcafe -r html:c:\path\to\results.html`

You can also run the following commands in series to test the different parts of the site, with or without the -r parameter.

## These tests test the administration section of the site
testcafe -f "Sidebar-Menu-Component"

## These sites test a section of the site

testcafe -f "Some Test Group"

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI README](https://github.com/angular/angular-cli/blob/master/README.md).

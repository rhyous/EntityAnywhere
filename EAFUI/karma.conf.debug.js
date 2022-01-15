module.exports = function (config) {
    config.set({
        basePath: '',
        frameworks: ['jasmine', '@angular-devkit/build-angular'],
        plugins: [
            require('karma-jasmine'),
            require('karma-chrome-launcher'),
            require('karma-junit-reporter'),
            require('karma-coverage-istanbul-reporter'),
            require('@angular-devkit/build-angular/plugins/karma'),
            require('karma-spec-reporter')
        ],
        client: {
            clearContext: false, // leave Jasmine Spec Runner output visible in browser
            // jasmine: {
            // random: false
            // }
        },
        coverageIstanbulReporter: {
            dir: require('path').join(__dirname, './coverage/ILS-Portal'),
            reports: ['html', 'lcovonly', 'text-summary', 'cobertura'],
            fixWebpackSourcePaths: true
        },
        junitReporter: {
            outputDir: 'tests/'
        },
        reporters: ['spec', 'progress', 'junit'],
        specReporter: {
            maxLogLines: 5, // limit number of lines logged per test
            suppressErrorSummary: false, // do not print error summary
            suppressFailed: false, // do not print information about failed tests
            suppressPassed: false, // do not print information about passed tests
            suppressSkipped: false, // do not print information about skipped tests
            showSpecTiming: true, // print the time elapsed for each spec
            failFast: false // test would finish with error when a first fail occurs.
        },
        port: 9877,
        colors: true,
        logLevel: config.LOG_INFO,
        autoWatch: true,
        browsers: ['Chrome'],
        // customLaunchers: {
        // ChromeHeadlessNoSandbox: {
        // base: 'ChromeHeadless',
        // flags: [
        // '--no-sandbox',
        // ]
        // }
        // },
        singleRun: false,
        restartOnFileChange: false,
        browserNoActivityTimeout: 400000
    });
};
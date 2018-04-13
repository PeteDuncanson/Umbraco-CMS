LazyLoad.js(
    [
        "lib/jquery/jquery.min.js",
        /* 1.2.23 */
        "lib/angular/1.2.23/angular.min.js",
        "lib/angular/1.2.23/angular-cookies.min.js",
        "lib/angular/1.2.23/angular-touch.min.js",
        "lib/angular/1.2.23/angular-mocks.js",
        "lib/angular/1.2.23/angular-sanitize.min.js",
        "lib/angular/1.2.23/angular-route.min.js",
        "lib/underscore/underscore-min.js",
        "lib/angular/angular-ui-sortable.js",
        "js/installer.app.js",
        "js/umbraco.directives.js",
        "js/umbraco.installer.js"
    ],
    function() {
        jQuery(document).ready(function() {
            angular.bootstrap(document, ["umbraco"]);
        });
    }
);

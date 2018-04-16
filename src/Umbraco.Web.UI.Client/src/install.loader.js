LazyLoad.js(
    [
        "lib/jquery/jquery.min.js",
        /* 1.2.9 */
        "lib/angular/1.2.9/angular.min.js",
        "lib/angular/1.2.9/angular-cookies.min.js",
        "lib/angular/1.2.9/angular-touch.min.js",
        //  "lib/angular/1.2.9/angular-mocks.js",   // TODO: Needed for production?
        "lib/angular/1.2.9/angular-sanitize.min.js",
        //  "lib/angular/1.2.9/angular-route.min.js", // TODO: Don't think we need this for the installer either, unless its loading everything the app needs for later?
        "lib/underscore/underscore-min.js",
        //  "lib/angular/angular-ui-sortable.min.js", // TODO: Is this even needed here?
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

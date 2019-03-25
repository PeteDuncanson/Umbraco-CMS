"use strict";

var config = require("../config");
var gulp = require("gulp");
const prettier = require("gulp-prettier");
var _ = require("lodash");
var MergeStream = require("merge-stream");

gulp.task("views", function() {
    var stream = new MergeStream();

    _.forEach(config.sources.views, function(group) {
        console.log("Prettifying all files in " + group.files + " in place");

        stream.add(
            gulp
                .src(group.files)
                .pipe(prettier())
                .pipe(gulp.dest("./pretty")) // This should write them inplace!
        );

        // TODO: If we inline the templates into the directives/components (as we should) then we won't need to do this copy anymore
        // However in that case the order we do the "prettifying" above will matter, we need to do it before we inline the templates
        console.log(
            "copying " +
                group.files +
                " to " +
                config.root +
                config.targets.views +
                group.folder
        );
        stream.add(
            gulp
                .src(group.files)
                .pipe(
                    gulp.dest(config.root + config.targets.views + group.folder)
                )
        );
    });

    return stream;
});

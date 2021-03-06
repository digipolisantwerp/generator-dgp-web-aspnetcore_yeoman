'use strict';
const path = require('path');
const assert = require('yeoman-assert');
const helpers = require('yeoman-generator').test;

describe('generator-dgp-web-aspnetcore:app', function () {
  before(function (done) {
    helpers.run(path.join(__dirname, '../generators/app'))
      .withOptions({someOption: true})
      .withPrompts({someAnswer: true})
      .on('end', done);
  });

  it('creates files', function () {
    assert.file([
      'dummyfile.txt'
    ]);
  });
});

(function (self) {
  'use strict'

  self.toParams = function () {
    var permissions = $('.filter #permissions').select2('data');
    var clusters = $('.filter #clusters').select2('data');
    var requestTypes = $('.filter #requestTypes').select2('data');
    var status = $('.filter #status').select2('data');

    function appendParam(p, key, val) {
      if (p.length > 0) {
        p += '&';
      }
      p += key + '=' + val;
      return p;
    }

    var params = '';

    if (permissions) {
      permissions.forEach((e, i) => {
        params = appendParam(params, 'permissions[' + i + ']', e.id);
      });
    }

    if (clusters) {
      clusters.forEach((e, i) => {
        params = appendParam(params, 'clusters[' + i + ']', e.id);
      });
    }

    if (requestTypes) {
      requestTypes.forEach((e, i) => {
        params = appendParam(params, 'requestTypes[' + i + ']', e.id);
      });
    }

    if (status) {
      params = appendParam(params, 'status', status[0].id);
    }

    return params;
  }

  function hide() {
    $('.filter #close').click();
  }

  function clear() {
    $('.filter #permissions').val(null).trigger('change');
    $('.filter #clusters').val(null).trigger('change');
    $('.filter #requestTypes').val(null).trigger('change');
    $('#status').val('0').trigger('change');
  }

  $(function () {
    var clusters = app.page.model.clusters.map(e => {
      var nodes = '';
      if (e.nodes) {
        e.nodes.forEach(n => {
          if (nodes.length > 0) {
            nodes += ', ';
          }
          nodes += n;
        });
      }

      return { id: e.id, text: e.district + (nodes ? ' (' + nodes + ')' : '') };
    });

    $('.filter #permissions').select2();

    $('.filter #clusters').select2({
      data: clusters
    });

    $('.select-multiple').select2();

    $('.select-single').select2();

    $('.filter #apply').click(function () {
      app.page.fetch();
      hide();
    });

    $(".filter #clear").click(function () {
      clear();
    });
  });

})(app.page.filter = app.page.filter || {});

$(document).ready(function () {

});
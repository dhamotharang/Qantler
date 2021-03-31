function Map(elem) {
  this.elem = elem;

  var self = this;

  elem.on('click', '.geo:not(.readonly) path:not(.disabled)', function () {
    var e = $(this);
    var id = e.attr('id');

    e.toggleClass('selected');
    if (e.hasClass('selected')) {
      self.onSelect(id);
    } else {
      self.onDeselect(id);
    }
  });
}

Map.prototype.selectAllNodesColor = function (ids, dataset) {
  var self = this;
  if (ids) {
    ids.forEach(e => {
      self.selectNodeColor(e, dataset);
    });
  }
}

Map.prototype.selectNodeColor = function (id, dataset) {
  var nodes = $.grep(dataset, function (e) { return e.nodes.indexOf(pad(id, 2)) > -1 });
  if (nodes.length > 0) {
    var p = this.elem.find('.geo #' + id);
    if (!p.hasClass(nodes[0].color)) {
      p.addClass(nodes[0].color);
    }
    if (!p.hasClass('readonly')) {
      p.addClass('readonly');
    }
  }
}

Map.prototype.selectAll = function (ids) {
  var self = this;
  if (ids) {
    ids.forEach(e => {
      self.select(e);
    });
  }
}

Map.prototype.select = function (id) {
  var p = this.elem.find('.geo #' + id);
  if (!p.hasClass('selected')) {
    p.addClass('selected');
  }
}

Map.prototype.onSelect = function (id) {
}

Map.prototype.onDeselect = function (id) {
}

Map.prototype.getSelectedIDs = function () {
  return this.elem.find('.geo path.selected').map(function () {
    return $(this).attr('id');
  });
}

function pad(str, max) {
  str = str.toString();
  return str.length < max ? pad("0" + str, max) : str;
}
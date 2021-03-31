(function (self) {
  'use strict'

  var data;
  var jobOrder;

  var controller;

  self.init = function (jo) {
    jobOrder = jo;
    data = jo.invitees || [];

    if (jobOrder.status != 200
        || jobOrder.assignedTo != app.identity.id) {
      $('.invites .action').removeClass('d-flex');
    }

    render();
  }

  function render() {
    var container = $('.widget.invites .list');
    container.empty();

    if (!data.length) {
      $('.widget.invites .widget-placeholder').removeClass('d-none');

      var placeholder = jobOrder.status == 200
        && jobOrder.assignedTo == app.identity.id
        ? 'No invites yet.<br>Tap "Invite Officer" to invite.'
        : 'No invites sent.';

      $('.widget.invites .widget-placeholder span').html(placeholder);
      return;
    }
    $('.widget.invites .widget-placeholder').addClass('d-none');

    data = data.sort((a, b) => a.name.localeCompare(b.name));

    var itemTemplate = $('.invites-template .item').prop('outerHTML');

    var canEdit = jobOrder.status == 200
      && jobOrder.assignedTo == app.identity.id;

    data.forEach(e => {
      container.append(itemTemplate
        .replaceAll('{id}', e.id)
        .replaceAll('{name}', app.utils.userContext(e.id, e.name))
        .replaceAll('{initial}', app.utils.initials(e.name))
        .replaceAll('{color}', 'bg-' + app.utils.randomColor())
        .replaceAll('{actionVisibility}', canEdit ? '' : 'd-none'));
    });
  }

  function initInviteOfficer() {
    app.select.officer.onSelect = onOfficerSelected;

    var skip = data.map(e => e.id);
    skip.push(jobOrder.assignedTo);

    app.select.officer.init({
      key: 'officers',
      title: 'Invite Officer',
      actionText: 'Invite',
      permissions: [],
      skip: skip
    });
  }

  function onOfficerSelected(o) {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Inviting officer. Please wait...');

    fetch('/api/jobOrder/' + jobOrder.id + '/invitee', {
      method: 'POST',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      body: JSON.stringify(o),
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(res => {
        data.push(o);

        render();

        Swal.fire(
          'Invite sent!',
          'Invite successfully sent.',
          'success'
        ).then(r => {
          window.location.reload(true);
        });

      }).catch(app.http.catch);
  }

  function removeInvite(id) {
    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Removing invite. Please wait...');

    fetch('/api/jobOrder/' + jobOrder.id + '/invitee/' + id, {
      method: 'DELETE',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(res => {
        data = data.filter(e => e.id != id);

        render();

        Swal.fire(
          'Invite cancelled!',
          'Invite successfully cancelled.',
          'success'
        ).then(r => {
          window.location.reload(true);
        });

      }).catch(app.http.catch);
  }

  $(function () {

    $('.invites .action a').click(function () {
      initInviteOfficer();
    });

    $('.invites').on('click', 'button', function () {
      removeInvite($(this).data('id'));
    });

  });

})(app.page.invites = app.page.invites || {});
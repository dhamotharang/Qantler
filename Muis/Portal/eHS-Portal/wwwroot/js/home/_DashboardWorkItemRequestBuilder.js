(function (self) {

  function appendParam(p, key, val) {
    if (p.length > 0) {
      p += '&';
    }
    p += key + '=' + val;
    return p;
  }

  self.buildRequest = function (hasPermission, options, controller) {
    if (!hasPermission) {
      return Promise.resolve([]);
    }

    var params = '';

    if (options.id) {
      params = appendParam(params, 'assingedTo', options.id);
    }

    if (options.statuses) {
      options.statuses.forEach((e, i) => {
        params = appendParam(params, 'status[' + i + ']', e);
      });
    }

    return fetch('/api/request/index?' + params, {
      method: 'GET',
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
        return Promise.resolve(
          res.map(e => {
            return {
              Type: 'Request',
              RefID: e.refID,
              Id: e.id,
              Item: e.customerCode?.value + ', ' + e.customerName + ' @ ' + app.utils.formatPremise(e.premises[0]),
              Status: e.status,
              StatusText: e.statusText,
              LastAction: app.utils.formatDate(e.lastAction, true)
            }
          })
        );
      })
      .catch(err => Promise.resolve([]));
  }

  self.buildPayment = function (hasPermission, options, controller) {
    if (!hasPermission) {
      return Promise.resolve([]);
    }

    var params = '';

    if (options.status) {
      params = appendParam(params, 'status', options.status);
    }

    return fetch('/api/payments/index/list?' + params, {
      method: 'GET',
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

        return Promise.resolve(
          res.map(e => {
            return {
              Type: 'Payments',
              RefID: e.id,
              Id: e.id,
              Item: '<div class="badge badge-outline-info">Payment</div> ' + e.name + ' @ ' + e.totalAmount,
              Status: e.status,
              StatusText: e.statusText,
              LastAction: app.utils.formatDate(e.paidOn, true)
            }
          })
        );

      })
      .catch(err => Promise.resolve([]));
  }

  self.buildRFA = function (hasPermission, options, controller) {
    if (!hasPermission) {
      return Promise.resolve([]);
    }

    var params = '';

    if (options.id) {
      params = appendParam(params, 'raisedBy', options.id);
    }

    if (options.statuses) {
      options.statuses.forEach((e, i) => {
        params = appendParam(params, 'status[' + i + ']', e);
      });
    }

    return fetch('/api/rfa/list?' + params, {
      method: 'GET',
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

        return Promise.resolve(
          res.map(e => {
            return {
              Type: 'RFA',
              RefID: e.id,
              Id: e.id,
              Item: '<div class="badge badge-outline-warning">RFA</div> ' + app.utils.titleCase(e.request.customerName),
              Status: e.status,
              StatusText: e.statusText,
              LastAction: app.utils.formatDate(e.modifiedOn, true)
            }
          })
        );

      })
      .catch(err => Promise.resolve([]));
  }

})(app.page.workitem.builder = app.page.workitem.builder || {});
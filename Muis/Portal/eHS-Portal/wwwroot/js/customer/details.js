(function (self) {
  'use strict'
  var data;

  var controller;

  function invalidateCustomerDetails() {

    var customerCode = data.code;

    var groupCode = data.groupCode;

    var codeText = customerCode && customerCode.id > 0 ? ('<b>' + customerCode.value + '</b>, ' + customerCode.text) : 'No code assigned';
    var groupCodeText = groupCode && groupCode.id > 0 ? ('<b>' + groupCode.value + '</b>, ' + groupCode.text) : 'No group code assigned';

    var parent = 'No parent assigned';
    if (data.parent
      && data.parent.name) {
      parent = app.utils.titleCase(data.parent.name);
    }
    var office = 'No officer assigned';
    if (data.officer
      && data.officer.name) {
      office = app.utils.titleCase(data.officer.name);
    }

    $('.customer-info #code').html(codeText);
    $('.customer-info #code-group').html(groupCodeText);
    $('.customer-info #parent-code').html(parent);
    $('.customer-info #officer-code').html(office);

    $('#codeBtn').removeClass('d-none');
    $('#groupCodeBtn').removeClass('d-none');
    $('#parentBtn').removeClass('d-none');
    $('#officerBtn').removeClass('d-none');

    $('.customer-info #name').html(app.utils.titleCase(data.name));
    $('.customer-info #altid').html(data.altID);
  }

  function initSelectCode(type) {
    var skip = [];

    if (type == 0 && data.code) {
      skip.push(data.code.id);
    } else if (type == 1 && data.groupCode) {
      skip.push(data.groupCode.id);
    }

    app.select.code.init({
      type: type,
      skip: skip,
      defaults: {
        text: app.utils.titleCase(data.name)
      }
    });

    app.select.code.create.onCodeCreated = function (c) {
      app.select.code.reset();
      app.select.code.dismiss();
      app.select.code.create.dismiss();
      setCustomerCode(c);
    }

    app.select.code.onSelect = setCustomerCode;
  }

  function setCustomerCode(c) {

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Updating. Please wait');

    fetch('/api/customer/' + data.id + '/code', {
      method: 'PUT',
      cache: 'no-cache',
      credentials: 'include',
      signal: controller.signal,
      body: JSON.stringify(c),
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json'
      }
    }).then(app.http.errorInterceptor)
      .then(res => res.json())
      .then(res => {
        app.dismissProgress();

        if (c.type == 0) {
          data.code = c;
        } else {
          data.groupCode = c;
        }
        invalidateCustomerDetails();

      }).catch(app.http.catch);
  }

  function setParent(c) {

    if (controller) {
      controller.abort();
    }
    controller = new AbortController();

    app.showProgress('Updating. Please wait');

    fetch('/api/customer/' + data.id + '/parent/' + c.id, {
      method: 'PUT',
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
        app.dismissProgress();

        data.parent = c;
        invalidateCustomerDetails();

      }).catch(app.http.catch);
  }

  $(function () {
    data = self.model.data;

    invalidateCustomerDetails();

    $('.customer-other button[id="codeBtn"]').click(function () {
      initSelectCode(0);
    });

    $('.customer-other button[id="groupCodeBtn"]').click(function () {
      initSelectCode(1);
    });

    $('.customer-other button[id="officerBtn"]').click(function () {
      app.page.officer.init(data, function (o) {
        var office = 'No officer assigned';
        if (o
          && o.name) {
          office = app.utils.titleCase(o.name);
        }
        $('.customer-info #officer-code').html(office);
      });
    });

    $('.customer-other button[id="parentBtn"]').click(function () {
      var skip = [];
      if (data.parent) {
        skip.push(data.parent.id);
      }

      skip.push(data.customerID);

      app.select.customer.init({
        skip: skip,
        title: 'Select Parent'
      });

      app.select.customer.onSelect = function (c) {
        setParent(c);
      };
    });

    self.recentrequest.init(self.model.id);
    self.recentpayment.init(self.model.id);
    self.premise.init(self.model.id);
  });

})(app.page);
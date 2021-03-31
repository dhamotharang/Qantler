(function (app) {

  function initState() {
    var path = window.location.pathname.toLowerCase();

    if (path.startsWith('/request')) {
      $('.sidebar .nav-item.resources').addClass('active');
      $('.sidebar .nav-item.resources .nav-link.request').addClass('active');
    } else if (path.startsWith('/rfa')) {
      $('.sidebar .nav-item.resources').addClass('active');
      $('.sidebar .nav-item.resources .nav-link.rfa').addClass('active');
    } else if (path.startsWith('/joborder')) {
      $('.sidebar .nav-item.resources').addClass('active');
      $('.sidebar .nav-item.resources .nav-link.job-order').addClass('active');
    } else if (path.startsWith('/periodicinspection')) {
      $('.sidebar .nav-item.resources').addClass('active');
      $('.sidebar .nav-item.resources .nav-link.pi-poll').addClass('active');
    } else if (path.startsWith('/delivery')) {
      $('.sidebar .nav-item.resources').addClass('active');
      $('.sidebar .nav-item.resources .nav-link.certificate-delivery').addClass('active');
    } else if (path.startsWith('/batch/certificate')) {
      $('.sidebar .nav-item.resources').addClass('active');
      $('.sidebar .nav-item.resources .nav-link.certificate-printout').addClass('active');
    } else if (path.startsWith('/halallibrary')) {
      $('.sidebar .nav-item.halal-library').addClass('active');
    } else if (path.startsWith('/customer')) {
      $('.sidebar .nav-item.customers').addClass('active');
    } else if (path.startsWith('/mufti')) {
      $('.sidebar .nav-item.mufti').addClass('active');
    } else if (path.startsWith('/case')) {
      $('.sidebar .nav-item.case').addClass('active');
    } else if (path.startsWith('/payments')) {
      $('.sidebar .nav-item.finance').addClass('active');
      $('.sidebar .nav-item.finance .nav-link.payments').addClass('active');
    } else if (path.startsWith('/bill')) {
      $('.sidebar .nav-item.finance').addClass('active');
      $('.sidebar .nav-item.finance .nav-link.invoice').addClass('active');
    } else if (path.startsWith('/transactioncode')) {
      $('.sidebar .nav-item.finance').addClass('active');
      $('.sidebar .nav-item.finance .nav-link.transcation-code').addClass('active');
    } else if (path.startsWith('/user')) {
      $('.sidebar .nav-item.security').addClass('active');
      $('.sidebar .nav-item.security .nav-link.users').addClass('active');
    } else if (path.startsWith('/settings')) {
      $('.sidebar .nav-item.settings').addClass('active');
      $('.sidebar .nav-item.settings .nav-link.system-config').addClass('active');
    } else if (path.startsWith('/checklist/scheme')) {
      $('.sidebar .nav-item.settings').addClass('active');
      $('.sidebar .nav-item.settings .nav-link.checklist').addClass('active');
    } else if (path.startsWith('/master/email')) {
      $('.sidebar .nav-item.settings').addClass('active');
      $('.sidebar .nav-item.settings .nav-link.email-template').addClass('active');
    } else if (path.startsWith('/master/letter')) {
      $('.sidebar .nav-item.settings').addClass('active');
      $('.sidebar .nav-item.settings .nav-link.letter-template').addClass('active');
    } else if (path.startsWith('/cluster')) {
      $('.sidebar .nav-item.settings').addClass('active');
      $('.sidebar .nav-item.settings .nav-link.cluster').addClass('active');
    } else if (path.startsWith('/master')) {
      $('.sidebar .nav-item.settings').addClass('active');
      $('.sidebar .nav-item.settings .nav-link.master').addClass('active');
    }
  }

  function setupView() {
    if (app.hasPermission(0)
      || app.hasPermission(1)
      || app.hasPermission(2)
      || app.hasPermission(3)
      || app.hasPermission(4)
      || app.hasPermission(5)
      || app.hasPermission(6)
      || app.hasPermission(7)
      || app.hasPermission(21)
      || app.hasPermission(30)) {
      $('.sidebar .nav-item.resources').removeClass('d-none');
      $('.sidebar .nav-item.request').removeClass('d-none');
    }

    if (app.hasPermission(0)
      || app.hasPermission(1)
      || app.hasPermission(2)
      || app.hasPermission(21)
      || app.hasPermission(30)) {
      $('.sidebar .nav-item.resources').removeClass('d-none');
      $('.sidebar .nav-item.rfa').removeClass('d-none');
    }

    if (app.hasPermission(0)
      || app.hasPermission(1)
      || app.hasPermission(2)
      || app.hasPermission(3)
      || app.hasPermission(4)
      || app.hasPermission(5)
      || app.hasPermission(6)
      || app.hasPermission(7)) {
      $('.sidebar .nav-item.resources').removeClass('d-none');
      $('.sidebar .nav-item.job-order-category').removeClass('d-none');
      $('.sidebar .nav-item.job-order').removeClass('d-none');
    }

    if (app.hasPermission(9)) {
      $('.sidebar .nav-item.resources').removeClass('d-none');
      $('.sidebar .nav-item.job-order-category').removeClass('d-none');
      $('.sidebar .nav-item.pi-poll').removeClass('d-none');
    }

    if (app.hasPermission(4)) {
      $('.sidebar .nav-item.resources').removeClass('d-none');
      $('.sidebar .nav-item.certificate-category').removeClass('d-none');
      $('.sidebar .nav-item.certificate-delivery').removeClass('d-none');
      $('.sidebar .nav-item.certificate-printout').removeClass('d-none');
    }

    if (app.hasPermission(10)
      || app.hasPermission(11)
      || app.hasPermission(12)) {
      $('.sidebar .nav-item.mufti').removeClass('d-none');
    }

    if (app.hasPermission(13)
      || app.hasPermission(14)) {
      $('.sidebar .nav-item.finance').removeClass('d-none');
      $('.sidebar .nav-item.payments').removeClass('d-none');
      $('.sidebar .nav-item.invoice').removeClass('d-none');
    }

    if (app.hasPermission(15)
      || app.hasPermission(16)) {
      $('.sidebar .nav-item.finance').removeClass('d-none');
      $('.sidebar .nav-item.finance-settings-category').removeClass('d-none');
      $('.sidebar .nav-item.transaction-code').removeClass('d-none');
    }

    if (app.hasPermission(17)
      || app.hasPermission(18)) {
      $('.sidebar .nav-item.security').removeClass('d-none');
      $('.sidebar .nav-item.users').removeClass('d-none');
    }

    if (app.hasPermission(19)
      || app.hasPermission(20)) {
      $('.sidebar .nav-item.settings').removeClass('d-none');
    }

    if (app.hasPermission(23)
      || app.hasPermission(24)) {
      $('.sidebar .nav-item.case').removeClass('d-none');
    }

    if (app.hasPermission(40)
      || app.hasPermission(41)) {
      $('.sidebar .nav-item.halal-library').removeClass('d-none');
    }

    if (app.hasPermission(50)
      || app.hasPermission(51)) {
      $('.sidebar .nav-item.customers').removeClass('d-none');
    }
  }

  $(function () {
    initState();
    setupView();
  });

})(app);
(function () {

  this.createLogElement = function (user, action, notes, time) {
    var result = '<li class="timeline-item">' +
      '<p class="timeline-content">' +
      (user ? ('<code>' + user + '</code> ') : '') +
      action +
      '</p>';

    if (!app.utils.isNullOrEmpty(notes)) {
      result += '<p class="timeline-notes text-muted">' + notes.replace(/(?:\r\n|\r|\n)/g, '<br>') + '</p>';
    }

    result += '<p class="event-time">' + time + '</p>' +
      '</li>';

    return result;
  }

  this.createHashtag = function (type, refID) {
    if (app.utils.isNullOrEmpty(type)
      || app.utils.isNullOrEmpty(refID)) {
      return '';
    }

    var tag = '';
    switch (type) {
      case 1: // Request
        tag = 'request';
        break;
      case 2: // RFA
        tag = 'rfa';
        break;
      case 3: // JobOrder
        tag = 'jo';
        break;
    }

    return tag.length == 0
      ? ''
      : ('<span class="hashtag" data-tag="' + tag + '" data-id="' + refID + '">#' + tag + refID + '</span>');
  }

  this.formatScheme = function (scheme, subScheme) {
    return scheme + (app.utils.isNullOrEmpty(subScheme) ? '' : ('&nbsp;(<b>' + subScheme + '</b>)'));
  }

  this.gradeLabel = function (grade) {
    if (app.utils.isNullOrEmpty(grade)) {
      return '';
    }

    switch (grade) {
      case 1:
        return 'A';
      case 2:
        return 'B';
      case 3:
        return 'C';
      default:
        return '';
    }
  }

  this.paymentStatusColor = function (status) {
    switch (status) {
      case 100: // draft
      case 200: // pending
        return 'dark';
      case 300: // processed
        return 'success'
      case 400: // rejected
      case 500: // cancelled
        return 'danger';
      case 600: // expired
        return 'warning';
      case 700: // pending payment
        return 'info';
      default:
        return 'dark';
    }
  }

  this.rfaStatusColor = function (status) {
    switch (status) {
      case 1: // draft
      case 100: // pending
        return 'dark';
      case 200: // pendingReview
        return 'warning'
      case 300: // closed
        return 'success'
      case 400: // expired
        return 'danger';
      default:
        return 'dark';
    }
  }

  this.schemeInitial = function (scheme) {
    switch (scheme) {
      case "Eating Establishment":
        return "EE";
      case "Food Preparation Area":
        return "FPA";
      case "Food Manufacturing":
        return "FM";
      case "Poultry":
        return "PA";
      case "Poultry Abattoir":
        return "PA";
      case "Endorsement":
        return "EDM";
      case "Storage Facility":
        return "SF";
      case "Product":
        return "Product";
      case "Whole Plant":
        return "WP";
      default:
        return "";
    }
  }

  this.caseStatusColor = function (status) {
    switch (status) {
      case 300: // PendingShowCause
      case 200: // PendingInspection
      case 900: // PendingPayment
      case 400: // ShowCauseForApproval
      case 500: // PendingAcknowledgement
      case 800: // PendingSanctionLetter
      case 1000: // CertificateCollection
      case 1100: // PendingReinstateInspection
        return 'primary';
      case 100: // Open
        return 'secondary'
      case 600: // PendingFOC
      case 700: // FOCForApproval
        return 'warning'
      case 1200: // PendingAppeal
      case 1400: // Dismissed
        return 'info'
      case 1300: // Closed
        return 'success'
      default:
        return 'dark';
    }
  }

}).apply(app.common = app.common || {});
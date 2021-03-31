var app = app || {};

(function () {
  'use strict';

  this.initials = function (text) {
    if (!text) return '';

    var chunks = text.split(' ');
    var result = chunks[0].substring(0, 1);

    if (chunks.length > 1) {
      result += chunks[1].substring(0, 1)
    }
    return result.toUpperCase();
  };

  this.certified = function (iscertified) {

    var result = '';

    if (!iscertified) {
      return result = '';
    } else {
      result = 'Certified';
    }

    return result;
  };

  this.userContext = function (id, name) {
    return id === app.identity.id ? 'You' : name;
  }

  this.randomColor = function () {
    var colors = ["primary", "secondary", "success", "danger", "warning", "info", "dark"];
    return colors[Math.floor(Math.random() * colors.length)];
  };

  var colorIndex = -1;
  this.seriesColor = function () {
    var colors = ["primary", "secondary", "success", "danger", "warning", "info", "dark"];
    return colors[++colorIndex % colors.length];
  };

  this.requestStatusColor = function (status) {
    switch (status) {
      case 100: // draft
      case 200: // open
        return 'dark';
      case 300: // inspection
        return 'primary'
      case 400: // pending approval
        return 'warning';
      case 500: // approved
        return 'success';
      case 550: // For Mufti Acknowledgement
      case 600: // pending bill
      case 700: // pending payment
        return 'info';
      case 800: // issuance
      case 900: // closed
        return 'success';
      case 1000: // rejected
      case 1100: // cancelled
        return 'danger';
      default:
        return 'dark';
    }
  };

  this.certificateStatusColor = function (status) {
    switch (status) {
      case 100: // active
        return 'success';
      case 200: // cancelled
      case 500: // suspended
      case 400: // expired
      case 600: // revoked
        return 'danger';
      case 300: // invalid
        return 'warning';
      default:
        return 'dark';
    }
  };

  this.rfaStatusColor = function (status) {
    switch (status) {
      case 100: // open
        return 'secondary';
      case 200: // pending review
        return 'warning';
      case 300: // closed
        return 'success';
      case 400: // expired
        return 'danger';
      default:
        return 'dark';
    }
  };

  this.halalLibraryStatusColor = function (status) {
    switch (status) {
      case 0:
        return 'dark';
      case 1:
        return 'success';
      default:
        return 'dark';
    }
  };

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
  };

  this.premiseColor = function (type) {
    switch (type) {
      case 0: // main
        return 'primary';
      case 1: // organization
        return 'info';
      case 2: // mailing
        return 'warning';
      case 3: // billing
        return 'success';
      case 4: // shipping
        return 'secondary';
      default:
        return 'dark';
    }
  }

  this.timeAgo = function (time) {
    var now = moment.utc();
    var timeInUtc = moment.utc(time);
    var seconds = now.diff(timeInUtc, 'seconds');
    if (seconds < 10) {
      return 'Just now';
    }

    if (now.isSame(timeInUtc, 'day')) {
      return 'Today, ' + timeInUtc.local().format('h:mm A');
    }

    var yesterday = now.add(-1, 'days');
    if (yesterday.isSame(timeInUtc, 'day')) {
      return 'Yesterday, ' + timeInUtc.local().format('h:mm A');
    }

    return timeInUtc.local().format('Do MMM YY h:mm A')
  };

  this.dayAgo = function (time) {
    var now = moment.utc();
    var timeInUtc = moment.utc(time);

    if (now.isSame(timeInUtc, 'day')) {
      return 'Today';
    }

    var yesterday = now.add(-1, 'days');
    if (yesterday.isSame(timeInUtc, 'day')) {
      return 'Yesterday';
    }

    return timeInUtc.local().format('Do MMM YYYY')
  };

  this.fileExtensionIcon = function (ext) {
    switch (ext.toLowerCase()) {
      case 'pdf':
        return 'mdi-file-pdf';
      case 'doc':
      case 'docx':
        return 'mdi-file-word';
      case 'xls':
      case 'xlsx':
      case 'csv':
        return 'mdi-file-excel';
      case 'png':
      case 'bmp':
      case 'jpg':
      case 'gif':
      case 'jpeg':
        return 'mdi-file-image';
      default:
        return 'mdi-paperclip';
    }
  };

  this.riskColor = function (risk) {
    switch (risk) {
      case 999:
        return 'risk-uncategorized';
      case 500:
        return 'risk-non-halal';
      case 400:
        return 'risk-high';
      case 300:
        return 'risk-medium-high';
      case 200:
        return 'risk-medium-low';
      case 100:
        return 'risk-low';
    }
  }

  this.formatFileSize = function (size) {
    if (size < 1024) {
      return size + ' bytes';
    }

    var i = -1;
    var units = ['KB', 'MB', 'GB', 'TB'];
    do {
      size = size / 1024;
      i++;
    } while (size > 1024);

    return size.toFixed(2).replace(".00", "") + ' ' + units[i];

  };

  this.formatPremise = function (prem, includeArea = false) {
    var result = '';

    function append(val, separator) {
      if (!val || val.trim().length === 0) {
        return;
      }
      if (result.length > 0 && separator) {
        result += separator;
      }
      result += ' ' + val;
    }

    if (includeArea
      && prem.area) {
      append('<span class="text-danger">' + prem.area + 'sqm.</span>');
    }
    append(prem.schedule);

    append(prem.blockNo);
    append(app.utils.titleCase(prem.address1));
    append(app.utils.titleCase(prem.address2));

    var floorUnit = '';
    if (prem.floorNo) {
      floorUnit += prem.floorNo;
    }

    if (prem.unitNo) {
      if (floorUnit.length > 0) {
        floorUnit += '-';
      }
      floorUnit += prem.unitNo;
    }

    if (floorUnit.length > 0) {
      if (!floorUnit.startsWith("#")) {
        floorUnit = '#' + floorUnit;
      }
      append(floorUnit);
    }

    append(app.utils.titleCase(prem.buildingName));
    append(app.utils.titleCase(prem.city), ',');
    append(app.utils.titleCase(prem.province), ',');
    append(app.utils.titleCase(prem.country), ',');

    if (prem.postal) {
      append('<span class="text-primary font-weight-bold">' + prem.postal + '</span>', '');
    }

    return result;
  };

  this.formatDate = function (d, fromUtc) {
    if (!d) {
      return '';
    }

    if (fromUtc) {
      return moment.utc(d).local().format('DD MMM \'YY');
    } else {
      return moment(d).format('DD MMM \'YY');
    }
  }

  this.formatTime = function (d, fromUtc) {
    if (!d) {
      return '';
    }

    if (fromUtc) {
      return moment.utc(d).local().format('h:mm A');
    } else {
      return moment(d).format('h:mm A');
    }
  }

  this.formatDateTime = function (d, fromUtc) {
    if (!d) {
      return '';
    }

    if (fromUtc) {
      return moment.utc(d).local().format('DD MMM \'YY h:mm A');
    } else {
      return moment(d).format('DD MMM \'YY h:mm A');
    }
  }

  this.addTimeToDate = function (d, t) {
    if (!d || !t) {
      return '';
    }

    return moment(moment(d).format('DD MMM \'YY') + ' ' + t, 'DD MMM \'YY hh:mm A');
  }

  this.titleCase = function (string) {
    if (!string || string.trim().length === 0) {
      return string;
    }

    var sentence = string.toLowerCase().split(' ');
    for (var i = 0; i < sentence.length; i++) {
      if (sentence[i][0] != undefined) {
        sentence[i] = sentence[i][0].toUpperCase() + sentence[i].slice(1);
      }

    }
    return sentence.join(' ');
  }

  this.hide = function (e) {
    if (!e.hasClass('d-none')) {
      e.addClass('d-none');
    }
  };

  this.show = function (e) {
    if (e.hasClass('d-none')) {
      e.removeClass('d-none');
    }
  };

  this.hashtagToHtml = function (str) {
    return str.replace(/(#[a-z\d-]+)/ig, function (x) {
      return '<span class="hashtag" data-tag="' + x + '">' + x + '</span>';
    });
  }

  this.isNullOrEmpty = function (val) {
    return val == null || !val;
  }

  this.emptyIfNullOrEmpty = function (val) {
    return val == null || !val ? '' : val;
  }

  this.escapeHtml = function (val) {
    if (app.utils.isNullOrEmpty(val)) {
      return val;
    }

    return val.replace(/[&<>"]/g, function (tag) {
      var toReplace = {
        '&': '&amp;',
        '<': '&lt;',
        '>': '&gt;',
        '"': '&quot;'
      };
      return toReplace[tag] || tag;
    });
  }

  this.formatShortDate = function (d, fromUtc) {
    if (!d) {
      return '';
    }

    if (fromUtc) {
      return moment.utc(d).local().format('DD MMM YYYY');
    } else {
      return moment(d).format('DD MMM YYYY');
    }
  }

  this.isValidEmail = function (e) {
    if (!e || e.trim().length == 0) {
      return false;
    }

    return e.match(/^[a-zA-Z0-9._]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/) != null;
  }

  this.certDeliveryStatusColor = function (status) {
    switch (status) {
      case 100:
        return 'dark';
      case 200:
        return 'primary';
      case 300:
        return 'danger';
      default:
        return 'dark';
    }
  };

  this.formatCustomer = function (code, name) {
    return code + ', ' + name;
  };

  this.formatShortDateWithComma = function (d, fromUtc) {
    if (!d) {
      return '';
    }

    if (fromUtc) {
      return moment.utc(d).local().format('MMM DD, YYYY');
    } else {
      return moment(d).format('MMM DD, YYYY');
    }
  }

}).apply(app.utils = app.utils || {});
(function (self) {
  'use strict';

  var cache = {};
  var controller;

  self.AttachmentSizeFormat = function () {
    $('.file-size-format').each(function () {
      $(this).html(app.utils.formatFileSize($(this).html()));
      $(this).removeClass('d-none');
    });
  };

  self.OffenderFormat = function () {
    $('.expires-on').each(function () {
      $(this).removeClass('d-none');
      $(this).html(app.utils.formatShortDateWithComma($(this).html()));
    });
  };

  self.ActivityFormat = function () {
    $('.section-time-frame').each(function () {
      $(this).removeClass('d-none');
      $(this).html(app.utils.dayAgo($(this).html(), true));
    });

    $('.activity-time').each(function () {
      $(this).removeClass('d-none');
      $(this).html(app.utils.formatTime($(this).html(), true));
    });
  };

  self.PremisesFormat = function () {
    $('.premise-value').each(function () {
      var id = $(this).attr('id');
      var e = app.utils.formatPremise(self.model.premises.find(x => x.id == id))
      $(this).html(e);
      $(this).removeClass('d-none');
    });
  };

  self.initNote = function () {
    self.notes.init(self.model.id)
  };

  self.initSchedule = function () {
    self.schedule.init(self.model);
  };

  self.initAcknowledgeCause = function () {
    self.AcknowledgeCause.init(self.model.id);
  };

  function init() {
    self.AttachmentSizeFormat();
    self.OffenderFormat();
    self.ActivityFormat();
    self.PremisesFormat();

    if (self.model.managedByID == app.identity.id) {
      $('#add-notes-link').removeClass('d-none');
    }

    if ((self.model.status == 100
      || (self.model.status == 200
        && (self.model.minorStatus == 210
          || self.model.minorStatus == 230)))
      && self.model.managedByID == app.identity.id
      && self.model.sanction == null) {
      $('#add-Inspection').removeClass('d-none');

      var option = {
        title: 'Schedule an Inspection',
        type: 2
      };

      self.schedule.init(self.model, option);
    }

    if (self.model.managedByID == app.identity.id
      && ((self.model.status == 300
        && self.model.minorStatus == null)
        || (self.model.status == 200
          && self.model.minorStatus == 210))) {
      $('#add-draft-letter').removeClass('d-none');
    }

    if (self.model.managedByID == app.identity.id
      && self.model.status == 500) {
      $('#add-acknowledgeShowCause').removeClass('d-none');
    }

    if (self.model.managedByID == app.identity.id
      && self.model.status == 600) {
      $('#add-foc').removeClass('d-none');
      if ($('.file-icon[data-status="Final"][data-type-id="401"]').length > 0) {
        $('#show-foc').html('Update Case File');
      }
    }

    if (self.model.assignedToID == app.identity.id
      && self.model.status == 700) {
      $('#add-focReview').removeClass('d-none');
    }

    if (self.model.managedByID == app.identity.id
      && self.model.status == 750) {
      $('#add-focDecision').removeClass('d-none');
    }

    if (self.model.managedByID == app.identity.id
      && self.model.status == 800
      && self.model.minorStatus == null) {
      $('#add-sanction').removeClass('d-none');
    }

    if (self.model.managedByID == app.identity.id
      && self.model.status == 1000
      && self.model.minorStatus == null) {
      $('#add-collectcertificate').removeClass('d-none');
    }

    if (self.model.status == 1100
      && (self.model.minorStatus == null
        || self.model.minorStatus == 210
        || self.model.minorStatus == 230)
      && self.model.managedByID == app.identity.id) {
      $('#add-reinstate').removeClass('d-none');

      var option = {
        title: 'Schedule Reinstate Inspection',
        type: 3
      };

      self.schedule.init(self.model, option);
    }

    if (self.model.managedByID == app.identity.id
      && self.model.status == 1100
      && self.model.minorStatus == 210) {
      $('#add-reinstateDecision').removeClass('d-none');
    }

    if ((self.model.status > 800
      || (self.model.status == 100
        && self.model.sanction != null))
      && self.model.managedByID == app.identity.id
      && self.model.otherStatus != 1200
      && ((self.model.status == 1300
        && self.model.sanction == 2
        && new Date(app.page.model.certificates[0].expiresOn) < new Date())
        || (self.model.status < 1300))) {
      $('#add-caseAppeal').removeClass('d-none');
    }

    if (self.model.managedByID == app.identity.id
      && self.model.otherStatus == 1200) {
      $('#add-appealDecision').removeClass('d-none');
    }

    if (self.model.managedByID == app.identity.id
      && self.model.sanction == 1
      && (self.model.status == 1000
        || (self.model.status == 900
          && (self.model.minorStatus == null
            || self.model.minorStatus == 930)))) {
      $('#add-fileCaseToCourt').removeClass('d-none');
    }

    if (self.model.managedByID == app.identity.id
      && self.model.status == 1250) {
      $('#add-caseVerdict').removeClass('d-none');
    }

    if (self.model.managedByID == app.identity.id
      && (self.model.status == 100
        || (self.model.status == 200
          && (self.model.minorStatus == 210
            || self.model.minorStatus == 230)))) {
      $('#add-caseDismiss').removeClass('d-none');
    }

    if (self.model.managedByID == app.identity.id
      && self.model.certificates != null
      && self.model.certificates.filter(x => x.status == 100).length > 0
      && self.model.status == 200
      && self.model.minorStatus == 210) {
      $('#add-caseImmediatesuspension').removeClass('d-none');
    }

    if (self.model.managedByID == app.identity.id
      && self.model.status == 1275) {
      $('#add-caseReinstateCertificate').removeClass('d-none');
    }

    if (self.model.managedByID == app.identity.id
      && self.model.status == 1300
      && ([2, 3, 4].filter(x => x == self.model.sanction).length > 0)
      && (self.model.certificates != null)
      && (self.model.certificates.filter(x => new Date(x.expiresOn) < new Date()))) {
      $('#add-caseReopen').removeClass('d-none');
    }

    if (self.model.managedByID == app.identity.id
      && self.model.status == 100
      && self.model.sanction != null) {
      $('#add-caseClose').removeClass('d-none');
    }

    $('.quick-links li:not(.d-none):last').css('border-right', 'none')

    self.showCause.init(self.model);

    self.focLetter.init(self.model);

    self.focReview.init(self.model);

    self.focDecision.init(self.model);

    self.sanctionLetter.init(self.model);

    self.payment.init(self.model);

    self.collectcertificate.init(self.model);

    self.reinstateDecision.init(self.model);

    self.reinstatecertificate.init(self.model);

    self.appeal.init(self.model);

    self.appeal.decision.init(self.model);

    self.court.init(self.model);

    self.verdict.init(self.model);

    self.dismiss.init(self.model);

    self.immediateSuspension.init(self.model);

    self.caseClose.init(self.model.id);

    self.caseReopen.init(self.model.id);
  };

  async function fetchLetterByID(id) {
    if (controller) {
      controller.abort();
    }

    controller = new AbortController();

    app.showProgress('Processing. Please wait...');

    await fetch('/api/caseletter/' + id, {
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
      .then(r => {
        if (r) {
          cache[id] = r;
          app.dismissProgress();
          return r;
        }
      })
      .catch(err => {
        app.http.catch
      });
  };

  function openFinalLetter(letterID) {
    var letter = cache[letterID];
    if (!letter) {
      fetchLetterByID(letterID).then(x => {
        openFinalLetter(letterID);
      });
    }
    else {
      app.page.letterReadonly.init(letter, { title: letter.typeText });
      $('#viewLetter').modal({ show: true });
    }
  }

  function openDraftLetter(_letter) {
    var letter = cache[_letter.id];
    if (!letter
      && _letter.id > 0) {
      fetchLetterByID(_letter.id).then(x => {
        openDraftLetter(cache[_letter.id]);
      });
    }
    else {
      if (_letter.id == 0) {
        letter = _letter;
      }
      if (letter.type == "400") {
        app.page.showCause.onLetterTapped(letter);
      }
      else if (letter.type == "401"
        && self.model.status == 600) {
        var focActivity = $('.file-icon[data-status="Final"][data-type-id="401"]');
        if (focActivity.length > 0 && letter.id == 0) {
          var focletterID = $(focActivity).first().attr('data-id');
          if (!cache[focletterID]) {
            fetchLetterByID(focletterID).then(x => {
              openDraftLetter(letter);
            });
          }
          else {
            letter.body = cache[focletterID].body;
            app.page.focLetter.onLetterTapped(letter);
          }
        }
        else {
          app.page.focLetter.onLetterTapped(letter);
        }
      }
      else if (letter.type == "401"
        && (self.model.status == 700
          || self.model.status != 600)) {
        openFocApprovalLetter(letter);
      }
      else if (letter.type == "402"
        || letter.type == "403"
        || letter.type == "404"
        || letter.type == "405"
        || letter.type == "406") {
        app.page.sanctionLetter.onLetterTapped(letter);
      }
      else if (letter.type == "407"
        || letter.type == "408") {
        app.page.appeal.letter.onLetterTapped(letter);
      }
    }
  }

  function openFocApprovalLetter(letter) {
    if (letter.id == 0) {
      var focApprovalLetterElement = $('.file-icon[data-status="Final"][data-type-id="401"]');
      var focApprovalLetterID = $(focApprovalLetterElement).attr('data-id');
      if (!cache[focApprovalLetterID]) {
        fetchLetterByID(focApprovalLetterID).then(x => {
          openFocApprovalLetter(letter);
        });
      }
      else {
        letter.body = cache[focApprovalLetterID].body
        app.page.focReview.onLetterTapped(letter);
      }
    }
    else {
      app.page.focReview.onLetterTapped(letter);
    }
  }

  $(function () {
    init();

    $('.file-icon[data-status="Final"]').click(function () {
      var letterID = $(this).attr('data-id');
      openFinalLetter(letterID);
    });

    $('.file-icon[data-status="Draft"]').click(function () {
      var letter = {};
      letter.id = $(this).attr('data-id');
      letter.type = $(this).attr('data-type-id');
      openDraftLetter(letter)
    });

    $('.action-letter').click(function () {
      var type = $(this).attr('data-type-id');

      var letterElement = $('.file-icon[data-status="Draft"][data-type-id="' + type + '"]');

      if (letterElement.length > 0) {
        letterElement.click();
      }
      else {
        var letter = {};
        letter.id = 0;
        letter.type = type;
        openDraftLetter(letter)
      }
    });

  });

})(app.page);

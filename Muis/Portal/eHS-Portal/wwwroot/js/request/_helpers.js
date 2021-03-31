(function () {

  this.prepareDraftRejectionEmail = function (template, data) {
    var body = '';
    if (!app.utils.isNullOrEmpty(template.body)) {

      var bodyTemplate = template.body;

      if (bodyTemplate.indexOf('{draft}') != -1
        && bodyTemplate.indexOf('{enddraft}') != -1) {

        bodyTemplate = bodyTemplate.substring(
          bodyTemplate.indexOf('{draft}') + 7,
          bodyTemplate.lastIndexOf('{enddraft}'));
      }

      body = setContentProperties(bodyTemplate, data);
    }

    return {
      from: setEmailProperties(template.from),
      body: body
    };
  }

  this.prepareRejectionEmail = function (template, dataSet, drafts) {

    var to = '';

    dataSet.forEach((e, i) => {
      var person = e.agent || e.requestor;
      if (person) {
        to = appendEmail(to, getPersonEmail(person));
      }
    });

    var body = '';

    if (!app.utils.isNullOrEmpty(template.body)) {
      var bodyTemplate = template.body;

      if (bodyTemplate.indexOf('{draft}') != -1
        && bodyTemplate.indexOf('{enddraft}') != -1) {

        body += bodyTemplate.substring(0, bodyTemplate.indexOf('{draft}'));

        if (drafts.length > 0) {
          drafts.forEach((e, i) => {
            if (i > 0) {
              body += '<br />';
            }

            body += e.body;
          });
        }

        body += bodyTemplate.substring(bodyTemplate.indexOf('{enddraft}') + 10);
      } else {
        body = bodyTemplate;
      }

      body = setContentProperties(body, dataSet[0]);
    }

    return {
      from: setEmailProperties(template.from),
      to: to,
      cc: setEmailProperties(template.cc),
      bcc: setEmailProperties(template.bcc),
      title: setContentProperties(template.title, dataSet[0]),
      body: body,
      isBodyHtml: true
    };
  }

  function setContentProperties(val, data) {
    if (app.utils.isNullOrEmpty(val)) {
      return val;
    }

    var customer;
    if (data.customer) {
      customer = data.customer.name;
    }

    var premise;
    if (data.premises && data.premises.length > 0) {
      // By assumption, there should only be one primary premise
      var primaryPremises = data.premises.filter(e => e.isPrimary);
      if (primaryPremises.length > 0) {
        premise = app.utils.formatPremise(primaryPremises[0]);
      }
    }

    var inspectedOn;

    if (data.jobOrder) {
      inspectedOn = app.utils.formatDateTime(data.jobOrder.completedOn, true);
    }

    var findings;
    if (data.jobOrder) {
      findings = createEmailFindings(data.jobOrder);
    }

    return val.replaceAll('{customer}', app.utils.emptyIfNullOrEmpty(customer))
      .replaceAll('{premise}', app.utils.emptyIfNullOrEmpty(premise))
      .replaceAll('{refID}', app.utils.emptyIfNullOrEmpty(data.refID))
      .replaceAll('{type}', app.utils.emptyIfNullOrEmpty(data.typeText))
      .replaceAll('{auditorName}', app.utils.emptyIfNullOrEmpty(data.assignedToName))
      .replaceAll('{inspectedOn}', app.utils.emptyIfNullOrEmpty(inspectedOn))
      .replaceAll('{findings}', app.utils.emptyIfNullOrEmpty(findings))
      .replaceAll('{officerEmail}', app.utils.emptyIfNullOrEmpty(app.identity.email))
      .replaceAll('{officerName}', app.utils.emptyIfNullOrEmpty(app.identity.name));
  }

  function setEmailProperties(val) {
    if (app.utils.isNullOrEmpty(val)) {
      return val;
    }

    return val.replaceAll('{officerEmail}', app.utils.emptyIfNullOrEmpty(app.identity.email));
  }

  function createEmailFindings(jo) {
    if (!jo || !jo.findings || jo.findings.length == 0) {
      return '';
    }

    var findings = jo.findings.filter(e => e.officerID == jo.assignedTo && !e.complied)
      .sort((a, b) => moment.utc(b.createdOn).diff(moment.utc(a.createdOn)));
    if (findings.length == 0) {
      return '';
    }

    var schemeMap = {};
    jo.lineItems.forEach(li => {
      schemeMap[li.scheme] = {
        schemeText: li.schemeText,
        subSchemeText: li.subSchemeText
      }
    });

    var checklist = findings[0].lineItems.sort((a, b) => {
      var result = a.scheme - b.scheme;

      if (result == 0) {
        result = a.checklistCategoryID - b.checklistCategoryID;
      }

      if ((a.checklistCategoryID != -1 && b.checklistCategoryID == -1)
        || (a.checklistCategoryID == -1 && b.checklistCategoryID != -1)) {
        return a.checklistCategoryID == -1 ? 1 : -1;
      }

      if (result == 0) {
        result = a.checklistItemID - b.checklistItemID;
      }
      return result;
    });

    var tdStyle = 'padding: 8px;vertical-align: top;border: 1px solid #cfcfcf;';

    var result = '<table style="border-collapse: collapse; width: 100%;" border="1">' +
      '<tbody>' +
      '<tr>' +
      '<td style="width: 4%;' + tdStyle + '"></td>' +
      '<td style="width: 32%;' + tdStyle + '">Audit findings</td>' +
      '<td style="width: 32%;' + tdStyle + '">Their rectification</td>' +
      '<td style="width: 32%;' + tdStyle + '"></td>' +
      '</tr>';

    var schemes = [];
    var categories = [];

    checklist.forEach((e, i) => {
      if (e.complied) {
        return;
      }

      if (!schemes.includes(e.scheme)) {
        categories = [];
        schemes.push(e.scheme);

        var scheme = schemeMap[e.scheme];
        var schemeDisplay = scheme.schemeText;
        if (scheme.subSchemeText) {
          schemeDisplay += ' (' + scheme.subSchemeText + ')';
        }

        result += '<tr>' +
          '<td colspan="4" style="' + tdStyle + '">' + schemeDisplay + '</td>' +
          '</tr>';
      }

      var remarks = '<div>' + e.remarks;

      if (e.attachments) {
        e.attachments.forEach(a => {
          remarks += '<p><img src="/api/file/' + a.fileID + '?filename=' + encodeURI(a.fileName) + '" alt="' + a.fileName + '" width="280" height="172" />';
        });
      }

      remarks += '</div>';

      var showCategoryText = !categories.includes(e.checklistCategoryText);
      categories.push(e.checklistCategoryText);

      result += '<tr>' +
        '<td style="width: 4%;text-align:center;' + tdStyle + '">' + (i + 1) + '</td>' +
        '<td style="width: 32%;' + tdStyle + '">' + remarks + '</td>' +
        '<td style="width: 32%;' + tdStyle + '"></td>' +
        '<td style="width: 32%;' + tdStyle + '"><div>Clause ' + e.checklistCategoryID + ':' + e.checklistItemID + '</div>' +
        '<p><b>' + e.checklistCategoryText + '</b><br/>' + e.checklistItemText + '</p>' +
        '</td>';
    });

    result += '</tbody></table>';

    return result;
  }

  function getPersonEmail(per) {
    if (!per || !per.contactInfos || per.contactInfos.length == 0) {
      return '';
    }

    var emails = '';

    var contactInfo = per.contactInfos.filter(e => e.type == 5);
    if (contactInfo.length > 0) {
      contactInfo.forEach(e => {
        if (emails.length > 0) {
          emails += '; ';
        }

        emails += e.value;
      })
    }
    return emails;
  }

  function appendEmail(val, toAppend) {
    if (app.utils.isNullOrEmpty(toAppend)) {
      return val;
    }

    var emails = toAppend.split(';');
    emails.forEach(e => {
      var email = e.trim().toLowerCase();

      if (!app.utils.isNullOrEmpty(email)
        && val.indexOf(email) == -1) {
        if (val.length > 0) {
          val += '; ';
        }
        val += email;
      }

    });

    return val;
  }

}).apply(app.page.helper = app.page.helper || {});
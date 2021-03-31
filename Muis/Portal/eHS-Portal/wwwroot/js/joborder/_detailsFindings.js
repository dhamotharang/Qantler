(function (self) {

  self.init = function (schemes, d) {
    if (!$('.findings').length) {
      return;
    }

    var data = initData(schemes, d);
    render(data);

    var remarks = data.filter(e => e.remarks);
    if (remarks.length > 0) {
      $('.findings .conclusion .remarks').html(remarks[0].remarks);
    }
  }

  function initData(schemes, d) {
    var result = [];
    if (!schemes) {
      return result;
    }

    var holder = {};

    schemes.forEach(function (e, i) {

      var wrapper = {
        scheme: e.id,
        schemeText: e.text,
        items: []
      }

      result.push(wrapper);
      holder[e.id] = wrapper;
    });

    if (d) {
      d.forEach(function (f) {

        if (f.lineItems) {

          f.lineItems.forEach(function (li) {
            li.officer = f.officer;

            var wrapper = holder[li.scheme];
            wrapper.items.push(li);

            if (f.remarks) {
              wrapper.remarks = f.remarks;
            }
          });
        }

      });
    }

    return result.sort((a, b) => a.scheme - b.scheme);
  }

  function render(d) {
    if (!d) {
      return;
    }

    var tabContainer = $('.findings .nav');
    var tabPaneContainer = $('.findings .tab-content');

    var tabTemplate = $('.findings-template .nav-item');
    var tabPaneTemplate = $('.findings-template .tab-pane');

    var checklistTemplate = $('.checklist-template .checklist');
    var itemTemplate = $('.checklist-template .checklist-item');
    var infoTemplate = $('.checklist-template .info');
    var categoryTemplate = $('.checklist-template .info.category');
    var remarksTemplate = $('.checklist-template .remarks');
    var attachmentsTemplate = $('.checklist-template .attachments');
    var attachmentTemplate = $('.checklist-template .attachment');

    d.forEach(function (e, i) {

      var id = e.schemeText.replaceAll(' ', '');

      tabContainer.append(tabTemplate
        .prop('outerHTML')
        .replaceAll('{active}', i == 0 ? 'active' : '')
        .replaceAll('{id}', id)
        .replaceAll('{text}', e.schemeText));

      var items = e.items.sort((a, b) => {
        var result = a.checklistCategoryID - b.checklistCategoryID;

        if (result == 0) {
          result = a.checklistItemID - b.checklistItemId;
        }

        return result;
      });


      var checklist = checklistTemplate.clone();

      var currCategory;
      var currItem;
      var currItemID;

      items.forEach(function (li) {

        if (currCategory != li.checklistCategoryID) {

          if (currItem) {
            checklist.append(currItem);
            currItem = null;
          }

          checklist.append(categoryTemplate
              .prop('outerHTML')
              .replaceAll('{id}', li.checklistCategoryID)
              .replaceAll('{text}', li.checklistCategoryText.toUpperCase()));

          currCategory = li.checklistCategoryID;
        }

        if (currItemID != li.checklistItemID)
        {
          if (currItem) {
            checklist.append(currItem);
          }

          currItem = itemTemplate.clone();

          currItem.append(infoTemplate
            .clone()
            .prop('outerHTML')
            .replaceAll('{id}', li.checklistCategoryID + '.' + li.checklistItemID)
            .replaceAll('{text}', li.checklistItemText)
            .replaceAll('{tooltipVisibility}', 'd-none'));

          currItemID = li.checklistItemID;
        }

        var remarks = remarksTemplate.clone();

        if (li.attachments) {
          var attachments = attachmentsTemplate.clone();

          li.attachments.forEach(function (a) {

            attachments.append(attachmentTemplate
              .prop('outerHTML')
              .replaceAll('{fileName}', a.fileName)
              .replaceAll('{fileSize}', app.utils.formatFileSize(a.size))
              .replaceAll('{fileID}', a.fileID));

          });

          remarks.append(attachments);
        }

        currItem.append(remarks
          .prop('outerHTML')
          .replaceAll('{complianceVisibility}', '')
          .replaceAll('{officer}', app.utils.titleCase(app.utils.userContext(li.officer.id, li.officer.name)))
          .replaceAll('{compliance}', li.complied ? 'Compliance' : 'Non-compliance')
          .replaceAll('{color}', li.complied ? 'text-success' : 'text-danger')
          .replaceAll('{text}', li.remarks)
          .replaceAll('{separatorVisibility}', app.utils.isNullOrEmpty(li.remarks) ? 'd-none' : ''));
      });

      if(currItem) {
        checklist.append(currItem);
      }

      tabPaneContainer.append(tabPaneTemplate.clone()
        .addClass(i == 0 ? 'active show' : '')
        .append(checklist)
        .prop('outerHTML')
        .replaceAll('{id}', id));
    });
  }

})(app.page.findings = app.page.findings || {});
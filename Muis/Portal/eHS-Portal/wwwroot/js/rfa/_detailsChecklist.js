(function (self) {

  self.init = function (d) {
    if (!$('.checklist').length) {
      return;
    }

    var data = initData(d);
    render(data);
  }

  function initData(d) {
    var result = [];

    var map = {};

    d.lineItems.forEach(function (e, i) {

      var wrapper = map[e.scheme];
      if (!wrapper) {
        wrapper = {
          scheme: e.scheme,
          schemeText: e.schemeText,
          items: []
        }

        result.push(wrapper);
        map[e.scheme] = wrapper;
      }

      wrapper.items.push(e);
    });

    return result.sort((a, b) => a.scheme - b.scheme);
  }

  function render(d) {
    if (!d) {
      return;
    }

    var tabContainer = $('.checklist .nav');
    var tabPaneContainer = $('.checklist .tab-content');

    var tabTemplate = $('.checklist .template .nav-item');
    var tabPaneTemplate = $('.checklist .template .tab-pane');

    var checklistTemplate = $('.checklist-template .checklist');
    var itemTemplate = $('.checklist-template .checklist-item');
    var infoTemplate = $('.checklist-template .info');
    var categoryTemplate = $('.checklist-template .info.category');
    var remarksTemplate = $('.checklist-template .remarks');
    var attachmentsTemplate = $('.checklist-template .attachments');
    var attachmentTemplate = $('.checklist-template .attachment');

    remarksTemplate.find('.officer').parent().remove();

    d.forEach(function (e, i) {

      var id = e.schemeText.replaceAll(' ', '');

      tabContainer.append(tabTemplate
        .prop('outerHTML')
        .replaceAll('{active}', i == 0 ? 'active' : '')
        .replaceAll('{id}', id)
        .replaceAll('{text}', e.schemeText));

      var items = e.items.sort((a, b) => {
        var result = a.checklistCategoryID - b.checklistCategoryID;

        if ((a.checklistCategoryID != -1 && b.checklistCategoryID == -1)
          || (a.checklistCategoryID == -1 && b.checklistCategoryID != -1)) {
          return a.checklistCategoryID == -1 ? 1 : -1;
        }

        if (result == 0) {
          result = a.checklistID - b.checklistId;
        }

        return result;
      });


      var checklist = checklistTemplate.clone();

      var currCategory;

      items.forEach(function (li, i) {

        if (currCategory != li.checklistCategoryID) {

          checklist.append(categoryTemplate
            .prop('outerHTML')
            .replaceAll('{id}', li.checklistCategoryID < 0 ? '' : li.checklistCategoryID)
            .replaceAll('{text}', li.checklistCategoryText.toUpperCase()));

          currCategory = li.checklistCategoryID;
        }

        var item = itemTemplate.clone();

        item.append(infoTemplate
          .clone()
          .prop('outerHTML')
          .replaceAll('{id}', li.checklistCategoryID < 0 ? '' : li.checklistCategoryID + '.' + li.checklistID)
          .replaceAll('{text}', li.checklistText)
          .replaceAll('{tooltipVisibility}', 'd-none'));

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

        item.append(remarks
          .prop('outerHTML')
          .replaceAll('{text}', li.remarks)
          .replaceAll('{separatorVisibility}', 'd-none'));

        if (li.replies) {
          li.replies.forEach(function (r) {

            item.append(remarksTemplate.clone()
              .prop('outerHTML')
              .replaceAll('{text}', r.text)
              .replaceAll('{separatorVisibility}', 'd-none'));

          });
        }

        checklist.append(item);
      });

      tabPaneContainer.append(tabPaneTemplate.clone()
        .addClass(i == 0 ? 'active show' : '')
        .append(checklist)
        .prop('outerHTML')
        .replaceAll('{id}', id));
    });
  }

})(app.page.checklist = app.page.checklist || {});
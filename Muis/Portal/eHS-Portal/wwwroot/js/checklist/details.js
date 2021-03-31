(function (page) {
  $('.datepicker').datepicker({ startDate: new Date() });

  var schemeList = [
    { id: 100, value: "Eating Establishment" },
    { id: 200, value: "Food Preparation Area" },
    { id: 300, value: "Food Manufacturing" },
    { id: 400, value: "Poultry" },
    { id: 500, value: "Endorsement" },
    { id: 600, value: "Storage Facility" },
  ];

  var schemeText = schemeList[schemeList.findIndex(x => x.id === app.page.model.schemeID)].value;

  page.setSchemeText = function setSchemeText() {
    $('#scheme').html(schemeText);
  }

  page.initTinymce = function initTinymce(id, value = "") {
    tinymce.init({
      selector: "#" + id,
      height: 200,
      menubar: false,
      max_chars: 4000,
      setup: function (ed) {
        var allowedKeys = [8, 37, 38, 39, 40, 46];

        ed.on('keydown', function (e) {
          if (allowedKeys.indexOf(e.keyCode) != -1) {
            return true;
          }

          if (tinymceGetContentLength() + 1 > this.settings.max_chars) {
            e.preventDefault();
            e.stopPropagation();
            return false;
          }

          return true;
        });

        ed.on('keyup', function (e) {
          tinymceUpdateCharCounter(this, tinymceGetContentLength());
        });

        ed.on('init', function (e) {
          ed.setContent(value);
        });
      },

      init_instance_callback: function () {
        $('#' + this.id).prev().append('<div class="char_count" style="text-align:right"></div>');

        tinymceUpdateCharCounter(this, tinymceGetContentLength());
      },

      paste_preprocess: function (plugin, args) {
        var editor = tinymce.get(tinymce.activeEditor.id);
        var len = editor.contentDocument.body.innerText.length;
        var text = args.content;

        if (len + text.length > editor.settings.max_chars) {
          alert('Pasting this exceeds the maximum allowed number of ' + editor.settings.max_chars + ' characters.');
          args.content = '';
        }
        else {
          tinymceUpdateCharCounter(editor, len + text.length);
        }

      },

      plugins: [
        'advlist autolink lists link image charmap print preview anchor',
        'searchreplace visualblocks code fullscreen',
        'insertdatetime media table paste code help wordcount'
      ],

      toolbar: 'undo redo | formatselect | ' +
        'bold italic backcolor | alignleft aligncenter ' +
        'alignright alignjustify | bullist numlist outdent indent | ' +
        'removeformat | help',

      content_style: 'body { font-family:Helvetica,Arial,sans-serif; font-size:14px }'
    });
  }

  page.addItem = function addItem(element, value = "") {
    var preSection = $(element).closest('tr').prev('tr');
    var presectionIndex = $(preSection).attr('section');
    var categoryIndex = $(preSection).attr('category');
    var result = app.page.validateCategoryItem(categoryIndex);
    if (result == true) {
      if (presectionIndex == undefined) {
        presectionIndex = 0;
      }

      var html = $('#addItem').find("tbody").html();
      html = html.replaceAll("{sectionIndex}", (parseInt(presectionIndex) + 1)).replaceAll("{categoryIndex}", categoryIndex);

      $(html).insertAfter(preSection);

      app.page.initTinymce("tinyMce" + categoryIndex + "s" + (parseInt(presectionIndex) + 1), value);
    }
  }

  page.addCategory = function addCategory(element, note = "") {
    var result = app.page.validateCategoryTitle();
    if (result == true) {
      var lastCategoryIndex = $('.categories').length;
      var lastCategory = $('.categories')[lastCategoryIndex - 1];
      var html = $('#addsection').html();

      html = html.replaceAll("{categoryIndex}", (parseInt(lastCategoryIndex) + 1)).replaceAll("{class}", "categories");

      if (lastCategoryIndex == 0) {
        $('#form').append(html);
      }
      else {
        $(html).insertAfter(lastCategory);
      }

      app.page.initTinymce('tinyMce' + (lastCategoryIndex + 1) + 's1', note);
    }
  }

  page.addNote = function addNote(element) {
    $(element).parent().next().show();
    $(element).hide();
  }

  page.removeItem = function removeItem(element) {
    var removedSection = $(element).attr('section');
    var removedCategory = $(element).attr('category');

    tinymce.get("tinyMce" + removedCategory + "s" + removedSection).remove();

    $("[category='" + removedCategory + "'][section='" + removedSection + "']tr").remove();

    $.each($("[category='" + removedCategory + "']tr"), function (key, value) {
      var section = $(value).attr('section');
      if (section > removedSection) {

        $(value).find('.index').html("<i category='" + removedCategory + "' section='" + parseInt(section - 1) + "'  class='item mdi mdi-minus-circle'  onclick='app.page.removeItem(this)'></i>" + removedCategory + "." + parseInt(section - 1));

        $.each($(value).find("[section='" + section + "']"), function (key1, value1) {
          $(value1).attr('section', section - 1);

          if ($(value1).hasClass("text")) {
            var name = "categoryText" + removedCategory + "." + parseInt(section - 1);

            $(value1).attr('name', name);
          }

          if ($(value1).hasClass("note")) {
            var id = "tinyMce" + removedCategory + "s" + parseInt(section - 1);
            var oldId = "tinyMce" + removedCategory + "s" + parseInt(section);
            var note = tinymce.get(oldId).getContent();

            tinymce.get(oldId).remove();

            $(value1).attr('id', id);

            app.page.initTinymce(id, note);

          }
        });
        $(value).attr('section', section - 1);
      }
    });
    return true;
  }

  page.removeSection = function removeSection(element) {
    var removedCategory = $(element).attr('category');

    $.each($("[category='" + removedCategory + "'].note"), function (key, value) {
      tinymce.get(value.id).remove();
    });

    $('.categories')[removedCategory - 1].remove();
    $.each($('.content').find("[category!='" + removedCategory + "']tr"), function (key, value) {
      var category = $(value).attr('category');
      var section = $(value).attr('section');

      if (category > removedCategory) {

        $(value).find('.index').html("<i category='" + parseInt(category - 1) + "' section='" + section + "' onclick='app.page.removeItem(this)'  class='item mdi mdi-minus-circle' ></i>" + parseInt(category - 1) + "." + section);

        $(value).find('.category-index').html("<i category='" + parseInt(category - 1) + "' section='" + section + "'  onclick='app.page.removeSection(this)'  class='remove-section mdi mdi-minus-circle'></i>" + parseInt(category - 1));

        if ($(value).find(".category-text").length != 0) {
          var name = "category" + parseInt(category - 1);

          $(value).find(".category-text").attr('name', name);

          $(value).find(".category-text").attr('category', parseInt(category - 1));
        }

        $.each($(value).find("[section='" + section + "']"), function (key1, value1) {

          if ($(value1).hasClass("text")) {
            var name = "categoryText" + parseInt(category - 1) + "." + section;

            $(value1).attr('name', name);
          }

          if ($(value1).hasClass("note")) {
            var id = "tinyMce" + parseInt(category - 1) + "s" + section;
            var oldId = "tinyMce" + category + "s" + section;
            var note = tinymce.get(oldId).getContent();

            tinymce.get(oldId).remove();

            $(value1).attr('id', id);

            app.page.initTinymce(id, note);
          }

          $(value1).attr('category', category - 1);
        });

        $(value).attr('category', category - 1);

        $(value).find('.remove-item').attr('category', category - 1);
      }
    });
  }

  page.loadChecklist = function loadChecklist() {
    var effectiveDate = new Date(app.page.model.checklist.effectiveFrom);
    effectiveDate.setHours(0, 0, 0, 0);
    var today = new Date();
    today.setHours(0, 0, 0, 0);

    if (effectiveDate >= today) {
      $('#effectiveDate').datepicker('setDate', new Date(effectiveDate));
    }
    else {
      var date = new Date();
      $('#effectiveDate').datepicker('setDate', date);
    }

    $.each(app.page.model.checklist.categories, function (index, value) {

      if (index != 0) {
        app.page.addCategory($('.add-category'), value.items[0].notes);
      }

      else {
        app.page.initTinymce('tinyMce1s1', value.items[0].notes);
      }

      $("input[category='" + (index + 1) + "'].category-text").val(value.text);

      $.each(app.page.model.checklist.categories[index].items, function (index1, value1) {
        if (parseInt(index1) != 0) {
          app.page.addItem($('.add-section')[index], value1.notes);
        }

        var sectionIndexTemp = parseInt(index1) + 1;
        var categoryIndexTemp = parseInt(index) + 1;
        var elementTextName = "categoryText" + categoryIndexTemp + "." + sectionIndexTemp;

        $("textarea[name='" + elementTextName + "']").val(value1.text);

        if (value1.notes && value1.notes != '') {
          $("[name='" + elementTextName + "']").parent().next().find('button').click();
        }

      });
    });
  }

  page.cancelChecklist = function cancelChecklist() {
    var url = window.location.origin + "/checklist/scheme";
    window.location.href = url;
  }

  page.validateOnKeyup = function validateOnKeyup() {
    if ($(this).val() != '') {
      $(this).removeClass('error');
      $(this).next().remove();
    }
    else {
      $(this).addClass('error');
      $(this).next().remove();
      $(this).after('<div class="error">This field is required</div>');
    }
  }

  page.validateCategoryTitle = function validateCategoryTitle() {
    var result = true;
    var element = $('.category-text');
    element.splice(-1, 1);

    $.each(element, function (index, value) {
      if ($(value).val() == '') {
        $(value).addClass('error');
        $(value).addClass('validate');
        $(value).on('keyup', app.page.validateOnKeyup);
        $(this).next().remove();
        $(value).after('<div class="error">This field is required</div>');
        result = false;
      }

    });

    return result;
  }

  page.validateCategoryItem = function validateCategoryItem(index) {
    var result = true;
    if (index != 0) {
      var element = $('.text[category="' + index + '"]');
    }
    else {
      var element = $('.text');
      element.splice(element.length - 2, 2);
    }

    $.each(element, function (index, value) {
      if ($(value).val() == '') {
        $(value).addClass('error');
        $(value).addClass('validate');
        $(value).on('keyup', app.page.validateOnKeyup);
        $(value).next().remove();
        $(value).after('<div class="error">This field is required</div>');
        result = false;
      }

    });

    return result;
  };

  $('#saveChecklist').click(function () {
    var iscategoryTitleValid = app.page.validateCategoryTitle();
    var iscategoryItemValid = app.page.validateCategoryItem(0);
    var isEffectiveDateValid = validateEffectiveDate();

    if (iscategoryTitleValid == true && iscategoryItemValid == true && isEffectiveDateValid == true) {
      var EffectiveFrom = moment($('#effectiveDate').datepicker('getDate')).startOf('day').local().format();
      var saveModel;
      var method;

      if (app.page.model.action != "Edit") {
        method = "Post"
        saveModel =
        {
          id: 0,
          version: app.page.model.lastVersion + 1,
          scheme: app.page.model.schemeID,
          createdBy: app.identity.id,
          effectiveFrom: EffectiveFrom,
          categories: []
        }
      }

      else {
        method = "Put"
        saveModel =
        {
          id: app.page.model.checklist.id,
          version: app.page.model.lastVersion,
          scheme: app.page.model.schemeID,
          CceatedBy: app.identity.id,
          effectiveFrom: EffectiveFrom,
          categories: []
        }
      }

      var categoriesList = $('.categories');
      $.each(categoriesList, function (index, value) {

        var tempCategoryIndex = (parseInt(index) + 1);
        var tempElement = $(value).find("[name='category" + tempCategoryIndex + "']");
        var checklistCategoryModel =
        {
          id: 0,
          index: tempCategoryIndex,
          text: $(tempElement).val(),
          items: []
        };

        $.each($(categoriesList[index]).find("textarea[name^='categoryText" + tempCategoryIndex + ".']"), function (index1, value1) {
          var tempSectionIndex = (parseInt(index1) + 1);
          var item =
          {
            id: 0,
            index: tempSectionIndex,
            text: $("[section='" + tempSectionIndex + "'][category='" + tempCategoryIndex + "'].text").val(),
            notes: tinymce.get("tinyMce" + tempCategoryIndex + "s" + tempSectionIndex).getContent()
          };
          checklistCategoryModel.items.push(item);
        });

        saveModel.categories.push(checklistCategoryModel);
      });

      fetch('/api/checklist', {
        method: method,
        cache: 'no-cache',
        credentials: 'include',
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(saveModel)
      }).then(app.http.errorInterceptor)
        .then(res => res.json())
        .then(result => {
          if (result) {
            var url = window.location.origin + "/checklist/scheme";
            window.location.href = url;
          }
        })
        .catch(app.http.catch);
    }
  });

})(app.page);

$(document).ready(function () {

  $('#effectiveDate').change(function () {
    validateEffectiveDate();
  });

  app.page.setSchemeText();

  if (app.page.model.action != "New") {
    app.page.loadChecklist();
  }
  else {
    app.page.initTinymce('tinyMce1s1');
  }

});

function tinymceUpdateCharCounter(el, len) {
  $('#' + el.id).prev().find('.char_count').text(len + '/' + el.settings.max_chars);
}

function tinymceGetContentLength() {
  return tinymce.get(tinymce.activeEditor.id).contentDocument.body.innerText.length;
}

function validateEffectiveDate() {
  var today = new Date();
  today.setHours(0, 0, 0, 0);
  if ($('#effectiveDate').datepicker('getDate') != "" && (new Date($('#effectiveDate').datepicker('getDate')) >= today)) {
    $('#effectiveDate').css('border-color', '');
    $('#effectivedate-error').remove();
    return true;
  }
  else if ($('#effectivedate-error').length == 0) {
    $('#effectiveDate').css('border-color', 'red');
    $("<div id='effectivedate-error' style='font-size: .875rem;' class='error'>Invalid date</div>")
      .insertAfter("#effectiveDate");
    return false;
  }
  else {
    return false;
  }
}
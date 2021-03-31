var ChartColor = ["#5D62B4", "#54C3BE", "#EF726F", "#F9C446", "rgb(93.0, 98.0, 180.0)", "#21B7EC", "#04BCCC"];
var primaryColor = getComputedStyle(document.body).getPropertyValue('--primary');
var secondaryColor = getComputedStyle(document.body).getPropertyValue('--secondary');
var successColor = getComputedStyle(document.body).getPropertyValue('--success');
var warningColor = getComputedStyle(document.body).getPropertyValue('--warning');
var dangerColor = getComputedStyle(document.body).getPropertyValue('--danger');
var infoColor = getComputedStyle(document.body).getPropertyValue('--info');
var darkColor = getComputedStyle(document.body).getPropertyValue('--dark');
var lightColor = getComputedStyle(document.body).getPropertyValue('--light');
if ($('body').hasClass("dark-theme")) {
  var chartFontcolor = '#b9c0d3';
  var chartGridLineColor = '#383e5d';

} else {
  var chartFontcolor = '#6c757d';
  var chartGridLineColor = 'rgba(0,0,0,0.08)';
}

(function ($) {
  'use strict';
  $(function () {
    var body = $('body');
    var contentWrapper = $('.content-wrapper');
    var scroller = $('.container-scroller');
    var footer = $('.footer');
    var sidebar = $('#sidebar');

    //Add active class to nav-link based on url dynamically
    //Active class can be hard coded directly in html file also as required
    if (!$('#sidebar').hasClass("dynamic-active-class-disabled")) {
      var current = location.pathname.split("/").slice(-1)[0].replace(/^\/|\/$/g, '');
      $('#sidebar >.nav > li:not(.not-navigation-link) a').each(function () {
        var $this = $(this);
        if (current === "") {
          //for root url
          if ($this.attr('href').indexOf("index.html") !== -1) {
            $(this).parents('.nav-item').last().addClass('active');
            if ($(this).parents('.sub-menu').length) {
              $(this).addClass('active');
            }
          }
        } else {
          //for other url
          if ($this.attr('href').indexOf(current) !== -1) {
            $(this).parents('.nav-item').last().addClass('active');
            if ($(this).parents('.sub-menu').length) {
              $(this).addClass('active');
            }
            if (current !== "index.html") {
              $(this).parents('.nav-item').last().find(".nav-link").attr("aria-expanded", "true");
              if ($(this).parents('.sub-menu').length) {
                $(this).closest('.collapse').addClass('show');
              }
            }
          }
        }
      })
    }

    // Themeswitch function
    function themeSwitch(url) {
      var currentURL = window.location.href;
      var res = currentURL.split("/");
      var abs_url = currentURL.replace(/demo_.\d*/, url);
      window.location.href = abs_url;
    }
    $("#theme-light-switch").on("click", function (e) {
      e.preventDefault();
      themeSwitch('demo_1');
    });
    $("#theme-dark-switch").on("click", function (e) {
      e.preventDefault();
      themeSwitch('demo_3');
    });


    $(".email-wrapper .mail-list-container .mail-list").on("click", function () {
      $(".email-wrapper .mail-list-container").addClass("d-none");
      $(".email-wrapper .mail-view").addClass("d-block");
    });
    $(".email-wrapper .mail-back-button").on("click", function () {
      $(".email-wrapper .mail-list-container").removeClass("d-none");
      $(".email-wrapper .mail-view").removeClass("d-block");
    });
    $(".aside-toggler").on("click", function () {
      $(".mail-sidebar,.chat-list-wrapper").toggleClass("menu-open");
    });
    $("#color-setting").on("click", function () {
      $("#color-settings").addClass("open");
    });
    $("#layout-toggler").on("click", function () {
      $("#theme-settings").addClass("open");
    });
    $("#chat-toggler").on("click", function () {
      $("#right-sidebar").addClass("open");
    });

    //Close other submenu in sidebar on opening any
    $("#sidebar > .nav > .nav-item > a[data-toggle='collapse']").on("click", function () {
      $("#sidebar > .nav > .nav-item").find('.collapse.show').collapse('hide');
    });


    //Change sidebar and content-wrapper height
    applyStyles();

    function applyStyles() {
      //Applying perfect scrollbar
      if (!body.hasClass("rtl")) {

      }
    }

    $('[data-toggle="minimize"]').on("click", function () {
      if ((body.hasClass('sidebar-toggle-display')) || (body.hasClass('sidebar-absolute'))) {
        body.toggleClass('sidebar-hidden');
      } else {
        body.toggleClass('sidebar-icon-only');
      }
    });

    //checkbox and radios
    $(".form-check label,.form-radio label").append('<i class="input-helper"></i>');
  });

  $('[data-toggle="tooltip"]').tooltip();

  $(".sidebar .sidebar-inner > .nav > .nav-item").not(".brand-logo").attr('toggle-status', 'closed');
  $(".sidebar .sidebar-inner > .nav > .nav-item").on('click', function () {
    $(".sidebar .sidebar-inner > .nav > .nav-item").removeClass("active");
    $(this).addClass("active");
    $(".sidebar .sidebar-inner > .nav > .nav-item").find(".submenu").removeClass("open");
    $(".sidebar .sidebar-inner > .nav > .nav-item").not(this).attr('toggle-status', 'closed');
    var toggleStatus = $(this).attr('toggle-status');
    if (toggleStatus == 'closed') {
      $(this).find(".submenu").addClass("open");
      $(this).attr('toggle-status', 'open');
    } else {
      $(this).find(".submenu").removeClass("open");
      $(this).not(".brand-logo").attr('toggle-status', 'closed');
    }
  });
})(jQuery);
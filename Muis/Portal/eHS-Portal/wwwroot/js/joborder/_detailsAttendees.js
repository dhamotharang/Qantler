(function (self) {

  $('.attendees .item').each(function (i) {
    var name = $(this).find('.name').html();

    $(this).find('.text-avatar').html(app.utils.initials(name));
    $(this).find('.text-avatar').addClass('bg-' + app.utils.randomColor());
  });

})(app.page.attendees = app.page.attendees || {});
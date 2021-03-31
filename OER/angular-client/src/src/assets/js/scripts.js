$(document).ready(function () {
  $('#create-slider').slick({
    autoplay: false,
    arrows: false,
    draggable: false
  });

  $('.create-slider-controls .slick-next').click(function () {
    $('#create-slider').slick("slickNext");
  });
  $('.create-slider-controls .slick-prev').click(function () {
    $('#create-slider').slick("slickPrev");
  });

});

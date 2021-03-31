var certificate = certificate || {};

(function () {
  var productTemplate = '<div class="page product">' +
    '<img class="background #backgroundVisibility" src="/images/blue_certificate.png" alt="">' +
    '<span class="number">#number</span>' +
    '<div class="expiry">' +
    '<span>#expiry</span>' +
    '</div>' +
    '<div class="issuedOn #issuedOnVisibility">' +
    '<span class="label">Issued on:</span>' +
    '<span class="value">#issuedOnVal</span>' +
    '</div>' +
    '<span class="declaration">' +
    'The Majlis Ugama Islam Singapura hereby certifies that the following products are Halal to Muslims according to Islamic Law:' +
    '</span>' +
    '<ol class="list" start="#start">#products</ol>' +
    '<div class="customer">' +
    '<span class="label">(Name & Address of Company)</span>' +
    '<span class="name">#customer</span>' +
    '<span class="premise">#premise</span>' +
    '</div>' +
    '<div class="schemes">' +
    '<span class="scheme">#scheme<br/>#subScheme</span>' +
    '</div>' +
    '<div class="serialNo">' +
    '<span>#serialNo</span>' +
    '</div>' +
    '</div >';

  var cateringTemplate = '<div class="page catering">' +
    '<img class="background #backgroundVisibility" src = "/images/catering_certificate.png" alt ="">' +
    '<span class="number">#number</span>' +
    '<div class="expiry">' +
    '<span>#expiry</span>' +
    '</div>' +
    '<div class="issuedOn #issuedOnVisibility">' +
    '<span class="label">Issued on:</span>' +
    '<span class="value">#issuedOnVal</span>' +
    '</div>' +
    '<div class="info-group customer">' +
    '<span class="name">#customer</span>' +
    '</div>' +
    '<div class="info-group premise">' +
    '<span class="name">#premise</span>' +
    '</div>' +
    '<div class="schemes">' +
    '<span class="scheme">#scheme<br/>#subScheme</span>' +
    '</div>' +
    '<div class="serialNo">' +
    '<span>#serialNo</span>' +
    '</div>' +
    '</div>';

  var eeTemplate = '<div class="page ee">' +
    '<img class="background #backgroundVisibility" src="/images/green_certificate.png" alt="">' +
    '<span class="number">#number</span>' +
    '<div class="expiry">' +
    '<span>#expiry</span>' +
    '</div>' +
    '<div class="issuedOn #issuedOnVisibility">' +
    '<span class="label">Issued on:</span>' +
    '<span class="value">#issuedOnVal</span>' +
    '</div>' +
    '<div class="info-group customer">' +
    '<span class="name">#customer</span>' +
    '</div>' +
    '<div class="info-group premise">' +
    '<span class="name">#premise</span> ' +
    '</div>' +
    '<div class="schemes">' +
    '<span class="scheme">#scheme<br/>#subScheme</span>' +
    '</div>' +
    '<div class="serialNo">' +
    '<span>#serialNo</span>' +
    '</div>' +
    '</div>';

  var fpaTemplate = '<div class="page fpa">' +
    '<img class="background #backgroundVisibility" src="/images/fpa_certificate.png" alt="">' +
    '<span class="number">#number</span>' +
    '<div class="expiry">' +
    '<span>#expiry</span>' +
    '</div>' +
    '<div class="issuedOn #issuedOnVisibility">' +
    '<span class="label">Issued on:</span>' +
    '<span class="value">#issuedOnVal</span>' +
    '</div>' +
    '<div class="info-group customer">' +
    '<span class="name">#customer</span>' +
    '</div>' +
    '<div class="info-group premise">' +
    '<span class="name">#premise</span>' +
    '</div>' +
    '<div class="schemes">' +
    '<span class="scheme">#scheme<br/>#subScheme</span>' +
    '</div>' +
    '<div class="serialNo">' +
    '<span>#serialNo</span>' +
    '</div>' +
    '</div>';

  var poultryTemplate = '<div class="page poultry">' +
    '<img class="background #backgroundVisibility" src="/images/brown_certificate.png" alt="">' +
    '<span class="number">#number</span>' +
    '<div class="expiry">' +
    '<span>#expiry</span>' +
    '</div>' +
    '<div class="issuedOn #issuedOnVisibility">' +
    '<span class="label">Issued on:</span>' +
    '<span class="value">#issuedOnVal</span>' +
    '</div>' +
    '<span class="declaration">' +
    'The Majlis Ugama Islam Singapura hereby certifies that' +
    '</span>' +
    '<div class="info-group customer">' +
    '<span class="name">#customer</span>' +
    '<div class="separator"></div>' +
    '<span class="label">(Name of Poultry Abattoir)</span>' +
    '</div>' +
    '<div class="info-group premise">' +
    '<span class="name">#premise</span>' +
    '<div class="separator"></div>' +
    '<span class="label">(Address of Poultry Abattoir)</span>' +
    '</div>' +
    '<div class="schemes">' +
    '<span class="scheme">#scheme<br/>#subScheme</span>' +
    '</div>' +
    '<div class="serialNo">' +
    '<span>#serialNo</span>' +
    '</div>' +
    '<span class="declaration-footer">' +
    'conducts Halal-slaughtering of fresh poultry according to the Islamic Law.' +
    '</span>' +
    '<div class="footnote">' +
    'This Halal certificate is non-transferable and applicable only to freshly-slaughtered whole poultry tagged with Muis-approved labels.' +
    '</div>' +
    '</div>';

  var storageTemplate = '<div class="page storage">' +
    '<img class="background #backgroundVisibility" src = "/images/brown_certificate.png" alt="">' +
    '<span class="number">#number</span>' +
    '<div class="expiry">' +
    '<span>#expiry</span>' +
    '</div>' +
    '<div class="issuedOn #issuedOnVisibility">' +
    '<span class="label">Issued on:</span>' +
    '<span class="value">#issuedOnVal</span>' +
    '</div>' +
    '<span class="declaration">' +
    'The Majlis Ugama Islam Singapura hereby certifies that' +
    '</span>' +
    '<div class="info-group customer">' +
    '<span class="name">#customer</span>' +
    '<div class="separator"></div>' +
    '<span class="label">(Name of Storage Facility)</span>' +
    '</div>' +
    '<div class="info-group premise">' +
    '<span class="name">#premise</span>' +
    '<div class="separator"></div>' +
    '<span class="label">(Address of Storage Facility)</span>' +
    '</div>' +
    '<div class="schemes">' +
    '<span class="scheme">#scheme<br/>#subScheme</span>' +
    '</div>' +
    '<div class="serialNo">' +
    '<span>#serialNo</span>' +
    '</div>' +
    '<span class="declaration-footer">' +
    'is free from any non-Halal items or other elements of impurities according to the Islamic Law.' +
    '</span>' +
    '<div class="footnote">' +
    'This Halal certificate is non-transferable and not allowed to be displayed outside the premises of the above mentioned company.' +
    '</div>' +
    '</div >';

  this.getTemplate = function (t) {
    switch (t) {
      case 0:
      case 1:
        return productTemplate;
      case 2:
        return cateringTemplate;
      case 3:
        return fpaTemplate;
      case 4:
        return poultryTemplate;
      case 5:
        return endorsementTemplate;
      case 6:
        return eeTemplate;
      case 7:
        return storageTemplate;
    }
  }

}).apply(certificate = certificate || {});
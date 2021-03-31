function Calendar(e, defaultDate = moment().local()) {
  this.cal = e;
  this.defaultDate = defaultDate;
  this.currentDate = moment().local();
  this.minDate;
}

Calendar.prototype.setCurrentDate = function (date) {
  date = moment(date).local();

  this.currentDate = date;
  this.gotoDate(date);
  this.select(date);
}

Calendar.prototype.setMinDate = function (date) {
  this.minDate = date;
}

Calendar.prototype.gotoDate = function (date) {
  this.cal.fullCalendar('gotoDate', date);
}

Calendar.prototype.select = function (date) {
  this.cal.fullCalendar('select', date);
}

Calendar.prototype.init = function () {
  var self = this;
  this.cal.fullCalendar('destroy');

  this.cal.fullCalendar({
    header: {
      left: 'prev,next today',
      center: 'title',
      right: 'month,basicWeek,basicDay'
    },
    locale: 'en',
    dayNames: ['Sunday', 'Monday', 'Tuesday', 'Wednesday',
      'Thursday', 'Friday', 'Saturday'
    ],
    dayNamesShort: ['SUN', 'MON', 'TUE', 'WED', 'THU', 'FRI', 'SAT'],
    monthNames: ['January', 'February', 'March', 'April', 'May', 'June', 'July',
      'August', 'September', 'October', 'November', 'December'
    ],
    monthNamesShort: ['January', 'February', 'March', 'April', 'May', 'June', 'July',
      'August', 'September', 'October', 'November', 'December'
    ],
    defaultDate: self.defaultDate,
    nowIndicator: true,
    navLinks: false,
    editable: false,
    eventLimit: true, // allow "more" link when too many events
    eventSources: self.eventSources(),
    selectable: true,
    unselectAuto: false,
    displayEventTime: false,
    select: function (start, end) {
      self.onSelect(start, end);
    },
    selectAllow: function (info) {
      return self.selectAllow(info);
    },
    dayClick: function (info) {
      selectedDate = info;
    },
    validRange: function (nowDate) {
      return self.validRange(nowDate);
    },
    viewRender: function (view, element) {
      self.select(self.currentDate);
      self.viewRender(view, element);
    }
  });
}

Calendar.prototype.render = function () {
  this.cal.fullCalendar('render');
}

Calendar.prototype.onSelect = function (start, end) {
}

Calendar.prototype.selectAllow = function (info) {
  var diff = info.end.diff(info.start, 'days');
  if (diff > 1) {
    this.setCurrentDate(this.currentDate);
    return false;
  }

  if (   this.minDate
      && info.start < this.minDate) {
    this.setCurrentDate(this.currentDate);
    return false;
  }

  this.setCurrentDate(info.start);

  return true;
}

Calendar.prototype.viewRender = function (view, element) {
}

Calendar.prototype.validRange = function (nowDate) {
  return { };
}

Calendar.prototype.eventSources = function () {
  return [];
}
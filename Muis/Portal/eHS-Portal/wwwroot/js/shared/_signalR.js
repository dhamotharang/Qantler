(function () {
  'use strict';

  var holder = {};

  this.subscribe = function (topic, fn) {
    var observers = holder[topic];
    if (!observers) {
      observers = [];
      holder[topic] = observers;
    }
    observers.push(fn);
  }

  this.unsubscribe = function (topic, fn) {
    var observers = holder[topic];
    if (observers) {
      holder[topic] = observers.filter(function (o) {
        if (o !== fn) return i;
      });
    }
  };

  var connection = new signalR.HubConnectionBuilder().withUrl("/signalRHub").build();

  connection.on("message", function (message) {
    var observers = holder[message.topic];
    if (observers) {
      observers.forEach(function (o) {
        o(message);
      });
    }
  });

  connection
    .start()
    .then(function () { })
    .catch(function (err) {
      return console.error(err.toString());
    });
}).apply(app.signalR = app.signalR || {});
kontaktScheduleApp.controller('AddEventModalController', function ($scope, $modalInstance, calendarConfig, interpreters, period, events, privateEvents) {

    $scope.period = period;
    $scope.interpreters = interpreters.slice(0);

    var allEvents = events.concat(privateEvents);

    if (!$scope.period.allDay) {
        $scope.intersectingEvents = getIntersectingEvents(allEvents, $scope.period);
        handleIntersectingEvents($scope.interpreters, $scope.intersectingEvents);
        for (var i = 0; i < $scope.interpreters.length; i++) {
            countAppearences($scope.interpreters[i], events, $scope.period);
        }
        $scope.interpreters.sort(function (a, b) { return a.count - b.count });
    }
    else {
        $scope.period.dayName = $.fullCalendar.formatDate($scope.period.start, 'dddd, MMMM d.', calendarConfig.localConfig)
    }

    $scope.ok = function () {
        $modalInstance.close($scope.interpreter);
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

});

kontaktScheduleApp.controller('EditEventModalController', function ($scope, $modalInstance, calendarConfig, interpreters, event, events, privateEvents) {

    $scope.period = {
        start: event.start,
        end: event.end,
        allDay: event.allDay
    };
    $scope.interpreters = interpreters.slice(0);

    var allEvents = events.concat(privateEvents);

    if (!$scope.period.allDay) {
        $scope.intersectingEvents = getIntersectingEvents(allEvents, $scope.period, event);
        handleIntersectingEvents($scope.interpreters, $scope.intersectingEvents);
        for (var i = 0; i < $scope.interpreters.length; i++) {
            countAppearences($scope.interpreters[i], events, $scope.period);
        }
        $scope.interpreters.sort(function (a, b) { return a.count - b.count });
    }
    else {
        $scope.period.dayName = $.fullCalendar.formatDate($scope.period.start, 'dddd, MMMM d.', calendarConfig.localConfig)
    }

    $scope.selectedIndex = 0;
    for (var i = 0; i < $scope.interpreters.length; i++) {
        if ($scope.interpreters[i].Id === event.userId) {
            $scope.selectedIndex = i;
        }
    }

    $scope.ok = function () {
        $modalInstance.close($scope.interpreter);
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.delete = function () {
        $modalInstance.dismiss('delete');
    };

});

function countAppearences(interpreter, events, period) {
    var from = new Date(period.start);
    var to = new Date(period.end);

    from = getMonday(period.start);
    to.setDate(from.getDate() + 7);

    interpreter.count = 0;
    for (var i = 0; i < events.length; i++) {
        if (events[i].userId == interpreter.Id && from <= events[i].start && events[i].end <= to) {
            interpreter.count++;
        }
    }
}

function getMonday(d) {
    d = new Date(d);
    var day = d.getDay(),
        diff = d.getDate() - day + (day == 0 ? -6 : 1); // adjust when day is sunday
    var monday = new Date(d.setDate(diff));
    monday.setHours(0);
    return monday;
}

function getIntersectingEvents(events, period, currentEvent) {
    var intersectingEvents = [];
    for (var i = 0; i < events.length; i++) {
        if(period.start < events[i].end && events[i].start < period.end) {
            if (currentEvent == null || currentEvent.eventId != events[i].eventId) {
                intersectingEvents.push(events[i]);
            }
        }
    }
    return intersectingEvents
}

function handleIntersectingEvents(interpreters, intersectingEvents) {
    var removeInterpreterIds = [];
    for (var i = 0; i < intersectingEvents.length; i++) {           // check for same interpreter in the same time and add area data
        for (var j = 0; j < interpreters.length; j++) {
            if (intersectingEvents[i].userId === interpreters[j].Id) {
                intersectingEvents[i].areaName = interpreters[j].AreaName;
                removeInterpreterIds.push(interpreters[j].Id);
            }
        }
    }

    for (var i = 0; i < intersectingEvents.length; i++) {           // check for same interpreters in the same area
        for (var j = 0; j < interpreters.length; j++) {
            if (intersectingEvents[i].areaName === interpreters[j].AreaName) {
                removeInterpreterIds.push(interpreters[j].Id);
            }
        }
    }

    for (var i = 0; i < removeInterpreterIds.length; i++) {
        removeInterpreter(interpreters, removeInterpreterIds[i]);
    }
}

function removeInterpreter(interpreters, userId) {
    var found = false;
    for (var i = 0; i < interpreters.length; i++) {
        if (interpreters[i].Id === userId) {
            found = true;
            break;
        }
    }
    if(found) {
        interpreters.splice(i, 1);
    }
};

var isEventInValidPlace = function(events, privateEvents, event) {
    var period = {
        start: event.start,
        end: event.end
    };

    var allEvents = events.concat(privateEvents);

    var intersectingEvents = getIntersectingEvents(allEvents, period, event);
    for (var i = 0; i < intersectingEvents.length; i++) {
        if (intersectingEvents[i].activity == 0 || intersectingEvents[i].activity == 1) {
            if (event.user.AreaName === intersectingEvents[i].user.AreaName) {  // in case of kontakt schedules we check by area match
                return false;
            }
        } else {
            if (event.userId === intersectingEvents[i].userId) {  // in case of private schedules we check by user match
                return false;
            }
        }
    }

    return true;
}

var hasDayAllDayEvent = function(events, day) {
    for (var i = 0; i < events.length; i++) {
        if(events[i].allDay && events[i].start.getTime() == day.getTime()) {
            return true;
        }
    }
    return false;
}
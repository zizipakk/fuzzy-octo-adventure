kontaktScheduleApp.factory('kontaktSchedule', function ($http, $q) {
    
    var closedWeeks = [];
    var events = [];
    var privateEvents = [];
    var interpreterMap = {};
    var instance = {};

    instance.getInterpreters = function () {
        var d = $q.defer();
        $http.get('Interpreters')
            .success(function (data, status) {
                d.resolve(data);
            });
        return d.promise;
    }

    instance.getScheduleItems = function () {
        var d = $q.defer();
        $http.get('ScheduleItems')
            .success(function (data, status) {
                d.resolve(data);
            });
        return d.promise;
    }

    instance.getScheduleItemsPrivate = function () {
        var d = $q.defer();
        $http.get('ScheduleItemsPrivate')
            .success(function (data, status) {
                d.resolve(data);
            });
        return d.promise;
    }

    instance.isLocked = function(day)
    {
        for (var i = 0; i < closedWeeks.length; i++) {
            if (closedWeeks[i].start <= day && day < closedWeeks[i].end ) {
                return true;
            }
        }
        return false;
    }

    var parseOldJSONDate = function (jsonDate) {
        return new Date(parseInt(jsonDate.substr(6)));
    };

    instance.createCalendarEvenets = function (scheduleItems, interpreters) {
        events = [];
        privateEvents = [];

        buildInterpreterMap(interpreters);
        for (var i = 0; i < scheduleItems.length; i++) {
            var calendarEvent = {
                title: scheduleItems[i].DisplayName,
                start: parseOldJSONDate(scheduleItems[i].Start),
                end: parseOldJSONDate(scheduleItems[i].End),
                user: interpreterMap[scheduleItems[i].UserId],
                userId: scheduleItems[i].UserId,
                activity: scheduleItems[i].Activity,
                eventId: scheduleItems[i].Id,
                allDay: (scheduleItems[i].Activity == 1),
                description: scheduleItems[i].Description
            };

            if (scheduleItems[i].Activity == 0 || scheduleItems[i].Activity == 1) {
                events.push(calendarEvent);
            } else {
                privateEvents.push(calendarEvent);
            }
        }
        return events;
    }

    instance.createCalendarEvenetsPrivate = function (scheduleItems, interpreters) {
        events = [];

        buildInterpreterMap(interpreters);
        for (var i = 0; i < scheduleItems.length; i++) {
            var calendarEvent = {
                title: scheduleItems[i].DisplayName,
                start: parseOldJSONDate(scheduleItems[i].Start),
                end: parseOldJSONDate(scheduleItems[i].End),
                user: interpreterMap[scheduleItems[i].UserId],
                userId: scheduleItems[i].UserId,
                activity: scheduleItems[i].Activity,
                eventId: scheduleItems[i].Id,
                allDay: (scheduleItems[i].Activity == 1),
                description: scheduleItems[i].Description
            };

            if (scheduleItems[i].Activity > 1) {
                calendarEvent.title = scheduleItems[i].Description;
                calendarEvent.color = 'DarkGreen';
            }

            events.push(calendarEvent);
        }
        return events;
    }

    instance.getPrivateEvents = function () {
        return privateEvents;
    };

    instance.setClosedWeeks = function (closedWeekArray) {
        closedWeeks = [];
        for (var i = 0; i < closedWeekArray.length; i++) {
            closedWeeks.push({
                start: parseOldJSONDate(closedWeekArray[i].SchedulePeriodStart),
                end: parseOldJSONDate(closedWeekArray[i].SchedulePeriodEnd)
            });
        }
    }

    var buildInterpreterMap = function (interpreters) {
        interpreterMap = {};
        for (var i = 0; i < interpreters.length; i++) {
            interpreterMap[interpreters[i].Id] = interpreters[i];
        }
    }

    return instance;
});
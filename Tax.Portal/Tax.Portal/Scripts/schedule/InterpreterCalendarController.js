kontaktScheduleApp.controller('InterpreterCalendarController', function ($scope, $http, $modal, $q, kontaktSchedule, calendarConfig) {

    $scope.view = { name: 'agendaWeek' };
    $scope.week = { editable: true, past: false };
    $scope.eventSources = [];
    $scope.interpreters = {};
    $scope.interpreter = {};

    $scope.loadEventsAndInterpreters = function () {
        $q.all([
           kontaktSchedule.getScheduleItemsPrivate(),
           kontaktSchedule.getInterpreters(),
        ]).then(function (data) {
            $scope.interpreters = data[1];
            $scope.eventSources[0] = kontaktSchedule.createCalendarEvenetsPrivate(data[0], data[1]);
        });
    };

    $scope.uiConfig = calendarConfig.uiConfig;
    $scope.uiConfig.calendar.eventDurationEditable = false;
    $scope.uiConfig.calendar.eventStartEditable = false;
    //$scope.uiConfig.calendar.editable = false;
    //$scope.uiConfig.calendar.selectable = false;
    //$scope.uiConfig.calendar.selectHelper = false;

    $scope.loadClosedWeeks = function () {
        $http.get('ClosedSchedulePeriods')
        .then(function (result) {
            kontaktSchedule.setClosedWeeks(result.data);
            $scope.week.editable = !$scope.week.past && !kontaktSchedule.isLocked($scope.view.start);
        });
    }

    $scope.loadIntereterProfile = function () {
        $http.get('IntereterProfile')
        .then(function (result) {
            $scope.interpreter = result.data;
        });
    }

    $scope.onViewRender = function (view, element) {
        $scope.week.past = view.end < new Date();
        $scope.week.editable = !$scope.week.past && !kontaktSchedule.isLocked(view.start);
        $scope.view.name = view.name;
        $scope.view.start = view.start;
        $scope.view.end = view.end;
    }

    $scope.saveEvent = function (event) {
        return $http.post('SaveSchedule',
            {
                Id: event.eventId,
                Start: event.start,
                End: event.allDay ? event.start : event.end,
                Activity: event.activity,
                Description: event.activity > 1 ? event.title : "",
                UserId: event.userId
            });
    }

    $scope.addEvent = function (start, end, description) {
        var event = {
            title: description,
            start: start,
            end: end,
            allDay: false,
            eventId: calendarConfig.guid(),
            userId: $scope.interpreter.Id,
            user: $scope.interpreter,
            activity: 2,
            description: description,
            color: 'DarkGreen'
        };

        $scope.saveEvent(event)
        .then(function () {
            $scope.eventSources[0].push(event);
        }, function (error) {
            if (error.status == 409) {
                alert("Nem sikerület az esemény rögzítése, a diszpécser önt az adott időpontra már beosztotta.");
            } else {
                alert("Nem sikerült az esemény rögzítése");
            }

        })
    };

    $scope.deleteEvent = function (event) {
        return $http.post('DeleteSchedule',
            {
                Id: event.eventId
            });
    }

    $scope.removeEvent = function (event) {
        var events = $scope.eventSources[0];
        for (var i = 0; i < events.length; i++) {
            if (events[i].eventId === event.eventId) {
                break;
            }
        }
        events.splice(i, 1);
    };

    $scope.onSelect = function (startDate, endDate, allDay, jsEvent, view) {
        if (!jsEvent.shiftKey && !$scope.week.past && $scope.week.editable && !allDay) {

            var modalInstance = $modal.open({
                templateUrl: 'addPrivateEventModal.html',
                controller: 'AddPrivateEventModalController',
                resolve: {
                    period: function () { return { start: startDate, end: endDate, allDay: allDay } }
                }
            });

            modalInstance.result.then(function (description) {
                $scope.addEvent(startDate, endDate, description);
            });

            $scope.rawCalendar.fullCalendar('unselect');
        }
    };

    $scope.onEventClick = function (event, allDay, jsEvent, view) {
        if (!$scope.week.past && $scope.week.editable && event.activity > 1) {
            var modalInstance = $modal.open({
                templateUrl: 'editPrivateEventModal.html',
                controller: 'EditPrivateEventModalController',
                resolve: {
                    event: function () { return event; }
                }
            });

            modalInstance.result.then(function (description) {
                if (event.title != description) {
                    event.title = description;
                    $scope.saveEvent(event);
                }
            }, function (reason) {
                if (reason === 'delete') {
                    $scope.deleteEvent(event)
                    .then(function () {
                        $scope.removeEvent(event);
                    });
                }
            });
        }
    };

    $scope.uiConfig.calendar.select = $scope.onSelect;
    $scope.uiConfig.calendar.eventClick = $scope.onEventClick;

    $scope.uiConfig.calendar.viewRender = $scope.onViewRender;
    
    $scope.loadIntereterProfile();
    $scope.loadClosedWeeks();
    $scope.loadEventsAndInterpreters();

});

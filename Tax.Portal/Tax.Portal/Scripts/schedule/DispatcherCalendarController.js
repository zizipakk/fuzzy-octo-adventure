kontaktScheduleApp.controller('DispatcherCalendarController', function ($scope, $modal, $http, $q, $cookieStore, kontaktSchedule, calendarConfig) {

    $scope.view = { name: 'agendaWeek' };
    $scope.week = { editable: true, past: false };
    $scope.eventSources = [];
    $scope.privateEvents = [];
    $scope.interpreters = {};

    $scope.uiConfig = calendarConfig.uiConfig;

    $scope.loadEventsAndInterpreters = function () {
        $q.all([
           kontaktSchedule.getScheduleItems(),
           kontaktSchedule.getInterpreters(),
        ]).then(function (data) {
            $scope.interpreters = data[1];
            $scope.eventSources[0] = kontaktSchedule.createCalendarEvenets(data[0], data[1]);
            $scope.privateEvents = kontaktSchedule.getPrivateEvents();
        });
    }

    $scope.loadClosedWeeks = function () {
        $http.get('ClosedSchedulePeriods')
        .then(function (result) {
            kontaktSchedule.setClosedWeeks(result.data);
            $scope.week.editable = !$scope.week.past && !kontaktSchedule.isLocked($scope.view.start);
        });
    }

    $scope.saveEvent = function(event) {
        return $http.post('SaveSchedule', 
            {
                Id: event.eventId,
                Start: event.start,
                End: event.allDay ? event.start : event.end,
                Activity: event.activity,
                UserId: event.userId
            });
    }

    $scope.deleteEvent = function (event) {
        return $http.post('DeleteSchedule',
            {
                Id: event.eventId
            });
    }

    $scope.closeWeek = function () {
        if (confirm("Biztos benne, hogy befejezi a heti beosztás szerkesztést?")) {
            $http.post('CloseSchedule',
                {
                    start: $scope.view.start,
                    end: $scope.view.end
                })
            .then(function () {
                $scope.loadClosedWeeks();
            })
        }
    }

    $scope.generateSchedule = function () {

        var start = $scope.view.start;
        var end = $scope.view.end;

        var modalInstance = $modal.open({
            templateUrl: 'generateSchedule.html',
            controller: 'GenerateModalController',
            resolve: {
                period: function () { return { start: start, end: end }; }
            }
        });

        modalInstance.result.then(function (result) {
            $http.post('GenerateSchedule', result)
            .then(function () {
               location.reload();
            }, function (error) {
                alert("Ezekkel a paraméterekkel nem sikerült a beosztás generálás!");
            })
        });

    }

    $scope.openWeek = function () {
        if (confirm("Biztos benne, hogy újra megnyitja heti beosztás szerkesztést?")) {
            $http.post('OpenSchedule',
                {
                    start: $scope.view.start,
                    end: $scope.view.end
                })
            .then(function () {
                $scope.loadClosedWeeks();
            })
        }
    }

    $scope.onEventDrop = function (event, dayDelta, minuteDelta, allDay, revertFunc, jsEvent, ui, view) {
        if (!$scope.week.past &&
            event.activity != 1 &&
            isEventInValidPlace($scope.eventSources[0], $scope.privateEvents, event))
        {
            $scope.saveEvent(event)
            .then(function () {

            }, function () {
                revertFunc();
            });
        } else {
            revertFunc();
        }
    };

    $scope.onEventClick = function (event, allDay, jsEvent, view) {
        if (!$scope.week.past) {
            var modalInstance = $modal.open({
                templateUrl: 'editEventModal.html',
                controller: 'EditEventModalController',
                resolve: {
                    interpreters: function () {
                        return $scope.interpreters;
                    },
                    event: function () { return event; },
                    events: function () { return $scope.eventSources[0]; },
                    privateEvents: function () {return $scope.privateEvents; }
                }
            });

            modalInstance.result.then(function (selectedInterpreter) {
                $scope.changeInterpreter(event, selectedInterpreter);
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

    $scope.onEventResize = function (event, dayDelta, minuteDelta, revertFunc, jsEvent, ui, view) {
        if ($scope.week.past || !isEventInValidPlace($scope.eventSources[0], $scope.privateEvents, event)) {
            revertFunc();
        }
        else {
            $scope.saveEvent(event)
            .then(function () {

            }, function () {
                revertFunc();
            });
        }
    };

    $scope.onViewRender = function (view, element) {
        $scope.week.past = view.end < new Date();
        $scope.week.editable = !$scope.week.past && !kontaktSchedule.isLocked(view.start);
        $scope.view.name = view.name;
        $scope.view.start = view.start;
        $scope.view.end = view.end;

        $cookieStore.put("calendarStart", view.start);
    }

    $scope.addEvent = function (interpreter, start, end, allDay) {
        var event = {
            title: interpreter.DisplayName,
            start: start,
            end: end,
            allDay: allDay,
            eventId: calendarConfig.guid(),
            userId: interpreter.Id,
            user: interpreter,
            activity: allDay ? 1 : 0
        };
        
        $scope.saveEvent(event)
        .then(function() {
            $scope.eventSources[0].push(event);
        }, function() {

        })
    };

    $scope.changeInterpreter = function (event, interpreter) {

        var origUserId = event.userId;
        var origTitle = event.title;
        var origUser = event.user;

        event.userId = selectedInterpreter.Id;
        event.title = selectedInterpreter.DisplayName;
        event.user = selectedInterpreter;

        $scope.saveEvent(event)
        .then(function () {
            
        }, function () {
            event.userId = origUserId;
            event.title = origTitle;
            event.user = origUser;
        })
    };

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
        if (!jsEvent.shiftKey && !$scope.week.past) {

            if (allDay && hasDayAllDayEvent($scope.eventSources[0], startDate)) {
                return;
            }

            var modalInstance = $modal.open({
                templateUrl: 'addEventModal.html',
                controller: 'AddEventModalController',
                resolve: {
                    interpreters: function () {
                        return $scope.interpreters;
                    },
                    period: function () { return { start: startDate, end: endDate, allDay: allDay } },
                    events: function () { return $scope.eventSources[0]; },
                    privateEvents: function () { return $scope.privateEvents; }
                }
            });

            modalInstance.result.then(function (selectedInterpreter) {
                $scope.addEvent(selectedInterpreter, startDate, endDate, allDay);
            });

            $scope.rawCalendar.fullCalendar('unselect');
        }
    };

    $scope.onDayClick = function (date, allDay, jsEvent, view) {
        if (jsEvent.shiftKey) {
            $scope.rawCalendar.fullCalendar('gotoDate', date);
            $scope.rawCalendar.fullCalendar('changeView', 'agendaDay');
        }
    }

    $scope.uiConfig.calendar.dayClick = $scope.onDayClick;
    $scope.uiConfig.calendar.select = $scope.onSelect;
    $scope.uiConfig.calendar.eventClick = $scope.onEventClick;
    $scope.uiConfig.calendar.eventResize = $scope.onEventResize;
    $scope.uiConfig.calendar.eventDrop = $scope.onEventDrop;

    $scope.uiConfig.calendar.viewRender = $scope.onViewRender;
    
    var startTime = $cookieStore.get("calendarStart");
    if (startTime) {
        var startTimeParsed = new Date(Date.parse(startTime));
        $scope.uiConfig.calendar.year = startTimeParsed.getFullYear();
        $scope.uiConfig.calendar.month = startTimeParsed.getMonth();
        $scope.uiConfig.calendar.date = startTimeParsed.getDate();
    }

    $scope.loadClosedWeeks();
    $scope.loadEventsAndInterpreters();
});

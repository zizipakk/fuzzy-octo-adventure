kontaktScheduleApp.factory('calendarConfig', function () {
    var instance = {};

    instance.localConfig = {
        dayNames: ['Vasárnap', 'Hétfő', 'Kedd', 'Szerda', 'Csütörtök', 'Péntek', 'Szombat'],
        dayNamesShort: ['Vas', 'Hét', 'Kedd', 'Sze', 'Csüt', 'Pén', 'Szo'],
        monthNames: ['Január', 'Február', 'Március', 'Április', 'Május', 'Június', 'Július', 'Augusztus', 'Szeptember', 'Október', 'November', 'December'],
        monthNamesShort: ['Jan', 'Febr', 'Márc', 'Ápr', 'Máj', 'Jún', 'Júl', 'Aug', 'Szept', 'Okt', 'Nov', 'Dec']
    };

    instance.uiConfig = {
        calendar: {
            height: 450,
            editable: true,
            header: {
                left: 'prev,next',
                center: 'title',
                right: 'agendaWeek,agendaDay'
            },
            defaultView: 'agendaWeek',
            allDaySlot: true,
            allDayDefault: false,
            allDayText: "Ügyelet" ,
            slotMinutes: 60,
            firstDay: 1,
            firstHour: 8,
            minTime: 8,
            maxTime: 16,
            eventDurationEditable: true,
            slotEventOverlap: false,
            selectable: true,
            selectHelper: true,
            buttonText: {
                week: 'hét',
                day: 'nap'
            },
            timeFormat: {
                agenda: 'H:mm{ - H:mm}',
                '': 'H(:mm)'
            },
            columnFormat: {
                month: 'ddd',
                week: 'ddd M. d.',
                day: 'dddd M. d.'
            },
            titleFormat: {
                month: 'MMMM yyyy',
                week: "MMMM d.[ yyyy]{ '&#8212;'[ MMMM] d. yyyy.}",
                day: 'dddd, MMMM d. yyyy.'
            },
            axisFormat: 'H:mm',
            dayNames: instance.localConfig.dayNames,
            dayNamesShort: instance.localConfig.dayNamesShort,
            monthNames: instance.localConfig.monthNames,
            monthNamesShort: instance.localConfig.monthNamesShort
        }
    };

    var s4 = function() {
        return Math.floor((1 + Math.random()) * 0x10000)
                   .toString(16)
                   .substring(1);
    };

    instance.guid = function guid() {
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
               s4() + '-' + s4() + s4() + s4();
    }

    return instance;
});
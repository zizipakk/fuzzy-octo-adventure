kontaktScheduleApp.controller('GenerateModalController', function ($scope, $modalInstance, calendarConfig, period) {

    $scope.period = period;
    $scope.period.weekName = $.fullCalendar.formatDate($scope.period.start, 'MMMM d.', calendarConfig.localConfig) + " - " + $.fullCalendar.formatDate($scope.period.end, 'MMMM d.', calendarConfig.localConfig);
    $scope.interpreterNumbers = [1, 2, 3, 4, 5];

    $scope.result = {
        start: $scope.period.start,
        end: $scope.period.end,
        selectedNumber: 1,
        selectedDays: [true, true, true, true, true, false, false]
    };

    $scope.ok = function () {
        $modalInstance.close($scope.result);
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

});
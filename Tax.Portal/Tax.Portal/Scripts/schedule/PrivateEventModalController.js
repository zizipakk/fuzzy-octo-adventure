kontaktScheduleApp.controller('AddPrivateEventModalController', function ($scope, $modalInstance, calendarConfig, period) {

    $scope.period = period;
    $scope.result = { description: "" };

    $scope.ok = function () {
        if ($scope.result.description != "") {
            $modalInstance.close($scope.result.description);
        }
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

});

kontaktScheduleApp.controller('EditPrivateEventModalController', function ($scope, $modalInstance, calendarConfig, event) {

    $scope.period = {
        start: event.start,
        end: event.end
    };
    $scope.result = { description: event.title };

    $scope.ok = function () {
        if ($scope.result.description != "") {
            $modalInstance.close($scope.result.description);
        }
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };

    $scope.delete = function () {
        $modalInstance.dismiss('delete');
    };

});
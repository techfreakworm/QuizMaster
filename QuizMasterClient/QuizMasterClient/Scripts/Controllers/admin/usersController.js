adminPortalApp.controller('usersController', ['$scope', '$http', function ($scope, $http) {

    var adminDetails;
    var presenterDetails;

    //ADMIN FUNCTIONS
    $scope.editAdminUser = function (index) {

    };

    $scope.deleteAdminUser = function (index) {

    };

    $scope.initAdmin = function () {

        var config = {
            headers: {
                'Content-Type':'application/json'
            }
        };

        $http.post("")
    };


    //PRESENTER FUNCTIONS
    $scope.editPresenterUser = function (index) {

    };

    $scope.deletePresenterUser = function (index) {

    };

    $scope.initPresenter = function () {

    };
}]);
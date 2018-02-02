presenterApp.controller('teamsController', ['$scope', '$http', '$cookies', function ($scope, $http, $cookies)  {

    $scope.initTeam = function () { //initialize team table
        var config = {
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + $cookies.getObject('currentUser').token
            }
        };

        $http.get("http://localhost:50827/api/team/get",  config).then(function (successResponse) {
            $scope.teams = successResponse.data;
        }, function (errorResponse) {
            console.log("Cannot fetch all admin user details");
        });
    };

}]);
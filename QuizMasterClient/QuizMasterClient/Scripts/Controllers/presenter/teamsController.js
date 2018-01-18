presenterApp.controller('teamsController', ['$scope', '$http', '$cookies', function ($scope, $http, $cookies)  {

    $scope.initTeam = function () { //initialize admin table
        var config = {
            headers: {
                'Content-Type': 'application/json'
            }
        };

        $http.post("http://localhost:50827/api/team/get", $cookies.getObject('user'), config).then(function (successResponse) {
            $scope.teams = successResponse.data;
        }, function (errorResponse) {
            console.log("Cannot fetch all admin user details");
        });
    };

}]);
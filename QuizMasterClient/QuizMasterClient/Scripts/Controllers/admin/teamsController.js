adminPortalApp.controller('teamsController', ['$scope', '$http', '$cookies', function ($scope, $http, $cookies)  {

    //GLOBALS
    $scope.isTeamAddVisible = false;
    $scope.isTeamEditVisible = false;
    $scope.isTeamVisible = false;

    //FUNCTIONS
    //ADMIN FUNCTIONS
    $scope.addTeam = function () { //Unhide add team menu
        $scope.isTeamVisible = true;
        $scope.isTeamAddVisible = true;
    };

    $scope.editTeam = function (index) { //Unhide edit presenter menu
        $scope.isTeamVisible = true;
        $scope.isTeamEditVisible = true;
        $scope.editTeam = $scope.teams[index];
    };

    $scope.deleteTeam = function (index) { //delete admin user
        var config = {
            data: $cookies.getObject('user'),
            headers: {
                'Content-Type': 'application/json'
            }
        };

        var uri = 'http://localhost:50827/api/team/' + $scope.teams[index].TeamId;
        $http.delete(uri, config).then(function (successResponse) {
            $scope.initTeam();
        }, function (errorResponse) {

        });
    };

    $scope.initTeam = function () { //initialize admin table
        var config = {
            headers: {
                'Content-Type': 'application/json'
            }
        };

        $http.post("http://localhost:50827/api/team/get", $cookies.getObject('user'), config).then(function (successResponse) {
            $scope.teams = successResponse.data;
            //console.log(adminDetails);
        }, function (errorResponse) {
            console.log("Cannot fetch all admin user details");
        });
    };

    $scope.saveTeamEdit = function () {//Edit an admin user
        var config = {
            headers: {
                'Content-Type': 'application/json'
            }
        };

        var data = {
            currentUser: $cookies.getObject('user'),
            team: {
                'TeamId': $scope.editTeam.TeamId,
                'TeamName': $scope.editTeam.TeamName,
                'TeamMembers': $scope.editTeam.TeamMembers
            }
        };

        console.log(data);

        var uri = 'http://localhost:50827/api/team/' + $scope.editTeam.TeamId;

        $http.put(uri, data, config).then(function (successResponse) {
            $scope.isTeamVisible = false;
            $scope.isTeamEditVisible = false;

        }, function (errorResponse) {
            console.log(errorResponse);

        });


    };

    $scope.saveTeamAdd = function () {//Add an admin user
        var config = {
            headers: {
                'Content-Type': 'application/json'
            }
        };

        var data = {
            currentUser: $cookies.getObject('user'),
            team: {
                'TeamName': $scope.addTeam.TeamName,
                'TeamMembers': $scope.addTeam.TeamMembers
            }
        };

        $http.post('http://localhost:50827/api/team', data, config).then(function (successResponse) {
            $scope.isTeamVisible = false;
            $scope.isTeamAddVisible = false;
            $scope.initTeam();

        }, function (errorResponse) {
            console.log(errorResponse);

        });
    };

}]);
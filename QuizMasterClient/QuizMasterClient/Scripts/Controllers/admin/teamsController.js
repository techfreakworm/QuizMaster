adminPortalApp.controller('teamsController', ['$scope', '$http', '$cookies',  function ($scope, $http, $cookies)  {

    //GLOBALS
    $scope.isTeamAddVisible = false;
    $scope.isTeamEditVisible = false;
    $scope.isTeamVisible = false;

    //FUNCTIONS
    //TEAM FUNCTIONS
    $scope.addTeam = function () { //Unhide add team menu
        $scope.isTeamVisible = true;
        $scope.isTeamAddVisible = true;
    };

    $scope.editTeam = function (index) { //Unhide edit team menu
        $scope.isTeamVisible = true;
        $scope.isTeamEditVisible = true;
        $scope.editTeam = $scope.teams[index];
    };

    $scope.deleteTeam = function (index) { //delete team
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

    $scope.initTeam = function () { //initialize team table
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

    $scope.saveTeamEdit = function () {//Edit a team
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
            console.log("Team edit successful!")
        }, function (errorResponse) {
            console.log(errorResponse);

        });


    };

    $scope.saveTeamAdd = function () {//Add a team
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
            console.log("Team added!");
        }, function (errorResponse) {
            console.log(errorResponse);

        });
    };

}]);
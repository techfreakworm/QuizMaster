presenterApp.controller('questionsController', ['$scope', '$http', '$cookies', function ($scope, $http, $cookies) {

    var teamIndex = 0;
    $scope.teams = [];
    var isPassed = 0;
    

    $scope.getQuestion = function () { 
        var config = {
            headers: {
                'Content-Type': 'application/json'
            }
        };

        $http.post("http://localhost:50827/api/question/random", $cookies.getObject('user'), config).then(function (successResponse) {
            $scope.currentQuestion = successResponse.data;
            $scope.selectedOption = $scope.currentQuestion.OptionOne;

        }, function (errorResponse) {
            console.log("Problem in displaying question");
        });
    };

    $scope.initTeam = function () {
        var config = {
            headers: {
                'Content-Type': 'application/json'
            }
        };

        $http.post("http://localhost:50827/api/team/get", $cookies.getObject('user'), config).then(function (successResponse) {
            $scope.teams = successResponse.data;
            $scope.currentTeam = $scope.teams[teamIndex];
        }, function (errorResponse) {
            console.log("Cannot fetch all team details");
        });
    };

    $scope.submitAnswer = function () {
        var config = {
            headers: {
                'Content-Type': 'application/json'
            }
        };

        var data = {
            currentUser: $cookies.getObject('user'),
            qId: $scope.currentQuestion.QId,
            tId: $scope.currentTeam.TeamId,
            answer: $scope.selectedOption,
            PassFlag: isPassed
        };

        $http.post("http://localhost:50827/api/question/check", data, config).then(function (successResponse) {
            console.log("Answer submitted successfully");
            changeTeam();
            isPassed = 0;
        }, function (errorResponse) {
            console.log("Cannot fetch all team details");
        });
    };

    $scope.passQuestion = function (passedQuestionDetails) {
        isPassed = 1;
    };

    function changeTeam() {
        teamIndex++;
        if (teamIndex > ($scope.teams.length - 1))
            teamIndex = 0;
        $scope.currentTeam = teams[teamIndex];
    };
}]);
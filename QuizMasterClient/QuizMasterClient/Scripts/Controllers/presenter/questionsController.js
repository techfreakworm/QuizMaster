presenterApp.controller('questionsController', ['$scope', '$http', '$cookies', function ($scope, $http, $cookies) {

    var teamIndex = 0;
    var isPassed = 0;
    var teams;
    

    $scope.getQuestion = function () { 
        var config = {
            headers: {
                'Content-Type': 'application/json'
            }
        };

        $http.post("http://localhost:50827/api/question/random", $cookies.getObject('user'), config).then(function (successResponse) {
            $scope.currentQuestion = successResponse.data;
            $scope.selectedOption = $scope.currentQuestion.OptionOne;
            alert("Random Question generated!");
        }, function (errorResponse) {
            alert("Cannot fetch question from server!");
        });
    };

    $scope.initTeam = function () {
        var config = {
            headers: {
                'Content-Type': 'application/json'
            }
        };

        $http.post("http://localhost:50827/api/team/get", $cookies.getObject('user'), config).then(function (successResponse) {
            teams = successResponse.data;
            $scope.currentTeam = teams[teamIndex];
            alert("Team details retreival completed!");
        }, function (errorResponse) {
            alert("Cannot fetch team details!");
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
            alert("Correct answer");
            changeTeam();
            isPassed = 0;
            $scope.getQuestion();
        }, function (errorResponse) {
            alert("InCorrect answer");
        });
    };

    $scope.passQuestion = function (passedQuestionDetails) {
        isPassed = 1;
        changeTeam();
    };

    function changeTeam() {
        teamIndex++;
        if (teamIndex > (teams.length - 1))
            teamIndex = 0;
        $scope.currentTeam = teams[teamIndex];
    };

    $scope.initData = function () {
        $scope.initTeam();
        $scope.getQuestion();
    };
}]);
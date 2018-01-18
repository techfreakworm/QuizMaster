presenterApp.controller('questionsController', ['$scope', '$http', '$cookies', function ($scope, $http, $cookies) {

    //GLOBALS
    var teamIndex = 0;
    var isPassed = 0;
    var teams;
    
    //FUNCTIONS
    $scope.getQuestion = function () { //Gets a random question from server
        var config = {
            headers: {
                'Content-Type': 'application/json'
            }
        };

        $http.post("http://localhost:50827/api/question/random", $cookies.getObject('user'), config).then(function (successResponse) {
            $scope.currentQuestion = successResponse.data;
            $scope.selectedOption = $scope.currentQuestion.OptionOne;
            if (successResponse.data == "")
                alert("No questions left, QUIZ OVER");
            else
                alert("Random question generated.");
            isPassed = 0;
        }, function (errorResponse) {
            alert("Cannot fetch question from server!");
        });
    };

    $scope.initTeam = function () { //Initialize team info onload tab
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

    $scope.submitAnswer = function () { //Submit and check the answer to server
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
            alert(successResponse.data.Message);
            changeTeam();
            isPassed = 0;
            $scope.getQuestion();
        }, function (errorResponse) {
            alert("Error occured while submitting answer.");
        });
    };

    $scope.passQuestion = function (passedQuestionDetails) { ////LOGIC: Pass question to another team
        isPassed = 1;
        changeTeam();
    };

    function changeTeam() { ////LOGIC: Change team 
        teamIndex++;
        if (teamIndex > (teams.length - 1))
            teamIndex = 0;
        $scope.currentTeam = teams[teamIndex];
    };

    $scope.initData = function () {// Initialize team and question data onload
        $scope.initTeam();
        $scope.getQuestion();
    };
}]);
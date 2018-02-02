presenterApp.controller('questionsController', ['$scope', '$http', '$cookies', function ($scope, $http, $cookies) {

    //GLOBALS
    var teamIndex = 0;
    var teamIndexPassed;
    var isPassed = 0;
    var teams;
    
    //FUNCTIONS
    $scope.getQuestion = function () { //Gets a random question from server
        var config = {
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + $cookies.getObject('currentUser').token
            }
        };

        $http.get("http://localhost:50827/api/question/random", config).then(function (successResponse) {
            $scope.currentQuestion = successResponse.data;
            $scope.selectedOption = $scope.currentQuestion.OptionOne;
            if (successResponse.data == "")
                alert("No questions left, QUIZ OVER");
            $scope.isPassed = 0;
        }, function (errorResponse) {
            alert("Cannot fetch question from server!");
        });
    };

    $scope.initTeam = function () { //Initialize team info onload tab
        var config = {
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + $cookies.getObject('currentUser').token
            }
        };

        $http.get("http://localhost:50827/api/team/get", config).then(function (successResponse) {
            teams = successResponse.data;
            $scope.currentTeam = teams[teamIndex];
            //alert("Team details retreival completed!");
        }, function (errorResponse) {
            alert("Cannot fetch team details!");
        });
    };

    $scope.submitAnswer = function () { //Submit and check the answer to server
        var config = {
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + $cookies.getObject('currentUser').token
            }
        };

        var data = {
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
        
        if (isPassed != 1) {
            teamIndexPassed = teamIndex;
        }
        isPassed = 1;
        teamIndexPassed++;
        if (teamIndexPassed > teams.length - 1) {
            $scope.getQuestion();
            teamIndex++;
            $scope.currentTeam = teams[teamIndex];
        }

        else
            $scope.currentTeam = teams[teamIndexPassed];
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
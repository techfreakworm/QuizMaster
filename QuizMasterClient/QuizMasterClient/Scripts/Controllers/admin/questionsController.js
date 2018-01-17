adminPortalApp.controller('questionsController', ['$scope', '$http', '$cookies', function ($scope, $http, $cookies) {


    //GLOBALS
    $scope.isQuestionAddVisible = false;
    $scope.isQuestionEditVisible = false;
    $scope.isQuestionVisible = false;

    //FUNCTIONS
    //QUESTION FUNCTIONS
    $scope.addQuestion = function () { //Unhide add question menu
        $scope.isQuestionVisible = true;
        $scope.isQuestionAddVisible = true;
    };

    $scope.editQuestion = function (index) { //Unhide edit question menu
        $scope.isQuestionVisible = true;
        $scope.isQuestionEditVisible = true;
        $scope.editQuestion = $scope.questions[index];
    };

    $scope.deleteQuestion = function (index) { //delete question
        var config = {
            data: $cookies.getObject('user'),
            headers: {
                'Content-Type': 'application/json'
            }
        };

        var uri = 'http://localhost:50827/api/question/' + $scope.questions[index].QId;
        $http.delete(uri, config).then(function (successResponse) {
            $scope.initQuestion();
        }, function (errorResponse) {

        });
    };

    $scope.initQuestion = function () { //initialize admin table
        var config = {
            headers: {
                'Content-Type': 'application/json'
            }
        };

        $http.post("http://localhost:50827/api/question/get", $cookies.getObject('user'), config).then(function (successResponse) {
            $scope.questions = successResponse.data;
            //console.log(adminDetails);
        }, function (errorResponse) {
            console.log("Cannot fetch all admin user details");
        });
    };

    $scope.saveQuestionEdit = function () {//Edit a question
        var config = {
            headers: {
                'Content-Type': 'application/json'
            }
        };

        var data = {
            currentUser: $cookies.getObject('user'),
            question: {
                'QId': $scope.editQuestion.QId,
                'Ques': $scope.editQuestion.Ques,
                'OptionOne': $scope.editQuestion.OptionOne,
                'OptionTwo': $scope.editQuestion.OptionTwo,
                'OptionThree': $scope.editQuestion.OptionThree,
                'OptionFour': $scope.editQuestion.OptionFour,
                'Answer': $scope.editQuestion.Answer
            }
        };

        console.log(data);

        var uri = 'http://localhost:50827/api/question/' + $scope.editQuestion.QId;

        $http.put(uri, data, config).then(function (successResponse) {
            $scope.isQuestionVisible = false;
            $scope.isQuestionEditVisible = false;

        }, function (errorResponse) {
            console.log(errorResponse);

        });


    };

    $scope.saveQuestionAdd = function () {//Add an admin user
        var config = {
            headers: {
                'Content-Type': 'application/json'
            }
        };

        var data = {
            currentUser: $cookies.getObject('user'),
            question: {
                'QId': $scope.addQuestion.QId,
                'Ques': $scope.addQuestion.Ques,
                'OptionOne': $scope.addQuestion.OptionOne,
                'OptionTwo': $scope.addQuestion.OptionTwo,
                'OptionThree': $scope.addQuestion.OptionThree,
                'OptionFour': $scope.addQuestion.OptionFour,
                'Answer': $scope.addQuestion.Answer
            }
        };

        $http.post('http://localhost:50827/api/question', data, config).then(function (successResponse) {
            $scope.isQuestionVisible = false;
            $scope.isQuestionAddVisible = false;
            $scope.initQuestion();

        }, function (errorResponse) {
            console.log(errorResponse);

        });
    };

}]);
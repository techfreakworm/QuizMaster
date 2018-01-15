loginApp.controller('loginPortalController', ['$scope', '$http', '$cookies', '$window', function($scope, $http, $cookies, $window){

    $scope.email = '';
    $scope.password = '';
    $scope.responseMessage = '';
    $scope.isSubmitButtonDisabled = false;

	$scope.loginSubmit = function(){

        var userdata = {
            'userName': $scope.email,
            'userPass': $scope.password
        };

        var config = {
            headers: {
                'Content-Type': 'application/json'
            }
        };

        $http.post('http://localhost:50827/api/Users/Login', userdata, config).then(function (successResponse) {
            $scope.isSubmitButtonDisabled = true;
            $cookies.putObject('user', userdata);
            $window.location.href = 'Views/adminPortal.html'
        }, function (errorResponse) {
            $scope.responseMessage = 'Email or Password is incorrect';
        });


		//$http({
		//	method: 'POST',
		//	url: 'http://localhost:50827/api/Users/Login',
		//	data: userdata,
		//	dataType: 'json',
		//	headers: {
		//		'Content-Type': 'application/json'
		//	}
		//});
	}
}]);
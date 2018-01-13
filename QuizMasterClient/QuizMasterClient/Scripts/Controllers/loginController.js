app.controller('loginController', ['$scope', '$http', function($scope, $http){
	
	$scope.emailMessage = '';
	$scope.passwordMessage = '';
    $scope.successResponse = '';
    $scope.errorResponse = '';

	$scope.loginSubmit = function(){

        var userdata = {
            'userName': $scope.email,
            'userPass': $scope.password
        }

		console.log(userdata);

        $http.get()
		$http({
			method: 'POST',
			url: 'http://localhost:50827/api/Users/Login',
			data: userdata,
			dataType: 'json',
			headers: {
				'Content-Type': 'application/json'
			}
		});
	}
}]);
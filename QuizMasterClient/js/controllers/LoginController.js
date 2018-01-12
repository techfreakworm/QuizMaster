app.controller('LoginController', ['$scope', function($scope){
	
	$scope.email= '';
	$scope.password= '';

	$scope.loginSubmit = function($scope, $http, $log){
		$http({
			method: 'GET',
			url: "api/Users",
			contentType: "json"
		}).then(function successCallback(response){
			//Success callback
		}, function errorCallback(response){
			//Error callback
		});
	}
}]);
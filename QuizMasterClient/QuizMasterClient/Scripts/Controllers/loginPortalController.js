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

        $http.post('http://localhost:50827/api/user/login', userdata, config).then(function (successResponse) {
            $scope.isSubmitButtonDisabled = true;
            var currentUserData = {
                'userName': $scope.email,
                'userType': successResponse.data.UserType,
                'token': successResponse.data.token
            };
            $cookies.putObject('currentUser', currentUserData );
            // Pass authorization token with every request
          //  $http.defaults.headers.common.Authorization = 'Bearer ' + $cookies.getObject('currentUser').token;
            if ($cookies.getObject('currentUser').userType == "admin") {
                $window.location.href = 'Views/admin/adminPortal.html';
            }
            else
                $window.location.href = 'Views/presenter/PresenterPortal.html';
            
            
        }, function (errorResponse) {
            $scope.responseMessage = 'Email or Password is incorrect';
        });
    }

    $scope.init = function () {
        var currentUser = $cookies.getObject('currentUser');

        if (currentUser != null) {
            var config = {
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer ' + currentUser.token
                }
            };

            $http.get('http://localhost:50827/api/user/verify', config).then(function (successResponse) {
                if (successResponse.data == "admin") {
                    $window.location.href = 'Views/admin/adminPortal.html';
                }
                else
                    $window.location.href = 'Views/presenter/PresenterPortal.html';
            }, function (errorResponse) {

            });
        }
    };
}]);
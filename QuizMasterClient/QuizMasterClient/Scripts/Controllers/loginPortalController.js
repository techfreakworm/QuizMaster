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
            $cookies.putObject('user', userdata);
            if (successResponse.data == "admin") {
                $window.location.href = 'Views/admin/adminPortal.html';
            }
            else
                $window.location.href = 'Views/presenter/PresenterPortal.html';
            
            
        }, function (errorResponse) {
            $scope.responseMessage = 'Email or Password is incorrect';
        });
    }

    $scope.init = function () {
        var userdata = $cookies.getObject('user');

        if (userdata != null) {
            var config = {
                headers: {
                    'Content-Type': 'application/json'
                }
            };
            $http.post('http://localhost:50827/api/user/login', userdata, config).then(function (successResponse) {
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
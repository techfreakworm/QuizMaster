adminPortalApp.controller('profileController', ['$scope', '$http', '$cookies', function ($scope, $http, $cookies) {

    var currentUser = $cookies.getObject('currentUser');

    var config = {
        headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + currentUser.token
        }
    };

    $http.get('http://localhost:50827/api/user/verify', config).then(function (successResponse) {
        $scope.userName = successResponse.data;
    }, function (errorResponse) {

    });

}]);
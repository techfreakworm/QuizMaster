presenterApp.controller('presenterPortalController', ['$scope', '$cookies','$window', function ($scope, $cookies,$window) {

    $scope.message = 'Hello from presenter portal controller';
    $scope.content = '';
    $scope.init = function () {

        $scope.showIfAuthorized = false;
        var userdata = $cookies.getObject('user');
        console.log('Init called');
        if (userdata == null) {
            $scope.content = 'You are not an authorized user but this will not show';
        }
        else {
            $scope.content = 'You are an authorized user';
            $scope.showIfAuthorized = true;
        }
    }

    $scope.logOut = function () {
        $cookies.remove('user', { path: '/' });
        $window.location.href = "../../index.html";
    };
}]);
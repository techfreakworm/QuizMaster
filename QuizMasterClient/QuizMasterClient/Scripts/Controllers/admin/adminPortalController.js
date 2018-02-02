adminPortalApp.controller('adminPortalController', ['$scope', '$cookies', '$window', function ($scope, $cookies, $window) {

    $scope.message = 'Hello from admin portal controller';
    $scope.content = '';
    $scope.init = function () {

        $scope.showIfAuthorized = false;
        var currentUser = $cookies.getObject('currentUser');
        if (currentUser == null) {
            $scope.content = 'You are not an authorized user but this will not show';
        }
        else {
            $scope.content = 'You are an authorized user';
            $scope.showIfAuthorized = true;
        }
            
    }

    $scope.logOut = function () {
        $cookies.remove('currentUser', { path: '/' });
        $window.location.href = "../../index.html";
    };
}]);
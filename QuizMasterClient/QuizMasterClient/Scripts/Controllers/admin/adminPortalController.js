adminPortalApp.controller('adminPortalController', ['$scope', '$cookies', '$window', function ($scope, $cookies, $window) {

    $scope.message = 'Hello from admin portal controller';
    $scope.content = '';
    $scope.init = function () {

        $scope.showIfAuthorized = false;
        var userdata = $cookies.getObject('user');
        if (userdata == null) {
            $scope.content = 'You are not an authorized user but this will not show';
        }
        else {
            $scope.content = 'You are an authorized user';
            $scope.showIfAuthorized = true;
        }
            
        console.log(userdata);
    }

    $scope.logOut = function () {
        $cookies.user = { 'UserName': 'abc', 'UserPass': 'xyz' };
        $window.location.href = "../../index.html";
    };
}]);
adminPortalApp.controller('adminPortalController', ['$scope', '$cookies', function ($scope, $cookies) {

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
            
        console.log(userdata);
    }
}]);
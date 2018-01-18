presenterApp.controller('profileController', ['$scope', '$http', '$cookies', function ($scope, $http, $cookies) {

    var user = $cookies.getObject('user');
    $scope.userName = user.userName;
    $scope.userPass = user.userPass;

}]);
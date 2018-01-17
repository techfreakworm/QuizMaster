adminPortalApp.controller('homeController', ['$scope', '$http', '$cookies', function ($scope, $http, $cookies) {
    var user = $cookies.getObject('user');
    $scope.userName = user.userName;
}]);
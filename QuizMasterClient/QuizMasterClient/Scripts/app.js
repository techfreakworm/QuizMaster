var loginApp = angular.module('loginPortal', ['ngCookies']);
var presenterApp = angular.module('presenterPortal', ['ngCookies'],['ngRoute']);
var adminPortalApp = angular.module('adminPortal', ['ngCookies', 'ngRoute', 'ui.router']);

adminPortalApp.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {

    $stateProvider.state('home', {
        url: '/home',
        templateUrl: 'home.html',
        controller: 'homeController'
    });

    $stateProvider.state('questions', {
        url:'/questions',
        templateUrl: 'questions.html',
        controller: 'questionsController'
    });

    $stateProvider.state('users', {
        url: '/users',
        templateUrl: 'users.html',
        controller: 'usersController'
    });

    $urlRouterProvider.otherwise('/');
}]);

//loginApp.config(function ($routeProvider) {
//    $routeProvider

//        .when('/admin', {
//            templateUrl: 'Views/admin/adminPortal.html',
//            controller: 'adminPortalController'
//        })

//        .otherwise({ redirectTo: 'admin' });
//});


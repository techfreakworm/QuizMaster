var loginApp = angular.module('loginPortal', ['ngCookies']);
var presenterApp = angular.module('presenterPortal', ['ngCookies', 'ui.router']);
var adminPortalApp = angular.module('adminPortal', ['ngCookies', 'ngRoute', 'ui.router']);



//ADMIN PORTAL ROUTE CONFIGURATION
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

    $stateProvider.state('teams', {
        url: '/teams',
        templateUrl: 'teams.html',
        controller: 'teamsController'
    });

    $stateProvider.state('profile', {
        url: '/profile',
        templateUrl: 'profile.html',
        controller: 'profileController'
    });

    $urlRouterProvider.otherwise('/');
}]);

//PRESENTER PORTAL ROUTE CONFIGURATION
presenterApp.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {

    $stateProvider.state('teams', {
        url: '/teams',
        templateUrl: 'teams.html',
        controller: 'teamsController'
    });

    $stateProvider.state('home', {
        url: '/home',
        templateUrl: 'home.html',
        controller: 'homeController'
    });

    $stateProvider.state('questions',{
        url: '/questions',
        templateUrl: 'questions.html',
        controller: 'questionsController'
    });

    $urlRouterProvider.otherwise('/');
}]);


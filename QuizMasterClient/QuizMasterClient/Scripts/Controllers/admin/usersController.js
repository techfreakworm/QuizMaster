adminPortalApp.controller('usersController', ['$scope', '$http', '$cookies', function ($scope, $http, $cookies) {

    /*------------------GLOABLS FOR ALL-----------------------*/



    //ADMIN GLOBALS
    $scope.isAdminAddVisible = false;
    $scope.isAdminEditVisible = false;
    $scope.isAdminVisible = false;

    //PRESENTER GLOBALS
    $scope.isPresenterAddVisible = false;
    $scope.isPresenterEditVisible = false;
    $scope.isPresenterVisible = false;


    /*-------------------------------------------ADMIN--------------------------------------*/
 

    //ADMIN FUNCTIONS
    $scope.addAdminUser = function () { //Unhide add admin menu
        $scope.isAdminEditVisible = false;
        $scope.isAdminVisible = true;
        $scope.isAdminAddVisible = true;
    };

    $scope.editAdminUser = function (index) { //Unhide edit admin menu
        $scope.isAdminAddVisible = false;
        $scope.isAdminVisible = true;
        $scope.isAdminEditVisible = true;
        $scope.editAdmin = $scope.adminUsers[index];
    };

    $scope.deleteAdminUser = function (index) { //delete admin user
        var config = {
            data: $cookies.getObject('user'),
            headers: {
                'Content-Type': 'application/json'
            }
        };

        var uri = 'http://localhost:50827/api/user/' + $scope.adminUsers[index].UserId;
        $http.delete(uri, config).then(function (successResponse) {
            $scope.initAdmin();
        }, function (errorResponse) {

        });
    };

    $scope.initAdmin = function () { //initialize admin table
        var config = {
            headers: {
                'Content-Type': 'application/json'
            }
        };

        $http.post("http://localhost:50827/api/user/getadmin", $cookies.getObject('user'), config).then(function (successResponse) {
            $scope.adminUsers = successResponse.data;
            //console.log(adminDetails);
        }, function (errorResponse) {
            console.log("Cannot fetch all admin user details");
        });
    };

    $scope.saveAdminEdit = function () {//Edit an admin user
        var config = {
            headers: {
                'Content-Type': 'application/json'
            }
        };

        var data = {
            currentUser: $cookies.getObject('user'),
            user: {
                'UserId': $scope.editAdmin.UserId,
                'UserType': 'admin',
                'UserName': $scope.editAdmin.UserName,
                'UserPass': $scope.editAdmin.UserPass
            }
        };

        console.log(data);

        var uri = 'http://localhost:50827/api/user/' + $scope.editAdmin.UserId;

        $http.put(uri, data, config).then(function (successResponse) {
            $scope.isAdminVisible = false;
            $scope.isAdminEditVisible = false   ;

        }, function (errorResponse) {
            console.log(errorResponse);

            });


    };

    $scope.saveAdminAdd = function () {//Add an admin user
        var config = {
            headers: {
                'Content-Type': 'application/json'
            }
        };

        var data = {
            currentUser: $cookies.getObject('user'),
            user: {
                'UserName': $scope.addAdmin.UserName,
                'UserType' : 'admin',
                'UserPass': $scope.addAdmin.UserPass
            }
        };

        $http.post('http://localhost:50827/api/user', data, config).then(function (successResponse) {
            $scope.isAdminVisible = false;
            $scope.isAdminAddVisible = false;

        }, function (errorResponse) {
            console.log(errorResponse);

        });
    };



    /*--------------------------------------PRESENTER----------------------------------------*/

    //PRESENTER FUNCTIONS
    $scope.addPresenterUser = function () {//Unhide add presenter menu
        $scope.isPresenterEditVisible = false;
        $scope.isPresenterVisible = true;
        $scope.isPresenterAddVisible = true;
    };

    $scope.editPresenterUser = function (index) {  //Unhide edit presenter menu
        $scope.isPresenterAddVisible = false;
        $scope.isPresenterVisible = true;
        $scope.isPresenterEditVisible = true;
        $scope.editPresenter = $scope.presenterUsers[index];
    };

    $scope.deletePresenterUser = function (index) { //Delete a presenter
        var config = {
            data: $cookies.getObject('user'),
            headers: {
                'Content-Type': 'application/json'
            }
        };

        var uri = 'http://localhost:50827/api/user/' + $scope.presenterUsers[index].UserId;
        $http.delete(uri, config).then(function (successResponse) {
            $scope.initPresenter();
        }, function (errorResponse) {

            });
    };

    $scope.initPresenter = function () { //Initialize presenter table
        var config = {
            headers: {
                'Content-Type': 'application/json'
            }
        };

        $http.post("http://localhost:50827/api/user/getpresenter", $cookies.getObject('user'), config).then(function (successResponse) {
            $scope.presenterUsers = successResponse.data;
            console.log($scope.presenterUsers);
        }, function (errorResponse) {
            console.log("Cannot fetch all presenter user details");
        });
    };

    $scope.savePresenterEdit = function () { //Save presenter edits
        var config = {
            headers: {
                'Content-Type': 'application/json'
            }
        };

        var data = {
            currentUser: $cookies.getObject('user'),
            user: {
                'UserId': $scope.editPresenter.UserId,
                'UserType': 'admin',
                'UserName': $scope.editPresenter.UserName,
                'UserPass': $scope.editPresenter.UserPass
            }
        };

        console.log(data);

        var uri = 'http://localhost:50827/api/user/' + $scope.editPresenter.UserId;

        $http.put(uri, data, config).then(function (successResponse) {
            $scope.isPresenterVisible = false;
            $scope.isPresenterEditVisible = false;

        }, function (errorResponse) {
            console.log(errorResponse);

        });
    };

    $scope.savePresenterAdd = function () { //Add a presenter user
        var config = {
            headers: {
                'Content-Type': 'application/json'
            }
        };

        var data = {
            currentUser: $cookies.getObject('user'),
            user: {
                'UserName': $scope.addPresenter.UserName,
                'UserType': 'presenter',
                'UserPass': $scope.addPresenter.UserPass
            }
        };

        $http.post('http://localhost:50827/api/user', data, config).then(function (successResponse) {
            $scope.isPresenterVisible = false;
            $scope.isPresenterAddVisible = false;


        }, function (errorResponse) {
            console.log(errorResponse);

        });
    };

    
}]);
adminPortalApp.controller('usersController', ['$scope', '$http', '$cookies', function ($scope, $http, $cookies) {

    /*------------------GLOABLS FOR ALL-----------------------*/
    $scope.isEditVisible = false;
    $scope.isAddVisible = false;


    /*-------------------------------------------ADMIN--------------------------------------*/
    //ADMIN GLOBALS
    $scope.isAdminEditVisible = false;

    //ADMIN FUNCTIONS
    $scope.addAdminUser = function () { //Unhide add admin menu
        
    };

    $scope.editAdminUser = function (index) { //Unhide edit presenter menu
        $scope.isEditVisible = true;
        $scope.isAdminEditVisible = true;
    };

    $scope.deleteAdminUser = function (index) { //delete admin user

    };

    $scope.initAdmin = function () { //initialize admin table

        var config = {
            headers: {
                'Content-Type':'application/json'
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

    };

    $scope.saveAdminAdd = function () {//Add an admin user

    };



    /*--------------------------------------PRESENTER----------------------------------------*/
    //PRESENTER GLOBALS
    $scope.isPresenterEditVisible = false;

    //PRESENTER FUNCTIONS
    $scope.addPresenterUser = function () {//Unhide add presenter menu

    };

    $scope.editPresenterUser = function (index) {  //Unhide edit presenter menu

    };

    $scope.deletePresenterUser = function (index) { //Delete a presenter

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

    };

    $scope.savePresenterAdd = function () { //Add a presenter user

    };

    
}]);
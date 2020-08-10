var CrawlPanelApp = angular.module('Crawl', ['ui.bootstrap']);

CrawlPanelApp.controller('SearchProductController',
    function SearchProductController($scope, $http, $interval) {
        /*def*/
        $scope.dataBase = "MSSQL";
        $scope.jobList = [];
        $scope.host = location.origin;
        $scope.getFeeBeeDataApiUrl = location.origin + "/SearchProduct/GetFeeBeeData";
        $scope.sendFeeBeeDataToJobApiUrl = location.origin + "/SearchProduct/sendFeeBeeDataToJob";
        $scope.sendAllFeeBeeDataToJobApiUrl = location.origin + "/SearchProduct/sendAllFeeBeeDataToJob";
        //$scope.showTable = 'default';
        $scope.reqModel = { "count": 50 };
        $scope.getdata = function () {
            var datas = { "product": $scope.reqProduct };
            var req = {
                method: 'Post',
                url: $scope.getFeeBeeDataApiUrl,
                data: datas,
                headers: { 'Content-Type': 'application/json' },
            }
            $http(req).then(function (response) {
                $scope.dataArray = response.data;
            });
        },
            $scope.SendToJobList = function (x) {
                var req = {
                    method: 'Post',
                    url: $scope.sendFeeBeeDataToJobApiUrl,
                    data: x,
                    headers: { 'Content-Type': 'application/json' },
                }
                $http(req).then(function (response) {
                    alert(response.data.returnMsg);
                });
            },
            $scope.SendAllToJobList = function () {
                var req = {
                    method: 'Post',
                    url: $scope.sendAllFeeBeeDataToJobApiUrl,
                    data: $scope.dataArray,
                    headers: { 'Content-Type': 'application/json' },
                }
                $http(req).then(function (response) {
                    alert(response.data.returnMsg);
                });
            }
        
        //$scope.getCrawlPanel = function () {
        //    var req = {
        //        method: 'Get',
        //        url: $scope.crawlPanelApiUrl,
        //        headers: { 'Content-Type': 'application/json' },
        //    }
        //    $http(req).then(function (response) {
        //        $scope.dataArray = response.data;
        //    });
        //}
        //$scope.getStatusCount();
        //$interval(function () {
        //    $scope.getStatusCount();

        //}, 100000);

        //connection.on("ReceiveJobInfo", (seq, jobType, url, startTime) => {  
        //    const index = $scope.jobList.findIndex(x => x.seq === seq);
        //    if (index > -1) {
        //        $scope.jobList.splice(index, 1);
        //    } else {
        //        $scope.jobList.push({ seq: seq, jobType: jobType, url: url, startTime: startTime });
        //    }   
        //    $scope.$apply();
        //});        

        //$interval(function () {
        //    $scope.getCrawlPanel();
        //}, 300000);
        //  $interval($scope.getStatusCount,1000);
    }
);

//connection.onreconnecting(error => {
//    console.assert(connection.state === signalR.HubConnectionState.Reconnecting);
//    console.log(`Connection lost due to error "${error}". Reconnecting.`);
//});
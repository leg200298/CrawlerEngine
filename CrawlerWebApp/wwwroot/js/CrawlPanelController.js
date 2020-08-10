var CrawlPanelApp = angular.module('Crawl', ['ui.bootstrap']);

var connection = new signalR.HubConnectionBuilder()
    .withUrl("https://crawlpanel.azurewebsites.net/jobhub")
    .withAutomaticReconnect()
    .build();

connection.start().catch(err => console.log(err));

CrawlPanelApp.controller('CrawlPanelController',
    function CrawlPanelController($scope, $http, $interval) {
        /*def*/
        $scope.dataBase = "MSSQL";
        $scope.dataBaseList = ["MSSQL", "POSTGRESSQL"];
        $scope.jobList = [];
        $scope.host = location.origin;
        $scope.crawlPanelApiUrl = location.origin + "/Home/CrawlPanel";
        $scope.crawlPanelGetDiffStatusApiUrl = location.origin + "/Home/CrawlPanelGetDiffStatus";
        //$scope.showTable = 'default';
        $scope.reqModel = { "count": 50 };
        $scope.getStatusCount = function () {
            var req = {
                method: 'Post',
                url: $scope.crawlPanelGetDiffStatusApiUrl,
                data: { "database": $scope.dataBase },
                headers: { 'Content-Type': 'application/json' },
            }
            $http(req).then(function (response) {
                $scope.statusArray = response.data;
                $scope.updateTime = Date().toLowerCase();
            });
        }
        $scope.getdata = function () {
            var datas = { "count": $scope.reqModel.count };
            if ($scope.reqModel.job_status) {
                datas = {
                    "database": $scope.dataBase,
                    "command": " job_status = '" + $scope.reqModel.job_status + "' ",
                    "count": $scope.reqModel.count
                };
            }
            var req = {
                method: 'Post',
                url: $scope.crawlPanelApiUrl,
                data: datas,
                headers: { 'Content-Type': 'application/json' },
            }
            $http(req).then(function (response) {
                $scope.dataArray = response.data;
            });
        }

        $scope.getCrawlPanel = function () {
            var req = {
                method: 'Get',
                url: $scope.crawlPanelApiUrl,
                headers: { 'Content-Type': 'application/json' },
            }
            $http(req).then(function (response) {
                $scope.dataArray = response.data;
            });
        }
        $scope.getStatusCount();
        $interval(function () {
            $scope.getStatusCount();

        }, 100000);

        connection.on("ReceiveAddJobInfo", (seq, jobType, url, startTime) => {
            $scope.jobList.push({ seq: seq, jobType: jobType, url: url, startTime: startTime });
            $scope.$apply();
        });

        connection.on("ReceiveRemoveJobInfo", (seq) => {
            const index = $scope.jobList.findIndex(x => x.seq === seq);
            if (index > -1) {
                $scope.jobList.splice(index, 1);
            }
            $scope.$apply();
        });

        //$interval(function () {
        //    $scope.getCrawlPanel();
        //}, 300000);
        //  $interval($scope.getStatusCount,1000);
    }
);

connection.onreconnecting(error => {
    console.assert(connection.state === signalR.HubConnectionState.Reconnecting);
    console.log(`Connection lost due to error "${error}". Reconnecting.`);
});
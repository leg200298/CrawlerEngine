var CrawlPanelApp = angular.module('Crawl', ['ui.bootstrap']);

CrawlPanelApp.controller('CrawlPanelController',
    function CrawlPanelController($scope, $http, $interval) {
        /*def*/
        $scope.dataBase = "MSSQL";
        $scope.dataBaseList = ["MSSQL", "POSTGRESSQL"];
        $scope.host = location.origin;
        $scope.crawlPanelApiUrl = location.origin + "/api/v1/CrawlPanel";
        $scope.crawlPanelGetDiffStatusApiUrl = location.origin + "/api/v1/CrawlPanelGetDiffStatus";
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

        $interval(function () {
            $scope.getCrawlPanel();
        }, 300000);
        //  $interval($scope.getStatusCount,1000);
    }
);

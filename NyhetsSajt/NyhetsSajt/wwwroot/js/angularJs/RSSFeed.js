var RSSFeedApp = angular.module('RSSFeedApp', ['ngSanitize']);


RSSFeedApp.controller("RSSFeedCtrl", function ($scope, $http, $rootScope, $interval, $window) {

    $scope.GetRSSFeeds = function () {

        var feedItems = [];

        $scope.feedUpdated = new Date();

        $http.get('/RSSFeeds/GetRSSFeedItems')
            .then(function (response) {

                response.data.forEach(function (feedItem) {

                    feedItems.push(feedItem);

                });

                $scope.feedItems = feedItems;


            }, function (response) {
                $scope.responseData.statusText = response.statusText;
                $scope.responseData.status = response.status;

                $rootScope.noError = false;
            });
    };

    $scope.GetRSSFeeds();
    $interval($scope.GetRSSFeeds, 50000);


    $scope.selectedFeed = '';

    $scope.setFeed = function (feed) {
        $scope.selectedFeed = feed;
    }

    $scope.clearFeed = function () {
        $scope.selectedFeed = '';
    }


    $scope.selectedCategory = '';
    $scope.setCategory = function (category) {
        $scope.selectedCategory = category;
    }


    $scope.clearCategory = function () {
        $scope.selectedCategory = '';
    }


    $scope.redirectToUrl = function (url) {
        $window.open(url, '_blank');
    };


});

RSSFeedApp.filter('isCategory', function () {
    return function (values, category) {
        if (!category) {

            return values;
        }

        return values.filter(function (value) {

            if (value.category == null)
                return null;

            return value.category.toUpperCase() === category.toUpperCase();
        })
    }
});



RSSFeedApp.filter('isFeed', function () {
    return function (values, title) {
        if (!title) {

            return values;
        }

        return values.filter(function (value) {

            if (value.rssFeed.title == null)
                return null;

            return value.rssFeed.title === title;
        })
    }
});

RSSFeedApp.filter('uniqueCategory', function () {

    return function (collection, keyname) {

        var unique = [],
            keys = [];


        angular.forEach(collection, function (item) {

            var key = item[keyname];

            if (keys.indexOf(key) === -1) {
                keys.push(key);
                unique.push(item);
            }
        });

        return unique;
    };
});

RSSFeedApp.filter('uniqueFeed', function () {

    return function (collection, keyname) {

        var unique = [],
            keys = [];


        angular.forEach(collection, function (item) {

            var key = item.rssFeed[keyname];

            //alert(item.rssFeed.title);

            if (keys.indexOf(key) === -1) {
                keys.push(key);
                unique.push(item);
            }
        });

        var x = 10;

        return unique;
    };
});
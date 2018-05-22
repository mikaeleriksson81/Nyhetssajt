var RSSFeedApp = angular.module('RSSFeedApp', ['ngSanitize']);


RSSFeedApp.controller("RSSFeedCtrl", function ($scope, $http, $interval, $window, $filter) {

    $scope.showingFeeds = false;

    $scope.GetRSSFeeds = function () {        

        if ($scope.feedItems === undefined)
        {
            $scope.feedItems = [];
        }

        console.log("Fetching feeds..");

        $scope.feedUpdated = new Date();

        $http.get('/RSSFeeds/GetRSSFeedItems? rnd =' + new Date().getTime())
            .then(function (response) {

                response.data.forEach(function (feedItem) {                                        

                    this.duplicate = IsDuplicate($scope.feedItems, feedItem.link);

                    if (this.duplicate === false) {
                        $scope.feedItems.push(feedItem);
                    }

                    $scope.error = '';

                });
            })
            .catch(function (response) {
                $scope.error = "Error connecting to server";
            });
            
    };

    $scope.selectedFeed = '';

    $scope.GetRSSFeeds();
    $interval($scope.GetRSSFeeds, 60000);


    

    $scope.setFeed = function (feed) {
        $scope.selectedFeed = feed;
    };

    $scope.clearFeed = function () {
        $scope.selectedFeed = '';
    };


    $scope.selectedCategory = '';
    $scope.setCategory = function (category) {
        $scope.selectedCategory = category;
    };


    $scope.clearCategory = function () {
        $scope.selectedCategory = '';
    };


    $scope.redirectToUrl = function (url) {
        $window.open(url, '_blank');
    };


    //Would rather use LINQ but it caused issues in IE11
    var IsDuplicate = function (feedArray, link) {

        this.duplicate = false;

        feedArray.forEach(function (obj) {

            if (obj.link === link)
                this.duplicate = true;
        });

        if (this.duplicate === false)
            return false;       
        else
            return true;
    }


});

RSSFeedApp.filter('isCategory', function () {
    return function (values, category) {
        if (!category) {

            return values;
        }

        return values.filter(function (value) {

            if (value.category === null)
                return null;

            return value.category.toUpperCase() === category.toUpperCase();
        });
    };
});



RSSFeedApp.filter('isFeed', function () {
    return function (values, title) {
        if (!title) {

            return values;
        }

        return values.filter(function (value) {

            if (value.rssFeed.title === null)
                return null;

            return value.rssFeed.title === title;
        });
    };
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


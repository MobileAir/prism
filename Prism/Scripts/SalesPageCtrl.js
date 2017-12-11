angular.module("salesCart", [])
        .controller('salesCartCtrl', SalesCartCtrl);

function SalesCartCtrl($scope) {
    var index = 0;
    $scope.cart = [];
    $scope.amountPaid = 0;
    $scope.posCode = "";
    $scope.isPOStransaction = false;

    $scope.addItemtoCart = function (product) {
        var cartIndex = $scope.cart.indexOf(product);
        if (cartIndex === -1) {
            product.qty = 1;
            $scope.cart.unshift(product);
            PlaySound('success');
        } else {
            MoveToFirstPosition($scope.cart, cartIndex);
            ++$scope.cart[0].qty;
            PlaySound('success');
        }

    };
 
    $scope.removeItem = function (selectedIndex) {
        $scope.cart.splice(selectedIndex, 1);
    };

    //use this for the getChange function since it cant use the formatted number with a comma
    $scope.getSumOfProductsInNum = function () {
        var sum = 0;
        for (var i in $scope.cart) {
            sum = sum + ($scope.cart[i].qty * $scope.cart[i].UnitPrice);
        }
        return sum;
    };

    $scope.getSumOfProducts = function () {
        var sum = 0;
        for (var i in $scope.cart) {
            sum = sum + ($scope.cart[i].qty * $scope.cart[i].UnitPrice);
        }
        return Number(sum).toLocaleString('en');
    };

    $scope.getChange = function() {
        var sum = $scope.getSumOfProductsInNum();
        var change = $scope.amountPaid - sum;
        return change;
    };

    $scope.getChangeClass = function() {
        var color = "";
        if ($scope.getChange() >= 0) {
            color = "positiveChange";
        } else {
            color = "negativeChange";
        }
        return color;
    };

    //$scope.getposCodeState = function () {
    //    if ($scope.isPOStransaction)
    //};

    $scope.getPayBtnState = function () {
        if ($scope.getSumOfItems() > 0 && $scope.getChange() >= 0) {
            return false;
        } else {
            return true;
        }
    };
   
    $scope.getSumOfItems = function () {
        var sum = 0;
        for (var i in $scope.cart) {
            sum = sum + ($scope.cart[i].qty);
        }
        return sum;
    };

    $scope.setAmountPaid = function (amt) {
        $scope.amountPaid = amt;
    };

    $scope.numToWords = function (number) {
        if (number != 0) {
            return (toWords(number) + 'Naira Only');
        }
        
    };
}
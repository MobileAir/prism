
var products;
var isPOS = false;
$(document).ready(function () {
    $('#amountPaid').val('0');
    $('#amountPaid').val('');
    $.ajax({
        url: '/SupplyCart/GetSupplyProductList',
        context: document.body
    }).done(function (serverdata) {
        products = serverdata;

        populateSelect(products);

    });

    

    $("#upcClear").click(function () {
        $("#upc").val('');
        $("#upc").focus();
    });



    $("#payBtn").click(function () {
        var recieptNo = $("#recieptNumber").val();
        var sumOfProducts = $("#sumOfProducts").text();
        var change = $("#change span").text();
        var customerId = $("#SupplyCustomerID").val();

        var table = $('#productsTable').tableToJSON({
            onlyColumns: [1, 4]
        });
        $('#productListJson').val(JSON.stringify(table));

        //alert(customerId);

        $.post("/SupplyCart/RecordSupply/",
            {
                productListJson: JSON.stringify(table),
                recieptNumber: recieptNo,
                customerId: customerId
            },
            function (data) {
                bootbox.dialog({
                    message: "<div class='modal-body row'>" 
                        +"<div class='col-md-6'>" 
                        + "<img class='check-mark' src='/images/big_supply.png'>"
                        +"</div>" 
                        +"<div class='col-md-6'>" 
                        +"<h1>" + data + " items supplied.</h1>"
                        + "<p></p>"
                        + "<h3>Total: " + sumOfProducts + "</h3>"
                        + "<p></p>"
                        + "</div></div>"
                    ,
                    title: "Supply Summary",
                    onEscape: function() {
                        location.reload();
                    },
                    closeButton: false,
                    buttons: {
                        print: {
                            label: "Print Invoice",
                            className: "btn-primary",
                            callback: function () {
                                window.print();
                                location.reload();
                            }
                        },
                        main: {
                            label: "OK",
                            className: "btn-success",
                            callback: function () {
                                location.reload();
                            }
                        }
                    }
                });
            });
    });



});

function populateSelect(productList) {
    /*Populate the productName select element with the products array*/

    var sel = document.getElementById('productName');
    var fragment = document.createDocumentFragment();

    var optt = document.createElement('option');

    optt.innerHTML = "Product Name";
    optt.value = "";
    fragment.appendChild(optt);

    productList.forEach(function (product, index) {
        var opt = document.createElement('option');
        opt.innerHTML = product.Name;
        opt.value = product.UPC;
        fragment.appendChild(opt);
    });

    sel.appendChild(fragment);

    $('#productName').selectize({
        create: true,
        sortField: {
            field: 'text',
            direction: 'asc'
        },
        dropdownParent: 'body'
    });

    $('#SupplyCustomerID').selectize({
        create: true,
        sortField: {
            field: 'text'
        },
        dropdownParent: 'body'
    });

    $('#SupplyCustomerID').on("change", function (event) {
        getPreviousDebit();
    })
}

function getPreviousDebit() {
    var customerId = $("#SupplyCustomerID").val();

    $.get('/SupplyCart/GetPreviousDebit', {customerId: customerId}, function (data) {        
        //$('#debit').html(data);
        angular.element(document.getElementById('mainApp')).scope().setPreviousDebit(data);
        angular.element(document.getElementById('mainApp')).scope().$apply();
    });

}

function EnterUPC(upc) {
    $('#upc').val(upc);
    FindProduct();
}

function IsDoneSelecting() {
    if ($('#doneSelecting').prop("checked")) {
        isPOS = true;
        var totalAmt = parseInt(($('#sumOfProducts').text()).replace(',', ''));

        angular.element(document.getElementById('mainApp')).scope().setAmountPaid(totalAmt);
        angular.element(document.getElementById('mainApp')).scope().$apply();

        $('#amountPaid').val(totalAmt);
    } else {
        isPOS = false;
        angular.element(document.getElementById('mainApp')).scope().setAmountPaid(0);
        angular.element(document.getElementById('mainApp')).scope().$apply();
    }
}

function FindProduct() {
    var productUPC = document.getElementById('upc').value;
    var snapshot = Defiant.getSnapshot(products);
    search = JSON.search(snapshot, '//*[UPC = ' + productUPC + ']');

    if (search.length > 0) {
        angular.element(document.getElementById('mainApp')).scope().addItemtoCart(search[0]);
        angular.element(document.getElementById('mainApp')).scope().$apply();

        document.getElementById('upc').value = '';
        
    }

}

function FindProductByName() {
    var productUPC = $('#productName').val();
    var snapshot = Defiant.getSnapshot(products);
    search = JSON.search(snapshot, '//*[UPC = ' + productUPC + ']');

    if (search.length > 0) {
        angular.element(document.getElementById('mainApp')).scope().addItemtoCart(search[0]);
        angular.element(document.getElementById('mainApp')).scope().$apply();

        document.getElementById('upc').value = '';
        $('select option:selected').removeAttr('selected');
        //$("#productName").prop('selectedIndex', 0);
    }

}

function PlaySound(type) {
    var audio;
    if (type === 'success') {
        audio = new Audio('/mp3/success.mp3');
    } else {
        audio = new Audio('/mp3/error.mp3');
    }

    audio.play();

}

function MoveToFirstPosition(arr, objectIndex) {
    arr.unshift(arr[objectIndex]);
    arr.splice(objectIndex + 1, 1);

    return this;
};

function CalculateChange() {
    var amountPaid = ($("#amountPaid").val()) - ($("#sumOfProducts").text());
    $("#changeToCollect").html(amountPaid);
    if (amountPaid < 0) {
        $("#changeToCollect").css("color", "red");
    }
};

$('#createJson').click(function() {
    var table = $('#productsTable').tableToJSON({
        onlyColumns: [1, 4]
    });
    $('#productListJson').val(JSON.stringify(table));

    $.post("/SupplyCart/RecordSale/",
    {
        productListJson :JSON.stringify(table),
        customerName: "CustomerNamo"
    },
    function(data) {
        alert(data);
    }
    );

    //alert(JSON.stringify(table));
});


//function commaSeparateNumber(val) {
//    while (/(\d+)(\d{3})/.test(val.toString())) {
//        val = val.toString().replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
//    }
//    return val;
//}
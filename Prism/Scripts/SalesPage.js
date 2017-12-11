
var products;
var isPOS = false;
$(document).ready(function () {
    $('#amountPaid').val('0');
    $('#amountPaid').val('');
    $.ajax({
        url: '/Cart/GetProductList',
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
        var amountPaid = $("#amountPaid").val();
        var sumOfProducts = $("#sumOfProducts").text();
        var change = $("#change span").text();
        var customerId = $("#CustomerID").val();
        var posCode = $("#posCode").val();

        if (isPOS && posCode.length < 3) {
            alert('This is a POS Sale. Enter the POS Code');
            return;
        }

        var table = $('#productsTable').tableToJSON({
            onlyColumns: [1, 4]
        });
        $('#productListJson').val(JSON.stringify(table));        

        $.post("/Cart/RecordSale/",
            {
                productListJson: JSON.stringify(table),
                recieptNumber: recieptNo,
                customerId: customerId,
                amountPaid: amountPaid,
                balance: change,
                isPOS: isPOS,
                posCode: posCode
            },
            function (data) {
                bootbox.dialog({
                    message: "<div class='modal-body row'>" 
                        +"<div class='col-md-6'>" 
                        + "<img class='check-mark' src='/images/big_check.png'>"
                        +"</div>" 
                        +"<div class='col-md-6'>" 
                        +"<h1>" + data + " items sold.</h1>"
                        + "<p></p>"
                        + "<h3>Total: " + sumOfProducts + "</h3>"
                        + "<p></p>"
                        + "<h3>Paid: " + amountPaid + "</h3>"
                        + "<p></p>"
                        + "<h3>Change: " + change + "</h3>"
                        + "</div></div>"
                    ,
                    title: "Purchase Summary",
                    onEscape: function() {
                        location.reload();
                    },
                    closeButton: false,
                    buttons: {
                        print: {
                            label: "Print Reciept",
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

    $('#CustomerID').selectize({
        create: true,
        sortField: {
            field: 'text'
        },
        dropdownParent: 'body'
    });
}


function EnterUPC(upc) {
    $('#upc').val(upc);
    FindProduct();
}

function IsPOStransaction() {
    if ($('#isPOS').prop("checked")) {
        isPOS = true;
        $('#posCode').show(300);
        var totalAmt = parseInt(($('#sumOfProducts').text()).replace(',', ''));

        angular.element(document.getElementById('mainApp')).scope().setAmountPaid(totalAmt);
        angular.element(document.getElementById('mainApp')).scope().$apply();

        $('#amountPaid').val(totalAmt);
    } else {        
        isPOS = false;
        $('#posCode').val('');
        $('#posCode').hide(300);
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

function Sound(source, volume, loop) {
    this.source = source;
    this.volume = volume;
    this.loop = loop;
    var son;
    this.son = son;
    this.finish = false;
    this.stop = function () {
        document.body.removeChild(this.son);
    }
    this.start = function () {
        if (this.finish) return false;
        this.son = document.createElement("embed");
        this.son.setAttribute("src", this.source);
        this.son.setAttribute("hidden", "true");
        this.son.setAttribute("volume", this.volume);
        this.son.setAttribute("autostart", "true");
        this.son.setAttribute("loop", this.loop);
        document.body.appendChild(this.son);
    }
    this.remove = function () {
        document.body.removeChild(this.son);
        this.finish = true;
    }
    this.init = function (volume, loop) {
        this.finish = false;
        this.volume = volume;
        this.loop = loop;
    }
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

    $.post("/Cart/RecordSale/",
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
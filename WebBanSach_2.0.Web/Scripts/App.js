$(document).ready(function () {
    //Initially load pagenumber=1  
    //ajaxData(1, "", "");
    
});

(function () {
    createEvents();
    $("#back2Top").click(function (event) {
        event.preventDefault();
        $("html, body").animate({ scrollTop: 0 }, "slow");
        return false;
    });
    $('#blogCarousel').carousel({
        interval: 5000
    });
})();

/*Scroll to top when arrow up clicked BEGIN*/
$(window).scroll(function () {
    var height = $(window).scrollTop();
    if (height > 100) {
        $('#back2Top').fadeIn();
    } else {
        $('#back2Top').fadeOut();
    }
});
 /*Scroll to top when arrow up clicked END*/

function createEvents() {    
    $(".delete-btn").off('click').on('click', function () {
        $("#deleteId").val($(this).data('id'));
        $("#deleteModal").modal('show');
    });

    $(".add-to-cart").off('click').on('click', function () {
        var id = $(this).data("nameid");
        addToCart(id);
    });

    $(".cart-item-quantity").off('change').on('change', function () {
        var id = $(this).data("nameid");
        var quan = $(this).val();
        UpdateCart(id, quan);
    });

    $(".cart-item-quantity").off('keypress').on('keypress', function () {
        return isNumberKey(event);
    });

    $(".btn-order-detail").off('click').on('click', function () {
        var id = $(this).data("id");
        GetOrderDetail(id);
        $("#order-detail-modal").modal('show');
    });
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    return !(charCode > 31 && (charCode < 48 || charCode > 57));
}

function GetOrderDetail(id) {
    $.ajax({
        url: "/Manage/GetOrderDetail",
        type: "post",
        dataType: "json",
        data: { id: id },
        success: function (response) {
            if (response.status == true) {
                $("#order-detail-dataTable").empty();
                var data = response.data;
                var rowData = "";
                for (let i = 0; i < data.length; i++) {
                    rowData += `
                            <tr>
                                <td><img src="${data[i].Product.Image}" style="width:100px;height:auto" /> </td>
                                <td>${data[i].Product.Name}</td>
                                <td>${data[i].Product.Price.toLocaleString('vi', { style: 'currency', currency: 'VND' })}</td>
                                <td>${data[i].Quantity}</td>
                                <td class="text-right">${(data[i].Product.Price * data[i].Quantity).toLocaleString('vi', { style: 'currency', currency: 'VND' })}</td>
                            </tr>
                    `;
                }

                rowData += `<tr>
                                <td></td>
                                <td></td>
                                <td><strong>Tạm tính:</strong> ${response.total.toLocaleString('vi', { style: 'currency', currency: 'VND' })}</td>
                                <td><strong>Phí ship:</strong> 50,000.đ</td>
                                <td><strong>Tổng:</strong> ${(response.total + 50000).toLocaleString('vi', { style: 'currency', currency: 'VND' })}</td>
                            </tr>`;
                $("#order-detail-dataTable").append(rowData);
            }
        }
    });
}

function addToCart(id) {
    $.ajax({
        url: "/Cart/AddToCart",
        type: "post",
        data: { nameID: id },
        success: function (response) {
            if (response.status == true) {
                $.toast({
                    text: "Thêm sản phẩm vào giỏ hàng thành công !",
                    heading: 'Thông báo',
                    icon: 'success',
                    showHideTransition: 'fade',
                    allowToastClose: true,
                    hideAfter: 3000,
                    stack: 5,
                    position: 'top-right',
                    textAlign: 'left',
                    loader: false,
                    loaderBg: '#9EC600',
                });
                $("#cart-quantity").html(response.cartLength);               
            }
        }
    });
}

function UpdateCart(nameid, quantity) {
    $("#sub-total").empty();
    //$("#item-total").empty();
    $("#grand-total").empty();
    $.ajax({
        url: "/Cart/UpdateCart",
        type: "post",
        data: { nameID: nameid, quantity: quantity },
        success: function (response) {
            if (response.status == true) {
                $("#sub-total").html(response.total.toLocaleString('vi', { style: 'currency', currency: 'VND' }));
                //$("#item-total").empty();
                $("#grand-total").html((response.total + 50000).toLocaleString('vi', { style: 'currency', currency: 'VND' }));
            }
        }
    });
}

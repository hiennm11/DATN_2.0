$(document).ready(function () {
    //Initially load pagenumber=1  
    
    $(".btn-order-completed").off('click').on('click', function () {
        var id = $(this).data("id");
        var td = $(this).closest("tr").find(".item-status");    
        UpdateStatus(id, 2)
        td.html("<span class='text-success'>Đã hoàn thành</span>");     
    });
    $(".btn-order-canceled").off('click').on('click', function () {
        var id = $(this).data("id");
        var td = $(this).closest("tr").find(".item-status");
        UpdateStatus(id, 0)
        td.html("<span class='text-danger'>Đã hủy</span>");
        
    });
});

function UpdateStatus(id, status) {
    $.ajax({
        url: "/ManageOrder/UpdateStatus",
        type: "post",
        dataType: "json",
        data: { id: id, orderStatus: status },
        success: function (response) {
            if (response.status == true) {
                return true;
            }
        }
    });
}

function GetDetail(id) {
    $.ajax({
        url: "/ManageOrder/UpdateStatus",
        type: "get",
        data: { id: id, orderStatus: status },
        success: function (response) {
                $(".detail").replaceWith(response)
        }
    });
}

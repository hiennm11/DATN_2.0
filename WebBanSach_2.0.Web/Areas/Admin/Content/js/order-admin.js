$(document).ready(function () {
    //Initially load pagenumber=1  
    $(".btn-order-detail").off('click').on('click', function () {
        var id = $(this).data("id");
        GetOrderDetail(id);
        $("#order-detail-modal").modal('show');
    });
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
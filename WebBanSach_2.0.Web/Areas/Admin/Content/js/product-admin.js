$(document).ready(function () {
    //Initially load pagenumber=1
    ajaxData(1,"");
    createEvent();
});
function createEvent() {
    $("#createForm").validate();

    $("#searchDetail").off('change').on('change', function () {
        $("#Category").val(0);
        ajaxData(1, $("#searchDetail").val());
    }); 

    $("#Category").off('change').on('change', function () {
        $("#searchDetail").val("");
        ajaxData(1, "", $(this).val());
    }); 

    $("#btnCreate").off('click').on('click', function () {
        $("#createModal").modal('show');
        resetForm();
    });

    $("#content-wrapper").off('click', '.btn-edit').on('click', '.btn-edit', function () {
        $("#createModal").modal('show');
        resetForm();
        LoadDetail($(this).data("id"), $(this).data("page"));
    });

    $("#content-wrapper").off('click', '.open-delete-modal').on('click', '.open-delete-modal', function () {
        $("#deleteModal").modal('show');
        $("#modelID").val($(this).data("id"));
        $("#modelName").html($(this).data("name"));
    });

    $("#deleteDetail").off('click').on('click', function () {
        DeleteData($("#modelID").val());
    });

    $("#file").change(function () {
        readURL(this);
    });
}

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#productImg').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

function resetForm() {
    $("#ID").val(0);
    $("#Name").val("");
    $("#CateID").val(0);
    $("#productImg").attr("src", "")
    $("#Description").val("");
    $("#Price").val(0);
    $("#Status").prop('checked', true)
}

function LoadDetail(id) {
    $.ajax({
        url: "/Admin/Product/GetDetail",
        type: "get",
        dataType: "json",
        data: { id: id },
        success: function (response) {
            if (response.status == true) {
                var data = response.data;
                $("#ID").val(data.ID);
                $("#Name").val(data.Name);
                $("#CateID").val(data.CateID);
                $("#productImg").attr("src", data.Image)
                $("#Description").val(data.Description);
                $("#Price").val(data.Price);
                $("#Status").prop('checked', data.Status)
            }
            else {
                alert(response.message);
            }
        },
        error: function (e) {
            console.log(e);
        }
    })
}

function SaveProduct(formData) {
    var cur = window.location.href;
    if ($("#createForm").valid()) {
        var ajaxConfig = {
            url: "/Admin/Product/SaveDetail",
            type: "post",
            data: new FormData(formData),
            success: function (response) {
                if (response.status == true) {
                    alert("Save success!");
                    $("#createModal").modal('hide');
                    window.location.assign(cur);
                }
                else alert(response.message);
            }
        }

        if ($(formData).attr("enctype") == "multipart/form-data") {
            ajaxConfig["contentType"] = false;
            ajaxConfig["processData"] = false;
        }

        $.ajax(ajaxConfig);
        return false;
    }
}

function DeleteData(data) {
    var token = $("[name='__RequestVerificationToken']").val();
    var cur = window.location.href;
    $.ajax({
        url: "/Admin/Product/DeleteConfirmed",
        type: "post",
        dataType: "json",
        data: { __RequestVerificationToken: token, id: data },
        success: function (response) {
            if (response.status == true) {
                alert("Delete success!");
                $("#deleteModal").modal('hide');
                window.location.assign(cur);
            }
            else alert(response.message);
        }
    })
}

function ajaxData(pageNum, sstring, cate) {
    $("#dataTableBody").empty();
    $("#paged").empty();
    $.ajax({
        url: "/Admin/Product/GetPaggedData",
        type: "get",
        dataType: "json",
        data: { page: pageNum, search: sstring, cate: cate },
        success: function (response) {
            var data = response.data;
            if (response.status == true) {
                var rowData = "";
                for (var i = 0; i < data.Items.length; i++) {                  
                    rowData = rowData +
                        `<tr>
                            <td>${data.Items[i].Name}</td>
                            <td>${data.Items[i].CateID}</td>
                            <td>${data.Items[i].Description}</td>
                            <td><img src="${data.Items[i].Image}" height="100" width="90" style="border: solid" /></td>
                            <td>${data.Items[i].Price}</td>
                            <td>${data.Items[i].Purchase}</td>`;
                          
                    if (data.Items[i].Status) {
                        rowData += `<td>
                            <div class="btn btn-success btn-sm">Actived</div>
                        </td>`;
                    }
                    else {
                        rowData += `<td>
                            <div class="btn btn-danger btn-sm">Blocked</div>
                        </td>`;
                    }

                    rowData += `<td>
                            <button class="btn btn-outline btn-edit" data-page="${data.Pager.CurrentPage}" data-id="${data.Items[i].ID}"><i class="fas fa-pencil-alt" style="color: blue"></i></button>       
                            <button class="btn btn-outline-light open-delete-modal" data-id="${data.Items[i].ID}" data-name="${data.Items[i].Name}">
                                <i class="fas fa-fw fa-trash-alt" style="color: red"></i>
                            </button>
                        </td>
                    </tr>`;

                }
                $("#dataTableBody").append(rowData);
                PaggingTemplate(data.Pager, sstring, cate);
            }

        }
    })
}

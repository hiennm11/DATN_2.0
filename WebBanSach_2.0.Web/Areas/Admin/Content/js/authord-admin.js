$(document).ready(function () {
    //Initially load pagenumber=1
    ajaxData(1,"");
    createEvent();    
});

function createEvent() {
    $("#createForm").validate();

    $("#searchDetail").off('change').on('change', function () {
        ajaxData(1, $("#searchDetail").val());
    }); 

    $("#btnCreate").off('click').on('click', function () {
        $("#createModal").modal('show');
        resetForm();
    });

    $("#content-wrapper").off('click', '.btn-edit').on('click', '.btn-edit', function () {
        $("#createModal").modal('show');
        resetForm();
        LoadDetail($(this).data("id"));
    });

    $("#content-wrapper").off('click', '.open-delete-modal').on('click', '.open-delete-modal', function () {
        $("#deleteModal").modal('show');
        $("#modelID").val($(this).data("id"));
        $("#modelName").html($(this).data("name"));
    });

    $("#deleteDetail").off('click').on('click', function () {
        DeleteData($("#modelID").val());
    });

    $("#saveData").off('click').on('click', function () {
        if ($("#createForm").valid()) {

            var model = {
                ID: $("#ID").val(),
                Name: $("#Name").val(),
                Description: $("#Description").val(),
                Status: $("#Status").prop('checked')
            }

            SaveData(model);
        }
    });
}

function resetForm() {
    $("#ID").val(0);
    $("#CName").val("");
    $("#Description").val("");
    $("#Status").prop('checked', true)
}

function LoadDetail(id) {
    $.ajax({
        url: "/Admin/AuthorDetail/GetDetail",
        type: "get",
        dataType: "json",
        data: { id: id },
        success: function (response) {
            if (response.status == true) {
                var data = response.data;
                $("#ID").val(response.data.ID);
                $("#Name").val(data.Name);
                $("#Description").val(data.Description);
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

function SaveData(data) {
    var token = $("[name='__RequestVerificationToken']").val();
    $.ajax({
        url: "/Admin/AuthorDetail/SaveDetail",
        type: "post",
        dataType: "json",
        data: { __RequestVerificationToken: token, postData: JSON.stringify(data) },
        success: function (response) {
            if (response.status == true) {
                alert("Save success!");
                $("#createModal").modal('hide');
                window.location.href = "/Admin/AuthorDetail/";
            }
            else alert(response.message);
        }
    })
}

function DeleteData(data) {
    var token = $("[name='__RequestVerificationToken']").val();

    $.ajax({
        url: "/Admin/AuthorDetail/DeleteConfirmed",
        type: "post",
        dataType: "json",
        data: { __RequestVerificationToken: token, id: data },
        success: function (response) {
            if (response.status == true) {
                alert("Delete success!");
                $("#deleteModal").modal('hide');
                window.location.href = "/Admin/AuthorDetail/";
            }
            else alert(response.message);
        }
    })
}

function ajaxData(pageNum, sstring) {
    $("#dataTableBody").empty();
    $("#paged").empty();
    $.ajax({
        url: "/Admin/AuthorDetail/GetPaggedData",
        type: "get",
        dataType: "json",
        data: { page: pageNum, search: sstring },
        success: function (response) {
            var data = response.data;
            if (response.status == true) {
                var rowData = "";
                for (var i = 0; i < data.Items.length; i++) {
                    var createDate = new Date(parseInt(data.Items[i].CreateDate.substr(6)));
                    var updateDate = new Date(parseInt(data.Items[i].UpdatedDate.substr(6)));
                    rowData = rowData +
                        `<tr>
                        <td>${data.Items[i].Name}</td>
                        <td>${createDate.format("dd/mm/yyyy HH:MM:ss")}</td>
                        <td>${updateDate.format("dd/mm/yyyy HH:MM:ss")}</td>
                        <td>${data.Items[i].CreateBy}</td> `;
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
                PaggingTemplate(data.Pager, sstring);
            }

        }
    })
}


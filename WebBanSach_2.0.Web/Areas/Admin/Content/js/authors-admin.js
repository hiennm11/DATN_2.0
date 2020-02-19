$(document).ready(function () {
    //Initially load pagenumber=1  
    //GetPageData(1);
    $("#authorIDnew").attr("value", $("#AuthorID").val());
    LoadAuthorListl(null, $("#productListID").val());

    $("#authorTags").change(function () {
        searchAuthorOnchange();
    });

    $("#content-wrapper").off('click', '.open-delete-modal').on('click', '.open-delete-modal', function () {
        $("#deleteModal").modal('show');
        $("#modelID").val($(this).data("id"));
        $("#modelName").html($(this).data("id"));
    });

    $("#deleteDetail").off('click').on('click', function () {
        DeleteData($("#modelName").html());
    });

    $("#btnCreate").off('click').on('click', function () {
        //resetForm();
        $("#createModal").modal('show');
    });

    $("#submitCreate").off('click').on('click', function () {
        if ($("#createForm").valid()) {
            var id = $("#productListID").val();
            var list = [];
            $('input[name="author"]:checked').each(function () {
                list.push($(this).val());
            })
            SaveData(id, list);
        }
    });

});

function SaveData(id, data) {
    var token = $("[name='__RequestVerificationToken']").val();
    $.ajax({
        url: "/Admin/ProductAuthor/Create",
        type: "post",
        dataType: "json",
        data: { __RequestVerificationToken: token, productID: id, author: data },
        success: function (response) {
            if (response.status == true) {
                alert("Save success!");
                $("#deleteModal").modal('hide');
                window.location.href = "/Admin/ProductAuthor/";
            }
            else alert(response.message);
        }
    })
}

function DeleteData(data) {
    var token = $("[name='__RequestVerificationToken']").val();

    $.ajax({
        url: "/Admin/ProductAuthor/Delete",
        type: "post",
        dataType: "json",
        data: { __RequestVerificationToken: token, id: data },
        success: function (response) {
            if (response.status == true) {
                alert("Delete success!");
                $("#deleteModal").modal('hide');
                window.location.href = "/Admin/ProductAuthor/";
            }
            else alert(response.message);
        }
    })
}

function searchAuthorOnchange() {
    var s = $("#authorTags").val();
    var id = $("#productListID").val();

    LoadAuthorListl(s, id);

}

function LoadAuthorListl(s, id) {
    $("#listAuthor").empty();
    $.getJSON("/Admin/productauthor/getAuthor", { tags: s, productID: id }, function (response) {
        var data = `<div class="d-flex flex-wrap align-items-baseline">`;
        for (var i = 0; i < response.length; i++) {
            data +=
                //`<div class="form-check">
                //        <label class="form-check-label">
                //            <input type="checkbox" class="form-check-input" name="author" value="${response[i].ID}">${response[i].Name}
                //        </label>
                //    </div>`;
                `<div class="card">
                    <div class="card-body text-center">
                        <label class="form-check-label">
                             <input type="checkbox" class="form-check-input" name="author" value="${response[i].ID}">${response[i].Name}
                        </label> 
                    </div>
                  </div>`;                       
        }
        data += `</div>`
        $("#listAuthor").html(data);
    });

}

function GetPageData(pageNum) {
    //After every trigger remove previous data and paging
    $("#dataTableBody").empty();
    $("#paged").empty();
    $.getJSON("/Admin/productauthor/GetPaggedData", { page: pageNum }, function (response) {
        var rowData = "";
        response.Items.forEach(function (item) {
            rowData += `<tr>
                            <td>${item.Key.Name}</td> </tr>`
        })



        //for (var i = 0; i < response.Items.length; i++) {          
        //    rowData = rowData +
        //        `<tr>
        //            <td>${response.Items[i].Key.Name}</td>
        //            <td>`for(var ) `</td >
        //            <td>
        //                <a class="btn btn-outline" href="/admin/author/detail?productid=${response.Items[i].ProductID}&authorid=${response.Items[i].AuthorID}"><i class="fas fa-fw fa-pencil-alt"></i></a>            
        //                <button class="btn btn-outline-light open-delete-modal" type="submit" data-toggle="modal" data-target="#deleteModal" data-id="${response.Items[i].ProductID}" data-name="${response.Items[i].ProductName}">
        //                    <i class="fas fa-fw fa-trash-alt" style="color: red"></i>
        //                </button>
        //            </td>
        //        </tr>`;

        //}
        $("#dataTableBody").append(rowData);
        PaggingTemplate(response.Pager);
    });
}

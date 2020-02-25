$(document).ready(function () {
    //Initially load pagenumber=1  
    //ajaxData(1, "", "");
    createEvents();

    $("#back2Top").click(function (event) {
        event.preventDefault();
        $("html, body").animate({ scrollTop: 0 }, "slow");
        return false;
    });
});

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
    //$("#search-text").off('change').on('change', function () {
    //    ajaxData(1, "", $("#search-text").val());
    //});

    //$(".cate-item").off('click').on('click', function () {
    //    $("#search-text").val("");
    //    var id = $(this).data('id');
    //    ajaxData(1, id, "");
    //});

    //$(".cate-link").off('click').on('click', function () {

    //    var id = $(this).data('id');
    //    ajaxData(1, id, "");
    //});

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
        addToCart(id);
    });
}

function addToCart(id) {
    var quantity = $("#cart-quantity").html();    
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
                console.log(response.cart);
                $("#cart-quantity").empty();
                $("#cart-quantity").html(response.cart.length);
            }
        }
    });
}

function ajaxData(pageNum, cate, sstring) {
    $("#data-box").empty();
    $("#paged").empty();
    $.ajax({
        url: "/Home/GetPaggedData",
        type: "get",
        dataType: "json",
        data: { page: pageNum, cateID: cate, search: sstring },
        success: function (response) {
            var data = response.data;
            if (response.status == true) {
                var rowData = "";
                for (var i = 0; i < data.Items.length; i++) {
                    
                    rowData += `
                        <div class="col-lg-2 col-md-3 col-sm-4 col-xs-6">
                            <div class="card item-card">
                                <a href="/chi-tiet/${data.Items[i].NameID}"><img src="${data.Items[i].Image}" alt="${data.Items[i].Name}" style="width:100%; height:245px"></a><br />`;
                    if (data.Items[i].Name.length > 25) {
                        rowData += `<a href="/chi-tiet/${data.Items[i].NameID}">
                            <h6>
                                ${data.Items[i].Name.substring(0, 25)} ...
                                        </h6>
                        </a>`;
                    }

                    else {
                        rowData += `<a href="/chi-tiet/${data.Items[i].NameID}">
                        <h6>${data.Items[i].Name}</h6>
                    </a>`;
                    }

                    rowData +=`<p class="price">${data.Items[i].Price}</p>
                                <p><button>Add to Cart</button></p>
                            </div>
                        </div>`;

                }
                $("#data-box").append(rowData);
                PaggingTemplate(data.Pager, cate, sstring);
            }

        }
    })
}


//Paging temlpate
function PaggingTemplate(pager,cate ,search) {
    var template = "";

    var info = "<p>" + pager.CurrentPage + " of " + pager.TotalPages + " pages</p>"
    if (pager.CurrentPage > 1) {
        template += `<li class="page-item"><a class="page-link" href="#" onclick="ajaxData(1,'${cate}','${search}')">First</a></li>
                     <li class="page-item"><a class="page-link" href="#" onclick="ajaxData(${pager.CurrentPage - 1},'${cate}', '${search}')"> Previous</a ></li>`;
    }

    var numberingLoop = "";
    for (var i = pager.StartPage; i <= pager.EndPage; i++) {
        if (i == pager.CurrentPage) {
            numberingLoop = numberingLoop + `<li class="page-item active"><a class="page-link" onclick="ajaxData(${i},'${cate}', '${search}')" href="#">${i}</a></li>`;
        }
        else {
            numberingLoop = numberingLoop + `<li class="page-item"><a class="page-link" onclick="ajaxData(${i},'${cate}', '${search}')" href="#">${i}</a></li>`;
        }
    }
    template = template + numberingLoop
    if (pager.CurrentPage < pager.TotalPages) {
        template += `<li class="page-item"><a class="page-link" href="#" onclick="ajaxData(${pager.CurrentPage + 1},'${cate}', '${search}')"> Next</a ></li>
                     <li class="page-item"><a class="page-link" href="#" onclick="ajaxData(${pager.TotalPages},'${cate}', '${search}')"> Last</a ></li>`
    }

    $("#paged").append(template);
    $("#dataTable_info").html(info);
}

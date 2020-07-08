(function () {
    $(".btn-order-detail").prepend("<i class='fas fa-info-circle'></i>")
    $(".btn-order-detail").off('click').on('click', function (e) {
        e.preventDefault();
        $("#order-detail-modal").modal('show');
    });
})();
function exportToPDF(id, root) {
    $.ajax({
        url: 'ExportToPDF',
        data: { orderId: id },
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        success: function (result) {

            $('#frmPDF').attr('src', '/' + result);

            setTimeout(function () {
                frame = document.getElementById("frmPDF");
                framedoc = frame.contentWindow;
                framedoc.focus();
                framedoc.print();
            }, 1000);
        },
        error: function (xhr, status, err) {
            alert(err);
        }
    });
    return false;
}
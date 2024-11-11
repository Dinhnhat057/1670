var load = function (keyWord, pageIndex, pageSize) {
    $.ajax({
        url: "/Admin/Account/GetPaging",
        data: {
            keyWord: keyWord,
            pageIndex: pageIndex,
            pageSize: pageSize
        },
        type: "GET",
        success: function (response) {
            var pageCurrent = response.pageCurrent;
            var toalPage = response.toalPage;

            var str = "";
            var info = `Trang ${pageCurrent} / ${toalPage}`;
            $("#selection-datatable_info").text(info);
            $.each(response.data, function (index, value) {
                var txtClass = value.status === "Khóa" ? "btn btn-info" : "btn btn-warning";
                var txtText = value.status === "Khóa"  ? "Mở khóa" : "Khóa"
                str += "<tr>";
                str += "<td>" + value.id + "</td>";
                str += "<td>" + value.email + "</td>";
                str += "<td>" + value.role + "</td>";
                str += "<td>" + value.createdDate + "</td>";
                str += "<td>" + (value.status ) + "</td>";
                str += '<td class="d-flex justify-content-around">';
                str += '<a class="btn btn-danger" href="#" data-user=' + value.id + '>Xóa</a> <a class="' + txtClass + '" href="#" data-user=' + value.id + '>' + txtText + '</a>';
                str += '</td>'
                str += "</tr>";

                //create pagination
                var pagination_string = "";

                if (pageCurrent > 1) {
                    var pagePrevious = pageCurrent - 1;
                    pagination_string += '<li class="paginate_button page-item previous"><a href="#" class="page-link" data-page="' + pagePrevious + '">‹</a></li>';
                }
                for (var i = 1; i <= toalPage; i++) {
                    if (i == pageCurrent) {
                        pagination_string += '<li class="paginate_button page-item active"><a class="page-link" href="#" data-page=' + i + '>' + i + '</a></li>';
                    } else {
                        pagination_string += '<li class="paginate_button page-item"><a href="#" class="page-link" data-page=' + i + '>' + i + '</a></li>';
                    }
                }
                //create button next
                if (pageCurrent > 0 && pageCurrent < toalPage) {
                    var pageNext = pageCurrent + 1;
                    pagination_string += '<li class="paginate_button page-item next"><a href="#" class="page-link" data-page=' + pageNext + '>›</a></li>';
                }
                $("#load-pagination").html(pagination_string);
            });
            //load str to class="load-list"
            $("#datatablesSimple > tbody").html(str);
        }
    });
}

//click delete button
$("body").on("click", "#datatablesSimple a.btn.btn-danger", function (event) {
    event.preventDefault();
    var user_delete = $(this).attr('data-user');
    if (confirm("Bạn có muốn xóa tài khoản có id = " + user_delete + " này không?")) {
        $.ajax({
            url: "/Admin/Account/Delete",
            type: "POST",
            data: { id: user_delete },
            dataType: "json",
            success: (result) => {
                location.reload();
            }
        });
    }
});

$("body").on("click", "#datatablesSimple a.btn.btn-warning", function (event) {
    event.preventDefault();
    var user_delete = $(this).attr('data-user');
    if (confirm("Bạn có muốn khóa tài khoản có id = " + user_delete + " này không?")) {
        $.ajax({
            url: "/Admin/Account/LockAccount",
            type: "POST",
            data: { id: user_delete },
            dataType: "json",
            success: (result) => {
                location.reload();
            }
        });
    }
});

$("body").on("click", "#datatablesSimple a.btn.btn-info", function (event) {
    event.preventDefault();
    var user_delete = $(this).attr('data-user');
    if (confirm("Bạn có muốn mở khóa tài khoản có id = " + user_delete + " này không?")) {
        $.ajax({
            url: "/Admin/Account/UnLockAccount",
            type: "POST",
            data: { id: user_delete },
            dataType: "json",
            success: (result) => {
                location.reload();
            }
        });
    }
});
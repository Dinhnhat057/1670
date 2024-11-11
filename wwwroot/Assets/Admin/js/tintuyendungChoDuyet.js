var load = function (keyWord, pageIndex, pageSize) {
    $.ajax({
        url: "/Admin/Recruitment/GetPaging",
        data: {
            status: 0,
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
                str += "<tr>";
                str += "<td>" + value.id + "</td>";
                str += "<td>" + value.name + "</td>";
                str += "<td>" + value.tenNTD + "</td>";
                str += "<td>" + value.quatity + "</td>";
                str += "<td>" + value.createdDate + "</td>";
                str += "<td>" + value.dealine + "</td>";
                str += "<td><span class='badge badge-success'>" + value.status + "</span></td>";
                str += '<td class="display: inline-grid;"><a style="min-width: 90px" class="btn btn-warning" href="/Admin/Recruitment/Edit/' + value.id + '">Cập nhật</a>';
                str += '<a class="btn btn-success mt-1" href="#" data-user=' + value.id + '>Duyệt bài</a>';
                str += "</td></tr>";

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

//click button
$("body").on("click", "#datatablesSimple a.btn.btn-success", function (event) {
    event.preventDefault();
    var member_delete = $(this).attr('data-user');
    if (confirm("Bạn có muốn duyệt tin tuyển dụng có Mã = " + member_delete + " này không?")) {
        $.ajax({
            url: "/Admin/Recruitment/Approved",
            type: "POST",
            data: { id: member_delete },
            dataType: "json",
            success: (result) => {
                location.reload();
            }
        });
    }
});
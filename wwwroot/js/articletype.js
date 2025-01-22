var dataTable;

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url":"/ArticleType/GetAll"
        },
        "columns": [
            {"data":"id", width:"30%"},
            { "data": "type", width: "30%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <div class="text-center">
                    <a href="/ArticleType/Update/${data}" class="btn btn-info">Edit&nbsp<i class="fas fa-edit"></i></a>
                    <a class="btn btn-danger" onclick="Delete('/ArticleType/Delete/${data}')">Delete&nbsp<i class="fas fa-trash-alt"></i></a>
                    </div>
                    `;
                }
            }
        ]
    })
}
function Delete(url) {
    swal({
        title: "Want to Delete Data",
        text: "Delete Information",
        icon: "error",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        $.ajax({
            url: url,
            type: "Delete",
            success: function (data) {
                if (data.success) {
                    toastr.success(data.message);
                    dataTable.ajax.reload();

                }
                else {
                    toastr.error(data.message);
                }
            }
        })
    })
}
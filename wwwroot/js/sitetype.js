﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url":"/Admin/SiteType/GetAll"
        },
        "columns": [
            {"data":"typeName","width":"50%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <div class="text-center">
                    <a href="/Admin/SiteType/Update/${data}" class="btn btn-info">Edit&nbsp <i class="fas fa-edit"></i></a>
                    <a class="btn btn-danger" onclick="Delete('/Admin/SiteType/Delete/${data}')">Delete&nbsp <i class="fas fa-trash-alt"></i></a>
                    </div>`;
                }
            }
        ]
    })
}
function Delete(url) {
    swal({
        title: "Want to Delete Data?",
        text: "Delete Information",
        icon: "error",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    if (data != null && data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                },
                error: function () {
                    toastr.error("Something Went Wrong while Deleting Data")
                }
            });
        }
        else {
            toastr.info("Delete Operation was Cancelled")
        }
    });
}
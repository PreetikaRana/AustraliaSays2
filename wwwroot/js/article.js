var dataTable;

$(document).ready(function (){
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url":"/Admin/Articles/GetAll"
        },
        "columns": [
            { "data": "id", "width": "30%" },
            { "data": "name", "width": "30%" },
            {
                "data": "id", "render": function (data) {
                    return `
                    <div class="text-center">
                    <a href="/Admin/Articles/Update/${data}" class=" btn btn-info">Edit&nbsp;<i class="fas fa-edit"></i></a>
                    <a class="btn btn-danger" onclick="Delete('/Admin/Articles/Delete/${data}')">Delete&nbsp<i class="fas fa-trash-alt"></i></a>


                    </div>
                    `;
                }}
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
                    toastr.error("something went wrong while deleting Data.")
                }

            });
        } else {
            toastr.info("Delete operation was canceled.");
        }
    });
}




//function setDeleteId(id) {
//    document.getElementById('deleteId').value = id;
//}


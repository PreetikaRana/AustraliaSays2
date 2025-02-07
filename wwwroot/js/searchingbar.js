$(function () {
    $("#search-input").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/User/Home/Search",
                type: "Get",
                data: {
                    query: request.term
                },
                success: function (data) {
                    response(data);
                }
            });
        },

        select: function (event, ui) {
            window.location.href = ui.item.url;
        },

        minlength: 2
    });
});
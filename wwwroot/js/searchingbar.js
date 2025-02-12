$(function () {
    // Remove the form submission override
    $("#search-form").on("submit", function (e) {
        const query = $("#search-input").val().trim();

        if (query.length === 0) {
            e.preventDefault(); // Stop form submission if query is empty
        }
    });

    $("#search-input").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/User/Home/Search",
                type: "GET",
                data: { query: request.term },
                success: function (data) {
                    // Map results for autocomplete
                    response($.map(data, function (item) {
                        return {
                            label: `${item.Name} (${item.Type})`,
                            value: item.Name,
                            url: item.Url,
                        };
                    }));
                },
                error: function () {
                    response([]);
                },
            });
        },
        select: function (event, ui) {
            // Redirect when a suggestion is selected
            window.location.href = ui.item.url;
        },
        minlength: 2,
    });
});

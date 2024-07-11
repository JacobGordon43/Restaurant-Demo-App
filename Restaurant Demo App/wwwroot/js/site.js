$(function () {
    console.log("Document is ready");
    $(document).on("click", "#add-category", function () {
        console.log("Clicked");
        $("#category-form").modal("show");
    })

    $("#close-category-modal").click(function () {
        $("#category-form").modal("hide");
    })

    $("#create-category").click(function () {
        console.log("The category input is " + $("#category-input").val())
        var category = {"Category": $("#category-input").val(), "Items": null};

        $.ajax({
            type: 'POST',
            contentType: "application/json",
            data: JSON.stringify(category),
            url: "/Admin/CreateCategory",
        }).done(function () {
            console.log("Done");
            $("#category-form").modal('hide');
            location.reload();
        })
    })
})
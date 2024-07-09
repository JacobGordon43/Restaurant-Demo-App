// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    console.log("Document is ready");
    $(document).on("click", "#add-category", function () {
        console.log("Clicked");
        $("#category-form").modal("show");
    })
})
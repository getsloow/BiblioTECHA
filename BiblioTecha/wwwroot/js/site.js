// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    var divTopPosition = $('.my-div').offset().top; // Get the initial top position of the div

    $(window).scroll(function () {
        var scrollTopPosition = $(window).scrollTop(); // Get the current scroll position of the window

        if (scrollTopPosition >= divTopPosition) {
            $('.my-div').addClass('fixed');
        } else {
            $('.my-div').removeClass('fixed');
        }
    });
});

function updateText() {
    var input = document.getElementById("file");
    var textChange = document.getElementById("textChange");
    var uploadButton = document.getElementById("uploadButton");

    if (input.files.length > 0) {
        textChange.innerHTML = "Uploaded: " + input.files[0].name;
        uploadButton.removeAttribute("disabled");
    } else {
        textChange.innerHTML = "Drag and drop a file here or click to choose a file.";
        uploadButton.setAttribute("disabled", true);
    }
}
var dropArea = document.getElementById("drop-area");

dropArea.addEventListener("dragenter", function (event) {
    event.preventDefault();
    dropArea.style.backgroundColor = "#eee";
});

dropArea.addEventListener("dragover", function (event) {
    event.preventDefault();
    dropArea.style.backgroundColor = "#eee";
});

dropArea.addEventListener("dragleave", function (event) {
    event.preventDefault();
    dropArea.style.backgroundColor = "";
});

dropArea.addEventListener("drop", function (event) {
    event.preventDefault();
    dropArea.style.backgroundColor = "";

    var fileInput = document.getElementById("file");
    fileInput.files = event.dataTransfer.files;
});

dropArea.addEventListener("click", function () {
    var fileInput = document.getElementById("file");
    fileInput.click();
});
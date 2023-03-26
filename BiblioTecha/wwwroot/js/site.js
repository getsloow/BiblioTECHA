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

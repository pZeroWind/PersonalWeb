// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function GetPostData(body) {
    return {
        method: "POST",
        headers: {
            'RequestVerificationToken': document.querySelector('input[type="hidden"][name="__RequestVerificationToken"]').value,
            'content-type': 'application/json'
        },
        body: body
    }
}

let nav = false;

function ToggleNav() {
    let dom = document.querySelector(".top-nav");
    if (!nav) {
        dom.className = "top-nav top-nav-open";
        //document.querySelector(".top-nav").className = "top-nav top-nav-open"
    } else {
        dom.className = "top-nav";
    }
    nav = !nav
}
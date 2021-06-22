// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function HideFunction(id, buttonid) {
    let x = document.getElementById(id);
    let y = document.getElementById(buttonid);

    if (x.style.display === "none") {
        x.style.display = "block";
        //document.getElementById("message").innerHTML = "˂"
        y.innerHTML = "Dölj";
    } else {
        x.style.display = "none";
        //document.getElementById("message").innerHTML = "˅"
        y.innerHTML = "Visa fem sensate";


    }
}
$(document).ready(function () {

    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
    });

});
function setCookie(cname, cvalue) {
    document.cookie = cname + "=" + cvalue;
}
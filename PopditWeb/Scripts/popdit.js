function ToggleDiv(id) {
    // Show all the headers.
    var toHide = document.getElementsByClassName('header');
    for (var i = 0; i < toHide.length; i++) { toHide[i].style.display = "table-row"; }
    // Hide this header
    document.getElementById("header" + id).style.display = "none";
    // Hide all the details.
    var toHide = document.getElementsByClassName('detail');
    for (var i = 0; i < toHide.length; i++) { toHide[i].style.display = "none"; }
    // Show this detail.
    document.getElementById("detail" + id).style.display = "table-row";
}
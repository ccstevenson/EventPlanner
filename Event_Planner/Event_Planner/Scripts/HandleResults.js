function boxChecked(cb) {
    var column = document.getElementById("SecondColumn");
    column.innerHTML += cb.checked;
    // display("Changed, new value = " + cb.checked);
    // cb.DELETE;
}
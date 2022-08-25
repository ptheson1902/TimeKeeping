var tabs = document.getElementsByClassName("nav-item-icon");
var hump = document.getElementById("hump");
var humpLeftFromWindow = hump.getBoundingClientRect().left;
var active = tabs[2];
if (location.href.indexOf("Perforation") != -1) {
    active = tabs[1]
}
if (location.href.indexOf("WorkSchedule") != -1) {
    active = tabs[0]
}
function select(active) {
    active.classList.add("active");
    hump.style.left =
        active.getBoundingClientRect().left - humpLeftFromWindow;
}

select(active);

/*
if (window.history.replaceState) {
    window.history.replaceState(null, null, window.location.href);
}*/
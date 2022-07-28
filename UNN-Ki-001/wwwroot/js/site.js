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

function getdate() {
    var today = new Date();
    var h = today.getHours();
    var m = today.getMinutes();

    if (m < 10) {
        m = "0" + m;
    }
    $(".main .left .time").text(h + " : " + m);
    setTimeout(function () { getdate() }, 1);
}
getdate()


// Calendar
function day(i) {
    var str = "";
    let j = 0;
    while (j < 7) {
        str += "<td class='fc-day px-2' data-date='" + new Date().getFullYear() + ("0" + (new Date().getMonth() + 1)).slice(-2) + ("0" + i).slice(-2) + "'>" +
            "<div class=''>" +
            "<div class='fc-day-number py-2'>" + i + "</div>" +
            "</div>" +
            "</td>"
        i++;
        j++;
    }
    return str;
}
function day(i, b) {
    var str = "";
    let j = 0;
    while (j < 7) {
        if (j < b) {
            str += "<td class='fc-day px-2' data-date=''>" +
                "<div class=''>" +
                "<div class='fc-day-number py-2'></div>" +
                "</div>" +
                "</td>"
        } else {
            str += "<td class='fc-day px-2' data-date='" + new Date().getFullYear() + ("0" + (new Date().getMonth() + 1)).slice(-2) + ("0" + i).slice(-2) + "'>" +
                "<div class=''>" +
                "<div class='fc-day-number py-2'>" + i + "</div>" +
                "</div>" +
                "</td>"
            i++;
        }
        j++;
    }
    return str;
}
function day1(i, b) {
    var str = "";
    let j = 0;
    while (j < 7) {
        if (j < 7 - b) {
            str += "<td class='fc-day px-2' data-date='" + new Date().getFullYear() + ("0" + (new Date().getMonth() + 1)).slice(-2) + ("0" + i).slice(-2) + "'>" +
                "<div class=''>" +
                "<div class='fc-day-number py-2'>" + i + "</div>" +
                "</div>" +
                "</td>"
            i++;
        } else {
            str += "<td class='fc-day px-2' data-date=''>" +
                "<div class=''>" +
                "<div class='fc-day-number py-2'></div>" +
                "</div>" +
                "</td>"
        }
        j++;
    }
    return str;
}
var dayOfWeek = new Date(new Date().getFullYear() + "/" + (new Date().getMonth() + 1) + "/1").getDay();
var dayNumOfMonth = new Date(new Date().getFullYear(), new Date().getMonth() + 1, 0).getDate()
$(".calendar-data tr.week1").append(day(1, dayOfWeek))
$(".calendar-data tr.week2").append(day(8 - dayOfWeek))
$(".calendar-data tr.week3").append(day(15 - dayOfWeek))
$(".calendar-data tr.week4").append(day(22 - dayOfWeek))
$(".calendar-data tr.week5").append(day1(29 - dayOfWeek, 35 - dayNumOfMonth - dayOfWeek))
if (dayNumOfMonth + dayOfWeek > 35) { $(".calendar-data tr.week6").append(day1((36 - dayOfWeek), 7 - (37 - dayOfWeek - dayNumOfMonth))) }
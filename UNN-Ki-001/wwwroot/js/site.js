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
        str += "<td class='fc-day px-2' data-date='" + year + ("0" + month).slice(-2) + ("0" + i).slice(-2) + "'>" +
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
            str += "<td class='fc-day px-2' data-date='" + year + ("0" + month).slice(-2) + ("0" + i).slice(-2) + "'>" +
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
            str += "<td class='fc-day px-2' data-date='" + year + ("0" + month).slice(-2) + ("0" + i).slice(-2) + "'>" +
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
let year = new Date().getFullYear()
let month = new Date().getMonth() + 1
function Calendar(year, month, e) {
    $(".fc-header .fc-header-center .year").text(year);
    $(".fc-header .fc-header-center .month").text(month);
    var dayOfWeek = new Date(year + "/" + month + "/1").getDay();
    var dayNumOfMonth = new Date(year, month, 0).getDate()
    $(".calendar-data tr.week1").append(day(1, dayOfWeek));
    $(".calendar-data tr.week2").append(day(8 - dayOfWeek))
    $(".calendar-data tr.week3").append(day(15 - dayOfWeek))
    $(".calendar-data tr.week4").append(day(22 - dayOfWeek))
    $(".calendar-data tr.week5").append(day1(29 - dayOfWeek, 35 - dayNumOfMonth - dayOfWeek))
    if (dayNumOfMonth - 35 + dayOfWeek > 0) {
        $(".calendar-data tr.week6").append(day1((36 - dayOfWeek), 42 - dayNumOfMonth - dayOfWeek))
    }

    var q = 0;
    $(".fc-day").each(function () {
        var it = $(this);
        for (let j = 0; j < e.length; j++) {
            if (e[j].kinmuDt == it.data("date") && e[j].dakokuFrDt != null && e[j].dakokuFrTm != null && e[j].dakokuToDt != null && e[j].dakokuToTm != null) {
                it.children().addClass("taikin");
            }
            if (e[j].kinmuDt == it.data("date") && e[j].dakokuFrDt != null && e[j].dakokuFrTm != null && e[j].dakokuToDt == null && e[j].dakokuToTm == null) {
                it.children().addClass("shukin");   
            }
            if (e[j].kinmuDt == it.data("date") && e[j].dakokuFrDt == null && e[j].dakokuFrTm == null && e[j].dakokuToDt == null && e[j].dakokuToTm == null) {
                it.children().addClass("yasumi");
            }
        }
    })
    /*e.forEach(item => {
        var str = "";
        if (q < 7 - dayOfWeek) {
            str += "<td class='fc-day px-2' data-date='" + *//*new Date().getFullYear() + ("0" + (new Date().getMonth() + 1)).slice(-2) + ("0" + i).slice(-2)*//*item.kinmuDt + "'>" +
        "<div class=''>" +
        "<div class='fc-day-number py-2'>" + item.kinmuDt.substring(6, 8) + "</div>" +
        "</div>" +
        "</td>"
    $(".calendar-data tr.week1").append(str);
    q++;
}
else if (q < 14 - dayOfWeek) {
    str += "<td class='fc-day px-2' data-date='" + *//*new Date().getFullYear() + ("0" + (new Date().getMonth() + 1)).slice(-2) + ("0" + i).slice(-2)*//*item.kinmuDt + "'>" +
        "<div class=''>" +
        "<div class='fc-day-number py-2'>" + item.kinmuDt.substring(6, 8) + "</div>" +
        "</div>" +
        "</td>"
    $(".calendar-data tr.week2").append(str);
    q++;
} else if (q < 21 - dayOfWeek) {
    str += "<td class='fc-day px-2' data-date='" + *//*new Date().getFullYear() + ("0" + (new Date().getMonth() + 1)).slice(-2) + ("0" + i).slice(-2)*//*item.kinmuDt + "'>" +
        "<div class=''>" +
        "<div class='fc-day-number py-2'>" + item.kinmuDt.substring(6, 8) + "</div>" +
        "</div>" +
        "</td>"
    $(".calendar-data tr.week3").append(str);
    q++;
} else if (q < 28 - dayOfWeek) {
    str += "<td class='fc-day px-2' data-date='" + *//*new Date().getFullYear() + ("0" + (new Date().getMonth() + 1)).slice(-2) + ("0" + i).slice(-2)*//*item.kinmuDt + "'>" +
        "<div class=''>" +
        "<div class='fc-day-number py-2'>" + item.kinmuDt.substring(6, 8) + "</div>" +
        "</div>" +
        "</td>"
    $(".calendar-data tr.week4").append(str);
    q++;
} else if (q < 35 - dayOfWeek) {
    str += "<td class='fc-day px-2' data-date='" + *//*new Date().getFullYear() + ("0" + (new Date().getMonth() + 1)).slice(-2) + ("0" + i).slice(-2)*//*item.kinmuDt + "'>" +
        "<div class=''>" +
        "<div class='fc-day-number py-2'>" + item.kinmuDt.substring(6, 8) + "</div>" +
        "</div>" +
        "</td>"
    $(".calendar-data tr.week5").append(str);
    q++;
} else {
    str += "<td class='fc-day px-2' data-date='" + *//*new Date().getFullYear() + ("0" + (new Date().getMonth() + 1)).slice(-2) + ("0" + i).slice(-2)*//*item.kinmuDt + "'>" +
        "<div class=''>" +
        "<div class='fc-day-number py-2'>" + item.kinmuDt.substring(6, 8) + "</div>" +
        "</div>" +
        "</td>"
    $(".calendar-data tr.week6").append(str);
    q++;
}
})*/

};
GetData(year, month);

$(".prev-month").click(function () {
    $(".calendar-data tr td").remove()
    month--;
    if (month == 0) {
        year--;
        month = 12;
    }
    GetData(year, month);
})
$(".next-month").click(function () {
    $(".calendar-data tr td").remove()
    month++;
    if (month == 13) {
        year++;
        month = 1;
    }
    GetData(year, month);
})

function GetData(year, month) {
    $.ajax({
        url: "/Attendance/GetData/" + year + "-" + month,
        method: "get",
        Cache: "false",
        success: function (e) {
            Calendar(year, month, e);
        }
    })
}
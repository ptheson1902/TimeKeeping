function getdate() {
    var today = new Date();
    var h = today.getHours();
    var m = today.getMinutes();

    if (m < 10) {
        m = "0" + m;
    }
    if (h < 10) {
        h = "0" + h;
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

// yyyyMMdd文字列を日付に交換するfunction()
// params str
// return 日付
function strToDate(str) {
    var y = str.substring(0, 4)
    var m = str.substring(4, 6)
    var d = str.substring(6, 8)
    str = y + "/" + m + "/" + d
    return new Date(str);
}

// 今年
let year = new Date().getFullYear()
// 今月
let month = new Date().getMonth() + 1

// 実のカレンダーが作成
// @param year
// @param month
// @param e (月によってデータベースからデーターを取得)
function Calendar(year, month, e) {
    $(".calendar-data tr td").remove()
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

    if (e != null) {
        $(".fc-day").each(function () {
            let it = $(this);
            let count = 0;
            for (let j = 0; j < e.length; j++) {
                if (e[j].kinmuDt == it.data("date") && e[j].kinmuFrDate != null && e[j].kinmuToDate != null) {
                    count = 1;
                }
                else if (e[j].kinmuDt == it.data("date") && e[j].kinmuFrDate != null && e[j].kinmuToDate == null) {
                    count = 2
                }
            }
            if(count == 1)
                it.children().addClass("taikin");
            if(count == 2)
                it.children().addClass("shukin");
            if (count == 0 && it.data("date") != "" && new Date(new Date().getTime() - 86400000) > strToDate(it.data("date").toString()))
                it.children().addClass("yasumi");
        })
    }
};

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
        cache: false,
        url: "/Attendance/GetData/" + year + "-" + month,
        method: "get",
        Cache: "false",
        success: function (e) {
            Calendar(year, month, e);
        }
    })
}
GetData(year, month);
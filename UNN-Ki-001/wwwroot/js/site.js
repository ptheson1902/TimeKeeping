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
let year = new Date().getFullYear()
let month = new Date().getMonth() + 1
function calendar() {
    $(".fc-header .fc-header-center .year").text(year);
    $(".fc-header .fc-header-center .month").text(month);
    var dayOfWeek = new Date(year + "/" + month + "/1").getDay();
    var dayNumOfMonth = new Date(year , month, 0).getDate()
    $(".calendar-data tr.week1").append(day(1, dayOfWeek))
    $(".calendar-data tr.week2").append(day(8 - dayOfWeek))
    $(".calendar-data tr.week3").append(day(15 - dayOfWeek))
    $(".calendar-data tr.week4").append(day(22 - dayOfWeek))
    $(".calendar-data tr.week5").append(day1(29 - dayOfWeek, 35 - dayNumOfMonth - dayOfWeek))
    if (dayNumOfMonth - 35 + dayOfWeek > 0) {
        $(".calendar-data tr.week6").append(day1((36 - dayOfWeek), 42 - dayNumOfMonth - dayOfWeek))
    }
}
calendar();

$(".prev-month").click(function () {
    $(".calendar-data tr td").remove()
    month--;
    if (month == 0) {
        year --;
        month = 12;
    }
    calendar()
})
$(".next-month").click(function () {
    $(".calendar-data tr td").remove()
    month++;
    if (month == 13) {
        year ++;
        month = 1;
    }
    calendar()
})

// 所属コードをクリックする時、修正・削除のポップアップが表示
$(".shozoku_cd").click(function () {
    $("#updateModal input[name='shozoku_cd2']").val($(this).text())
    $("#updateModal input[name='shozoku_nm2']").val($(this).parent().next().text())
    $("#updateModal input[name='valid_flg2']").attr("checked", false);
    $("#updateModal input[name='valid_flg2'][value='" + $(this).parent().next().next().children().val() + "']").attr("checked", true)
})
//  所属メンテナンスの複写機能(Copy)
$(".shozokuData tr").click(function () {

    $(".shozokuData tr").removeClass("selected bg-light")
    $(this).addClass("selected bg-light");
})

$(".copy").click(function () {
    $(".tsuika").click();
    $("#tsuikaModal input[name='shozoku_cd1']").val($(".shozokuData tr.selected").children().children().text())
    $("#tsuikaModal input[name='shozoku_nm1']").val($(".shozokuData tr.selected").children()[1].textContent)
    $("#tsuikaModal input[name='valid_flg1']").attr("checked", false);
    $("#tsuikaModal input[name='valid_flg1'][value='" + $(".shozokuData tr.selected").children().next().next().children().val() + "']").attr("checked", true)
})

//  職種コードをクリックする時、修正・削除のポップアップが表示
$(".shokushu_cd").click(function () {
    $("#updateModal input[name='shokushu_cd2']").val($(this).text())
    $("#updateModal input[name='shokushu_nm2']").val($(this).parent().next().text())
    $("#updateModal input[name='valid_flg2']").attr("checked", false);
    $("#updateModal input[name='valid_flg2'][value='" + $(this).parent().next().next().children().val() + "']").attr("checked", true)
})
// 職種メンテナンスの複写機能(Copy)
$(".shokushuData tr").click(function () {

    $(".shokushuData tr").removeClass("selected bg-light")
    $(this).addClass("selected bg-light");
})

$(".skCopy").click(function () {
    $(".tsuika").click();
    $("#tsuikaModal input[name='shokushu_cd1']").val($(".shokushuData tr.selected").children().children().text())
    $("#tsuikaModal input[name='shokushu_nm1']").val($(".shokushuData tr.selected").children()[1].textContent)
    $("#tsuikaModal input[name='valid_flg1']").attr("checked", false);
    $("#tsuikaModal input[name='valid_flg1'][value='" + $(".shokushuData tr.selected").children().next().next().children().val() + "']").attr("checked", true)
})
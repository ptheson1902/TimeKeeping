var changeList;

function attach(kinmuDt) {
    Console.log("勤務レコード" + kinmuDt + "の変更を検知しました。");
}

$("form").submit(function () {
    $(this).children()[0].value = "test"
    //alert($(this).children()[0].value)
})
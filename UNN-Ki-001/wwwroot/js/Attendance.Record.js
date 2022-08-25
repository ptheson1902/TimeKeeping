var changeList;

function attach(kinmuDt) {
    Console.log("勤務レコード" + kinmuDt + "の変更を検知しました。");
}

$(window).on('load', function () {

    // ページ内の全てのフォームにTargetListのデータを紐づける処理
    $("form").submit(function () {
        $(this).append($("#targetListJson"));
    })
})
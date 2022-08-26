


$(window).on("load", function () {
    // 変更リクエストパラメーター
    var ChangeTracker = {};

    // 勤務表変更時の処理
    $(".kinmu").change(function () {
        // 連想配列を作成
        var OneChange = {
            kinmuCd: $(this).children()[0]
            , dakokuFr: $(this).children(".kinmu_dakoku_fr").children()[0].value
            , dakokuTo: $(this).children(".kinmu_dakoku_to").children()[0].value
            , kinmuFr: $(this).children(".kinmu_kinmu_fr").children()[0].value
            , kinmuTo: $(this).children(".kinmu_kinmu_to").children()[0].value
        }

        ChangeTracker[$(this).data("origin")] = OneChange
        console.log(ChangeTracker);
    })
})
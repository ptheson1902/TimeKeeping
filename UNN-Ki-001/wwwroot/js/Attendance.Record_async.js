// ページ内の全てのフォームにTargetListのデータを紐づける処理
$("form").append($("#targetListJson"));

// 変更リクエストパラメーター
var ChangeTracker = {};

// 勤務表変更時の処理
$(".kinmu").change(function () {
    // 要素を取得します。
    let dt = $(this).data("origin");
    let cd = $(this).children(".mkinmu").children("select");
    let dakoku_fr = $(this).children(".kinmu_dakoku_fr");
    let dakoku_fr_tm = dakoku_fr.children("input")[0];
    let dakoku_fr_kbn = dakoku_fr.children("select")[0];
    let dakoku_to = $(this).children(".kinmu_dakoku_to");
    let dakoku_to_tm = dakoku_to.children("input")[0];
    let dakoku_to_kbn = dakoku_to.children("select")[0];
    let kinmu_fr = $(this).children(".kinmu_kinmu_fr");
    let kinmu_fr_tm = kinmu_fr.children("input")[0];
    let kinmu_fr_kbn = kinmu_fr.children("select")[0];
    let kinmu_to = $(this).children(".kinmu_kinmu_to");
    let kinmu_to_tm = kinmu_to.children("input")[0];
    let kinmu_to_kbn = kinmu_to.children("select")[0];
    let biko = $(this).children(".kinmu_biko").children("textarea");

    // 値を取り出します。
    let res_cd = cd[0].value;
    // C#で取り込み時にまとめてパースするためにくっつけます。
    let res_dakoku_fr = dt + "_" + dakoku_fr_tm.value + "," + dakoku_fr_kbn.value;
    let res_dakoku_to = dt + "_" + dakoku_to_tm.value + "," + dakoku_to_kbn.value;
    let res_kinmu_fr = dt + "_" + kinmu_fr_tm.value + "," + kinmu_fr_kbn.value;
    let res_kinmu_to = dt + "_" + kinmu_to_tm.value + "," + kinmu_to_kbn.value;
    let res_biko = biko[0].value;

    // 連想配列を作成
    var OneChange = {
        kinmuCd: res_cd
        , dakokuFr: res_dakoku_fr
        , dakokuTo: res_dakoku_to
        , kinmuFr: res_kinmu_fr
        , kinmuTo: res_kinmu_to
        , biko: res_biko
    }

    ChangeTracker[$(this).data("origin")] = OneChange
    console.log(ChangeTracker);
})

// 各ボタン処理
$("form .submit").click(function () {
    $(this).parent().submit();
})

// データベース更新処理
$("form .save-change").click(function () {
    var kakunin = window.confirm("データベースへ登録実行しますか？");
    if (!kakunin) {
        return;
    }

    // チェンジトラッカーをjsonに変換
    let ChangeTrackerJson = JSON.stringify(ChangeTracker);
    // フォームに追加する
    $(this).parent().append($("<input type=\"hidden\" name=\"json\"/>").val(ChangeTrackerJson));
    // 送信
    console.log(ChangeTrackerJson);
    $(this).parent().submit();
})
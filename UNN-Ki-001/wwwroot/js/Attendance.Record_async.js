// ページ内の全てのフォームにTargetListのデータを紐づける処理
$("form").append($("#targetListJson"));

// 変更リクエストパラメーター
var ChangeTracker = {};
var ErrorList = {};

// 基本情報変更登録時の処理
$(".kinmu").focusout(function () {
    // オブジェクトを生成します
    var target = kinmuRecord($(this));

    // エラーチェック
    if (target.kinmuFrDate > target.kinmuToDate) {
        $(this).css('background-color', 'orange');
        ErrorList[$(this).data("origin")] = target;
        delete ChangeTracker[$(this).data("origin")];
    } else {
        $(this).css('background-color', '');
        ChangeTracker[$(this).data("origin")] = target;
        delete ErrorList[$(this).data("origin")];
    }

    // 休憩入力可否チェック
    let kyukeiButton = $(this).find(".btn-kyukei");
    if (target.kinmuFrTm == "" || target.kinmuToTm == "") {
        kyukeiButton.prop("disabled", true);
        console.log(false);
    } else {
        kyukeiButton.prop("disabled", false);

        console.log(true);
    }

    // チェッククリア後
    errorMessageRefresh();
})

function errorMessageRefresh() {
    // 要素の取得
    let elem = $(".errorMessage")
    elem.css("color", "red");

    let submitButton = $(".save-change");
    // 値を変更
    if (Object.keys(ErrorList).length > 0) {
        elem.text("入力したデータが不正です。");
        // 実行ボタンをdissableに
        submitButton.prop("disabled", true);
    } else {
        elem.text("");
        // 実行ボタンをアクティブに
        submitButton.prop("disabled", false);
    }
}

// 打刻オブジェクトを日付型に変換します
function formatToDate(kinmuDt, tm, kbn) {
    let year = kinmuDt.substr(0, 4);
    let month = kinmuDt.substr(4, 2);
    let day = kinmuDt.substr(6);
    let dateString = year + "-" + month + "-" + day + "T" + tm;

    var date = new Date(dateString);

    let temp = date.getDate();
    if (kbn == "1") {
        temp -= 1;
    }
    if (kbn == "2") {
        temp += 1;
    }
    date.setDate(temp);

    return date;
}

// 勤務レコード（1日分）を取り扱う関数オブジェクトの宣言
function kinmuRecord(kinmu) {
    // 対象が存在しなければnullを返す
    if (kinmu.length == 0) {
        return;
    }

    // 値の取り出し
    let kinmuDt = kinmu.data("origin").toString();
    let kinmuCd = kinmu.children(".mkinmu").children("select").value;

    let dakokuFr = kinmu.children(".kinmu_dakoku_fr");
    let dakokuFrTm = dakokuFr.children("input")[0].value;
    let dakokuFrKbn = dakokuFr.children("select")[0].value;

    let dakokuTo = kinmu.children(".kinmu_dakoku_to");
    let dakokuToTm = dakokuTo.children("input")[0].value;
    let dakokuToKbn = dakokuTo.children("select")[0].value;

    let kinmuFr = kinmu.children(".kinmu_kinmu_fr");
    let kinmuFrTm = kinmuFr.children("input")[0].value;
    let kinmuFrKbn = kinmuFr.children("select")[0].value;

    let kinmuTo = kinmu.children(".kinmu_kinmu_to");
    let kinmuToTm = kinmuTo.children("input")[0].value;
    let kinmuToKbn = kinmuTo.children("select")[0].value;

    // オブジェクトの生成
    let obj = {
        kinmuDt: kinmuDt,
        kinmuCd: kinmuCd,
        kinmuCd: kinmu.children(".mkinmu").children("select").val(),
        dakokuFrTm: dakokuFrTm,
        dakokuFrKbn: dakokuFrKbn,
        dakokuToTm: dakokuToTm,
        dakokuToKbn: dakokuToKbn,
        kinmuFrTm: kinmuFrTm,
        kinmuFrKbn: kinmuFrKbn,
        kinmuToTm: kinmuToTm,
        kinmuToKbn: kinmuToKbn,
        biko: kinmu.children(".kinmu_biko").children("textarea")[0].value,
        dakokuFrDate: formatToDate(kinmuDt, dakokuFrTm, dakokuFrKbn),
        dakokuToDate: formatToDate(kinmuDt, dakokuToTm, dakokuToKbn),
        kinmuFrDate: formatToDate(kinmuDt, kinmuFrTm, kinmuFrKbn),
        kinmuToDate: formatToDate(kinmuDt, kinmuToTm, kinmuToKbn)
    }

    return obj;
}

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

    // パラメーターの作成
    let paramArray = {};
    Object.keys(ChangeTracker).forEach(function (key) {
        let target = ChangeTracker[key];
        var res_cd = target.kinmuCd;
        console.log(res_cd);
        // C#で取り込み時にまとめてパースするためにくっつけます。
        var res_dakoku_fr = target.kinmuDt + "," + target.dakokuFrTm + "," + target.dakokuFrKbn;
        var res_dakoku_to = target.kinmuDt + "," + target.dakokuToTm + "," + target.dakokuToKbn
        var res_kinmu_fr = target.kinmuDt + "," + target.kinmuFrTm + "," + target.kinmuFrKbn;
        var res_kinmu_to = target.kinmuDt + "," + target.kinmuToTm + "," + target.kinmuToKbn;
        var res_biko = target.biko;
        var res_kinmuCd = target.kinmuCd;

        // 連想配列を作成
        var OneChange = {
            kinmuCd: res_cd
            , dakokuFr: res_dakoku_fr
            , dakokuTo: res_dakoku_to
            , kinmuFr: res_kinmu_fr
            , kinmuTo: res_kinmu_to
            , biko: res_biko
            , kinmuCd: res_kinmuCd
        }

        paramArray[key] = OneChange;
    })

    // チェンジトラッカーをjsonに変換
    let param = JSON.stringify(paramArray);
    $("input[name='json']").remove();

    // フォームに追加する
    $(this).parent().append($("<input type=\"hidden\" name=\"json\"/>").val(param));
    // 送信
    ajaxSubmit($(this).parent(), true);
})


function ajaxSubmit(elem, isTodoku) {
    $.ajax({
        method: "post",
        data: elem.serialize(),
        async: false,
    }).done(function (data) {
        if (data.message == null) {
            window.location.reload();
        } else {
            if (isTodoku) {
                $(".updatedErrorMessage").text(data.message)
                let kinmuDt = data.kinmuDt;
                if (kinmuDt != "") {

                    $(".kinmu").each(function () {
                        if ($(this).data("origin") == kinmuDt)
                            $(this).addClass("bg-danger");
                        else
                            $(this).removeClass("bg-danger");
                    })
                }
            }
            else {
                $(".kyukei-error-message").text(data.message);
            }
        }
    })
}

function kyukeiCheck(elem) {
    let start = elem.children(".start");
    let end = elem.children(".end");
    let kinmuDt = $(".kyukei-table").val();

    // 開始時間を取得
    let dakokuFrKbn = start.children("select").val();
    let dakokuFr = start.children("input").val();
    let dakokuFrDate = formatToDate(kinmuDt, dakokuFr, dakokuFrKbn);

    // 終了時間を取得
    let dakokuToKbn = end.children("select").val();
    let dakokuTo = end.children("input").val();
    let dakokuToDate = formatToDate(kinmuDt, dakokuTo, dakokuToKbn);

    // 入力チェック
    if (dakokuFrDate != null && dakokuToDate != null && dakokuToDate < dakokuFrDate) {
        elem.addClass("bg-danger");
        $(".kyukei-error-message").text("入力が不正です");
    } else {
        elem.removeClass("bg-danger");
        $(".kyukei-error-message").empty();
    }
}

// 休憩の登録処理
function kyukeiSaveChange() {
    // 1行ずつコレクションで取得
    allRow = $(".kyukei-row-data");
    var resArray = {};
    resArray["kinmuDt"] = $(".kyukei-table").val();
    var array = [];

    allRow.each(function () {
        let start = $(this).children(".start");
        let end = $(this).children(".end");

        // 開始時間を取得
        let dakokuFrKbn = start.children("select").val();
        let dakokuFr = start.children("input").val();

        // 終了時間を取得
        let dakokuToKbn = end.children("select").val();
        let dakokuTo = end.children("input").val();

        array.push({
            dakokuFrTm: dakokuFr,
            dakokuFrKbn: dakokuFrKbn,
            dakokuToTm: dakokuTo,
            dakokuToKbn: dakokuToKbn
        })
    })

    resArray["list"] = array;

    // 登録
    var kakunin = window.confirm("データベースへ登録実行しますか？");
    if (!kakunin) {
        return;
    }
    let form = $("#kyukeiForm")
    let kyukeiJsonParam = JSON.stringify(resArray);
    console.log(kyukeiJsonParam);
    form.children(".kyukeiJson").val(kyukeiJsonParam);
    ajaxSubmit(form, false);
}

// 休憩ボタン押下時の処理
function openKyukei(kinmuDt, shainNo, kigyoCd) {
    // 非同期で休憩レコードのjsonを受信する
    $.ajax({
        url: "./Record/Kyukei",
        method: "GET",
        contentType: "application/json",
        data: {
            kinmuDt: kinmuDt,
            shainNo: shainNo,
            kigyoCd: kigyoCd
        },
        cache: false
    }).done(function (res) {
        // モーダルウィンドウ初期化
        $target = $(".kyukei-table");
        $target.empty();
        $target.val(kinmuDt);

        // 行を追加
        res.forEach((record, index) => {
            addKyuke(record);
        })
    })
}

// 休憩モーダル行削除ボタン押下時の処理
function deleteKyukei() {
    // チェックボックスの入ったレコードを全て取得する
    var target = $(".kyukei-table input:checked").parent().parent();
    target.remove();
}

function addKyuke(data) {
    var target = $(".kyukei-table");
    // 行を追加
    $template = $(".kyukei-template .row-template").clone();
    $template.attr("class", "kyukei-row-data");
    $template.focusout(function () {
        kyukeiCheck($(this));
    });

    if (data != null) {
        // 開始時刻
        if (data.start != "") {
            $start = $template.children(".start");
            $start.children("input[type='time']").val(data.start);
            $start.children("select").val(data.startKbn);
        }

        // 終了時刻
        if (data.end != "") {
            $end = $template.children(".end");
            $end.children("input[type='time']").val(data.end);
            $end.children("select").val(data.endKbn);
        }
    }

    target.append($template);
}
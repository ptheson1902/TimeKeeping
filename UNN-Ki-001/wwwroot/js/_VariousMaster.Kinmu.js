var jsonData = JSON.parse($("#data").val())
$(".kinmuInsert").click(function () {
    $(".KinmuModalForm")[0].reset();
    $("#KyukeiAutoFlg").attr("checked", false)
    $("#KinmuFrCtrlFlg").attr("checked", false)
    $(".submitBtn").text("登録").removeClass("btn-warning").addClass("btn-success").val("insert")
    $(".deleteBtn").addClass("d-none")
})

$(".kinmuSelected").click(function () {
    var cd = $(this).text();
    jsonData.forEach(e => {
        if (e.KinmuCd == cd) {
            KinmuData(e, true)
        }
    })
    $(".submitBtn").text("更新").removeClass("btn-success").addClass("btn-warning").val("update")
    $(".deleteBtn").removeClass("d-none")
})

$(".kinmuCopy").click(function () {
    var cd = $(".kinmuData tr.selected").children().children().text();
    jsonData.forEach(e => {
        if (e.KinmuCd == cd) {
            KinmuData(e, false)
        }
    })
    $(".deleteBtn").addClass("d-none")
    $(".submitBtn").text("登録").addClass("btn-success")
})

$(".kinmuData tr").click(function () {
    $(".kinmuData tr").removeClass("selected bg-selected")
    $(this).addClass("selected bg-selected")
})

function KinmuData(e, isReadonly) {
    console.log($(".KinmuModalForm .kyukei2ToKbn option[value='" + e.Kyukei2ToKbn + "']"))
    // 勤務コード
    $(".KinmuModalForm .kinmuCd").val(e.KinmuCd).attr("readonly", isReadonly)
    // 勤務名
    $(".KinmuModalForm .kinmuNm").val(e.KinmuNm)
    // 勤務分類
    $(".KinmuModalForm .kinmuBunrui option[value='" + e.KinmuBunrui + "']").attr("selected",true)
    // 勤務開始区分
    $(".KinmuModalForm .kinmuFrKbn option[value='" + e.KinmuFrKbn + "']").attr("selected", true)
    // 勤務開始タイム
    $(".KinmuModalForm .kinmuFrTm").val(e.KinmuFrTm)
    // 勤務終了区分
    $(".KinmuModalForm .kinmuToKbn option[value='" + e.KinmuToKbn + "']").attr("selected", true)
    // 勤務終了タイム
    $(".KinmuModalForm .kinmuToTm").val(e.KinmuToTm)
    // 休憩開始区分１
    $(".KinmuModalForm .kyukei1FrKbn option[value='" + e.Kyukei1FrKbn + "']").attr("selected", true)
    // 休憩開始タイム１
    $(".KinmuModalForm .kyukei1FrTm").val(e.Kyukei1FrTm)
    // 休憩終了区分１
    $(".KinmuModalForm .kyukei1ToKbn option[value='" + e.Kyukei1ToKbn + "']").attr("selected", true)
    // 休憩終了タイム１
    $(".KinmuModalForm .kyukei1ToTm").val(e.Kyukei1ToTm)
    // 休憩開始区分２
    $(".KinmuModalForm .kyukei2FrKbn option[value='" + e.Kyukei2FrKbn + "']").attr("selected", true)
    // 休憩開始タイム２
    $(".KinmuModalForm .kyukei2FrTm").val(e.Kyukei2FrTm)
    // 休憩終了区分２
    $(".KinmuModalForm .kyukei2ToKbn option[value='" + e.Kyukei2ToKbn + "']").attr("selected", true)
    // 休憩終了タイム２
    $(".KinmuModalForm .kyukei2ToTm").val(e.Kyukei2ToTm)
    // 休憩開始区分３
    $(".KinmuModalForm .kyukei3FrKbn option[value='" + e.Kyukei3FrKbn + "']").attr("selected", true)
    // 休憩開始タイム３
    $(".KinmuModalForm .kyukei3FrTm").val(e.Kyukei3FrTm)
    // 休憩終了区分３
    $(".KinmuModalForm .kyukei3ToKbn option[value='" + e.Kyukei3ToKbn + "']").attr("selected", true)
    // 休憩終了タイム３
    $(".KinmuModalForm .kyukei3ToTm").val(e.Kyukei3ToTm)
    // 休憩打刻を退勤打刻時に自動追加する
    $("#KyukeiAutoFlg").attr("checked", e.KyukeiAutoFlg == 1 ? true : false)
    // 始業前打刻を勤務開始時間にあわせる
    $("#KinmuFrCtrlFlg").attr("checked", e.KinmuFrCtrlFlg == 1 ? true : false)
    // 勤務開始の丸目区分
    $(".KinmuModalForm .kinmuFrMarumeKbn option[value='" + e.KinmuFrMarumeKbn + "']").attr("selected", true)
    // 勤務開始の丸目タイム
    $(".KinmuModalForm .kinmuFrMarumeTm").val(e.KinmuFrMarumeTm)
    // 勤務終了の丸目区分
    $(".KinmuModalForm .kinmuToMarumeKbn option[value='" + e.KinmuToMarumeKbn + "']").attr("selected", true)
    // 勤務終了の丸目タイム
    $(".KinmuModalForm .kinmuToMarumeTm").val(e.KinmuToMarumeTm)
    // 休憩開始の丸目区分
    $(".KinmuModalForm .kyukeiFrMarumeKbn option[value='" + e.KyukeiFrMarumeKbn + "']").attr("selected", true)
    // 休憩開始の丸目タイム
    $(".KinmuModalForm .kyukeiFrMarumeTm").val(e.KyukeiFrMarumeTm)
    // 休憩終了の丸目区分
    $(".KinmuModalForm .kyukeiToMarumeKbn option[value='" + e.KyukeiToMarumeKbn + "']").attr("selected", true)
    // 休憩終了の丸目タイム
    $(".KinmuModalForm .kyukeiToMarumeTm").val(e.KyukeiToMarumeTm)
    // 勤務マスタの有効/無効
    $(".KinmuModalForm input[value='" + e.ValidFlg + "'].validFlg").attr("checked", true)
}
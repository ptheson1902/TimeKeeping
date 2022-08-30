var jsonData = JSON.parse($("#data").val())
console.log(jsonData)
$(".kyktInsert").click(function () {
    $(".KyktModalForm")[0].reset();
    $(".submitBtn").text("登録").removeClass("btn-warning").addClass("btn-success").val("insert")
    $(".deleteBtn").addClass("d-none")
})

$(".kyktSelected").click(function () {
    var cd = $(this).text();
    console.log(cd)
    jsonData.forEach(e => {
        if (e.KoyokeitaiCd == cd) {
            KyktData(e, true)
        }
    })
    $(".submitBtn").text("更新").removeClass("btn-success").addClass("btn-warning").val("update")
    $(".deleteBtn").removeClass("d-none")
})

$(".kyktCopy").click(function () {
    var cd = $(".kyktData tr.selected").children().children().text();
    jsonData.forEach(e => {
        if (e.KoyokeitaiCd == cd) {
            KyktData(e, false)
        }
    })
    $(".deleteBtn").addClass("d-none")
    $(".submitBtn").text("登録").addClass("btn-success")
})

$(".kyktData tr").click(function () {
    $(".kyktData tr").removeClass("selected bg-selected")
    $(this).addClass("selected bg-selected")
})

function KyktData(e, isReadonly) {
    // 雇用形態コード
    $(".KyktModalForm .kyktCd").val(e.KoyokeitaiCd).attr("readonly", isReadonly)
    // 雇用形態名
    $(".KyktModalForm .kyktNm").val(e.KoyokeitaiNm)
    // 雇用形態マスタの有効/無効
    $(".KyktModalForm input[value='" + e.ValidFlg + "'].validFlg").attr("checked", true)
}
var jsonData = JSON.parse($("#data").val())
console.log(jsonData)
$(".SksInsert").click(function () {
    $(".SksModalForm")[0].reset();
    $(".submitBtn").text("登録").removeClass("btn-warning").addClass("btn-success").val("insert")
    $(".deleteBtn").addClass("d-none")
})

$(".SksSelected").click(function () {
    var cd = $(this).text();
    jsonData.forEach(e => {
        if (e.ShokushuCd == cd) {
            SksData(e, true)
        }
    })
    $(".submitBtn").text("更新").removeClass("btn-success").addClass("btn-warning").val("update")
    $(".deleteBtn").removeClass("d-none")
})

$(".SksCopy").click(function () {
    var cd = $(".SksData tr.selected").children().children().text();
    jsonData.forEach(e => {
        if (e.ShokushuCd == cd) {
            SksData(e, false)
        }
    })
    $(".deleteBtn").addClass("d-none")
    $(".submitBtn").text("登録").addClass("btn-success")
})

$(".SksData tr").click(function () {
    $(".SksData tr").removeClass("selected bg-selected")
    $(this).addClass("selected bg-selected")
})

function SksData(e, isReadonly) {
    // 職種コード
    $(".SksModalForm .SksCd").val(e.ShokushuCd).attr("readonly", isReadonly)
    // 職種名
    $(".SksModalForm .SksNm").val(e.ShokushuNm)
    // 職種マスタの有効/無効
    $(".SksModalForm input[value='" + e.ValidFlg + "'].validFlg").attr("checked", true)
}
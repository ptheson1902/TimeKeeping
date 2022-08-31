var jsonData = JSON.parse($("#data").val())
$(".SzkInsert").click(function () {
    $(".SzkModalForm")[0].reset();
    $(".submitBtn").text("登録").removeClass("btn-warning").addClass("btn-success").val("insert")
    $(".deleteBtn").addClass("d-none")
})

$(".SzkSelected").click(function () {
    var cd = $(this).text();
    jsonData.forEach(e => {
        if (e.ShozokuCd == cd) {
            SzkData(e, true)
        }
    })
    $(".submitBtn").text("更新").removeClass("btn-success").addClass("btn-warning").val("update")
    $(".deleteBtn").removeClass("d-none")
})

$(".SzkCopy").click(function () {
    var cd = $(".SzkData tr.selected").children().children().text();
    jsonData.forEach(e => {
        if (e.ShozokuCd == cd) {
            SzkData(e, false)
        }
    })
    $(".deleteBtn").addClass("d-none")
    $(".submitBtn").text("登録").addClass("btn-success")
})

$(".SzkData tr").click(function () {
    $(".SzkData tr").removeClass("selected bg-selected")
    $(this).addClass("selected bg-selected")
})

function SzkData(e, isReadonly) {
    // 職種コード
    $(".SzkModalForm .SzkCd").val(e.ShozokuCd).attr("readonly", isReadonly)
    // 職種名
    $(".SzkModalForm .SzkNm").val(e.ShozokuNm)
    // 職種マスタの有効/無効
    $(".SzkModalForm input[value='" + e.ValidFlg + "'].validFlg").attr("checked", true)
}

$(".SzkModalForm").submit(function () {
    if (!confirm("test"))
        return false;
})
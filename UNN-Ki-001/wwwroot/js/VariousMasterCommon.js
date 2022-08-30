$(".SearchForm .card-header .minimizeBtn").click(function () {
    $(this).addClass("d-none")
    $(this).next().removeClass("d-none")
    $(".SearchForm .card-body").addClass("minimize")
    //$(".SearchForm .card-body").sliceUp(300)
})
$(".SearchForm .card-header .maximizeBtn").click(function () {
    $(this).addClass("d-none")
    $(this).prev().removeClass("d-none")
    $(".SearchForm .card-body").removeClass("minimize")
})
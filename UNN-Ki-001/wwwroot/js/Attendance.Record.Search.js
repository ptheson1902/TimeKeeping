function SendPost(target) {
    $("#SubForm input[name=target]").val(target);
    $("#SubForm").submit();
}
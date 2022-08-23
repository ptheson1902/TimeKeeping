function SendPost(target) {
    $("#SubForm input[name='index']").val(target);
    $("#SubForm").submit();
}
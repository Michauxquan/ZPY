$(function() {
    $("#popup").click(function() { $("#popup").hide() });
});
function showBigImg(src) { 
    $("#popup").find("img").attr('src', src);
    $("#popup").show();
}


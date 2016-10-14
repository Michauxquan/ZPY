new PCAS("province3", "city3", "area3");
$(function() {
    $('#focusit').click(function() { focususer(); });
});

function focususer() {
    $.post('/User/Focususer', { id: $('#userinfoli').data('id') }, function (data) {
        console.log(data.result);
        alert(data.result==1?"关注成功":data.result==-2?"不能关注自己":"请登录后在操作");
    });
}





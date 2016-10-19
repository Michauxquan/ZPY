
var szReg = /^(\w-*\.*)+@(\w-?)+(\.\w{2,})+$/;
$(function () {
    $('#getNewPWD').click(function () {
        if ($('#username').val() == "") {
            $('#nametip').html('*登陆账号不能为空');
            return false;
        }
        if ($('#codetext').val().toLowerCase() != $('#txt1').val().toLowerCase()) {
            $('#nametip').html('*验证码不正确');
            show();
            return false;
        }
        if (!szReg.test($('#useremail').val())) {
            $('#nametip').html('*邮箱格式不正确');
            return false;
        }
        $.post('/Help/RestPwd', { loginname: $('#username').val(), useremail: $('#useremail').val() }, function (data) {
            if (data.result) {
                alert('密码重置成功,请查收注册邮箱');
                location.href = '/Home/Login';
            } else {
                alert(data.errorMsg);
            }
        });
    });
});
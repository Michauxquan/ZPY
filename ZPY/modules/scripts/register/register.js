
var contineRegister = false;
$(function () {
    $("#register").validate({ meta: "validate" });
    GetAddvert();
    $('#username').blur(function () {
        var reg = /^([a-zA-Z0-9_]){6,12}/;
        if (!reg.test($('#username').val())) {
            $('#username').next().addClass('red').html('用户名格式输入错误');
            return false;
        } else {
            $.get("UserNameCheck", { username: $('#username').val() }, function (data) {
                if (!data.result) {
                    $('#username').next().addClass('red').html('用户名已存在');
                } else {
                    contineRegister = true;
                }
            });
            $('#username').next().removeClass('red').html('由字符数字或下划线组成');
        }
    });
    $('#password').blur(function () {
        if ($('#password').val().length < 6 || $('#password').val().length > 12) {
            $('#password').next().addClass('red').html('密码格式输入不正确');
            contineRegister = false;
            return false;
        } else {
            $('#password').next().removeClass('red').html('长度为6-16个字符'); 
        }
    });
    $('#email').blur(function () {
        var szReg = /^(\w-*\.*)+@(\w-?)+(\.\w{2,})+$/;
        if (!szReg.test($('#email').val())) {
            $('#email').next().addClass('red').html('邮箱格式输入不正确');
            contineRegister = false;
            return false;
        } else {
            $('#email').next().removeClass('red').html('输入有效邮箱地址');
            contineRegister = true;
        }
    });
    $('#inputRandom').blur(function () {
        if ($('#inputRandom').val().toLowerCase() != $('#txt1').val().toLowerCase()) {
            $('#txt1').next().addClass('red').html('验证码输入有误');
            contineRegister = false;
            return false;
        } else {
            $('#txt1').next().html('');
            contineRegister = true;
        }
    });
    $('#confirm_password').blur(function () {
        if (($('#confirm_password').val() == '' && $('#confirm_password') != null) || $('#confirm_password').val() != $('#password').val()) {
            $('#confirm_password').next().addClass('red').html('密码输入不一致');
            contineRegister = false;
            return false;
        } else {
            $('#confirm_password').next().removeClass('red').html('再次输入密码');
            contineRegister = true;
        }
    }); 
    $('.btnregister').bind('click', function () {  
        if (contineRegister) {
            if (!$('#agree').is(':checked')) { 
                alert('注册协议未选中，注册失败');
                return false;
            } 
            $.post("UserRegister", { loginname: $('#username').val(), pwd: $('#password').val() }, function (data) {
                if (data.result) {
                    window.location.href = "/Home/Register2";
                }
            });
        } else {
            alert('验证未通过请检查后再提交！');
        }
    });
});
function GetAddvert() {
    $.post('/Home/GetAdvertList',
        {
            imgtype: "",
            view: $('#pagecontroller').val() + '/' + $('#pageaction').val()
        }, function (data) {
            var header = "";
            for (var i = 0; i < data.items.length; i++) {
                var item = data.items[i];
                if (item.ImgType == "Header") {
                    header = '<a href="' + (item.LinkUrl != "" ? item.LinkUrl : 'javascript:void(0);') + '" title="' + item.Content + '">' +
                        '<img style="width:960px;height:80px;" src="' + data.BaseUrl + item.ImgUrl + '" alt="' + item.Content + '" >' +
                        '</a>';
                }
            }
            $('.advertisement').html(header);
        });
}
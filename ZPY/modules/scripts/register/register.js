
var contineRegister = false;
$(function () {
    $("#register").validate({ meta: "validate" });

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
        if ($('#inputRandom').val() != $('#txt1').val()) {
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
       
        if (typeof ($('input:radio[name="sex"]:checked').data('value')) == 'undefined') {
            contineRegister = false;
            alert('性别未选择，注册失败');
            return false;
        }
        if (contineRegister) {
            if (!$('#agree').is(':checked')) { 
                alert('注册协议未选中，注册失败');
                return false;
            }
            var item = {
                LoginName: $('#username').val(),
                LoginPWD: $('#password').val(),
                Email: $('#email').val(),
                Sex: $('input:radio[name="sex"]:checked').data('value'),
                IsMarry: $('#marry option:selected').val(),
                Education: $('#education option:selected').val(),
                BHeight: $('#bheight option:selected').val(),
                QQ: $('#qqCode').val(),
                MobilePhone: $('#mobilephone').val(),
                Provine: $('.province option:selected').val(),
                City: $('.city option:selected').val(),
                District: $('.areaqu option:selected').val()
            }
            $.post("UserRegister",{entity: JSON.stringify(item)},function (data) {
                if (data.result) {
                    window.location.href = "/User/UserInfo";
                }
            });
        } else {
            alert('验证未通过请检查后再提交！');
        }
    });

});

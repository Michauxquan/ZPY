var szReg = /^(\w-*\.*)+@(\w-?)+(\.\w{2,})+$/;
new PCAS("province3", "city3", "area3"); 
$(function () {
    $('#username').blur(function () { 
        if ($('#username').val()=="") {
            $('#username').next().addClass('red').html('用户名格式输入错误');
            return false;
        } 
    });
    $('.myservice').click(function () {
        if (typeof ($(this).attr('checked')) != 'undefined') {
            $(this).removeAttr('checked');
        } else {
            $(this).attr('checked', 'checked');
        }
    });
    $('.btnregister').bind('click', function () {
        if ($('#username').val() == "") {
            alert('昵称未填写，注册失败');
            return false;
        }
        if (typeof ($('input:radio[name="sex"]:checked').data('value')) == 'undefined') {
            alert('性别未选择，注册失败');
            return false;
        }
        if ($('.province option:selected').val() == "") {
            alert('当前所在地未选择，不予提交');
            return false;
        }
        if ($('#userTalkTo').val() == "") {
            alert('自我介绍为空，不予提交');
            return false;
        } 
       
        if (!szReg.test($('#email').val())) {
            alert('邮箱格式输入不正确');
            return false;
        }  
        var myservic = '';
        $('.myservice').each(function (i, v) {
            if ($(v).attr('checked') == 'checked') {
                myservic += $(v).val() + ',';
            }
        });

        var age = 16;
        if ($('#userbirthday').val() == "") {
            alert('出生日期未填写，不予提交');
            return false;
        } else {
            if (new Date().getFullYear() - new Date($('#userbirthday').val()).getFullYear() < 16) {
                alert('年龄小于16岁，不予提交');
                return false;
            }
        }
        age = new Date().getFullYear() - new Date($('#userbirthday').val()).getFullYear();
        var item = {
            name: $('#username').val(),
            email: $('#email').val(),
            sex: $('input:radio[name="sex"]:checked').data('value'),
            isMarry: $('#marry option:selected').val(),
            education: $('#education option:selected').val(),
            bHeight: $('#bheight option:selected').val(),
            QQ: $('#qqCode').val(),
            mobilePhone: $('#mobilephone').val(),
            province: $('.province option:selected').val(),
            city: $('.city option:selected').val(),
            district: $('.areaqu option:selected').val(),
            bWeight: $('#BWeight option:selected').val(),
            jobs: $('#Jobs option:selected').val(),
            bPay: $('#BPay option:selected').val(),
            birthday:$('#userbirthday').val()+" 00:00:00",
            age: age,
            talkTo: $('#userTalkTo').val(),
            myservice: myservic, 
            myContent: $('#MyContent option:selected').val()
        } 
        $.post("UserRegister2",{entity: JSON.stringify(item)},function (data) {
            if (data.result) {
                window.location.href = "/User/UserInfo";
            }
        }); 
    });

});

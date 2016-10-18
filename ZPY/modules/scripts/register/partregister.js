
new PCAS("province3", "city3", "area3"); 
$(function () {
    $('#username').blur(function () { 
        if ($('#username').val()=="") {
            $('#username').next().addClass('red').html('用户名格式输入错误');
            return false;
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
            var item = {
                Name: $('#username').val(),
                Email: $('#email').val(),
                Sex: $('input:radio[name="sex"]:checked').data('value'),
                IsMarry: $('#marry option:selected').val(),
                Education: $('#education option:selected').val(),
                BHeight: $('#bheight option:selected').val(),
                QQ: $('#qqCode').val(),
                MobilePhone: $('#mobilephone').val(),
                Provine: $('.province option:selected').val(),
                City: $('.city option:selected').val(),
                District: $('.areaqu option:selected').val(),
                bWeight: $('#BWeight option:selected').val(),
                jobs: $('#Jobs option:selected').val(),
                bPay: $('#BPay option:selected').val(),
                birthday:$('#userbirthday').val(),
                age: age,
                talkTo: $('#userTalkTo').val(),
                myservice: myservic, 
                myContent: $('#MyContent option:selected').val()
        }
            console.log(item);
            //$.post("UserRegister",{entity: JSON.stringify(item)},function (data) {
            //    if (data.result) {
            //        window.location.href = "/User/UserInfo";
            //    }
            //}); 
    });

});

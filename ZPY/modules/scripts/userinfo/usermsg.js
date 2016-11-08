new PCAS("province3", "city3", "area3");
var reg = /^[1-9]*[1-9][0-9]*$/; 
$(function() {
    $('#focusit').click(function () { focususer(); });
    getUserRate();
    $('#feedlink').click(function () { feedshow(); });
    $('.feed-cancle').click(function () { feedcancle(); });
    $('.feed-ok').click(function () { feedSave(); });
    $('#zhdialog .tips-content li').each(function (i, v) {
        $(v).click(function () {
            var content = $(v).html();
             var item= {
                GUID: $('#userinfoli').data('id'),
                Content: content.replace("\n", "<br>").replace(/</g, "&lt").replace(/>/g, "&gt"),
                Type: 0
            }
            SaveZH(item);
        });
    });
    $('.showreply').click(function() {
        showreply($(this), $('#replydialog'));
    });
    $('.showzh').click(function () {
        showreply($(this), $('#zhdialog'));
    });
    $('#nextli').click(function () {
        $('#nextli').parent().hide();
        $('.payli').show();
    });
    $('input:radio[name="jinbi"]').each(function (i, v) { 
        $(v).click(function() {
            $('#othergold').val(''); 
        });
    });
    $('#othergold').change(function () {
        if ($(this).val() != '') {
            $('input:radio[name="jinbi"]').each(function (i, v) {
                $(v).removeAttr("checked");
            });
        }
    });
    $('#paybtn').click(function() {
        var golds = $('input:radio[name="change"]:checked').val();
        var paytype = $('input:radio[name="payway"]:checked').val();
        if (typeof (golds) == 'undefined') { golds = $('#othergold').val(); }
        if (!reg.test(golds)) {  alert('金额格式不正确，请重新选择或输入'); return false;  }
        if (typeof (paytype) == 'undefined') {  alert('支付类型为选择'); return false; }
        $.post('/Help/PayOtherMoney', { gold: golds, paytype: paytype },
        function (data) {
            if (data.result) {
                $('#zhdialog').hide();
                alert('提交成功');
            } else {
                alert(data.errorMsg);
                location.href = '/Home/Login';
            }
        });
    });
    $(document).click(function (e) { 
        if (!$(e.target).parents().hasClass("replybtns") && !$(e.target).parents().hasClass("tips-content") && !$(e.target).parents().hasClass("showreply")
            && !$(e.target).hasClass("box-main") && !$(e.target).parents().hasClass("box-main")) {
            $("#replydialog").hide();
        }
        if (!$(e.target).parents().hasClass("tips-content") && !$(e.target).parents().hasClass("showzh")
           && !$(e.target).hasClass("box-main") && !$(e.target).parents().hasClass("box-main")) {
            $("#zhdialog").hide();
        }
    });
});

function focususer() {
    $.post('/User/Focususer', { id: $('#userinfoli').data('id') }, function (data) {
        alert(data.result==1?"关注成功":data.result==-2?"不能关注自己":"请登录后在操作");
    });
}

function getUserRate() {
    $.post('/User/GetUserRateds', { type: 2, userid: $('#userinfoli').data('id'), pageIndex: 1, pageSize: 5 }, function (data) {
        var html = '';
        for (var i = 0; i < data.items.length; i++) {
            var item = data.items[i];
            if (item.Rated != null && item.Rated != "") {
                html += ' <li><img src="' + (item.UserAvatar != "" ? item.UserAvatar : "/modules/images/pingjia.png") + '" width="30px">' + (item.Rated.length > 30 ? item.Rated.substring(0, 30) : item.Rated) + '</li>';
            }
        }
        $('#userrade').html(html);
        getNewNeeds();
        getuserlike('',6);
    });
}

function getUserLink(cname) {
    $.post('/User/GetUserLinkInfo', { cname: cname, seeid: $('#userinfoli').data('id'), seename: $('#userinfoli').data('name') }, function (data) {
        if (data.msgError != "") {
            alert(data.msgError);
        } else {
            if (cname == "MobilePhone") {
                $('#userMobilePhone').html('手机：' + data.result).next().hide();
            } else if (cname == "Email") {
                $('#userEmail').html('邮箱：' + data.result).next().hide();
            } else if (cname == "QQ") {
                $('#userQQ').html('Q&nbsp;Q：' + data.result).next().hide();
            } else {
                $('#userWX').html('Q&nbsp;Q：' + data.result).next().hide();
            }
        }
    });
}

function getNewNeeds() {
    $.post('/User/GetNewNeeds', { type: "1,2", pageIndex: 1, pageSize: 10 }, function (data) {
        var html = "";
        for (var i = 0; i < data.items.length; i++) {
            html += "<li style='cursor:pointer;' data-value='" + data.items[i].AutoID + "'><a href='/RFriend/HireDetail/"+data.items[i].AutoID+"' >&nbsp;&nbsp;" + (i+1) + "、" + data.items[i].Title + "</a></li>";
        }
        $('#needul').html(html);
        $('.myscroll3').myScroll({
            speed: 40, //数值越大，速度越慢
            rowHeight: 38 //li的高度
        });
    });
}

function getuserlike(address, pagesize) {
      $.post('/RFriend/GetUserRecommenCount', {
        sex:-1,
        pageIndex: 1,
        pageSize: pagesize,
        address: address,
        age: '',
        cdesc: 'b.RecommendCount'
    }, function (data) {
        var html = "";
        for (var i = 0; i < data.items.length; i++) {
            var item = data.items[i]; 
            html += ' <li><a  href="/User/UserMsg/' + item.UserID + '"><img src="' + ((item.Avatar != "" && item.Avatar != null) ? item.Avatar : "/modules/images/photo2.jpg") + '" width="70px" height="70px"></a></li>';
        } 
        $('#likeul').html(html); 
    });
}

function feedSave() {
    if ($('#feedtitel').val() == '') {
        $('.feedtips').html('请填写标题');
        return false;
    }
    var item={
        TipedID: $('#tipedid').val(),
        TipedName:$('#tipedname').val(),
        Remark:$('#feedcontent').val(),
        Title:$('#feedtitel').val(),
        Type: $('#feedtype option:selected').val()
    }
    $.post('/Help/SaveFeedBack', { entity: JSON.stringify(item) },
    function(data) {
        if (data.result) {
            alert('提交成功');
        } else {
            alert(data.errorMsg); 
        }
        feedcancle();
    }); 
}

function feedcancle() {
    $('#feedback-dialog').hide();
    $('.feedtips').html('');
    $('#feedtitel').val('');
    $('#tipedid').val('');
    $('#tipedname').val('');
    $('#feedcontent').val(''); 
}
function feedshow() {
    $('#feedback-dialog').show(); 
    $('#tipedid').val($('#userinfoli').data('id'));
    $('#tipedname').val($('#userinfoli').data('name'));
}

function showreply(_this,obj) {
    var xy = _this.position();
    var top = xy.top + 20;
    $('.reply-dialog').hide(); 
    if (obj.attr('id') == 'replydialog') {
        $('#replycontent').val('');
        top = top +13;
    } 
    obj.css({ left: xy.left, top: top });
    obj.show();
}

function SavaReply() {
    if ($('#replycontent').val() == '') { 
        return false;
    }
    var item = {
        GUID: $('#userinfoli').data('id'),
        Content: $('#replycontent').val(), 
        Type: 1
    }
    $.post('/Help/SaveReply', { entity: JSON.stringify(item) },
    function (data) {
        if (data.result) {
            $('#replycontent').val('');
            $('#replydialog').hide();
            alert('提交成功');
        } else {
            alert(data.errorMsg);
            location.href = '/Home/Login';
        } 
    });
};

function SaveZH(item) {
    $.post('/Help/SaveReply', { entity: JSON.stringify(item) },
    function (data) {
        if (data.result) { 
            $('#zhdialog').hide();
            alert('提交成功');
        } else {
            alert(data.errorMsg);
            location.href = '/Home/Login';
        } 
    });
}
 

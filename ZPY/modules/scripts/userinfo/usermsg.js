new PCAS("province3", "city3", "area3");
$(function() {
    $('#focusit').click(function () { focususer(); });
    getUserRate();
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
    });
}

function getUserLink(cname) {
    $.post('/User/GetUserLinkInfo', { cname: cname, seeid: $('#userinfoli').data('id'), seename: $('#userinfoli').data('name') }, function (data) {
        if (data.msgError == "") {
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
            html += "<li style='cursor:pointer;' data-value='" + data.items[i].AutoID + "'>&nbsp;&nbsp;" + (i+1) + "、" + data.items[i].Title + "</li>";
        }
        $('#needul').html(html);
    });
}




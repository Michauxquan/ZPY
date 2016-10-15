$(function () {
    new PCAS("province3", "city3", "area3");
    getUserRecomment();
});

function getUserByType(pageindex) {
    var address = "";
    if ($('.province option:selected').val() != "") {
        address+=$('.province option:selected').val()+","+$('.city option:selected').val()+","+$('.district option:selected').val()
    }
    $.post('/RFriend/GetUserInfoByType', {
        sex: $('#seachtype').data('value'),
        pageIndex: pageindex,
        pageSize: 12,
        address: address,
        age: $('#seachage option:selected').val()
    }, function (data) {
        var html = "";
        for (var i = 0; i < data.items.length; i++) {
            var item = data.items[i];
            html += '<li><a href="/User/UserMsg/' + item.UserID + '" title="点击查看详情"><img src="' + (item.Avatar != "" && item.Avatar != null ? item.Avatar : "/modules/images/photo1.jpg") + '" width="140px" height="180px"></a>' +
                '<div><a href="/User/UserMsg/' + item.UserID + '">' + item.Name + '</a><br/><span>' +(item.Sex == 0 ? "帅哥" : "美女") + '</span>一枚&nbsp;<span>' + item.Age + '</span>岁&nbsp;(状态：<span>出租</span>)' +
                '<p class="desc">' + item.MyService + '</p><p>来自：<span>' + item.Province + item.City + '</span></p><a href="/User/UserMsg/' + item.UserID + '">查看详细>></a> ' +
                '</div><div class="clear"></div></li>';
        }
        $('#rentfriendul').html(html);
        $('#page').paginate({
            count: data.pageCount,
            start: pageindex,
            display: 12,
            border: false,
            text_color: '#79B5E3',
            background_color: 'none',
            text_hover_color: '#2573AF',
            background_hover_color: 'none',
            images: false,
            mouse: 'press',
            onChange: function (page) {
                getUserByType(page);
            }
        });
    });
}

function getUserRecomment() { 
    $.post('/RFriend/GetUserRecommenCount', {
        sex: $('#seachtype').data('value'),
        pageIndex: 1,
        pageSize: 4,
        address: '',
        age: '',
        cdesc: 'b.RecommendCount'
    }, function (data) {
        var html = "";
        for (var i = 0; i < data.items.length; i++) {
            var item = data.items[i];
            html += '<li><a href="/User/UserMsg/' + item.UserID + '" title="点击查看详情"><img src="' + (item.Avatar != "" && item.Avatar != null ? item.Avatar : "/modules/images/photo1.jpg") + '" width="140px" height="180px"></a>' +
                '<div><a href="/User/UserMsg/' + item.UserID + '">' + item.Name + '</a><br/><span>' + (item.Sex == 0 ? "帅哥" : "美女") + '</span>一枚&nbsp;<span>' + item.Age + '</span>岁&nbsp;(状态：<span>出租</span>)' +
                '<p class="desc">' + item.MyService + '</p><p>来自：<span>' + item.Province + item.City + '</span></p><a href="/User/UserMsg/' + item.UserID + '">查看详细>></a> ' +
                '</div><div class="clear"></div></li>';
        }
        $('#rentfriendul').html(html);
        getUserByType(1);
    });
}
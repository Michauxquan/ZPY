var ObjetJS = {}
$(function () {
    new PCAS("province3", "city3", "area3");

    $(".slideInner").slide({
        slideContainer: $('.slideInner a'),
        effect: 'easeOutCirc',
        autoRunTime: 5000,
        slideSpeed: 1000,
        nav: true,
        autoRun: true,
        prevBtn: $('a.prev'),
        nextBtn: $('a.next')
    });

    $('#seacha').click(function() {
        var ruul = "/RFriend/RentFriend?id=" + $('#seachtype option:selected').val() +
        ($('#seachage option:selected').val() != "" ? "&agerange=" + $('#seachage option:selected').val() : "") +
        ($('.province option:selected').val() != "" ? "&address=" + $('.province option:selected').val() + "," + $('.city option:selected').val() : "");
        $(this).attr("href", ruul);
    });
    ObjetJS.GetRboy(1);
    ObjetJS.getUserRecomment();
});
ObjetJS.GetRboy= function(type) {
    $.post('/RFriend/GetUserInfoByType', {
        sex: type,
        pageIndex: 1,
        pageSize: 4,
        address: '',
        age: ''
    }, function(data) {
        var html = "";
        for (var i = 0; i < data.items.length; i++) {
            var item = data.items[i];


            html += '<li><a href="/User/UserMsg/' + item.UserID + '"><img src="'+(item.Avatar != "" && item.Avatar != null ? item.Avatar : "/modules/images/photo1.jpg")+'" width="270px" height="200px"></a>' +
                '<div><a href="/User/UserMsg/' + item.UserID + '">' + item.Name + '</a>&nbsp;(状态：<span>出租</span>)<br/><span>' + (item.Sex == 0 ? "帅哥" : "美女") + '</span>一枚&nbsp;<span>' + item.Age + '</span>岁' +
                '&nbsp;' + item.Province +'&nbsp;'+ item.Jobs+ '<p class="desc" class="desc">' + item.MyService + '</p>' +
                '<a href="/User/UserMsg/' + item.UserID + '">查看详细>></a></div><div class="clear"></div></li>';
        }
        if (type == 1) {
            $('#rGirlUl').html(html);
            ObjetJS.GetRboy(0);
        } else {
            $('#rBoyUl').html(html);
        }
    });
}
ObjetJS.getUserRecomment = function () {
    $.post('/RFriend/GetUserRecommenCount', {
        sex:-1,
        pageIndex: 1,
        pageSize: 4,
        address: '',
        age: '',
        cdesc: 'b.RecommendCount'
    }, function (data) {
        var html = "";
        for (var i = 0; i < data.items.length; i++) {
            var item = data.items[i]; 
            html += ' <li title="点击查看详情" data-value=' + item.UserID + '><a href="/User/UserMsg/' + item.UserID + '">' +
                '<img src="' + ((item.Avatar != "" && item.Avatar != null) ? item.Avatar : "/modules/images/photo.jpg") + '" width="220px" height="280px"></a>' +
                '<p>' + (item.Name != "" ? item.Name : item.LoginName) + '<br/>' + (item.Sex == 1 ? "女" : "男") + '&nbsp;' + item.Age + '岁&nbsp;' + item.Province + '</p></li>';
            
        } 
        $('#recommentul').html(html);
        $(".recommend ul li a:first-child img").mouseover(function() {
            $(this).parent().siblings("p").fadeIn(100);
            $(this).parent().parent().css({ "outline": "1px solid #f15481", "box-shadow": "5px 5px 5px rgba(150,150,150,0.5)" });
        }).mouseout(function() {
            $(this).parent().siblings("p").fadeOut(100);
            $(this).parent().parent().css({ "outline": "none", "box-shadow": "none" });
        });
        ObjetJS.GetNewUser();
        ObjetJS.GetUserAction();
        ObjetJS.GetNeedList();
        ObjetJS.GetNewImg();
    });
}
ObjetJS.GetNewUser= function() { 
    $.post('/RFriend/GetUserInfoByType', {
        sex: -1,
        pageIndex: 1,
        pageSize: 10,
        address: '',
        age: ''
    }, function (data) {
        var html = "";
        for (var i = 0; i < data.items.length; i++) {
            var item = data.items[i];
            html += '<li><p><span>['+item.City+']</span><a href="/User/UserMsg/'+item.UserID+'"><strong>'+item.Name+'</strong></a><span>'+(item.Sex==0?"男":"女")+'</span><span>'+item.Age+'岁</span>出租</p></li>';
        }
        $('#newUserUl').html(html);
        $('.myscroll1').myScroll({
            speed: 40, //数值越大，速度越慢
            rowHeight: 36 //li的高度
        });
    });
}
ObjetJS.GetUserAction=function (){
    $.post('/User/UserActions', { type: "1,2,3", pageIndex: 1, pageSize: 10 }, function (data) {
        if (data.items.length > 0) {
            var html = ''; 
            for (var i = 0; i < data.items.length; i++) {
                var item = data.items[i];
                html += '<li>' + (item.LeveID != "" ? item.LeveID : "")
                    + '<span><a href="/User/UserMsg/' + item.UserID + '">' + item.UserName + '</a></span>'
                    + (item.Type == 1 ? "发表日志" : item.Type == 2 ? "查看" : item.Type == 3 ? "浏览" : "向") +
                    '<span><a href="/User/UserMsg/' + item.UserID + '">' + item.SeeName + '</a></span>'
                    + item.Remark + '<small><span>' + getdiff(item.CreateTime, true)
                    + '</span>前</small></li>';
            }
            $('#userActionUl').html(html); 
        }
    });
}
ObjetJS.GetNeedList = function () {
    $.post('/RFriend/GetUserNeedsList', { type: "1,2", pageIndex: 1, pageSize: 10 ,sex:-1}, function (data) {
        if (data.items.length > 0) {
            var html = '';
            for (var i = 0; i < data.items.length; i++) {
                var item = data.items[i];
                html += ' <li><span><a href="/User/UserMsg/'+item.UserID+'">' + item.UserName+ '</a></span>[' + (item.Type == 1 ? "求租" : "出租") + ']<span><a href="/RFriend/HireDetail'+item.AutoID+'">' + item.Title + '</a></span></li>'
            }
            $('#userNeesdul').html(html);
            $('.myscroll').myScroll({
                speed: 40, //数值越大，速度越慢
                rowHeight: 42 //li的高度
            });
        }
    });
}
ObjetJS.GetNewImg = function () {
    $.post('/RFriend/GetNewImg', {}, function (data) {
        if (data.items.length > 0) {
            var html = '';
            for (var i = 0; i < data.items.length; i++) {
                var item = data.items[i];
                html += ' <li><a style="width:216px;height:216px;"  href="/RFriend/UserPic/' + item.UserID + '"><img style="width:216px;height:216px;" src="' + item.ImgUrl + '"></a></li>';
            }
            $('#newImgUl').html(html);
            $('.myscroll2').myScroll({
                speed: 20, //数值越大，速度越慢
                rowHeight: 236//li的高度
            });
        }
    });
}
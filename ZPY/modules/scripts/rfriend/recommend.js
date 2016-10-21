
$(function () {
    new PCAS("province3", "city3", "area3");
    getUserRecomment('', 1, 1,12, $('#seachtype').data('value'));
    getNewNeeds();
    GetAddvert();
});
 

function getUserRecomment(address, type, pageindex,pageSize) {
    $.post('/RFriend/GetUserRecommenCount', {
        sex: -1,
        pageIndex: pageindex,
        pageSize: pageSize,
        address: address,
        age: '',
        cdesc: 'b.RecommendCount'
    }, function (data) {
        var html = "";
        for (var i = 0; i < data.items.length; i++) {
            var item = data.items[i];
            if (type == 1) {

                html += '<li><a href="/User/UserMsg/' + item.UserID + '" title="点击查看详情"><img src="' + (item.Avatar != "" && item.Avatar != null ? item.Avatar : "/modules/images/photo1.jpg") + '" width="140px" height="180px"></a>' +
              '<div><a href="/User/UserMsg/' + item.UserID + '">' + item.Name + '</a><br/><span>' + (item.Sex == 0 ? "帅哥" : "美女") + '</span>一枚&nbsp;<span>' + item.Age + '</span>岁&nbsp;(状态：<span>出租</span>)' +
              '<p class="desc">' + item.MyService + '</p><p>来自：<span>' + item.Province + item.City + '</span></p><a href="/User/UserMsg/' + item.UserID + '">查看详细>></a> ' +
              '</div><div class="clear"></div></li>';
                //html += ' <li title="点击查看详情" data-value=' + item.UserID + '><a href="/User/UserMsg/' + item.UserID + '">' +
                //    '<img src="' + ((item.Avatar != "" && item.Avatar != null) ? item.Avatar : "/modules/images/photo.jpg") + '" width="220px" height="280px"></a>' +
                //    '<p>' + (item.Name != "" ? item.Name : item.LoginName) + '<br/>' + (item.Sex == 1 ? "女" : "男") + '&nbsp;' + item.Age + '岁&nbsp;' + item.Province + '</p></li>';
            } else {
                html += ' <li><a  href="/User/UserMsg/' + item.UserID + '"><img src="' + ((item.Avatar != "" && item.Avatar != null) ? item.Avatar : "/modules/images/photo2.jpg") + '" width="70px" height="70px"></a></li>';
            }
        }
        if (type == 1) {
            $('#recommentul').html(html);
            $("#recommentul ul li a:first-child img").mouseover(function () {
                $(this).parent().siblings("p").fadeIn(100);
                $(this).parent().parent().css({ "outline": "1px solid #f15481", "box-shadow": "5px 5px 5px rgba(150,150,150,0.5)" });
            }).mouseout(function () {
                $(this).parent().siblings("p").fadeOut(100);
                $(this).parent().parent().css({ "outline": "none", "box-shadow": "none" })
            });
            $('#page').html('');
            if (data.pageCount > 0) {
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
                        getUserRecomment(address, type, page, pageSize);
                    }
                });
            }
            getUserRecomment($('#UserCity').val(),-1, 1, 9);
        } else {
            $('#likeul').html(html);
        }
    });
}


function getNewNeeds() {
    $.post('/User/GetNewNeeds', { type: "1,2", pageIndex: 1, pageSize: 10 }, function (data) {
        var html = "";
        for (var i = 0; i < data.items.length; i++) {
            html += "<li style='cursor:pointer;' data-value='" + data.items[i].AutoID + "'>&nbsp;&nbsp;" + (i + 1) + "、" + data.items[i].Title + "</li>";
        }
        $('#needul').html(html);
        $('.myscroll3').myScroll({
            speed: 40, //数值越大，速度越慢
            rowHeight: 38 //li的高度
        });
    });
}
function GetAddvert() {
    $.post('/Home/GetAdvertList',
        {
            imgtype: "",
            view: $('#pagecontroller').val() + '/' + $('#pageaction').val()
        }, function (data) {
            var bright = "";
            for (var i = 0; i < data.items.length; i++) {
                var item = data.items[i]; if (item.ImgType == "BottomRight") {
                    bright += '<a href="' + (item.LinkUrl != "" ? item.LinkUrl : 'javascript:void(0);') + '" title="' + item.Content + '"><img style="width:220px;height:128px;" src="' + data.BaseUrl + item.ImgUrl + '" alt="' + item.Content + '" ></a>';
                }
            }
            $('.ad').html(bright);
        });
}
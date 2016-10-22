
var udedit = UE.getEditor('editor');
var regprice = /(^[1-9]*[1-9][0-9]*$)|(^1?\d\.\d$)|(^2[0-3]\.\d$)/;
var regday = /^[1-9]*[1-9][0-9]*$/;
$(function() {
    new PCAS("province3", "city3", "area3");
    new PCAS("needsprovice", "needscity","");
    new PCAS("saleprovice", "salecity", "");
    var uploader;     
    /*绑定事件 begin*/
    $('.addbtn').click(function() {
        udedit.setHeight(280);
        $('.adddiary').removeClass('hide');
        $('.listdiary').hide();
    });
    $('.backbtn').click(function() {
        $('.adddiary').addClass('hide');
        udedit.setEnabled();
        udedit.setContent('', false);
        $('#diarytitle').val('');
        $('#editor').next().show();
        $('.listdiary').show();
        getUserDiary(1);
    });
    $('.saleservice').click(function() {
        if (typeof ($(this).attr('checked')) != 'undefined') {
            $(this).removeAttr('checked');
        } else {
            $(this).attr('checked', 'checked');
        }
    });
    $('.myservice').click(function () {
        if (typeof ($(this).attr('checked')) != 'undefined') {
            $(this).removeAttr('checked');
        } else {
            $(this).attr('checked', 'checked');
        }
    });
    $(".content .sidebar ul li").each(function () {
        var _this = $(this);
        _this.click(function () {
            _this.css({ background: "#36B0F3", color: "white" }).siblings().css({ background: "white", color: "black" });
            $('.navcontent').hide();
            $('.' + _this.data('value')).show();
            switch (_this.data('value')) {
                case "user-msg":
                    //getUserActions('1,2,3');
                    break;
                case "whocame":
                    if (!_this.data('frist')) {
                        getUserActions('2', 1, 10);
                    }
                    break;
                case "myfocus":
                    if (!_this.data('frist')) {
                        getUserMyFocus(1);
                    }
                    break;
                case "userreply":
                    getReplyList(1, 1);
                    break;
                case "picset":
                    uploader = creatUpload({
                        id: 'as', 
                        header:'选择头像',  
                        sucess: function (file, response) {
                            if (response.msgError == "") {
                                $('.useravator').attr("src", response.imgUrl);
                            } else {
                                alert(response.msgError);
                            }
                        },
                        UploadUrl: '/User/UserAvatarUpload',
                        Upheader: uploader
                    });
                    break;
                case "upload":
                    uploader = creatUpload({
                        id: 'test', 
                        fileList: 'mulitList',
                        filePicker: 'mulitPicker',
                        ctlBtn: 'mulitBtn', 
                        UploadUrl: '/User/UserImgUpload',
                        Upheader: uploader
                    });
                    break;
                case "change-msg":
                    getUserMyInfo();
                    break;
                case "release-diary":
                    getUserDiary(1);
                    break;
                case "business":
                    if (!_this.data('frist')) {
                        getNeedsList(1, 'hirelist');
                    }
                    break;
            }
            _this.data('frist', 'true');
        });
    });
    $(".business>ul li").each(function () {
        var _this = $(this);
        _this.click(function () {
            _this.addClass("cur").siblings().removeClass("cur");
            $('.navbusiness').hide(); 
            $('.' + _this.data('value')).show();
            if (!_this.data('frist')) {
                getNeedsList(1, _this.data('value'));
            }
            _this.data('frist', 'true');
        });
    });
    $(".userreply>ul li").each(function () {
        var _this = $(this);
        _this.click(function () {
            if (!_this.hasClass('cur')) {
                _this.addClass("cur").siblings().removeClass("cur");
                $('#replylistthead').html('');
                getReplyList(1, _this.data('value'));
            }  
        });
    });

    $('#seacha').click(function () {
        var ruul = "/RFriend/RentFriend?id=" + $('#seachtype option:selected').val() +
        ($('#seachage option:selected').val() != "" ? "&agerange=" + $('#seachage option:selected').val() : "") +
        ($('.province option:selected').val() != "" ? "&address=" + $('.province option:selected').val() + "," + $('.city option:selected').val() : "");
        $(this).attr("href", ruul);
    });
    /*绑定事件结束     */
    getUserMyInfo();
    getUserActions('1,2,3', 1, 10);
    getUserReport();
});
function getUserReport() {
    $.post('/User/GetUserReport', { userid: $('.spuserid').data('value') }, function (data) {
        if (data.item != null) {
            $('.seecount').html(data.item.SeeCount);
        }
    });
}

function getUserActions(type,pageindex,pagesize) {
    $.post('/User/UserActions', { type: type, pageIndex: pageindex, pageSize: pagesize }, function (data) {
        if (data.items.length > 0) {
            var html = '';
            if (type == '2') {
                for (var i = 0; i < data.items.length; i++) {
                    var item = data.items[i];
                    html += '<li data-value="' + item.SeeID + '"><a href="/User/UserMsg/' + item.SeeID + '"><img src="' + (item.SeeAvatar != null && item.SeeAvatar != "" ? item.SeeAvatar : "/modules/images/photo4.jpg") + '" width="61" height="73"><br/>' +
                        '<span>' + item.SeeName + '</span><br/><i>' + getdiff(item.CreateTime, true) + '</i>前</a></li>';
                } 
                $('#whocameul').html(html); 
                $('#page1').html('');
                if (data.pageCount > 0) {
                    $('#page1').paginate({
                        count: data.pageCount,
                        start: 1,
                        display: pagesize,
                        border: false,
                        text_color: '#79B5E3',
                        background_color: 'none',
                        text_hover_color: '#2573AF',
                        background_hover_color: 'none',
                        images: false,
                        mouse: 'press',
                        onChange: function(page) {
                            getUserActions(type, page, pagesize);
                        }
                    });
                }
            } else {
                for (var i = 0; i < data.items.length; i++) {
                    var item = data.items[i];
                    html += '<li>' + (item.LeveID != "" ? item.LeveID : "")
                        + '<span><a href="/User/UserMsg/' + item.UserID + '">' + item.UserName + '</a></span>'
                        + (item.Type == 1 ? "发表日志" : item.Type == 2 ? "查看" : item.Type == 3 ? "浏览" : "向") +
                        '<span><a href="/User/UserMsg/' + item.UserID + '">' + item.SeeName + '</a></span>'
                        + item.Remark + '<small><span>' + getdiff(item.CreateTime, true)
                        + '</span>前</small></li>';
                }
                $('#actionul').html(html);
            }
            
        }
    }); 
}

function getUserMyFocus(pageindex) {
    $.post('UserMyFocus', { pageIndex: pageindex, pageSize: 10 }, function (data) {
        if (data.items.length > 0) {
            var html = '';
            for (var i = 0; i < data.items.length; i++) {
                var item = data.items[i];
                html += '<li data-value="' + item.SeeID + '"><a href="/User/UserMsg/' + item.FocusID + '"><img src="' + (item.FocusAvatar != null && item.FocusAvatar != "" ? item.FocusAvatar : "/modules/images/photo4.jpg") + '" width="61" height="73"><br/>' +
                    '<span>' + item.FocusName + '</span></a></li>';
            }
            $('#myfocusul').html(html);
            $('#page2').html('');
            if (data.pageCount > 0) {
                $('#page2').paginate({
                    count: data.pageCount,
                    start: 1,
                    display: 10,
                    border: false,
                    text_color: '#79B5E3',
                    background_color: 'none',
                    text_hover_color: '#2573AF',
                    background_hover_color: 'none',
                    images: false,
                    mouse: 'press',
                    onChange: function(page) {
                        getUserActions(type, page, pagesize);
                    }
                });
            }
        }
    });
}

function getUserDiary(pageindex) {
    $.post('UserDiaryList', { type: 0, pageIndex: pageindex, pageSize: 10 }, function (data) {
        if (data.items.length > 0) {
            var html = '';
            for (var i = 0; i < data.items.length; i++) {
                var item = data.items[i];
                html += '<li ><a data-value="' + item.AutoID + '" class="title" style="line-height:24px;cursor:pointer;color:#36B0F3;";>' + (item.Title.length > 18 ? item.Title.substring(0, 18) : item.Title) + '</a><span style="margin-left:30px;">' + convertdate(item.CreateTime, true) + '</span></li>';
            }
            $('#mydiary').html(html);
            $('#mydiary li a').click(function () {
                getUserDiaryDetail($(this).data('value'),'adddiary');
            });
            $('#pagediary').html('');
            if (data.pageCount > 0) {
                $('#pagediary').paginate({
                    count: data.pageCount,
                    start: 1,
                    display: 10,
                    border: false,
                    text_color: '#79B5E3',
                    background_color: 'none',
                    text_hover_color: '#2573AF',
                    background_hover_color: 'none',
                    images: false,
                    mouse: 'press',
                    onChange: function(page) {
                        getUserDiary(page);
                    }
                });
            }
        }
    });
}

function getNeedsList(pageindex, type) { 
    $.post('/User/NeedsList', { type: type == "lease" ? 1 : 2, ismyself: type == "hirelist" ? false : true, pageIndex: pageindex, pageSize: 10 }, function (data) {
        if (data.items.length > 0) {
            var html = '';
            for (var i = 0; i < data.items.length; i++) {
                var item = data.items[i];
                html += '<tr><td>' + item.Province + '</td><td>' + (item.NeedSex == 0 ? "男" : "女") + '</td><td>' + item.UserName +
                    '</td><td><a style="color:#36B0F3;cursor:pointer;" data-value="'+item.AutoID+'">' + item.Title + '</a></td><td>' + (item.Type == 1 ? "求租" : "出租") +
                    '</td><td>' + convertdate(item.CreateTime, true) + '</td></tr>';
            } 
            $('#'+type+'thead').html(html);
            $('#'+type+'thead tr a').click(function() {
                getUserDiaryDetail($(this).data('value'), type);
            });
            $('#' + type + 'page').html('');
            if (data.pageCount > 0) {
                $('#' + type + 'page').paginate({
                    count: data.pageCount,
                    start: 1,
                    display: 10,
                    border: false,
                    text_color: '#79B5E3',
                    background_color: 'none',
                    text_hover_color: '#2573AF',
                    background_hover_color: 'none',
                    images: false,
                    mouse: 'press',
                    onChange: function(page) {
                        getNeedsList(page, type);
                    }
                });
            }
        }
    });
}

function getUserDiaryDetail(autoid,classname) {
    $.post('NeedsDetail', {autoid:autoid}, function(data) {
        if (data != null) {
            var content = data.item.Content;
            content = content.replace(/&lt/g, '<').replace(/&gt/g, '>').replace(/<br>/g, '\n');
            if (classname == 'adddiary') {
                udedit.setContent(content, false);
                $('#diarytitle').val(data.item.Title);
                udedit.setDisabled('fullscreen');
                $('#editor').next().hide();
                $('.adddiary').removeClass('hide');
                $('.listdiary').hide();
            } else {
                $('.' + classname).hide();
                $('.hiredetail').show();    
                $('#msgusername').html(data.item.UserName);
                $('#msgtitle').html(data.item.Title);
                $('#msgsex').html(data.item.NeedSex==1?"女":"男");
                var pyaprice = data.item.NeedType == 7 ? "待议" : (
                    data.item.Price + "/" + (data.item.NeedType == 1 ? "年" : data.item.Price == 2 ? "季度" : data.item.Price == 3 ? "月" : data.item.Price == 4 ? "周" : data.item.Price == 5 ? "天" : "小时")
                    );
                $('#msgprice').html(pyaprice);
                $('#msgservice').html(data.item.ServiceConten);
                $('#msgcontent').html(content);
                $('#msgtime').html();
                $('.hireback').unbind('click').bind('click', function () {
                    $('.hiredetail').hide();
                    $('.' + classname).show();
                });
            }
        }
    });
}

function getUserMyInfo() {
    $.post('UserMyInfo', null, function (data) {
        if (data.item != null) { 
            $('.useravator').attr("src", (data.item.Avatar != null && data.item.Avatar != "") ? data.item.Avatar : '/modules/images/head.png');
            $('#BHeight').val(data.item.BHeight);
            $('#BWeight').val(data.item.BWeight); 
            $('#Jobs').val(data.item.Jobs);
            $('#BPay').val(data.item.BPay);
            $('#userAge').val(data.item.Age);
            $('#userTalkTo').val(data.item.TalkTo);
            $('#IsMarry').val(data.item.IsMarry);
            $('#MyContent').val(data.item.MyContent);
            if (data.item.MyService != "" && data.item.MyService != null) {
                $('.myservice').each(function(i, v) {
                    if (data.item.MyService.indexOf($(v).val()) > -1) {
                        $(v).attr('checked', 'checked');
                    }
                });
            }
        }
    });
}
function saveUserInfo() {
    var myservic = '';
    $('.myservice').each(function(i, v) {
        if ($(v).attr('checked') == 'checked') {
            myservic += $(v).val() + ',';
        }
    }); 
$.post('SaveUserInfo', {
    bHeight:$('#BHeight option:selected').val(),
    bWeight:$('#BWeight option:selected').val(),
    jobs:$('#Jobs option:selected').val(),
    bPay: $('#BPay option:selected').val(),
    name: $('#userTrueName').val(),
    age: $('#userAge').val(),
    talkTo: $('#userTalkTo').val(),
    myservice:myservic,
    isMarry:$('#IsMarry option:selected').val(),
    myContent:$('#MyContent option:selected').val()
    }, function (data) {
        if (data.result) {
            alert('个人资料更新成功');
        }
    },"json"); 
}
function saveDiary() {
    if ($('#diarytitle').val() != '' && udedit.hasContents() && $('#diarytitle').val().length <= 20) {
        $('.diarywarring').hide();
        var content = udedit.getContent();
        content = content.replace("\n", "<br>").replace(/</g, "&lt").replace(/>/g, "&gt"); 
        var item = { title: $('#diarytitle').val(), type: 0, content: content,needsex:0,needType:0,serviceConten:'',price:0.00,needCity:'',needDate:jeDate.now(0) }
        savaUserNeeds(item);
    } else {
        $('.diarywarring').show();
    }
}
function saveSaleInfo() {
    var servicecontent = '';
    $('.saleservice').each(function(i, v) {
        if ($(v).attr('checked') == 'checked') {
            servicecontent += $(v).val() + ',';
        }
    });
    var price = $('#saleprice').val();
    if (price == '' && $('#salePriceType option:selected').val() == 7) {
        price = 0;
    }
    if (!regprice.test(price)) {
        alert('金额输入不正确');
        return false;
    }
    if ($('#saledate').val()=="") {
        alert('出租日期输入不正确');
        return false;
    }
    if ($('#saletitle').val() != '' && $('#saletitle').val().length <= 20 && $('#servicecontent').val() != '' && $('#salecontent').val() != "" && $('#saleprice').val() != '') {
        var item = {
            title: $('#saletitle').val(),
            type: 2,
            content: $('#salecontent').val(),
            needSex: $('#salesex option:selected').val(),
            needType: $('#salePriceType option:selected').val(),
            serviceConten: servicecontent,
            needCity:$('.saleprovice option:selected').val()==""?"":($('.saleprovice option:selected').val()+","+$('.salecity option:selected').val()),
            needDate:$('#saledate').val(),
            price: price
        }
        savaUserNeeds(item);
    }  
}
/*求租信息保存*/
function saveNeedsInfo() {
    var price = $('#needsprice').val(); 
    if (price == '' && $('#needPriceType option:selected').val()==7) {
        price = 0;
    }
    if (!regprice.test(price)) {
        alert('金额输入不正确！');
        return false;
    }
    if(!regday.test($('#needsdays').val()))
    {
        alert('招聘天数输入不正确！');
        return false;
    }
    if ($('#needsdate').val() == "") {
        alert('求租日期输入不正确');
        return false;
    }
    if (typeof ($('#needscity option:selected').val()) == 'undefined') {
        alert('目的城市未选择！');
        return false;
    }
    if ($('#needtitle').val() != '' && $('#needtitle').val().length <= 20 && $('#needscontent').val() != '' ) {
        var item = {
            title: $('#needtitle').val(),
            type: 1,
            content: $('#needscontent').val(),
            needSex: $('#needssex option:selected').val(),
            needType: $('#needPriceType option:selected').val(),
            serviceConten:'',
            needCity:$('.needsprovice option:selected').val()==""?"":($('.needsprovice option:selected').val()+","+$('.needscity option:selected').val()),
            needDate:$('#needsdate').val(),
            letDays: $('#needsdays').val(),
            price: price
        }
        console.log(item);
        //savaUserNeeds(item);
    }
}
function savaUserNeeds(entity) {
    $.post('/User/SaveNeeds', { entity: JSON.stringify(entity) }, function (data) { 
        if (data.result) {
            alert('发布成功');
            if (entity.type == 0) { 
                $('.adddiary').addClass('hide');
                udedit.setContent('', false);
                getUserDiary(1);
                $('.listdiary').show();

            } else if (entity.type == 2) { 
                $('.saleservice').each(function (i, v) {
                    if ($(v).attr('checked') == 'checked') {
                        $(v).removeAttr('checked');
                    }
                });
                $('#saleprice').val('');
                $('#saletitle').val('');
                $('#salecontent').val('');
                $('.saleprovice').val('');
                $('.salecity').val('');
            } else {
                $('#needtitle').val('');
                $('#needsdays').val('');
                $('#needsprice').val('');
                $('.needsprovice').val('');
                $('.needscity').val('');
                $('#needscontent').val(''); 
            }
        }
    }, "json");
}


function getReplyList(pageindex, type) {
    $.post('/Help/ReplyList', { type:type,pageIndex: pageindex, pageSize: 10 }, function (data) {
        if (data.items.length > 0) {
            var html = '';
            for (var i = 0; i < data.items.length; i++) {
                var item = data.items[i];
                html += '<tr><td>' + item.UserName + '</td><td>' + item.Content+ '</td><td>' + convertdate(item.CreateTime, true) + '</td>' +
                    '<td><a data-id="' + item.ReplyID + '" style="cursor:pointer;">删除</a><a data-id="' + item.ReplyID + '" style="cursor:pointer;display:' + ($('.cur').data('value') == 2 ? "block;" : "none;") + '">回复</a></td></tr>';
            }
            $('#replylistthead').html(html);
            $('#replylistthead tr a').click(function () {
                alert(1);
            });
            $('#replylistpage').html(''); 
            $('#replylistpage').paginate({
                count: data.pageCount,
                start: 1,
                display: 10,
                border: false,
                text_color: '#79B5E3',
                background_color: 'none',
                text_hover_color: '#2573AF',
                background_hover_color: 'none',
                images: false,
                mouse: 'press',
                onChange: function (page) {
                    getReplyList(page, type);
                }
            }); 
        }
    });
}

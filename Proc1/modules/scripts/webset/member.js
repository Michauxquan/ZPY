define(function (require, exports, module) {
    var Global = require("global"), 
        Upload = require("upload");   
    var ObjectJS = {};
    var reg = /^[0-9]*[1-9][0-9]*$/; 
    var reg2 = /^(((\d[0]\.\d+))|\+?0\.\d*|\+?1)$/;
    var reg3 = /^\d+(\.\d+)?$/;
    ObjectJS.init = function () {
        var _self = this;
        _self.bindEvent(); 
    }
    ObjectJS.bindEvent = function () {
        var _self = this;  
        $('#tipInfo').mousemove(function() {
            var _this = $(this);
            var position = _this.position();
            $("#wareInfo").css({ "top": position.top-45, "left": position.left + 30 }).show().mouseleave(function() {
                $(this).hide();
            });
        }).mouseout(function() {
            $("#wareInfo").hide(); 
        });
        /*积分等级保存*/
        $('#saveMemberLevel').click(function () {
            _self.saveMemberLevel();
        });
        /*积分等级新建*/
        $('#createMemberLevel').click(function () {
            _self.createMemberLevel();
        });
        _self.getMemberLevelList();
    }  
    //*客户会员等级列表*/
    ObjectJS.getMemberLevelList = function () {
        $(".memberlevelul").html('');
        $(".memberlevelul").html("<h1><div class='data-loading' ><div></h1>");
        Global.post("/WebSet/GetMemberLevel", {type:0}, function (data) {
            $(".memberlevelul").html('');
            var items = data.items;
            if (items.length > 0) {
                var innnerHtml = '';
                for (var i = 0; i < items.length; i++) {
                    if (i == 0) {
                        innnerHtml += "<li id='memberLi" + i + "' class='lineHeight30'><div class='levelitem left' data-origin='" + (items[i].Origin - 1) + "' data-imgurl='" + items[i].ImgUrl + "' data-golds='" + items[i].Golds + "'  data-integfeemore='" + items[i].IntegFeeMore + "' data-name='" + items[i].Name + "' data-discountfee='" + items[i].DiscountFee + "' data-id='" + items[i].LevelID + "' >" +
                            "<div class='left'><span  class='spanimg mTop5' ><img name='MemberImg' id='MemberImg" + i + "' style='display:inline-block;' class='memberimg' title='点击替换等级图标 '  src='" + (items[i].ImgUrl != '' ? items[i].ImgUrl : '/Content/menuico/custom.png') + "' alt=''></span></div><span class='hide' id='SpanImg" + i + "'></span>" +
                            "<span  class='mLeft5 mRight5' style='display:inline-block;'>客户支付</span>" +
                            "<input id='IntegFeeMore" + i + "' name='IntegFeeMore'  class='width50 mRight5' type='text' value='" + items[i].IntegFeeMore + "' />" +
                            "<span class='mRight5'>元，折扣价</span>" +
                            "<input id='changeFeeMore" + i + "' title='默认为0则无折扣' name='DiscountFee' class='width50 mRight5' type='text'  value='" + items[i].DiscountFee + "' />" +
                            "<span class='mRight5'>元，购买</span>" +
                            "<input class='width80 mRight5' type='text' name='MemberName' placeholder='请填写等级名' value='" + items[i].Name + "' />" +
                            "<span class='mRight5'>会员 </span>" +
                            "<input name='Golds' class='width50 mRight5' title='默认单位为天' type='text' value='" + (i == items.length - 1 ? '无上限' : items[i].Golds) + "' />" +
                            "<span  class='mRight5'>天</span>" +
                            "<span id='delMeber" + i + "' data-ind='" + i + "' class='" + (i == 0 ? "hide" : i == items.length - 1 ? "" : "hide") + " borderccc circle12 mLeft10'>X</span>" +
                            "</div></li>";
                    } else {
                        innnerHtml += "<li id='memberLi" + i + "' class='lineHeight30'><div class='levelitem left' data-origin='" + (items[i].Origin - 1) + "' data-imgurl='" + items[i].ImgUrl + "' data-golds='" + items[i].Golds + "' data-integfeemore='" + items[i].IntegFeeMore + "' data-name='" + items[i].Name + "' data-discountfee='" + items[i].DiscountFee + "' data-id='" + items[i].LevelID + "' >" +
                            "<div class='left'><span  class='spanimg mTop5' ><img name='MemberImg' id='MemberImg" + i + "' style='display:inline-block;' class='memberimg' title='点击替换等级图标 '  src='" + (items[i].ImgUrl != '' ? items[i].ImgUrl : '/Content/menuico/custom.png') + "' alt=''></span></div><span class='hide' id='SpanImg" + i + "'></span>" +
                            "<span  class='mLeft5 mRight5' style='display:inline-block;'>客户支付</span>" +
                            "<input id='IntegFeeMore" + i + "' name='IntegFeeMore'  class='width50 mRight5' type='text' value='" + items[i].IntegFeeMore + "' />" +
                            "<span class='mRight5'>元，折扣价</span>" +
                             "<input id='changeFeeMore" + i + "' title='默认为0则无折扣' name='DiscountFee' placeholder='折扣价,默认为0则无折扣' class='width50 mRight5' type='text'  value='" + items[i].DiscountFee + "' />" +
                             "<span class='mRight5'>元，购买</span>" +
                             "<input class='width80 mRight5' type='text' name='MemberName' placeholder='请填写等级名' value='" + items[i].Name + "' />" +
                             "<span class='mRight5'>会员 </span>" +
                             "<input name='Golds' class='width50 mRight5' title='默认单位为天' type='text' value='" + (i == items.length - 1 ? '无上限' : items[i].Golds) + "' />" +
                              "<span  class='mRight5'>天</span>" +
                            "<span id='delMeber" + i + "' data-ind='" + i + "' class='" + (i == 0 ? "hide" : i == items.length - 1 ? "" : "hide") + " borderccc circle12 mLeft10'>X</span>" +
                            "</div></li>";
                    }
                }
                $(".memberlevelul").html(innnerHtml);
                ObjectJS.bindMemberLi();
            } else {
                $(".memberlevelul").html("<h1><div class='nodata-txt' >暂无数据!<div></h1>");
            }
        });
    }
    /*客户会员等级弹窗*/
    ObjectJS.createMemberLevel = function () {
        var _self = this;
        var i = $('.levelitem').length;
        
        var intefee = 200;
        if (i > 0) {
            console.log($('#memberLi' + (i - 1) + ' div:first-child').html());
            var intefee = parseInt($('#memberLi' + (i - 1) + ' div:first-child').data('integfeemore')) + 300;
            $('#changeFeeMore' + (i - 1)).val($('#IntegFeeMore' + (i - 1)).val());
        } 
        var innnerHtml = "<li id='memberLi" + i + "' class='lineHeight30'><div class='levelitem left' data-origin='" + i + "' data-imgurl='' data-golds='0.00'  data-integfeemore='" + intefee + "' data-name='' data-discountfee='1.00' data-id='' title='' >" +
                      "<div class='left'><span  class='spanimg mTop5' ><span class='hide' id='SpanImg" + i + "'></span><img name='MemberImg' style='display:inline-block;' id='MemberImg" + i + "' class='memberimg'   src='/Content/menuico/custom.png' alt=''></span></div>" +
                      "<span  class='mLeft5 mRight5'>客户支付</span>" +
                      "<input id='IntegFeeMore" + i + "' name='IntegFeeMore' class='width50 mRight5' type='text' value='" + intefee + "' />" +
                      "<span class='mRight5'>元，折扣价</span>" +
                      "<input id='changeFeeMore" + i + "' name='DiscountFee' class='width50 mRight5' placeholder='折扣价,默认为0则无折扣'   type='text' value='" + intefee + "' />" +
                      "<span class='mRight5'>元，购买</span>" +
                      "<input class='width80 mRight5' name='MemberName' type='text'  placeholder='请填写等级名' value='' />" +
                      "<span class='mRight5'>会员 </span>" +
                      "<input name='Golds'  class='width50 mRight5' placeholder='请填写有效天数'  type='text' value='0' />" +
                      "<span  class='mRight5'>天</span>" +
                      "<span id='delMeber" + i + "' data-ind='" + i + "' style='cursor:pointer;' class=' borderccc circle12 mLeft10'>X</span>" +
                      "</div></li>";
        if ($(".memberlevelul li:last-child").length > 0) {
            $(".memberlevelul li:last-child").after(innnerHtml);
        } else {
            $(".memberlevelul").html(innnerHtml);
        }
        $('#delMeber' + (i - 1)).hide();
        _self.bindMemberLi();
    }

    ObjectJS.saveMemberLevel = function () {
        var list = [];
        var _self = this;
        var gonext = true;
        $('.levelitem').each(function (i, v) {
            if ($(v).data('origin') != '-1') {
                if ($(v).data('name') == '') {
                    gonext = false;
                }
                var item = {};
                item.IntegFeeMore = $(v).data('integfeemore');
                item.Golds = $(v).data('golds');
                item.DiscountFee = $(v).data('discountfee');
                item.Name = $(v).data('name').trim();
                item.ImgUrl = $(v).data('imgurl');
                item.Origin = parseInt($(v).data('origin')) + 1;
                item.LevelID = $(v).data('id');
                item.Type = 0;
                list.push(item);
            }
        });
        if (gonext) {
            Global.post("/WebSet/SaveMemberLevel", { memberlevel: JSON.stringify(list) }, function (data) {
                if (data.result == "") {
                    alert('等级配置成功');
                    _self.getMemberLevelList();
                } else {
                    alert(data.result);
                }
            });
        } else {
            alert('客户等级不能为空，请修改后再保存');
        }
    }

    ObjectJS.hideMember = function (ind) {
        $('#memberLi' + ind).remove();
        if (ind > 1) {
            $('#delMeber' + (ind - 1)).show();
        }
    }

    ObjectJS.bindMemberLi = function () {
        $(".circle12").click(function () { ObjectJS.hideMember($(this).parent().data("origin")); });
        $("input[name^='IntegFeeMore']").change(function () {
            ObjectJS.changeInput(1, $(this));
        });
        $("input[name^='DiscountFee']").change(function () {
            ObjectJS.changeInput(2, $(this));
        });
        $("input[name^='MemberName']").change(function () {
            ObjectJS.changeInput(3, $(this));
        });

        $("img[name^='MemberImg']").unbind('click').click(function () {
            var _this = $(this);
            var elem = "#SpanImg" + _this[0].id.replace('MemberImg', '');
            $(elem).html('');
            Upload.createUpload({
                element: elem,
                buttonText: "",
                className: "",
                data: { folder: '', action: 'add', oldPath: '' },
                success: function (data, status) {
                    if (data.Items.length > 0) {
                        _this.attr("src", data.Items[0]); 
                        _this.parent().parent().parent().data('imgurl', data.Items[0]);
                    } else {
                        alert("只能上传jpg/png/gif类型的图片，且大小不能超过1M！");
                    }
                }
            });
            $(elem + '_buttonSubmit').click();
        });
    }

    ObjectJS.changeInput = function (type, _this) {
        var s = parseInt(_this.parent().data("origin")) - 1;
        if (type == 1) {
            if (reg.test(_this.val())) {
                if (s != $('.levelitem').length) {
                    if (parseInt($('#IntegFeeMore' + (s + 2)).val()) <= parseInt(_this.val())) {
                        alert('当前充值金额不能大于等于下一等级充值金额');
                        _this.val(_this.parent().data('integfeemore'));
                    } if (parseInt($('#IntegFeeMore' + s).val()) >= parseInt(_this.val())) {
                        alert('当前充值金额不能小于等于上一等级充值金额');
                        _this.val(_this.parent().data('integfeemore'));
                    } else {
                        $('#changeFeeMore' + s).val(_this.val());
                        _this.parent().data('integfeemore', _this.val());
                    }
                } else {
                    _this.parent().data('integfeemore', _this.val());
                }
            } else {
                alert('充值金额格式输入有误，请重新输入');
                _this.val(_this.parent().data('integfeemore'));
            }
        } else if (type == 2) {
            if (!reg.test(_this.val())) {
                alert('金币格式输入有误，请重新输入');
                _this.val(_this.parent().data('discountfee'));
            } else {
                _this.parent().data('discountfee', _this.val());
            }
        } else if (type == 3) {
            if (_this.val() != '') {
                _this.parent().data('name', _this.val());
            }
        }
    } 
    module.exports = ObjectJS;
});
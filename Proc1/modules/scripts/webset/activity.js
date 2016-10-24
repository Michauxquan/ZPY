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

        $('#integerFee').change(function () {
            if (!reg3.test($(this).val())) {
                alert('金币格式输入有误,请重新输入');
                $(this).val($(this).data('oldvalue'));
            } else {
                $('#saveClientRule').show();
            }
        });
        $('#saveClientRule').click(function() {
            _self.saveGoldRule();
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
        Global.post("/WebSet/GetMemberLevel", {type:1}, function (data) {
            $(".memberlevelul").html('');
            var items = data.items;
            if (items.length > 0) {
                var innnerHtml = '';
                for (var i = 0; i < items.length; i++) {
                    if (i == 0) {
                        innnerHtml += "<li id='memberLi" + i + "' class='lineHeight30'>" +
                            "<div class='levelitem left' data-origin='" + (items[i].Origin - 1) + "' data-imgurl='" + items[i].ImgUrl + "' data-golds='" + items[i].Golds + "'  data-integfeemore='" + items[i].IntegFeeMore + "' data-name='" + items[i].Name + "' data-discountfee='" + items[i].DiscountFee + "' data-id='" + items[i].LevelID + "' >" +
                            "<span  class='mLeft5 mRight5' style='display:inline-block;'>客户充值</span>" +
                            "<input id='IntegFeeMore" + i + "' name='IntegFeeMore'  class='width50 mRight5' type='text' value='" + items[i].IntegFeeMore + "' /><span class='mRight5'>元，购买金币</span>" +
                            "<input name='DiscountFee' class='width50 mRight5' type='text'  value='" + items[i].DiscountFee + "' /><span class='mRight5'>赠送</span>" +
                            "<input name='Golds' class='width50 mRight5' type='text' value='" + items[i].Golds + "' />" +
                            "<span  class='mRight5'>金币 &nbsp;</span>" +
                            " <span id='delMeber" + i + "' data-ind='" + i + "' class='" + (i == 0 ? "hide" : i == items.length - 1 ? "" : "hide") + " borderccc circle12 mLeft10'>X</span>" +
                            "</div></li>";
                    } else {
                        innnerHtml += "<li id='memberLi" + i + "' class='lineHeight30'><div class='levelitem left' data-origin='" + (items[i].Origin - 1) + "' data-imgurl='" + items[i].ImgUrl + "' data-golds='" + items[i].Golds + "' data-integfeemore='" + items[i].IntegFeeMore + "' data-name='" + items[i].Name + "' data-discountfee='" + items[i].DiscountFee + "' data-id='" + items[i].LevelID + "' >" +
                            "<span  class='mLeft5 mRight5' style='display:inline-block;'>客户充值</span>" +
                            "<input id='IntegFeeMore" + i + "' name='IntegFeeMore'  class='width50 mRight5' type='text' value='" + items[i].IntegFeeMore + "' /><span class='mRight5'>元，购买金币</span>" +
                            "<input name='DiscountFee' class='width50 mRight5' type='text'   value='" + items[i].DiscountFee + "' /><span class='mRight5'>赠送</span>" +
                            "<input name='Golds' class='width50 mRight5' type='text' value='" + items[i].Golds + "' />" +
                            "<span  class='mRight5'>金币 &nbsp; </span>" +
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
        } 
        var innnerHtml = "<li id='memberLi" + i + "' class='lineHeight30'>" +
                      "<div class='levelitem left' data-origin='" + i + "' data-imgurl='' data-golds='0.00'  data-integfeemore='" + intefee + "' data-name='' data-discountfee='1.00' data-id='' title='' >" +
                      "<span  class='mLeft5 mRight5' style='display:inline-block;'>客户充值</span>" +
                      "<input id='IntegFeeMore" + i + "' name='IntegFeeMore' class='width50 mRight5' type='text' value='" + intefee + "' />" +
                      "<span class='mRight5'>元，购买金币</span>" +
                      "<input name='DiscountFee' class='width50 mRight5' placeholder='赠送金币'   type='text' value='" + intefee + "' />" +
                      "<span class='mRight5'>赠送</span>" +
                      "<input name='Golds'  class='width50 mRight5' placeholder='请填写赠送金币'  type='text' value='0' />" +
                      "<span  class='mRight5'>金币 &nbsp; </span><span id='delMeber" + i + "' data-ind='" + i + "' style='cursor:pointer;' class=' borderccc circle12 mLeft10'>X</span>" +
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
                var item = {};
                item.IntegFeeMore = $(v).data('integfeemore');
                item.Golds = $(v).data('golds');
                item.DiscountFee = $(v).data('discountfee');
                item.Name = $(v).data('name').trim();
                item.ImgUrl = $(v).data('imgurl');
                item.Origin = parseInt($(v).data('origin')) + 1;
                item.LevelID = $(v).data('id');
                item.Type = 1;
                list.push(item);
            }
        }); 
        Global.post("/WebSet/SaveMemberLevel", { memberlevel: JSON.stringify(list) }, function (data) {
            if (data.result == "") {
                alert('优惠设置成功');
                _self.getMemberLevelList();
            } else {
                alert(data.result);
            }
        }); 
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
        $("input[name^='Golds']").change(function () {
            ObjectJS.changeInput(3, $(this));
        });

    }

    ObjectJS.changeInput = function (type, _this) {
        var s = parseInt(_this.parent().data("origin")) - 1; 
        if (type == 1) {
            if (reg.test(_this.val())) {
                if (s != $('.levelitem').length) {
                    if (parseInt($('#IntegFeeMore' + (s + 2)).val()) < parseInt(_this.val())) {
                        alert('当前充值金额不能大于等于下一等级充值金额');
                        _this.val(_this.parent().data('integfeemore'));
                    }else if (parseInt($('#IntegFeeMore' + s).val()) > parseInt(_this.val())) {
                        alert('当前充值金额不能小于等于上一等级充值金额');
                        _this.val(_this.parent().data('integfeemore'));
                    } else { 
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
            if (!reg.test(_this.val())) {
                alert('赠送金币格式输入有误，请重新输入');
                _this.val(_this.parent().data('discountfee'));
            } else {
                _this.parent().data('golds', _this.val());
            } 
        }
    }
    ObjectJS.saveGoldRule = function () {
        Global.post("/WebSet/SaveGoldRule", { integerFee: $('#integerFee').val() }, function (data) {
            if (data.result) {
                alert('金币兑换比例设置成功');
                $('#integerFee').data('oldvalue', $('#integerFee').val());
                $('#saveClientRule').hide();
            } else {
                alert('操作失败');
            }
        });
    }
    module.exports = ObjectJS;
});
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
    }  
    //*客户会员等级列表*/
    ObjectJS.getMemberLevelList = function () {
        $(".memberlevelul").html('');
        $(".memberlevelul").html("<h1><div class='data-loading' ><div></h1>");
        Global.post("/System/GetClientMemberLevel", {}, function (data) {
            $(".memberlevelul").html('');
            var items = data.items;
            if (items.length > 0) {
                var innnerHtml = '';
                for (var i = 0; i < items.length; i++) {
                    if (i == 0) {
                        innnerHtml += "<li id='memberLi" + i + "' class='lineHeight30'><div class='levelitem left' data-origin='" + (items[i].Origin - 1) + "' data-imgurl='" + items[i].ImgUrl + "'  data-integfeemore='" + items[i].IntegFeeMore + "' data-name='" + items[i].Name + "' data-discountfee='" + items[i].DiscountFee + "' data-id='" + items[i].LevelID + "' title='创建人:" + (items[i].CreateUser ? items[i].CreateUser.Name : "--") + "' >" +
                            "<div class='left'><span  class='spanimg mTop5' ><img name='MemberImg' id='MemberImg" + i + "' style='display:inline-block;' class='memberimg' title='点击替换等级图标 '  src='" + (items[i].ImgUrl != '' ? items[i].ImgUrl : '/Content/menuico/custom.png') + "' alt=''></span></div><span class='hide' id='SpanImg" + i + "'></span>" +
                            "<span  class='mLeft5 mRight5' style='display:inline-block;'>当客户积分在</span><input id='IntegFeeMore" + i + "' name='IntegFeeMore'  disabled='disabled' class='width50 mRight5' type='text' value='" + items[i].IntegFeeMore + "' /><span class='mRight5'>到</span>" +
                            "<input id='changeFeeMore" + i + "' name='IntegFeeMore' class='width50 mRight5' type='text' readOnly='readOnly' disabled='disabled'  value='" + (i == items.length - 1 ? '无上限' : items[i + 1].IntegFeeMore) + "' /><span class='mRight5'>之间，可享受</span><input name='DiscountFee' class='width50 mRight5' type='text' value='" + items[i].DiscountFee + "' />" +
                            "<span  class='mRight5'>折优惠 &nbsp; | 等级名称</span><input class='width80 mRight5' type='text' name='MemberName' placeholder='请填写等级名' value='" + items[i].Name + "' /><span id='delMeber" + i + "' data-ind='" + i + "' class='" + (i == 0 ? "hide" : i == items.length - 1 ? "" : "hide") + " borderccc circle12 mLeft10'>X</span>" +
                            "</div></li>";
                    } else {
                        innnerHtml += "<li id='memberLi" + i + "' class='lineHeight30'><div class='levelitem left' data-origin='" + (items[i].Origin - 1) + "' data-imgurl='" + items[i].ImgUrl + "'  data-integfeemore='" + items[i].IntegFeeMore + "' data-name='" + items[i].Name + "' data-discountfee='" + items[i].DiscountFee + "' data-id='" + items[i].LevelID + "' title='创建人:" + (items[i].CreateUser ? items[i].CreateUser.Name : "--") + "' >" +
                            "<div class='left'><span  class='spanimg mTop5' ><img name='MemberImg' id='MemberImg" + i + "' style='display:inline-block;' class='memberimg' title='点击替换等级图标 '  src='" + (items[i].ImgUrl != '' ? items[i].ImgUrl : '/Content/menuico/custom.png') + "' alt=''></span></div><span class='hide' id='SpanImg" + i + "'></span>" +
                            "<span  class='mLeft5 mRight5' style='display:inline-block;'>当客户积分在</span><input id='IntegFeeMore" + i + "' name='IntegFeeMore'  class='width50 mRight5' type='text' value='" + items[i].IntegFeeMore + "' /><span class='mRight5'>到</span>" +
                            "<input id='changeFeeMore" + i + "' name='IntegFeeMore' class='width50 mRight5' type='text' readOnly='readOnly' disabled='disabled'  value='" + (i == items.length - 1 ? '无上限' : items[i + 1].IntegFeeMore) + "' /><span class='mRight5'>之间，可享受</span><input name='DiscountFee' class='width50 mRight5' type='text' value='" + items[i].DiscountFee + "' />" +
                            "<span  class='mRight5'>折优惠 &nbsp; | 等级名称</span><input class='width80 mRight5' type='text' name='MemberName' placeholder='请填写等级名' value='" + items[i].Name + "' /><span id='delMeber" + i + "' data-ind='" + i + "' class='" + (i == 0 ? "hide" : i == items.length - 1 ? "" : "hide") + " borderccc circle12 mLeft10'>X</span>" +
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
            var intefee = parseInt($('#memberLi' + i + ' div:first-child').data('integfeemore')) + 300;
        }
        $('#changeFeeMore' + i).val(intefee);
        i = i + 1;
        var innnerHtml = "<li id='memberLi" + i + "' class='lineHeight30'><div class='levelitem left' data-origin='" + i + "' data-imgurl=''  data-integfeemore='" + intefee + "' data-name='' data-discountfee='1.00' data-id='' title='' >" +
                      "<div class='left'><span  class='spanimg mTop5' ><span class='hide' id='SpanImg" + i + "'></span><img name='MemberImg' style='display:inline-block;' id='MemberImg" + i + "' class='memberimg'   src='/Content/menuico/custom.png' alt=''></span></div>" +
                      "<span  class='mLeft5 mRight5'>当客户积分在</span><input id='IntegFeeMore" + i + "' name='IntegFeeMore' class='width50 mRight5' type='text' value='" + intefee + "' /><span class='mRight5'>到</span>" +
                      "<input id='changeFeeMore" + i + "'  class='width50 mRight5' placeholder='赠送金币'   type='text' value='无上限' /><span class='mRight5'>之间，可享受</span><input name='DiscountFee'  class='width50 mRight5' placeholder='请填写折扣'  type='text' value='1.00' />" +
                      "<span  class='mRight5'>折优惠 &nbsp; | 等级名称</span><input class='width80 mRight5' name='MemberName' type='text'  placeholder='请填写等级名' value='' /><span id='delMeber" + i + "' data-ind='" + i + "' class=' borderccc circle12 mLeft10'>X</span>" +
                      "</div></li>";
        if ($(".memberlevelul li:last-child").length > 0) {
            $(".memberlevelul li:last-child").after(innnerHtml);
        } else {
            $(".memberlevelul").after(innnerHtml);
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
                item.DiscountFee = $(v).data('discountfee');
                item.Name = $(v).data('name');
                item.ImgUrl = $(v).data('imgurl');
                item.Origin = parseInt($(v).data('origin')) + 1;
                item.LevelID = $(v).data('id');
                list.push(item);
            }
        });
        if (gonext) {
            Global.post("/System/SaveClientMemberLevel", { clientmemberlevel: JSON.stringify(list) }, function (data) {
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
        $('#changeFeeMore' + (ind - 1)).val('无上限');
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
                        _this.parent().parent().data('imgurl', data.Items[0]);
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
                        alert('当前积分阶段不能大于等于下一等级积分阶段');
                        _this.val(_this.parent().data('integfeemore'));
                    } if (parseInt($('#IntegFeeMore' + s).val()) >= parseInt(_this.val())) {
                        alert('当前积分阶段不能小于等于上一等级积分阶段');
                        _this.val(_this.parent().data('integfeemore'));
                    } else {
                        $('#changeFeeMore' + s).val(_this.val());
                        _this.parent().data('integfeemore', _this.val());
                    }
                } else {
                    _this.parent().data('integfeemore', _this.val());
                }
            } else {
                alert('积分格式输入有误，请重新输入');
                _this.val(_this.parent().data('integfeemore'));
            }
        } else if (type == 2) {
            if (!reg2.test(_this.val())) {
                alert('折扣格式输入有误，请重新输入');
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
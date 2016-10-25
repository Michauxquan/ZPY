define(function (require, exports, module) {
    var Global = require("global"),
        doT = require("dot"),
        Easydialog = require("easydialog"), 
        moment = require("moment");
    require("daterangepicker");
    require("pager");

    var Params = {
        paytype: '-1',
        status: "-1",
        userID: "",
        keywords: "",
        beginTime: "",
        endTime: "", 
        pageIndex: 1,
        pageSize: 15
    };

    var ObjectJS = {};

    //初始化
    ObjectJS.init = function () {
        var _self = this;
        _self.bindEvent();
    }

    //绑定事件
    ObjectJS.bindEvent = function () {
        var _self = this;

        //日期插件
        $("#iptCreateTime").daterangepicker({
            showDropdowns: true,
            empty: true,
            opens: "right",
            ranges: {
                '今天': [moment(), moment()],
                '昨天': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                '上周': [moment().subtract(6, 'days'), moment()],
                '本月': [moment().startOf('month'), moment().endOf('month')]
            }
        }, function (start, end, label) {
            Params.pageIndex = 1;
            Params.beginTime = start ? start.format("YYYY-MM-DD") : "";
            Params.endTime = end ? end.format("YYYY-MM-DD") : "";
            _self.getList();
        });
         
        require.async("dropdown", function () {
            var OrderType = [
               { name: "支付宝", value: "0" },
               { name: "线下汇款", value: "1" },
               { name: "银行卡", value: "2" },
               { name: "微信", value: "3" }
            ];
            $("#orderType").dropdown({
                prevText: "付款类型-",
                defaultText: "全部",
                defaultValue: "-1",
                data: OrderType,
                dataValue: "value",
                dataText: "name",
                width: "140",
                onChange: function (data) {
                    Params.paytype = data.value;
                    Params.pageIndex = 1;
                    _self.getList();
                }
            });
        });
        require.async("dropdown", function () {
            var OrderStatus = [
               { name: "待处理", value: "0" },
               { name: "支付成功", value: "1" },
               { name: "支付失败", value: "2" },
               { name: "作废", value: "3" }
            ];
            $("#orderStatus").dropdown({
                prevText: "状态-",
                defaultText: "全部",
                defaultValue: "-1",
                data: OrderStatus,
                dataValue: "value",
                dataText: "name",
                width: "140",
                onChange: function (data) {
                    Params.status = data.value;
                    Params.pageIndex = 1;
                    _self.getList();
                }
            });
        });
        //关键字搜索
        require.async("search", function () {
            $(".searth-module").searchKeys(function (keyWords) {
                Params.pageIndex = 1;
                Params.keywords = keyWords;
                _self.getList();
            });
        }); 
        //全部选中
        $("#checkAll").click(function () {
            var _this = $(this);
            if (!_this.hasClass("ico-checked")) {
                _this.addClass("ico-checked").removeClass("ico-check");
                $(".table-list .check").addClass("ico-checked").removeClass("ico-check");
            } else {
                _this.addClass("ico-check").removeClass("ico-checked");
                $(".table-list .check").addClass("ico-check").removeClass("ico-checked");
            }
        });

        $(".mulitImgUpdate").click(function () {
            var checks = $(".table-list .ico-checked");
            if (checks.length > 0) {
                var ids = "";
                checks.each(function (i, v) { ids += $(v).data('id') + "," });
                _self.updateStatus($(this).data('type'), ids);
            } else {
                alert("未选中数据，不能批量操作");
            }
        });
        _self.getList();
        $(document).click(function (e) {
            //隐藏下拉
            if (!$(e.target).parents().hasClass("dropdown-ul") && !$(e.target).parents().hasClass("dropdown") && !$(e.target).hasClass("dropdown")) {
                $(".dropdown-ul").hide();
            }
        });

        $('#updateObject').click(function() {
            var code = $(this).data('ordercode');
            _self.updateStatus(1, code);
        });
        $('#deleteObject').click(function () {
            var code = $(this).data('ordercode');
            _self.updateStatus(3, code);
        });
    }

    //获取列表
    ObjectJS.getList = function () {
        var _self = this;
        $("#checkAll").addClass("ico-check").removeClass("ico-checked");
        $(".tr-header").nextAll().remove();
        $(".tr-header").after("<tr><td colspan='9'><div class='data-loading' ><div></td></tr>");

        Global.post("/FAudit/OrdersList", Params, function (data) {
            _self.bindList(data);
        });
    }

    //加载列表
    ObjectJS.bindList = function (data) {
        var _self = this;
        $(".tr-header").nextAll().remove();

        if (data.items.length > 0) {
            doT.exec("template/faudit/orderlist.html", function (template) {
                var innerhtml = template(data.items);
                innerhtml = $(innerhtml);

                //下拉事件
                innerhtml.find(".dropdown").click(function () {
                    var _this = $(this);
                    var position = _this.find(".ico-dropdown").position();
                    $(".dropdown-ul li").data("ordercode", _this.data("ordercode")).data("id", _this.data("id"));
                    $(".dropdown-ul").css({ "top": position.top + 20, "left": position.left-50 }).show().mouseleave(function () {
                        $(this).hide();
                    });
                    return false;
                });
                innerhtml.find(".check").click(function () {
                    var _this = $(this);
                    if (!_this.hasClass("ico-checked")) {
                        _this.addClass("ico-checked").removeClass("ico-check");
                    } else {
                        _this.addClass("ico-check").removeClass("ico-checked");
                    }
                    return false;
                }); 
                $(".tr-header").after(innerhtml);

            });
        }
        else {
            $(".tr-header").after("<tr><td colspan='7'><div class='nodata-txt' >暂无数据!</div></td></tr>");
        }

        $("#pager").paginate({
            total_count: data.totalCount,
            count: data.pageCount,
            start: Params.pageIndex,
            display: 5,
            border: true,
            border_color: '#fff',
            text_color: '#333',
            background_color: '#fff',
            border_hover_color: '#ccc',
            text_hover_color: '#000',
            background_hover_color: '#efefef',
            rotate: true,
            images: false,
            mouse: 'slide',
            onChange: function (page) {
                Params.pageIndex = page;
                _self.getList();
            }
        });
    }
     
    ObjectJS.updateStatus = function (type, code) {
        var _self = this;
        console.log(type);
        Global.post("/FAudit/" + (type == 1 ? "OrderAuditing" : "BoutOrder"), {
             ordercode: code
        }, function (data) {
            if (data.result) {
                _self.getList();
            } else {
                if (typeof (data.errorMsg) != 'undefined') {
                    alert(data.errorMsg);
                } else {
                    alert('修改失败');
                }
            }
            $('.dropdown-ul').hide();
        });
    }

    module.exports = ObjectJS;
});
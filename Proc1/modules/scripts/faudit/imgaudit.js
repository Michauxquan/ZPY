define(function (require, exports, module) {
    var Global = require("global"),
        doT = require("dot"),
        moment = require("moment");
    require("daterangepicker");
    require("pager");

    var Params = {
        Status: 0,
        Keywords: "",
        BeginTime: "",
        EndTime: "",
        PageIndex: 1,
        PageSize: 20
    };

    var ObjectJS = {};

    //初始化
    ObjectJS.init = function () {
        var _self = this;
        _self.getList();
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
            Params.PageIndex = 1;
            Params.BeginTime = start ? start.format("YYYY-MM-DD") : "";
            Params.EndTime = end ? end.format("YYYY-MM-DD") : "";
            _self.getList();
        });
         
        //关键字搜索
        require.async("search", function () {
            $(".searth-module").searchKeys(function (keyWords) {
                Params.PageIndex = 1;
                Params.Keywords = keyWords;
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

        //批量转移
        $("#batchChangeOwner").click(function () {
            var checks = $(".table-list .ico-checked");
            if (checks.length > 0) {
            }
        }); 
    }

    //获取列表
    ObjectJS.getList = function () {
        var _self = this;
        $("#checkAll").addClass("ico-check").removeClass("ico-checked");
        $(".productlist").empty();
        $(".productlist").append("<div class='data-loading' ><div>");
        Global.post("/FAudit/ImgList", Params, function (data) {
            _self.bindList(data);
        });
    }

    //加载列表
    ObjectJS.bindList = function (data) {
        var _self = this; 
        $(".productlist").empty();
        $(".productlist").append("<div class='data-loading' ><div>");
        if (data.items.length > 0) {
            doT.exec("template/faudit/imglist.html", function (template) {
                var innerhtml = template(data.items);
                innerhtml = $(innerhtml); 
                innerhtml.find(".check").click(function () {
                    var _this = $(this);
                    if (!_this.hasClass("ico-checked")) {
                        _this.addClass("ico-checked").removeClass("ico-check");
                    } else {
                        _this.addClass("ico-check").removeClass("ico-checked");
                    }
                    return false;
                });

                $(".productlist").html(innerhtml);

            });
        }
        else {
            $(".productlist").after("<div class='nodata-txt' >暂无数据!</div>");
        }

        $("#pager").paginate({
            total_count: data.totalCount,
            count: data.pageCount,
            start: Params.PageIndex,
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
                Params.PageIndex = page;
                _self.getList();
            }
        });
    } 

    module.exports = ObjectJS;
});
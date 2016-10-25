
define(function (require, exports, module) {
    var Global = require("global"),
        Verify = require("verify"), VerifyObject, DetailsVerify, editor,
        doT = require("dot"),
        Easydialog = require("easydialog"),
        moment = require("moment");
    require("daterangepicker");
    require("pager");

    var ObjectJS = {};
    var Model = {};
    var Params = { 
        status: "-1",
        userID: "",
        keywords: "",
        beginTime: "",
        endTime: "",
        pageIndex: 1,
        pageSize: 15
    };
    //列表页初始化
    ObjectJS.initList = function () {
        var _self = this;  
        _self.bindListEvent();
    } 
    //绑定列表页事件
    ObjectJS.bindListEvent = function () {
        var _self = this;
        $(".btn-add").click(function () { 
            Model.View = '';
            Model.Remark = '';
            Model.Golds = '';
            Model.AutoID = -1;
            _self.editimg();
        });
        $("#popup").click(function () { $("#popup").hide() });

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
            var OrderStatus = [
               { name: "启用", value: "0" },
               { name: "禁用", value: "1" }
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

        _self.getList();
    } 
    ObjectJS.getList = function () {
        var _self = this;
        $(".tr-header").nextAll().remove();
        $(".tr-header").after("<tr><td colspan='6'><div class='data-loading' ><div></td></tr>");

        Global.post("/WebSet/GetChargeList", Params, function (data) {
            $(".tr-header").nextAll().remove();
            if (data.items.length > 0) {
                doT.exec("template/webset/chargesetlist.html", function (templateFun) {
                    var innerText = templateFun(data.items);
                    innerText = $(innerText);
                    $(".tr-header").after(innerText);
                    //绑定启用插件 
                    innerText.find(".auditimg").click(function () { 
                        Model.View = $(this).data("view");
                        Model.Golds = $(this).data("golds");
                        Model.Remark = $(this).data("remark");
                        Model.AutoID = $(this).data("id");
                        _self.editimg();
                    }); 
                    innerText.find(".deleteimg").click(function () {
                        var autoid = $(this).data("id");
                        confirm("确认删除设置吗？", function () { _self.cancelImg(autoid,9); });
                    });
                    innerText.find(".updimg").click(function () {
                        var autoid = $(this).data("id");
                        var status = $(this).data("status");
                        confirm("确认" + (status == 1 ? "禁用" : "启用") + "设置吗？", function () { _self.cancelImg(autoid,status); });
                    });

                }); 
            }
            else
            {
                $(".tr-header").after("<tr><td colspan='6'><div class='noDataTxt' >暂无数据!<div></td></tr>");
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
        });
    }
    ObjectJS.editimg= function() {
        var _self = this;
        doT.exec("template/webset/chargeset-detail.html", function (template) {
            var html = template([]);
            Easydialog.open({
                container: {
                    id: "show-chargeset-detail",
                    header: Model.AutoID==-1 ? "新建设置" : "编辑设置",
                    content: html,
                    yesFn: function () {
                        if (!VerifyObject.isPass()) {
                            return false;
                        }  
                        Model.View = $('#view').val();
                        Model.Golds = $('#golds').val();
                        Model.Remark = $("#remark").val();
                        Model.AutoID = $("#autoid").val();
                        _self.saveModel(Model);
                    },
                    callback: function () {

                    }
                }
            });
            VerifyObject = Verify.createVerify({
                element: ".verify",
                emptyAttr: "data-empty",
                verifyType: "data-type",
                regText: "data-text"
            });  
            $("#view").focus();
            $("#autoid").val(Model.AutoID);
            $("#view").val(Model.View); 
            $("#remark").val(Model.Remark); 
            $('#golds').val(Model.Golds); 
        });
    }
    ObjectJS.cancelImg = function (id, status) {
        var _self = this; 
        Global.post("/WebSet/UpdateChargeStatus", { autoid: id, status: status }, function (data) {
            if (data.result) {
                _self.getList();
            }
        });
    }
    ObjectJS.saveModel = function () {
        var _self = this;
        Global.post("/WebSet/SaveCharge", {
            entity: JSON.stringify(Model)
        }, function (data) {
            if (data.result) {
                _self.getList();
            }
        });
    }
    ObjectJS.showBigImg = function (src) {
        $("#popup").find("img").attr('src', src);
        $("#popup").show();
    }
    module.exports = ObjectJS;
})
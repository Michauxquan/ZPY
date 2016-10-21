

define(function (require, exports, module) {
    require("pager");
    var Global = require("global"),
        doT = require("dot"),
        moment = require("moment");
    require("daterangepicker");

    var FeedBack = {};
   
    FeedBack.Params = {
        pageIndex: 1,
        type: -1,
        status: -1,
        beginDate: '',
        endDate:'',
        keyWords: '',
        id:''
    };

    //列表初始化
    FeedBack.init = function () {
        FeedBack.bindEvent();
        FeedBack.bindData();
    };

    //绑定事件
    FeedBack.bindEvent = function () {
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
            _self.bindData();
        });

        //关键字查询
        require.async("search", function () {
            $(".searth-module").searchKeys(function (keyWords) {
                if (_self.Params.keyWords != keyWords) {
                    _self.Params.pageIndex = 1;
                    _self.Params.keyWords = keyWords;
                    _self.bindData();
                }
            });
        });

        //下拉状态、类型查询
        require.async("dropdown", function () {
            var Types = [
                {
                    ID: "1",
                    Name: "问题"
                },
                {
                    ID: "2",
                    Name: "建议"
                },
                {
                    ID: "3",
                    Name: "需求"
                }
            ];
            $("#FeedTypes").dropdown({
                prevText: "意见类型-",
                defaultText: "所有",
                defaultValue: "-1",
                data: Types,
                dataValue: "ID",
                dataText: "Name",
                width: "120",
                onChange: function (data) {
                    _self.Params.pageIndex = 1;
                    _self.Params.type = parseInt(data.value); 
                    _self.bindData();
                }
            });

            $(".search-tab li").click(function () {
                $(this).addClass("hover").siblings().removeClass("hover");
                var index = $(this).data("index");
                _self.Params.pageIndex = 1;
                _self.Params.status = index; 
                _self.bindData();
            });
        }); 
    };

    //绑定数据列表
    FeedBack.bindData = function () {
        $(".tr-header").nextAll().remove();
        Global.post("/SysSet/GetFeedBacks", FeedBack.Params, function (data) {
            doT.exec("template/sysset/FeedBack-list.html?3", function (templateFun) {
                var innerText = templateFun(data.Items);
                innerText = $(innerText);
                $(".tr-header").after(innerText);
            });

            $("#pager").paginate({
                total_count: data.TotalCount,
                count: data.PageCount,
                start: FeedBack.Params.pageIndex,
                display: 5,
                border: true,
                rotate: true,
                images: false,
                mouse: 'slide',
                onChange: function (page) {
                    FeedBack.Params.pageIndex = page;
                    FeedBack.bindData();
                }
            });
        });
    }

    FeedBack.detailInit = function (id) {
        FeedBack.Params.id = id;
        FeedBack.detailBindEvent();
        FeedBack.getFeedBackDetail();
    }

    FeedBack.detailBindEvent = function () {
        $("#btn-finish").click(function () {
            FeedBack.updateFeedBackStatus(2);
        });
        $("#btn-cancel").click(function () {
            FeedBack.updateFeedBackStatus(3);
        });
        $("#btn-delete").click(function () {
            FeedBack.updateFeedBackStatus(9);
        });
    }

    //详情
    FeedBack.getFeedBackDetail = function () {
        Global.post("/SysSet/GetFeedBackDetail", { id: FeedBack.Params.id }, function (data) {
            if (data.Item) {
                var item = data.Item;
                $("#Title").html(item.Title);
                var typeName = "问题";
                if (item.Type == 2)
                    typeName = "建议";
                else if (item.Type == 3)
                    typeName = "需求";
                $("#Type").html(typeName);

                var statusName = "待解决";
                if (item.Status == 1) {
                    statusName = "已解决";
                    $('#btn-finish').hide();
                    $('#btn-cancel').hide();
                    $('#btn-delete').hide();
                } else if (item.Status == 2) {
                    statusName = "驳回";
                    $('#btn-finish').hide();
                    $('#btn-cancel').hide();
                    $('#btn-delete').hide();
                } else if (item.Status == 9) {
                    statusName = "删除";
                }
                $("#Status").html(statusName);
                $("#TipedName").html(item.ContactName); 
                $("#Remark").html(item.Remark);
                $("#Content").html(item.Content);
                $("#CreateTime").html(item.CreateTime.toDate("yyyy-MM-dd hh:mm:ss"));
            } 
        });
    };

    //更改状态
    FeedBack.updateFeedBackStatus = function (status) {
        Global.post("/SysSet/UpdateFeedBackStatus", { id: FeedBack.Params.id, status: status, content: $('#Content').val() }, function (data) {
            if (data.Result == 1) {
                alert("保存成功");
                FeedBack.getFeedBackDetail();
            } else {
                alert("保存失败");
            }
        });
    };
    module.exports = FeedBack;
});
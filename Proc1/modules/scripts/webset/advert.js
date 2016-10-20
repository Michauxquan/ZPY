
define(function (require, exports, module) {
    var Upload = require("upload"),  ImgsIco,
        Global = require("global"),
        Verify = require("verify"), VerifyObject, DetailsVerify, editor,
        doT = require("dot"),
        Easydialog = require("easydialog");
    var ObjectJS = {};
    var Model = {};
    //列表页初始化
    ObjectJS.initList = function (url) {
        var _self = this;
        _self.url = url; 
        _self.bindListEvent();
    } 
    //绑定列表页事件
    ObjectJS.bindListEvent = function () {
        var _self = this;
        $(".btn-add").click(function () {
            Model.ImgType ='';
            Model.ImgUrl = '';
            Model.View = '';
            Model.Content = '';
            Model.AutoID = -1;
            _self.editimg();
        });
        $("#popup").click(function () { $("#popup").hide() });
        _self.getList();
    } 
    ObjectJS.getList = function () {
        var _self = this;
        $(".tr-header").nextAll().remove();
        $(".tr-header").after("<tr><td colspan='9'><div class='data-loading' ><div></td></tr>");

        Global.post("/WebSet/GetAdvertList", {}, function (data) {
            $(".tr-header").nextAll().remove();
            if (data.items.length > 0) {
                doT.exec("template/webset/advertlist.html", function (templateFun) {
                    var innerText = templateFun(data.items);
                    innerText = $(innerText);
                    $(".tr-header").after(innerText);
                    //绑定启用插件 
                    innerText.find(".auditimg").click(function () {
                        Model.ImgType = $(this).data("imgtype");
                        Model.ImgUrl = $(this).data("src");
                        Model.View = $(this).data("view");
                        Model.Content = $(this).data("content");
                        Model.AutoID = $(this).data("id");
                        _self.editimg();
                    }); 
                    innerText.find(".deleteimg").click(function () {
                        var autoid = $(this).data("id");
                        confirm("确认删除设置吗？", function () { _self.cancelImg(autoid); });
                    });
                    innerText.find("img").each(function () {
                        var _this = $(this);
                        _this.attr("src", _self.url + _this.attr("src"));
                        _this.click(function () { _self.showBigImg($(this).attr('src')); });
                    });

                }); 
            }
            else
            {
                $(".tr-header").after("<tr><td colspan='9'><div class='noDataTxt' >暂无数据!<div></td></tr>");
            } 
        });
    }
    ObjectJS.editimg= function() {
        var _self = this;
        doT.exec("template/webset/advert-detail.html", function (template) {
            var html = template([]);
            Easydialog.open({
                container: {
                    id: "show-advert-detail",
                    header: Model.AutoID==-1 ? "新建设置" : "编辑设置",
                    content: html,
                    yesFn: function () {
                        if (!VerifyObject.isPass()) {
                            return false;
                        } 
                        Model.ImgType = $("#imgtype option:selected").val();
                        Model.ImgUrl = $("#imgurl").val();
                        Model.View = $('#view').val();
                        Model.Content = $("#content").val();
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
            if (Model.AutoID >-1) {
                $(".imgtypeli").hide(); 
            } else { 
                $(".imgtypeli").show();
            }
            $("#view").focus();
            $("#autoid").val(Model.AutoID);
            $("#view").val(Model.View);
            $("#imgurl").val(Model.ImgUrl);
            $("#content").val(Model.Content);
            $("#imgtype").val(Model.ImgType);
            $('#imgslt').attr("src", Model.ImgUrl);
            ImgsIco = Upload.createUpload({
                element: '#ImgBtn',
                buttonText: "选择图片",
                className: "",
                data: { folder: '', action: 'add', oldPath: '' },
                success: function (data, status) {
                    if (data.Items.length > 0) {
                        $('#imgslt').attr("src", data.Items[0]);
                        $('#imgurl').val(data.Items[0]);
                    } else {
                        alert("只能上传jpg/png/gif类型的图片，且大小不能超过1M！");
                    }
                }
            });
        });
    }
    ObjectJS.cancelImg = function (id) {
        var _self = this; 
        Global.post("/WebSet/DeleteAdvertSet", {autoid: id}, function (data) {
            if (data.result) {
                _self.getList();
            }
        });
    }
    ObjectJS.saveModel = function () {
        var _self = this;
        Global.post("/WebSet/SaveAdvert", {
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
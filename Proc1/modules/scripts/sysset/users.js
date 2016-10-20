define(function (require, exports, module) {
    var Global = require("global"),
        doT = require("dot"),
        Verify = require("verify"), VerifyObject,
        Easydialog = require("easydialog");

    var Model = {},
        Rolelist = [];

    var Paras = {
        keyWords: "",
        pageIndex: 1
    }

    var ObjectJS = {};
    //初始化
    ObjectJS.init = function (rolelist) {
        var _self = this;
        _self.bindEvent();
        _self.getList();
        Rolelist = JSON.parse(rolelist.replace(/&quot;/g, '"'));
    }
    //绑定事件
    ObjectJS.bindEvent = function () {
        var _self = this;

        $(document).click(function (e) {
            //隐藏下拉
            if (!$(e.target).parents().hasClass("dropdown-ul") && !$(e.target).parents().hasClass("dropdown") && !$(e.target).hasClass("dropdown")) {
                $(".dropdown-ul").hide();
            }
        });

        $("#createModel").click(function () {
            var _this = $(this);
            Model.RoleID = "";
            Model.UserID = "";
            Model.Name = "";
            Model.Description = "";
            Model.LoginName = "";
            Model.LoginPWD = "";
            Model.Jobs = "";
            Model.Email = "";
            Model.MobilePhone = "";
            Model.OfficePhone = "";
            _self.createModel();
        });
        //删除
        $("#deleteObject").click(function () {
            var id = $(this).data("id");
             confirm("用户删除后不可恢复,确认删除吗？",function() {
                Global.post("/SysSet/DeleteMUser", { id: id }, function(data) {
                    if (data != null && data.status==1) {
                        _self.getList();
                    } else {
                        alert("操作失败,请重新操作！");
                    }
                });
            }); 
        });
        //编辑
        $("#updateObject").click(function () {
            var _this = $(this);
            Global.post("/SysSet/GetUserDetail", { id: _this.data("id") }, function (data) {
                var model = data.Item;
                Model.Name = model.Name;
                Model.UserID = model.UserID;
                Model.LoginName = model.LoginName;
                Model.LoginPWD = model.LoginPWD;
                Model.Jobs = model.Jobs;
                Model.Email = model.Email;
                Model.MobilePhone = model.MobilePhone;
                Model.OfficePhone = model.OfficePhone;
                Model.RoleID = model.RoleID;
                Model.Description = model.Description;
                Model.Sex = model.Sex;
                Model.IsMarry = model.IsMarry;
                Model.Education =model.Education;
                Model.BHeight = model.BHeight;
                Model.QQ =model.QQ;
                Model.Province = model.Province;
                Model.City = model.City;
                Model.District =model.District;
                _self.createModel();
            });
        });
    }
    //绑定权限
    ObjectJS.initRoleSelect = function (roleid) {
        $("#modelRoles option").remove();
        $.each(Rolelist, function (i, roleobj) {
            $("#modelRoles").append("<option value='" + roleobj.RoleID + "'>" + roleobj.Name + "</option>");
        });
        if (roleid != "") { $('#modelRoles').val(roleid); }
    }
    //登陆名称 密码失焦验证
    ObjectJS.initBindblur = function () {
        $("#modelLoginName").change(function () {
            $("#LoginameInfo").html('');
            if ($("#modelLoginName").val() != '') {
                Global.post("/SysSet/ValidateLoginName", { loginName: $("#modelLoginName").val() }, function (data) {
                    $("#LoginameInfo").html(data.Info);
                });
            }
        });
        $("#modelLoginPWD").blur(function () {
            $("#NewPwdError").html('');
            if ($("#modelLoginPWD").val() == '') {
                $("#NewPwdError").html('密码不能为空');
            }
        });
        $("#modelNewConfirmPwd").blur(function () {
            $("#NewConfirmPwdError").html('');
            if ($("#modelNewConfirmPwd").val() != $("#modelLoginPWD").val()) {
                $("#NewConfirmPwdError").html('确认密码与登录密码不一致');
            }
        });
    }
    //添加/编辑弹出层
    ObjectJS.createModel = function () {
        var _self = this;
        doT.exec("template/SysSet/user-detail.html", function (template) {
            var html = template([]);
            Easydialog.open({
                container: {
                    id: "show-model-detail",
                    header: !Model.UserID ? "新建用户" : "编辑用户",
                    content: html,
                    yesFn: function () {
                        if (!VerifyObject.isPass() || !_self.validatePWD(!Model.UserID)) {
                            return false;
                        }
                        Model.Name = $("#modelName").val();
                        Model.Email = $("#modelEmail").val();
                        Model.MobilePhone = $("#modelMobilePhone").val(); 
                        Model.RoleID = $('#modelRoles').val();
                        Model.Description = $("#modelDescription").val();
                        Model.LoginName = $("#modelLoginName").val();
                        Model.LoginPWD = $("#modelLoginPWD").val();
                        Model.Sex =  $("#modelSex").val(); 
                        Model.IsMarry= $("#modelIsMarry").val();
                        Model.Education = $('#modelEducation').val();
                        Model.BHeight = $('#modelBHeight').val();
                        Model.QQ = $('#modelQQ').val();
                        Model.Province = $('#modelProvince').val();
                        Model.City = $('#modelCity').val();
                        Model.District = $('#modelDistrict').val(); 
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
            _self.initRoleSelect(Model.RoleID);
            if (Model.UserID != "") {
                $(".userlihide").hide();
                $("#modelLoginName").val(Model.LoginName);
                $("#modelLoginPWD").val(Model.LoginPWD);
                $("#modelNewConfirmPwd").val(Model.LoginPWD);
                $("#modelSex").val();
                $("#modelIsMarry").val( Model.IsMarry);
                $('#modelEducation').val(Model.Education);
                $('#modelBHeight').val( Model.BHeight);
                $('#modelQQ').val(Model.QQ);
                $('#modelProvince').val(Model.Province);
                $('#modelCity').val(Model.City);
                $('#modelDistrict').val(Model.District);
            } else {
                _self.initBindblur();
                $(".userlihide").show();
            }
            $("#modelName").focus();
            $("#modelName").val(Model.Name);
            $("#modelEmail").val(Model.Email); 
            $("#modelMobilePhone").val(Model.MobilePhone);
            $("#modelOfficePhone").val(Model.OfficePhone);
            $("#modelDescription").val(Model.Description);
        });
    }

    //获取列表
    ObjectJS.getList = function () {
        var _self = this;
        $(".tr-header").nextAll().remove();
        $(".tr-header").after("<tr><td colspan='6'><div class='data-loading'><div></td></tr>");
        Global.post("/SysSet/GetUsers", Paras, function (data) {
            _self.bindList(data.Items);
        });
    }

    //加载列表
    ObjectJS.bindList = function (items) {
        var _self = this;
        $(".tr-header").nextAll().remove();
        if (items.length > 0) {
            doT.exec("template/SysSet/users.html", function (template) {
                var innerhtml = template(items);
                innerhtml = $(innerhtml);
                //操作
                innerhtml.find(".dropdown").click(function () {
                    var _this = $(this);
                    if (_this.data("type") != 1) {
                        var position = _this.find(".ico-dropdown").position();
                        $(".dropdown-ul li").data("id", _this.data("id"));
                        $(".dropdown-ul").css({ "top": position.top + 20, "left": position.left - 55 }).show().mouseleave(function () {
                            $(this).hide();
                        });
                    }
                });

                $(".tr-header").after(innerhtml);
            });
        }
        else {
            $(".tr-header").after("<tr><td colspan='6'><div class='noDataTxt' >暂无数据!</div></td></tr>");
        }
    }
    ObjectJS.validatePWD = function (userid) {
        if (userid) {
            if ($("#modelNewConfirmPwd").val() != $("#modelLoginPWD").val()) {
                return false;
            }
        }
        if ($("#LoginameInfo").html() != "") { return false; }
        return true;
    }
    //保存实体
    ObjectJS.saveModel = function (model) {
        var _self = this;
        Global.post("/SysSet/SaveUser", { entity: JSON.stringify(model) }, function (data) {
            if (data.errmeg == "执行成功") {
                _self.getList();
            } else { alert(data.errmeg); }
        })
    }

    module.exports = ObjectJS;
});
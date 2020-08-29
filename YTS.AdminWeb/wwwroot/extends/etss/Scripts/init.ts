$.fn.textbox.defaults.cls = "theme-border-radius";
$.fn.combobox.defaults.cls = "theme-border-radius";
$.fn.dialog.defaults.cls = "theme-border-radius";
$.fn.panel.defaults.cls = "theme-border-radius";
$.fn.datagrid.defaults.cls = "theme-border-radius";
$.fn.treegrid.defaults.cls = "theme-border-radius";
$.fn.combo.defaults.panelHeight = null;

$.extend($.serializeJSON.defaultOptions, { checkboxUncheckedValue: "false" });
$.fn.switchbutton.defaults.height = 28;

$.extend($.fn.validatebox.defaults.rules, {
    selectRequired: {
        validator: function (value, param) {
            if (value == '请选择' || Enumerable.from<any>(param).contains(value)) {
                return false;
            }
            return true;
        },
        message: '该选择框为必选'
    },
    number: {
        validator: function (value, param) {
            if (/\d*/ig.test(value)) {
                return true;
            }
            return false;
        },
        message: '必须为数字'
    },
    mobile: {
        validator: function (value, param) {
            var reg = /^[1][3-9][0-9]{9}$/;
            if (!reg.test(value)) {
                return false;
            } else {
                return true;
            }
        },
        message: '手机号格式不正确'
    },
    mobiles: {
        validator: function (value, param) {
            var reg = /^[0-9,]+$/;
            if (!reg.test(value)) {
                return false;
            } else {
                return true;
            }
        },
        message: '格式不正确,请填写正确手机号并用英文逗号分割'
    }
});

$(function () {
    Application.LoadPromise = new Promise(async (resolve, reject) => {
        var currentUser = await Call(AdminWebApi.System.GetCurrentUser);
        if (currentUser.Error) {
            $.messager.alert({
                title: '错误',
                msg: "加载当前用户出错。",
                icon: 'error'
            });
        } else {
            Application.User = currentUser.Data
        }
        resolve();
    });
    Application.LoadPromise.then();
});

$(async function () {
    if (window.top == window.self) {
        $.cookie('mainUrl', window.location.href, { path: '/' });

        if (!Application.SystemId) {
            var systemId = await Call(AdminWebApi.System.GetDefaultSystemId);
            Application.DefaultSystemId = systemId;
        }

        var href = '/index.html';
        href = urlParam(href, 'sid', Application.SystemId);

        window.top.location.href = href;
        return;
    }

    var checkPermissionResult = await Call(AdminWebApi.Permission.CheckPermission, { url: window.location.href });
    if (checkPermissionResult.Error) {
        $.messager.alert({
            title: '错误',
            msg: "验证权限错误:" + GetMessage(checkPermissionResult),
            icon: 'error'
        });
        return;
    } else {
        if (checkPermissionResult.Data != null && !checkPermissionResult.Data) {
            document.execCommand("Stop");
            window.stop();
            document.write("授权被禁止");
            document.body.innerHTML = "授权被禁止";
            $.messager.alert({
                title: '错误',
                msg: "无权访问该页面。",
                icon: 'error'
            });

            window.history.back();
            return;
        }
    }

    var breadcrumbsResult = await Call(AdminWebApi.Permission.GetBreadcrumbs, { url: window.location.href });
    if (breadcrumbsResult.Error) {
        $.messager.alert({
            title: '错误',
            msg: "获取面包屑导航错误:" + GetMessage(breadcrumbsResult),
            icon: 'error'
        });
        return;
    } else {
        var breadcrumbsContainer = $('.ibox-title>h5');
        breadcrumbsContainer.html('');
        breadcrumbsContainer.append('<i class="fa fa-home" style="font-size: 16px;"></i>');

        for (var i in breadcrumbsResult) {
            var item = breadcrumbsResult[i];
            breadcrumbsContainer.append(' <span>' + item.Name + '</span> <i class="fa fa-angle-double-right"></i>');
        }

        breadcrumbsContainer.find('.fa-angle-double-right:last').remove();
    }

    $('.ibox-content > .panel > .easyui-panel').panel('resize', {
        width: ($(window).width()) - 35
    });
    var height = Enumerable.from<any>($('.ibox-content > .panel > .easyui-panel').map((i, e) => $(e).parents('.panel').outerHeight()).get()).sum();
    $('#datagrid').datagrid('resize', {
        width: ($(window).width()) - 35,
        height: ($(window).height()) - height - 77
    });
    $('#treegrid').treegrid('resize', {
        width: ($(window).width()) - 35,
        height: ($(window).height()) - height - 77
    });
});

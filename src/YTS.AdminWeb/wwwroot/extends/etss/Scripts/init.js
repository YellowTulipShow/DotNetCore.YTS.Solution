var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
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
            if (value == '请选择' || Enumerable.from(param).contains(value)) {
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
            }
            else {
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
            }
            else {
                return true;
            }
        },
        message: '格式不正确,请填写正确手机号并用英文逗号分割'
    }
});
$(function () {
    var _this = this;
    Application.LoadPromise = new Promise(function (resolve, reject) { return __awaiter(_this, void 0, void 0, function () {
        var currentUser;
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, Call(AdminWebApi.System.GetCurrentUser)];
                case 1:
                    currentUser = _a.sent();
                    if (currentUser.Error) {
                        $.messager.alert({
                            title: '错误',
                            msg: "加载当前用户出错。",
                            icon: 'error'
                        });
                    }
                    else {
                        Application.User = currentUser.Data;
                    }
                    resolve();
                    return [2 /*return*/];
            }
        });
    }); });
    Application.LoadPromise.then();
});
$(function () {
    return __awaiter(this, void 0, void 0, function () {
        var systemId, href, checkPermissionResult, breadcrumbsResult, breadcrumbsContainer, i, item, height;
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0:
                    if (!(window.top == window.self)) return [3 /*break*/, 3];
                    $.cookie('mainUrl', window.location.href, { path: '/' });
                    if (!!Application.SystemId) return [3 /*break*/, 2];
                    return [4 /*yield*/, Call(AdminWebApi.System.GetDefaultSystemId)];
                case 1:
                    systemId = _a.sent();
                    Application.DefaultSystemId = systemId;
                    _a.label = 2;
                case 2:
                    href = '/index.html';
                    href = urlParam(href, 'sid', Application.SystemId);
                    window.top.location.href = href;
                    return [2 /*return*/];
                case 3: return [4 /*yield*/, Call(AdminWebApi.Permission.CheckPermission, { url: window.location.href })];
                case 4:
                    checkPermissionResult = _a.sent();
                    if (checkPermissionResult.Error) {
                        $.messager.alert({
                            title: '错误',
                            msg: "验证权限错误:" + GetMessage(checkPermissionResult),
                            icon: 'error'
                        });
                        return [2 /*return*/];
                    }
                    else {
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
                            return [2 /*return*/];
                        }
                    }
                    return [4 /*yield*/, Call(AdminWebApi.Permission.GetBreadcrumbs, { url: window.location.href })];
                case 5:
                    breadcrumbsResult = _a.sent();
                    if (breadcrumbsResult.Error) {
                        $.messager.alert({
                            title: '错误',
                            msg: "获取面包屑导航错误:" + GetMessage(breadcrumbsResult),
                            icon: 'error'
                        });
                        return [2 /*return*/];
                    }
                    else {
                        breadcrumbsContainer = $('.ibox-title>h5');
                        breadcrumbsContainer.html('');
                        breadcrumbsContainer.append('<i class="fa fa-home" style="font-size: 16px;"></i>');
                        for (i in breadcrumbsResult) {
                            item = breadcrumbsResult[i];
                            breadcrumbsContainer.append(' <span>' + item.Name + '</span> <i class="fa fa-angle-double-right"></i>');
                        }
                        breadcrumbsContainer.find('.fa-angle-double-right:last').remove();
                    }
                    $('.ibox-content > .panel > .easyui-panel').panel('resize', {
                        width: ($(window).width()) - 35
                    });
                    height = Enumerable.from($('.ibox-content > .panel > .easyui-panel').map(function (i, e) { return $(e).parents('.panel').outerHeight(); }).get()).sum();
                    $('#datagrid').datagrid('resize', {
                        width: ($(window).width()) - 35,
                        height: ($(window).height()) - height - 77
                    });
                    $('#treegrid').treegrid('resize', {
                        width: ($(window).width()) - 35,
                        height: ($(window).height()) - height - 77
                    });
                    return [2 /*return*/];
            }
        });
    });
});
//# sourceMappingURL=init.js.map
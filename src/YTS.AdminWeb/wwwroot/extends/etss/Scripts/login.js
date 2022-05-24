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
if (window.top !== window.self) {
    window.top.location.href = window.location.href;
}
if (AdminWebApi.UserToken) {
    login();
}
function login() {
    return __awaiter(this, void 0, void 0, function () {
        var defaultSystemId, mainUrl, url;
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, Call(AdminWebApi.System.GetDefaultSystemId)];
                case 1:
                    defaultSystemId = _a.sent();
                    if (!!defaultSystemId) return [3 /*break*/, 3];
                    $.messager.alert({
                        title: '错误',
                        msg: "您无权访问该系统。",
                        icon: 'error'
                    });
                    return [4 /*yield*/, logoutLogin()];
                case 2:
                    _a.sent();
                    return [2 /*return*/];
                case 3:
                    if (!defaultSystemId.Error) return [3 /*break*/, 5];
                    $.messager.alert({
                        title: '错误',
                        msg: "获取默认系统时出错：" + GetMessage(defaultSystemId),
                        icon: 'error'
                    });
                    return [4 /*yield*/, logoutLogin()];
                case 4:
                    _a.sent();
                    return [2 /*return*/];
                case 5:
                    mainUrl = getParam('url');
                    if (mainUrl) {
                        $.cookie('mainUrl', mainUrl, { path: '/' });
                    }
                    url = '/index.html';
                    url = urlParam(url, 'sid', defaultSystemId);
                    window.location.href = url;
                    return [2 /*return*/];
            }
        });
    });
}
function logoutLogin() {
    return __awaiter(this, void 0, void 0, function () {
        var usertoken, result;
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0:
                    usertoken = AdminWebApi.UserToken;
                    return [4 /*yield*/, Call(AdminWebApi.User.Logout.UrlData({ UserToken: usertoken }))];
                case 1:
                    result = _a.sent();
                    AdminWebApi.UserToken = null;
                    usertoken = ShopWebApi.UserToken;
                    return [4 /*yield*/, Call(ShopWebApi.User.Logout.UrlData({ UserToken: usertoken }))];
                case 2:
                    result = _a.sent();
                    ShopWebApi.UserToken = null;
                    return [2 /*return*/];
            }
        });
    });
}
$(function () {
    $('#login_submit').click(function () {
        (function () {
            return __awaiter(this, void 0, void 0, function () {
                var loginModel, resultAdminLogin, resultShopLogin;
                return __generator(this, function (_a) {
                    switch (_a.label) {
                        case 0:
                            if ($("#login_Account").val() == "") {
                                $.messager.alert({
                                    title: '错误',
                                    msg: '用户名不能为空!',
                                    icon: 'error'
                                });
                                return [2 /*return*/];
                            }
                            if ($("#login_Password").val() == "") {
                                $.messager.alert({
                                    title: '错误',
                                    msg: '密码不能为空!',
                                    icon: 'error'
                                });
                                return [2 /*return*/];
                            }
                            $.messager.progress({
                                noheader: true,
                                title: false,
                                msg: '页面加载中...',
                                interval: 100
                            });
                            loginModel = $('#login').serializeJSON();
                            return [4 /*yield*/, Call(AdminWebApi.User.Login.UrlData(loginModel))];
                        case 1:
                            resultAdminLogin = _a.sent();
                            $.messager.progress('close');
                            if (resultAdminLogin.Error) {
                                $.messager.alert({
                                    title: '错误',
                                    msg: GetMessage(resultAdminLogin),
                                    icon: 'error'
                                });
                                return [2 /*return*/];
                            }
                            AdminWebApi.UserToken = resultAdminLogin.Data.UserToken;
                            loginModel.SystemNameSpace = "ErTuiShengShi.ShopWeb";
                            return [4 /*yield*/, Call(ShopWebApi.User.Login.UrlData(loginModel))];
                        case 2:
                            resultShopLogin = _a.sent();
                            if (resultShopLogin.Error) {
                                $.messager.alert({
                                    title: '错误',
                                    msg: GetMessage(resultShopLogin),
                                    icon: 'error'
                                });
                                //await logoutLogin();
                                //return;
                            }
                            else {
                                ShopWebApi.UserToken = resultShopLogin.Data.UserToken;
                            }
                            login();
                            return [2 /*return*/];
                    }
                });
            });
        })();
        return false;
    });
});
//# sourceMappingURL=login.js.map
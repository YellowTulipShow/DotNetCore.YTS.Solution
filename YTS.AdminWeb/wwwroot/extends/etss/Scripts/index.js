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
var websocket = new WebSocket(('https:' == document.location.protocol ? 'wss://' : 'ws://') + 'localhost:52009');
websocket.onopen = function (e) {
    Heartbeat();
};
websocket.onmessage = function (e) {
    var obj = JSON.parse(e.data);
    switch (obj.Method) {
        case 'Heartbeat':
            console.log(dateFormat(new Date(obj.Content), 'yyyy-MM-dd HH:mm:ss'));
            break;
        case 'InitMessage':
            var messageList = obj.Content;
            $('#MessageCount').html(messageList.total);
            if (messageList.total == 0) {
                $('#MessageCount').hide();
            }
            else {
                $('#MessageCount').show();
            }
            $('#TopMessageList li').not('#ShowAllMessage').remove();
            for (var i in messageList.rows) {
                var item = messageList.rows[i];
                var userHeadsrc = item.HeadImage ? item.HeadImage : '/Image/timg1.jpg';
                $('<li>' +
                    '    <div class="dropdown-messages-box">' +
                    '        <a href="profile.html" class="pull-left">' +
                    '            <img alt="image" class="img-circle" src="' + userHeadsrc + '">' +
                    '        </a>' +
                    '        <div class="media-body ">' +
                    '            ' + item.MessageTitle + ' <br>' +
                    '            <small class="text-muted">' + dateFormat(new Date(item.SendTime), 'yyyy-MM-dd HH:mm:ss') + '</small>' +
                    '        </div>' +
                    '    </div>' +
                    '</li><li class="divider"></li>').insertBefore($('#ShowAllMessage'));
            }
        default:
            break;
    }
};
websocket.onclose = function (e) {
    //AdminWebApi.UserToken = null;
    //$.messager.alert({
    //    title: '提示',
    //    msg: "登录失效，请重新登录。",
    //    icon: 'error',
    //    fn: function () {
    //        var url = '/login.html';
    //        $('.J_mainContent iframe').each(function () { if ($(this).css('display') != 'none') { url = urlParam(url, 'url', this.src); } });
    //        window.location.href = url;
    //    }
    //});
};
function Heartbeat() {
    if (window.isInit) {
        $.messager.progress({
            noheader: true,
            title: false,
            msg: '页面加载中...',
            interval: 100
        });
    }
    websocket.send(JSON.stringify({ Method: 'Heartbeat', Content: '' }));
    if (window.isInit) {
        $.messager.progress('close');
        window.isInit = false;
    }
    return true;
}
function logout() {
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
                    if (result.Error) {
                        $.messager.alert({
                            title: '错误',
                            msg: "退出错误：" + GetMessage(result),
                            icon: 'error'
                        });
                    }
                    usertoken = ShopWebApi.UserToken;
                    return [4 /*yield*/, Call(ShopWebApi.User.Logout.UrlData({ UserToken: usertoken }))];
                case 2:
                    result = _a.sent();
                    ShopWebApi.UserToken = null;
                    if (result.Error) {
                        $.messager.alert({
                            title: '错误',
                            msg: "退出错误：" + GetMessage(result),
                            icon: 'error'
                        });
                    }
                    else {
                        clearInterval(window.timerHeartbeat);
                        $.messager.alert({
                            title: '提示',
                            msg: "已安全退出。",
                            icon: 'info',
                            fn: function () {
                                window.location.href = '/login.html';
                            }
                        });
                    }
                    return [2 /*return*/];
            }
        });
    });
}
function GoToSystem(id) {
    return __awaiter(this, void 0, void 0, function () {
        var currentSystem, mainUrl;
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0:
                    Application.SystemId = id;
                    return [4 /*yield*/, LoadLeftMenu()];
                case 1:
                    _a.sent();
                    $('.J_tabCloseAll').click();
                    $("#side-menu").metisMenu();
                    currentSystem = Enumerable.from(Application.SystemList).where(function (a) { return a.Id == Application.SystemId; }).firstOrDefault({});
                    mainUrl = mainUrl ? mainUrl : (currentSystem.Url + '').trim() ? currentSystem.Url : '/welcome.html';
                    $('.J_menuTabs a').data('id', mainUrl);
                    $('.J_mainContent iframe').data('id', mainUrl).attr('src', mainUrl);
                    $('.logo-title').html('<i class="' + (currentSystem.Icon ? currentSystem.Icon : 'fa fa-folder') + '" style="font-size:18px;"></i> ' + currentSystem.Name + ' <i class="fa fa-angle-down" style="font-size:18px;"></i>');
                    return [2 /*return*/];
            }
        });
    });
}
function LoadLeftMenu() {
    return __awaiter(this, void 0, void 0, function () {
        var leftMenuList, oneLevelNodes, oneLevelNodeElement, i, oneLevelNode, hasChild, urldata, twoLevelContainer, twoLevelNodes, j, twoLevelNode, twoLevelNodeElement, threeLevelContainer, threeLevelNodes, k, threeLevelNode, threeLevelNodeElement;
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0: return [4 /*yield*/, Call(AdminWebApi.System.GetLeftMenuList, { parentId: Application.SystemId })];
                case 1:
                    leftMenuList = _a.sent();
                    if (leftMenuList.Error) {
                        $.messager.alert({
                            title: '错误',
                            msg: "加载系统菜单出错：" + GetMessage(leftMenuList),
                            icon: 'error'
                        });
                    }
                    $('#side-menu>li:gt(1)').remove();
                    oneLevelNodes = Enumerable.from(leftMenuList).where(function (a) { return a.ParentId == Application.SystemId; }).toArray();
                    oneLevelNodeElement = $('#CurrentSystemTitle');
                    for (i in oneLevelNodes) {
                        oneLevelNode = oneLevelNodes[i];
                        hasChild = Enumerable.from(leftMenuList).where(function (a) { return a.ParentId == oneLevelNode.Id; }).any();
                        urldata = oneLevelNode.UrlData ? JSON.parse(oneLevelNode.UrlData) : null;
                        oneLevelNodeElement = $('<li><a data-index="' + oneLevelNode.Id + '" ' + (!hasChild ? 'class="J_menuItem"' : '') + ' href="' + (hasChild ? '#' : urlParamObj(oneLevelNode.Url, urldata)) + '"><i class="' + (oneLevelNode.Icon ? oneLevelNode.Icon : 'fa fa-sticky-note') + '"></i> <span class="nav-label">' + oneLevelNode.Name + '</span> ' + (hasChild ? '<span class="fa arrow"></span>' : '') + '</a></li>').insertAfter(oneLevelNodeElement);
                        if (hasChild) {
                            twoLevelContainer = $('<ul class="nav nav-second-level"></ul>').appendTo(oneLevelNodeElement);
                            twoLevelNodes = Enumerable.from(leftMenuList).where(function (a) { return a.ParentId == oneLevelNode.Id; }).toArray();
                            for (j in twoLevelNodes) {
                                twoLevelNode = twoLevelNodes[j];
                                hasChild = Enumerable.from(leftMenuList).where(function (a) { return a.ParentId == twoLevelNode.Id; }).any();
                                urldata = twoLevelNode.UrlData ? JSON.parse(twoLevelNode.UrlData) : null;
                                twoLevelNodeElement = $('<li><a data-index="' + twoLevelNode.Id + '" ' + (!hasChild ? 'class="J_menuItem"' : '') + ' href="' + (hasChild ? '#' : urlParamObj(twoLevelNode.Url, urldata)) + '">' + twoLevelNode.Name + (hasChild ? '<span class="fa arrow"></span>' : '') + '</a></li>').appendTo(twoLevelContainer);
                                if (hasChild) {
                                    threeLevelContainer = $('<ul class="nav nav-third-level"></ul>').appendTo(twoLevelNodeElement);
                                    threeLevelNodes = Enumerable.from(leftMenuList).where(function (a) { return a.ParentId == twoLevelNode.Id; }).toArray();
                                    for (k in threeLevelNodes) {
                                        threeLevelNode = threeLevelNodes[k];
                                        urldata = threeLevelNode.UrlData ? JSON.parse(threeLevelNode.UrlData) : null;
                                        threeLevelNodeElement = $('<li><a data-index="' + threeLevelNode.Id + '" class="J_menuItem" href="' + urlParamObj(threeLevelNode.Url, urldata) + '">' + threeLevelNode.Name + '</a></li>').appendTo(threeLevelContainer);
                                    }
                                }
                            }
                        }
                    }
                    $(".J_menuItem").on("click", window.J_menuItem_click);
                    return [2 /*return*/];
            }
        });
    });
}
$(function () {
    return __awaiter(this, void 0, void 0, function () {
        var mainUrl, checkPermissionResult, currentUser, user, groups, userHeadsrc, _a, currentSystem, i, system;
        return __generator(this, function (_b) {
            switch (_b.label) {
                case 0:
                    mainUrl = $.cookie('mainUrl');
                    $.cookie('mainUrl', '', { expires: -1 });
                    $('.J_menuTabs a').data('id', mainUrl);
                    $('.J_mainContent iframe').data('id', mainUrl).attr('src', mainUrl);
                    window.timerHeartbeat = setInterval(Heartbeat, 200000);
                    return [4 /*yield*/, Call(AdminWebApi.Permission.CheckPermission, { url: window.location.href })];
                case 1:
                    checkPermissionResult = _b.sent();
                    if (checkPermissionResult.Error) {
                        $.messager.alert({
                            title: '错误',
                            msg: "验证权限错误：" + GetMessage(checkPermissionResult),
                            icon: 'error',
                            fn: function () {
                                window.location.href = '/login.html';
                            }
                        });
                        return [2 /*return*/];
                    }
                    else {
                        if (checkPermissionResult.Data != null && !checkPermissionResult.Data) {
                            $.messager.alert({
                                title: '错误',
                                msg: "无权访问。",
                                icon: 'error',
                                fn: function () {
                                    window.location.href = '/login.html';
                                }
                            });
                            return [2 /*return*/];
                        }
                    }
                    return [4 /*yield*/, Call(AdminWebApi.System.GetCurrentUser)];
                case 2:
                    currentUser = _b.sent();
                    if (currentUser.Error) {
                        $.messager.alert({
                            title: '错误',
                            msg: "加载当前用户出错：" + GetMessage(currentUser),
                            icon: 'error'
                        });
                    }
                    else {
                        Application.User = currentUser.Data;
                        user = Application.User.User;
                        groups = Application.User.UserGroups;
                        if (!user.UserInfo) {
                            user.UserInfo = {};
                        }
                        userHeadsrc = user.UserInfo.EmployeeImg ? user.UserInfo.EmployeeImg : user.UserInfo.Sex == '女' ? '/Image/timg2.jpg' : '/Image/timg1.jpg';
                        $('#user_Head').attr('src', userHeadsrc);
                        $('#user_Name').html(user.Name ? user.Name : user.Account);
                        $('#user_Group').html(groups.length ? groups[0].Name : '无');
                    }
                    _a = Application;
                    return [4 /*yield*/, Call(AdminWebApi.System.GetSystemList)];
                case 3:
                    _a.SystemList = _b.sent();
                    if (Application.SystemList.Error) {
                        $.messager.alert({
                            title: '错误',
                            msg: "加载系统列表出错：" + GetMessage(Application.SystemList),
                            icon: 'error'
                        });
                    }
                    else {
                        Application.SystemId = parseInt(getParam('sid'));
                        if (!Application.SystemId) {
                            Application.SystemId = Enumerable.from(Application.SystemList).firstOrDefault({}).Id;
                        }
                        currentSystem = Enumerable.from(Application.SystemList).where(function (a) { return a.Id == Application.SystemId; }).firstOrDefault({});
                        mainUrl = mainUrl ? mainUrl : (currentSystem.Url + '').trim() ? currentSystem.Url : '/welcome.html';
                        $('.J_menuTabs a').data('id', mainUrl);
                        $('.J_mainContent iframe').data('id', mainUrl).attr('src', mainUrl);
                        $('.logo-title').html('<i class="' + (currentSystem.Icon ? currentSystem.Icon : 'fa fa-folder') + '" style="font-size:18px;"></i> ' + currentSystem.Name + ' <i class="fa fa-angle-down" style="font-size:18px;"></i>');
                        for (i in Application.SystemList) {
                            system = Application.SystemList[i];
                            $('#SystemList').append($('<li><a href="#" onclick="GoToSystem(' + system.Id + ');return false;"><i class="' + (system.Icon ? system.Icon : 'fa fa-folder') + '"></i> ' + system.Name + '</a></li>'));
                        }
                    }
                    return [4 /*yield*/, LoadLeftMenu()];
                case 4:
                    _b.sent();
                    window.hplusInit();
                    return [2 /*return*/];
            }
        });
    });
});
function OpenUpdatePass() {
    $('#dialogPass').dialog('open').dialog('setTitle', '修改密码');
}
function SavePass() {
    return __awaiter(this, void 0, void 0, function () {
        var model, result;
        return __generator(this, function (_a) {
            switch (_a.label) {
                case 0:
                    model = $("#dialog-form").serializeJSON();
                    return [4 /*yield*/, Call(AdminWebApi.User.UpdatePass.UrlData(model))];
                case 1:
                    result = _a.sent();
                    if (result.ErrorCode == 0) {
                        $.messager.alert({
                            title: '成功',
                            msg: '操作成功!',
                            icon: 'success'
                        });
                        $('#dialogPass').dialog('close');
                    }
                    else {
                        $.messager.alert({
                            title: '错误',
                            msg: result.Message,
                            icon: 'error'
                        });
                    }
                    return [2 /*return*/];
            }
        });
    });
}
//# sourceMappingURL=index.js.map
var websocket = new WebSocket(('https:' == document.location.protocol ? 'wss://' : 'ws://')+'localhost:52009');
websocket.onopen = function (e) {
    Heartbeat();
}
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
            } else {
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
}
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
}

function Heartbeat() {
    if ((window as any).isInit) {
        $.messager.progress({
            noheader: true,
            title: false,
            msg: '页面加载中...',
            interval: 100
        });
    }
    websocket.send(JSON.stringify({ Method: 'Heartbeat', Content: '' }));

    if ((window as any).isInit) {
        $.messager.progress('close');
        (window as any).isInit = false;
    }
    return true;
}

async function logout() {
    var usertoken = AdminWebApi.UserToken;
    var result = await Call(AdminWebApi.User.Logout.UrlData({ UserToken: usertoken }));
    AdminWebApi.UserToken = null;

    if (result.Error) {
        $.messager.alert({
            title: '错误',
            msg: "退出错误：" + GetMessage(result),
            icon: 'error'
        });
    }

    usertoken = ShopWebApi.UserToken;
    result = await Call(ShopWebApi.User.Logout.UrlData({ UserToken: usertoken }));
    ShopWebApi.UserToken = null;

    if (result.Error) {
        $.messager.alert({
            title: '错误',
            msg: "退出错误：" + GetMessage(result),
            icon: 'error'
        });
    } else {
        clearInterval((window as any).timerHeartbeat);
        $.messager.alert({
            title: '提示',
            msg: "已安全退出。",
            icon: 'info',
            fn: function () {
                window.location.href = '/login.html';
            }
        });
    }
}

async function GoToSystem(id) {
    Application.SystemId = id;

    await LoadLeftMenu();

    $('.J_tabCloseAll').click();
    $("#side-menu").metisMenu();

    var currentSystem = Enumerable.from<any>(Application.SystemList).where(a => a.Id == Application.SystemId).firstOrDefault({});
    var mainUrl = mainUrl ? mainUrl : (currentSystem.Url + '').trim() ? currentSystem.Url : '/welcome.html';

    $('.J_menuTabs a').data('id', mainUrl);
    $('.J_mainContent iframe').data('id', mainUrl).attr('src', mainUrl);

    $('.logo-title').html('<i class="' + (currentSystem.Icon ? currentSystem.Icon : 'fa fa-folder') + '" style="font-size:18px;"></i> ' + currentSystem.Name + ' <i class="fa fa-angle-down" style="font-size:18px;"></i>');
}

async function LoadLeftMenu() {
    var leftMenuList = await Call(AdminWebApi.System.GetLeftMenuList, { parentId: Application.SystemId });

    if (leftMenuList.Error) {
        $.messager.alert({
            title: '错误',
            msg: "加载系统菜单出错：" + GetMessage(leftMenuList),
            icon: 'error'
        });
    }

    $('#side-menu>li:gt(1)').remove();

    var oneLevelNodes = Enumerable.from<any>(leftMenuList).where(a => a.ParentId == Application.SystemId).toArray();
    var oneLevelNodeElement = $('#CurrentSystemTitle');


    for (var i in oneLevelNodes) {
        var oneLevelNode = oneLevelNodes[i];
        var hasChild = Enumerable.from<any>(leftMenuList).where(a => a.ParentId == oneLevelNode.Id).any();
        var urldata = oneLevelNode.UrlData ? JSON.parse(oneLevelNode.UrlData) : null;
        oneLevelNodeElement = $('<li><a data-index="' + oneLevelNode.Id + '" ' + (!hasChild ? 'class="J_menuItem"' : '') + ' href="' + (hasChild ? '#' : urlParamObj(oneLevelNode.Url, urldata)) + '"><i class="' + (oneLevelNode.Icon ? oneLevelNode.Icon : 'fa fa-sticky-note') + '"></i> <span class="nav-label">' + oneLevelNode.Name + '</span> ' + (hasChild ? '<span class="fa arrow"></span>' : '') + '</a></li>').insertAfter(oneLevelNodeElement);
        if (hasChild) {
            var twoLevelContainer = $('<ul class="nav nav-second-level"></ul>').appendTo(oneLevelNodeElement);
            var twoLevelNodes = Enumerable.from<any>(leftMenuList).where(a => a.ParentId == oneLevelNode.Id).toArray();
            for (var j in twoLevelNodes) {
                var twoLevelNode = twoLevelNodes[j];
                hasChild = Enumerable.from<any>(leftMenuList).where(a => a.ParentId == twoLevelNode.Id).any();
                urldata = twoLevelNode.UrlData ? JSON.parse(twoLevelNode.UrlData) : null;
                var twoLevelNodeElement = $('<li><a data-index="' + twoLevelNode.Id + '" ' + (!hasChild ? 'class="J_menuItem"' : '') + ' href="' + (hasChild ? '#' : urlParamObj(twoLevelNode.Url, urldata)) + '">' + twoLevelNode.Name + (hasChild ? '<span class="fa arrow"></span>' : '') + '</a></li>').appendTo(twoLevelContainer);
                if (hasChild) {
                    var threeLevelContainer = $('<ul class="nav nav-third-level"></ul>').appendTo(twoLevelNodeElement);
                    var threeLevelNodes = Enumerable.from<any>(leftMenuList).where(a => a.ParentId == twoLevelNode.Id).toArray();
                    for (var k in threeLevelNodes) {
                        var threeLevelNode = threeLevelNodes[k];
                        urldata = threeLevelNode.UrlData ? JSON.parse(threeLevelNode.UrlData) : null;
                        var threeLevelNodeElement = $('<li><a data-index="' + threeLevelNode.Id + '" class="J_menuItem" href="' + urlParamObj(threeLevelNode.Url, urldata) + '">' + threeLevelNode.Name + '</a></li>').appendTo(threeLevelContainer);
                    }
                }
            }
        }
    }
    $(".J_menuItem").on("click", (window as any).J_menuItem_click);
}

$(async function () {
    var mainUrl = $.cookie('mainUrl');
    $.cookie('mainUrl', '', { expires: -1 });

    $('.J_menuTabs a').data('id', mainUrl);
    $('.J_mainContent iframe').data('id', mainUrl).attr('src', mainUrl);

    (window as any).timerHeartbeat = setInterval(Heartbeat, 200000);

    var checkPermissionResult = await Call(AdminWebApi.Permission.CheckPermission, { url: window.location.href });
    if (checkPermissionResult.Error) {
        $.messager.alert({
            title: '错误',
            msg: "验证权限错误：" + GetMessage(checkPermissionResult),
            icon: 'error',
            fn: function () {
                window.location.href = '/login.html';
            }
        });
        return;
    } else {
        if (checkPermissionResult.Data != null && !checkPermissionResult.Data) {
            $.messager.alert({
                title: '错误',
                msg: "无权访问。",
                icon: 'error',
                fn: function () {
                    window.location.href = '/login.html';
                }
            });
            return;
        }
    }

    var currentUser = await Call(AdminWebApi.System.GetCurrentUser);
    if (currentUser.Error) {
        $.messager.alert({
            title: '错误',
            msg: "加载当前用户出错：" + GetMessage(currentUser),
            icon: 'error'
        });
    } else {
        Application.User = currentUser.Data
        var user = Application.User.User;
        var groups = Application.User.UserGroups;

        if (!user.UserInfo) {
            user.UserInfo = {};
        }
        var userHeadsrc = user.UserInfo.EmployeeImg ?  user.UserInfo.EmployeeImg : user.UserInfo.Sex == '女' ? '/Image/timg2.jpg' : '/Image/timg1.jpg';
        $('#user_Head').attr('src', userHeadsrc);
        $('#user_Name').html(user.Name ? user.Name : user.Account);
        $('#user_Group').html(groups.length ? groups[0].Name : '无');
    }


    Application.SystemList = await Call(AdminWebApi.System.GetSystemList);

    if (Application.SystemList.Error) {
        $.messager.alert({
            title: '错误',
            msg: "加载系统列表出错：" + GetMessage(Application.SystemList),
            icon: 'error'
        });
    } else {

        Application.SystemId = parseInt(getParam('sid'));

        if (!Application.SystemId) {
            Application.SystemId = Enumerable.from<any>(Application.SystemList).firstOrDefault({}).Id;
        }

        var currentSystem = Enumerable.from<any>(Application.SystemList).where(a => a.Id == Application.SystemId).firstOrDefault({});

        mainUrl = mainUrl ? mainUrl : (currentSystem.Url + '').trim() ? currentSystem.Url : '/welcome.html';

        $('.J_menuTabs a').data('id', mainUrl);
        $('.J_mainContent iframe').data('id', mainUrl).attr('src', mainUrl);

        $('.logo-title').html('<i class="' + (currentSystem.Icon ? currentSystem.Icon : 'fa fa-folder') + '" style="font-size:18px;"></i> ' + currentSystem.Name + ' <i class="fa fa-angle-down" style="font-size:18px;"></i>');
        for (var i in Application.SystemList) {
            var system = Application.SystemList[i];
            $('#SystemList').append($('<li><a href="#" onclick="GoToSystem(' + system.Id + ');return false;"><i class="' + (system.Icon ? system.Icon : 'fa fa-folder') + '"></i> ' + system.Name + '</a></li>'));
        }
    }

    await LoadLeftMenu();

    (window as any).hplusInit();
});

function OpenUpdatePass() {
    $('#dialogPass').dialog('open').dialog('setTitle', '修改密码');
}
async function SavePass() {
    var model = $("#dialog-form").serializeJSON();
    var result = await Call(AdminWebApi.User.UpdatePass.UrlData(model));
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
}

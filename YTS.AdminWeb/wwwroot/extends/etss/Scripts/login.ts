if (window.top !== window.self) {
    window.top.location.href = window.location.href;
}
if (AdminWebApi.UserToken) {
    login();
}

async function login() {
    var defaultSystemId = await Call(AdminWebApi.System.GetDefaultSystemId);

    if (!defaultSystemId) {
        $.messager.alert({
            title: '错误',
            msg: "您无权访问该系统。",
            icon: 'error'
        });
        await logoutLogin();
        return;
    }
    if (defaultSystemId.Error) {
        $.messager.alert({
            title: '错误',
            msg: "获取默认系统时出错：" + GetMessage(defaultSystemId),
            icon: 'error'
        });
        await logoutLogin();
        return;
    }

    var mainUrl = getParam('url');
    if (mainUrl) {
        $.cookie('mainUrl', mainUrl, { path: '/' });
    }

    var url = '/index.html';
    url = urlParam(url, 'sid', defaultSystemId);

    window.location.href = url;
}

async function logoutLogin() {
    var usertoken = AdminWebApi.UserToken;
    var result = await Call(AdminWebApi.User.Logout.UrlData({ UserToken: usertoken }));
    AdminWebApi.UserToken = null;

    usertoken = ShopWebApi.UserToken;
    result = await Call(ShopWebApi.User.Logout.UrlData({ UserToken: usertoken }));
    ShopWebApi.UserToken = null;
}

$(function () {
    $('#login_submit').click(function () {
        (async function () {
            if ($("#login_Account").val()=="") {
                $.messager.alert({
                    title: '错误',
                    msg: '用户名不能为空!',
                    icon: 'error'
                });
                return;
            }
            if ($("#login_Password").val() == "") {
                $.messager.alert({
                    title: '错误',
                    msg: '密码不能为空!',
                    icon: 'error'
                });
                return;
            }
            $.messager.progress({
                noheader: true,
                title: false,
                msg: '页面加载中...',
                interval: 100
            });
            var loginModel = $('#login').serializeJSON();
            var resultAdminLogin = await Call(AdminWebApi.User.Login.UrlData(loginModel));
            $.messager.progress('close');
            if (resultAdminLogin.Error) {
                $.messager.alert({
                    title: '错误',
                    msg: GetMessage(resultAdminLogin),
                    icon: 'error'
                });
                return;
            }
            AdminWebApi.UserToken = resultAdminLogin.Data.UserToken;

            loginModel.SystemNameSpace = "ErTuiShengShi.ShopWeb";
            var resultShopLogin = await Call(ShopWebApi.User.Login.UrlData(loginModel));
           

            if (resultShopLogin.Error) {
                $.messager.alert({
                    title: '错误',
                    msg: GetMessage(resultShopLogin),
                    icon: 'error'
                });
                //await logoutLogin();
                //return;
            } else {
                ShopWebApi.UserToken = resultShopLogin.Data.UserToken;
            }

            login();
        })();
        return false;
    });
});

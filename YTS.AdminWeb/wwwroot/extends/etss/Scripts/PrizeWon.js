$(document).ready(function () {
    //$("#Won").show();
    //$("#NotWon").show();
    //判断是否中奖
    checkisScan();
    var href = window.location.href;
    var Openid = getParam("Openid");

    $("#ReceivePrize").click(function () {
        $(".submit_info p").text("");
        var height1 = window.screen.height;
        var width1 = window.screen.width;
        $("#fade").css({
            height: height1,
            width: width1,
            display: "block"
        });
        $("#light").slideDown();
    });
    $("#close").click(function () {
        $("#light").slideUp();
        $("#fade").fadeOut();
    });
    $(".btn_Submit").click(function () {
        Submit();
    });
});
function Submit() {
    //校验数据
    var url = window.location.href;
    var Model = $('#Submit_data').serializeJSON();
    console.log(Model);
    if (Model.UserName == "") {
        $(".submit_info p").text("姓名不能为空");
        return;
    }
    if (Model.Phone == "") {
        $(".submit_info p").text("手机号不能为空");
        return;
    }

    var myreg = /^[1][3,4,5,7,8,9][0-9]{9}$/;
    if (!myreg.test(Model.Phone)) {
        $(".submit_info p").text("请输入合法的手机号");
        return;
    }

    if (Model.ssq == "") {
        $(".submit_info p").text("省市区不能为空");
        return;
    }

    if (Model.Address == "") {
        $(".submit_info p").text("地址不能为空");
        return;
    }

    Model.Address = Model.ssq + Model.Address;
    var that = this.Openid;
    console.log(Model);


    $.ajax({
        type: "POST",
        url: "http://localhost:52008/api/PrizeRecord/Update?QRCodeUrl=" + url + "&Openid=" + that,
        data: Model,
        dataType: "json",
        success: function (data) {
            console.log(data);
            if (!data.IsShowError) {
                $("#light").slideUp();
                $("#fade").fadeOut();
                window.location.href = window.location.protocol + "//" + window.location.host + "/SystemMeetingManage/PrizeSubmitSuccessfully.html";
            } else {
                $(".submit_info p").text("提交失败");
                console.log("提交失败");
            }
        }
    });

}
function getParam(paramName) {
    if (window.location.search.indexOf("?") == 0 && window.location.search.indexOf("=") > 1) {
        var querystring = window.location.search.substring(1);
        var query = parseQuery(querystring);
        return Enumerable.from < any > (query).where(a => a.key.toLowerCase() == paramName.toLowerCase()).select(a => a.value).firstOrDefault();
    }
    return null;
}
function checkisScan() {
    var url = window.location.href;
    var that = this.Openid;

    $.ajax({
        type: "GET",
        url: "http://localhost:52008/api/PrizeRecord/CheckIsScan?QRCodeUrl=" + url,
        dataType: "json",
        success: function (data) {
            console.log(data);
            if (data.IsShowError) {
                //未中奖
                //跳转界面
                console.log("已经被扫描过了");
                $("#IsScan").show();
                return;
            } else {

                checkWon();
            }
        }
    });
}
function checkWon() {
    var url = window.location.href;
    var that = this.Openid;

    $.ajax({
        type: "GET",
        url: "http://localhost:52008/api/PrizeRecord/UpdateIsUse?QRCodeUrl=" + url,
        dataType: "json",
        success: function (data) {
        }
    });

    $.ajax({
        type: "GET",
        url: "http://localhost:52008/api/PrizeRecord/CheckIsPrize?QRCodeUrl=" + url,
        dataType: "json",
        success: function (data) {
            console.log(data);
            if (data.IsShowError) {
                console.log("未中奖");
                $("#NotWon").show();
            } else {
                //改变中奖内容的描述
                console.log(data.Data.PrizeName);
                $("#prize_txt").text("恭喜你获得" + data.Data.PrizeName);
                $("#prize_txt2").text("奖励" + data.Data.PrizeRemark);
                $("#Won").show();
            }
        }
    });
}

function ObjectSplitPoint(obj) {
    var result = {};
    for (var key in obj) {
        var value = obj[key];
        var array = key.split('.');
        var tempobj = result;
        for (var i = 0; i < array.length; i++) {
            var item = array[i];
            if (i == array.length - 1) {
                tempobj[item] = value;
            }
            else {
                if (!Enumerable.from(Object.keys(tempobj)).contains(item)) {
                    tempobj[item] = {};
                }
                tempobj = tempobj[item];
            }
        }
    }
    return result;
}
function ObjectJoinPoint(obj) {
    var result = {};
    var tempArray = [];
    for (var key in obj) {
        var value = obj[key];
        tempArray[tempArray.length] = { key: key, value: value, path: key, level: 1 };
    }
    for (var i = 0; i < tempArray.length; i++) {
        var item = tempArray[i];
        if (typeof item.value == 'object') {
            for (var key in item.value) {
                var value = item.value[key];
                tempArray[tempArray.length] = { key: key, value: value, path: item.path + '.' + key, level: item.level + 1 };
            }
        }
        else {
            result[item.path] = item.value;
        }
    }
    return result;
}
function GetMessage(data) {
    if (!data) {
        return null;
    }
    var message = data.Message;
    if (data.ExceptionMessage) {
        message = getInnerMessage(data);
    }
    return message;
}
function getInnerMessage(data) {
    if (!data) {
        return null;
    }
    var message = data.ExceptionMessage;
    while (message && (message.indexOf('内部异常') > 0 || message.indexOf("inner exception") > 0)) {
        data = data.InnerException;
        message = data.ExceptionMessage;
    }
    return message;
}
function Call(api, data) {
    return new Promise(function (resolve, reject) {
        if (api.Method == 'post') {
            var body = data ? JSON.stringify(data) : null;
            $.ajax({
                type: "POST",
                url: api.Url,
                data: body,
                contentType: 'application/json',
                success: function (result) {
                    if (!(result instanceof Array)) {
                        result.Error = result ? result.ErrorCode : false;
                    }
                    resolve(result);
                },
                error: function (result) {
                    var json = $.extend({}, result.responseJSON);
                    json.Error = result.status;
                    if (result.status == 0) {
                        json.Error = -1;
                        json.Message = '连接服务器出错。';
                    }
                    resolve(json);
                }
            });
        }
        else if (api.Method == 'get') {
            $.ajax({
                type: "GET",
                url: api.Url,
                data: data,
                success: function (result) {
                    if (!(result instanceof Array)) {
                        result.Error = result ? result.ErrorCode : false;
                    }
                    resolve(result);
                },
                error: function (result) {
                    var json = $.extend({}, result.responseJSON);
                    json.Error = result.status;
                    if (result.status == 0) {
                        json.Error = -1;
                        json.Message = '连接服务器出错。';
                    }
                    resolve(json);
                }
            });
        }
    });
}
function parseQuery(query) {
    var reg = /([^=&\s#]+)=([^&\s#]*)/gi;
    var array = [];
    while (reg.exec(query)) {
        array[array.length] = { key: RegExp.$1, value: decodeURIComponent(RegExp.$2) };
    }
    return array;
}
function parseObjectToQueryList(obj) {
    var array = [];
    if (obj == null) {
        return array;
    }
    var keys = Object.keys(obj);
    for (var i in keys) {
        var key = keys[i];
        if (obj[key] == undefined) {
            obj[key] = '';
        }
        var value = obj[key];
        if (value instanceof Array) {
            for (var j in value) {
                array[array.length] = { key: key, value: value[j] };
            }
        }
        else {
            array[array.length] = { key: key, value: value };
        }
    }
    return array;
}
function urlParamObj(url, obj) {
    if (!obj) {
        return url;
    }
    var result = url;
    for (var key in obj) {
        var value = obj[key];
        result = urlParam(result, key, value);
    }
    return result;
}
function urlParam(url, key, value) {
    if (value == undefined || value == NaN || value == null) {
        value = '';
    }
    if (value instanceof Array) {
        var num2 = url.indexOf("?");
        if (num2 < 0) {
            url = url + "?";
        }
        url = url + $.map(value, function (v) { return key + "=" + v; }).join('&');
        return url;
    }
    var str = encodeURIComponent(value);
    var index = url.indexOf("#");
    var str2 = "";
    if (index > -1) {
        str2 = url.substring(index);
        url = url.substring(0, index - 1);
    }
    var num2 = url.indexOf("?");
    if (num2 < 0) {
        url = url + "?" + key + "=" + str;
    }
    else if (num2 == (url.length - 1)) {
        url = url + key + "=" + str;
    }
    else {
        var regex = new RegExp(key + "=[^\\s&#]*", "gi");
        if (regex.test(url)) {
            url = url.replace(regex, key + "=" + str);
        }
        else {
            var str4 = url;
            url = str4 + "&" + key + "=" + str;
        }
    }
    return (url + str2);
}
function getParamForUrl(url, paramName) {
    if (url.indexOf("?") > 0 && url.indexOf("=") > 1) {
        var querystring = url.substring(url.indexOf("?") + 1);
        var query = parseQuery(querystring);
        return Enumerable.from(query).where(function (a) { return a.key.toLowerCase() == paramName.toLowerCase(); }).select(function (a) { return a.value; }).firstOrDefault();
    }
    return null;
}
function getParam(paramName) {
    if (window.location.search.indexOf("?") == 0 && window.location.search.indexOf("=") > 1) {
        var querystring = window.location.search.substring(1);
        var query = parseQuery(querystring);
        return Enumerable.from(query).where(function (a) { return a.key.toLowerCase() == paramName.toLowerCase(); }).select(function (a) { return a.value; }).firstOrDefault();
    }
    return null;
}
function dateUtcFormat(date, fmt) {
    var o = {
        "M+": date.getUTCMonth() + 1,
        "d+": date.getUTCDate(),
        "h+": date.getUTCHours() - (date.getUTCHours() > 12 ? 12 : 0),
        "H+": date.getUTCHours(),
        "m+": date.getUTCMinutes(),
        "s+": date.getUTCSeconds(),
        "q+": Math.floor((date.getUTCMonth() + 3) / 3),
        "f+": date.getUTCMilliseconds() //毫秒
    };
    if (/(y+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, (date.getUTCFullYear() + "").substr(4 - RegExp.$1.length));
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        }
    }
    return fmt;
}
;
function dateFormat(date, fmt) {
    var o = {
        "M+": date.getMonth() + 1,
        "d+": date.getDate(),
        "h+": date.getHours() - (date.getHours() > 12 ? 12 : 0),
        "H+": date.getHours(),
        "m+": date.getMinutes(),
        "s+": date.getSeconds(),
        "q+": Math.floor((date.getMonth() + 3) / 3),
        "f+": date.getMilliseconds() //毫秒
    };
    if (/(y+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, (date.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        }
    }
    return fmt;
}
;
//加法函数，用来得到精确的加法结果
//说明：javascript的加法结果会有误差，在两个浮点数相加的时候会比较明显。这个函数返回较为精确的加法结果。
//调用：accAdd(arg1,arg2)
//返回值：arg1加上arg2的精确结果
function accAdd(arg1, arg2) {
    var r1, r2, m;
    try {
        r1 = arg1.toString().split(".")[1].length;
    }
    catch (e) {
        r1 = 0;
    }
    try {
        r2 = arg2.toString().split(".")[1].length;
    }
    catch (e) {
        r2 = 0;
    }
    m = Math.pow(10, Math.max(r1, r2));
    return (arg1 * m + arg2 * m) / m;
}
//乘法函数，用来得到精确的乘法结果
//说明：javascript的乘法结果会有误差，在两个浮点数相乘的时候会比较明显。这个函数返回较为精确的乘法结果。
//调用：accMul(arg1,arg2)
//返回值：arg1乘以arg2的精确结果
function accMul(arg1, arg2) {
    var m = 0, s1 = arg1.toString(), s2 = arg2.toString();
    try {
        m += s1.split(".")[1].length;
    }
    catch (e) { }
    ;
    try {
        m += s2.split(".")[1].length;
    }
    catch (e) { }
    ;
    return Number(s1.replace(".", "")) * Number(s2.replace(".", "")) / Math.pow(10, m);
    //var s = "";
    //var ml = Math.pow(10, m) + "";
    //if (ml.length > 1) {
    //    s = pad(Number(s1.replace(".", "")) * Number(s2.replace(".", "")), ml.length).toString();
    //     s1 = s.substr(0, s.length - (ml.length - 1));
    //     s2 = s.substring(s.length - (ml.length - 1));
    //    s = s1 + "." + s2;
    //} else {
    //    s = (Number(s1.replace(".", "")) * Number(s2.replace(".", ""))).toString();
    //}
    //return s;
}
function pad(num, n) {
    var tbl = [];
    var len = n - num.toString().length;
    if (len <= 0)
        return num;
    if (!tbl[len])
        tbl[len] = (new Array(len + 1)).join('0');
    return tbl[len] + num;
}

function addDays(date, number) {
    var adjustDate = new Date(date.getTime() + 24 * 60 * 60 * 1000 * number);
    return adjustDate;
}

function cloneObject(obj) {
    // Handle null or undefined or function
    if (null == obj || "object" != typeof obj)
        return obj;
    // Handle the 3 simple types, Number and String and Boolean
    if (obj instanceof Number || obj instanceof String || obj instanceof Boolean)
        return obj.valueOf();
    // Handle Date
    if (obj instanceof Date) {
        var copy = new Date();
        copy.setTime(obj.getTime());
        return copy;
    }
    // Handle Array or Object
    if (obj instanceof Object || obj instanceof Array) {
        var copy1 = (obj instanceof Array) ? [] : {};
        for (var attr in obj) {
            if (obj.hasOwnProperty(attr))
                copy1[attr] = obj[attr] ? cloneObject(obj[attr]) : obj[attr];
        }
        return copy1;
    }
    throw new Error("Unable to clone obj! Its type isn't supported.");
}
var ApiItem = /** @class */ (function () {
    function ApiItem(url, method) {
        this.Url = url;
        this.Method = method;
    }
    ApiItem.prototype.UrlData = function (urldata) {
        var api = cloneObject(this);
        var url = api.Url;
        if (urldata) {
            for (var i in urldata) {
                url = urlParam(url, i, urldata[i]);
            }
        }
        api.Url = url;
        return api;
    };
    return ApiItem;
}());
var Application = /** @class */ (function () {
    function Application() {
    }
    Object.defineProperty(Application, "SystemId", {
        get: function () {
            return top.window.SystemId;
        },
        set: function (value) {
            top.window.SystemId = value;
        },
        enumerable: true,
        configurable: true
    });
    return Application;
}());
//# sourceMappingURL=common.js.map

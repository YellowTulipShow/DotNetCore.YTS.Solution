/* 原生支持 JSON */
(function() {
    /* https://developer.mozilla.org/zh-CN/docs/Web/JavaScript/Reference/Global_Objects/JSON */
    if (!window.JSON) {
        window.JSON = {
            parse: function(sJSON) {
                return eval('(' + sJSON + ')');
            },
            stringify: (function() {
                var toString = Object.prototype.toString;
                var isArray = Array.isArray || function(a) {
                    return toString.call(a) === '[object Array]';
                };
                var escMap = { '"': '\\"', '\\': '\\\\', '\b': '\\b', '\f': '\\f', '\n': '\\n', '\r': '\\r', '\t': '\\t' };
                var escFunc = function(m) {
                    return escMap[m] || '\\u' + (m.charCodeAt(0) + 0x10000).toString(16).substr(1);
                };
                var escRE = /[\\"\u0000-\u001F\u2028\u2029]/g;
                return function stringify(value) {
                    if (value == null) {
                        return 'null';
                    } else if (typeof value === 'number') {
                        return isFinite(value) ? value.toString() : 'null';
                    } else if (typeof value === 'boolean') {
                        return value.toString();
                    } else if (typeof value === 'object') {
                        if (typeof value.toJSON === 'function') {
                            return stringify(value.toJSON());
                        } else if (isArray(value)) {
                            var res = '[';
                            for (var i = 0; i < value.length; i++) res += (i ? ', ': '') + stringify(value[i]);
                            return res + ']';
                        } else if (toString.call(value) === '[object Object]') {
                            var tmp = [];
                            for (var k in value) {
                                if (value.hasOwnProperty(k)) tmp.push(stringify(k) + ': ' + stringify(value[k]));
                            }
                            return '{' + tmp.join(', ') + '}';
                        }
                    }
                    return '"' + value.toString().replace(escRE, escFunc) + '"';
                };
            })()
        };
    }
})();

/* 字符串 String */
(function() {
    String.prototype.format = function(args) {
        // js中的string.format
        // http://www.cnblogs.com/loogn/archive/2011/06/20/2085165.html
        var result = this;
        if (arguments.length > 0) {
            if (arguments.length == 1 && typeof (args) == "object") {
                for (var key in args) {
                    if(args[key]!=undefined){
                        var reg = new RegExp("({" + key + "})", "g");
                        result = result.replace(reg, args[key]);
                    }
                }
            }
            else {
                for (var i = 0; i < arguments.length; i++) {
                    if (arguments[i] != undefined) {
                        var reg= new RegExp("({)" + i + "(})", "g");
                        result = result.replace(reg, arguments[i]);
                    }
                }
            }
        }
        return result;
    }
    String.prototype.trimStart = function(symbol) {
        symbol = symbol || "\\s+"
        var pattern = "^{0}".format(symbol);
        var re = new RegExp(pattern, "g");
        return this.replace(re, "");
    }
    String.prototype.trimEnd = function(symbol) {
        symbol = symbol || "\\s+"
        var pattern = "{0}$".format(symbol);
        var re = new RegExp(pattern, "g");
        return this.replace(re, "");
    }
    String.prototype.trim = function(symbol) {
        return this.trimStart(symbol).trimEnd(symbol);
    }
})();

/* 对象 Object */
(function() {
    Object.get = function(object, name, default_value) {
        if (arguments.length <= 0) {
            return null;
        }
        object = object || {};
        name = name || {};
        default_value = default_value || null;
        name = name.toString();
        var value = object[name];
        return value ? value : default_value;
    };
})();

/* 扩展 Date 时间 */
(function() {
    Date.prototype.FormatAsString = function(format) {
        if (arguments.length <= 0) {
            format = 'yyyy-MM-dd HH:mm:ss';
        }

        var o = {
            "M+" : this.getMonth()+1, //month
            "d+" : this.getDate(), //day
            "[hH]+" : this.getHours(), //hour
            "m+" : this.getMinutes(), //minute
            "s+" : this.getSeconds(), //second
            "q+" : Math.floor((this.getMonth()+3)/3), //quarter
            "S" : this.getMilliseconds() //millisecond
        }
        if(/(y+)/.test(format)) {
            format=format.replace(RegExp.$1, (this.getFullYear()+"").substr(4 - RegExp.$1.length));
        }
        for(var k in o) {
            if (new RegExp("(" + k + ")").test(format)) {
                format = format.replace(RegExp.$1, RegExp.$1.length==1 ? o[k] : ("00"+ o[k]).substr((""+ o[k]).length));
            }
        }
        return format;
    }
    Date.prototype.GetNormalValue = function() {
        return {
            Year: this.getFullYear(),
            Month: this.getMonth() + 1, // especially!
            Day: this.getDate(), // // especially!
            Hour: this.getHours(),
            Minute: this.getMinutes(),
            Second: this.getSeconds(),
            Millisecond: this.getMilliseconds(),
        };
    }
    Date.prototype.AppendValueClone = function(matchFmt, diffValue) {
        matchFmt = matchFmt || 'TTT';
        var v = this.GetNormalValue();
        diffValue = (!diffValue || isNaN(diffValue)) ? 0 : parseInt(diffValue);
        switch(matchFmt) {
            case 'yyyy': v.Year += diffValue; break;
            case 'MM': v.Month += diffValue; break;
            case 'dd': v.Day += diffValue; break;
            case 'HH':
            case 'hh': v.Hour += diffValue; break;
            case 'mm': v.Minute += diffValue; break;
            case 'ss': v.Second += diffValue; break;
        }
        return window.CommonData.CreateDateTime(v.Year, v.Month, v.Day, v.Hour, v.Minute, v.Second, v.Millisecond);
    }
    Date.prototype.AppendValue = function(matchFmt, diffValue) {
        if (isNaN(diffValue))
            return this;
        switch(matchFmt) {
            case 'yyyy': this.setFullYear(this.getFullYear() + Number(diffValue)); break;
            case 'MM': this.setMonth(this.getMonth() + Number(diffValue)); break;
            case 'dd': this.setDate(this.getDate() + Number(diffValue)); break;
            case 'HH':
            case 'hh': this.setHours(this.getHours() + Number(diffValue)); break;
            case 'mm': this.setMinutes(this.getMinutes() + Number(diffValue)); break;
            case 'ss': this.setSeconds(this.getSeconds() + Number(diffValue)); break;
        }
        return this;
    }
})();

/* 扩展 Array 数组 */
(function() {
    /* js 删除数组几种方法 (大神解决方案) */
    /* http://www.cnblogs.com/qiantuwuliang/archive/2010/09/01/1814706.html */
    Array.prototype.del = function(n) {
        return n < 0 ? this : this.slice(0, n).concat(this.slice(n + 1, this.length));
    }
})();

/* 静态 检查数据 */
(function() {
    window.CheckData = {
        IsUndefined: function(obj) {
            //获得undefined，保证它没有被重新赋值
            var undefined = void(0);
            return obj === undefined;
        },
        IsObjectNull: function(obj) {
            return this.IsUndefined(obj) || obj == null;
        },
        IsStringNull: function(str) {
            return this.IsObjectNull(str) || str.toString() == "" || str.toString() == '';
        },
        IsChinaChar: function(value) {
            return /.*[\u4e00-\u9fa5]+.*$/.test(value);
        },
        IsSizeEmpty: function(array) {
            return this.IsObjectNull(array) || !array.length || array.length <= 0;
        },
    };
})();

/* 静态 页面信息 */
(function() {
    window.PageInfo = {
        Width: function () {
            if (window.innerWidth) {
                return window.innerWidth;
            }
            if ((document.body) && (document.body.clientWidth)) {
                return document.body.clientWidth;
            }
            if (document.documentElement && document.documentElement.clientWidth) {
                return document.documentElement.clientWidth;
            }
            return 0;
        },
        Height: function () {
            if (window.innerHeight) {
                return window.innerHeight;
            }
            if ((document.body) && (document.body.clientHeight)) {
                return document.body.clientHeight;
            }
            if (document.documentElement && document.documentElement.clientHeight) {
                return document.documentElement.clientHeight;
            }
            return 0;
        },
        QueryString: function(name, location_search_string) {
            if (window.CheckData.IsStringNull(location_search_string)) {
                location_search_string = window.location.search;
            }
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = location_search_string.substr(1).match(reg);
            return r != null ? decodeURI(r[2]) : null;
        },
        RenderingHTML: function(jqElement, jsonobj) {
            // 功能: 渲染生成 HTML
            // 参考学习:
            // http://www.thinkphp.cn/code/1901.html
            // http://www.cnblogs.com/lori/archive/2012/08/31/2665802.html
            //i g m是指分别用于指定区分大小写的匹配、全局匹配和多行匹配。
            var reg = new RegExp("\\[([^\\[\\]]*?)\\]", 'igm');
            jqElement = $(jqElement)[0];
            var html = jqElement.innerHTML;
            var source = html.replace(reg, function (node, key) {
                return jsonobj[key];
            });
            return source;
        },
        BorwserType: function () {
            // 智能机浏览器版本信息:
            var browser = {
                versions: function () {
                    var u = window.navigator.userAgent;
                    var app = window.navigator.appVersion;
                    //移动终端浏览器版本信息
                    return {
                        trident: u.indexOf('Trident') > -1, //IE内核
                        presto: u.indexOf('Presto') > -1, //opera内核
                        webKit: u.indexOf('AppleWebKit') > -1, //苹果、谷歌内核
                        gecko: u.indexOf('Gecko') > -1 && u.indexOf('KHTML') == -1, //火狐内核
                        mobile: !!u.match(/AppleWebKit.*Mobile.*/) || !!u.match(/AppleWebKit/), //是否为移动终端
                        ios: !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/), //ios终端
                        android: u.indexOf('Android') > -1 || u.indexOf('Linux') > -1, //android终端或者uc浏览器
                        iPhone: u.indexOf('iPhone') > -1 || u.indexOf('Mac') > -1, //是否为iPhone或者QQHD浏览器
                        iPad: u.indexOf('iPad') > -1, //是否iPad
                        webApp: u.indexOf('Safari') == -1, //是否web应该程序，没有头部与底部
                        wechatbrowser: u.toLowerCase().match(/MicroMessenger/i) == 'micromessenger', // 是否微信浏览器
                    };
                }(),
                language: (navigator.browserLanguage || navigator.language).toLowerCase()
            }
            if (browser.versions.mobile ||
                browser.versions.android ||
                browser.versions.iPhone ||
                browser.versions.iPad) {
                return "Mobile";
            } else {
                return "PC";
            }
        },
        BorwserVersion: function () {
            var u = window.navigator.userAgent;
            var uStr = u.toString();
            var uStr_lower = uStr.toLowerCase();

            if (uStr_lower.indexOf('micromessenger') >= 0) {
                return 'WeChatBrowser';
            }
            if (uStr_lower.indexOf('baidubrowser') >= 0) {
                return 'BaiduBrowser';
            }
            if (uStr_lower.indexOf('baiduboxapp') >= 0) {
                return 'BaiduBoxApp';
            }
            return "Unrecognized"; // 无法识别
        },
        ToHttpGETParameter: function(object) {
            var strs = [];
            for (var key in object) {
                var value = encodeURIComponent(object[key]);
                strs.push(key + "=" + value);
            }
            return strs.join('&');
        },
    };
})();

/* 静态 常用数据 */
(function() {
    window.CommonData = {
        MaxDayCount: function(year, month) {
            if (month == 2) {
                var calc_num = year % 100 == 0 ? 400 : 4;
                return (year % calc_num == 0) ? 29 : 28;
            }
            return (month <= 7 ? month : month + 1) % 2 == 1 ? 31 : 30;
        },
        CreateDateTime: function(year, month, day, hour, minute, second, millisecond) {
            var year = year || 1;
            var month = month || 1;
            var day = day || 1;
            var hour = hour || 0;
            var minute = minute || 0;
            var second = second || 0;
            var millisecond = millisecond || 0;
            return new Date(year, month - 1, day, hour, minute, second, millisecond);
        },
        ASCII_Number: function() {
            return ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
        },
        ASCII_UpperEnglish: function() {
            return ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
        },
        ASCII_LowerEnglish: function() {
            return ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'];
        },
        ASCII_Special: function() {
            return ['!', '"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/', ':', ';', '<', '=', '>', '?', '@', '[', '\\', ']', '^', '_', '`', '{', '|', '}', '~'];
        },
        ASCII_WordText: function() {
            var self = this;
            var arr_number = self.ASCII_Number();
            var arr_en_upper = self.ASCII_UpperEnglish();
            var arr_en_lower = self.ASCII_LowerEnglish();
            return arr_number.concat(arr_en_upper, arr_en_lower);
        },
        ASCII_ALL: function() {
            var self = this;
            var arr_number = self.ASCII_Number();
            var arr_en_upper = self.ASCII_UpperEnglish();
            var arr_en_lower = self.ASCII_LowerEnglish();
            var arr_special = self.ASCII_Special();
            return arr_number.concat(arr_en_upper, arr_en_lower, arr_special);
        },
        ASCII_Hexadecimal: function() {
            return ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'];
        },
    };
})();

/* 静态 随机数据 */
(function() {
    window.RandomData = {
        ID: function() {
            function S4() {
                return (((1+Math.random())*0x10000)|0).toString(16).substring(1);
            }
            var self = this;
            var str = S4();
            var ki = 4;
            for (var i = 0; i < 6; i++) {
                if (self.Int(0, 2) === 0 && ki > 0) {
                    str += '-';
                    ki -= 1;
                }
                str += S4();
            }
            return str;
        },
        Item: function(array) {
            if (window.CheckData.IsSizeEmpty(array)) {
                return null;
            }
            return array[this.Int(0, array.length)];
        },
        Int: function(min, max) {
            if (min > max) {
                var zhong = min;
                min = max;
                max = zhong;
            }
            max = parseInt(max) - 1;
            switch(arguments.length) {
                case 1: return parseInt(Math.random() * min + 1);
                case 2: return parseInt(Math.random() * (max - min + 1) + min);
                default: return 0;
            }
        },
        DateTime: function(min, max) {
            if (min > max) {
                var zhong = min;
                min = max;
                max = zhong;
            }
            var min_val_obj = min.GetNormalValue();
            var max_val_obj = max.GetNormalValue();
            var obj = {
                result: 0,
                upstatue: 0,
            };
            function TimeRangeSelect(obj, min, max, start, end) {
                if (obj.upstatue == 4) {
                    obj.result = this.Int(min, max);
                } else {
                    var minvalue = (obj.upstatue == 3) ? min : start;
                    var maxvalue = (obj.upstatue == 2) ? max - 1 : end;
                    if (minvalue > maxvalue) {
                        var zhong = minvalue;
                        minvalue = maxvalue;
                        maxvalue = zhong;
                    }
                    obj.result = this.Int(minvalue, maxvalue + 1);

                    var selfstatus = 0;
                    if (minvalue == obj.result && obj.result == maxvalue) {
                        selfstatus = 1;
                    }
                    if (minvalue == obj.result && obj.result < maxvalue) {
                        selfstatus = (obj.upstatue == 3) ? 4 : 2;
                    }
                    if (minvalue < obj.result && obj.result == maxvalue) {
                        selfstatus = (obj.upstatue == 2) ? 4 : 3;
                    }
                    if (minvalue < obj.result && obj.result < maxvalue) {
                        selfstatus = 4;
                    }
                    obj.upstatue = (selfstatus < obj.upstatue) ? obj.upstatue : selfstatus;
                }
                return obj;
            }
            obj = TimeRangeSelect(obj, 1, 9999 + 1, min_val_obj.Year, max_val_obj.Year);
            var r_Year = obj.result;
            obj = TimeRangeSelect(obj, 1, 12 + 1, min_val_obj.Month, max_val_obj.Month);
            var r_Month = obj.result;
            obj = TimeRangeSelect(obj, 1, window.CommonData.GetMaxDayCount(r_Year, r_Month) + 1, min_val_obj.Day, max_val_obj.Day);
            var r_Day = obj.result;
            obj = TimeRangeSelect(obj, 0, 23 + 1, min_val_obj.Hour, max_val_obj.Hour);
            var r_Hour = obj.result;
            obj = TimeRangeSelect(obj, 0, 59 + 1, min_val_obj.Minute, max_val_obj.Minute);
            var r_Minute = obj.result;
            obj = TimeRangeSelect(obj, 0, 59 + 1, min_val_obj.Second, max_val_obj.Second);
            var r_Second = obj.result;
            obj = TimeRangeSelect(obj, 0, 999 + 1, min_val_obj.Millisecond, max_val_obj.Millisecond);
            var r_Millisecond = obj.result;
            return window.CommonData.CreateDateTime(r_Year, r_Month, r_Day, r_Hour, r_Minute, r_Second, r_Millisecond);
        },
        CombinedString: function(array, strlength) {
            var self = this;
            if (window.CheckData.IsSizeEmpty(array) || arguments.length <= 0) {
                array = window.CommonData.ASCII_WordText();
            }
            if (arguments.length <= 1) {
                strlength = 32;
            }
            var result = new Array();
            for (var i = 0; i < strlength; i++) {
                var index = self.Int(0, array.length);
                var item = array[index].toString();
                result.push(item);
            }
            return result.join('');
        }
    };
})();

/* 静态 转化工具 */
(function() {
    window.ConvertTool = {
        ArrayToString: function(arrayobj, symbol) {
            if (window.CheckData.IsStringNull(symbol)) {
                return '';
            }
            var resustr = '';
            for (var i = 0; i < arrayobj.length; i++) {
                if (window.CheckData.IsStringNull(arrayobj[i])) {
                    continue;
                } else if (i != 0) {
                    resustr += symbol;
                }
                resustr += arrayobj[i];
            }
            return resustr;
        },
        ToInt: function(objvalue, defint) {
            if (arguments.length <= 1) {
                defint = 0;
            }
            var parsed = Number.parseInt(objvalue.toString(), 0);
            if (Number.isNaN(parsed)) {
                return defint;
            }
            return parsed;
        },
        ToDate: function(s_date, format) {
            if (arguments.length <= 1) {
                format = 'yyyy-MM-dd HH:mm:ss';
            }
            if (arguments.length <= 0) {
                return new Date();
            }

            s_date = s_date.replace(/-/g,"/");
            return new Date(s_date);
        },
        getBase64Chars: function (argument) {
            return "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
        },
        UTF8ToBase64: function(base64_string) {
            var gStrs = this.getBase64Chars();
            var output = "";
            var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
            var i = 0;
            var input = this.ToUTF8(base64_string);
            while (i < input.length) {
                chr1 = input.charCodeAt(i++);
                chr2 = input.charCodeAt(i++);
                chr3 = input.charCodeAt(i++);
                enc1 = chr1 >> 2;
                enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
                enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
                enc4 = chr3 & 63;
                if (isNaN(chr2)) {
                    enc3 = enc4 = 64;
                } else if (isNaN(chr3)) {
                    enc4 = 64;
                }
                output +=
                    gStrs.charAt(enc1) +
                    gStrs.charAt(enc2) +
                    gStrs.charAt(enc3) +
                    gStrs.charAt(enc4);
            }
            return output;
        },
        Base64ToUTF8: function (input) {
            var gStrs = this.getBase64Chars();
            var output = "";
            var chr1, chr2, chr3;
            var enc1, enc2, enc3, enc4;
            var i = 0;
            input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");
            while (i < input.length) {
                enc1 = gStrs.indexOf(input.charAt(i++));
                enc2 = gStrs.indexOf(input.charAt(i++));
                enc3 = gStrs.indexOf(input.charAt(i++));
                enc4 = gStrs.indexOf(input.charAt(i++));
                chr1 = (enc1 << 2) | (enc2 >> 4);
                chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
                chr3 = ((enc3 & 3) << 6) | enc4;
                output = output + String.fromCharCode(chr1);
                if (enc3 != 64) {
                    output = output + String.fromCharCode(chr2);
                }
                if (enc4 != 64) {
                    output = output + String.fromCharCode(chr3);
                }
            }
            output = this.UTF8ToText(output);
            return output;
        },
        ToUTF8: function (string) {
            string = string.replace(/\r\n/g,"\n");
            var utftext = "";
            for (var n = 0; n < string.length; n++) {
                var c = string.charCodeAt(n);
                if (c < 128) {
                    utftext += String.fromCharCode(c);
                } else if((c > 127) && (c < 2048)) {
                    utftext += String.fromCharCode((c >> 6) | 192);
                    utftext += String.fromCharCode((c & 63) | 128);
                } else {
                    utftext += String.fromCharCode((c >> 12) | 224);
                    utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                    utftext += String.fromCharCode((c & 63) | 128);
                }
            }
            return utftext;
        },
        UTF8ToText: function (utftext) {
            var string = "";
            var i = 0;
            var c = c1 = c2 = 0;
            while ( i < utftext.length ) {
                c = utftext.charCodeAt(i);
                if (c < 128) {
                    string += String.fromCharCode(c);
                    i++;
                } else if((c > 191) && (c < 224)) {
                    c2 = utftext.charCodeAt(i+1);
                    string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                    i += 2;
                } else {
                    c2 = utftext.charCodeAt(i+1);
                    c3 = utftext.charCodeAt(i+2);
                    string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                    i += 3;
                }
            }
            return string;
        },
    };
})();

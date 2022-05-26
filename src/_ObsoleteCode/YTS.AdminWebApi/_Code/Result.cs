using YTS.Tools;

namespace YTS.WebApi
{
    /// <summary>
    /// HTTP状态结果码
    /// 参考取自: https://www.runoob.com/http/http-status-codes.html
    /// </summary>
    [Explain("HTTP状态结果码")]
    public enum ResultCode
    {
        /**
            1** 	信息，服务器收到请求，需要请求者继续执行操作
            2** 	成功，操作被成功接收并处理
            3** 	重定向，需要进一步的操作以完成请求
            4** 	客户端错误，请求包含语法错误或无法完成请求
            5** 	服务器错误，服务器在处理请求的过程中发生了错误
         */

        /// <summary>
        /// 继续。客户端应继续其请求
        /// </summary>
        [Explain("继续。客户端应继续其请求")]
        Continue = 100,

        /// <summary>
        /// 切换协议。服务器根据客户端的请求切换协议。只能切换到更高级的协议，例如，切换到HTTP的新版本协议
        /// </summary>
        [Explain("切换协议。服务器根据客户端的请求切换协议。只能切换到更高级的协议，例如，切换到HTTP的新版本协议")]
        SwitchingProtocols = 101,

        /// <summary>
        /// 请求成功。一般用于GET与POST请求
        /// </summary>
        [Explain("请求成功。一般用于GET与POST请求")]
        OK = 200,

        /// <summary>
        /// 已创建。成功请求并创建了新的资源
        /// </summary>
        [Explain("已创建。成功请求并创建了新的资源")]
        Created = 201,

        /// <summary>
        /// 已接受。已经接受请求，但未处理完成
        /// </summary>
        [Explain("已接受。已经接受请求，但未处理完成")]
        Accepted = 202,

        /// <summary>
        /// 非授权信息。请求成功。但返回的meta信息不在原始的服务器，而是一个副本
        /// </summary>
        [Explain("非授权信息。请求成功。但返回的meta信息不在原始的服务器，而是一个副本")]
        NonAuthoritativeInformation = 203,

        /// <summary>
        /// 无内容。服务器成功处理，但未返回内容。在未更新网页的情况下，可确保浏览器继续显示当前文档
        /// </summary>
        [Explain("无内容。服务器成功处理，但未返回内容。在未更新网页的情况下，可确保浏览器继续显示当前文档")]
        NoContent = 204,

        /// <summary>
        /// 重置内容。服务器处理成功，用户终端（例如：浏览器）应重置文档视图。可通过此返回码清除浏览器的表单域
        /// </summary>
        [Explain("重置内容。服务器处理成功，用户终端（例如：浏览器）应重置文档视图。可通过此返回码清除浏览器的表单域")]
        ResetContent = 205,

        /// <summary>
        /// 部分内容。服务器成功处理了部分GET请求
        /// </summary>
        [Explain("部分内容。服务器成功处理了部分GET请求")]
        PartialContent = 206,

        /// <summary>
        /// 多种选择。请求的资源可包括多个位置，相应可返回一个资源特征与地址的列表用于用户终端（例如：浏览器）选择
        /// </summary>
        [Explain("多种选择。请求的资源可包括多个位置，相应可返回一个资源特征与地址的列表用于用户终端（例如：浏览器）选择")]
        MultipleChoices = 300,

        /// <summary>
        /// 永久移动。请求的资源已被永久的移动到新URI，返回信息会包括新的URI，浏览器会自动定向到新URI。今后任何新的请求都应使用新的URI代替
        /// </summary>
        [Explain("永久移动。请求的资源已被永久的移动到新URI，返回信息会包括新的URI，浏览器会自动定向到新URI。今后任何新的请求都应使用新的URI代替")]
        MovedPermanently = 301,

        /// <summary>
        /// 临时移动。与301类似。但资源只是临时被移动。客户端应继续使用原有URI
        /// </summary>
        [Explain("临时移动。与301类似。但资源只是临时被移动。客户端应继续使用原有URI")]
        Found = 302,

        /// <summary>
        /// 查看其它地址。与301类似。使用GET和POST请求查看
        /// </summary>
        [Explain("查看其它地址。与301类似。使用GET和POST请求查看")]
        SeeOther = 303,

        /// <summary>
        /// 未修改。所请求的资源未修改，服务器返回此状态码时，不会返回任何资源。客户端通常会缓存访问过的资源，通过提供一个头信息指出客户端希望只返回在指定日期之后修改的资源
        /// </summary>
        [Explain("未修改。所请求的资源未修改，服务器返回此状态码时，不会返回任何资源。客户端通常会缓存访问过的资源，通过提供一个头信息指出客户端希望只返回在指定日期之后修改的资源")]
        NotModified = 304,

        /// <summary>
        /// 使用代理。所请求的资源必须通过代理访问
        /// </summary>
        [Explain("使用代理。所请求的资源必须通过代理访问")]
        UseProxy = 305,

        /// <summary>
        /// 已经被废弃的HTTP状态码
        /// </summary>
        [Explain("已经被废弃的HTTP状态码")]
        Unused = 306,

        /// <summary>
        /// 临时重定向。与302类似。使用GET请求重定向
        /// </summary>
        [Explain("临时重定向。与302类似。使用GET请求重定向")]
        TemporaryRedirect = 307,

        /// <summary>
        /// 客户端请求的语法错误，服务器无法理解
        /// </summary>
        [Explain("客户端请求的语法错误，服务器无法理解")]
        BadRequest = 400,

        /// <summary>
        /// 请求要求用户的身份认证
        /// </summary>
        [Explain("请求要求用户的身份认证")]
        Unauthorized = 401,

        /// <summary>
        /// 保留，将来使用
        /// </summary>
        [Explain("保留，将来使用")]
        PaymentRequired = 402,

        /// <summary>
        /// 服务器理解请求客户端的请求，但是拒绝执行此请求
        /// </summary>
        [Explain("服务器理解请求客户端的请求，但是拒绝执行此请求")]
        Forbidden = 403,

        /// <summary>
        /// 服务器无法根据客户端的请求找到资源（网页）。通过此代码，网站设计人员可设置\"您所请求的资源无法找到\"的个性页面
        /// </summary>
        [Explain("服务器无法根据客户端的请求找到资源（网页）。通过此代码，网站设计人员可设置\"您所请求的资源无法找到\"的个性页面")]
        NotFound = 404,

        /// <summary>
        /// 客户端请求中的方法被禁止
        /// </summary>
        [Explain("客户端请求中的方法被禁止")]
        MethodNotAllowed = 405,

        /// <summary>
        /// 服务器无法根据客户端请求的内容特性完成请求
        /// </summary>
        [Explain("服务器无法根据客户端请求的内容特性完成请求")]
        NotAcceptable = 406,

        /// <summary>
        /// 请求要求代理的身份认证，与401类似，但请求者应当使用代理进行授权
        /// </summary>
        [Explain("请求要求代理的身份认证，与401类似，但请求者应当使用代理进行授权")]
        ProxyAuthenticationRequired = 407,

        /// <summary>
        /// 服务器等待客户端发送的请求时间过长，超时
        /// </summary>
        [Explain("服务器等待客户端发送的请求时间过长，超时")]
        RequestTimeOut = 408,

        /// <summary>
        /// 服务器完成客户端的PUT请求时可能返回此代码，服务器处理请求时发生了冲突
        /// </summary>
        [Explain("服务器完成客户端的PUT请求时可能返回此代码，服务器处理请求时发生了冲突")]
        Conflict = 409,

        /// <summary>
        /// 客户端请求的资源已经不存在。410不同于404，如果资源以前有现在被永久删除了可使用410代码，网站设计人员可通过301代码指定资源的新位置
        /// </summary>
        [Explain("客户端请求的资源已经不存在。410不同于404，如果资源以前有现在被永久删除了可使用410代码，网站设计人员可通过301代码指定资源的新位置")]
        Gone = 410,

        /// <summary>
        /// 服务器无法处理客户端发送的不带Content-Length的请求信息
        /// </summary>
        [Explain("服务器无法处理客户端发送的不带Content-Length的请求信息")]
        LengthRequired = 411,

        /// <summary>
        /// 客户端请求信息的先决条件错误
        /// </summary>
        [Explain("客户端请求信息的先决条件错误")]
        PreconditionFailed = 412,

        /// <summary>
        /// 由于请求的实体过大，服务器无法处理，因此拒绝请求。为防止客户端的连续请求，服务器可能会关闭连接。如果只是服务器暂时无法处理，则会包含一个Retry-After的响应信息
        /// </summary>
        [Explain("由于请求的实体过大，服务器无法处理，因此拒绝请求。为防止客户端的连续请求，服务器可能会关闭连接。如果只是服务器暂时无法处理，则会包含一个Retry-After的响应信息")]
        RequestEntityTooLarge = 413,

        /// <summary>
        /// 请求的URI过长（URI通常为网址），服务器无法处理
        /// </summary>
        [Explain("请求的URI过长（URI通常为网址），服务器无法处理")]
        RequestURITooLarge = 414,

        /// <summary>
        /// 服务器无法处理请求附带的媒体格式
        /// </summary>
        [Explain("服务器无法处理请求附带的媒体格式")]
        UnsupportedMediaType = 415,

        /// <summary>
        /// 客户端请求的范围无效
        /// </summary>
        [Explain("客户端请求的范围无效")]
        Requestedrangenotsatisfiable = 416,

        /// <summary>
        /// 服务器无法满足Expect的请求头信息
        /// </summary>
        [Explain("服务器无法满足Expect的请求头信息")]
        ExpectationFailed = 417,

        /// <summary>
        /// 服务器内部错误，无法完成请求
        /// </summary>
        [Explain("服务器内部错误，无法完成请求")]
        InternalServerError = 500,

        /// <summary>
        /// 服务器不支持请求的功能，无法完成请求
        /// </summary>
        [Explain("服务器不支持请求的功能，无法完成请求")]
        NotImplemented = 501,

        /// <summary>
        /// 作为网关或者代理工作的服务器尝试执行请求时，从远程服务器接收到了一个无效的响应
        /// </summary>
        [Explain("作为网关或者代理工作的服务器尝试执行请求时，从远程服务器接收到了一个无效的响应")]
        BadGateway = 502,

        /// <summary>
        /// 由于超载或系统维护，服务器暂时的无法处理客户端的请求。延时的长度可包含在服务器的Retry-After头信息中
        /// </summary>
        [Explain("由于超载或系统维护，服务器暂时的无法处理客户端的请求。延时的长度可包含在服务器的Retry-After头信息中")]
        ServiceUnavailable = 503,

        /// <summary>
        /// 充当网关或代理的服务器，未及时从远端服务器获取请求
        /// </summary>
        [Explain("充当网关或代理的服务器，未及时从远端服务器获取请求")]
        GatewayTimeOut = 504,

        /// <summary>
        /// 服务器不支持请求的HTTP协议的版本，无法完成
        /// </summary>
        [Explain("服务器不支持请求的HTTP协议的版本，无法完成")]
        HTTPVersionnotsupported = 505,
    }

    public class Result
    {
        public ResultCode Code { get; set; } = ResultCode.OK;
        public string Message { get; set; }
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }
    }
}

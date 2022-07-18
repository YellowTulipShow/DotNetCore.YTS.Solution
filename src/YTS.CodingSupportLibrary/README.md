# 文件文本编码支持库

[点击查看 - 修改日志文件](ChangeLog.md)

## 使用

使用前请全局注册代码页, 否则GBK, GB2312中文编码不支持会报错

```C#
// 测试开始前引用官方代码页引用, 增加支持中文GBK
UseExtend.SupportCodePages();
```

## 学习链接

### 官方介绍

* [Encoding 类](https://docs.microsoft.com/zh-cn/dotnet/api/system.text.encoding?view=netcore-3.1)
* [Unicode 字符百科](https://unicode-table.com/cn/)
* [GBK 编码](https://www.qqxiuzi.cn/zh/hanzi-gbk-bianma.php)
* [汉字字符集编码查询](https://www.qqxiuzi.cn/bianma/zifuji.php)

### 基础知识

* [字符，字节和编码](http://www.regexlab.com/zh/encoding.htm)
* [一文帮你彻底弄懂编码/字节/字符](https://zhuanlan.zhihu.com/p/363036851)
* [C# System.Text.Encoding 简介](https://blog.csdn.net/weixin_44813895/article/details/111624540)
* [C#基础教程（十一）字符编码ASCII,Unicode 和 UTF-8](https://blog.csdn.net/yangwenxue1989/article/details/119728263)
* [UTF-8编码规则](https://blog.csdn.net/sandyen/article/details/1108168)
* [UTF-8还是UTF-8 BOM](https://www.jianshu.com/p/fad568d89a29)
* [ UNICODE编码UTF-16 中的Endian（FE FF） 和 Little Endian（FF FE）](https://www.cnblogs.com/yzl050819/p/6667702.html)
* [大小端字节序(Big Endian和Little Endian)](https://blog.csdn.net/xionglangs/article/details/122082613)

### 代码片段

* [C#中将字符串与字节数组相互转换，如0055转换为0x00,0x55](https://blog.csdn.net/weixin_41425047/article/details/92197406)
* [在C#中，如何将一种编码的字符串转换成另外一种编码。](https://blog.csdn.net/weixin_34410662/article/details/89666798)

### 解决方案

* [libiconv字符集转换库在C#中的使用](https://blog.csdn.net/weixin_34256074/article/details/85882688)
* [dotnet core（C#）下读取ANSI（GB2312）编码的文本](https://blog.csdn.net/sunnyzls/article/details/104751426)

#### 头部判断代码

* [C＃ 判断文件编码](https://cloud.tencent.com/developer/article/1342448)
* [C# 判断一个文本文件的编码格式](https://blog.csdn.net/yelin042/article/details/82255551)

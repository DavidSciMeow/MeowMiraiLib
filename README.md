# Meow.MiraiLib

>## 一个简易使用的轻量化 Mirai-C# 后端  
>> .net 5[ver &lt;= 4.0.0]  
>> .net 6[ver > 4.0.0]  

```
@{
    框架集合从4.0.0后升级为 .net 6,
    跟随微软产品生命周期逐步取消 .net 5 支持.  
    我们建议所有用户升级至 .net 6 进行编写, 统一适应微软的生命周期.  
    指南将 .不.再.继.续.提.供. 关于 .net 5 的任何后续编写支持. 请您自行研判. 
    -- .net 6 最低编写版本为 Visual Studio 2022 --  
}
```
----
# <center> 程序编写指南目录 </center>
> 1. [主 框](#1)
> 1. [端实例化](#2)  
>    1. [普通实例化的两种方式](#21)  
>    1. [推荐的实例化方案](#22)  
> 1. [信 息](#3)
>    1. [信息的收取](#31)
>    1. [信息的发送](#32)
>        1. [信息的被动发送](#321)
>        1. [信息的主动发送](#322)
>    1. [信息和端的关系](#33)
> 1. [事件的处理](#4)
>    1. [端类型 (MeowMiraiLib朝您代管的事件)](#41)
>    1. [仅通知类型 (MML帮您解析的一般通知)](#42)
>    1. [需要决断类型 (被邀请加群,有人加群等)](#43)
>    1. [用户自定义类型 (其他类型)](#44)
> 1. [如何扩展](#5)
>    1. [扩展未知的信息类型](#51)
>    1. [扩展未知的事件类型](#52)
> 1. [最新版本 & 特性](#6)
> 1. [其他参考资料](#7)
>    1. [类图和参照设计图](#71)
>    1. [处理时序](#72)
> 1. [鸣 谢](#8)
-----    
# 1. 主框<a name="1"></a>  
## 对应的 信息/事件 类型等请参照源码内的第一行注释.
```csharp
//这是一个最简.net6版本顶级语句 [仅对于高级开发人员使用]
MeowMiraiLib.Client c = new("ws://ip:port/all?verifyKey=...&qq=...", true, true, -1);
c.Connect();
//c.ConnectAsync()
c.OnFriendMessageReceive += (s, e) => { };
//c.事件 += (s,e) => { 处理函数 }
while (true)
{
    switch (System.Console.ReadLine()) // 控制台操作
    {
        //.....
    }
}
```  
# 2. 端实例化<a name="2"></a>  
## 2.1 以下的端实例化完全一致,由 4.1.0 版本后的 .net 6 提供支持<a name="21"></a>  
```csharp
MeowMiraiLib.Client c = new("ws://test.com.cn:8888/all?verifyKey=123456&qq=1234567", true, true);
MeowMiraiLib.Client c2 = new("test.com.cn", 8888, "123456", 1234567, "all", true, true);
```
## 2.2 我们建议的标准端实例化<a name="22"></a>   
方案为, `在顶级语句外进行实例化`, 在顶级语句内引用.  
例如在ClientX.cs文件内,实例化所有您需要使用的端.  
您可以使用`同时使用多个端`, 或者`重复链接`一个端, 他们会`自动组织`Session和其他问题.  
注:本教程后续的发送端均调用在这个类
```csharp
//文件ClientX.cs
namespace Test
{
    public static class ClientX
    {
        //换成自己要用的参数
        public static MeowMiraiLib.Client c = new("ws://test.com.cn:8888/all?verifyKey=123456&qq=1234567", true, true);
        public static MeowMiraiLib.Client c2 = new("test.com.cn", 8888, "123456", 1234567, "all", true, true);
    }
}
```
顶级语句则变成如下写法:
```csharp
using System;
using Test; //项目名
using static Test.ClientX;

c.Connect();
c.OnFriendMessageReceive += MessageX.OnFriendMessageReceive;
//c.事件 += (s,e) => { 处理函数 }

while (true)
{
    switch (Console.ReadLine()) // 控制台操作
    {
        //.....
    }
}
```
# 3. 信息<a name="3"></a>  
ps.为了保持顶级语句的整洁, 当发送信息时, 我们建议您应当另开新文件, 创建新静态类处理, 使用事件注册来控制.  
ps2.
本框架调用段完全收发自由, 您无需先收后发,   
您可以`直接`进行`发送信息`, 也可以`接受到信息后不产生回应`,   
比如整点报时, 不需要收到信息然后产生动作.  
## 3.1 信息的收取<a name="31"></a>  
当然,在本例中我们使用MessageX.cs文件中的MessageX类来处理客户端接受的信息.  
您可以 自定义 类的名称.  
```csharp
//MessageX类
using MeowMiraiLib.Msg.Sender;
using MeowMiraiLib.Msg.Type;
namespace Test
{
    internal class MessageX
    {
        public static void OnFriendMessageReceive(FriendMessageSender s, Message[] e)
        {
            System.Console.WriteLine($"好友信息 [qq:{s.id},昵称:{s.nickname},备注:{s.remark}] \n内容:{e.MGetPlainString()}");  
            //MGetPlainString() 是一个关于Message类的数组扩展方法,用于返回信息里的文字部分
            //....其他操作
        }
    }
}
```
在顶级语句内添加如下字段   
```csharp
//顶级语句...
c.OnFriendMessageReceive += MeowMiraiLibTest.MessageX.OnFriendMessageReceive;
//...
```
运行,并尝试发送给您的机器人一段文字.   
~至此,接收端就这样做成了.   
## 3.2 信息的发送<a name="32"></a>  
在本节中,我们主要讨论 `收到信息后的发送` 和 `主动发送信息`    
### 3.2.1 被动处理消息<a name="321"></a>  
>假设我们从`好友信息`中读取数据.   
1.首先需要先规定命令的格式 例如: 重复 [要重复的内容]   
2.写一个处理这个事件的函数, 传入数据为原始信息需要的部分, 输出为一个Message[]数组等待发送.  
3.发送这个信息,使用某个端发送.  
### 注: 您也可以从`端 A` 接受信息 发送到`端 B` 的某个用户, 互操作性`由您自己在发送函数中规定`.
原始写法
```csharp
using MeowMiraiLib.Msg;
using MeowMiraiLib.Msg.Sender;
using MeowMiraiLib.Msg.Type;
namespace Test
{
    internal class MessageX
    {
        public static void OnFriendMessageReceive(FriendMessageSender s, Message[] e)
        {
            //MGetPlainStringSplit() 是一个关于Message类的数组扩展方法,
            //用于返回信息里的文字部分后按照 Splitor进行分割, splitor默认是空格
            var sx = e.MGetPlainStringSplit();
            var sendto = s.id;
            var msg = sx[0] switch
            {
                "重复" => Repeat(sx[1]),
                _ => null,
            };
            new FriendMessage(sendto, msg).Send(ClientX.c);
        }
        //重复消息
        public static Message[] Repeat(string s) => new Message[] { new Plain(s) };
    }
}
```
扩展写法
```csharp
using MeowMiraiLib.Msg;
using MeowMiraiLib.Msg.Sender;
using MeowMiraiLib.Msg.Type;
namespace Test
{
    internal class MessageX
    {
        public static void OnFriendMessageReceive(FriendMessageSender s, Message[] e)
        {
            //MGetPlainStringSplit() 是一个关于Message类的数组扩展方法,
            //用于返回信息里的文字部分后按照 Splitor进行分割, splitor默认是空格
            var sx = e.MGetPlainStringSplit();
            var sendto = s.id;
            var (t,j) = sx[0] switch
            {
                                                            //使用信息类的扩展发送方法
                "重复" => new Message[] { new Plain(sx[1]) }.SendToFriend(sendto,ClientX.c),
            };
            Console.WriteLine(j);
        }
    }
}
```
到现在为止, 您可以尝试发送给您的机器人一个消息, 内容如下:  `重复 这句话`   
您的机器人应该回复: `这句话`  
~至此,被动处理消息已经完成
### 3.2.2 主动发送消息 (不建议大量发送,可能会被风控)<a name="322"></a>  
您可以使用消息发送功能`主动`朝某个端的好友/群/陌生人发送消息.  
例如下例的整点报时系统.  
```csharp
using System;
using MeowMiraiLib.Msg;
using MeowMiraiLib.Msg.Type;

MeowMiraiLib.Client c = new("ws://test.com.cn:8888/all?verifyKey=123456&qq=1234567", true, true);
c.Connect();

System.Timers.Timer t = new(1000*60); //每一分钟查看一次
t.Start(); //启动计时器
t.Elapsed += (s,e) =>
{
    if(DateTime.Now.Minute == 0) //检测当前的分钟是否为0 (整点)
    {
        //发送信息给某个人..
        new FriendMessage(/*qq号*/, new Message[] { new Plain($"{DateTime.Now.Hour}点 啦!") }).Send(c);
    }
};

while (true)
{
    switch (Console.ReadLine()) // 控制台操作
    {
        //.....
    }
}
```
> 您也未必需要都写在顶级语句内, 您可以 `自己规定发送形式`, 以及其他的任何程序处理逻辑.  
> 如上例的计时器可以完全写在另一个类内.   
> 但您要注意, 您无法获得顶级语句内的任何变量,  
> 所以我们 `建议在外域(其他类) 定 义 端`, 然后在顶级语句使用完全限定名称引用.   
### 3.3 信息和端的关系<a name="33"></a>  
简单的说, 没有任何关系, 任何`一组信息`, 可以被任何端发送.  
您可以使用扩展方法 `Message[].Send(Client,Type,SendTo)`.
一组信息可以发送给群也可以发送给人, 所以需要类型, 当完全明确的时候,  
我们建议使用 `MessageType*.Send()`, 而不是使用 `Message[].Send()`  

# 4 事件的处理<a name="4"></a>  
> 本章节主要讨论生成的事件和一些通用事件的处理方案  
> 包含`端类型`, `仅通知类型`, `需要决断类型`和`用户定义类型`.  
## 4.1 端类型<a name="41"></a>  
*端发送的信息例如, 已连接, 已断开, 正在重试,出错..等, 都是由日志直接打印*  
其事件定义在 ./Client/ClientEvent.cs 文件的 /\*--Type of Service--\*/行后
## 4.2 仅通知类型<a name="42"></a>  
*端通知的通知类型如 客户端下线,Bot被邀请加入群聊,客户端网络无法连接(内网程序)...等*  
*均由事件推送,并不会打印日志,除非您在构造时设置 Client.EventDebug = true*  
其事件定义在 ./Client/ClientEvent.cs 文件的 /\*--Type of Event--\*/ 行后
### 4.3 需要决断类型<a name="43"></a>  
*在仅通知类型内,有些需要我们做出决断,例如是否加群等*  
下例便展示了 `只要符合某条件的(本例中为邀请信息为123456)` 则加入,其他拒绝  
```csharp
//下列参数 c 为要发送的端,而且必须是接收这个信息的端
//老写法[通用写法] (需要 using MeowMiraiLib.Msg;)
c.OnEventBotInvitedJoinGroupRequestEvent += (s) =>
{
    if(s.message == "123456") //某个条件
    {
        new Resp_botInvitedJoinGroupRequestEvent(s.eventId, s.fromId, s.groupId, 0, "").Send(c); //0为同意
    }
    else
    {
        new Resp_botInvitedJoinGroupRequestEvent(s.eventId, s.fromId, s.groupId, 1, "").Send(c); //1为同意
    }
};
//扩展写法 (需要 using MeowMiraiLib.Event;)
c.OnEventBotInvitedJoinGroupRequestEvent += (s) =>
{
    if(s.message == "123456") //某个条件
    {
        s.Grant(c); 
    }
    else
    {
        s.Deny(c);
    }
};
//你也可以接受回值查看是否成功
//.....
var (t,j) = s.Grant(c); 
Console.WriteLine(j);
//.....
```
### 4.4 用户定义类型<a name="44"></a>  
*目前用户定义类型事件已经合并到返回值中处理,您也可以尝试重写暴露出来的Websocket端(`client`.ws.DataReceived事件)*  
例如获取群列表等用户操作类型, 请执行 var (t,j) = new `*Type*`.Send();
# 5 如何扩展<a name="5"></a>  
## 5.1 扩展还没有生效的消息类型<a name="51"></a>  
所有消息都继承自 Message.cs 的 Message 类,  
继承时检查是否符合其他已经继承类的特性(字段/属性)  
使用如下代码扩展
```csharp
public class NewMessageType : Message //..Plain
{
    //...字段(属性)

    //...方法
}
```
## 5.2 扩展未知的事件类型<a name="52"></a>  
所有未知的事件处理都由 Client实例的 事件代理 _OnUnknownEvent触发 `(ClientEvent.cs文件内)`   
此事件代理含有一个 string 的内参, 负责传输标准的 Json字符串.  
如果您知道这是个什么类型,为什么传输,您可以自己捕获然后自己处理.  
# 6 最新版本&特性<a name="6"></a>  
>>3.0.0 加装了异步处理的标准流程, 优化了事件处理, 独立(实例了)端和信息发送的方案.  
>>4.0.0 更新了异步处理的标准流程,防止CPU空转等待  
>>4.1.0 更新到 `.net 6` 使用更简单书写方案来控制程序, 增加了扩展方法  
>>### 4.1.1 增加了扩展方法 Message[].send(c); 简易发送方案. 
# 7 其他参考资料<a name="7"></a>  
## 7.1 类图和参照设计图<a name="71"></a>  
### *正在更新制作*
## 7.2 处理参照时序<a name="72"></a>  
WebSocketClientRecieve  
| => Client.Ws_MessageReceived \{./Client/ClientParser.cs\}  
|| => *EventName* .Invoke() \{./Client/ClientEvent.cs\}  
||| => *User* _Lambda() ...  
...  
User .Send() / .SendAsync()  
| => Client.SSM.*type*.Construct() \{./SSM.cs\}  
|| => Client.SendAndWaitResponse() \{./Client/Client.cs\}  
|| => *Wait...**  
||| => *User*.__Return_JObject_
# 8 鸣谢<a name="8"></a>  
## 感谢大佬 [@Executor-Cheng](https://github.com/Executor-Cheng) 的初版建议和意见.  
## 也感谢各位其他大佬对小项目的关注.

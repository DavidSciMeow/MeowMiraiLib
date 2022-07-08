# Meow.MiraiLib
## 一个简易使用的轻量化 Mirai-C# 后端  

[![CI](https://github.com/DavidSciMeow/MeowMiraiLib/actions/workflows/main.yml/badge.svg)](https://github.com/DavidSciMeow/MeowMiraiLib/actions/workflows/main.yml)
![](https://img.shields.io/nuget/vpre/Electronicute.MeowMiraiLib?label=NuGet%20Version)
![](https://img.shields.io/nuget/dt/Electronicute.MeowMiraiLib?label=Nuget%20Download)

<<<<<<< Updated upstream
>## 一个简易使用的轻量化 Mirai-C# 后端  
>> .net 6 [ver 7.0.x]
>>> maj 1.修复了在模式下由于网络波动而引起的队列空置问题, 由于我本地没有网络问题无法复现,   
>>> 感谢 [@LittleFish-233](https://github.com/LittleFish-233) 的调试,努力探究和辛勤付出.

----
=======
## 最新版本更新改动较大,请酌情更新或者适配.

|MI 维护指数|CC 圈复杂度|DoI 继承深度|ClC 类耦合度|LoSc 源码行数|LoEc 执行代码行数|
|---------|--------|----------|---------|------------|--------------|
|86 :green_book: |420|4|187|5464|747|

-------
>>>>>>> Stashed changes
# <center> 程序编写指南目录 </center>
> 0. [最新更新速报](#0)
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
> 1. [其他参考资料](#6)
>    1. [类图和参照设计图](#61)
>    1. [处理时序](#62)
> 1. [信息快速编写功能类](#7)
> 1. [鸣 谢](#8)
-----    
# 0. 最新更新<a name="0"></a>  
## 8.0.0 更新信息
1. 将SSM(发送信息回收端) 的标准方案适配回值改为`T`而非`(bool,JObject)`,  
   您可以通过检查MessageId来进行判断消息是否成功, 您也可以使用MessageId快速撤回消息  
   如果您需要以前的返回值请使用函数`OSend`或者`OSendAsync`  
   除了`文件管理`类,其他类均已解析实例类  

   详细信息如本例:
   ```csharp
    var k = new Message[] { new Plain("test") }.SendToFriend(qq,client);
    if(k.Id > 0)//您可以不适用此检测方案
    {
        //发送成功 且MessageId = k.Id
    }
    else if(k.Id == -1)
    {
        //失败
    }
    k.ReCall();//快速撤回
    var d = k.ReCall(2);//2秒后撤回
    if (d)//您可以不适用此检测方案
    {
        //撤回成功
    }
    else
    {
        //撤回失败
    }
   ```
   接口对应解析类有`ConfirmationTypoGeneral`/`MessageTypoGeneral`/`ProfileClassGeneral`  
   `ConfirmationTypoGeneral` 返回一个 `int` 类型值, `-1`为失败, `0`为成功, +N为其他API保留值  
   `MessageTypoGeneral` 返回一个 `MessageId` 结构体, 您可以使用他进行撤回, 或者其他高级消息操作  
   `ProfileClassGeneral` 返回一个 `QQProfile` 类, 本类是所有资料类型类的标准扩展类
   
    任意类继承对应关系如下表:  

    |原指令类|扩展适配接口类|自是否含有扩展功能|
    |:---|:---:|:---:|
    |About|SSM(Base)| :x: |
    |MessageFromId|SSM(Base)| :x: |
    |FriendList|SSM(Base)| :heavy_check_mark: (重写) Send/SendAsync|
    |GroupList|SSM(Base)| :heavy_check_mark: (重写) Send/SendAsync|
    |MemberList|SSM(Base)| :heavy_check_mark: (重写) Send/SendAsync|
    |BotProfile|ProfileClassGeneral| :heavy_check_mark: (继承)|
    |FriendProfile|ProfileClassGeneral| :heavy_check_mark: (继承)|
    |MemberProfile|ProfileClassGeneral| :heavy_check_mark: (继承)|
    |UserProfile|ProfileClassGeneral| :heavy_check_mark: (继承)|
    |FriendMessage|MessageTypoGeneral| :heavy_check_mark: (继承)|
    |GroupMessage|MessageTypoGeneral| :heavy_check_mark: (继承)|
    |TempMessage|MessageTypoGeneral| :heavy_check_mark: (继承)|
    |SendNudge|ConfirmationTypoGeneral| :heavy_check_mark: (继承)|
    |Recall|ConfirmationTypoGeneral| :heavy_check_mark: (继承)|
    |File_list|SSM(Base)| :x: |
    |File_info|SSM(Base)| :x: |
    |File_mkdir|SSM(Base)| :x: |
    |File_delete|SSM(Base)| :x: |
    |File_move|SSM(Base)| :x: |
    |File_rename|SSM(Base)| :x: |
    |DeleteFriend|ConfirmationTypoGeneral| :heavy_check_mark: (继承) |
    |Mute|ConfirmationTypoGeneral| :heavy_check_mark: (继承) |
    |Unmute|ConfirmationTypoGeneral| :heavy_check_mark: (继承) |
    |Kick|ConfirmationTypoGeneral| :heavy_check_mark: (继承) |
    |Quit|ConfirmationTypoGeneral| :heavy_check_mark: (继承) |
    |MuteAll|ConfirmationTypoGeneral| :heavy_check_mark: (继承) |
    |UnmuteAll|ConfirmationTypoGeneral| :heavy_check_mark: (继承) |
    |SetEssence|ConfirmationTypoGeneral| :heavy_check_mark: (继承) |
    |GroupConfig_Get|SSM(Base)| :heavy_check_mark: (重写) Send/SendAsync |
    |GroupConfig_Update|ConfirmationTypoGeneral| :heavy_check_mark: (继承) |
    |MemberInfo_Get|SSM(Base)| :heavy_check_mark: (重写) Send/SendAsync |
    |MemberInfo_Update|ConfirmationTypoGeneral| :heavy_check_mark: (继承) |
    |MemberAdmin|ConfirmationTypoGeneral| :heavy_check_mark: (继承) |
    |Anno_list|SSM(Base)| :heavy_check_mark: (重写) Send/SendAsync |
    |Anno_publish|SSM(Base)| :heavy_check_mark: (重写) Send/SendAsync |
    |Anno_delete|SSM(Base)| :heavy_check_mark: (重写) Send/SendAsync |
    |Resp_newFriendRequestEvent|SSM(Base)| :x: |
    |Resp_memberJoinRequestEvent|SSM(Base)| :x: |
    |Resp_botInvitedJoinGroupRequestEvent|SSM(Base)| :x: |


1. 加入了GenericModel.cs文件用于解析标准返回值  
   例如`QQFriend`/`QQGroup`/`QQGroupMember`等,  
   如下可以解析好友列表/群列表
   ```csharp
    //获取好友列表
    var fl = new FriendList().Send(c);
    Console.WriteLine(fl.Length);
    (fl[1], new Message[] { new Plain("test") }).SendMessage(c); //朝好友1快速发消息

    //获取群列表
    var gl = new GroupList().Send(c);
    Console.WriteLine(gl.Length);
    (gl[1], new Message[] { new Plain("test") }).SendMessage(c); //朝群1快速发消息

    //获取群列表并朝第一个群的一个群员发信息
    var gl = new GroupList().Send(c);
    gl[1].GetMemberList(c)[1].SendMessage(c, new Message[] { new Plain("test") });
    
    //朝群1里的每个群员发信息
    var gl = new GroupList().Send(rinko);
    foreach(var i in gl[1].GetMemberList(c))
    {
        i.SendMessage(c, new Message[] { new Plain("test") });
    }
   ```
   新增合并模式快速写法 (a,b).Send(c); 等...  
   剩余特性您可以详细查看 [信息快速编写功能类](#7) 的函数定义, 或者`MessageUtil.cs`文件
1. 实例化类增加了获取资料类
   ```csharp
   //获取bot的资料
   var bp = new BotProfile().Send(c); //获取Bot资料
   var fp = new FriendProfile(qq).Send(c);//获取好友资料
   var mp = new MemberProfile(qqgroup,qqnumber).Send(c);//获取群员资料
   var up = new UserProfile(1500294830).Send(c);//获取用户资料
   Console.WriteLine(bp.ToString());//重写了ToString方法适配逻辑
   Console.WriteLine(fp.ToString());
   Console.WriteLine(mp.ToString());
   Console.WriteLine(up.ToString());
   ```
1. 新增了`群公告`相关接口, 其接口表达式值(函数)为 `Anno_list` `Anno_publish` `Anno_delete`  
   ```csharp
   //获取群公告&&推送群公告
   var k = new Anno_list(qqgroup).Send(c); 
   k[1].Delete(c);//删除群公告1 (快速写法)
   var k1 = new Anno_publish(qqgroup, "Bot 带图公告推送").Send(c);
   var k2 = new Anno_publish(qqgroup, "Bot 带图公告推送实验",imageUrl: "https://static.rinko.com.cn/static/bggs.png").Send(c);
   ```

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
## 2.1 以下的端实例化完全一致,由 .net 6 提供支持<a name="21"></a>  
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

<<<<<<< Updated upstream
# 6 最新版本&特性<a name="6"></a>  
>>3.0.0 加装了异步处理的标准流程, 优化了事件处理, 独立(实例了)端和信息发送的方案.  
>>4.0.0 更新了异步处理的标准流程,防止CPU空转等待  
>>4.1.0 更新到 `.net 6` 使用更简单书写方案来控制程序, 增加了扩展方法  
>>4.1.1 增加了扩展方法 Message[].send(c); 简易发送方案.
>>4.2.0 修复了XML发送的匹配Json问题
>>5.0.0 增加了Image适配linux的(libgdi+)
>>6.0.0 移除了Image问题和Base64自动转换,相关类库(System.Drawing.Common),*由于微软对其跨平台不再支持*
>>6.1.0 增加优化了守护链,保持进程稳定
>>7.0.0 更改了队列机制,防止CPU空转和等待出队消耗. 


# 7 其他参考资料<a name="7"></a>  
## 7.1 类图和参照设计图<a name="71"></a>  
=======
# 7 其他参考资料<a name="6"></a>  
## 7.1 类图和参照设计图<a name="61"></a>  
>>>>>>> Stashed changes
### *正在更新制作*
## 7.2 处理参照时序<a name="62"></a>  
WebSocketClientRecieve  
| => Client.Ws_MessageReceived \{./Client/ClientParser.cs\}  
|| => *EventName* .Invoke() \{./Client/ClientEvent.cs\}  
||| => *User* _Lambda() ...  
...  
User .OSend() / .OSendAsync()  
| => Client.SSM.*type*.Construct() \{./SSM.cs\}  
|| => Client.SendAndWaitResponse() \{./Client/Client.cs\}  
|| => *Wait...(Queue.)**  
||| => *User*.__Return_JObject_
<<<<<<< Updated upstream

# 8 信息快速编写功能类 <a name="8"></a>
## (MessageUtil / 引用位置:MeowMiraiLib.Msg.Type)
=======
>>>>>>> Stashed changes

User .Send() / .SendAsync()  
| => Client.SSM.*type*.Construct() \{./SSM.cs\}  
|| => Client.SendAndWaitResponse() \{./Client/Client.cs\}  
|| => *Wait...**  
||| => *Interpreter* - \<T\> -> new T();  
|||| => *return* (type)T Instance*
# 8 信息快速编写功能类 <a name="7"></a>
## (MessageUtil / 引用位置:MeowMiraiLib.Msg.Type)
> ### 1. MGetPlainString 获取消息中的所有字符集合
```csharp
rinko.OnFriendMessageReceive += (s, e) =>
{
    if(s.id != qqid) //过滤自己发出的信息
    {
        var str = e.MGetPlainString();
        Console.WriteLine(str);
    }
};
```

> ### 2. MGetPlainString 获取消息中的所有字符集合并且使用(splitor参数)分割
```csharp
rinko.OnFriendMessageReceive += (s, e) =>
{
    if(s.id != qqid) //过滤自己发出的信息
    {
        var str = e.MGetPlainStringSplit(); //默认使用空格分隔
        //var str = e.MGetPlainStringSplit(","); //使用逗号分割
        Console.WriteLine(str);
    }
};
```
> ### 3. MGetEachImageUrl 获取消息中的所有图片集合的Url
```csharp
rinko.OnFriendMessageReceive += (s, e) =>
{
    if(s.id != qqid) //过滤自己发出的信息
    {
        var sx = e.MGetEachImageUrl();
        Console.WriteLine(sx[1].url);
    }
};
```
> ### 4. SendToFriend 信息类前置发送好友信息
```csharp
new Message[] { new Plain("...") }.SendToFriend(qqnumber,c);
```
> ### 5. SendToGroup 信息类前置发送群信息
```csharp
new Message[] { new Plain("...") }.SendToGroup(qqgroupnumber,c);
```
> ### 6. SendToTemp 信息类前置发送临时信息
```csharp
new Message[] { new Plain("...") }.SendToTemp(qqnumber,qqgroupnumber,c);
```
> ### 7. SendMessage 对于GenericModel的群发信息逻辑
> 注:您也可以使用`foreach`对每个`群`/`好友`/`群员`发送
```csharp
var msg = new Message[] { new Plain("...") };
var fl = new FriendList().Send(c);//获取好友列表
(fl[1], msg).SendMessage(c);
fl[1].SendMessage(msg,c);

var gl = new GroupList().Send(c);//获取群列表
(gl[1], msg).SendMessage(c);
gl[1].SendMessage(msg,c);

var gl = new GroupList().Send(c);//获取群列表
var gml = gl[1].GetMemberList(c);//获取群1的群员列表
(gml[1], msg).SendMessage(c);
gml[1].SendMessage(msg,c);
```

<<<<<<< Updated upstream
# 9 鸣谢<a name="9"></a>  
## 感谢大佬 [@Executor-Cheng](https://github.com/Executor-Cheng) 的初版建议和意见.
## 感谢大佬 [@LittleFish-233](https://github.com/LittleFish-233) 对于多线程稳定度的探索以及对新版算法的优化.
=======
# 9 鸣谢<a name="8"></a>  
## 感谢大佬 [@Executor-Cheng](https://github.com/Executor-Cheng) 的初版建议和意见.  
## 感谢大佬 [@LittleFish-233](https://github.com/LittleFish-233) 对于网络问题的探索和修改.  
>>>>>>> Stashed changes
## 也感谢各位其他大佬对小项目的关注.

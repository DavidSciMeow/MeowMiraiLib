using MeowMiraiLib.Msg.Sender;
using MeowMiraiLib.Msg.Type;

/*
 * 所有后台引起事件文件
 * 本文件用于存储对标Mirai-HTTP-API EventType内的所有事件实例 以及 Socket/Client事件的定义
 * 逻辑上本文件展示的所有事件均交由开发者自己覆盖和反写, 但不允许更改Parser内的对应事件解析方案
 * 在 WebSocket 链接成功后客户端会进行输出 "Connected" 如果想覆盖本事件请重新编辑 ClientParser.cs 内的 Ws_Opened 方法.
 * -----------------
 * This File is using to present all Mirai-Backend Event
 * This file is an one to one mapping of Mirai-HTTP-API EventType file, and Socket/Client Event defines
 * logically all event inside this file is responsable to dev-people,
 * but restricted to alter any function/method that written in 'ClientParser.cs' File
 * When Websocket connected then screen will print one single-line text of 'Connected',
 * if you want change this settings then, you can edit the 'ClientParser.cs' File -> 'Ws_Opened' function/method to achieve this goal.
 */

namespace MeowMiraiLib
{
    public partial class Client
    {
        /// <summary>
        /// 接收到Websocket端链接成功信息
        /// </summary>
        /// <param name="e">信息内容</param>
        public delegate void ServiceConnected(string e);
        /// <summary>
        /// 接收到Websocket端链接成功信息
        /// </summary>
        public event ServiceConnected OnServeiceConnected;

        /// <summary>
        /// 收到戳一戳信息
        /// </summary>
        /// <param name="e">戳一戳句柄</param>
        public delegate void NudgeEvent(Event.NudgeEvent e);
        /// <summary>
        /// 收到戳一戳信息
        /// </summary>
        public event NudgeEvent OnNudgeMessageRecieve;
        /// <summary>
        /// 接收到好友私聊信息
        /// </summary>
        /// <param name="s">信息发送者参数句柄</param>
        /// <param name="e">信息内容</param>
        public delegate void FriendMessage(FriendMessageSender s, Message[] e);
        /// <summary>
        /// 接收到好友私聊信息
        /// </summary>
        public event FriendMessage OnFriendMessageRecieve;
        /// <summary>
        /// 接收到群聊信息
        /// </summary>
        /// <param name="s">信息发送者参数句柄</param>
        /// <param name="e">信息内容</param>
        public delegate void GroupMessage(GroupMessageSender s, Message[] e);
        /// <summary>
        /// 接收到群聊信息
        /// </summary>
        public event GroupMessage OnGroupMessageRecieve;
        /// <summary>
        /// 接收到临时信息
        /// </summary>
        /// <param name="s">信息发送者参数句柄</param>
        /// <param name="e">信息内容</param>
        public delegate void TempMessage(TempMessageSender s, Message[] e);
        /// <summary>
        /// 接收到临时信息
        /// </summary>
        public event TempMessage OnTempMessageRecieve;
        /// <summary>
        /// 接收到陌生人信息
        /// </summary>
        /// <param name="s">信息发送者参数句柄</param>
        /// <param name="e">信息内容</param>
        public delegate void StrangerMessage(StrangerMessageSender s, Message[] e);
        /// <summary>
        /// 接收到陌生人信息
        /// </summary>
        public event StrangerMessage OnStrangerMessageRecieve;
        /// <summary>
        /// 接收到其他类型信息
        /// </summary>
        /// <param name="s">信息发送者参数句柄</param>
        /// <param name="e">信息内容</param>
        public delegate void OtherClientMessage(OtherClientMessageSender s, Message[] e);
        /// <summary>
        /// 接收到其他类型信息
        /// </summary>
        public event OtherClientMessage OnOtherMessageRecieve;

        /// <summary>
        /// 接收到自身加入群聊请求
        /// </summary>
        /// <param name="e">自身入群句柄</param>
        public delegate void BotJoinGroupEvent(Event.BotJoinGroupEvent e);
        /// <summary>
        /// 接收到自身加入群聊请求
        /// </summary>
        public event BotJoinGroupEvent OnEventBotJoinGroupEvent;
        /// <summary>
        /// 接收到新好友请求
        /// </summary>
        /// <param name="e">好友请求句柄</param>
        public delegate void NewFriendRequestEvent(Event.NewFriendRequestEvent e);
        /// <summary>
        /// 接收到新好友请求
        /// </summary>
        public event NewFriendRequestEvent OnEventBotNewFriendEventRecieve;

        /*-*/
        /// <summary>
        /// Mirai后台证实QQ上线
        /// </summary>
        /// <param name="e">Bot上线句柄</param>
        public delegate void BotOnlineEvent(Event.BotOnlineEvent e);
        /// <summary>
        /// Mirai后台证实QQ上线
        /// </summary>
        public event BotOnlineEvent OnEventBotOnlineEvent;
        /// <summary>
        /// Mirai后台证实QQ主动离线
        /// </summary>
        /// <param name="e">主动离线QQ句柄</param>
        public delegate void BotOfflineEventActive(Event.BotOfflineEventActive e);
        /// <summary>
        /// Mirai后台证实QQ主动离线
        /// </summary>
        public event BotOfflineEventActive OnEventBotOfflineEventActive;
        /// <summary>
        /// Mirai后台证实QQ被挤下线
        /// </summary>
        /// <param name="e">被挤下线QQ句柄</param>
        public delegate void BotOfflineEventForce(Event.BotOfflineEventForce e);
        /// <summary>
        /// Mirai后台证实QQ被挤下线
        /// </summary>
        public event BotOfflineEventForce OnEventBotOfflineEventForce;
        /// <summary>
        /// Mirai后台证实QQ由于网络问题掉线
        /// </summary>
        /// <param name="e">掉线QQ的句柄</param>
        public delegate void BotOfflineEventDropped(Event.BotOfflineEventDropped e);
        /// <summary>
        /// Mirai后台证实QQ由于网络问题掉线
        /// </summary>
        public event BotOfflineEventDropped OnEventBotOfflineEventDropped;
        /// <summary>
        /// Mirai后台证实QQ重新连接完毕
        /// </summary>
        /// <param name="e">链接成功的QQ句柄</param>
        public delegate void BotReloginEvent(Event.BotReloginEvent e);
        /// <summary>
        /// Mirai后台证实QQ重新连接完毕
        /// </summary>
        public event BotReloginEvent OnEventBotReloginEvent;
        /// <summary>
        /// 好友的输入状态改变
        /// </summary>
        /// <param name="e">好友输入状态改变句柄</param>
        public delegate void FriendInputStatusChangedEvent(Event.FriendInputStatusChangedEvent e);
        /// <summary>
        /// 好友输入状态改变了
        /// </summary>
        public event FriendInputStatusChangedEvent OnEventFriendInputStatusChangedEvent;
        /// <summary>
        /// 好友的昵称改变
        /// </summary>
        /// <param name="e">好友昵称改变句柄</param>
        public delegate void FriendNickChangedEvent(Event.FriendNickChangedEvent e);
        /// <summary>
        /// 好友昵称改变了
        /// </summary>
        public event FriendNickChangedEvent OnEventFriendNickChangedEvent;

    }
}

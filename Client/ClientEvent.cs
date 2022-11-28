using MeowMiraiLib.Msg.Sender;
using MeowMiraiLib.Msg.Type;
using System;

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
        /*--Type of Service--*/
        /// <summary>
        /// Websocket端链接成功信息
        /// </summary>
        public delegate void ServiceConnected(string e);
        /// <summary>
        /// 接收到Websocket端链接成功信息
        /// </summary>
        public event ServiceConnected _OnServeiceConnected;
        /// <summary>
        /// Websocket端错误信息
        /// </summary>
        public delegate void ServiceError(Exception e);
        /// <summary>
        /// 接收到Websocket端错误信息
        /// </summary>
        public event ServiceError _OnServeiceError;
        /// <summary>
        /// Websocket端链接关闭信息
        /// </summary>
        public delegate void ServiceDropped(string e);
        /// <summary>
        /// 接收到Websocket端链接关闭信息
        /// </summary>
        public event ServiceDropped _OnServiceDropped;
        /// <summary>
        /// 后端传送其他客户端上线通知
        /// </summary>
        /// <param name="e">客户端上线通知句柄</param>
        public delegate void OtherClientOnlineEvent(Event.OtherClientOnlineEvent e);
        /// <summary>
        /// 接收到后端传送其他客户端上线通知
        /// </summary>
        public event OtherClientOnlineEvent _OnClientOnlineEvent;
        /// <summary>
        /// 后端传送其他客户端下线通知
        /// </summary>
        /// <param name="e">客户端下线通知句柄</param>
        public delegate void OtherClientOfflineEvent(Event.OtherClientOfflineEvent e);
        /// <summary>
        /// 接收到后端传送其他客户端下线通知
        /// </summary>
        public event OtherClientOfflineEvent _OnOtherClientOfflineEvent;
        /// <summary>
        /// 后端传送命令被执行通知
        /// </summary>
        /// <param name="e">命令被执行通知句柄</param>
        public delegate void CommandExecutedEvent(Event.CommandExecutedEvent e);
        /// <summary>
        /// 接收到后端传送命令被执行通知
        /// </summary>
        public event CommandExecutedEvent _OnCommandExecutedEvent;
        /// <summary>
        /// 后端传送命令未知解析
        /// </summary>
        /// <param name="e">命令被执行通知句柄</param>
        public delegate void UnknownEvent(string e);
        /// <summary>
        /// 接收到后端传送未知命令
        /// </summary>
        public event UnknownEvent _OnUnknownEvent;

        /*--Type of Message--*/
        /// <summary>
        /// 接收到好友私聊信息
        /// </summary>
        /// <param name="s">信息发送者参数句柄</param>
        /// <param name="e">信息内容</param>
        public delegate void FriendMessage(FriendMessageSender s, Message[] e);
        /// <summary>
        /// 接收到好友私聊信息
        /// </summary>
        public event FriendMessage OnFriendMessageReceive;
        /// <summary>
        /// 接收到群聊信息
        /// </summary>
        /// <param name="s">信息发送者参数句柄</param>
        /// <param name="e">信息内容</param>
        public delegate void GroupMessage(GroupMessageSender s, Message[] e);
        /// <summary>
        /// 接收到群聊信息
        /// </summary>
        public event GroupMessage OnGroupMessageReceive;
        /// <summary>
        /// 接收到临时信息
        /// </summary>
        /// <param name="s">信息发送者参数句柄</param>
        /// <param name="e">信息内容</param>
        public delegate void TempMessage(TempMessageSender s, Message[] e);
        /// <summary>
        /// 接收到临时信息
        /// </summary>
        public event TempMessage OnTempMessageReceive;
        /// <summary>
        /// 接收到陌生人信息
        /// </summary>
        /// <param name="s">信息发送者参数句柄</param>
        /// <param name="e">信息内容</param>
        public delegate void StrangerMessage(StrangerMessageSender s, Message[] e);
        /// <summary>
        /// 接收到陌生人信息
        /// </summary>
        public event StrangerMessage OnStrangerMessageReceive;
        /// <summary>
        /// 接收到其他类型信息
        /// </summary>
        /// <param name="s">信息发送者参数句柄</param>
        /// <param name="e">信息内容</param>
        public delegate void OtherClientMessage(OtherClientMessageSender s, Message[] e);
        /// <summary>
        /// 接收到其他类型信息
        /// </summary>
        public event OtherClientMessage OnOtherMessageReceive;
        /// <summary>
        /// 接收到同步好友消息
        /// </summary>
        /// <param name="s">同步好友消息接收者句柄</param>
        /// <param name="e">信息内容</param>
        public delegate void FriendSyncMessage(FriendSyncMessageSender s, Message[] e);
        /// <summary>
        /// 接收到同步好友消息
        /// </summary>
        public event FriendSyncMessage OnFriendSyncMessageReceive;
        /// <summary>
        /// 接收到同步群消息
        /// </summary>
        /// <param name="s">同步群消息接收者句柄</param>
        /// <param name="e">信息内容</param>
        public delegate void GroupSyncMessage(GroupSyncMessageSender s, Message[] e);
        /// <summary>
        /// 接收到同步群消息
        /// </summary>
        public event GroupSyncMessage OnGroupSyncMessageReceive;
        /// <summary>
        /// 接收到同步群临时消息
        /// </summary>
        /// <param name="s">同步群临时接收者句柄</param>
        /// <param name="e">信息内容</param>
        public delegate void TempSyncMessage(TempSyncMessageSender s, Message[] e);
        /// <summary>
        /// 接收到同步群临时消息
        /// </summary>
        public event TempSyncMessage OnTempSyncMessageReceive;
        /// <summary>
        /// 接收到同步陌生人消息
        /// </summary>
        /// <param name="s">同步陌生人接收者句柄</param>
        /// <param name="e">信息内容</param>
        public delegate void StrangerSyncMessage(StrangerSyncMessageSender s, Message[] e);
        /// <summary>
        /// 接收到同步陌生人消息
        /// </summary>
        public event StrangerSyncMessage OnStrangerSyncMessageReceive;

        /*--Type of Event--*/
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
        /// <summary>
        /// Bot在群里的权限被改变
        /// </summary>
        /// <param name="e">权限改变句柄</param>
        public delegate void BotGroupPermissionChangeEvent(Event.BotGroupPermissionChangeEvent e);
        /// <summary>
        /// Bot在群里的权限被改变了
        /// </summary>
        public event BotGroupPermissionChangeEvent OnEventBotGroupPermissionChangeEvent;
        /// <summary>
        /// Bot被禁言
        /// </summary>
        /// <param name="e">禁言句柄</param>
        public delegate void BotMuteEvent (Event.BotMuteEvent e);
        /// <summary>
        /// Bot被禁言了
        /// </summary>
        public event BotMuteEvent OnEventBotMuteEvent;
        /// <summary>
        /// Bot被解除禁言
        /// </summary>
        /// <param name="e">解除禁言句柄</param>
        public delegate void BotUnmuteEvent (Event.BotUnmuteEvent e);
        /// <summary>
        /// Bot被解除禁言了
        /// </summary>
        public event BotUnmuteEvent OnEventBotUnmuteEvent;
        /// <summary>
        /// Bot加入新群
        /// </summary>
        /// <param name="e">自身入群句柄</param>
        public delegate void BotJoinGroupEvent(Event.BotJoinGroupEvent e);
        /// <summary>
        /// Bot加入了一个新群
        /// </summary>
        public event BotJoinGroupEvent OnEventBotJoinGroupEvent;
        /// <summary>
        /// Bot主动退群
        /// </summary>
        /// <param name="e">退群句柄</param>
        public delegate void BotLeaveEventActive(Event.BotLeaveEventActive e);
        /// <summary>
        /// Bot主动退出一个群
        /// </summary>
        public event BotLeaveEventActive OnEventBotLeaveEventActive;
        /// <summary>
        /// Bot被群踢出
        /// </summary>
        /// <param name="e">被群踢出句柄</param>
        public delegate void BotLeaveEventKick(Event.BotLeaveEventKick e);
        /// <summary>
        /// Bot被群踢出
        /// </summary>
        public event BotLeaveEventKick OnEventBotLeaveEventKick;
        /// <summary>
        /// 群员撤回信息
        /// </summary>
        /// <param name="e">群员撤回信息句柄</param>
        public delegate void GroupRecallEvent(Event.GroupRecallEvent e);
        /// <summary>
        /// 群员撤回了信息
        /// </summary>
        public event GroupRecallEvent OnEventGroupRecallEvent;
        /// <summary>
        /// 好友撤回信息
        /// </summary>
        /// <param name="e">撤回信息句柄</param>
        public delegate void FriendRecallEvent (Event.FriendRecallEvent e);
        /// <summary>
        /// 好友撤回了信息
        /// </summary>
        public event FriendRecallEvent OnEventFriendRecallEvent;
        /// <summary>
        /// 戳一戳事件
        /// </summary>
        /// <param name="e">戳一戳句柄</param>
        public delegate void NudgeEvent(Event.NudgeEvent e);
        /// <summary>
        /// 收到戳一戳信息
        /// </summary>
        public event NudgeEvent OnEventNudgeEvent;
        /// <summary>
        /// 群名改变信息
        /// </summary>
        /// <param name="e">群名改变句柄</param>
        public delegate void GroupNameChangeEvent(Event.GroupNameChangeEvent e);
        /// <summary>
        /// 某个群名改变了
        /// </summary>
        public event GroupNameChangeEvent OnEventGroupNameChangeEvent;
        /// <summary>
        /// 某个群的入群公告改变
        /// </summary>
        /// <param name="e">群公告改变句柄</param>
        public delegate void GroupEntranceAnnouncementChangeEvent(Event.GroupEntranceAnnouncementChangeEvent e);
        /// <summary>
        /// 某个群的入群公告改变了
        /// </summary>
        public event GroupEntranceAnnouncementChangeEvent OnEventGroupEntranceAnnouncementChangeEvent;
        /// <summary>
        /// 群全员禁言
        /// </summary>
        /// <param name="e">全员禁言状态句柄</param>
        public delegate void GroupMuteAllEvent(Event.GroupMuteAllEvent e);
        /// <summary>
        /// 某个群更改了全员禁言状态
        /// </summary>
        public event GroupMuteAllEvent OnEventGroupMuteAllEvent;
        /// <summary>
        /// 群匿名聊天
        /// </summary>
        /// <param name="e">全群匿名状态句柄</param>
        public delegate void GroupAllowAnonymousChatEvent(Event.GroupAllowAnonymousChatEvent e);
        /// <summary>
        /// 某个群更改了匿名聊天状态
        /// </summary>
        public event GroupAllowAnonymousChatEvent OnEventGroupAllowAnonymousChatEvent;
        /// <summary>
        /// 群坦白说
        /// </summary>
        /// <param name="e">群坦白说状态句柄</param>
        public delegate void GroupAllowConfessTalkEvent(Event.GroupAllowConfessTalkEvent e);
        /// <summary>
        /// 某个群更改了坦白说的状态
        /// </summary>
        public event GroupAllowConfessTalkEvent OnEventGroupAllowConfessTalkEvent;
        /// <summary>
        /// 群员邀请好友加群
        /// </summary>
        /// <param name="e">群员邀请加群句柄</param>
        public delegate void GroupAllowMemberInviteEvent(Event.GroupAllowMemberInviteEvent e);
        /// <summary>
        /// 某个群员邀请好友加群
        /// </summary>
        public event GroupAllowMemberInviteEvent OnEventGroupAllowMemberInviteEvent;
        /// <summary>
        /// 新人入群
        /// </summary>
        /// <param name="e">新人入群句柄</param>
        public delegate void MemberJoinEvent(Event.MemberJoinEvent e);
        /// <summary>
        /// 某群有新人入群了
        /// </summary>
        public event MemberJoinEvent OnEventMemberJoinEvent;
        /// <summary>
        /// 某人被踢(不是Bot)
        /// </summary>
        /// <param name="e">被踢句柄</param>
        public delegate void MemberLeaveEventKick(Event.MemberLeaveEventKick e);
        /// <summary>
        /// 某群把某人踢出了(不是Bot)
        /// </summary>
        public event MemberLeaveEventKick OnEventMemberLeaveEventKick;
        /// <summary>
        /// 成员主动退群
        /// </summary>
        /// <param name="e">群员主动离群句柄</param>
        public delegate void MemberLeaveEventQuit(Event.MemberLeaveEventQuit e);
        /// <summary>
        /// 某群有成员主动退群了
        /// </summary>
        public event MemberLeaveEventQuit OnEventMemberLeaveEventQuit;
        /// <summary>
        /// 群名片改动
        /// </summary>
        /// <param name="e">群名片改动句柄</param>
        public delegate void MemberCardChangeEvent(Event.MemberCardChangeEvent e);
        /// <summary>
        /// 某群有人的群名片改动了
        /// </summary>
        public event MemberCardChangeEvent OnEventCardChangeEvent;
        /// <summary>
        /// 群头衔改动
        /// </summary>
        /// <param name="e">头衔改动句柄</param>
        public delegate void MemberSpecialTitleChangeEvent(Event.MemberSpecialTitleChangeEvent e);
        /// <summary>
        /// 某群有某个群主改动了某人头衔
        /// </summary>
        public event MemberSpecialTitleChangeEvent OnEventSpecialTitleChangeEvent;
        /// <summary>
        /// 某个成员权限改变
        /// </summary>
        /// <param name="e"></param>
        public delegate void MemberPermissionChangeEvent(Event.MemberPermissionChangeEvent e);
        /// <summary>
        /// 某群有某个成员权限被改变了(不是Bot)
        /// </summary>
        public event MemberPermissionChangeEvent OnEventPermissionChangeEvent;
        /// <summary>
        /// 群成员被禁言
        /// </summary>
        /// <param name="e">成员禁言句柄</param>
        public delegate void MemberMuteEvent(Event.MemberMuteEvent e);
        /// <summary>
        /// 某群的某个群成员被禁言
        /// </summary>
        public event MemberMuteEvent OnEventMemberMuteEvent;
        /// <summary>
        /// 群成员被取消禁言
        /// </summary>
        /// <param name="e">成员取消禁言句柄</param>
        public delegate void MemberUnmuteEvent(Event.MemberUnmuteEvent e);
        /// <summary>
        /// 某群的某个群成员被取消禁言
        /// </summary>
        public event MemberUnmuteEvent OnEventMemberUnmuteEvent;
        /// <summary>
        /// 成员称号改变
        /// </summary>
        /// <param name="e">称号改变句柄</param>
        public delegate void MemberHonorChangeEvent(Event.MemberHonorChangeEvent e);
        /// <summary>
        /// 某群的某个成员的群称号改变
        /// </summary>
        public event MemberHonorChangeEvent OnEventMemberHonorChangeEvent;
        /// <summary>
        /// 新好友请求
        /// </summary>
        /// <param name="e">好友请求句柄</param>
        public delegate void NewFriendRequestEvent(Event.NewFriendRequestEvent e);
        /// <summary>
        /// 接收到新好友请求
        /// </summary>
        public event NewFriendRequestEvent OnEventNewFriendRequestEvent;
        /// <summary>
        /// 用户入群申请
        /// </summary>
        /// <param name="e">用户入群申请句柄</param>
        public delegate void MemberJoinRequestEvent(Event.MemberJoinRequestEvent e);
        /// <summary>
        /// 接收到用户入群申请
        /// </summary>
        public event MemberJoinRequestEvent OnEventMemberJoinRequestEvent;
        /// <summary>
        /// Bot被邀请入群
        /// </summary>
        /// <param name="e">被邀请入群句柄</param>
        public delegate void BotInvitedJoinGroupRequestEvent(Event.BotInvitedJoinGroupRequestEvent e);
        /// <summary>
        /// 接收到Bot被邀请入群申请
        /// </summary>
        public event BotInvitedJoinGroupRequestEvent OnEventBotInvitedJoinGroupRequestEvent;
    }
}

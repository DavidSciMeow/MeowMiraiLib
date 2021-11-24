using MeowMiraiLib.Msg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* 所有事件模型文件 
 * 本文件类解析方案与网页 https://github.com/project-mirai/mirai-api-http/blob/master/docs/api/EventType.md 完全一致
 * 本文件用于储存一切Mirai触发的事件,请勿修改内部的包含类或者使用反射修改类内内容,
 * 在逻辑设计上,本文件内部所有类均不可继承(除了基类定义),其值也仅为只读.
 * -------------------
 * Event;
 * this file still is follow the website of https://github.com/project-mirai/mirai-api-http/blob/master/docs/api/EventType.md
 * this file is holder of all Mirai-Event, do not change the classes defines.
 * also don't use reflection to change inner value.
 * and most importantly all classes inside this file (except MiraiEvent) is Final-classes [or sealed-class]
 * and classes' properties values is readonly.
 */

namespace MeowMiraiLib.Event
{
    #region EventDefines -- 事件基础定义
    /// <summary>
    /// Mirai事件继承基类
    /// </summary>
    public class MiraiEvent : EventArgs
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public EventType type;
    }
    /// <summary>
    /// 好友类
    /// </summary>
    public sealed class MiraiFriend
    {
        /// <summary>
        /// 好友qq号
        /// </summary>
        public long id;
        /// <summary>
        /// 好友昵称
        /// </summary>
        public string nickname;
        /// <summary>
        /// 好友备注
        /// </summary>
        public string remark;
        /// <summary>
        /// 实例化好友类
        /// </summary>
        /// <param name="id">QQ号{Friend->id}</param>
        /// <param name="nickname">昵称{Friend->nickname}</param>
        /// <param name="remark">备注{Friend->remark}</param>
        public MiraiFriend(long id, string nickname, string remark)
        {
            this.id = id;
            this.nickname = nickname;
            this.remark = remark;
        }
    }
    /// <summary>
    /// 群类
    /// </summary>
    public sealed class MiraiGroup
    {
        /// <summary>
        /// 群号
        /// </summary>
        public long id;
        /// <summary>
        /// 群名
        /// </summary>
        public string name;
        /// <summary>
        /// 外围基类(包含)的对应人在群的权限
        /// </summary>
        public string permission;
        /// <summary>
        /// 实例化一个群类
        /// </summary>
        /// <param name="id">群号{group->id}</param>
        /// <param name="name">群名{group->name}</param>
        /// <param name="permission">群权限{group->permission}</param>
        public MiraiGroup(long id, string name, string permission)
        {
            this.id = id;
            this.name = name;
            this.permission = permission;
        }
    }
    /// <summary>
    /// 长成员类
    /// </summary>
    public sealed class MiraiMemberHolder
    {
        /// <summary>
        /// 成员QQ号
        /// </summary>
        public long id;
        /// <summary>
        /// 成员名称
        /// </summary>
        public string memberName;
        /// <summary>
        /// 成员群头衔
        /// </summary>
        public string specialTitle;
        /// <summary>
        /// 成员权限
        /// </summary>
        public string permission;
        /// <summary>
        /// 加入时的时间戳
        /// </summary>
        public long joinTimestamp;
        /// <summary>
        /// 最后一次说话时间戳
        /// </summary>
        public long lastSpeakTimestamp;
        /// <summary>
        /// 禁言剩余时间
        /// </summary>
        public long muteTimeRemaining;
        /// <summary>
        /// 引用成员的群类
        /// </summary>
        public MiraiGroup group;
        /// <summary>
        /// 生成一个长成员
        /// </summary>
        /// <param name="id"></param>
        /// <param name="memberName"></param>
        /// <param name="specialTitle"></param>
        /// <param name="permission"></param>
        /// <param name="joinTimestamp"></param>
        /// <param name="lastSpeakTimestamp"></param>
        /// <param name="muteTimeRemaining"></param>
        /// <param name="group"></param>
        public MiraiMemberHolder(long id, string memberName, string specialTitle, 
            string permission, long joinTimestamp, long lastSpeakTimestamp, 
            long muteTimeRemaining, MiraiGroup group)
        {
            this.id = id;
            this.memberName = memberName;
            this.specialTitle = specialTitle;
            this.permission = permission;
            this.joinTimestamp = joinTimestamp;
            this.lastSpeakTimestamp = lastSpeakTimestamp;
            this.muteTimeRemaining = muteTimeRemaining;
            this.group = group;
        }
    }
    /// <summary>
    /// 短成员类
    /// </summary>
    public sealed class MiraiMemberHolderShort
    {
        /// <summary>
        /// 短成员QQ号
        /// </summary>
        public long id;
        /// <summary>
        /// 短成员昵称
        /// </summary>
        public string memberName;
        /// <summary>
        /// 短成员权限
        /// </summary>
        public string permission;
        /// <summary>
        /// 引用短成员的群类
        /// </summary>
        public MiraiGroup group;
        /// <summary>
        /// 生成一个短成员
        /// </summary>
        /// <param name="id"></param>
        /// <param name="memberName"></param>
        /// <param name="permission"></param>
        /// <param name="group"></param>
        public MiraiMemberHolderShort(long id, string memberName, string permission, MiraiGroup group)
        {
            this.id = id;
            this.memberName = memberName;
            this.permission = permission;
            this.group = group;
        }
    }
    #endregion

    #region Event & EventClasses -- 事件和事件内容
    /// <summary>
    /// 可接收到的逻辑事件类型
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// Bot登陆成功
        /// </summary>
        BotOnlineEvent,
        /// <summary>
        /// Bot主动离线
        /// </summary>
        BotOfflineEventActive,
        /// <summary>
        /// Bot被挤下线
        /// </summary>
        BotOfflineEventForce,
        /// <summary>
        /// Bot被服务器断开(网络问题)
        /// </summary>
        BotOfflineEventDropped,
        /// <summary>
        /// Bot主动重新登录
        /// </summary>
        BotReloginEvent,
        /// <summary>
        /// 好友输入状态改变
        /// </summary>
        FriendInputStatusChangedEvent,
        /// <summary>
        /// 好友昵称改变
        /// </summary>
        FriendNickChangedEvent,
        /// <summary>
        /// Bot在群里的权限改变
        /// </summary>
        BotGroupPermissionChangeEvent,
        /// <summary>
        /// Bot被禁言
        /// </summary>
        BotMuteEvent,
        /// <summary>
        /// Bot被解除禁言
        /// </summary>
        BotUnmuteEvent,
        /// <summary>
        /// Bot自身入群事件
        /// </summary>
        BotJoinGroupEvent,
        /// <summary>
        /// Bot主动退出一个群
        /// </summary>
        BotLeaveEventActive,
        /// <summary>
        /// Bot被踢出一个群
        /// </summary>
        BotLeaveEventKick,
        /// <summary>
        /// 群消息撤回
        /// </summary>
        GroupRecallEvent,
        /// <summary>
        /// 好友消息撤回
        /// </summary>
        FriendRecallEvent,
        /// <summary>
        /// 戳一戳
        /// </summary>
        NudgeEvent,
        /// <summary>
        /// 某个群名改变
        /// </summary>
        GroupNameChangeEvent,
        /// <summary>
        /// 某群入群公告改变
        /// </summary>
        GroupEntranceAnnouncementChangeEvent,
        /// <summary>
        /// 全员禁言
        /// </summary>
        GroupMuteAllEvent,
        /// <summary>
        /// 匿名聊天
        /// </summary>
        GroupAllowAnonymousChatEvent,
        /// <summary>
        /// 坦白说
        /// </summary>
        GroupAllowConfessTalkEvent,
        /// <summary>
        /// 允许群员邀请好友加群
        /// </summary>
        GroupAllowMemberInviteEvent,
        /// <summary>
        /// 新人入群
        /// </summary>
        MemberJoinEvent,
        /// <summary>
        /// 成员被踢出(不是Bot)
        /// </summary>
        MemberLeaveEventKick,
        /// <summary>
        /// 成员主动离开(不是Bot)
        /// </summary>
        MemberLeaveEventQuit,
        /// <summary>
        /// 群名片改动
        /// </summary>
        MemberCardChangeEvent,
        /// <summary>
        /// 群头衔改动(仅群主操作)
        /// </summary>
        MemberSpecialTitleChangeEvent,
        /// <summary>
        /// 成员权限改变(不是Bot)
        /// </summary>
        MemberPermissionChangeEvent,
        /// <summary>
        /// 群成员被禁言事件(不是Bot)
        /// </summary>
        MemberMuteEvent,
        /// <summary>
        /// 群成员被取消禁言事件(不是Bot)
        /// </summary>
        MemberUnmuteEvent,
        /// <summary>
        /// 群员称号改变
        /// </summary>
        MemberHonorChangeEvent,
        /// <summary>
        /// 添加好友申请
        /// </summary>
        NewFriendRequestEvent,
        /// <summary>
        /// bot被邀请入群申请
        /// </summary>
        BotInvitedJoinGroupRequestEvent,
        /// <summary>
        /// 其他客户端上线
        /// </summary>
        OtherClientOnlineEvent,
        /// <summary>
        /// 其他客户端下线
        /// </summary>
        OtherClientOfflineEvent,
        /// <summary>
        /// 命令执行反馈
        /// </summary>
        CommandExecutedEvent
    }
    /// <summary>
    /// 命令执行反馈
    /// </summary>
    public sealed class CommandExecutedEvent : MiraiEvent
    {
        /// <summary>
        /// 命令名称
        /// </summary>
        public string name;
        /// <summary>
        /// 发送命令的好友, 从控制台发送为 null
        /// </summary>
        public object friend;
        /// <summary>
        /// 发送命令的群成员, 从控制台发送为 null
        /// </summary>
        public object memeber;
        /// <summary>
        /// 指令的参数, 以消息类型传递
        /// </summary>
        public List<(string type, string text)> args;
        /// <summary>
        /// 命令执行反馈
        /// </summary>
        /// <param name="name">命令名称</param>
        /// <param name="friend">发送命令的好友, 从控制台发送为 null</param>
        /// <param name="memeber">发送命令的群成员, 从控制台发送为 null</param>
        /// <param name="args">指令的参数, 以消息类型传递</param>
        public CommandExecutedEvent(string name, object friend, 
            object memeber, List<(string type, string text)> args)
        {
            this.name = name;
            this.friend = friend;
            this.memeber = memeber;
            this.args = args;
        }
    }
    /// <summary>
    /// 其他客户端下线
    /// </summary>
    public sealed class OtherClientOfflineEvent : MiraiEvent
    {
        /// <summary>
        /// 客户端标识号
        /// </summary>
        public long id;
        /// <summary>
        /// 客户端类型
        /// </summary>
        public string platform;
        /// <summary>
        /// 其他客户端下线
        /// </summary>
        /// <param name="id">客户端标识号</param>
        /// <param name="platform">客户端类型</param>
        public OtherClientOfflineEvent(long id, string platform)
        {
            this.type = EventType.OtherClientOfflineEvent;
            this.id = id;
            this.platform = platform;
        }
    }
    /// <summary>
    /// 其他客户端上线
    /// </summary>
    public sealed class OtherClientOnlineEvent : MiraiEvent
    {
        /// <summary>
        /// 客户端标识号
        /// </summary>
        public long id;
        /// <summary>
        /// 客户端类型
        /// </summary>
        public string platform;
        /// <summary>
        /// 详细设备类型
        /// </summary>
        public long? kind;
        /// <summary>
        /// 其他客户端上线
        /// </summary>
        /// <param name="id">客户端标识号</param>
        /// <param name="platform">客户端类型</param>
        /// <param name="kind">详细设备类型</param>
        public OtherClientOnlineEvent(long id, string platform, long? kind)
        {
            this.type = EventType.OtherClientOnlineEvent;
            this.id = id;
            this.platform = platform;
            this.kind = kind;
        }
    }
    /// <summary>
    /// bot被邀请入群申请
    /// </summary>
    public sealed class BotInvitedJoinGroupRequestEvent : MiraiEvent
    {
        /// <summary>
        /// 事件标识，响应该事件时的标识
        /// </summary>
        public long eventId;
        /// <summary>
        /// 邀请人（好友）的QQ号
        /// </summary>
        public long fromId;
        /// <summary>
        /// 被邀请进入群的群号
        /// </summary>
        public long groupId;
        /// <summary>
        /// 被邀请进入群的群名称
        /// </summary>
        public string groupName;
        /// <summary>
        /// 邀请人（好友）的昵称
        /// </summary>
        public string nick;
        /// <summary>
        /// 邀请消息
        /// </summary>
        public string message;
        /// <summary>
        /// bot被邀请入群申请
        /// </summary>
        /// <param name="eventId">事件标识，响应该事件时的标识</param>
        /// <param name="fromId">邀请人（好友）的QQ号</param>
        /// <param name="groupId">被邀请进入群的群号</param>
        /// <param name="groupName">被邀请进入群的群名称</param>
        /// <param name="nick">邀请人（好友）的昵称</param>
        /// <param name="message">邀请消息</param>
        public BotInvitedJoinGroupRequestEvent(
            long eventId, long fromId, long groupId, 
            string groupName, string nick, string message)
        {
            this.type = EventType.BotInvitedJoinGroupRequestEvent;
            this.eventId = eventId;
            this.fromId = fromId;
            this.groupId = groupId;
            this.groupName = groupName;
            this.nick = nick;
            this.message = message;
        }
    }
    /// <summary>
    /// 用户入群申请
    /// </summary>
    public sealed class MemberJoinRequestEvent : MiraiEvent
    {
        /// <summary>
        /// 事件标识，响应该事件时的标识
        /// </summary>
        public long eventId;
        /// <summary>
        /// 申请人QQ号
        /// </summary>
        public long fromId;
        /// <summary>
        /// 申请人申请入群的群号
        /// </summary>
        public long groupId;
        /// <summary>
        /// 申请人申请入群的群名称
        /// </summary>
        public string groupName;
        /// <summary>
        /// 申请人的昵称或群名片
        /// </summary>
        public string nick;
        /// <summary>
        /// 申请消息
        /// </summary>
        public string message;
        /// <summary>
        /// 用户入群申请
        /// </summary>
        /// <param name="eventId">事件标识，响应该事件时的标识</param>
        /// <param name="fromId">申请人QQ号</param>
        /// <param name="groupId">申请人申请入群的群号</param>
        /// <param name="groupName">申请人申请入群的群名称</param>
        /// <param name="nick">申请人的昵称或群名片</param>
        /// <param name="message">申请消息</param>
        public MemberJoinRequestEvent(
            long eventId, long fromId, long groupId,
            string groupName, string nick, string message)
        {
            this.type = EventType.NewFriendRequestEvent;
            this.eventId = eventId;
            this.fromId = fromId;
            this.groupId = groupId;
            this.groupName = groupName;
            this.nick = nick;
            this.message = message;
        }
    }
    /// <summary>
    /// 添加好友申请
    /// </summary>
    public sealed class NewFriendRequestEvent : MiraiEvent
    {
        /// <summary>
        /// 事件标识，响应该事件时的标识
        /// </summary>
        public long eventId;
        /// <summary>
        /// 申请人QQ号
        /// </summary>
        public long fromId;
        /// <summary>
        /// 申请人如果通过某个群添加好友，该项为该群群号；否则为0
        /// </summary>
        public long groupId;
        /// <summary>
        /// 申请人的昵称或群名片
        /// </summary>
        public string nick;
        /// <summary>
        /// 申请消息
        /// </summary>
        public string message;
        /// <summary>
        /// 添加好友申请
        /// </summary>
        /// <param name="eventId">事件标识，响应该事件时的标识</param>
        /// <param name="fromId">申请人QQ号</param>
        /// <param name="groupId">申请人如果通过某个群添加好友，该项为该群群号；否则为0</param>
        /// <param name="nick">申请人的昵称或群名片</param>
        /// <param name="message">申请消息</param>
        public NewFriendRequestEvent(
            long eventId, long fromId, long groupId,
            string nick, string message)
        {
            this.type = EventType.NewFriendRequestEvent;
            this.eventId = eventId;
            this.fromId = fromId;
            this.groupId = groupId;
            this.nick = nick;
            this.message = message;
        }
    }
    /// <summary>
    /// 群员称号改变
    /// </summary>
    public sealed class MemberHonorChangeEvent : MiraiEvent
    {
        /// <summary>
        /// 被取消禁言的群员的信息
        /// </summary>
        public MiraiMemberHolder member;
        /// <summary>
        /// 称号变化行为：achieve获得称号，lose失去称号
        /// </summary>
        public string action;
        /// <summary>
        /// 称号名称
        /// </summary>
        public string honor;
        /// <summary>
        /// 群员称号改变
        /// </summary>
        /// <param name="member">被取消禁言的群员的信息</param>
        /// <param name="action">称号变化行为：achieve获得称号，lose失去称号</param>
        /// <param name="honor">称号名称</param>
        public MemberHonorChangeEvent(MiraiMemberHolder member, string action, string honor)
        {
            this.type = EventType.MemberHonorChangeEvent;
            this.member = member;
            this.action = action;
            this.honor = honor;
        }
    }
    /// <summary>
    /// 群成员被取消禁言事件(不是Bot)
    /// </summary>
    public sealed class MemberUnmuteEvent : MiraiEvent
    {
        /// <summary>
        /// 被取消禁言的群员的信息
        /// </summary>
        public MiraiMemberHolder member;
        /// <summary>
        /// 操作者的信息，当null时为Bot操作
        /// </summary>
        public MiraiMemberHolder @operator;
        /// <summary>
        /// 群成员被取消禁言事件(不是Bot)
        /// </summary>
        /// <param name="member">被取消禁言的群员的信息</param>
        /// <param name="operator">操作者的信息，当null时为Bot操作</param>
        public MemberUnmuteEvent(MiraiMemberHolder member, MiraiMemberHolder @operator)
        {
            this.type = EventType.MemberUnmuteEvent;
            this.member = member;
            this.@operator = @operator;
        }
    }
    /// <summary>
    /// 群成员被禁言事件(不是Bot)
    /// </summary>
    public sealed class MemberMuteEvent : MiraiEvent
    {
        /// <summary>
        /// 禁言时长，单位为秒
        /// </summary>
        public long durationSecond;
        /// <summary>
        /// 被禁言的群员的信息
        /// </summary>
        public MiraiMemberHolder member;
        /// <summary>
        /// 操作者的信息，当null时为Bot操作
        /// </summary>
        public MiraiMemberHolder @operator;
        /// <summary>
        /// 群成员被禁言事件(不是Bot)
        /// </summary>
        /// <param name="durationSecond">禁言时长，单位为秒</param>
        /// <param name="member">被禁言的群员的信息</param>
        /// <param name="operator">操作者的信息，当null时为Bot操作</param>
        public MemberMuteEvent(long durationSecond, MiraiMemberHolder member, MiraiMemberHolder @operator)
        {
            this.type = EventType.MemberMuteEvent;
            this.durationSecond = durationSecond;
            this.member = member;
            this.@operator = @operator;
        }
    }
    /// <summary>
    /// 成员权限改变(不是Bot)
    /// </summary>
    public sealed class MemberPermissionChangeEvent : MiraiEvent
    {
        /// <summary>
        /// 原权限
        /// </summary>
        public string origin;
        /// <summary>
        /// 现权限
        /// </summary>
        public string current;
        /// <summary>
        /// 权限改动的群员的信息
        /// </summary>
        public MiraiMemberHolderShort member;
        /// <summary>
        /// 成员权限改变(不是Bot)
        /// </summary>
        /// <param name="origin">原权限</param>
        /// <param name="current">现权限</param>
        /// <param name="member">权限改动的群员的信息</param>
        public MemberPermissionChangeEvent(string origin, string current, MiraiMemberHolderShort member)
        {
            this.type = EventType.MemberPermissionChangeEvent;
            this.origin = origin;
            this.current = current;
            this.member = member;
        }
    }
    /// <summary>
    /// 群头衔改动(仅群主操作)
    /// </summary>
    public sealed class MemberSpecialTitleChangeEvent : MiraiEvent
    {
        /// <summary>
        /// 原头衔
        /// </summary>
        public string origin;
        /// <summary>
        /// 现头衔
        /// </summary>
        public string current;
        /// <summary>
        /// 头衔改动的群员的信息
        /// </summary>
        public MiraiMemberHolderShort member;
        /// <summary>
        /// 群头衔改动(仅群主操作)
        /// </summary>
        /// <param name="origin">原头衔</param>
        /// <param name="current">现头衔</param>
        /// <param name="member">头衔改动的群员的信息</param>
        public MemberSpecialTitleChangeEvent(string origin, string current, MiraiMemberHolderShort member)
        {
            this.type = EventType.MemberSpecialTitleChangeEvent;
            this.origin = origin;
            this.current = current;
            this.member = member;
        }
    }
    /// <summary>
    /// 群名片改动
    /// </summary>
    public sealed class MemberCardChangeEvent : MiraiEvent
    {
        /// <summary>
        /// 原本名片
        /// </summary>
        public string origin;
        /// <summary>
        /// 现在名片
        /// </summary>
        public string current;
        /// <summary>
        /// 名片改动的群员的信息
        /// </summary>
        public MiraiMemberHolder member;
        /// <summary>
        /// 群名片改动
        /// </summary>
        /// <param name="origin">原本名片</param>
        /// <param name="current">现在名片</param>
        /// <param name="member">名片改动的群员的信息</param>
        public MemberCardChangeEvent(string origin, string current, MiraiMemberHolder member)
        {
            this.type = EventType.MemberCardChangeEvent;
            this.origin = origin;
            this.current = current;
            this.member = member;
        }
    }
    /// <summary>
    /// 成员主动离开(不是Bot)
    /// </summary>
    public sealed class MemberLeaveEventQuit : MiraiEvent
    {
        /// <summary>
        /// 退群群员的信息
        /// </summary>
        public MiraiMemberHolderShort member;
        /// <summary>
        /// 成员主动离开(不是Bot)
        /// </summary>
        /// <param name="member">退群群员的信息</param>
        public MemberLeaveEventQuit( MiraiMemberHolderShort member)
        {
            this.type = EventType.MemberLeaveEventKick;
            this.member = member;
        }
    }
    /// <summary>
    /// 成员被踢出(不是Bot)
    /// </summary>
    public sealed class MemberLeaveEventKick : MiraiEvent
    {
        /// <summary>
        /// 被踢者的信息
        /// </summary>
        public MiraiMemberHolder member;
        /// <summary>
        /// 操作的管理员或群主信息，当null时为Bot操作
        /// </summary>
        public MiraiMemberHolder @operator;
        /// <summary>
        /// 成员被踢出(不是Bot)
        /// </summary>
        /// <param name="member">被踢者的信息</param>
        /// <param name="operator">操作的管理员或群主信息，当null时为Bot操作</param>
        public MemberLeaveEventKick(MiraiMemberHolder member, MiraiMemberHolder @operator)
        {
            this.type = EventType.MemberLeaveEventKick;
            this.member = member;
            this.@operator = @operator;
        }
    }
    /// <summary>
    /// 新人入群
    /// </summary>
    public sealed class MemberJoinEvent : MiraiEvent
    {
        /// <summary>
        /// 新人信息
        /// </summary>
        public MiraiMemberHolder member;
        /// <summary>
        /// 如果被要求入群的话，则为邀请人的 Member 对象
        /// </summary>
        public object invitor;
        /// <summary>
        /// 新人入群
        /// </summary>
        /// <param name="member">新人信息</param>
        /// <param name="invitor">如果被要求入群的话，则为邀请人的 Member 对象</param>
        public MemberJoinEvent(MiraiMemberHolder member, object invitor)
        {
            this.type = EventType.MemberJoinEvent;
            this.member = member;
            this.invitor = invitor;
        }
    }
    /// <summary>
    /// 允许群员邀请好友加群
    /// </summary>
    public sealed class GroupAllowMemberInviteEvent : MiraiEvent
    {
        /// <summary>
        /// 原本是否允许群员邀请好友加群
        /// </summary>
        public bool origin;
        /// <summary>
        /// 现在是否允许群员邀请好友加群
        /// </summary>
        public bool current;
        /// <summary>
        /// 允许群员邀请好友加群状态改变的群信息
        /// </summary>
        public MiraiGroup group;
        /// <summary>
        /// 操作的管理员或群主信息，当null时为Bot操作
        /// </summary>
        public MiraiMemberHolder @operator;
        /// <summary>
        /// 允许群员邀请好友加群
        /// </summary>
        /// <param name="origin">原本是否允许群员邀请好友加群</param>
        /// <param name="current">现在是否允许群员邀请好友加群</param>
        /// <param name="group">允许群员邀请好友加群状态改变的群信息</param>
        /// <param name="operator">操作的管理员或群主信息，当null时为Bot操作</param>
        public GroupAllowMemberInviteEvent(bool origin, bool current, MiraiGroup group, MiraiMemberHolder @operator)
        {
            this.type = EventType.GroupAllowMemberInviteEvent;
            this.origin = origin;
            this.current = current;
            this.group = group;
            this.@operator = @operator;
        }
    }
    /// <summary>
    /// 坦白说
    /// </summary>
    public sealed class GroupAllowConfessTalkEvent : MiraiEvent
    {
        /// <summary>
        /// 原本坦白说是否开启
        /// </summary>
        public bool origin;
        /// <summary>
        /// 现在坦白说是否开启
        /// </summary>
        public bool current;
        /// <summary>
        /// 坦白说状态改变的群信息
        /// </summary>
        public MiraiGroup group;
        /// <summary>
        /// 是否Bot进行该操作
        /// </summary>
        public bool isByBot;
        /// <summary>
        /// 坦白说
        /// </summary>
        /// <param name="origin">原本坦白说是否开启</param>
        /// <param name="current">现在坦白说是否开启</param>
        /// <param name="group">坦白说状态改变的群信息</param>
        /// <param name="isByBot">是否Bot进行该操作</param>
        public GroupAllowConfessTalkEvent(bool origin, bool current, MiraiGroup group, bool isByBot)
        {
            this.type = EventType.GroupAllowConfessTalkEvent;
            this.origin = origin;
            this.current = current;
            this.group = group;
            this.isByBot = isByBot;
        }
    }
    /// <summary>
    /// 匿名聊天
    /// </summary>
    public sealed class GroupAllowAnonymousChatEvent : MiraiEvent
    {
        /// <summary>
        /// 原本匿名聊天是否开启
        /// </summary>
        public bool origin;
        /// <summary>
        /// 现在匿名聊天是否开启
        /// </summary>
        public bool current;
        /// <summary>
        /// 匿名聊天状态改变的群信息
        /// </summary>
        public MiraiGroup group;
        /// <summary>
        /// 操作的管理员或群主信息，当null时为Bot操作
        /// </summary>
        public MiraiMemberHolder @operator;
        /// <summary>
        /// 匿名聊天
        /// </summary>
        /// <param name="origin">原本匿名聊天是否开启</param>
        /// <param name="current">现在匿名聊天是否开启</param>
        /// <param name="group">匿名聊天状态改变的群信息</param>
        /// <param name="operator">操作的管理员或群主信息，当null时为Bot操作</param>
        public GroupAllowAnonymousChatEvent(bool origin, bool current, MiraiGroup group, MiraiMemberHolder @operator)
        {
            this.type = EventType.GroupAllowAnonymousChatEvent;
            this.origin = origin;
            this.current = current;
            this.group = group;
            this.@operator = @operator;
        }
    }
    /// <summary>
    /// 全员禁言
    /// </summary>
    public sealed class GroupMuteAllEvent : MiraiEvent
    {
        /// <summary>
        /// 原本是否处于全员禁言
        /// </summary>
        public bool origin;
        /// <summary>
        /// 现在是否处于全员禁言
        /// </summary>
        public bool current;
        /// <summary>
        /// 全员禁言的群信息
        /// </summary>
        public MiraiGroup group;
        /// <summary>
        /// 操作的管理员或群主信息，当null时为Bot操作
        /// </summary>
        public MiraiMemberHolder @operator;
        /// <summary>
        /// 全员禁言
        /// </summary>
        /// <param name="origin">原本是否处于全员禁言</param>
        /// <param name="current">现在是否处于全员禁言</param>
        /// <param name="group">全员禁言的群信息</param>
        /// <param name="operator">操作的管理员或群主信息，当null时为Bot操作</param>
        public GroupMuteAllEvent(
            bool origin, bool current, 
            MiraiGroup group, MiraiMemberHolder @operator)
        {
            this.type = EventType.GroupMuteAllEvent;
            this.origin = origin;
            this.current = current;
            this.group = group;
            this.@operator = @operator;
        }
    }
    /// <summary>
    /// 某群入群公告改变
    /// </summary>
    public sealed class GroupEntranceAnnouncementChangeEvent : MiraiEvent
    {
        /// <summary>
        /// 原公告
        /// </summary>
        public string origin;
        /// <summary>
        /// 新公告
        /// </summary>
        public string current;
        /// <summary>
        /// 公告改变的群信息
        /// </summary>
        public MiraiGroup group;
        /// <summary>
        /// 操作的管理员或群主信息，当null时为Bot操作
        /// </summary>
        public MiraiMemberHolder @operator;
        /// <summary>
        /// 某群入群公告改变
        /// </summary>
        /// <param name="origin">原公告</param>
        /// <param name="current">新公告</param>
        /// <param name="group">公告改变的群信息</param>
        /// <param name="operator">操作的管理员或群主信息，当null时为Bot操作</param>
        public GroupEntranceAnnouncementChangeEvent(
            string origin, string current, 
            MiraiGroup group, MiraiMemberHolder @operator)
        {
            this.type = EventType.GroupEntranceAnnouncementChangeEvent;
            this.origin = origin;
            this.current = current;
            this.group = group;
            this.@operator = @operator;
        }
    }
    /// <summary>
    /// 某个群名改变
    /// </summary>
    public sealed class GroupNameChangeEvent : MiraiEvent
    {
        /// <summary>
        /// 原群名
        /// </summary>
        public string origin;
        /// <summary>
        /// 新群名
        /// </summary>
        public string current;
        /// <summary>
        /// 群名改名的群信息
        /// </summary>
        public MiraiGroup group;
        /// <summary>
        /// 操作的管理员或群主信息，当null时为Bot操作
        /// </summary>
        public MiraiMemberHolder @operator;
        /// <summary>
        /// 某个群名改变
        /// </summary>
        /// <param name="origin">原群名</param>
        /// <param name="current">新群名</param>
        /// <param name="group">群名改名的群信息</param>
        /// <param name="operator">操作的管理员或群主信息，当null时为Bot操作</param>
        public GroupNameChangeEvent(
            string origin, string current, 
            MiraiGroup group, MiraiMemberHolder @operator)
        {
            this.type = EventType.GroupNameChangeEvent;
            this.origin = origin;
            this.current = current;
            this.group = group;
            this.@operator = @operator;
        }
    }
    /// <summary>
    /// 戳一戳
    /// </summary>
    public sealed class NudgeEvent : MiraiEvent
    {
        /// <summary>
        /// 动作发出者的QQ号
        /// </summary>
        public long fromId;
        /// <summary>
        /// 动作目标的QQ号
        /// </summary>
        public long target;
        /// <summary>
        /// 来源的类型，"Friend"或"Group"
        /// </summary>
        public string fromKind;
        /// <summary>
        /// 来源的QQ号（好友）或群号
        /// </summary>
        public long fromKindId;
        /// <summary>
        /// 动作类型
        /// </summary>
        public string action;
        /// <summary>
        /// 自定义动作内容
        /// </summary>
        public string suffix;
        /// <summary>
        /// 戳一戳
        /// </summary>
        /// <param name="fromId">动作发出者的QQ号</param>
        /// <param name="target">动作目标的QQ号</param>
        /// <param name="fromKind">来源的类型，"Friend"或"Group"</param>
        /// <param name="fromKindId">来源的QQ号（好友）或群号</param>
        /// <param name="action">动作类型</param>
        /// <param name="suffix">自定义动作内容</param>
        public NudgeEvent(
            long fromId, long target, string fromKind,
            long fromKindId, string action, string suffix)
        {
            this.type = EventType.NudgeEvent;
            this.fromId = fromId;
            this.target = target;
            this.fromKind = fromKind;
            this.fromKindId = fromKindId;
            this.action = action;
            this.suffix = suffix;
        }
    }
    /// <summary>
    /// 好友消息撤回
    /// </summary>
    public sealed class FriendRecallEvent : MiraiEvent
    {
        /// <summary>
        /// 原消息发送者的QQ号
        /// </summary>
        public long authodId;
        /// <summary>
        /// 原消息messageId
        /// </summary>
        public long messageId;
        /// <summary>
        /// 原消息发送时间
        /// </summary>
        public long time;
        /// <summary>
        /// 好友QQ号或BotQQ号
        /// </summary>
        public long @operator;
        /// <summary>
        /// 好友消息撤回
        /// </summary>
        /// <param name="authodId">原消息发送者的QQ号</param>
        /// <param name="messageId">原消息messageId</param>
        /// <param name="time">原消息发送时间</param>
        /// <param name="operator">好友QQ号或BotQQ号</param>
        public FriendRecallEvent(long authodId, long messageId, long time, long @operator)
        {
            this.type = EventType.FriendRecallEvent;
            this.authodId = authodId;
            this.messageId = messageId;
            this.time = time;
            this.@operator = @operator;
        }
    }
    /// <summary>
    /// 群消息撤回
    /// </summary>
    public sealed class GroupRecallEvent : MiraiEvent
    {
        /// <summary>
        /// 原消息发送者的QQ号
        /// </summary>
        public long authorId;
        /// <summary>
        /// 原消息messageId
        /// </summary>
        public long messageId;
        /// <summary>
        /// 原消息发送时间
        /// </summary>
        public long time;
        /// <summary>
        /// 消息撤回所在的群
        /// </summary>
        public MiraiGroup group;
        /// <summary>
        /// 撤回消息的操作人，当null时为bot操作
        /// </summary>
        public MiraiMemberHolder @operator;
        /// <summary>
        /// 群消息撤回
        /// </summary>
        /// <param name="authorId">原消息发送者的QQ号</param>
        /// <param name="messageId">原消息messageId</param>
        /// <param name="time">原消息发送时间</param>
        /// <param name="group">消息撤回所在的群</param>
        /// <param name="operator">撤回消息的操作人，当null时为bot操作</param>
        public GroupRecallEvent(
            long authorId, long messageId, long time, 
            MiraiGroup group, MiraiMemberHolder @operator)
        {
            this.type = EventType.GroupRecallEvent;
            this.authorId = authorId;
            this.messageId = messageId;
            this.time = time;
            this.group = group;
            this.@operator = @operator;
        }
    }
    /// <summary>
    /// Bot被踢出一个群
    /// </summary>
    public sealed class BotLeaveEventKick : MiraiEvent
    {
        /// <summary>
        /// Bot被踢出的群的信息
        /// </summary>
        public MiraiGroup group;
        /// <summary>
        /// Bot被踢后获取操作人的 Member 对象
        /// </summary>
        public object op;
        /// <summary>
        /// Bot被踢出一个群
        /// </summary>
        /// <param name="group">Bot被踢出的群的信息</param>
        /// <param name="op">Bot被踢后获取操作人的 Member 对象</param>
        public BotLeaveEventKick(MiraiGroup group, object op)
        {
            this.type = EventType.BotLeaveEventKick;
            this.group = group;
            this.op = op;
        }
    }
    /// <summary>
    /// Bot主动退出一个群
    /// </summary>
    public sealed class BotLeaveEventActive : MiraiEvent
    {
        /// <summary>
        /// Bot退出的群的信息
        /// </summary>
        public MiraiGroup group;
        /// <summary>
        /// Bot主动退出一个群
        /// </summary>
        /// <param name="group">Bot退出的群的信息</param>
        public BotLeaveEventActive(MiraiGroup group)
        {
            this.type = EventType.BotLeaveEventActive;
            this.group = group;
        }
    }
    /// <summary>
    /// Bot自身入群事件
    /// </summary>
    public sealed class BotJoinGroupEvent : MiraiEvent
    {
        /// <summary>
        /// Bot新加入群的信息
        /// </summary>
        public MiraiGroup group;
        /// <summary>
        /// 如果被要求入群的话，则为邀请人的 Member 对象
        /// </summary>
        public object invitor;
        /// <summary>
        /// Bot自身入群事件
        /// </summary>
        /// <param name="group">Bot新加入群的信息</param>
        /// <param name="invitor">如果被要求入群的话，则为邀请人的 Member 对象</param>
        public BotJoinGroupEvent(MiraiGroup group, object invitor)
        {
            this.type = EventType.BotJoinGroupEvent;
            this.group = group;
            this.invitor = invitor;
        }
    }
    /// <summary>
    /// Bot被解除禁言
    /// </summary>
    public sealed class BotUnmuteEvent : MiraiEvent
    {
        /// <summary>
        /// 操作的管理员或群主信息
        /// </summary>
        public MiraiMemberHolder @operator;
        /// <summary>
        /// Bot被解除禁言
        /// </summary>
        /// <param name="operator">操作的管理员或群主信息</param>
        public BotUnmuteEvent(MiraiMemberHolder @operator)
        {
            this.type = EventType.BotUnmuteEvent;
            this.@operator = @operator;
        }
    }
    /// <summary>
    /// Bot被禁言
    /// </summary>
    public sealed class BotMuteEvent : MiraiEvent
    {
        /// <summary>
        /// 禁言时长，单位为秒
        /// </summary>
        public long durationSecond;
        /// <summary>
        /// 操作的管理员或群主信息
        /// </summary>
        public MiraiMemberHolder @operator;
        /// <summary>
        /// Bot被禁言
        /// </summary>
        /// <param name="durationSecond">禁言时长，单位为秒</param>
        /// <param name="operator">操作的管理员或群主信息</param>
        public BotMuteEvent(long durationSecond, MiraiMemberHolder @operator)
        {
            this.durationSecond = durationSecond;
            this.@operator = @operator;
        }
    }
    /// <summary>
    /// Bot在群里的权限改变
    /// </summary>
    public sealed class BotGroupPermissionChangeEvent : MiraiEvent
    {
        /// <summary>
        /// Bot的原权限，OWNER、ADMINISTRATOR或MEMBER
        /// </summary>
        public string origin;
        /// <summary>
        /// Bot的新权限，OWNER、ADMINISTRATOR或MEMBER
        /// </summary>
        public string current;
        /// <summary>
        /// 
        /// </summary>
        public MiraiGroup group;
        /// <summary>
        /// Bot在群里的权限改变
        /// </summary>
        /// <param name="origin">Bot的原权限，OWNER、ADMINISTRATOR或MEMBER</param>
        /// <param name="current">Bot的新权限，OWNER、ADMINISTRATOR或MEMBER</param>
        /// <param name="group">权限改变所在的群信息</param>
        public BotGroupPermissionChangeEvent(string origin, string current, MiraiGroup group)
        {
            this.type = EventType.BotGroupPermissionChangeEvent;
            this.origin = origin;
            this.current = current;
            this.group = group;
        }
    }
    /// <summary>
    /// 好友昵称改变
    /// </summary>
    public sealed class FriendNickChangedEvent : MiraiEvent
    {
        /// <summary>
        /// 好友详细信息
        /// </summary>
        public MiraiFriend friend;
        /// <summary>
        /// 从昵称...
        /// </summary>
        public string from;
        /// <summary>
        /// 改变为...
        /// </summary>
        public string to;
        /// <summary>
        /// 生成一个好友昵称改变事件
        /// </summary>
        /// <param name="friend"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public FriendNickChangedEvent(MiraiFriend friend, string from, string to)
        {
            this.type = EventType.FriendNickChangedEvent;
            this.friend = friend;
            this.from = from;
            this.to = to;
        }
    }
    /// <summary>
    /// 好友输入状态改变
    /// </summary>
    public sealed class FriendInputStatusChangedEvent : MiraiEvent
    {
        /// <summary>
        /// 好友的详细信息
        /// </summary>
        public MiraiFriend friend;
        /// <summary>
        /// 输入状态
        /// </summary>
        public bool inputting;
        /// <summary>
        /// 生成一个好友输入状态改变事件类
        /// </summary>
        /// <param name="friend"></param>
        /// <param name="inputting"></param>
        public FriendInputStatusChangedEvent(MiraiFriend friend, bool inputting)
        {
            this.type = EventType.FriendInputStatusChangedEvent;
            this.friend = friend;
            this.inputting = inputting;
        }
    }
    /// <summary>
    /// Bot主动重新登录
    /// </summary>
    public sealed class BotReloginEvent : MiraiEvent
    {
        /// <summary>
        /// 重新登录的QQ
        /// </summary>
        public long qq;
        /// <summary>
        /// 生成一个主动重新登陆事件类
        /// </summary>
        /// <param name="qq"></param>
        public BotReloginEvent(long qq)
        {
            this.type = EventType.BotReloginEvent;
            this.qq = qq;
        }
    }
    /// <summary>
    /// Bot被服务器断开(网络问题)
    /// </summary>
    public sealed class BotOfflineEventDropped : MiraiEvent
    {
        /// <summary>
        /// 被断线的QQ
        /// </summary>
        public long qq;
        /// <summary>
        /// 生成一个被网络断线事件类
        /// </summary>
        /// <param name="qq"></param>
        public BotOfflineEventDropped(long qq)
        {
            this.type = EventType.BotOfflineEventDropped;
            this.qq = qq;
        }
    }
    /// <summary>
    /// Bot被挤下线
    /// </summary>
    public sealed class BotOfflineEventForce : MiraiEvent
    {
        /// <summary>
        /// 被挤下线的QQ
        /// </summary>
        public long qq;
        /// <summary>
        /// 生成一个被挤下线事件类
        /// </summary>
        /// <param name="qq"></param>
        public BotOfflineEventForce(long qq)
        {
            this.type = EventType.BotOfflineEventForce;
            this.qq = qq;
        }
    }
    /// <summary>
    /// Bot主动离线
    /// </summary>
    public sealed class BotOfflineEventActive : MiraiEvent
    {
        /// <summary>
        /// 离线的QQ
        /// </summary>
        public long qq;
        /// <summary>
        /// 生成一个主动离线事件类
        /// </summary>
        /// <param name="qq"></param>
        public BotOfflineEventActive(long qq)
        {
            this.type = EventType.BotOfflineEventActive;
            this.qq = qq;
        }
    }
    /// <summary>
    /// Bot登陆成功
    /// </summary>
    public sealed class BotOnlineEvent : MiraiEvent
    {
        /// <summary>
        /// 登录的QQ
        /// </summary>
        public long qq;
        /// <summary>
        /// 生成一个登陆成功事件类
        /// </summary>
        /// <param name="qq"></param>
        public BotOnlineEvent(long qq)
        {
            this.type = EventType.BotOnlineEvent;
            this.qq = qq;
        }
    }
    #endregion
}

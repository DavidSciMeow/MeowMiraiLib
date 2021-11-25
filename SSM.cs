using MeowMiraiLib.Msg.Type;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
/*
 * 本文件是发送信息的标准定义文件,
 * 文件内容与 https://github.com/project-mirai/mirai-api-http/blob/master/docs/api/API.md 一一对应
 * 发送定义为 SSM (Socket Sendable Message) 类,
 * --------------------------
 * this file is by define message send function/method
 * this file is one to one match to website https://github.com/project-mirai/mirai-api-http/blob/master/docs/api/API.md
 * Sender function/method defines class is SSM (Socket Sendable Message) class.
 * --- command | 还未作的功能 --- 
 * recall
 * ------------
 * file_list
 * file_info
 * file_mkdir
 * file_delete
 * file_move
 * file_rename
 * ------------
 * mute
 * unmute
 * kick
 * muteAll
 * unmuteAll
 * setEssence
 * groupConfig ~ get
 * groupConfig ~ update
 * memberInfo ~ get
 * memberInfo ~ update
 * memberAdmin
 */
namespace MeowMiraiLib.Msg
{
    

    #region Bases SSM -- 基础定义
    /// <summary>
    /// Socket Send Message (命令报文)
    /// </summary>
    public class SSM
    {
        /// <summary>
        /// 同步id
        /// </summary>
        public int? syncId;
        /// <summary>
        /// 命令字段
        /// </summary>
        public string command;
        /// <summary>
        /// 子命令字段
        /// </summary>
        public string? subCommand = null;
        /// <summary>
        /// 内容
        /// </summary>
        public dynamic content;
        /// <summary>
        /// 发送报文到Mirai后端
        /// </summary>
        /// <param name="syncid">设置同步id,不设置为随机id</param>
        /// <param name="TimeOut">超时时间,20s(秒)</param>
        /// <returns>(SyncId[同步方案],ReturnObject[返回的JObject])</returns>
        public (int?,JObject?) Send(int? syncid = null,int TimeOut = 20)
        {
            string s;
            if (syncid != null)
            {
                syncId = syncid;
            }
            else
            {
                syncId = new Random().Next(int.MaxValue);
            }
            if (content is SMessage)
            {
                s = JsonConvert.SerializeObject(this, Formatting.None,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            else
            {
                s = $"{{\"syncId\":{syncId},\"command\":\"{command}\"," +
                    $"{(subCommand != null ? $"\"subCommand\":\"{subCommand}\"," : "")}" +
                    $"\"content\":{content}}}";
            }
            return (syncid, Client.SendAndWaitResponse(s, syncId, TimeOut));
        }
    }
    /// <summary>
    /// 发送的QQ信息
    /// </summary>
    public class SMessage
    {
        /// <summary>
        /// 目标
        /// </summary>
        public long? target = null;
        /// <summary>
        /// 备用字段,目标
        /// </summary>
        public long? qq = null;
        /// <summary>
        /// 群目标
        /// </summary>
        public long? group = null;
        /// <summary>
        /// 引用字段,回复字段
        /// </summary>
        public long? quote = null;
        /// <summary>
        /// 信息链
        /// </summary>
        public Message[] messageChain;
        /// <summary>
        /// 构造一个SendableMessage
        /// </summary>
        /// <param name="qq">发送目标</param>
        /// <param name="group">群</param>
        /// <param name="messageChain">信息链</param>
        /// <param name="quote">引用字段</param>
        public SMessage(long qq, long group, Message[] messageChain, long? quote = null)
        {
            this.qq = qq;
            this.group = group;
            this.quote = quote;
            this.messageChain = messageChain;
        }
        /// <summary>
        /// 构造一个SendableMessage
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="messageChain">信息链</param>
        /// <param name="quote">引用字段</param>
        /// <param name="qq">发送目标(备用)</param>
        /// <param name="group">群</param>
        public SMessage(long target, Message[] messageChain, 
            long? quote = null, long? qq = null, long? group = null)
        {
            this.target = target;
            this.quote = quote;
            this.messageChain = messageChain;
            this.qq = qq;
            this.group = group;
        }
    }
    #endregion

    #region Message&nudge -- 信息和戳一戳
    /// <summary>
    /// 发送好友信息
    /// </summary>
    public class FriendMessage : SSM
    {
        /// <summary>
        /// 发送好友信息
        /// </summary>
        /// <param name="target">可选，发送消息目标好友的QQ号</param>
        /// <param name="messageChain">消息链，是一个消息对象构成的数组</param>
        /// <param name="quote">引用一条消息的messageId进行回复</param>
        public FriendMessage(long target, Message[] messageChain, long? quote = null)
        {
            command = "sendFriendMessage";
            content = new SMessage(target, messageChain, quote);
        }
    }
    /// <summary>
    /// 发送群信息
    /// </summary>
    public class GroupMessage : SSM
    {
        /// <summary>
        /// 发送群信息
        /// </summary>
        /// <param name="target">可选，发送消息目标群的群号</param>
        /// <param name="messageChain">	消息链，是一个消息对象构成的数组</param>
        /// <param name="quote">引用一条消息的messageId进行回复</param>
        public GroupMessage(long target, Message[] messageChain, long? quote = null)
        {
            command = "sendGroupMessage";
            content = new SMessage(target, messageChain, quote);
        }
    }
    /// <summary>
    /// 发送临时信息
    /// </summary>
    public class TempMessage : SSM
    {
        /// <summary>
        /// 发送临时消息
        /// </summary>
        /// <param name="qq">临时会话对象QQ号</param>
        /// <param name="group">临时会话群号</param>
        /// <param name="messageChain">消息链，是一个消息对象构成的数组</param>
        /// <param name="quote">引用一条消息的messageId进行回复</param>
        public TempMessage(long qq, long group, Message[] messageChain, long? quote = null)
        {
            command = "sendTempMessage";
            content = new SMessage(qq, group, messageChain, quote);
        }
    }
    /// <summary>
    /// 发送戳一戳
    /// </summary>
    public class SendNudge : SSM
    {
        /// <summary>
        /// 发送戳一戳
        /// </summary>
        /// <param name="target">戳一戳的目标, QQ号, 可以为 bot QQ号</param>
        /// <param name="subject">戳一戳接受主体(上下文), 戳一戳信息会发送至该主体, 为群号/好友QQ号</param>
        /// <param name="kind">上下文类型, 可选值 Friend, Group, Stranger</param>
        public SendNudge(long target, long subject, string kind)
        {
            command = "sendNudge";
            content = $"{{\"sessionKey\":\"{Client.session}\"," +
                $"\"target\":{target},\"subject\":{subject},\"kind\":\"{kind}\"}}";
        }
    }
    #endregion

    #region response to request -- 请求处理
    /// <summary>
    /// 处理加好友请求
    /// </summary>
    public class Resp_newFriendRequestEvent : SSM
    {
        /// <summary>
        /// 处理加好友请求
        /// </summary>
        /// <param name="eventid"></param>
        /// <param name="fromid"></param>
        /// <param name="groupid"></param>
        /// <param name="operatenum">0同意,1拒绝,2添加至黑名单</param>
        /// <param name="message"></param>
        public Resp_newFriendRequestEvent(long eventid, long fromid, long groupid, int operatenum, string message)
        {
            command = "resp_newFriendRequestEvent";
            content = $"{{\"sessionKey\":\"{Client.session}\",\"eventId\":{eventid}," +
                      $"\"fromId\":{fromid},\"groupId\":{groupid},\"operate\":{operatenum}," +
                      $"\"message\":\"{message}\"}}";
        }
    }
    /// <summary>
    /// 用户加群
    /// </summary>
    public class Resp_memberJoinRequestEvent : SSM
    {
        /// <summary>
        /// 用户加群
        /// </summary>
        /// <param name="eventid"></param>
        /// <param name="fromid"></param>
        /// <param name="groupid"></param>
        /// <param name="operatenum">0同意,1拒绝,2忽略,3拒绝并黑名单,4忽略并黑名单</param>
        /// <param name="message"></param>
        public Resp_memberJoinRequestEvent(long eventid, long fromid, long groupid, int operatenum, string message)
        {
            command = "resp_memberJoinRequestEvent";
            content = $"{{\"sessionKey\":\"{Client.session}\",\"eventId\":{eventid}," +
                      $"\"fromId\":{fromid},\"groupId\":{groupid},\"operate\":{operatenum}," +
                      $"\"message\":\"{message}\"}}";
        }
    }
    /// <summary>
    /// Bot被邀请入群
    /// </summary>
    public class Resp_botInvitedJoinGroupRequestEvent : SSM
    {
        /// <summary>
        /// Bot被邀请入群
        /// </summary>
        /// <param name="eventid"></param>
        /// <param name="fromid"></param>
        /// <param name="groupid"></param>
        /// <param name="operatenum">0同意,1拒绝</param>
        /// <param name="message"></param>
        public Resp_botInvitedJoinGroupRequestEvent(long eventid, long fromid, long groupid, int operatenum, string message)
        {
            command = "resp_botInvitedJoinGroupRequestEvent";
            content = $"{{\"sessionKey\":\"{Client.session}\",\"eventId\":{eventid}," +
                      $"\"fromId\":{fromid},\"groupId\":{groupid},\"operate\":{operatenum}," +
                      $"\"message\":\"{message}\"}}";
        }
    }
    #endregion


    /// <summary>
    /// 获取插件信息
    /// </summary>
    public sealed class About : SSM
    {
        /// <summary>
        /// 获取插件信息
        /// </summary>
        public About()
        {
            command = "about";
        }
    }
    /// <summary>
    /// 通过MessageId获取消息
    /// </summary>
    public sealed class MessageFromId : SSM
    {
        /// <summary>
        /// 通过MessageId获取消息
        /// </summary>
        /// <param name="id">信息id</param>
        public MessageFromId(long id)
        {
            command = "messageFromId";
            content = $"{{\"sessionKey\":\"{Client.session}\",\"id\":{id}}}";
        }
    }
    /// <summary>
    /// 获取好友列表
    /// </summary>
    public sealed class FriendList : SSM
    {
        /// <summary>
        /// 获取好友列表
        /// </summary>
        public FriendList()
        {
            command = "friendList";
            content = $"{{\"sessionKey\":\"{Client.session}\"}}";
        }
    }
    /// <summary>
    /// 获取群列表
    /// </summary>
    public sealed class GroupList : SSM
    {
        /// <summary>
        /// 获取群列表
        /// </summary>
        public GroupList()
        {
            command = "groupList";
            content = $"{{\"sessionKey\":\"{Client.session}\"}}";
        }
    }
    /// <summary>
    /// 获取群员列表
    /// </summary>
    public sealed class MemberList : SSM
    {
        
        /// <summary>
        /// 获取群员列表
        /// </summary>
        /// <param name="target">目标群</param>
        public MemberList(long target)
        {
            command = "memberList";
            content = $"{{\"sessionKey\":\"{Client.session}\",\"target\":{target}}}";
        }
    }
    /// <summary>
    /// 获取bot资料
    /// </summary>
    public sealed class BotProfile : SSM
    {
        /// <summary>
        /// 获取bot资料
        /// </summary>
        public BotProfile()
        {
            command = "botProfile";
            content = $"{{\"sessionKey\":\"{Client.session}\"}}";
        }
    }
    /// <summary>
    /// 获取某个好友的资料
    /// </summary>
    public sealed class FriendProfile : SSM
    {
        /// <summary>
        /// 获取某个好友的资料
        /// </summary>
        /// <param name="qq">好友QQ</param>
        public FriendProfile(long qq)
        {
            command = "friendProfile";
            content = $"{{\"sessionKey\":\"{Client.session}\",\"target\":{qq}}}";
        }
    }
    /// <summary>
    /// 获取某个群友的资料
    /// </summary>
    public sealed class MemberProfile : SSM
    {
        /// <summary>
        /// 获取群友资料
        /// </summary>
        /// <param name="qqgroup">群号</param>
        /// <param name="qq">群员QQ</param>
        public MemberProfile(long qqgroup,long qq)
        {
            command = "memberProfile";
            content = $"{{\"sessionKey\":\"{Client.session}\",\"target\":{qqgroup},\"memberId\":{qq}}}";
        }
    }
    /// <summary>
    /// 退群
    /// </summary>
    public class QuitGroup : SSM
    {
        /// <summary>
        /// 退群
        /// </summary>
        /// <param name="target">目标群</param>
        public QuitGroup(long target)
        {
            command = "quit";
            content = $"{{\"sessionKey\":\"{Client.session}\",\"target\":{target}}}";
        }
    }
    /// <summary>
    /// 删除好友
    /// </summary>
    public class DeleteFriend : SSM
    {
        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="target">目标好友</param>
        public DeleteFriend(long target)
        {
            command = "deleteFriend";
            content = $"{{\"sessionKey\":\"{Client.session}\",\"target\":{target}}}";
        }
    }
}

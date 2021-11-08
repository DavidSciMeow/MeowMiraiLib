using MeowMiraiLib.Msg.Type;
using System;
using Newtonsoft.Json;

namespace MeowMiraiLib.Msg
{
    /*---command--- 
     * = about
     * = messageFromId
     * = friendList
     * = groupList
     * = memberList
     * = botProfile
     * = friendProfile
     * = memberProfile
     * = sendFriendMessage
     * = sendGroupMessage
     * = sendTempMessage
     * = sendNudge
     * recall
     * file_list
     * file_info
     * file_mkdir
     * file_delete
     * file_move
     * file_rename
     * = deleteFriend
     * mute
     * unmute
     * kick
     * = quit
     * muteAll
     * unmuteAll
     * setEssence
     * groupConfig ~ get
     * groupConfig ~ update
     * memberInfo ~ get
     * memberInfo ~ update
     * memberAdmin
     * = resp_newFriendRequestEvent
     * = resp_memberJoinRequestEvent
     * = resp_botInvitedJoinGroupRequestEvent
     */
    /// <summary>
    /// Socket Send Message (命令报文)
    /// </summary>
    public class SSM
    {
        public int syncId = new Random().Next(int.MaxValue);
        public string command;
        public string? subCommand = null;
        public dynamic content = null;
        public int Send()
        {
            var s = JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            Client.ws.Send(s);
            return syncId;
        }
    }
    /// <summary>
    /// 发送的QQ信息
    /// </summary>
    public class SMessage
    {
        public long? target = null;
        public long? qq = null;
        public long? group = null;
        public long? quote = null;
        public Message[] messageChain;

        public SMessage(long qq, long group, Message[] messageChain, long? quote = null)
        {
            this.qq = qq;
            this.group = group;
            this.quote = quote;
            this.messageChain = messageChain;
        }

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
    /// <summary>
    /// 获取插件信息
    /// </summary>
    public sealed class About : SSM
    {
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
        public MemberProfile(long qqgroup,long qq)
        {
            command = "memberProfile";
            content = $"{{\"sessionKey\":\"{Client.session}\",\"target\":{qqgroup},\"memberId\":{qq}}}";
        }
    }

    /// <summary>
    /// 发送好友信息
    /// </summary>
    public class FriendMessage : SSM
    {
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
        public TempMessage(long qq,long group, Message[] messageChain, long? quote = null)
        {
            command = "sendTempMessage";
            content = new SMessage(qq, group, messageChain, quote);
        }
    }

    /// <summary>
    /// 戳一戳
    /// </summary>
    public class SendNudge : SSM
    {
        public SendNudge(long target, long subject, string kind)
        {
            command = "sendNudge";
            content = $"{{\"sessionKey\":\"{Client.session}\"," +
                $"\"target\":{target},\"subject\":{subject},\"kind\":\"{kind}\"";
        }
    }
    /// <summary>
    /// 退群
    /// </summary>
    public class QuitGroup : SSM
    {
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
        public DeleteFriend(long target)
        {
            command = "deleteFriend";
            content = $"{{\"sessionKey\":\"{Client.session}\",\"target\":{target}}}";
        }
    }

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
        public Resp_newFriendRequestEvent(long eventid,long fromid,long groupid,int operatenum,string message)
        {
            command = "resp_newFriendRequestEvent";
            content = $"{{\"sessionKey\":\"{Client.session}\",\"eventId\":{eventid},"+
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
            content = $"{{\"sessionKey\":\"{Client.session}\",\"eventId\":{eventid},"+
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
}

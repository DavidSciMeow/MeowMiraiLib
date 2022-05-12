using MeowMiraiLib.Msg.Type;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

/*
 * 本文件是发送信息的标准定义文件,
 * 文件内容与 https://github.com/project-mirai/mirai-api-http/blob/master/docs/api/API.md 和 https://github.com/project-mirai/mirai-api-http/blob/master/docs/adapter/WebsocketAdapter.md 一一对应
 * 发送定义为 SSM (Socket Sendable Message) 类,
 * --------------------------
 * this file is by define message send function/method
 * this file is one to one match to website https://github.com/project-mirai/mirai-api-http/blob/master/docs/api/API.md and https://github.com/project-mirai/mirai-api-http/blob/master/docs/adapter/WebsocketAdapter.md
 * Sender function/method defines class is SSM (Socket Sendable Message) class.
 */

namespace MeowMiraiLib.Msg
{
    /// <summary>
    /// Socket Send Message (命令报文)
    /// </summary>
    public class SSM
    {
        /// <summary>
        /// 同步id
        /// </summary>
        public int? syncId { get; set; }
        /// <summary>
        /// 命令字段
        /// </summary>
        public string command { get; set; }
        /// <summary>
        /// 子命令字段
        /// </summary>
        public string? subCommand { get; set; } = null;
        /// <summary>
        /// 内容
        /// </summary>
        [JsonProperty(ItemIsReference = true, IsReference = true)]
        public dynamic content { get; set; }
        /// <summary>
        /// Session标识
        /// </summary>
        public string session { get; set; }

        private string PackMsg(int? syncid = null)
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
                s = JsonConvert.SerializeObject(this, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            else
            {
                s = JsonConvert.SerializeObject(new { syncId, session, command, subCommand, content }, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            return s;
        }
        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="c">要发送到的客户端</param>
        /// <param name="syncid">同步的id(默认自动生成)</param>
        /// <param name="TimeOut">超时取消,默认20s(秒)</param>
        /// <returns></returns>
        public (bool isTimedOut, JObject? Return) Send(Client c, int? syncid = null, int TimeOut = 10)
            => SendAsync(c, syncid, TimeOut).GetAwaiter().GetResult();
        /// <summary>
        /// 异步发送信息
        /// </summary>
        /// <param name="c">要发送到的客户端</param>
        /// <param name="syncid">同步的id(默认自动生成)</param>
        /// <param name="TimeOut">超时取消,默认20s(秒)</param>
        /// <returns></returns>
        public async Task<(bool isTimedOut, JObject? Return)> SendAsync(Client c, int? syncid = null, int TimeOut = 10)
        {
            session = c.session;
            var ms = PackMsg(syncid);
            return await c.SendAndWaitResponse(ms, syncId, TimeOut);
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
            content = new { sessionKey = session, id };
            //content = $"{{\"sessionKey\":\"{session}\",\"id\":{id}}}";
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
            content = new {sessionKey = session};
            //content = $"{{\"sessionKey\":\"{session}\"}}";
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
            content = new { sessionKey = session };
            //content = $"{{\"sessionKey\":\"{session}\"}}";
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
            content = new { sessionKey = session ,target };
            //content = $"{{\"sessionKey\":\"{session}\",\"target\":{target}}}";
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
            content = new { sessionKey = session };
            //content = $"{{\"sessionKey\":\"{session}\"}}";
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
            content = new { sessionKey = session, target = qq };
            //content = $"{{\"sessionKey\":\"{session}\",\"target\":{qq}}}";
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
        public MemberProfile(long qqgroup, long qq)
        {
            command = "memberProfile";
            content = new { sessionKey = session, target = qqgroup, memberId = qq };
            //content = $"{{\"sessionKey\":\"{session}\",\"target\":{qqgroup},\"memberId\":{qq}}}";
        }
    }
    /// <summary>
    /// 发送好友信息
    /// </summary>
    public sealed class FriendMessage : SSM
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
    public sealed class GroupMessage : SSM
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
    public sealed class TempMessage : SSM
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
    public sealed class SendNudge : SSM
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
            content = new { sessionKey = session, target, subject, kind };
            //content = $"{{\"sessionKey\":\"{session}\",\"target\":{target},\"subject\":{subject},\"kind\":\"{kind}\"}}";
        }
    }
    /// <summary>
    /// 撤回消息
    /// </summary>
    public sealed class Recall : SSM
    {
        /// <summary>
        /// 撤回消息
        /// </summary>
        /// <param name="target">撤回消息的MessageId</param>
        public Recall(long target)
        {
            command = "recall";
            content = new { sessionKey = session, target};
            //content = $"{{\"sessionKey\":\"{session}\",\"target\":{target}}}";
        }
    }
    /// <summary>
    /// 查看文件列表
    /// </summary>
    public sealed class File_list : SSM
    {
        /// <summary>
        /// 查看文件列表
        /// </summary>
        /// <param name="id">文件夹id, 空串为根目录</param>
        /// <param name="path">文件夹路径, 文件夹允许重名, 不保证准确, 准确定位使用 id</param>
        /// <param name="target">群号或好友QQ号</param>
        /// <param name="group">群号</param>
        /// <param name="qq">好友QQ号</param>
        /// <param name="withDownloadInfo">是否携带下载信息，额外请求，无必要不要携带(置空null</param>
        /// <param name="offset">分页偏移</param>
        /// <param name="size">分页大小</param>
        public File_list(string id,string path,long target,long group,long qq,bool? withDownloadInfo,long offset,long size)
        {
            command = "file_list";
            content = new { sessionKey = session, id, path, target, group, qq, withDownloadInfo, offset, size };
            //content = $"{{\"sessionKey\":\"{session}\",\"id\":\"{id}\",\"path\":\"{path}\",\"target\":{target}" +
            //    $",\"group\":{group},\"qq\":{qq},\"withDownloadInfo\":{(withDownloadInfo == null ? "" : ((withDownloadInfo ?? true) ? "true" : "false"))}" +
            //    $",\"offset\":{offset},\"size\":{size}}}";
        }
    }
    /// <summary>
    /// 获取文件信息
    /// </summary>
    public sealed class File_info : SSM
    {
        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="id">文件id,空串为根目录</param>
        /// <param name="path">文件夹路径, 文件夹允许重名, 不保证准确, 准确定位使用 id</param>
        /// <param name="target">群号或好友QQ号</param>
        /// <param name="group">群号</param>
        /// <param name="qq">好友QQ号</param>
        /// <param name="withDownloadInfo">是否携带下载信息，额外请求，无必要不要携带</param>
        public File_info(string id, string path, long target, long group, long qq, bool? withDownloadInfo)
        {
            command = "file_info";
            content = new { sessionKey = session, id, path, target, group, qq, withDownloadInfo };
            //content = $"{{\"sessionKey\":\"{session}\",\"id\":\"{id}\",\"path\":\"{path}\",\"target\":{target}" +
            //   $",\"group\":{group},\"qq\":{qq},\"withDownloadInfo\":{(withDownloadInfo == null ? "" : ((withDownloadInfo ?? true) ? "true" : "false"))} }}";
        }
    }
    /// <summary>
    /// 创建文件夹
    /// </summary>
    public sealed class File_mkdir : SSM
    {
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="id">父目录id,空串为根目录</param>
        /// <param name="path">文件夹路径, 文件夹允许重名, 不保证准确, 准确定位使用 id</param>
        /// <param name="target">群号或好友QQ号</param>
        /// <param name="group">群号</param>
        /// <param name="qq">好友QQ号</param>
        /// <param name="directoryName">新建文件夹名</param>
        public File_mkdir(string id, string path, long target, long group, long qq, string directoryName)
        {
            command = "file_mkdir";
            content = new { sessionKey = session, id, path, target, group, qq, directoryName };
            //content = $"{{\"sessionKey\":\"{session}\",\"id\":\"{id}\",\"path\":\"{path}\",\"target\":{target}" +
            //    $",\"group\":{group},\"qq\":{qq},\"directoryName\":\"{directoryName}\" }}";
        }
    }
    /// <summary>
    /// 删除文件
    /// </summary>
    public sealed class File_delete : SSM
    {
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="id">删除文件id</param>
        /// <param name="path">文件夹路径, 文件夹允许重名, 不保证准确, 准确定位使用 id</param>
        /// <param name="target">群号或好友QQ号</param>
        /// <param name="group">群号</param>
        /// <param name="qq">好友QQ号</param>
        public File_delete(string id, string path, long target, long group, long qq)
        {
            command = "file_delete";
            content = new { sessionKey = session, id, path, target, group, qq };
            //content = $"{{\"sessionKey\":\"{session}\",\"id\":\"{id}\",\"path\":\"{path}\",\"target\":{target},\"group\":{group},\"qq\":{qq}}}";
        }
    }
    /// <summary>
    /// 移动文件
    /// </summary>
    public sealed class File_move : SSM
    {
        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="id">移动文件id</param>
        /// <param name="path">文件夹路径, 文件夹允许重名, 不保证准确, 准确定位使用 id</param>
        /// <param name="target">群号或好友QQ号</param>
        /// <param name="group">群号</param>
        /// <param name="qq">好友QQ号</param>
        /// <param name="moveTo">移动目标文件夹id</param>
        /// <param name="moveToPath">移动目标文件路径, 文件夹允许重名, 不保证准确, 准确定位使用 moveTo</param>
        public File_move(string id, string path, long target, long group, long qq, string moveTo, string moveToPath)
        {
            command = "file_move";
            content = new { sessionKey = session, id, path, target, group, qq, moveTo, moveToPath };
            //content = $"{{\"sessionKey\":\"{session}\",\"id\":\"{id}\",\"path\":\"{path}\",\"target\":{target},\"group\":{group},\"qq\":{qq},\"moveTo\":\"{moveTo}\",\"moveToPath\":\"{moveToPath}\"}}";
        }
    }
    /// <summary>
    /// 重命名文件
    /// </summary>
    public sealed class File_rename : SSM
    {
        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="id">重命名文件id</param>
        /// <param name="path">文件夹路径, 文件夹允许重名, 不保证准确, 准确定位使用 id</param>
        /// <param name="target">群号或好友QQ号</param>
        /// <param name="group">群号</param>
        /// <param name="qq">好友QQ号</param>
        /// <param name="renameTo">新文件名</param>
        public File_rename(string id, string path, long target, long group, long qq, string renameTo)
        {
            command = "file_rename";
            content = new { sessionKey = session, id, path, target, group, qq, renameTo };
            //content = $"{{\"sessionKey\":\"{session}\",\"id\":\"{id}\",\"path\":\"{path}\",\"target\":{target},\"group\":{group},\"qq\":{qq},\"renameTo\":\"{renameTo}\"}}";
        }
    }
    /// <summary>
    /// 删除好友
    /// </summary>
    public sealed class DeleteFriend : SSM
    {
        /// <summary>
        /// 删除好友
        /// </summary>
        /// <param name="target">目标好友</param>
        public DeleteFriend(long target)
        {
            command = "deleteFriend";
            content = new { sessionKey = session, target};
            //content = $"{{\"sessionKey\":\"{session}\",\"target\":{target}}}";
        }
    }
    /// <summary>
    /// 禁言群成员
    /// </summary>
    public sealed class Mute : SSM
    {
        /// <summary>
        /// 禁言群成员
        /// </summary>
        /// <param name="target">指定群的群号</param>
        /// <param name="memberId">指定群员QQ号</param>
        /// <param name="time">禁言时长，单位为秒，最多30天(2592000)，默认为0</param>
        public Mute(long target, long memberId, long time)
        {
            command = "mute";
            content = new { sessionKey = session, target, memberId, time };
            //content = $"{{\"sessionKey\":\"{session}\",\"target\":{target},\"memberId\":{memberId},\"time\":{time}}}";
        }
    }
    /// <summary>
    /// 解除群成员禁言
    /// </summary>
    public sealed class Unmute : SSM
    {
        /// <summary>
        /// 解除群成员禁言
        /// </summary>
        /// <param name="target">指定群的群号</param>
        /// <param name="memberId">指定群员QQ号</param>
        public Unmute(long target, long memberId)
        {
            command = "unmute";
            content = new { sessionKey = session, target, memberId };
            //content = $"{{\"sessionKey\":\"{session}\",\"target\":{target},\"memberId\":{memberId}}}";
        }
    }
    /// <summary>
    /// 移除群成员
    /// </summary>
    public sealed class Kick : SSM
    {
        /// <summary>
        /// 移除群成员
        /// </summary>
        /// <param name="target">指定群的群号</param>
        /// <param name="memberId">指定群员QQ号</param>
        /// <param name="msg">信息</param>
        public Kick(long target, long memberId, long msg)
        {
            command = "kick";
            content = new { sessionKey = session, target, memberId, msg };
            //content = $"{{\"sessionKey\":\"{session}\",\"target\":{target},\"memberId\":{memberId},\"msg\":{msg}}}";
        }
    }
    /// <summary>
    /// 退群
    /// </summary>
    public sealed class Quit : SSM
    {
        /// <summary>
        /// 退群
        /// </summary>
        /// <param name="target">目标群</param>
        public Quit(long target)
        {
            command = "quit";
            content = new { sessionKey = session, target };
            //content = $"{{\"sessionKey\":\"{session}\",\"target\":{target}}}";
        }
    }
    /// <summary>
    /// 全体禁言
    /// </summary>
    public sealed class MuteAll : SSM
    {
        /// <summary>
        /// 全体禁言
        /// </summary>
        /// <param name="target">指定群的群号</param>
        public MuteAll(long target)
        {
            command = "muteAll";
            content = new { sessionKey = session, target };
            //content = $"{{\"sessionKey\":\"{session}\",\"target\":{target}}}";
        }
    }
    /// <summary>
    /// 取消全体禁言
    /// </summary>
    public sealed class UnmuteAll : SSM
    {
        /// <summary>
        /// 取消全体禁言
        /// </summary>
        /// <param name="target">指定群的群号</param>
        public UnmuteAll(long target)
        {
            command = "unmuteAll";
            content = new { sessionKey = session, target };
            //content = $"{{\"sessionKey\":\"{session}\",\"target\":{target}}}";
        }
    }
    /// <summary>
    /// 设置群精华消息
    /// </summary>
    public sealed class SetEssence : SSM
    {
        /// <summary>
        /// 设置群精华消息
        /// </summary>
        /// <param name="target">精华消息的messageId</param>
        public SetEssence(long target)
        {
            command = "setEssence";
            content = new { sessionKey = session, target };
            //content = $"{{\"sessionKey\":\"{session}\",\"target\":{target}}}";
        }
    }
    /// <summary>
    /// 获取群设置
    /// </summary>
    public sealed class GroupConfig_Get : SSM
    {
        /// <summary>
        /// 获取群设置
        /// </summary>
        /// <param name="target">指定群的群号</param>
        public GroupConfig_Get(long target)
        {
            command = "groupConfig";
            subCommand = "get";
            content = new { sessionKey = session, target };
            //content = $"{{\"sessionKey\":\"{session}\",\"target\":{target}}}";
        }
    }
    /// <summary>
    /// 修改群设置
    /// </summary>
    public sealed class GroupConfig_Update : SSM
    {
        /// <summary>
        /// 修改群设置
        /// </summary>
        /// <param name="target">指定群的群号</param>
        /// <param name="set">群设置类</param>
        public GroupConfig_Update(long target, GroupConfigSet set)
        {
            command = "groupConfig";
            subCommand = "update";
            content = new { sessionKey = session, target ,config = set };
            //content = $"{{\"sessionKey\":\"{session}\",\"target\":{target},\"config\":{{{set)}}}}}";
        }
        /// <summary>
        /// 群设置类
        /// </summary>
        public sealed class GroupConfigSet
        {
            /// <summary>
            /// 群名
            /// </summary>
            public string name;
            /// <summary>
            /// 群公告
            /// </summary>
            public string announcement;
            /// <summary>
            /// 坦白说状态
            /// </summary>
            public bool confessTalk;
            /// <summary>
            /// 允许群友邀请
            /// </summary>
            public bool allowMemberInvite;
            /// <summary>
            /// 允许入群自动同意
            /// </summary>
            public bool autoApprove;
            /// <summary>
            /// 允许匿名
            /// </summary>
            public bool anonymousChat;
        }
    }
    /// <summary>
    /// 获取群员设置
    /// </summary>
    public sealed class MemberInfo_Get : SSM
    {
        /// <summary>
        /// 获取群员设置
        /// </summary>
        /// <param name="target">指定群的群号</param>
        /// <param name="memberId">群员QQ号</param>
        public MemberInfo_Get(long target, long memberId)
        {
            command = "memberInfo";
            subCommand = "get";
            content = new { sessionKey = session, target, memberId };
            //content = $"{{\"sessionKey\":\"{session}\",\"target\":{target},\"memberId\":{memberId}}}";
        }
    }
    /// <summary>
    /// 修改群员设置
    /// </summary>
    public sealed class MemberInfo_Update : SSM
    {
        /// <summary>
        /// 修改群员设置
        /// </summary>
        /// <param name="target">指定群的群号</param>
        /// <param name="memberId">群员QQ号</param>
        /// <param name="set">群员资料</param>
        public MemberInfo_Update(long target, long memberId, MemberInfoSet set)
        {
            command = "memberInfo";
            subCommand = "update";
            content = new { sessionKey = session, target, memberId, config = set };
            //content = $"{{\"sessionKey\":\"{session}\",\"target\":{target},\"memberId\":{memberId},\"config\":{{{set)}}}}}";
        }
        /// <summary>
        /// 群员设置类
        /// </summary>
        public sealed class MemberInfoSet
        {
            /// <summary>
            /// 群昵称
            /// </summary>
            public string name;
            /// <summary>
            /// 群头衔
            /// </summary>
            public string specialTitle;
        }
    }
    /// <summary>
    /// 修改群员管理员
    /// </summary>
    public sealed class MemberAdmin : SSM
    {
        /// <summary>
        /// 修改群员管理员
        /// </summary>
        /// <param name="target">指定群的群号</param>
        /// <param name="memberId">群员QQ号</param>
        /// <param name="assign">是否设置为管理员</param>
        public MemberAdmin(long target, long memberId, bool assign)
        {
            command = "memberAdmin";
            content = new { sessionKey = session, target, memberId, assign };
            //content = $"{{\"sessionKey\":\"{session}\",\"target\":{target},\"memberId\":{memberId},\"assign\":{(assign?"true":"false")}}}";
        }
    }
    /// <summary>
    /// 处理加好友请求
    /// </summary>
    public sealed class Resp_newFriendRequestEvent : SSM
    {
        /// <summary>
        /// 处理加好友请求
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="fromId"></param>
        /// <param name="groupId"></param>
        /// <param name="operate">0同意,1拒绝,2添加至黑名单</param>
        /// <param name="message"></param>
        public Resp_newFriendRequestEvent(long eventId, long fromId, long groupId, int operate, string message)
        {
            command = "resp_newFriendRequestEvent";
            content = new { sessionKey = session, eventId, fromId, groupId, operate, message };
            //content = $"{{\"sessionKey\":\"{session}\",\"eventId\":{eventid}," +
            //          $"\"fromId\":{fromid},\"groupId\":{groupid},\"operate\":{operatenum}," +
            //          $"\"message\":\"{message}\"}}";
        }
    }
    /// <summary>
    /// 用户加群
    /// </summary>
    public sealed class Resp_memberJoinRequestEvent : SSM
    {
        /// <summary>
        /// 用户加群
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="fromId"></param>
        /// <param name="groupId"></param>
        /// <param name="operate">0同意,1拒绝,2忽略,3拒绝并黑名单,4忽略并黑名单</param>
        /// <param name="message"></param>
        public Resp_memberJoinRequestEvent(long eventId, long fromId, long groupId, int operate, string message)
        {
            command = "resp_memberJoinRequestEvent";
            content = new { sessionKey = session, eventId, fromId, groupId, operate, message };
            //content = $"{{\"sessionKey\":\"{session}\",\"eventId\":{eventid}," +
            //          $"\"fromId\":{fromid},\"groupId\":{groupid},\"operate\":{operatenum}," +
            //          $"\"message\":\"{message}\"}}";
        }
    }
    /// <summary>
    /// Bot被邀请入群
    /// </summary>
    public sealed class Resp_botInvitedJoinGroupRequestEvent : SSM
    {
        /// <summary>
        /// Bot被邀请入群
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="fromId"></param>
        /// <param name="groupId"></param>
        /// <param name="operate">0同意,1拒绝</param>
        /// <param name="message"></param>
        public Resp_botInvitedJoinGroupRequestEvent(long eventId, long fromId, long groupId, int operate, string message)
        {
            command = "resp_botInvitedJoinGroupRequestEvent";
            content = new { sessionKey = session, eventId, fromId, groupId, operate, message };
            //content = $"{{\"sessionKey\":\"{session}\",\"eventId\":{eventid}," +
            //          $"\"fromId\":{fromid},\"groupId\":{groupid},\"operate\":{operatenum}," +
            //          $"\"message\":\"{message}\"}}";
        }
    }
}

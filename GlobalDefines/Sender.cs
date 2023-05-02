/* 本文上下文定义了消息事件的引起者,
 * 根据消息方案查询引起者, 本类类内继承顺序不允许调换.
 * ------------------------------
 * this file defines the 'message sender'
 * by use of identify the sender,
 * and do not alter the order of the class
 */

using System;

namespace MeowMiraiLib.Msg.Sender
{
    /// <summary>
    /// 消息引起者定义
    /// </summary>
    public class Sender : IEquatable<Sender>
    {
        /// <summary>
        /// 消息位定义
        /// </summary>
        public long id { get; set; }

        /// <inheritdoc/>
        public bool Equals(Sender? other) => id == other.id;
        /// <inheritdoc/>
        public override bool Equals(object obj) => Equals(obj as Sender);
        /// <inheritdoc/>
        public override int GetHashCode() => id.GetHashCode();
    }

    /// <summary>
    /// 群信息句柄
    /// </summary>
    public class GroupMessageSender : Sender, IEquatable<GroupMessageSender>
    {
        /// <summary>
        /// 成员名称
        /// </summary>
        public string memberName { get; set; }
        /// <summary>
        /// 成员头衔
        /// </summary>
        public string specialTitle { get; set; }
        /// <summary>
        /// 成员群权限
        /// </summary>
        public string permission { get; set; }
        /// <summary>
        /// 入群时间
        /// </summary>
        public long joinTimestamp { get; set; }
        /// <summary>
        /// 上次发言时间
        /// </summary>
        public long lastSpeakTimestamp { get; set; }
        /// <summary>
        /// 剩余禁言时间
        /// </summary>
        public long muteTimeRemaining { get; set; }
        /// <summary>
        /// 群信息
        /// </summary>
        public Group group { get; set; }
        /// <summary>
        /// 群信息句柄群信息类
        /// </summary>
        public class Group
        {
            /// <summary>
            /// 群号
            /// </summary>
            public long id { get; set; }
            /// <summary>
            /// 群名
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 在群里的权限
            /// </summary>
            public string permission { get; set; }
        }

        /// <inheritdoc/>
        public bool Equals(GroupMessageSender? other) => (id == other.id) && (group.id == other.group.id);
        /// <inheritdoc/>
        public override bool Equals(object obj) => Equals(obj as GroupMessageSender);
        /// <inheritdoc/>
        public override int GetHashCode() => id.GetHashCode() ^ group.id.GetHashCode();

    }
    /// <summary>
    /// 临时信息句柄
    /// </summary>
    public class TempMessageSender : GroupMessageSender { }
    /// <summary>
    /// 同步群信息
    /// </summary>
    public class GroupSyncMessageSender : GroupMessageSender { }
    /// <summary>
    /// 同步群临时信息
    /// </summary>
    public class TempSyncMessageSender : TempMessageSender { }

    /// <summary>
    /// 好友信息好友句柄
    /// </summary>
    public class FriendMessageSender : Sender
    {
        /// <summary>
        /// 好友昵称
        /// </summary>
        public string nickname { get; set; }
        /// <summary>
        /// 好友备注
        /// </summary>
        public string remark { get; set; }
    }
    /// <summary>
    /// 陌生人信息句柄
    /// </summary>
    public class StrangerMessageSender : FriendMessageSender { }
    /// <summary>
    /// 同步好友信息
    /// </summary>
    public class FriendSyncMessageSender : FriendMessageSender { }
    /// <summary>
    /// 同步陌生人消息
    /// </summary>
    public class StrangerSyncMessageSender : StrangerMessageSender { }

    /// <summary>
    /// 其他客户端信息句柄
    /// </summary>
    public class OtherClientMessageSender : Sender
    {
        /// <summary>
        /// 平台句柄
        /// </summary>
        public string platform { get; set; }
    }
}

using MeowMiraiLib.Msg.Sender;
using MeowMiraiLib.Msg.Type;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowMiraiLib.MultiContext
{
    /// <summary>
    /// 上下文相关的发送定义
    /// </summary>
    public struct ContextualSender : IEqualityComparer<ContextualSender>
    {
        /// <summary>
        /// 发送者QQ
        /// </summary>
        public long SenderId;
        /// <summary>
        /// 发送者的群QQ
        /// </summary>
        public long GroupId;

        /// <inheritdoc/>
        public bool Equals(ContextualSender x, ContextualSender y)
        {
            return x.SenderId == y.SenderId && x.GroupId == y.GroupId;
        }
        /// <inheritdoc/>
        public int GetHashCode([DisallowNull] ContextualSender obj)
        {
            return SenderId.GetHashCode() ^ GroupId.GetHashCode();
        }
    }
    /// <summary>
    /// 上下文相关的消息定义
    /// </summary>
    public struct ContextualMessage
    {
        /// <summary>
        /// 发送者发送信息时的状态
        /// </summary>
        public Sender Sender;
        /// <summary>
        /// 信息逻辑
        /// </summary>
        public Message[] Message;
        /// <summary>
        /// 初始化一个 上下文相关的消息定义
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="message">消息体</param>
        public ContextualMessage(Sender sender, Message[] message)
        {
            Sender = sender;
            Message = message;
        }
    }
}

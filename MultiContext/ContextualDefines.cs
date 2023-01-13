using MeowMiraiLib.Msg;
using MeowMiraiLib.Msg.Sender;
using MeowMiraiLib.Msg.Type;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

/* 
 * 版本 8.1.0+ 上下文接收端的相关定义文件, 
 * 包括接受信息,发送信息等异步触发器(事件)
 * -------------------------------------------
 * version 8.1.0+ Contextual Client Member Define Class(Struct)
 * included how Client GetMessage and SendMessage, including Trigger(event).
 */

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
        /// 发送者的群号
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
        /// <inheritdoc/>
        public override string ToString() => $"{SenderId} :: {(GroupId != -1 ? GroupId : "Null")}";

        /// <summary>
        /// 往原处发送信息
        /// </summary>
        /// <param name="s">信息</param>
        /// <param name="c">端</param>
        /// <returns></returns>
        public MessageId SendMsgBack(Message[] s, ConClient? c = null) => SendMsgBackAsync(s, c).GetAwaiter().GetResult();
        /// <summary>
        /// 往原处发送信息
        /// </summary>
        /// <param name="s">信息</param>
        /// <param name="c">端</param>
        /// <returns></returns>
        public async Task<MessageId> SendMsgBackAsync(Message[] s, ConClient? c = null)
        {
            if (SenderId != -1 && GroupId == -1)
            {
                return await SendMessageToFriendAsync(s, c);
            }
            else if (SenderId != -1 && GroupId != -1)
            {
                return await SendMessageToGroupAsync(s, c);
            }
            else
            {
                Global.Log.Error(ErrorDefine.E1000);
                throw new Exception(ErrorDefine.E1000);
            }
        }
        /// <summary>
        /// 强行往发送者的私聊发送信息[异步]
        /// </summary>
        /// <param name="s">信息</param>
        /// <param name="c">端</param>
        /// <returns></returns>
        public async Task<MessageId> SendMessageToFriendAsync(Message[] s, ConClient? c = null) => await new FriendMessage(SenderId, s).SendAsync(c);
        /// <summary>
        /// 强行往发送者的私聊发送信息
        /// </summary>
        /// <param name="s">信息</param>
        /// <param name="c">端</param>
        /// <returns></returns>
        public MessageId SendMessageToFriend(Message[] s, ConClient? c = null) => SendMessageToFriendAsync(s,c).GetAwaiter().GetResult();
        /// <summary>
        /// 强行往发送者的群发送信息(如果有)[异步]
        /// </summary>
        /// <param name="s">信息</param>
        /// <param name="c">端</param>
        /// <returns></returns>
        public async Task<MessageId> SendMessageToGroupAsync(Message[] s, ConClient? c = null) => await new GroupMessage(GroupId, s).SendAsync(c);
        /// <summary>
        /// 强行往发送者的群发送信息(如果有)
        /// </summary>
        /// <param name="s">信息</param>
        /// <param name="c">端</param>
        /// <returns></returns>
        public MessageId SendMessageToGroup(Message[] s, ConClient? c = null) => SendMessageToGroupAsync(s, c).GetAwaiter().GetResult();
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


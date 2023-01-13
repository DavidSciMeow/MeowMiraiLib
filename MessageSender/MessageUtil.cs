using MeowMiraiLib.GenericModel;
using MeowMiraiLib.Msg;
using MeowMiraiLib.Msg.Type;
using MeowMiraiLib.MultiContext;
using System.Collections.Generic;
using System.Text;

/*
 * 本文件是快速信息编写类的基础文件,
 * 本类的编写为纯帮助类, 目的是加速软件开发速度和正确使用功能,
 * 大部分功能已经在此实现, 您可以提交PR进行增设功能
 * --------------------------
 * This File is for quick implement function (helper-class) File,
 * It's mainly for more quick program-writing and correctly use internal function .
 * The most majority function is already complete in doc.
 * You can also trigger a Pull-Request to insert function.
 */

namespace MeowMiraiLib
{
    /// <summary>
    /// 信息类的扩展方法
    /// </summary>
    public static class MessageUtil
    {
        /*caller Util*/
        /// <summary>
        /// 获取信息的字符串表示
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string MGetPlainString(this Message[] array)
        {
            StringBuilder sb = new();
            foreach (var i in array)
            {
                if (i is Plain)
                {
                    sb.Append((i as Plain).text);
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// 获取命令形式的扩展
        /// </summary>
        /// <param name="array"></param>
        /// <param name="splitor">分隔符</param>
        /// <returns></returns>
        public static string[] MGetPlainStringSplit(this Message[] array, string splitor = " ")
            => MGetPlainString(array).Trim().Split(splitor);
        /// <summary>
        /// 获得信息内可能的图片地址
        /// <para>通过测试数组长度来确定是否含有图片</para>
        /// </summary>
        /// <param name="array"></param>
        /// <returns>返回一个(bool,string)的结构来判断(是否闪图,图片地址)</returns>
        public static (bool _isFlashMessage, string url)[] MGetEachImageUrl(this Message[] array)
        {
            List<(bool, string)> l = new();
            foreach (var i in array)
            {
                if (i is Image)
                {
                    var url = (i as Image).url;
                    l.Add((false, url));
                }
                else if (i is FlashImage)
                {
                    var url = (i as FlashImage).url;
                    l.Add((true, url));
                }
            }
            return l.ToArray();
        }

        /*Sender Util*/
        /// <summary>
        /// 将信息发送给好友
        /// </summary>
        /// <param name="array">信息序列</param>
        /// <param name="target">目标</param>
        /// <param name="c">要发送的端</param>
        /// <param name="quote">是否回复某条</param>
        public static MessageId SendToFriend(this Message[] array, long target, Client c, long? quote = null)
        => new FriendMessage(target, array, quote).Send(c);
        /// <summary>
        /// 将信息发送给群
        /// </summary>
        /// <param name="array">信息序列</param>
        /// <param name="target">目标</param>
        /// <param name="c">要发送的端</param>
        /// <param name="quote">是否回复某条</param>
        public static MessageId SendToGroup(this Message[] array, long target, Client c, long? quote = null)
        => new GroupMessage(target, array, quote).Send(c);
        /// <summary>
        /// 将信息发送给临时聊天
        /// </summary>
        /// <param name="array">信息序列</param>
        /// <param name="target">目标</param>
        /// <param name="group">发起临时聊天的群号</param>
        /// <param name="c">要发送的端</param>
        /// <param name="quote">是否回复某条</param>
        public static MessageId SendToTemp(this Message[] array, long target, long group, Client c, long? quote = null)
        => new TempMessage(target, group, array, quote).Send(c);

        /*GenericModel util*/
        /// <summary>
        /// 给好友发送信息
        /// </summary>
        /// <param name="friend">好友列表的好友</param>
        /// <param name="array">信息组</param>
        /// <param name="c">端</param>
        /// <param name="quote">引用</param>
        /// <returns></returns>
        public static MessageId SendMessage(this QQFriend friend, Message[] array, Client c, long? quote = null)
        => new FriendMessage(friend.id, array, quote).Send(c);
        /// <summary>
        /// 给好友发送信息(简写逻辑)
        /// </summary>
        /// <param name="a">简写逻辑写法</param>
        /// <param name="c">端</param>
        /// <param name="quote">引用</param>
        /// <returns></returns>
        public static MessageId SendMessage(this (QQFriend friend, Message[] array) a, Client c, long? quote = null)
        => new FriendMessage(a.friend.id, a.array, quote).Send(c);
        /// <summary>
        /// 给某群发送信息
        /// </summary>
        /// <param name="g">群</param>
        /// <param name="array">信息组</param>
        /// <param name="c">端</param>
        /// <param name="quote">引用</param>
        /// <returns></returns>
        public static MessageId SendMessage(this QQGroup g, Message[] array, Client c, long? quote = null)
        => new GroupMessage(g.id, array, quote).Send(c);
        /// <summary>
        /// 给某群发送信息(简写逻辑)
        /// </summary>
        /// <param name="a">简写逻辑写法</param>
        /// <param name="c">端</param>
        /// <param name="quote">引用</param>
        /// <returns></returns>
        public static MessageId SendMessage(this (QQGroup g, Message[] array) a, Client c, long? quote = null)
        => new GroupMessage(a.g.id, a.array, quote).Send(c);
        /// <summary>
        /// 给某群成员(非好友)发送信息
        /// </summary>
        /// <param name="gm">群</param>
        /// <param name="array">信息组</param>
        /// <param name="c">端</param>
        /// <param name="quote">引用</param>
        /// <returns></returns>
        public static MessageId SendMessage(this QQGroupMember gm, Message[] array, Client c, long? quote = null)
        => new TempMessage(gm.id, gm.group.id, array, quote).Send(c);
        /// <summary>
        /// 给某群成员(非好友)发送信息(简写逻辑)
        /// </summary>
        /// <param name="a">简写逻辑写法</param>
        /// <param name="c">端</param>
        /// <param name="quote">引用</param>
        /// <returns></returns>
        public static MessageId SendMessage(this (QQGroupMember gm, Message[] array) a, Client c, long? quote = null)
        => new TempMessage(a.gm.id, a.gm.group.id, a.array, quote).Send(c);

        /// <summary>
        /// 获取群列表内的某个群的群员列表
        /// </summary>
        /// <param name="gp">群</param>
        /// <param name="c">端</param>
        /// <returns></returns>
        public static QQGroupMember[]? GetMemberList(this QQGroup gp, Client c) => new MemberList(gp.id).Send(c);


        /// <summary>
        /// 获取某个用户的信息(全局设置成功且按队列顺序)
        /// </summary>
        /// <param name="s">对象</param>
        /// <returns></returns>
        public static Message[]? PeekMsgs(this ContextualSender s)
        {
            if (Global.G_Client is ConClient)
            {
                return (Global.G_Client as ConClient).PeekMsgs(s)?.Message;
            }
            else
            {
                Global.Log.Error(ErrorDefine.E2000);
                throw new(ErrorDefine.E2000);
            }
        }
        /// <summary>
        /// 查看当前轮次首位信息
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string? PeekPlainMsg(this ContextualSender s)
        {
            if (Global.G_Client is ConClient)
            {
                return (Global.G_Client as ConClient).PeekMsgs(s)?.Message?.MGetPlainString();
            }
            else
            {
                Global.Log.Error(ErrorDefine.E2000);
                throw new(ErrorDefine.E2000);
            }
        }

        /// <summary>
        /// 获取某个用户的当前信息数量(全局设置成功且按队列顺序)
        /// </summary>
        /// <param name="s">对象</param>
        /// <returns></returns>
        public static int? MsgCount(this ContextualSender s)
        {
            if (Global.G_Client is ConClient)
            {
                return (Global.G_Client as ConClient).MsgCount(s);
            }
            else
            {
                Global.Log.Error(ErrorDefine.E2000);
                throw new(ErrorDefine.E2000);
            }
        }
        /// <summary>
        /// 获取某个用户的信息(全局设置成功且按队列顺序)
        /// </summary>
        /// <param name="s">对象</param>
        /// <param name="pos">队列位置</param>
        /// <returns></returns>
        public static ContextualMessage? GetMsg(this ContextualSender s, int pos = 0)
        {
            if (Global.G_Client is ConClient)
            {
                return (Global.G_Client as ConClient).GetMsg(s, pos);
            }
            else
            {
                Global.Log.Error(ErrorDefine.E2000);
                throw new(ErrorDefine.E2000);
            }
        }
        /// <summary>
        /// 获取某个用户的多个信息(全局设置成功且按队列顺序)
        /// </summary>
        /// <param name="s">对象</param>
        /// <param name="num">取数量,不能小于1</param>
        /// <returns></returns>
        public static ContextualMessage[] GetMsgs(this ContextualSender s, int num = 1)
        {
            if (Global.G_Client is ConClient)
            {
                return (Global.G_Client as ConClient).GetMsgs(s, num);
            }
            else
            {
                Global.Log.Error(ErrorDefine.E2000);
                throw new(ErrorDefine.E2000);
            }
        }
        /// <summary>
        /// 删除某个用户的多个信息(全局设置成功且按队列顺序)
        /// </summary>
        /// <param name="s">对象</param>
        /// <param name="num">取数量,大于1删除数量,[0全部删除]</param>
        /// <returns></returns>
        public static void DelMsgs(this ContextualSender s, int num = 0)
        {
            if (Global.G_Client is ConClient)
            {
                var k = (Global.G_Client as ConClient).MsgCount(s);
                if (num != 0)
                {
                    (Global.G_Client as ConClient).DelMsgs(s, num);
                }
                else
                {
                    (Global.G_Client as ConClient).DelMsgs(s, k ?? 0);
                }
            }
            else
            {
                Global.Log.Error(ErrorDefine.E2000);
                throw new(ErrorDefine.E2000);
            }
        }

        /// <summary>
        /// 获取上下文消息的某位置的消息
        /// </summary>
        /// <param name="queue">上下文消息段</param>
        /// <param name="num">个数(从1开始)</param>
        /// <returns></returns>
        public static string GetPlainMsgAt(this ContextualMessage[] queue, int num = 1)
        {
            if ((num < 1) || (num > (queue.Length + 1)))
            {
                Global.Log.Error(ErrorDefine.E1010);
                throw new(ErrorDefine.E1010);
            }
            return queue[num - 1].Message.MGetPlainString();
        }
    }
}

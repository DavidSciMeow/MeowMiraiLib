using System.Collections.Generic;
using System.Text;

namespace MeowMiraiLib.Msg.Type
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
        public static (bool isTimedOut, Newtonsoft.Json.Linq.JObject? Return) SendToFriend(this Message[] array, long target, Client c, long? quote = null)
        => new FriendMessage(target, array, quote).Send(c);
        /// <summary>
        /// 将信息发送给群
        /// </summary>
        /// <param name="array">信息序列</param>
        /// <param name="target">目标</param>
        /// <param name="c">要发送的端</param>
        /// <param name="quote">是否回复某条</param>
        public static (bool isTimedOut, Newtonsoft.Json.Linq.JObject? Return) SendToGroup(this Message[] array, long target, Client c, long? quote = null)
        => new GroupMessage(target, array, quote).Send(c);
        /// <summary>
        /// 将信息发送给临时聊天
        /// </summary>
        /// <param name="array">信息序列</param>
        /// <param name="target">目标</param>
        /// <param name="group">发起临时聊天的群号</param>
        /// <param name="c">要发送的端</param>
        /// <param name="quote">是否回复某条</param>
        public static (bool isTimedOut, Newtonsoft.Json.Linq.JObject? Return) SendToTemp(this Message[] array, long target, long group, Client c, long? quote = null)
        => new TempMessage(target, group, array, quote).Send(c);
    }
}

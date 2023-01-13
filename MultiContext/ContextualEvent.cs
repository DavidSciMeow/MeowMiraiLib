/* 
 * 版本 8.1.0+ 上下文接收端主要事件文件, 
 * 包括接受信息,发送信息等异步触发器(事件)
 * -------------------------------------------
 * 
 */

namespace MeowMiraiLib.MultiContext
{
    public partial class ConClient : Client
    {
        /// <summary>
        /// 信息定义
        /// </summary>
        /// <param name="e">信息发送者</param>
        public delegate void DelegateMessageTypo(ContextualSender e);
        /// <summary>
        /// 当上下文客户端接收到信息
        /// </summary>
        public event DelegateMessageTypo _OnMessageRecieve;
    }
}

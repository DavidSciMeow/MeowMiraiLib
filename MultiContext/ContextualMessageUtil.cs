using System;
using System.Collections.Generic;

namespace MeowMiraiLib.MultiContext
{
    /// <summary>
    /// 上下文依赖帮助类
    /// </summary>
    public class CMsgHelper
    {
        private ConClient Client;
        private int MessagePointer = 1;
        private Action<ContextualSender, ContextualMessage[]>[] ActionList;

        /// <summary>
        /// 上下文帮助类
        /// </summary>
        /// <param name="client">端</param>
        /// <param name="actionList">行为列表</param>
        public CMsgHelper(ConClient client,params Action<ContextualSender, ContextualMessage[]>[] actionList)
        {
            Client = client;
            ActionList = actionList;
        }

        /// <summary>
        /// 刷新(断线/语句结束)
        /// </summary>
        public void Fresh() => MessagePointer = 1;

        /// <summary>
        /// 上下文依赖的注入组件
        /// </summary>
        /// <param name="sender">发送者</param>
        public void Invoke(ContextualSender sender)
        {
            if (Client.MsgCount(sender) == MessagePointer) //提信息
            {
                ActionList[MessagePointer - 1]?.Invoke(sender, sender.GetMsgs(MessagePointer));
            }

            if (ActionList.Length > MessagePointer)
            {
                MessagePointer++;
            }
            else
            {
                Fresh();
            }
        }
    }
}

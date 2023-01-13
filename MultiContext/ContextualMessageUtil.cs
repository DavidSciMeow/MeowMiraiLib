using System;

/*
 * 8.1.0 本类为新加的操作类, 
 * 是关于上下文依赖的注入帮助类, 使用本类可以进行上下文交互的编写
 * -------------------------------------------
 * version 8.1.0+ Files
 * it's a helper class help SE to injection the Action<T> to Contextual Files, for Easy Edit.
 */

namespace MeowMiraiLib.MultiContext
{
    /// <summary>
    /// 上下文依赖帮助类
    /// </summary>
    public class CMsgHelper
    {
        private readonly ConClient Client;
        private int MessagePointer = 1;
        private readonly Action<ContextualSender, ContextualMessage[]>[] ActionList;

        /// <summary>
        /// 上下文帮助类
        /// </summary>
        /// <param name="client">端</param>
        /// <param name="actionList">行为列表</param>
        public CMsgHelper(ConClient? client = null,params Action<ContextualSender, ContextualMessage[]>[] actionList)
        {
            if(client is null)
            {
                if(Global.G_Client is ConClient)
                {
                    Client = Global.G_Client as ConClient;
                }
                else
                {
                    Global.Log.Error(ErrorDefine.E2000);
                    throw new(ErrorDefine.E2000);
                }
            }
            else
            {
                Client = client;
            }
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

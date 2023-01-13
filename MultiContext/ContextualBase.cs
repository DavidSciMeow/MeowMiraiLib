using MeowMiraiLib.Msg.Sender;
using MeowMiraiLib.Msg.Type;
using System.Collections.Generic;

/*
 * 本类是上下文交互端的基本定义类型文件,
 * 此功能为8.1.0新增,可以实现上下文交互.
 * ------------------------------
 * this file is a ContextualClient Define Files
 * this Function is update from version 8.1.0, can do a interact Action.
 */


namespace MeowMiraiLib.MultiContext
{
    /// <summary>
    /// 上下文类型集合
    /// </summary>
    public partial class ConClient : Client
    {
        /// <summary>
        /// 上下文端内部列表
        /// </summary>
        private readonly Dictionary<ContextualSender, Queue<ContextualMessage>> Set = new();

        /// <summary>
        /// 生成一个上下文类型的端
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="reconnect">0为不进行重连,-1为一直尝试重连,n(n>0)为尝试n次</param>
        public ConClient(string url, int reconnect = -1) 
            : base(url, reconnect)
        {
            InjectWholeProcess();
        }
        /// <summary>
        /// 生成一个上下文类型的端(含有VerifyKey)
        /// </summary>
        /// <param name="ip">地址</param>
        /// <param name="port">端口</param>
        /// <param name="verifyKey">验证</param>
        /// <param name="qq">登陆的机器人qq</param>
        /// <param name="type">登陆类型,建议默认all,否则无法同时解析推送事件和消息,仅限高级用户</param>
        /// <param name="reconnect">0为不进行重连,-1为一直尝试重连,n(n>0)为尝试n次</param>
        public ConClient(string ip, int port, string verifyKey, long qq, string type = "all", int reconnect = -1) 
            : base(ip, port, verifyKey, qq, type, reconnect)
        {
            InjectWholeProcess();
        }
        /// <summary>
        /// 生成一个上下文类型的端(不含VerifyKey)
        /// </summary>
        /// <param name="ip">地址</param>
        /// <param name="port">端口</param>
        /// <param name="qq">登陆的机器人qq</param>
        /// <param name="type">登陆类型,建议默认all,否则无法同时解析推送事件和消息,仅限高级用户</param>
        /// <param name="reconnect">0为不进行重连,-1为一直尝试重连,n(n>0)为尝试n次</param>
        public ConClient(string ip, int port, long qq, string type = "all", int reconnect = -1) 
            : base(ip, port, qq, type, reconnect)
        {
            InjectWholeProcess();
        }

        /// <summary>
        /// 注入上下文逻辑端
        /// <para>此操作会删除您的现有所有关于信息的事件订阅</para>
        /// </summary>
        private void InjectWholeProcess()
        {
            base.OnFriendMessageReceive += Base_OnMessageReceive;
            base.OnGroupMessageReceive += Base_OnMessageReceive;
            base.OnStrangerMessageReceive += Base_OnMessageReceive;
            base.OnTempMessageReceive += Base_OnMessageReceive;
        }
        /// <summary>
        /// 默认的基础信息分类逻辑
        /// </summary>
        /// <param name="s">发送者</param>
        /// <param name="e">信息源</param>
        private void Base_OnMessageReceive(Sender s, Message[] e)
        {
            try
            {
                ContextualSender ss;
                if (s is GroupMessageSender or TempMessageSender)
                {
                    ss = new()
                    {
                        GroupId = (s as GroupMessageSender).group.id,
                        SenderId = s.id,
                    };        
                }
                else if (s is FriendMessageSender or StrangerMessageSender)
                {
                    ss = new()
                    {
                        GroupId = -1,
                        SenderId = s.id,
                    };
                }
                else
                {
                    ss = new()
                    {
                        GroupId = -1,
                        SenderId = -1,
                    };
                }
                //sender init, require as if GroupId is Equal to SenderId

                if (Set.ContainsKey(ss))
                {
                    Set.TryGetValue(ss, out var sm);
                    sm.Enqueue(new(s, e));
                    _OnMessageRecieve?.Invoke(ss);
                }
                else
                {
                    var smx = new Queue<ContextualMessage>();
                    smx.Enqueue(new(s, e));
                    Set.Add(ss, smx);
                    _OnMessageRecieve?.Invoke(ss);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 当前信息队列长度
        /// </summary>
        /// <param name="s">发送者</param>
        /// <returns></returns>
        public int? MsgCount(ContextualSender s)
        {
            if(Set.TryGetValue(s, out var q))
            {
                return q.Count;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取某个用户的信息(按队列顺序)
        /// </summary>
        /// <param name="s">对象</param>
        /// <returns></returns>
        public ContextualMessage? PeekMsgs(ContextualSender s)
        {
            if (Set.TryGetValue(s, out var queue))
            {
                if(queue.TryPeek(out var res))
                {
                    return res;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取某个用户的信息(按队列顺序)
        /// </summary>
        /// <param name="s">对象</param>
        /// <param name="pos">队列位置</param>
        /// <returns></returns>
        public ContextualMessage? GetMsg(ContextualSender s, int pos = 0)
        {
            if (Set.TryGetValue(s, out var queue))
            {
                return queue.ToArray()[pos];
            }
            return null;
        }
        /// <summary>
        /// 获取某个用户的多个信息(按队列顺序)
        /// </summary>
        /// <param name="s">对象</param>
        /// <param name="num">取数量,不能小于1</param>
        /// <returns></returns>
        public ContextualMessage[] GetMsgs(ContextualSender s, int num = 1)
        {
            List<ContextualMessage> l = new();
            if (Set.TryGetValue(s, out var queue) && num > 0)
            {
                for(int i = 0; i < num; i++)
                {
                    l.Add(queue.ToArray()[i]);
                }
            }
            return l.ToArray();
        }
        /// <summary>
        /// 删除某个用户的多个信息(按队列顺序)
        /// </summary>
        /// <param name="s">对象</param>
        /// <param name="num">取数量,不能小于1</param>
        /// <returns></returns>
        public void DelMsgs(ContextualSender s, int num = 0)
        {
            if (Set.TryGetValue(s, out var queue))
            {
                for (int i = 0; i < num; i++)
                {
                    _ = queue.Dequeue(); //删除元素
                }
            }
        }
    }
}

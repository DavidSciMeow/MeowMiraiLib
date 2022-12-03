using MeowMiraiLib.Msg.Sender;
using MeowMiraiLib.Msg.Type;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MeowMiraiLib.MultiContext
{
    public static class ContextualDataSet
    {
        private static Dictionary<ContextualSender, ObservableCollection<ContextualMessage>> Set;
        private static Task InterpreterMainProcess = new Task(() =>
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CDS:MeowMiraiLib] {DateTime.Now:yyyy MM/dd HH:mm:ss} {ex.Message} --\n{ex}");
            }
        });

        ///内注入函数,解析逻辑
        public static Task<bool> DoInterpreter(this ObservableCollection<ContextualMessage> Queue, int WaitTime, params Func<ContextualMessage,dynamic>[] F)
        {
            return Task.Factory.StartNew(() =>
            {
                int funcnum = 0;
                while (true)
                {
                    if(funcnum < F.Length) // 执行的函数列有剩余
                    {
                        if (F.Length != 0) // 如果含有消息出队执行
                        {
                            F[funcnum]?.Invoke(Queue[Queue.Count - 1]);
                        }

                        if (!initstate) //初始条件 *(逻辑留空)
                        {
                            var k = Task.Factory.StartNew(() => //等待条件: 当且仅当当前队列内存在对象可提出, 若时间过长则触发结束.
                            {
                                while (true) //阻塞并等待
                                {
                                    if (Queue.Count != 0)// 队列内容不为零
                                    {
                                        return true; //返回任务完成
                                    }
                                }
                            }, TaskCreationOptions.LongRunning);

                            if (WaitTime > 0)
                            {
                                var bk = k.Wait(WaitTime * 1000);
                                if (bk)
                                {
                                    ; //下一步
                                }
                                else
                                {
                                    Console.WriteLine("超时");
                                    funcnum = 0; //从头读取
                                    initstate = true; //重置初始状态
                                }
                            }
                        }
                    }
                }
            });
        }
        /// <summary>
        /// 注入上下文逻辑端
        /// <para>此操作会删除您的现有所有关于信息的事件订阅</para>
        /// </summary>
        /// <param name="Base"></param>
        public static void InjectWholeProcess(this Client Base)
        {
            Base.ClearDelegateFriendMessage();
            Base.ClearDelegateGroupMessage();
            Base.ClearDelegateTempMessage();
            Base.ClearDelegateStrangerMessage();
            Base.OnFriendMessageReceive += (s, e) => RecieveMessageFrom(s, e);
            Base.OnGroupMessageReceive += (s, e) => RecieveMessageFrom(s, e);
            Base.OnStrangerMessageReceive += (s, e) => RecieveMessageFrom(s, e);
            Base.OnTempMessageReceive += (s, e) => RecieveMessageFrom(s, e);
        }
        private static bool RecieveMessageFrom(Sender s, Message[] e)
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
                else if(s is FriendMessageSender or StrangerMessageSender)
                {
                    ss = new()
                    {
                        GroupId = 0,
                        SenderId = s.id,
                    };
                }
                else
                {
                    return false;
                }

                if (Set.ContainsKey(ss))
                {
                    Set.TryGetValue(ss, out var sm);
                    sm.Add(new(s, e));
                }
                else
                {
                    Set.Add(ss, new ObservableCollection<ContextualMessage>
                    {
                        new(s, e)
                    });
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}

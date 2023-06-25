using MeowMiraiLib.Msg.Sender;
using MeowMiraiLib.Msg.Type;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace MeowMiraiLib.Extended
{
    /// <summary>
    /// 将信息数组当作纯文本
    /// </summary>
    public enum TreatAsText
    {
        /// <summary>
        /// 字面量
        /// </summary>
        Yes
    }
    /// <summary>
    /// 将信息数组当作文本并进行分割
    /// </summary>
    public enum TreatAsTextArray
    {
        /// <summary>
        /// 字面量
        /// </summary>
        Yes
    }
    /// <summary>
    /// 将信息数组作为信息数组
    /// </summary>
    public enum TreatAsMessage
    {
        /// <summary>
        /// 字面量
        /// </summary>
        Yes
    }

    /// <summary>
    /// 信息队列模式
    /// </summary>
    public class MUQueue
    {
        private List<Message[]> Q { get; set; } = new();
        /// <summary>
        /// 队列内长
        /// </summary>
        public int Length { get => Q.Count; }

        /// <summary>
        /// 入队信息队列
        /// </summary>
        /// <param name="msgs"></param>
        public void Enqueue(Message[] msgs)
        {
            lock(Q)
            {
                Q.Add(msgs);
            }
        }
        /// <summary>
        /// 清理队列头部元素
        /// </summary>
        public void Outqueue()
        {
            lock (Q)
            {
                Q.RemoveAt(0);
            }
        }
        /// <summary>
        /// 返回队列头部元素
        /// </summary>
        public Message[] Dequeue()
        {
            Message[] ret = this[0];
            Outqueue();
            return ret;
        }
        /// <summary>
        /// 清理头部元素
        /// </summary>
        public void Cleanqueue()
        {
            lock (Q)
            {
                Q.Clear();
            }
        }


        /// <summary>
        /// 获得队列元素
        /// </summary>
        /// <param name="x">队列元素次序</param>
        /// <returns></returns>
        public Message[] this[int x]
        {
            get
            {
                return Q[x];
            }
        }
        /// <summary>
        /// 获得队列元素,将元素作为纯信息数组
        /// </summary>
        /// <param name="x">队列元素次序</param>
        /// <param name="para">将元素作为纯信息数组</param>
        /// <returns></returns>
        public Message[] this[int x, TreatAsMessage para = TreatAsMessage.Yes]
        {
            get
            {
                _ = para;
                return Q[x];
            }
        }
        /// <summary>
        /// 获得队列元素:将元素作为纯字符串
        /// </summary>
        /// <param name="x">队列元素次序</param>
        /// <param name="para">将元素作为纯字符串</param>
        /// <returns></returns>
        public string this[int x, TreatAsText para = TreatAsText.Yes]
        {
            get
            {
                _ = para;
                return Q[x].MGetPlainString();
            }
        }
        /// <summary>
        /// 获得队列元素:将元素作为纯字符串
        /// </summary>
        /// <param name="x">队列元素次序</param>
        /// <param name="para">将元素使用默认(空格)分割字符</param>
        /// <returns></returns>
        public string[]? this[int x, TreatAsTextArray para = TreatAsTextArray.Yes]
        {
            get
            {
                _ = para;
                return Q[x].MGetPlainStringSplit();
            }
        }
        /// <summary>
        /// 获得队列元素:将元素作为纯字符串
        /// </summary>
        /// <param name="x">队列元素次序</param>
        /// <param name="para">将元素使用提交的分割方案分割字符</param>
        /// <param name="splitor">使用分割的字符串</param>
        /// <returns></returns>
        public string[]? this[int x, TreatAsTextArray para = TreatAsTextArray.Yes, string splitor = " "]
        {
            get
            {
                _ = para;
                return Q[x].MGetPlainStringSplit();
            }
        }
    }

    /// <summary>
    /// Extended Context Parser
    /// </summary>
    public class ECP
    {
        /// <summary>
        /// Inner Client
        /// </summary>
        public Client PonClient { get; private set; }
        /// <summary>
        /// Client Queue Generation
        /// </summary>
        public Task? ClientRectifyAction { get; private set; }

        /// <summary>
        /// 信息交互队列
        /// </summary>
        public Queue<(Sender Sender, Message[] Msg)> MsgQ { get; private set; } = new();
        /// <summary>
        /// Message Sets
        /// </summary>
        public Dictionary<Sender, MUQueue> Sets { get; private set; } = new();

        /// <summary>
        /// 接收信息
        /// </summary>
        public delegate void MessageRecieve(Sender s, MUQueue o);
        /// <summary>
        /// 接收到信息
        /// </summary>
        public event MessageRecieve _OnMessageRecieve;

        /// <summary>
        /// 使用全局端解析
        /// </summary>
        public ECP() : this(Global.G_Client)
        {
            if(Global.G_Client is null)
            {
                Global.Log.Error(ErrorDefine.E0003);
                throw new(ErrorDefine.E0003);
            }
            else
            {
                Global.Log.Info(ErrorDefine.E99999);
            }
        }
        /// <summary>
        /// 使用自定义端解析
        /// </summary>
        /// <param name="PonClient">Client</param>
        public ECP(Client PonClient)
        {
            this.PonClient = PonClient;
            PonClient.OnFriendMessageReceive += (s, e) => MsgQ.Enqueue(new(s, e));
            PonClient.OnGroupMessageReceive += (s, e) => MsgQ.Enqueue(new(s, e));
        }

        /// <summary>
        /// 开始解析
        /// </summary>
        public void Start()
        {
            if((ClientRectifyAction is null) || (ClientRectifyAction.Status is not TaskStatus.Running))
            {
                ClientRectifyAction = Task.Run(() =>
                {
                    while (true)
                    {
                        Task.Delay(1).GetAwaiter().GetResult();//强制确保CPU时间片分配
                        if (MsgQ.TryDequeue(out var s))//新信息入队
                        {
                            if (Sets.TryGetValue(s.Sender, out var muq))//含有信息结构体, 入队
                            {
                                muq.Enqueue(s.Msg);
                                _OnMessageRecieve.Invoke(s.Sender, muq);
                            }
                            else //不含信息结构体,新建 
                            {
                                var nmuq = new MUQueue();
                                nmuq.Enqueue(s.Msg);
                                Sets.TryAdd(s.Sender, nmuq);
                                _OnMessageRecieve.Invoke(s.Sender, nmuq);
                            }
                        }
                    }
                });
            }
            else
            {
                Global.Log.Warn(ErrorDefine.E0002);
            }
        }
    }
}

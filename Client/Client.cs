using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;
using WebSocket4Net;

/* 
 * 开发标准客户端文件
 * 本文件生成了基础的客户端定义,用于构建一个快速重写的Mirai Websocket客户端,
 * 在更改本文件的源码之前,您需要详细了解相关的构造,
 * 本文件为分部类定义文件,共有三部分组成,Client,ClientEvent,ClientParser,其中:
 * Client 负责定义客户端内部接口和基础方法,
 * ClientEvent 文件负责定义用户(程序员)可重写的方法,
 * ClientParser 用于解释Mirai后端传送的事件和操作.
 * -------------------------------------------
 * Development Standard Client File
 * this file generated the client definition, it's for quickly compile Mirai-Websocket-Client
 * before change the source code of whole client, you need to have a look what inside of it.
 * this file is a partial-class-define-file, which means it'd have three part.
 * the first part is file 'Client', this file defines internal interface and basic method for enterprise dev-peoples
 * the second part is file 'ClientEvent' this file defines User-able(programmer) function/method/event
 * the third part is file 'ClientParser' this file defines the interpreter for Mirai-backend strings, and converted into Csharp Class and Event. 
 */

namespace MeowMiraiLib
{
    /// <summary>
    /// 建造一个客户端
    /// </summary>
    public partial class Client
    {
        /// <summary>
        /// 地址
        /// </summary>
        public string url { get; }
        /// <summary>
        /// 客户端Websocket
        /// </summary>
        public WebSocket ws { get; }
        /// <summary>
        /// 会话进程号
        /// </summary>
        public string session;
        /// <summary>
        /// 调试标识
        /// </summary>
        public bool debug = false;
        /// <summary>
        /// 事件调试标识
        /// </summary>
        public bool eventdebug = false;
        /// <summary>
        /// 生成一个端
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="debug">全调试输出</param>
        /// <param name="eventdebug">事件调试输出</param>
        public Client(string url,bool debug = false,bool eventdebug = false)
        {
            this.url = url;
            this.debug = debug;
            this.eventdebug = eventdebug;
            ws = new WebSocket(url);
            ws.Opened += Ws_Opened;
            ws.MessageReceived += Ws_MessageReceived;
        }
        /// <summary>
        /// 发送并且等待回值
        /// </summary>
        /// <param name="json">发送的字段</param>
        /// <param name="syncId">同步字段</param>
        /// <param name="TimeOut">超时取消,默认20s(秒)</param>
        /// <returns></returns>
        public async Task<(bool isTimedOut,JObject? Return)> SendAndWaitResponse(string json, int? syncId = null, int TimeOut = 10)
        {
            var ts = new Task<JObject?> (() =>
            {
                ws.Send(json);
                while (true)
                {
                    if (SSMRequestList.Count != 0)
                    {
                        if (SSMRequestList.First()["syncId"].ToObject<int?>() == syncId)
                        {
                            return SSMRequestList.Dequeue();
                        }
                        else
                        {
                            SSMRequestList.Enqueue(SSMRequestList.Dequeue());
                        }

                        if(SSMRequestList.Count == 0)
                        {
                            return null;
                        }
                    }
                }
            });
            ts.Start();

            if (await Task.WhenAny(ts, Task.Delay(TimeOut * 1000)) == ts)
            {
                return (false, await ts);
            }
            else
            {
                return (true, null);
            }
        }
        /// <summary>
        /// 链接
        /// </summary>
        /// <returns></returns>
        public void Connect()
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new("No Url Specific");
            }
            else
            {
                ws.Open();
            }
        }
        /// <summary>
        /// 异步链接
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ConnectAsync()
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new("No Url Specific");
            }
            else
            {
                return await ws.OpenAsync();
            }
        }
    }
}

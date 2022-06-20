using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading;
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
        public string session { get; private set; }
        /// <summary>
        /// 调试标识
        /// </summary>
        public bool debug { get; private set; } = false;
        /// <summary>
        /// 事件调试标识
        /// </summary>
        public bool eventdebug { get; private set; } = false;
        /// <summary>
        /// 重连标识
        /// </summary>
        public int reconnect { get; private set; }
        /// <summary>
        /// 生成一个端(原始方法)
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="debug">全调试输出</param>
        /// <param name="eventdebug">事件调试输出</param>
        /// <param name="reconnect">0为不进行重连,-1为一直尝试重连,n(n>0)为尝试n次</param>
        public Client(string url, bool debug = false, bool eventdebug = false, int reconnect = -1)
        {
            this.url = url;
            this.debug = debug;
            this.eventdebug = eventdebug;
            this.reconnect = reconnect;
            ws = new(url);
            ws.Opened += (s, e) =>
            {
                System.Console.WriteLine($"[MeowMiraiLib-SocketWatchdog] - Socket Opened -");
                _OnServeiceConnected?.Invoke(e.ToString());
            };
            ws.Error += (s, e) =>
            {
                _OnServeiceError.Invoke(e.Exception);
                System.Console.WriteLine($"[MeowMiraiLib-SocketWatchdog] - Socket Error , Running to Close or Reconnect -");
                ws.Close();
                
            };
            ws.Closed += (s, e) =>
            {
                _OnServiceDropped.Invoke(e.ToString());
                System.Console.WriteLine($"[MeowMiraiLib-SocketWatchdog] - Socket Closed -");
                if (reconnect != 0)
                {
                    while (reconnect == -1 || reconnect --> 0)
                    {
                        if (ws.State == WebSocketState.Closed || ws.State == WebSocketState.None)
                        {
                            ws.Open();
                            System.Console.WriteLine($"[MeowMiraiLib-SocketWatchdog] - Trying To Reconnect (in 5 second)-");
                            Task.Delay(5 * 1000).GetAwaiter().GetResult();
                        }
                        else
                        {
                            System.Console.WriteLine($"[MeowMiraiLib-SocketWatchdog] - Reconnect Complete-");
                            return;
                        }
                    }
                }
            };
            ws.MessageReceived += Ws_MessageReceived;
        }
        /// <summary>
        /// 生成一个端(含有VerifyKey)
        /// </summary>
        /// <param name="ip">地址</param>
        /// <param name="port">端口</param>
        /// <param name="verifyKey">验证</param>
        /// <param name="qq">登陆的机器人qq</param>
        /// <param name="type">登陆类型,建议默认all,否则无法同时解析推送事件和消息,仅限高级用户</param>
        /// <param name="debug">全调试输出</param>
        /// <param name="eventdebug">事件调试输出</param>
        /// <param name="reconnect">0为不进行重连,-1为一直尝试重连,n(n>0)为尝试n次</param>
        public Client(string ip, int port, string verifyKey, long qq, string type = "all", bool debug = false, bool eventdebug = false, int reconnect = -1)
            : this($"ws://{ip}:{port}/{type}?verifyKey={verifyKey}&qq={qq}", debug, eventdebug, reconnect) { }
        /// <summary>
        /// 生成一个端(不含VerifyKey)
        /// </summary>
        /// <param name="ip">地址</param>
        /// <param name="port">端口</param>
        /// <param name="qq">登陆的机器人qq</param>
        /// <param name="type">登陆类型,建议默认all,否则无法同时解析推送事件和消息,仅限高级用户</param>
        /// <param name="debug">全调试输出</param>
        /// <param name="eventdebug">事件调试输出</param>
        /// <param name="reconnect">0为不进行重连,-1为一直尝试重连,n(n>0)为尝试n次</param>
        public Client(string ip, int port, long qq, string type = "all", bool debug = false, bool eventdebug = false, int reconnect = -1)
            : this($"ws://{ip}:{port}/{type}?&qq={qq}", debug, eventdebug, reconnect) { }
        /// <summary>
        /// 发送并且等待回值
        /// </summary>
        /// <param name="json">发送的字段</param>
        /// <param name="syncId">同步字段</param>
        /// <param name="TimeOut">超时取消,默认20s(秒)</param>
        /// <returns></returns>
        public async Task<(bool isTimedOut,JObject? Return)> SendAndWaitResponse(string json, int? syncId = null, int TimeOut = 10)
        {
            using var cancelTokenSource = new CancellationTokenSource(TimeOut * 1000);
            var ts = Task.Run(async () =>
            {
                if (ws == null)
                {
                    System.Console.WriteLine($"[MeowMiraiLib-SocketWatchdog * Sending with NetFlaws] - Socket Closed -");
                    ws.Close();
                    System.Console.WriteLine($"[MeowMiraiLib-SocketWatchdog * Sending with NetFlaws] - Trying Reconnect Socket -");
                    ws.Open();
                }
                ws?.Send(json);
                while (true)
                {
                    if (cancelTokenSource.IsCancellationRequested)
                    {
                        return null;
                    }
                    await Task.Delay(1);
                    if (SSMRequestList.Count != 0)
                    {
                        if (SSMRequestList.TryDequeue(out JObject ssm) && ssm != null)
                        {
                            if (ssm["syncId"].ToObject<int?>() == syncId)
                            {
                                return ssm;
                            }
                            else
                            {
                                SSMRequestList.Enqueue(ssm);
                            }
                        }
                    }
                }
            }, cancelTokenSource.Token);

            if (ts.Wait(TimeSpan.FromMilliseconds(TimeOut * 1000)))
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
        public Task<bool> ConnectAsync()
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new("No Url Specific");
            }
            else
            {
                return ws.OpenAsync();
            }
        }
    }
}

using System;
using System.Threading.Tasks;
using WebSocket4Net;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using MeowMiraiLib.Msg.Sender;
using MeowMiraiLib.Msg.Type;

namespace MeowMiraiLib
{
    /// <summary>
    /// 建造一个客户端
    /// </summary>
    public class Client
    {
        /// <summary>
        /// 地址
        /// </summary>
        public string url;
        /// <summary>
        /// 客户端Websocket
        /// </summary>
        private WebSocket ws;
        /// <summary>
        /// 会话进程号
        /// </summary>
        protected string session;
        /// <summary>
        /// 调试标识
        /// </summary>
        public bool debug = false;
        /// <summary>
        /// 事件调试标识
        /// </summary>
        public bool eventdebug = false;
        /// <summary>
        /// 设置客户端
        /// </summary>
        /// <param name="url">链接的后端url(完整)</param>
        /// <returns></returns>
        public Client SetClient(string url)
        {
            ws = new WebSocket(url);
            ws.Opened += (s, e) =>
            {
                Console.WriteLine("Connected");
            };
            ws.MessageReceived += (s, e) =>
            {
                var jo = JObject.Parse(e.Message);
                if (debug)
                {
                    Console.WriteLine(jo);
                }
                if (jo?["data"]?["type"] != null)
                {
                    switch (jo["data"]["type"].ToString())
                    {
                        case "GroupMessage":
                            OnGroupMessageRecieve?.Invoke(jo["data"]["sender"].ToObject<GroupMessageSender>(), RectifyMessage(jo["data"]["messageChain"].ToString()));
                            return;
                        case "FriendMessage":
                            OnFriendMessageRecieve?.Invoke(jo["data"]["sender"].ToObject<FriendMessageSender>(), RectifyMessage(jo["data"]["messageChain"].ToString()));
                            return;
                        case "TempMessage":
                            OnTempMessageRecieve?.Invoke(jo["data"]["sender"].ToObject<TempMessageSender>(), RectifyMessage(jo["data"]["messageChain"].ToString()));
                            return;
                        case "StrangerMessage":
                            OnStrangerMessageRecieve?.Invoke(jo["data"]["sender"].ToObject<StrangerMessageSender>(), RectifyMessage(jo["data"]["messageChain"].ToString()));
                            return;
                        case "OtherClientMessage":
                            OnOtherMessageRecieve?.Invoke(jo["data"]["sender"].ToObject<OtherClientMessageSender>(), RectifyMessage(jo["data"]["messageChain"].ToString()));
                            return;
                        case "NudgeEvent":
                            {
                                var j = jo["data"];
                                OnNudgeMessageRecieve?.Invoke(new(
                                    j["fromId"].ToObject<long>(),
                                    j["target"].ToObject<long>(),
                                    j["subject"]["kind"].ToString(),
                                    j["subject"]["id"].ToObject<long>(),
                                    j["action"].ToString(),
                                    j["suffix"].ToString()
                                ));
                                return;
                            }
                        case "BotJoinGroupEvent":
                            {
                                var j = jo["data"];
                                OnBotJoinGroupEvent?.Invoke(new(
                                    j["group"]["id"].ToObject<long>(),
                                    j["group"]["name"].ToString(),
                                    j["group"]["permission"].ToString()
                                ));
                                return;
                            }
                        default:
                            OnServiceMessageRecieve?.Invoke(jo.ToString());
                            return;
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(jo["syncId"].ToString().Trim()))
                    {
                        session = jo["data"]["session"].ToString();
                        OnServericeConnected?.Invoke(jo.ToString());
                    }
                    else
                    {
                        OnServiceMessageRecieve?.Invoke(jo.ToString());
                    }
                }
            };
            OnServiceMessageRecieve += (a) =>
            {
                try
                {
                    var jo = JObject.Parse(a);
                    if (debug || eventdebug)
                    {
                        Console.WriteLine(jo);
                    }
                    switch (jo?["data"]?["type"]?.ToString())
                    {
                        case "NewFriendRequestEvent":
                            {
                                //_E_BotNewFriendEventRecieve?.Invoke();
                                break;
                            }
                        case "BotInvitedJoinGroupRequestEvent":
                            {
                                break;
                            }
                    }
                }
                catch
                {

                }
            };
            return this;
        }
        /// <summary>
        /// 发送字段
        /// </summary>
        /// <param name="json">Json报文</param>
        public void Send(string json) => ws.Send(json);
        /// <summary>
        /// 解析报文
        /// </summary>
        /// <param name="messagestr">报文</param>
        /// <returns></returns>
        public static Message[] RectifyMessage(string messagestr)
        {
            try
            {
                List<Message> l = new();
                foreach (var k in JArray.Parse(messagestr))
                {
                    switch (k["type"].ToString())
                    {
                        case "Source": l.Add(k.ToObject<Source>()); break;
                        case "Quote":
                            {
                                l.Add(new Quote(
                                    k["id"].ToObject<long>(),
                                    k["groupId"].ToObject<long>(),
                                    k["senderId"].ToObject<long>(),
                                    k["targetId"].ToObject<long>(),
                                    RectifyMessage(k["origin"].ToString()))
                                );
                            }
                            break;
                        case "At": l.Add(k.ToObject<At>()); break;
                        case "AtAll": l.Add(k.ToObject<AtAll>()); break;
                        case "Face": l.Add(k.ToObject<Face>()); break;
                        case "Plain": l.Add(k.ToObject<Plain>()); break;
                        case "Image": l.Add(k.ToObject<Image>()); break;
                        case "FlashImage": l.Add(k.ToObject<FlashImage>()); break;
                        case "Voice": l.Add(k.ToObject<Voice>()); break;
                        case "Xml": l.Add(k.ToObject<Xml>()); break;
                        case "Json": l.Add(k.ToObject<Json>()); break;
                        case "App": l.Add(k.ToObject<App>()); break;
                        case "Poke": l.Add(k.ToObject<Poke>()); break;
                        case "Dice": l.Add(k.ToObject<Dice>()); break;
                        case "MusicShare": l.Add(k.ToObject<MusicShare>()); break;
                        case "Forward": l.Add(k.ToObject<ForwardMessage>()); break;
                        case "File": l.Add(k.ToObject<File>()); break;
                        default: Console.WriteLine($"err parse {k["type"]}"); return null;
                    }
                }
                return l.ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.Message} :: {messagestr}");
                Console.WriteLine();
                return null;
            }
        }
        /// <summary>
        /// 链接后端
        /// </summary>
        public void Connect() => ws.Open();
        /// <summary>
        /// 异步链接
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ConnectAsync() => await ws.OpenAsync();


        public delegate void BotJoinGroupEvent(Event.BotJoinGroupEvent e);
        public event BotJoinGroupEvent OnBotJoinGroupEvent;
        public delegate void NewFriendRequestEvent(Event.NewFriendRequestEvent e);
        public event NewFriendRequestEvent OnBotNewFriendEventRecieve;


        private delegate void ServiceMessage(string e);
        private event ServiceMessage OnServiceMessageRecieve;
        public delegate void NudgeEvent(Event.NudgeEvent e);
        public event NudgeEvent OnNudgeMessageRecieve;
        public delegate void FriendMessage(FriendMessageSender s, Message[] e);
        public event FriendMessage OnFriendMessageRecieve;
        public delegate void GroupMessage(GroupMessageSender s, Message[] e);
        public event GroupMessage OnGroupMessageRecieve;
        public delegate void TempMessage(TempMessageSender s, Message[] e);
        public event TempMessage OnTempMessageRecieve;
        public delegate void StrangerMessage(StrangerMessageSender s, Message[] e);
        public event StrangerMessage OnStrangerMessageRecieve;
        public delegate void OtherClientMessage(OtherClientMessageSender s, Message[] e);
        public event OtherClientMessage OnOtherMessageRecieve;
        public delegate void ServiceConnected(string e);
        public event ServiceConnected OnServericeConnected;
    }
}

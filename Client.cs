using System;
using System.Threading.Tasks;
using WebSocket4Net;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Reflection;
using MeowMiraiLib.Msg;
using MeowMiraiLib.Msg.Sender;
using MeowMiraiLib.Msg.Type;
using MeowMiraiLib.Event;

namespace MeowMiraiLib
{
    public class Client
    {
        /// ws://localhost:8880/all?verifyKey=elecrinko&qq=2040755401
        public string url;
        public static WebSocket ws;
        public static string session;
        public bool debug = false;
        public bool eventdebug = false;
        public Client(string url)
        {
            ws = new WebSocket(url);
            ws.Opened += (s, e) =>
            {
                Console.WriteLine("Connected");
            };
            ws.MessageReceived += (s, e) =>
            {
                if (debug)
                {
                    Console.WriteLine(e.Message);
                }
                var jo = JObject.Parse(e.Message);
                if (jo?["data"]?["type"] != null)
                {
                    switch (jo["data"]["type"].ToString())
                    {
                        case "GroupMessage":
                            _GroupMessageRecieve.Invoke(jo["data"]["sender"].ToObject<GroupMessageSender>(), RectifyMessage(jo["data"]["messageChain"].ToString()));
                            return;
                        case "FriendMessage":
                            _FriendMessageRecieve.Invoke(jo["data"]["sender"].ToObject<FriendMessageSender>(), RectifyMessage(jo["data"]["messageChain"].ToString()));
                            return;
                        case "TempMessage":
                            _TempMessageRecieve.Invoke(jo["data"]["sender"].ToObject<TempMessageSender>(), RectifyMessage(jo["data"]["messageChain"].ToString()));
                            return;
                        case "StrangerMessage":
                            _StrangerMessageRecieve.Invoke(jo["data"]["sender"].ToObject<StrangerMessageSender>(), RectifyMessage(jo["data"]["messageChain"].ToString()));
                            return;
                        case "OtherClientMessage":
                            _OtherMessageRecieve.Invoke(jo["data"]["sender"].ToObject<OtherClientMessageSender>(), RectifyMessage(jo["data"]["messageChain"].ToString()));
                            return;
                        case "NudgeEvent":
                            var j = jo["data"];
                            _NudgeEventRecieve.Invoke(
                                new Event.NudgeEvent(
                                    j["fromId"].ToObject<long>(),
                                    j["target"].ToObject<long>(),
                                    j["subject"]["kind"].ToString(),
                                    j["subject"]["id"].ToObject<long>(),
                                    j["action"].ToString(),
                                    j["suffix"].ToString()));
                            return;
                        default:
                            _ServiceMessageRecieve.Invoke(jo.ToString());
                            return;
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(jo["syncId"].ToString().Trim()))
                    {
                        session = jo["data"]["session"].ToString();
                        _ServericeConnected.Invoke(jo.ToString());
                    }
                    else
                    {
                        var syncid = int.Parse(jo["syncId"].ToString());
                        _ServiceMessageRecieve.Invoke(jo.ToString());
                    }
                }
            };
            //rectify event
            _ServiceMessageRecieve += (a) =>
            {
                if (debug || eventdebug)
                {
                    Console.WriteLine(a);
                }

                try
                {
                    var jo = JObject.Parse(a);
                    if (jo?["data"]?["type"]?.ToString() == "BotInvitedJoinGroupRequestEvent")
                    {
                        _E_BotInvitedJoinGroupRecieve.Invoke(new(
                            jo?["data"]?["eventId"].ToObject<long>() ?? 0,
                            jo?["data"]?["message"].ToString(),
                            jo?["data"]?["fromId"].ToObject<long>() ?? 0,
                            jo?["data"]?["groupId"].ToObject<long>() ?? 0,
                            jo?["data"]?["groupName"].ToString(),
                            jo?["data"]?["nick"].ToString()
                            ), new(
                            jo?["data"]?["eventId"].ToObject<long>() ?? 0,
                            jo?["data"]?["fromId"].ToObject<long>() ?? 0,
                            jo?["data"]?["groupId"].ToObject<long>() ?? 0,
                            0,
                            jo?["data"]?["message"].ToString()));
                    }
                }
                catch
                {
                    
                }
                
            };
            ///PLD NUL POINTER
            _GroupMessageRecieve += (s, e) => { };
            _FriendMessageRecieve += (s, e) => { };
            _TempMessageRecieve += (s, e) => { };
            _StrangerMessageRecieve += (s, e) => { };
            _OtherMessageRecieve += (s, e) => { };
            _ServericeConnected += (a) => { };
            _E_BotInvitedJoinGroupRecieve += (a, b) => { };
            _NudgeEventRecieve += (a) => { };
        }
        public void Send(string json) => ws.Send(json);
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
        public void Connect() => ws.Open();
        public async Task<bool> ConnectAsync() => await ws.OpenAsync();

        public delegate void BotInvitedJoinGroupRequestEvent(BotInvitedJoinGroupRequestEventArg m, EventBotInvitedGroup e);
        public event BotInvitedJoinGroupRequestEvent _E_BotInvitedJoinGroupRecieve;

        private delegate void ServiceMessage(string e);
        private event ServiceMessage _ServiceMessageRecieve;
        public delegate void FriendMessage(FriendMessageSender s, Message[] e);
        public event FriendMessage _FriendMessageRecieve;
        public delegate void GroupMessage(GroupMessageSender s, Message[] e);
        public event GroupMessage _GroupMessageRecieve;
        public delegate void NudgeEvent(Event.NudgeEvent e);
        public event NudgeEvent _NudgeEventRecieve;
        public delegate void TempMessage(TempMessageSender s, Message[] e);
        public event TempMessage _TempMessageRecieve;
        public delegate void StrangerMessage(StrangerMessageSender s, Message[] e);
        public event StrangerMessage _StrangerMessageRecieve;
        public delegate void OtherClientMessage(OtherClientMessageSender s, Message[] e);
        public event OtherClientMessage _OtherMessageRecieve;
        public delegate void ServiceConnected(string e);
        public event ServiceConnected _ServericeConnected;
    }
}

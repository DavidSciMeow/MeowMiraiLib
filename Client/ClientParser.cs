using MeowMiraiLib.Msg.Sender;
using MeowMiraiLib.Msg.Type;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using WebSocket4Net;

namespace MeowMiraiLib
{
    public partial class Client
    {
        private void Ws_Opened(object? sender, EventArgs e)
        {
            Console.WriteLine("Connected");
            OnServeiceConnected?.Invoke("Connected");
        }
        private Message[] RectifyMessage(string messagestr)
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
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} :: {messagestr}");
                Console.WriteLine();
                return null;
            }
        }
        private void Ws_MessageReceived(object? s, MessageReceivedEventArgs e)
        {
            var jo = JObject.Parse(e.Message);
            if (debug)
            {
                Console.WriteLine(jo);
            }
            if (jo?["data"]?["type"] != null)
            {
                try
                {
                    switch (jo["data"]["type"].ToString())
                    {
                        case "GroupMessage":
                            {
                                OnGroupMessageRecieve?.Invoke(jo["data"]["sender"].ToObject<GroupMessageSender>(), RectifyMessage(jo["data"]["messageChain"].ToString()));
                                return;
                            }
                        case "FriendMessage":
                            {
                                OnFriendMessageRecieve?.Invoke(jo["data"]["sender"].ToObject<FriendMessageSender>(), RectifyMessage(jo["data"]["messageChain"].ToString()));
                                return;
                            }
                        case "TempMessage":
                            {
                                OnTempMessageRecieve?.Invoke(jo["data"]["sender"].ToObject<TempMessageSender>(), RectifyMessage(jo["data"]["messageChain"].ToString()));
                                return;
                            }
                        case "StrangerMessage":
                            {
                                OnStrangerMessageRecieve?.Invoke(jo["data"]["sender"].ToObject<StrangerMessageSender>(), RectifyMessage(jo["data"]["messageChain"].ToString()));
                                return;
                            }
                        case "OtherClientMessage":
                            {
                                OnOtherMessageRecieve?.Invoke(jo["data"]["sender"].ToObject<OtherClientMessageSender>(), RectifyMessage(jo["data"]["messageChain"].ToString()));
                                return;
                            }
                        case "BotOnlineEvent":
                            {
                                OnEventBotOnlineEvent?.Invoke(new(jo["data"]["qq"].ToObject<long>()));
                                return;
                            }
                        case "BotOfflineEventActive":
                            {
                                OnEventBotOfflineEventActive?.Invoke(new(jo["data"]["qq"].ToObject<long>()));
                                return;
                            }
                        case "BotOfflineEventForce":
                            {
                                OnEventBotOfflineEventForce?.Invoke(new(jo["data"]["qq"].ToObject<long>()));
                                return;
                            }
                        case "BotOfflineEventDropped":
                            {
                                OnEventBotOfflineEventDropped?.Invoke(new(jo["data"]["qq"].ToObject<long>()));
                                return;
                            }
                        case "BotReloginEvent":
                            {
                                OnEventBotReloginEvent?.Invoke(new(jo["data"]["qq"].ToObject<long>()));
                                return;
                            }
                        case "FriendInputStatusChangedEvent":
                            {
                                var j = jo["data"];
                                OnEventFriendInputStatusChangedEvent?.Invoke(
                                    new(
                                        new(
                                            j["friend"]["id"].ToObject<long>(),
                                            j["friend"]["nickname"].ToString(),
                                            j["friend"]["remark"].ToString()
                                            ),
                                        j["inputting"].ToObject<bool>()
                                        )
                                    );
                                return;
                            }
                        case "FriendNickChangedEvent":
                            {
                                var j = jo["data"];
                                OnEventFriendNickChangedEvent?.Invoke(
                                    new(
                                        new(
                                            j["friend"]["id"].ToObject<long>(),
                                            j["friend"]["nickname"].ToString(),
                                            j["friend"]["remark"].ToString()
                                            ),
                                        j["from"].ToString(),
                                        j["to"].ToString())
                                    );
                                return;
                            }
                    }
                }
                catch (Exception ex)
                {
                    if (debug)
                    {
                        Console.WriteLine($"{DateTime.Now} :: {ex.Message}");
                    }
                }
                
            }
            else
            {
                if (string.IsNullOrWhiteSpace(jo["syncId"].ToString().Trim()))
                {
                    session = jo["data"]["session"].ToString();
                    OnServeiceConnected?.Invoke(jo.ToString());
                    return;
                }
            }
                /*
                    case "BotJoinGroupEvent":
                        {
                            var j = jo["data"];
                            OnEventBotJoinGroupEvent?.Invoke(
                                new(
                                    new(
                                    j["group"]["id"].ToObject<long>(),
                                    j["group"]["name"].ToString(),
                                    j["group"]["permission"].ToString()
                                    ),
                                j["group"]["invitor"].ToObject<object>()
                            ));
                            return;
                        }
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
                */
        }
    }
}

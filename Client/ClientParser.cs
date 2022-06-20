using MeowMiraiLib.Msg.Sender;
using MeowMiraiLib.Msg.Type;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebSocket4Net;

/*
 * 本文为客户端事件转换类文件
 * 本文件上下文严格和 https://github.com/project-mirai/mirai-api-http/blob/master/docs/api/EventType.md 一一对应
 * 总体解释方案为 Ws_MessageReceived 信息生成方案为 RectifyMessage
 * 本源码文件不对API进行公开, 其访问性为私有(Private), 除了官方更新解释方案外严禁更改本文内容
 * ------------------
 * this file is a client convert file,
 * this file is strict consistency to website https://github.com/project-mirai/mirai-api-http/blob/master/docs/api/EventType.md
 * Main Parser is Ws_MessageReceived function/method, and Message Parser is RectifyMessage function/method
 * this file will not Expose to outer API, Its accessibility is Private, 
 * Do Not Alter Any Function/Method Except Official Updates.
 */

namespace MeowMiraiLib
{
    public partial class Client
    {
        private readonly Queue<JObject> SSMRequestList = new(); //返回队列长度
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
                        case "MarketFace": l.Add(k.ToObject<MarketFace>()); break; //20221902 added MarketFace
                        case "MusicShare": l.Add(k.ToObject<MusicShare>()); break;
                        case "Forward": l.Add(k.ToObject<ForwardMessage>()); break;
                        case "File": l.Add(k.ToObject<File>()); break;
                        case "MiraiCode": l.Add(k.ToObject<MiraiCode>()); break;
                        default:
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"[MeowMiraiLib] Err Parse Message Type On: {k["type"]}");
                                Console.ForegroundColor = default;
                            }
                            break;
                    }
                }
                return l.ToArray();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[MeowMiraiLib] {DateTime.Now:g} {ex.Message} \n--:in:--\n :{messagestr}");
                Console.ForegroundColor = default;
                Console.WriteLine();
                return null;
            }
        }
        private async void Ws_MessageReceived(object? s, MessageReceivedEventArgs e)
        {
            var jo = JObject.Parse(e.Message);
            if (debug)
            {
                Console.WriteLine(jo);
            }
            if (!string.IsNullOrWhiteSpace(jo["syncId"].ToString().Trim()))
            {
                if (jo["syncId"].ToObject<long>() != -1)
                {
                    SSMRequestList.Enqueue(jo);
                }
            }
            if (jo?["data"]?["type"] != null)
            {
                try
                {
                    switch (jo["data"]["type"].ToString())
                    {
                        case "GroupMessage":
                            {
                                await Task.Run(() => OnGroupMessageReceive?.Invoke(jo["data"]["sender"].ToObject<GroupMessageSender>(), RectifyMessage(jo["data"]["messageChain"].ToString())));
                                return;
                            }
                        case "FriendMessage":
                            {
                                await Task.Run(() => OnFriendMessageReceive?.Invoke(jo["data"]["sender"].ToObject<FriendMessageSender>(), RectifyMessage(jo["data"]["messageChain"].ToString())));
                                return;
                            }
                        case "TempMessage":
                            {
                                await Task.Run(() => OnTempMessageReceive?.Invoke(jo["data"]["sender"].ToObject<TempMessageSender>(), RectifyMessage(jo["data"]["messageChain"].ToString())));
                                return;
                            }
                        case "StrangerMessage":
                            {
                                await Task.Run(() => OnStrangerMessageReceive?.Invoke(jo["data"]["sender"].ToObject<StrangerMessageSender>(), RectifyMessage(jo["data"]["messageChain"].ToString())));
                                return;
                            }
                        /*-20220219-*/
                        case "OnFriendSyncMessageReceive":
                            {
                                await Task.Run(() => OnFriendSyncMessageReceive?.Invoke(jo["data"]["sender"].ToObject<FriendSyncMessageSender>(), RectifyMessage(jo["data"]["messageChain"].ToString())));
                                return;
                            }
                        case "OnGroupSyncMessageReceive":
                            {
                                await Task.Run(() => OnGroupSyncMessageReceive?.Invoke(jo["data"]["sender"].ToObject<GroupSyncMessageSender>(), RectifyMessage(jo["data"]["messageChain"].ToString())));
                                return;
                            }
                        case "OnTempSyncMessageReceive":
                            {
                                await Task.Run(() => OnTempSyncMessageReceive?.Invoke(jo["data"]["sender"].ToObject<TempSyncMessageSender>(), RectifyMessage(jo["data"]["messageChain"].ToString())));
                                return;
                            }
                        case "OnStrangerSyncMessageReceive":
                            {
                                await Task.Run(() => OnStrangerSyncMessageReceive?.Invoke(jo["data"]["sender"].ToObject<StrangerSyncMessageSender>(), RectifyMessage(jo["data"]["messageChain"].ToString())));
                                return;
                            }
                        /*-*/
                        case "OtherClientMessage":
                            {
                                await Task.Run(() => OnOtherMessageReceive?.Invoke(jo["data"]["sender"].ToObject<OtherClientMessageSender>(), RectifyMessage(jo["data"]["messageChain"].ToString())));
                                return;
                            }
                        case "BotOnlineEvent":
                            {
                                await Task.Run(() => OnEventBotOnlineEvent?.Invoke(new(jo["data"]["qq"].ToObject<long>())));
                                return;
                            }
                        case "BotOfflineEventActive":
                            {
                                await Task.Run(() => OnEventBotOfflineEventActive?.Invoke(new(jo["data"]["qq"].ToObject<long>())));
                                return;
                            }
                        case "BotOfflineEventForce":
                            {
                                await Task.Run(() => OnEventBotOfflineEventForce?.Invoke(new(jo["data"]["qq"].ToObject<long>())));
                                return;
                            }
                        case "BotOfflineEventDropped":
                            {
                                await Task.Run(() => OnEventBotOfflineEventDropped?.Invoke(new(jo["data"]["qq"].ToObject<long>())));
                                return;
                            }
                        case "BotReloginEvent":
                            {
                                await Task.Run(() => OnEventBotReloginEvent?.Invoke(new(jo["data"]["qq"].ToObject<long>())));
                                return;
                            }
                        case "FriendInputStatusChangedEvent":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventFriendInputStatusChangedEvent?.Invoke(
                                    new(
                                        new(
                                            j["friend"]["id"].ToObject<long>(),
                                            j["friend"]["nickname"].ToString(),
                                            j["friend"]["remark"].ToString()
                                            ),
                                        j["inputting"].ToObject<bool>()
                                        )
                                    ));
                                return;
                            }
                        case "FriendNickChangedEvent":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventFriendNickChangedEvent?.Invoke(
                                    new(
                                        new(
                                            j["friend"]["id"].ToObject<long>(),
                                            j["friend"]["nickname"].ToString(),
                                            j["friend"]["remark"].ToString()
                                            ),
                                        j["from"].ToString(),
                                        j["to"].ToString())
                                    ));
                                return;
                            }
                        case "BotGroupPermissionChangeEvent":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventBotGroupPermissionChangeEvent?.Invoke(
                                    new(
                                        j["origin"].ToString(),
                                        j["current"].ToString(),
                                            new(
                                            j["group"]["id"].ToObject<long>(),
                                            j["group"]["name"].ToString(),
                                            j["group"]["permission"].ToString()
                                            )
                                        )
                                    ));
                                return;
                            }
                        case "BotMuteEvent":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventBotMuteEvent?.Invoke(
                                    new(
                                        j["durationSeconds"].ToObject<long>(),
                                        new(
                                            j["operator"]["id"].ToObject<long>(),
                                            j["operator"]["member"].ToString(),
                                            j["operator"]["permission"].ToString(),
                                            j["operator"]["specialTitle"].ToString(),
                                            j["operator"]["joinTimestamp"].ToObject<long>(),
                                            j["operator"]["lastSpeakTimestamp"].ToObject<long>(),
                                            j["operator"]["muteTimeRemaining"].ToObject<long>(),
                                            new(
                                                j["operator"]["group"]["id"].ToObject<long>(),
                                                j["operator"]["group"]["name"].ToString(),
                                                j["operator"]["group"]["permission"].ToString()
                                                )
                                            )
                                        )
                                    ));
                                return;
                            }
                        case "BotUnmuteEvent":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventBotUnmuteEvent?.Invoke(
                                    new(
                                        new(
                                            j["operator"]["id"].ToObject<long>(),
                                            j["operator"]["member"].ToString(),
                                            j["operator"]["permission"].ToString(),
                                            j["operator"]["specialTitle"].ToString(),
                                            j["operator"]["joinTimestamp"].ToObject<long>(),
                                            j["operator"]["lastSpeakTimestamp"].ToObject<long>(),
                                            j["operator"]["muteTimeRemaining"].ToObject<long>(),
                                            new(
                                                j["operator"]["group"]["id"].ToObject<long>(),
                                                j["operator"]["group"]["name"].ToString(),
                                                j["operator"]["group"]["permission"].ToString()
                                                )
                                            )
                                        )
                                    ));
                                return;
                            }
                        case "BotJoinGroupEvent":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventBotJoinGroupEvent?.Invoke(
                                    new(
                                        new(
                                            j["group"]["id"].ToObject<long>(),
                                            j["group"]["name"].ToString(),
                                            j["group"]["permission"].ToString()
                                            ),
                                        j["group"]["invitor"].ToObject<object>()
                                    )
                                ));
                                return;
                            }
                        case "BotLeaveEventActive":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventBotLeaveEventActive?.Invoke(
                                    new(
                                        new(
                                            j["group"]["id"].ToObject<long>(),
                                            j["group"]["name"].ToString(),
                                            j["group"]["permission"].ToString()
                                            )
                                        )
                                    ));
                                return;
                            }
                        case "BotLeaveEventKick":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventBotLeaveEventKick?.Invoke(
                                    new(
                                        new(
                                            j["group"]["id"].ToObject<long>(),
                                            j["group"]["name"].ToString(),
                                            j["group"]["permission"].ToString()
                                            ),
                                        j["operator"].ToObject<object>()
                                        )
                                    ));
                                return;
                            }
                        case "GroupRecallEvent":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventGroupRecallEvent?.Invoke(
                                    new(
                                        j["authorId"].ToObject<long>(),
                                        j["messageId"].ToObject<long>(),
                                        j["time"].ToObject<long>(),
                                        new(
                                            j["group"]["id"].ToObject<long>(),
                                            j["group"]["name"].ToString(),
                                            j["group"]["permission"].ToString()
                                            ),
                                        new(
                                            j["operator"]["id"].ToObject<long>(),
                                            j["operator"]["member"].ToString(),
                                            j["operator"]["permission"].ToString(),
                                            j["operator"]["specialTitle"].ToString(),
                                            j["operator"]["joinTimestamp"].ToObject<long>(),
                                            j["operator"]["lastSpeakTimestamp"].ToObject<long>(),
                                            j["operator"]["muteTimeRemaining"].ToObject<long>(),
                                            new(
                                                j["operator"]["group"]["id"].ToObject<long>(),
                                                j["operator"]["group"]["name"].ToString(),
                                                j["operator"]["group"]["permission"].ToString()
                                                )
                                            )
                                        )
                                    ));
                                return;
                            }
                        case "FriendRecallEvent":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventFriendRecallEvent?.Invoke(
                                    new(
                                        j["authorId"].ToObject<long>(),
                                        j["messageId"].ToObject<long>(),
                                        j["time"].ToObject<long>(),
                                        j["operator"].ToObject<long>()
                                        )
                                    ));
                                return;
                            }
                        case "NudgeEvent":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventNudgeEvent?.Invoke(new(
                                    j["fromId"].ToObject<long>(),
                                    j["target"].ToObject<long>(),
                                    j["subject"]["kind"].ToString(),
                                    j["subject"]["id"].ToObject<long>(),
                                    j["action"].ToString(),
                                    j["suffix"].ToString()
                                )));
                                return;
                            }
                        case "GroupNameChangeEvent":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventGroupNameChangeEvent?.Invoke(
                                    new(
                                        j["origin"].ToString(),
                                        j["current"].ToString(),
                                        new(
                                            j["group"]["id"].ToObject<long>(),
                                            j["group"]["name"].ToString(),
                                            j["group"]["permission"].ToString()
                                            ),
                                        new(
                                            j["operator"]["id"].ToObject<long>(),
                                            j["operator"]["member"].ToString(),
                                            j["operator"]["permission"].ToString(),
                                            j["operator"]["specialTitle"].ToString(),
                                            j["operator"]["joinTimestamp"].ToObject<long>(),
                                            j["operator"]["lastSpeakTimestamp"].ToObject<long>(),
                                            j["operator"]["muteTimeRemaining"].ToObject<long>(),
                                            new(
                                                j["operator"]["group"]["id"].ToObject<long>(),
                                                j["operator"]["group"]["name"].ToString(),
                                                j["operator"]["group"]["permission"].ToString()
                                                )
                                            )
                                        )
                                    ));
                                return;
                            }
                        case "GroupEntranceAnnouncementChangeEvent":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventGroupEntranceAnnouncementChangeEvent?.Invoke(
                                    new(
                                        j["origin"].ToString(),
                                        j["current"].ToString(),
                                        new(
                                            j["group"]["id"].ToObject<long>(),
                                            j["group"]["name"].ToString(),
                                            j["group"]["permission"].ToString()
                                            ),
                                        new(
                                            j["operator"]["id"].ToObject<long>(),
                                            j["operator"]["member"].ToString(),
                                            j["operator"]["permission"].ToString(),
                                            j["operator"]["specialTitle"].ToString(),
                                            j["operator"]["joinTimestamp"].ToObject<long>(),
                                            j["operator"]["lastSpeakTimestamp"].ToObject<long>(),
                                            j["operator"]["muteTimeRemaining"].ToObject<long>(),
                                            new(
                                                j["operator"]["group"]["id"].ToObject<long>(),
                                                j["operator"]["group"]["name"].ToString(),
                                                j["operator"]["group"]["permission"].ToString()
                                                )
                                            )
                                        )
                                    ));
                                return;
                            }
                        case "GroupMuteAllEvent":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventGroupMuteAllEvent?.Invoke(
                                    new(
                                        j["origin"].ToObject<bool>(),
                                        j["current"].ToObject<bool>(),
                                        new(
                                            j["group"]["id"].ToObject<long>(),
                                            j["group"]["name"].ToString(),
                                            j["group"]["permission"].ToString()
                                            ),
                                        new(
                                            j["operator"]["id"].ToObject<long>(),
                                            j["operator"]["member"].ToString(),
                                            j["operator"]["permission"].ToString(),
                                            j["operator"]["specialTitle"].ToString(),
                                            j["operator"]["joinTimestamp"].ToObject<long>(),
                                            j["operator"]["lastSpeakTimestamp"].ToObject<long>(),
                                            j["operator"]["muteTimeRemaining"].ToObject<long>(),
                                            new(
                                                j["operator"]["group"]["id"].ToObject<long>(),
                                                j["operator"]["group"]["name"].ToString(),
                                                j["operator"]["group"]["permission"].ToString()
                                                )
                                            )
                                        )
                                    ));
                                return;
                            }
                        case "GroupAllowAnonymousChatEvent":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventGroupAllowAnonymousChatEvent?.Invoke(
                                    new(
                                        j["origin"].ToObject<bool>(),
                                        j["current"].ToObject<bool>(),
                                        new(
                                            j["group"]["id"].ToObject<long>(),
                                            j["group"]["name"].ToString(),
                                            j["group"]["permission"].ToString()
                                            ),
                                        new(
                                            j["operator"]["id"].ToObject<long>(),
                                            j["operator"]["member"].ToString(),
                                            j["operator"]["permission"].ToString(),
                                            j["operator"]["specialTitle"].ToString(),
                                            j["operator"]["joinTimestamp"].ToObject<long>(),
                                            j["operator"]["lastSpeakTimestamp"].ToObject<long>(),
                                            j["operator"]["muteTimeRemaining"].ToObject<long>(),
                                            new(
                                                j["operator"]["group"]["id"].ToObject<long>(),
                                                j["operator"]["group"]["name"].ToString(),
                                                j["operator"]["group"]["permission"].ToString()
                                                )
                                            )
                                        )
                                    ));
                                return;
                            }
                        case "GroupAllowConfessTalkEvent":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventGroupAllowConfessTalkEvent?.Invoke(
                                    new(
                                        j["origin"].ToObject<bool>(),
                                        j["current"].ToObject<bool>(),
                                        new(
                                            j["group"]["id"].ToObject<long>(),
                                            j["group"]["name"].ToString(),
                                            j["group"]["permission"].ToString()
                                            ),
                                        j["isByBot"].ToObject<bool>()
                                        )
                                    ));
                                return;
                            }
                        case "GroupAllowMemberInviteEvent":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventGroupAllowMemberInviteEvent?.Invoke(
                                    new(
                                        j["origin"].ToObject<bool>(),
                                        j["current"].ToObject<bool>(),
                                        new(
                                            j["group"]["id"].ToObject<long>(),
                                            j["group"]["name"].ToString(),
                                            j["group"]["permission"].ToString()
                                            ),
                                        new(
                                            j["operator"]["id"].ToObject<long>(),
                                            j["operator"]["member"].ToString(),
                                            j["operator"]["permission"].ToString(),
                                            j["operator"]["specialTitle"].ToString(),
                                            j["operator"]["joinTimestamp"].ToObject<long>(),
                                            j["operator"]["lastSpeakTimestamp"].ToObject<long>(),
                                            j["operator"]["muteTimeRemaining"].ToObject<long>(),
                                            new(
                                                j["operator"]["group"]["id"].ToObject<long>(),
                                                j["operator"]["group"]["name"].ToString(),
                                                j["operator"]["group"]["permission"].ToString()
                                                )
                                            )
                                        )
                                    ));
                                return;
                            }
                        case "MemberJoinEvent":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventMemberJoinEvent?.Invoke(
                                    new(
                                        new(
                                            j["member"]["id"].ToObject<long>(),
                                            j["member"]["memberName"].ToString(),
                                            j["member"]["specialTitle"].ToString(),
                                            j["member"]["permission"].ToString(),
                                            j["member"]["joinTimestamp"].ToObject<long>(),
                                            j["member"]["lastSpeakTimestamp"].ToObject<long>(),
                                            j["member"]["muteTimeRemaining"].ToObject<long>(),
                                            new(
                                                j["member"]["group"]["id"].ToObject<long>(),
                                                j["member"]["group"]["name"].ToString(),
                                                j["member"]["group"]["permission"].ToString()
                                                )
                                            ),
                                        j["invitor"].ToObject<object>()
                                        )
                                    ));
                                return;
                            }
                        case "MemberLeaveEventKick":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventMemberLeaveEventKick?.Invoke(
                                    new(
                                        new(
                                            j["member"]["id"].ToObject<long>(),
                                            j["member"]["memberName"].ToString(),
                                            j["member"]["specialTitle"].ToString(),
                                            j["member"]["permission"].ToString(),
                                            j["member"]["joinTimestamp"].ToObject<long>(),
                                            j["member"]["lastSpeakTimestamp"].ToObject<long>(),
                                            j["member"]["muteTimeRemaining"].ToObject<long>(),
                                            new(
                                                j["member"]["group"]["id"].ToObject<long>(),
                                                j["member"]["group"]["name"].ToString(),
                                                j["member"]["group"]["permission"].ToString()
                                                )
                                            ),
                                        new(
                                            j["operator"]["id"].ToObject<long>(),
                                            j["operator"]["member"].ToString(),
                                            j["operator"]["permission"].ToString(),
                                            j["operator"]["specialTitle"].ToString(),
                                            j["operator"]["joinTimestamp"].ToObject<long>(),
                                            j["operator"]["lastSpeakTimestamp"].ToObject<long>(),
                                            j["operator"]["muteTimeRemaining"].ToObject<long>(),
                                            new(
                                                j["operator"]["group"]["id"].ToObject<long>(),
                                                j["operator"]["group"]["name"].ToString(),
                                                j["operator"]["group"]["permission"].ToString()
                                                )
                                            )

                                        )
                                    ));
                                return;
                            }
                        case "MemberLeaveEventQuit":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventMemberLeaveEventQuit?.Invoke(
                                    new(
                                        new(
                                            j["member"]["id"].ToObject<long>(),
                                            j["member"]["memberName"].ToString(),
                                            j["member"]["permission"].ToString(),
                                            new(
                                                j["member"]["group"]["id"].ToObject<long>(),
                                                j["member"]["group"]["name"].ToString(),
                                                j["member"]["group"]["permission"].ToString()
                                                )
                                            )
                                        )
                                    ));
                                return;
                            }
                        case "MemberCardChangeEvent":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventCardChangeEvent?.Invoke(
                                    new(
                                        j["origin"].ToString(),
                                        j["current"].ToString(),
                                        new(
                                            j["member"]["id"].ToObject<long>(),
                                            j["member"]["memberName"].ToString(),
                                            j["member"]["specialTitle"].ToString(),
                                            j["member"]["permission"].ToString(),
                                            j["member"]["joinTimestamp"].ToObject<long>(),
                                            j["member"]["lastSpeakTimestamp"].ToObject<long>(),
                                            j["member"]["muteTimeRemaining"].ToObject<long>(),
                                            new(
                                                j["member"]["group"]["id"].ToObject<long>(),
                                                j["member"]["group"]["name"].ToString(),
                                                j["member"]["group"]["permission"].ToString()
                                                )
                                            )
                                        )
                                    ));
                                return;
                            }
                        case "MemberSpecialTitleChangeEvent":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventSpecialTitleChangeEvent?.Invoke(
                                    new(
                                        j["origin"].ToString(),
                                        j["current"].ToString(),
                                        new(
                                            j["member"]["id"].ToObject<long>(),
                                            j["member"]["memberName"].ToString(),
                                            j["member"]["permission"].ToString(),
                                            new(
                                                j["member"]["group"]["id"].ToObject<long>(),
                                                j["member"]["group"]["name"].ToString(),
                                                j["member"]["group"]["permission"].ToString()
                                                )
                                            )
                                        )
                                    ));
                                return;
                            }
                        case "MemberPermissionChangeEvent":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventPermissionChangeEvent?.Invoke(
                                    new(
                                        j["origin"].ToString(),
                                        j["current"].ToString(),
                                        new(
                                            j["member"]["id"].ToObject<long>(),
                                            j["member"]["memberName"].ToString(),
                                            j["member"]["permission"].ToString(),
                                            new(
                                                j["member"]["group"]["id"].ToObject<long>(),
                                                j["member"]["group"]["name"].ToString(),
                                                j["member"]["group"]["permission"].ToString()
                                                )
                                            )
                                        )
                                    ));
                                return;
                            }
                        case "MemberMuteEvent":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventMemberMuteEvent?.Invoke(
                                    new(
                                        j["durationSeconds"].ToObject<long>(),
                                        new(
                                            j["member"]["id"].ToObject<long>(),
                                            j["member"]["memberName"].ToString(),
                                            j["member"]["specialTitle"].ToString(),
                                            j["member"]["permission"].ToString(),
                                            j["member"]["joinTimestamp"].ToObject<long>(),
                                            j["member"]["lastSpeakTimestamp"].ToObject<long>(),
                                            j["member"]["muteTimeRemaining"].ToObject<long>(),
                                            new(
                                                j["member"]["group"]["id"].ToObject<long>(),
                                                j["member"]["group"]["name"].ToString(),
                                                j["member"]["group"]["permission"].ToString()
                                                )
                                            ),
                                        new(
                                            j["operator"]["id"].ToObject<long>(),
                                            j["operator"]["memberName"].ToString(),
                                            j["operator"]["specialTitle"].ToString(),
                                            j["operator"]["permission"].ToString(),
                                            j["operator"]["joinTimestamp"].ToObject<long>(),
                                            j["operator"]["lastSpeakTimestamp"].ToObject<long>(),
                                            j["operator"]["muteTimeRemaining"].ToObject<long>(),
                                            new(
                                                j["operator"]["group"]["id"].ToObject<long>(),
                                                j["operator"]["group"]["name"].ToString(),
                                                j["operator"]["group"]["permission"].ToString()
                                                )
                                            )
                                        )
                                    ));
                                return;
                            }
                        case "MemberUnmuteEvent":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventMemberUnmuteEvent?.Invoke(
                                    new(
                                        new(
                                            j["member"]["id"].ToObject<long>(),
                                            j["member"]["memberName"].ToString(),
                                            j["member"]["specialTitle"].ToString(),
                                            j["member"]["permission"].ToString(),
                                            j["member"]["joinTimestamp"].ToObject<long>(),
                                            j["member"]["lastSpeakTimestamp"].ToObject<long>(),
                                            j["member"]["muteTimeRemaining"].ToObject<long>(),
                                            new(
                                                j["member"]["group"]["id"].ToObject<long>(),
                                                j["member"]["group"]["name"].ToString(),
                                                j["member"]["group"]["permission"].ToString()
                                                )
                                            ),
                                        new(
                                            j["operator"]["id"].ToObject<long>(),
                                            j["operator"]["memberName"].ToString(),
                                            j["operator"]["specialTitle"].ToString(),
                                            j["operator"]["permission"].ToString(),
                                            j["operator"]["joinTimestamp"].ToObject<long>(),
                                            j["operator"]["lastSpeakTimestamp"].ToObject<long>(),
                                            j["operator"]["muteTimeRemaining"].ToObject<long>(),
                                            new(
                                                j["operator"]["group"]["id"].ToObject<long>(),
                                                j["operator"]["group"]["name"].ToString(),
                                                j["operator"]["group"]["permission"].ToString()
                                                )
                                            )
                                        )
                                    ));
                                return;
                            }
                        case "MemberHonorChangeEvent":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventMemberHonorChangeEvent?.Invoke(
                                    new(
                                        new(
                                            j["member"]["id"].ToObject<long>(),
                                            j["member"]["memberName"].ToString(),
                                            j["member"]["specialTitle"].ToString(),
                                            j["member"]["permission"].ToString(),
                                            j["member"]["joinTimestamp"].ToObject<long>(),
                                            j["member"]["lastSpeakTimestamp"].ToObject<long>(),
                                            j["member"]["muteTimeRemaining"].ToObject<long>(),
                                            new(
                                                j["member"]["group"]["id"].ToObject<long>(),
                                                j["member"]["group"]["name"].ToString(),
                                                j["member"]["group"]["permission"].ToString()
                                                )
                                            ),
                                        j["action"].ToString(),
                                        j["honor"].ToString()
                                        )
                                    ));
                                return;
                            }
                        case "NewFriendRequestEvent":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventNewFriendRequestEvent?.Invoke(
                                        new(
                                            j["eventId"].ToObject<long>(),
                                            j["fromId"].ToObject<long>(),
                                            j["groupId"].ToObject<long>(),
                                            j["nick"].ToString(),
                                            j["message"].ToString()
                                        )
                                    ));
                                return;
                            }
                        case "MemberJoinRequestEvent":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventMemberJoinRequestEvent?.Invoke(
                                        new(
                                            j["eventId"].ToObject<long>(),
                                            j["fromId"].ToObject<long>(),
                                            j["groupId"].ToObject<long>(),
                                            j["groupName"].ToString(),
                                            j["nick"].ToString(),
                                            j["message"].ToString()
                                        )
                                    ));
                                return;
                            }
                        case "BotInvitedJoinGroupRequestEvent":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => OnEventBotInvitedJoinGroupRequestEvent?.Invoke(
                                        new(
                                            j["eventId"].ToObject<long>(),
                                            j["fromId"].ToObject<long>(),
                                            j["groupId"].ToObject<long>(),
                                            j["groupName"].ToString(),
                                            j["nick"].ToString(),
                                            j["message"].ToString()
                                        )
                                    ));
                                return;
                            }
                        case "OtherClientOnlineEvent":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => _OnClientOnlineEvent?.Invoke(
                                    new(
                                        j["client"]["id"].ToObject<long>(),
                                        j["client"]["platform"].ToString(),
                                        j["kind"].ToObject<long>()
                                        )
                                    ));
                                return;
                            }
                        case "OtherClientOfflineEvent":
                            {
                                JToken j = jo["data"];
                                await Task.Run(() => _OnOtherClientOfflineEvent?.Invoke(
                                    new(
                                        j["client"]["id"].ToObject<long>(),
                                        j["client"]["platform"].ToString()
                                        )
                                    ));
                                return;
                            }
                        case "CommandExecutedEvent":
                            {
                                JToken j = jo["data"];
                                JArray ja = JArray.Parse(j["args"].ToString());
                                List<(string, string)> args = new();
                                foreach (JToken i in ja)
                                {
                                    args.Add((i["type"].ToString(), i["text"].ToString()));
                                }
                                await Task.Run(() => _OnCommandExecutedEvent?.Invoke(
                                    new(
                                        j["name"].ToString(),
                                        j["friend"].ToObject<object>(),
                                        j["member"].ToObject<object>(),
                                        args
                                        )
                                    ));
                                return;
                            }
                        default:
                            {
                                await Task.Run(() => _OnUnknownEvent?.Invoke(jo.ToString()));
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
                    await Task.Run(() => _OnServeiceConnected?.Invoke(jo.ToString()));
                    return;
                }
            }
        }
    }
}

using MeowMiraiLib.Msg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowMiraiLib.Event
{
    public enum EventType
    {
        BotJoinGroupEvent,
        NewFriendRequestEvent,
        NudgeEvent,
    }
    public class NewFriendRequestEvent : EventArgs
    {
        public EventType type;
        public long eventId;
        public long fromId;
        public long groupId;
        public string nick;
        public string message;

        public NewFriendRequestEvent(long eventId, long fromId, long groupId, string nick, string message)
        {
            this.type = EventType.BotJoinGroupEvent;
            this.eventId = eventId;
            this.fromId = fromId;
            this.groupId = groupId;
            this.nick = nick;
            this.message = message;
        }
    }
    public class NudgeEvent : EventArgs
    {
        public EventType type;
        public long fromId;
        public long target;
        public string fromKind;
        public long fromKindId;
        public string action;
        public string suffix;

        public NudgeEvent(long fromId, long target, string fromKind, 
            long fromKindId, string action, string suffix)
        {
            this.type = EventType.NudgeEvent;
            this.fromId = fromId;
            this.target = target;
            this.fromKind = fromKind;
            this.fromKindId = fromKindId;
            this.action = action;
            this.suffix = suffix;
        }
    }
    public class BotJoinGroupEvent : EventArgs
    {
        public EventType type;
        public long groupId;
        public string groupName;
        public string permission;

        public BotJoinGroupEvent(long groupId, string groupName, string permission)
        {
            this.type = EventType.BotJoinGroupEvent;
            this.groupId = groupId;
            this.groupName = groupName;
            this.permission = permission;
        }
    }
}

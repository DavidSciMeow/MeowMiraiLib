using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowMiraiLib.Event
{
    public enum EventType
    {
        BotInvitedJoinGroupRequestEvent,
        BotJoinGroupEvent,
        NudgeEvent,
    }
    public class BotInvitedJoinGroupRequestEventArg : EventArgs
    {
        public EventType type;
        public long eventId;
        public string message;
        public long fromId;
        public long groupId;
        public string groupName;
        public string inviteNick;

        public BotInvitedJoinGroupRequestEventArg(long eventId, string message, long fromId, 
            long groupId, string groupName, string inviteNick)
        {
            this.type = EventType.BotInvitedJoinGroupRequestEvent;
            this.eventId = eventId;
            this.message = message;
            this.fromId = fromId;
            this.groupId = groupId;
            this.groupName = groupName;
            this.inviteNick = inviteNick;
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
    public class BotJoinGroupEventArg : EventArgs
    {
        public EventType type;
        public long groupId;
        public string groupName;
        public string permission;

        public BotJoinGroupEventArg(long groupId, string groupName, string permission)
        {
            this.type = EventType.BotJoinGroupEvent;
            this.groupId = groupId;
            this.groupName = groupName;
            this.permission = permission;
        }
    }
}

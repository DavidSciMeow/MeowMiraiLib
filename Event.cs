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
}

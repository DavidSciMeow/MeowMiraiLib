using MeowMiraiLib.Msg;
using MeowMiraiLib.Msg.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowMiraiLib.Msg
{
    public abstract class SSM
    {
        public int syncId = new Random().Next(int.MaxValue);
        public string command;
        public string? subCommand = null;
        public dynamic content = null;
        public int Send()
        {
            var jsonSetting = new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore };
            var s = Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.None, jsonSetting);
            Client.ws.Send(s);
            return syncId;
        }
    }

    public class NudgeSomebody : SSM
    {
        public NudgeSomebody(long target, long subject, string kind)
        {
            command = "sendNudge";
            content = new resp() { target = target, subject = subject, kind = kind };
        }
        public class resp
        {
            public string SessionKey = Client.session;
            public long target;
            public long subject;
            public string kind;
        }
    }

    public class EventBotInvitedGroup : SSM
    {
        public EventBotInvitedGroup(long eventId, long fromId, long groupId, int operate, string message)
        {
            command = "resp_botInvitedJoinGroupRequestEvent";
            content = new resp()
            {
                eventId = eventId,
                fromId = fromId,
                groupId = groupId,
                operate = operate,
                message = message,
            };
    }
        public class resp
        {
            public string SessionKey = Client.session;
            public long eventId;
            public long fromId;
            public long groupId;
            public int operate;
            public string message;
        }
    }
    public class EventQuitGroup : SSM
    {
        public EventQuitGroup(long target)
        {
            command = "quit";
            content = $"{{\"sessionKey\":\"{Client.session}\",\"target\":{target}}}";
        }
    }

    public class SMessage
    {
        public long? target = null;
        public long? qq = null;
        public long? group = null;
        public long? quote = null;
        public Message[] messageChain;

        public SMessage(long qq, long group, Message[] messageChain, long? quote = null)
        {
            this.qq = qq;
            this.group = group;
            this.quote = quote;
            this.messageChain = messageChain;
        }

        public SMessage(long target, Message[] messageChain, 
            long? quote = null, long? qq = null, long? group = null)
        {
            this.target = target;
            this.quote = quote;
            this.messageChain = messageChain;
            this.qq = qq;
            this.group = group;
        }
    }
    public class Target
    {
        public long target;
        public Target(long target)
        {
            this.target = target;
        }
    }
    public class About : SSM
    {
        public About()
        {
            command = "about";
        }
    }
    public class FriendList : SSM
    {
        public FriendList()
        {
            command = "friendList";
        }
    }
    public class GroupList : SSM
    {
        public GroupList()
        {
            command = "groupList";
        }
    }
    public class MemberList : SSM
    {
        public MemberList(long target)
        {
            command = "memberList";
            content = new Target(target);
        }
    }
    public class FriendMessage : SSM
    {
        public FriendMessage(long target, Message[] messageChain, long? quote = null)
        {
            command = "sendFriendMessage";
            content = new SMessage(target, messageChain, quote);
        }
    }
    public class GroupMessage : SSM
    {
        public GroupMessage(long target, Message[] messageChain, long? quote = null)
        {
            command = "sendGroupMessage";
            content = new SMessage(target, messageChain, quote);
        }
    }
    public class TempMessage : SSM
    {
        public TempMessage(long qq,long group, Message[] messageChain, long? quote = null)
        {
            command = "sendTempMessage";
            content = new SMessage(qq, group, messageChain, quote);
        }
    }
}

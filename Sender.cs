using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowMiraiLib.Msg.Sender
{
    public class Sender
    {
        public long id;
    }
    public class FriendMessageSender : Sender
    {
        public string nickname;
        public string remark;
    }
    public class GroupMessageSender : Sender
    {
        public string memberName;
        public string specialTitle;
        public string permission;
        public long joinTimestamp;
        public long lastSpeakTimestamp;
        public long muteTimeRemaining;
        public Group group;
        public class Group
        {
            public long id;
            public string name;
            public string permission;
        }
    }
    public class TempMessageSender : GroupMessageSender { }
    public class StrangerMessageSender : FriendMessageSender { }
    public class OtherClientMessageSender : Sender
    {
        public string platform;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowMiraiLib.Msg.Type
{
    public abstract class Message
    {
        public string type;
    }
    public class Source : Message
    {
        public long id;
        public long time;

        public Source(long id, long time)
        {
            this.id = id;
            this.time = time;
            this.type = nameof(Source);
        }
    }
    public class Quote : Message
    {
        public long id;
        public long groupId;
        public long senderId;
        public long targetId;
        public Message[] origin;

        public Quote(long id, long groupId, long senderId, long targetId, Message[] origin)
        {
            this.id = id;
            this.groupId = groupId;
            this.senderId = senderId;
            this.targetId = targetId;
            this.origin = origin;
            this.type = nameof(Quote);
        }
    }
    public class At : Message
    {
        public long target;
        public string display;

        public At(long target, string display)
        {
            this.target = target;
            this.display = display;
            this.type = nameof(At);
        }
    }
    public class AtAll : Message
    {
        public AtAll()
        {
            this.type = nameof(AtAll);
        }
    }
    public class Face : Message
    {
        public long faceId;
        public string name;

        public Face(long faceId, string name)
        {
            this.faceId = faceId;
            this.name = name;
            this.type = nameof(Face);
        }
    }
    public class Plain : Message
    {
        public string text;

        public Plain(string text)
        {
            this.text = text;
            this.type = nameof(Plain);
        }
    }
    public class Image : Message
    {
        public string? imageId = null;
        public string? url = null;
        public string? path = null;
        public string? base64 = null;

        public Image(string imageId = null, string url = null, string path = null, string base64 = null)
        {
            this.imageId = imageId;
            this.url = url;
            this.path = path;
            this.base64 = base64;
            this.type = nameof(Image);
        }
    }
    public class FlashImage : Image
    {
        public FlashImage(string imageId = null, string url = null, string path = null, string base64 = null) : base(imageId, url, path, base64)
        {
            this.type = nameof(FlashImage);
        }
    }
    public class Voice : Message
    {
        public string voiceId = null;
        public string url = null;
        public string path = null;
        public string base64 = null;

        public Voice(string voiceId = null, string url = null, string path = null, string base64 = null)
        {
            this.voiceId = voiceId;
            this.url = url;
            this.path = path;
            this.base64 = base64;
            this.type = nameof(Voice);
        }
    }
    public class Xml : Message
    {
        public string xml;

        public Xml(string xml)
        {
            this.xml = xml;
            this.type = nameof(Xml);
        }
    }
    public class Json : Message
    {
        public string json;

        public Json(string json)
        {
            this.json = json;
            this.type = nameof(Json);
        }
    }
    public class App : Message
    {
        public string content;

        public App(string content)
        {
            this.content = content;
            this.type = nameof(App);
        }
    }
    public class Poke : Message
    {
        public string name;

        public Poke(string name)
        {
            this.name = name;
            this.type = nameof(Poke);
        }
    }
    public class Dice : Message
    {
        public int value;

        public Dice(int value)
        {
            this.value = value;
            this.type = nameof(Dice);
        }
    }
    public class MusicShare : Message
    {
        public string kind;
        public string title;
        public string summary;
        public string jumpUrl;
        public string pictureUrl;
        public string musicUrl;
        public string brief;

        public MusicShare(string kind, string title, string summary, string jumpUrl, string pictureUrl, string musicUrl, string brief)
        {
            this.kind = kind;
            this.title = title;
            this.summary = summary;
            this.jumpUrl = jumpUrl;
            this.pictureUrl = pictureUrl;
            this.musicUrl = musicUrl;
            this.brief = brief;
            this.type = nameof(MusicShare);
        }
    }
    public class ForwardMessage : Message
    {
        public class Node
        {
            public long senderId;
            public long time;
            public string senderName;
            public Message[] messageChain;
            public long sourceId;
        }
        public Node[] nodeList;

        public ForwardMessage(Node[] nodeList)
        {
            this.nodeList = nodeList;
            this.type = nameof(ForwardMessage);
        }
    }
    public class File : Message
    {
        public string id;
        public string name;
        public long size;

        public File(string id, string name, long size)
        {
            this.id = id;
            this.name = name;
            this.size = size;
            this.type = nameof(File);
        }
    }
}

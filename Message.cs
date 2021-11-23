/* 消息类型定义文件 
 * 本文件和 https://github.com/project-mirai/mirai-api-http/blob/master/docs/api/MessageType.md 文件形成一对一映射关系
 * 所有消息类都会派生自基类 Message, 除了闪照派生自Image类外, 其他所有类均为密封类(最终类), 防止不正常的继承关系
 * 理论上来讲, 如果官方不更改解析操作, 除了Mirai码, 其他消息会由 ClientParser 进行解析并且形成一个 Message[] 数组
 * 关于信息类的类构造的操作类的标准文件以及Mirai码, 请访问: https://github.com/mamoe/mirai/blob/dev/docs/Messages.md 
 * ------------------------------
 * Message Type Defines File
 * this file is one to one match of website: https://github.com/project-mirai/mirai-api-http/blob/master/docs/api/MessageType.md
 * all class is directed inherit/derive from Base-class Message, except the class "FlashImage" is from Image.
 * all class is sealed(final)-class, due to prevent unexpect relation of inherit/derive
 * theoretically, if not officially alter the parser pattern, 
 * except 'Mirai Code', all other message will be parsing into an Array of message as 'Message[]'
 * for more of message and classes standard or Mirai Code, Visit website: https://github.com/mamoe/mirai/blob/dev/docs/Messages.md
 */
namespace MeowMiraiLib.Msg.Type
{
    /// <summary>
    /// 信息类的公开定义
    /// </summary>
    public class Message
    {
        /// <summary>
        /// 信息类型
        /// </summary>
        public string type;
    }
    /// <summary>
    /// 源类型(永远为信息链的第一个元素)
    /// </summary>
    public sealed class Source : Message
    {
        /// <summary>
        /// 消息的识别号，用于引用回复
        /// </summary>
        public long id;
        /// <summary>
        /// 时间戳
        /// </summary>
        public long time;
        /// <summary>
        /// 源类型(永远为信息链的第一个元素)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="time"></param>
        public Source(long id, long time)
        {
            this.id = id;
            this.time = time;
            this.type = nameof(Source);
        }
    }
    /// <summary>
    /// 回复类信息
    /// </summary>
    public sealed class Quote : Message
    {
        /// <summary>
        /// 被引用回复的原消息的messageId
        /// </summary>
        public long id;
        /// <summary>
        /// 被引用回复的原消息所接收的群号，当为好友消息时为0
        /// </summary>
        public long groupId;
        /// <summary>
        /// 被引用回复的原消息的发送者的QQ号
        /// </summary>
        public long senderId;
        /// <summary>
        /// 被引用回复的原消息的接收者者的QQ号（或群号）
        /// </summary>
        public long targetId;
        /// <summary>
        /// 被引用回复的原消息的消息链对象
        /// </summary>
        public Message[] origin;
        /// <summary>
        /// 回复类信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="groupId"></param>
        /// <param name="senderId"></param>
        /// <param name="targetId"></param>
        /// <param name="origin"></param>
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
    /// <summary>
    /// @类信息
    /// </summary>
    public sealed class At : Message
    {
        /// <summary>
        /// 群员QQ号
        /// </summary>
        public long target;
        /// <summary>
        /// At时显示的文字，发送消息时无效，自动使用群名片
        /// </summary>
        public string display;
        /// <summary>
        /// @类信息
        /// </summary>
        /// <param name="target"></param>
        /// <param name="display"></param>
        public At(long target, string display)
        {
            this.target = target;
            this.display = display;
            this.type = nameof(At);
        }
    }
    /// <summary>
    /// @全体类信息
    /// </summary>
    public sealed class AtAll : Message
    {
        /// <summary>
        /// @全体类信息
        /// </summary>
        public AtAll()
        {
            this.type = nameof(AtAll);
        }
    }
    /// <summary>
    /// 小图脸
    /// </summary>
    public sealed class Face : Message
    {
        /// <summary>
        /// QQ表情编号，可选，优先高于name
        /// </summary>
        public long faceId;
        /// <summary>
        /// QQ表情拼音，可选
        /// </summary>
        public string name;
        /// <summary>
        /// 小图脸
        /// </summary>
        /// <param name="faceId">QQ表情编号，可选，优先高于name</param>
        /// <param name="name">QQ表情拼音，可选</param>
        public Face(long faceId, string name)
        {
            this.faceId = faceId;
            this.name = name;
            this.type = nameof(Face);
        }
    }
    /// <summary>
    /// 文字消息
    /// </summary>
    public sealed class Plain : Message
    {
        /// <summary>
        /// 文字
        /// </summary>
        public string text;
        /// <summary>
        /// 文字消息
        /// </summary>
        /// <param name="text">文字</param>
        public Plain(string text)
        {
            this.text = text;
            this.type = nameof(Plain);
        }
    }
    /// <summary>
    /// 图片信息
    /// </summary>
    public class Image : Message
    {
        /// <summary>
        /// 图片的imageId，群图片与好友图片格式不同。不为空时将忽略url属性
        /// </summary>
        public string? imageId = null;
        /// <summary>
        /// 图片的URL，发送时可作网络图片的链接；接收时为腾讯图片服务器的链接，可用于图片下载
        /// </summary>
        public string? url = null;
        /// <summary>
        /// 图片的路径，发送本地图片，路径相对于 JVM 工作路径（默认是当前路径，可通过 -Duser.dir=...指定），也可传入绝对路径。
        /// </summary>
        public string? path = null;
        /// <summary>
        /// 图片的 Base64 编码
        /// </summary>
        public string? base64 = null;
        /// <summary>
        /// 图片信息(构造参数任选其一，出现多个参数时，按照imageId > url > path > base64的优先级)
        /// </summary>
        /// <param name="imageId">图片的imageId，群图片与好友图片格式不同。不为空时将忽略url属性</param>
        /// <param name="url">图片的URL，发送时可作网络图片的链接；接收时为腾讯图片服务器的链接，可用于图片下载</param>
        /// <param name="path">图片的路径，发送本地图片，路径相对于 JVM 工作路径（默认是当前路径，可通过 -Duser.dir=...指定），也可传入绝对路径。</param>
        /// <param name="base64">图片的 Base64 编码</param>
        public Image(string imageId = null, string url = null, string path = null, string base64 = null)
        {
            this.imageId = imageId;
            this.url = url;
            this.path = path;
            this.base64 = base64;
            this.type = nameof(Image);
        }
    }
    /// <summary>
    /// 闪照图片信息
    /// </summary>
    public sealed class FlashImage : Image
    {
        /// <summary>
        /// 闪照图片(构造参数任选其一，出现多个参数时，按照imageId > url > path > base64的优先级)
        /// </summary>
        /// <param name="imageId">图片的imageId，群图片与好友图片格式不同。不为空时将忽略url属性</param>
        /// <param name="url">图片的URL，发送时可作网络图片的链接；接收时为腾讯图片服务器的链接，可用于图片下载</param>
        /// <param name="path">图片的路径，发送本地图片，路径相对于 JVM 工作路径（默认是当前路径，可通过 -Duser.dir=...指定），也可传入绝对路径。</param>
        /// <param name="base64">图片的 Base64 编码</param>
        public FlashImage(string imageId = null, string url = null, string path = null, string base64 = null) : base(imageId, url, path, base64)
        {
            this.type = nameof(FlashImage);
        }
    }
    /// <summary>
    /// 语音信息
    /// </summary>
    public sealed class Voice : Message
    {
        /// <summary>
        /// 语音的voiceId，不为空时将忽略url属性
        /// </summary>
        public string voiceId = null;
        /// <summary>
        /// 语音的URL，发送时可作网络语音的链接；接收时为腾讯语音服务器的链接，可用于语音下载
        /// </summary>
        public string url = null;
        /// <summary>
        /// 语音的路径，发送本地语音，路径相对于 JVM 工作路径（默认是当前路径，可通过 -Duser.dir=...指定），也可传入绝对路径。
        /// </summary>
        public string path = null;
        /// <summary>
        /// 语音的 Base64 编码
        /// </summary>
        public string base64 = null;
        /// <summary>
        /// 返回的语音长度, 发送消息时可以不传
        /// </summary>
        public long length;
        /// <summary>
        /// 语音信息(构造参数任选其一，出现多个参数时，按照voiceId > url > path > base64的优先级)
        /// </summary>
        /// <param name="voiceId">语音的voiceId，不为空时将忽略url属性</param>
        /// <param name="url">语音的URL，发送时可作网络语音的链接；接收时为腾讯语音服务器的链接，可用于语音下载</param>
        /// <param name="path">语音的路径，发送本地语音，路径相对于 JVM 工作路径（默认是当前路径，可通过 -Duser.dir=...指定），也可传入绝对路径。</param>
        /// <param name="base64">语音的 Base64 编码</param>
        /// <param name="length">返回的语音长度, 发送消息时可以不传</param>
        public Voice(string voiceId = null, string url = null, string path = null, string base64 = null, long length = 0)
        {
            this.voiceId = voiceId;
            this.url = url;
            this.path = path;
            this.base64 = base64;
            this.length = length;
            this.type = nameof(Voice);
        }
    }
    /// <summary>
    /// XML信息
    /// </summary>
    public sealed class Xml : Message
    {
        /// <summary>
        /// XML文本
        /// </summary>
        public string xml;
        /// <summary>
        /// XML信息
        /// </summary>
        /// <param name="xml">XML文本</param>
        public Xml(string xml)
        {
            this.xml = xml;
            this.type = nameof(Xml);
        }
    }
    /// <summary>
    /// Json信息
    /// </summary>
    public sealed class Json : Message
    {
        /// <summary>
        /// Json文本
        /// </summary>
        public string json;
        /// <summary>
        /// Json信息
        /// </summary>
        /// <param name="json">Json文本</param>
        public Json(string json)
        {
            this.json = json;
            this.type = nameof(Json);
        }
    }
    /// <summary>
    /// 应用消息
    /// </summary>
    public sealed class App : Message
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string content;
        /// <summary>
        /// 应用消息
        /// </summary>
        /// <param name="content">内容</param>
        public App(string content)
        {
            this.content = content;
            this.type = nameof(App);
        }
    }
    /// <summary>
    /// 窗口震动(戳一戳)
    /// </summary>
    public sealed class Poke : Message
    {
        /// <summary>
        /// 戳一戳的类型
        /// "Poke": 戳一戳 "ShowLove": 比心 "Like": 点赞 "Heartbroken": 心碎 "SixSixSix": 666 "FangDaZhao": 放大招
        /// </summary>
        public string name;
        /// <summary>
        /// 窗口震动(戳一戳)
        /// </summary>
        /// <param name="name">戳一戳的类型 {"Poke": 戳一戳 "ShowLove": 比心 "Like": 点赞 "Heartbroken": 心碎 "SixSixSix": 666 "FangDaZhao": 放大招}</param>
        public Poke(string name)
        {
            this.name = name;
            this.type = nameof(Poke);
        }
    }
    /// <summary>
    /// 骰子信息
    /// </summary>
    public sealed class Dice : Message
    {
        /// <summary>
        /// 数值
        /// </summary>
        public int value;
        /// <summary>
        /// 骰子信息
        /// </summary>
        /// <param name="value">数值</param>
        public Dice(int value)
        {
            this.value = value;
            this.type = nameof(Dice);
        }
    }
    /// <summary>
    /// 音乐分享信息
    /// </summary>
    public sealed class MusicShare : Message
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string kind;
        /// <summary>
        /// 标题
        /// </summary>
        public string title;
        /// <summary>
        /// 概括
        /// </summary>
        public string summary;
        /// <summary>
        /// 跳转路径
        /// </summary>
        public string jumpUrl;
        /// <summary>
        /// 封面路径
        /// </summary>
        public string pictureUrl;
        /// <summary>
        /// 音乐路径
        /// </summary>
        public string musicUrl;
        /// <summary>
        /// 简介
        /// </summary>
        public string brief;
        /// <summary>
        /// 生成音乐分享
        /// </summary>
        /// <param name="kind">类型</param>
        /// <param name="title">标题</param>
        /// <param name="summary">概括</param>
        /// <param name="jumpUrl">跳转路径</param>
        /// <param name="pictureUrl">封面路径</param>
        /// <param name="musicUrl">音乐路径</param>
        /// <param name="brief">简介</param>
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
    /// <summary>
    /// 转发信息
    /// </summary>
    public sealed class ForwardMessage : Message
    {
        /// <summary>
        /// 转发信息节点
        /// </summary>
        public class Node
        {
            /// <summary>
            /// 发送人QQ号
            /// </summary>
            public long senderId;
            /// <summary>
            /// 时间
            /// </summary>
            public long time;
            /// <summary>
            /// 显示名称
            /// </summary>
            public string senderName;
            /// <summary>
            /// 转发的消息数组
            /// </summary>
            public Message[] messageChain;
            /// <summary>
            /// 源ID
            /// </summary>
            public long sourceId;
            /// <summary>
            /// 转发消息节点
            /// </summary>
            /// <param name="senderId">发送人QQ号</param>
            /// <param name="time">时间</param>
            /// <param name="senderName">显示名称</param>
            /// <param name="messageChain">转发的消息数组</param>
            /// <param name="sourceId">源ID</param>
            public Node(long senderId, long time, string senderName, Message[] messageChain, long sourceId)
            {
                this.senderId = senderId;
                this.time = time;
                this.senderName = senderName;
                this.messageChain = messageChain;
                this.sourceId = sourceId;
            }
        }
        /// <summary>
        /// 消息节点列表
        /// </summary>
        public Node[] nodeList;
        /// <summary>
        /// 转发消息
        /// </summary>
        /// <param name="nodeList">转发的节点</param>
        public ForwardMessage(Node[] nodeList)
        {
            this.nodeList = nodeList;
            this.type = nameof(ForwardMessage);
        }
    }
    /// <summary>
    /// 文件信息
    /// </summary>
    public sealed class File : Message
    {
        /// <summary>
        /// 文件识别ID
        /// </summary>
        public string id;
        /// <summary>
        /// 文件名
        /// </summary>
        public string name;
        /// <summary>
        /// 文件大小
        /// </summary>
        public long size;
        /// <summary>
        /// 文件信息
        /// </summary>
        /// <param name="id">文件识别ID</param>
        /// <param name="name">文件名</param>
        /// <param name="size">文件大小</param>
        public File(string id, string name, long size)
        {
            this.id = id;
            this.name = name;
            this.size = size;
            this.type = nameof(File);
        }
    }
    /// <summary>
    /// Mirai代码
    /// </summary>
    public sealed class MiraiCode : Message
    {
        /// <summary>
        /// Mirai代码
        /// </summary>
        public string code;
        /// <summary>
        /// Mirai码
        /// <para>关于Mirai码的对照: https://github.com/mamoe/mirai/blob/dev/docs/Messages.md</para>
        /// </summary>
        /// <param name="code">代码</param>
        public MiraiCode(string code)
        {
            this.code = code;
        }
    }
}

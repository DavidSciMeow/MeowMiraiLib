/* 本类是关于收发的Json格式对位解析的模块
 * 其内容和 https://github.com/project-mirai/mirai-api-http/blob/master/docs/api/API.md 内的响应内容完全一致
 * 本文件用于储存一切Mirai触发的回送事件,请勿修改内部的包含类或者使用反射修改类内内容,
 * 在逻辑设计上, 原则上本类内文件必须继承ReturnObject以适应类的功能,请勿外部使用接口多继承修改类内行为
 * ----------
 * Generic PostJson Model
 * this file still follow the `response` section of https://github.com/project-mirai/mirai-api-http/blob/master/docs/api/API.md
 * this file is using for Mirai-ReturnValue Handling
 * also don't use reflection of Extension to alter the classes that already sealed
 * in logic applie-ments all class must inhert `ReturnObject` class to infer the class defines
 */

namespace MeowMiraiLib.GenericModel
{

    /// <summary>
    /// Generic Return Object
    /// </summary>
    public class ReturnObject
    {
        /// <summary>
        /// 返回Newtonsoft打印的字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
    /// <summary>
    /// 短成员类
    /// </summary>
    public sealed class QQGroupMemberShort : ReturnObject
    {
        /// <summary>
        /// 短成员QQ号
        /// </summary>
        public long id;
        /// <summary>
        /// 短成员昵称
        /// </summary>
        public string memberName;
        /// <summary>
        /// 短成员权限
        /// </summary>
        public string permission;
        /// <summary>
        /// 引用短成员的群类
        /// </summary>
        public QQGroup group;
        /// <summary>
        /// 生成一个短成员
        /// </summary>
        /// <param name="id"></param>
        /// <param name="memberName"></param>
        /// <param name="permission"></param>
        /// <param name="group"></param>
        public QQGroupMemberShort(long id, string memberName, string permission, QQGroup group)
        {
            this.id = id;
            this.memberName = memberName;
            this.permission = permission;
            this.group = group;
        }
    }
    /// <summary>
    /// QQ好友(列表)
    /// </summary>
    public sealed class QQFriend : ReturnObject
    {
        /// <summary>
        /// QQ号
        /// </summary>
        public long id;
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname;
        /// <summary>
        /// 标记
        /// </summary>
        public string remark;
        /// <summary>
        /// 实例化一个好友
        /// </summary>
        /// <param name="id">QQ号</param>
        /// <param name="nickname">昵称</param>
        /// <param name="remark">备注</param>
        public QQFriend(long id, string nickname, string remark)
        {
            this.id = id;
            this.nickname = nickname;
            this.remark = remark;
        }

        /// <summary>
        /// 朝本好友(非好友)发送一个群消息
        /// </summary>
        /// <param name="c">端</param>
        /// <param name="msg">信息</param>
        /// <returns></returns>
        public MessageId SendMessage(Client c, Msg.Type.Message[] msg) => MessageUtil.SendMessage((this, msg), c);
    }
    /// <summary>
    /// 群(列表)
    /// </summary>
    public sealed class QQGroup : ReturnObject
    {
        /// <summary>
        /// 群号
        /// </summary>
        public long id;
        /// <summary>
        /// 群名
        /// </summary>
        public string name;
        /// <summary>
        /// 群中权限
        /// </summary>
        public string permission;
        /// <summary>
        /// 实例化一个群
        /// </summary>
        /// <param name="id">群号</param>
        /// <param name="name">群名</param>
        /// <param name="permission">Bot群权限</param>
        public QQGroup(long id, string name, string permission)
        {
            this.id = id;
            this.name = name;
            this.permission = permission;
        }

        /// <summary>
        /// 朝本群发送一个群消息
        /// </summary>
        /// <param name="c">端</param>
        /// <param name="msg">信息</param>
        /// <returns></returns>
        public MessageId SendMessage(Client c, Msg.Type.Message[] msg) => MessageUtil.SendMessage((this, msg), c);
    }
    /// <summary>
    /// 群成员(列表)
    /// </summary>
    public sealed class QQGroupMember : ReturnObject
    {
        /// <summary>
        /// 群员QQ号
        /// </summary>
        public long id;
        /// <summary>
        /// 群员名
        /// </summary>
        public string memberName;
        /// <summary>
        /// 群员权限
        /// </summary>
        public string permission;
        /// <summary>
        /// 群头衔
        /// </summary>
        public string specialTitle;
        /// <summary>
        /// 加入时间戳
        /// </summary>
        public long joinTimestamp;
        /// <summary>
        /// 最后一次发言时间戳
        /// </summary>
        public long lastSpeakTimestamp;
        /// <summary>
        /// 禁言剩余时间
        /// </summary>
        public long muteTimeRemaining;
        /// <summary>
        /// QQ群(列表)
        /// </summary>
        public QQGroup group;

        /// <summary>
        /// 生成一个群成员(列表)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="memberName"></param>
        /// <param name="specialTitle"></param>
        /// <param name="permission"></param>
        /// <param name="joinTimestamp"></param>
        /// <param name="lastSpeakTimestamp"></param>
        /// <param name="muteTimeRemaining"></param>
        /// <param name="group"></param>
        public QQGroupMember(long id, string memberName, string specialTitle,
            string permission, long joinTimestamp, long lastSpeakTimestamp,
            long muteTimeRemaining, QQGroup group)
        {
            this.id = id;
            this.memberName = memberName;
            this.specialTitle = specialTitle;
            this.permission = permission;
            this.joinTimestamp = joinTimestamp;
            this.lastSpeakTimestamp = lastSpeakTimestamp;
            this.muteTimeRemaining = muteTimeRemaining;
            this.group = group;
        }
        /// <summary>
        /// 朝本群友(非好友)发送一个群消息
        /// </summary>
        /// <param name="c">端</param>
        /// <param name="msg">信息</param>
        /// <returns></returns>
        public MessageId SendMessage(Client c, Msg.Type.Message[] msg) => MessageUtil.SendMessage((this, msg), c);
    }
    /// <summary>
    /// 获取Bot资料
    /// </summary>
    public sealed class QQProfile : ReturnObject
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname;
        /// <summary>
        /// 电子邮件
        /// </summary>
        public string email;
        /// <summary>
        /// 年龄
        /// </summary>
        public int age;
        /// <summary>
        /// QQ等级
        /// </summary>
        public long level;
        /// <summary>
        /// 签名
        /// </summary>
        public string sign;
        /// <summary>
        /// 性别(UNKNOWN, MALE, FEMALE)
        /// </summary>
        public string sex;
    }
    /// <summary>
    /// 群公告返回类
    /// </summary>
    public sealed class QQAno : ReturnObject
    {
        /// <summary>
        /// 群信息
        /// </summary>
        public QQGroup group;
        /// <summary>
        /// 群公告内容
        /// </summary>
        public string content;
        /// <summary>
        /// 发送者账号
        /// </summary>
        public long senderId;
        /// <summary>
        /// 唯一识别码
        /// </summary>
        public string fid;
        /// <summary>
        /// 是否全部确认
        /// </summary>
        public bool allConfirmed;
        /// <summary>
        /// 已经确认的人数
        /// </summary>
        public long confirmedMembersCount;
        /// <summary>
        /// 发布时间
        /// </summary>
        public long publicationTime;

        /// <summary>
        /// 删除这个公告
        /// </summary>
        /// <param name="c">端</param>
        /// <returns></returns>
        public bool Delete(Client c) => new Msg.Anno_delete(group.id, fid).Send(c) == 0;
    }
    /// <summary>
    /// 群设置返回类
    /// </summary>
    public sealed class QQGConf : ReturnObject
    {
        /// <summary>
        /// 群名
        /// </summary>
        public string name;
        /// <summary>
        /// 群置顶公告
        /// </summary>
        public string announcement;
        /// <summary>
        /// 坦白说开启状态
        /// </summary>
        public bool confessTalk;
        /// <summary>
        /// 是否允许群成员邀请
        /// </summary>
        public bool allowMemberInvite;
        /// <summary>
        /// 是否自动同意入群
        /// </summary>
        public bool autoApprove;
        /// <summary>
        /// 匿名聊天是否开启
        /// </summary>
        public bool anonymousChat;
        /// <summary>
        /// 是否开启全员禁言
        /// </summary>
        public bool muteAll;
    }
}

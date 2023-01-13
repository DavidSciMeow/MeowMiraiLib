
/* 
 * 本类用来定义错误列表的内容, 错误列表可以在 https://github.com/DavidSciMeow/MeowMiraiLib/wiki/AP.-3-%E9%94%99%E8%AF%AF%E5%88%97%E8%A1%A8 查询
 * --------------------------
 * this file is mainly for defines the Error String,
 * for detail please click https://github.com/DavidSciMeow/MeowMiraiLib/wiki/AP.-3-%E9%94%99%E8%AF%AF%E5%88%97%E8%A1%A8
 */

namespace MeowMiraiLib
{
    /// <summary>
    /// 错误列表
    /// </summary>
    public static class ErrorDefine
    {
        /// <summary>
        /// 没有实例化URL(为空)
        /// </summary>
        public const string E0001 = "[0001] InitPhase : No Url Specific.";
        /// <summary>
        /// 信息不符合标准要求
        /// </summary>
        public const string E0012 = "[0012] ParserPhase : Message Typo Error in ";
        /// <summary>
        /// 没有符合的信息类型
        /// </summary>
        public const string E0013 = "[0013] ParserPhase : Message Error in ";
        /// <summary>
        /// 没有最佳的发送选项
        /// </summary>
        public const string E1000 = "[1000] ContextualSender::SendMsgBack Func : SenderId and GroupId isn't Fit.";
        /// <summary>
        /// 上下文消息段数字位置超过队列长度
        /// </summary>
        public const string E1010 = "[1010] ContextualSender::GetPlainMsgAt : Number set is over or less than queue length.";
        /// <summary>
        /// 全局发送端未定义
        /// </summary>
        public const string E2000 = "[2000] GlobalErr : Global ConClient NotSet.";
    }
}

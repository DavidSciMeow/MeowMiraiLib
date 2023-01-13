namespace MeowMiraiLib
{
    /// <summary>
    /// 可能的错误类
    /// </summary>
    public static class ErrorDefine
    {
        public const string E0001 = "[0001] InitPhase : No Url Specific.";
        public const string E0012 = "[0012] ParserPhase : Message Typo Error in ";
        public const string E0013 = "[0013] ParserPhase : Message Error in ";
        public const string E1000 = "[1000] ContextualSender::SendMsgBack Func : SenderId and GroupId isn't Fit.";
        public const string E1010 = "[1010] ContextualSender::GetPlainMsgAt : Number set is over or less than queue length.";
        public const string E2000 = "[2000] GlobalErr : Global ConClient NotSet.";
    }
}

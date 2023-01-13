using System;

/* 
 * 版本 8.1.0+ 上下文接收端主要屏蔽类(事件)文件, 
 * 包括父类的某些不可使用方法, 使用这些方法会对程序的运行逻辑造成破坏,
 * 本类使用继承新创方法标注了错误, 使得方法不可使用.
 * -------------------------------------------
 * Version 8.1.0+ Contextual-Client Isolate-Class(event) file
 * This file contain functions which are not going to public to user(SE), but already contained in Base-class,
 * enable these event can do damage to the program runtime logic.
 * by using a method called Inherited+New, Now we disable these method entirely.
 */

namespace MeowMiraiLib.MultiContext
{
    public partial class ConClient : Client
    {
        /*--Type of Message need to relocated--*/
        /// <summary>
        /// 接收到好友私聊信息
        /// </summary>
        [Obsolete("this method is replace by _OnMessageRecieve", true)]
        public new event FriendMessage OnFriendMessageReceive;
        /// <summary>
        /// 接收到群聊信息
        /// </summary>
        [Obsolete("this method is replace by _OnMessageRecieve", true)]
        public new event GroupMessage OnGroupMessageReceive;
        /// <summary>
        /// 接收到临时信息
        /// </summary>
        [Obsolete("this method is replace by _OnMessageRecieve", true)]
        public new event TempMessage OnTempMessageReceive;
        /// <summary>
        /// 接收到陌生人信息
        /// </summary>
        [Obsolete("this method is replace by _OnMessageRecieve", true)]
        public new event StrangerMessage OnStrangerMessageReceive;
    }
}

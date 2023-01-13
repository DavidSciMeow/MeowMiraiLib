/*本类定义了基类逻辑和新基类操作用法, 实现了一些扩展的基类应用, 本类定义一般为最高类, 使用结构体定义数据集合
 * -----
 * this file is ussd for Base-Arch Logic and Base-Operation usage,
 * which is extension for other meta-type,
 * this struct(s) will be define in the up-most classes(namespace),
 * also mostly will be `struct-class`
 */

namespace MeowMiraiLib
{
    /// <summary>
    /// 信息返回Id
    /// </summary>
    public struct MessageId
    {
        /// <summary>
        /// 信息返回id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="c"></param>
        public MessageId(long id, Client c)
        {
            Id = id;
            C = c;
        }
        /// <summary>
        /// 信息Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 信息对应的端
        /// </summary>
        public Client C { get; set; }
        /// <summary>
        /// 撤回这条信息
        /// </summary>
        /// <returns></returns>
        public bool ReCall() => new Msg.Recall(Id).Send(C) == 0;
        /// <summary>
        /// 延时 x 秒撤回消息
        /// </summary>
        /// <param name="second">秒钟数</param>
        /// <returns></returns>
        public bool ReCall(int second)
        {
            System.Threading.Tasks.Task.Delay(1000 * second).GetAwaiter().GetResult();
            return ReCall();
        }
        /// <summary>
        /// 打印MsgId
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"MsgId:{Id}";
    }
}

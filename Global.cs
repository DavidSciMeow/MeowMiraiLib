using System;
using System.Reflection;

/* 
 * 版本 8.1.0+ 全局设置, 
 * 包括单端生成(快速发送接收), 全局日志设置等, 
 * 本页面的编写原则是全局设置, 如果进行更新或者更改请使用VS Ctrl+R命名方案, 防止更改后相关联函数无法获取参数
 * 本类置顶于MML内部类, 并且本类不应含有任何内部调用参数类, 也不应含有任何帮助类.
 * 更改前请确认全局均需要使用.
 * -------------------------------------------
 * Version 8.1.0+ Global Setting Class File
 * this file intergated Client Interpolation/Initiation (for quick Send/Recieve), Global Logger Setting, etc.
 * when editing this page you'll need to confirm the function need to run as 'GLOBAL', 
 * as if you're unsatisfact with the name I use, Please using VS-Func Ctrl+R Naming rules for preventing the function-name related errors
 * this class is topped as innerclass of MML(MeowMiraiLib) and should not contain class like func-generate-class or helper-class
 * AS SURE PLEASE CONFIRM FUNCTION YOU ADD IS GLOBALLY USED.
 */


namespace MeowMiraiLib
{
    /// <summary>
    /// 通用设置
    /// </summary>
    public static class Global
    {
        /// <summary>
        /// 是否输出全局日志
        /// <br/>
        /// 4:debug [3:info] 2:warn 1:error 
        /// </summary>
        public static int G_Debug_Flag = 3;
        /// <summary>
        /// 全局端
        /// </summary>
        public static Client G_Client = null;

        /// <summary>
        /// 日志处理模块
        /// </summary>
        public static class Log
        {
            /// <summary>
            /// 全局日志(总输出)
            /// </summary>
            /// <param name="s">字符串</param>
            /// <param name="intsenties">类型强度</param>
            /// <param name="f">前景色</param>
            /// <param name="b">背景色</param>
            public static void GlobalLog(string s, int intsenties, ConsoleColor f = default, ConsoleColor b = default)
            {
                if (G_Debug_Flag > intsenties)
                {
                    Console.ForegroundColor = f;
                    Console.BackgroundColor = b;
                    Console.WriteLine($"[MML {Assembly.GetExecutingAssembly().GetName().Version}] [{DateTime.Now : yyyy-MM-dd HH:mm:ss}] \n$ {s}");
                    Console.ForegroundColor = default;
                    Console.BackgroundColor = default;
                }
            }
            /// <summary>
            /// Debug 级别
            /// </summary>
            /// <param name="s">字符</param>
            public static void Debug(string s) => GlobalLog(s, 3, ConsoleColor.White);
            /// <summary>
            /// Info 级别
            /// </summary>
            /// <param name="s">字符</param>
            public static void Info(string s) => GlobalLog(s,2, ConsoleColor.Blue);
            /// <summary>
            /// Warn 级别
            /// </summary>
            /// <param name="s">字符</param>
            public static void Warn(string s) => GlobalLog(s,1, ConsoleColor.Yellow);
            /// <summary>
            /// Error 级别
            /// </summary>
            /// <param name="s">字符</param>
            public static void Error(string s) => GlobalLog(s,0, ConsoleColor.Red);
           
        }
        
    }
}

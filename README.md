# Meow.MiraiLib  
## 一个简易使用的轻量化 Mirai-C# (.net 5) 后端
## 简易使用方法(内部注释完整)  

1. ##异步写法
```
using MeowMiraiLib;
using MeowMiraiLib.Msg;
using System;
using System.Threading.Tasks;

namespace Test
{
    internal class Program
    {
        static readonly Client c = new("....",true,true);
        static async Task Main(string[] args)
        {
            await c.ConnectAsync();
            c.OnFriendMessageRecieve += (s, e) => 
            { 
                //.....
            };
            //....
            while (true)
            {
                switch (Console.ReadLine())
                {
                    case "t":
                        {
                            var k = await new GroupList().SendAsync(c); //查看群列表
                            Console.WriteLine(k); // 打印返回信息
                            break;
                        }
                }
            }
        }
    }
}
```
2. ##同步写法
```
using MeowMiraiLib;
using MeowMiraiLib.Msg;
using System;

namespace Test
{
    internal class Program
    {
        static readonly Client c = new("........",true,true);
        static void Main(string[] args)
        {
            c.Connect();
            c.OnFriendMessageRecieve += (s, e) => 
            { 
                //.....
            };
            //....
            while (true)
            {
                switch (Console.ReadLine())
                {
                    case "t":
                        {
                            var k = new GroupList().Send(c); //查看群列表
                            Console.WriteLine(k); // 打印返回信息
                            break;
                        }
                }
            }
        }
    }
}
```

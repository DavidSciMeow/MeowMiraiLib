# Meow.MiraiLib

一个简易使用的轻量化 Mirai-C# (.net 5) 后端

## 使用示例

1.异步

```csharp
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
            c.OnFriendMessageReceive += (s, e) => 
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

2.同步

```csharp
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
            c.OnFriendMessageReceive += (s, e) => 
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

其他功能详见源码注释或者右侧 GitPage/帮助

## 最新版本&特性

3.0.0  加装了异步处理的标准流程, 优化了事件处理, 独立(实例了)端和信息发送的方案.

## 鸣谢

感谢大佬 [@Executor-Cheng](https://github.com/Executor-Cheng) 的初版建议和意见.
感谢各位大佬对小项目的关注.

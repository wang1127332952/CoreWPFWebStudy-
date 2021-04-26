using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConsoleNetStudy.ThreadTask
{
    /// <summary>
    /// 线程池  .NetFramework 2.0（新的CLR）ThreadPool
    /// 池化资源管理设计思想，线程是一种资源，之前每次要用线程，就去申请一个线程
    /// 使用完之后释放掉；池化是一个容器，容器提前申请五个线程，程序使用就去线程池获取
    /// 用完后再放回容器（控制状态),避免啊频繁的申请和销毁；容器自己会根据限制数量
    /// 去申请和释放
    /// 
    /// 优点：1.线程复用 2.限制最大线程数量
    /// 缺点：1.API太少 2.线程等待，顺序控制特别弱 影响实战
    /// </summary>
    public class TreadPoolStudy
    {
        public void TreadPool()
        {
            WaitCallback waitCallback = o =>
            {
                Console.WriteLine($"this is threadpool start.{JsonConvert.SerializeObject(o)},{Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(2000);
                Console.WriteLine($"this is threadpool end.{JsonConvert.SerializeObject(o)},{Thread.CurrentThread.ManagedThreadId}");
            };
            ThreadPool.QueueUserWorkItem(waitCallback);

        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConsoleNetStudy.ThreadTask
{
    /// <summary>
    /// Thread .net FrameWork 1.0 1.1
    /// API很丰富 玩法很多 但是线程资源是操作系统管理的 响应并不灵敏
    /// Thread启动线程是没有控制的，可能导致死机
    /// </summary>
    public class TreadStudy
    {
        public void ThreadFunc() 
        {
            //线程入口是通过ThreadStart代理（delegate）来提供的，你可以把ThreadStart理解为一个函数指针，指向线程要执行的函数，当调用C#
            ThreadStart thread = ()=> 
            {
                Console.WriteLine($"This is a thread start.{Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(2000);
                Console.WriteLine($"This is a thread end.{Thread.CurrentThread.ManagedThreadId}");
            };

            Thread thread1 = new Thread(thread);
            Thread thread2 = new Thread(new ThreadStart(ThreadTest));

            thread1.Start();
            thread1.Join();//等待
            bool success= thread1.IsAlive;
            thread2.Start();
        }

        /// <summary>
        /// ThreadTest
        /// </summary>
        private void ThreadTest() 
        {
            Console.WriteLine($"This is thread function start.{Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(2000);
            Console.WriteLine($"This is thread function end.{Thread.CurrentThread.ManagedThreadId}");
        }

    }
}

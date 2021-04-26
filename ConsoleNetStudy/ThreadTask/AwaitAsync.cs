using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleNetStudy.ThreadTask
{
    /// <summary>
    /// await/async ：新语法 出现在C# 5.0 .netFramework 4.5  CLR4.0
    /// 是一个语法糖 不是一个新的异步多线程使用方式（语法糖）：编辑器提供的新功能
    /// 本身不会产生新的线程。但是依托于Task而存在所以执行时，也会有多线程
    /// 
    /// 原本没返回值的 可以返回Task
    /// 原本返回T的 可以返回Task<T>
    /// 一般来说 尽量不要返回void 因为不能返回Task
    /// 线程id  返回值 等待问题
    /// 
    /// 为了并发 为了不阻塞线程 会用异步多线程
    /// 但是执行顺序又很奇怪经常需要等待顺序完成
    /// await/async 就等同于用同步的方式 完成了异步
    ///
    /// 状态机+回调 就支持无限 层级await
    /// 
    /// yield
    /// 
    /// IL 里面没有await/async 
    /// 首先实例化一个状态是-1，然后再执行task前面的 启动线程
    /// 去执行task 判断完成 没有就把状态置为0 
    /// 然后让线程递归调用 自己回去 接续执行自己的使命
    /// </summary>
    public class AwaitAsync
    {
        public void Show() 
        {
            Console.WriteLine($"MAIN START.{Thread.CurrentThread.ManagedThreadId}");
            //this.NoReturn();
            //this.NoReturnAsync();
            //this.ReturnAsync();
            long v = this.ReturnLongAsync(1).GetAwaiter().GetResult();
            this.ReturnLongAsync(2);
            Console.WriteLine($"MAIN END.{Thread.CurrentThread.ManagedThreadId}");

        }

        private void NoReturn()
        {
            Console.WriteLine($"NoReturn START.{Thread.CurrentThread.ManagedThreadId}");
            Task.Run(() =>
            {
                Console.WriteLine($"NoReturn TASK START.{Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(2000);
                Console.WriteLine($"NoReturn TASK END.{Thread.CurrentThread.ManagedThreadId}");

            });
            Console.WriteLine($"NoReturn END.{Thread.CurrentThread.ManagedThreadId}");
        }

        /// <summary>
        /// async 可以随意添加
        /// await 必须出现在 Task前面
        /// </summary>
        private void NoReturnAsync()
        {
            Console.WriteLine($"NoReturn START.{Thread.CurrentThread.ManagedThreadId}");
            Task task = Task.Run(() =>
            {
                Console.WriteLine($"NoReturn TASK START.{Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(2000);
                Console.WriteLine($"NoReturn TASK END.{Thread.CurrentThread.ManagedThreadId}");
            });
            //await task;
            Console.WriteLine($"NoReturn END.{Thread.CurrentThread.ManagedThreadId}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task ReturnAsync()
        {
            Console.WriteLine($"ReturnAsync START.{Thread.CurrentThread.ManagedThreadId}");
            Task task = Task.Run(() =>
            {
                Console.WriteLine($"ReturnAsync TASK START.{Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(2000);
                Console.WriteLine($"ReturnAsync TASK END.{Thread.CurrentThread.ManagedThreadId}");
            });//调用线程发起会发起新线程执行内部动作
            await task;//调用线程回去忙自己的事
            ///可以认为 加了await 就等同于将await后面的代码包装一个回调  其实回调具有多种可能性
            ///同步形式的编码去写异步
           // //当task完成后执行回调
           // task.ContinueWith(t =>
           // {
           //     Console.WriteLine($"ReturnAsync 回调.{Thread.CurrentThread.ManagedThreadId}"); ;
           // });
            
            Console.WriteLine($"ReturnAsync END.{Thread.CurrentThread.ManagedThreadId}");
            //Task的子线程完成 如果没有await 那么调用线程完成
        }

        private async Task<long> ReturnLongAsync(int i)
        {
            long result = 0;
            Console.WriteLine($"ReturnLongAsync i:{i} START.{Thread.CurrentThread.ManagedThreadId}");
            Task<long> task = Task.Run(() =>
            {
                Console.WriteLine($"ReturnLongAsync i:{i} TASK START.{Thread.CurrentThread.ManagedThreadId}");
                
                for (int i = 0; i < 1000000; i++) 
                {
                    result += i;
                }
                Console.WriteLine($"ReturnLongAsync i:{i} TASK END.{Thread.CurrentThread.ManagedThreadId}");
                return result;
            });
            await task;
            Console.WriteLine($"ReturnAsync i:{i} END,{result}.{Thread.CurrentThread.ManagedThreadId}");
            return result;
        }
    }
}

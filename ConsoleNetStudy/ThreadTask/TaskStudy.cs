using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleNetStudy.ThreadTask
{
    /// <summary>
    /// 多线程 Task学习 3.0 出现
    /// Task被称之为多线程的最佳实践 
    /// 1.Task线程全部是线程池的线程 2. API非常丰富 非常适合开发
    /// 
    /// 任务并行库 TPL
    /// 任务并行库 (TPL) 支持通过 System.Threading.Tasks.Parallel 类实现的数据并行。 
    /// 此类对 for 循环和 foreach 循环（Visual Basic 中的 For 和 For Each）提供了基于方法的并行执行
    /// </summary>
    public class TaskStudy
    {
        #region Parallel
        public void Study()
        {
            Action action = () =>
            {
                Console.WriteLine($"Task action,{Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(2000);
                Console.WriteLine($"Task action,{Thread.CurrentThread.ManagedThreadId}");

            };
            Task.Run(action);
            Task task = new Task(action);
            task.Start();

            {
                /// Parallel可以启动多线程，主线程也参与计算
                /// ParallelOptions 轻松控制 最大并发量
                Parallel.Invoke(
                () =>
                {
                    Console.WriteLine($"Task Parallel1 start,{Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(2000);
                    Console.WriteLine($"Task Parallel1 end,{Thread.CurrentThread.ManagedThreadId}");
                },
                () =>
                {
                    Console.WriteLine($"Task Parallel2 start,{Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(2000);
                    Console.WriteLine($"Task Parallel2 end,{Thread.CurrentThread.ManagedThreadId}");
                },
                () =>
                {
                    Console.WriteLine($"Task Parallel3 start,{Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(2000);
                    Console.WriteLine($"Task Parallel3 end,{Thread.CurrentThread.ManagedThreadId}");
                });
            }
        }

        public void ParallelStudy() 
        {
            long totalSize = 0;
            String[] args = Environment.GetCommandLineArgs();
            if (args.Length == 1)
            {
                Console.WriteLine("There are no command line arguments.");
                return;
            }
            if (!Directory.Exists(args[1]))
            {
                Console.WriteLine("The directory does not exist.");
                return;
            }

            String[] files = Directory.GetFiles(args[1]);
            Parallel.For(0, files.Length,
                         index => {
                             FileInfo fi = new FileInfo(files[index]);
                             long size = fi.Length;
                             Interlocked.Add(ref totalSize, size);
                         });
            Console.WriteLine("Directory '{0}':", args[1]);
            Console.WriteLine("{0:N0} files, {1:N0} bytes", files.Length, totalSize);
            // Sequential version            
            //foreach (var item in sourceCollection)
            //{
            //    Process(item);
            //}
            //// Parallel equivalent
            //Parallel.ForEach(sourceCollection, item => Process(item));
        }
        #endregion

        #region Task解读
        /// <summary>
        /// Task 专题解析
        /// 不推荐:1.不要线程套线程 其实有更优秀的方法 
        /// 2.这里全部是子线程完成的,不能直接操作页面
        /// </summary>
        public void DoTaskStudy()
        {
           // Task.Run(() => Transformation("wht", "AspNetCore"));
            List<Task> tasks = new List<Task>();
            ///一个数据库查询需要10s 不能用多线程优化(不可拆分任务)
            /// 不能用多线程的条件 ：时间顺序 不可分割
            tasks.Add(Task.Run(() => Transformation("XXL", "AspNetCore")));
            tasks.Add(Task.Run(() => Transformation("WJ", "AspNetCore")));
            tasks.Add(Task.Run(() => Transformation("LDP", "AspNetCore")));
            tasks.Add(Task.Run(() => Transformation("LDP1", "AspNetCore")));
            tasks.Add(Task.Run(() => Transformation("LDP2", "AspNetCore")));
            tasks.Add(Task.Run(() => Transformation("LDP3", "AspNetCore")));
            tasks.Add(Task.Run(() => Transformation("LDP4", "AspNetCore")));
     
            //既需要用多线程提升性能，又需要在多线程全部做完后才能执行
            //会卡界面
            {
                //阻塞当前线程，直到任意一个任务结束
                Task.WaitAny(tasks.ToArray());
                Console.WriteLine($"其中一个做完,{Thread.CurrentThread.ManagedThreadId}");

                //阻塞当前线程，直到全部任务结束
                Task.WaitAll(tasks.ToArray());
                Console.WriteLine($"全部已经做完,{Thread.CurrentThread.ManagedThreadId}");
            }

            //不阻塞
            {
                TaskFactory taskFactory = new TaskFactory();
                //等着任意一个任务完成后启动一个新的task来完成后续动作
                taskFactory.ContinueWhenAny(tasks.ToArray(), tArray =>
                {
                    Console.WriteLine($"任何一个做完后就开始做.{Thread.CurrentThread.ManagedThreadId}");
                });

                //等着全部任务完成后启动一个新的task来完成后续动作
                taskFactory.ContinueWhenAll(tasks.ToArray(), tArray =>
                {
                    Console.WriteLine($"全部做完后才开始做,{JsonConvert.SerializeObject(tArray)},{Thread.CurrentThread.ManagedThreadId}");
                });

                //可以保证顺序
                tasks.Add(taskFactory.ContinueWhenAll(tasks.ToArray(), tArray =>
                {
                    Console.WriteLine($"上面的任务做完,才会做当前委托方法,{Thread.CurrentThread.ManagedThreadId}");
                }));
            }

        }

        /// <summary>
        ///  提升自己
        /// </summary>
        /// <param name="peopleName">学员名字</param>
        /// <param name="subjectName">学科</param>
        private void Transformation(string peopleName, string subjectName)
        {
            //一年刻苦学习
            //for (int i = 0; i < 1000; i++) 
            //{
            //    Console.WriteLine($"this is value.{i},{Thread.CurrentThread.ManagedThreadId}");
            //}
            Thread.Sleep(1000);
            Console.WriteLine($"{peopleName}--{subjectName},{Thread.CurrentThread.ManagedThreadId}");
        }

        #endregion

        #region 多线程安全学习
        //锁的应用和扩展
        /// <summary>
        /// 
        /// </summary>
        private void AddData()
        {
            for (int i = 0; i < 6; i++)
            {
                int k = i;
                Task.Run(() =>
                {
                    Console.WriteLine($"Task start i:{i},k:{k},{Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(2000);
                    Console.WriteLine($"Task end i:{i},k:{k},{Thread.CurrentThread.ManagedThreadId}");
                });
            }
        }

        //多线程去访问同一个集合一般没问题，线程问题一般都出现在修改一个对象的过程中

        /// <summary>
        /// 更新数组
        /// </summary>
        public void UpdateArray()
        {
            List<int> vs = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                //多线程之后数据小于10000
                //List是数组,在内存上连续摆放，同一时刻去增加一个数组，都是操作内存同一个位置
                //两个cpu 同时发送指令，内存先执行一个，在执行一个，就会出现覆盖
                int k = i; //不然值都是一样的
                Task.Run(() =>
                {
                    //Monitor.Enter(locks);
                    lock (locks)
                    {
                        vs.Add(k);
                    }
                    //Monitor.Exit(locks);
                });
            }
            Thread.Sleep(5000);
            Console.WriteLine($"this is Count.{vs.Count}");
            foreach (int i in vs)
            {
                Console.WriteLine($"this is value.{i}");
            }
            ///线程安全定义：一段代码 单线程执行和多线程执行结果不一致就说明有线程安全问题
            ///解决线程安全问题
            ///锁 加锁可以解决安全 --单线程化  lock 保证 方法快任意时刻只有一个线程在执行
            ///lock 语法糖 等价于Monitor 首先锁定一个（内存）引用地址--不能锁定 值类型 也不能是null
            ///null 是一个占据引用，需要一个引用
            ///lock 相关
            ///公用锁就是出现相互阻塞
            ///锁不同的变量，才能并发

            {
                ///锁定的是内存引用--字符串享元 堆栈上只有一个
                lock (lock_string)
                {

                }
            }

            {
                lock (this) ///this 当前实例
                {

                }

                lock(this)
                {
                    ///递归 不会死锁
                }
            }
        }
        private static readonly object locks = new object();
        private readonly string lock_string = "静待花开";
        ///泛型类 在类型参数相同时 是同一个类
        #endregion



    }

}

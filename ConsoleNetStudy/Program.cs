using ConsoleNetStudy.EventDemo;
using ConsoleNetStudy.ThreadTask;
using DesignPatternStudy.DesignPattern;
using Polly;
using Polly.Timeout;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleNetStudy
{
    class Program
    {
        static void Main(string[] args)
        {
            var t1 = Task.Run(()=>
            {
                int i=1, j = 1;
                var a = i + j;
                Console.WriteLine(a);
            });


            ///Thread学习
            //TreadStudy treadStudy = new TreadStudy();
            // treadStudy.ThreadFunc();

            //ThreadPool学习
            // TreadPoolStudy treadPoolStudy = new TreadPoolStudy();
            // treadPoolStudy.TreadPool();

            //TaskStudy taskStudy = new TaskStudy();
            //taskStudy.DoTaskStudy();
            //taskStudy.UpdateArray();

            //AwaitAsync awaitAsync = new AwaitAsync();
            //awaitAsync.Show();


            #region 设计模式启动
            ////单例模式 全局唯一 对象可以复用 但是会出现覆盖
            //PrototypeStudy prototypeStudy = PrototypeStudy.CreateInstance();
            //PrototypeStudy prototypeStudy1 = PrototypeStudy.CreateInstance();

            //prototypeStudy1.Id = "2";
            //prototypeStudy1.Name = "wht";
            ////前面的会被后面的值覆盖
            //prototypeStudy.Study(prototypeStudy);
            //prototypeStudy1.Study(prototypeStudy1);

            #endregion
            {
                ///C# 内存分配 进程堆（进程唯一）线程栈（每一个线程一个）
                ///引用类型在堆里，值类型在栈里--变量都在栈里
                ///引用类型对象里的值类型在堆里面
                ///值类型里面的引用类型 ----在堆里（任何引用类型的值都在堆里）
                ///
            }
            // object value = 23.6;
            //if (double.TryParse(value.ToString(), out var result))
            //{
            //    if ((0 <= result && result < 4) || (90 < result && result <= 100))
            //    {

            //    }
            //    else if (result < 0 || result > 100)
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            //else
            //{

            //}


            //{
            //    var task = new PollyStudy();
            //    string a = Console.ReadLine();
            //    if (a == "111")
            //    {
            //        task.TaskRun();
            //    }
            //    string b = "222";
            //    if (b == "222")
            //    {
            //        task.StopTaskRun();
            //    }
            //}

            //{
            //    try
            //    {
            //        int i = 0;
            //        var timeoutPolicy = Policy.TimeoutAsync(1, TimeoutStrategy.Pessimistic);//超时30秒重试
            //        timeoutPolicy.ExecuteAsync(() => Task.Run(() =>
            //        {
            //            Thread.Sleep(1);
            //            throw new Exception("dddd");
            //            Console.WriteLine(i++);
            //        })).ConfigureAwait(true).GetAwaiter();
            //    }
            //    catch (Exception ex) 
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            //}

            //{
            //    PollyStudy pollyStudy = new PollyStudy();
            //    pollyStudy.Pollys();
            //}

            Console.ReadLine();
        }
    }
}

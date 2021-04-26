using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleNetStudy.EventDemo
{
    /// <summary>
    /// Func<ΤResult> 委托：代理(delegate)一个返回类型为「由参数指定的类型的值(TResul)」 
    /// 的无参方法。使用 Func<ΤResult> 委托，无需显式定义一个委托与方法的关联。
    /// Action或Action委托（不带参数或带一个object参数）
    /// c# 委托（Func、Action）
    /// 1、Func 用法 (封装方法，传入参数， 有返回值）
    /// Func<in T1, in T2, ..., out TResult> (T1, T2, ...) 封装一个方法，该方法有 （0 /1/2/3  ... 16）个参数，且返回由 TResult 参数指定的值的类型。
    /// 2、Action 用法 （封装一个方法， 传入参数， 无返回值）
    /// Action<T1, T2, T3, ...>(t1, t2, t3...)封装一个方法， 该方法传入 （0/1/2 ...） 个参数， 且不返回值。
    /// </summary>
    public class FuncActionTask
    {
        public void DoWay() 
        {
            Func<bool> methodCall = () => GetTask();
            Func<Task> method1 = () => GetTask1();
            // 方法一： Func 相当于系统内置的 委托
            Func<int, int, string> method = Calculate;
            // 方法二： 调用 Lambda 方法实现， 更简洁
            Func<int, int, string> method_1 = (x, y) =>
            {
                int val = x + y;
                return string.Format("the calculate {0} plus {1} result is: {2}", x, y, val);
            };
            Console.WriteLine(method(3, 5));
            Console.WriteLine(method_1(10, 18));

            Method_First("Hi, Here!");
            Method_Sec("Hi, There!");

            Console.ReadLine();
        }
        public string Calculate(int x, int y)
        {
            int val = x + y;
            return string.Format("the calculate {0} plus {1} result is: {2}", x, y, val);
        }

        private void Method_First(string y)
        {
            Action<string> method;
            method = x => { Console.WriteLine("the input message is: {0}", x); };
            method(y);
        }

        private void Method_Sec(string y)
        {
            Action<string> method = x => { Console.WriteLine("the input message is : {0}", x); };
            method(y);
        }

        public Task GetTask1()
        {
            return Task.Run(() =>
            {
                int i = 5;
                int j = 6;
                return (i + j);
            });
        }

        public bool GetTask()
        {
            int a = 1;
            int i = 1;
            if (a == i)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

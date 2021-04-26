using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DesignPatternStudy.DesignPattern
{
    /// <summary>
    /// 原型设计模式 ---单例
    /// </summary>
    public class PrototypeStudy
    {
        public string Id { get; set; }

        public string Name { get; set; }

        ///单例模式：保证整个进程中只有一个实例
        /// 1. 构造函数私有化---避免随意构造
        private PrototypeStudy() 
        {
            //Thread.Sleep(2000);
            //long lResult = 0;
            //for (int i = 0; i < 100000; i++)
            //{
            //    lResult += i;
            //}
            Console.WriteLine($"{this.GetType().Name}被构造");

        }

        /// 2.通过公开的静态方法来提供实例
        public static PrototypeStudy CreateInstance()
        {
            //单例
            //return _prototypeStudy;
            
            //原型

            PrototypeStudy prototypeStudy =(PrototypeStudy)_prototypeStudy.MemberwiseClone();
            //MemberwiseClone 内存拷贝 不走构造函数 直接内存拷贝,没有性能损失 产生新对象
            //浅拷贝 只拷贝引用
            //string 类型的 ="wht" 等同于 new String("wht"); 开辟新空间  实际上string不能修改
           //深拷贝
           //1.直接 new  2.子类型提供原型模式 3.序列化/反序列化 
            return prototypeStudy;
        }

        /// 3.私有的静态字段（内存唯一 不会释放，且在第一次使用时初始化 且初始化一次）
        private static PrototypeStudy _prototypeStudy = new PrototypeStudy()
        {
            Id="23",
            Name="ywa",
        };


        public void Study(PrototypeStudy prototypeStudy) 
        {
            Console.WriteLine($"{prototypeStudy.Name} Study-{prototypeStudy.Id},threadId:{Thread.CurrentThread.ManagedThreadId}");
        }
        
    }
}

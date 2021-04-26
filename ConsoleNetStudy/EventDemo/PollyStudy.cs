using Polly;
using System;
using Polly.Retry;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using Polly.CircuitBreaker;
using Polly.Timeout;

namespace ConsoleNetStudy.EventDemo
{
    /// <summary>
    /// Polly学习
    /// 1.定义故障
    /// 
    /// </summary>
    public class PollyStudy
    {
        public void Pollys() 
        {
            ////三步
            ////1.定义故障 当发生了ArgumentException异常就会触发策略 (Handle)
            ////2.指定策略(Fallback)
            ////3.执行策略
            //Policy.Handle<ArgumentException>()
            //    .Fallback(() =>
            //    {
            //        Console.WriteLine("Polly Fallback!");
            //    }).Execute(() =>
            //    {
            //        //业务跨服务调用可以和HttpClient结合使用
            //        Console.WriteLine("DoSomething");
            //        throw new ArgumentException("hello Polly");
            //    });

            ////单个异常故障
            //Policy.Handle<Exception>();
            ////带条件的异常类型
            //Policy.Handle<Exception>(ex => ex.Message == "hello");
            ////多个类型异常
            //Policy.Handle<HttpRequestException>()
            //    .Or<AggregateException>()
            //    .Or<ArgumentException>();
            ////Polly故障处理库，有些异常需要处理 有些不需要处理 服务A访问服务B(请求出错)
            ////业务代码出问题（try 记录日志）

            ////弹性策略 响应式策略 重试 断路器
            ////不传参数 默认1次
            //Policy.Handle<Exception>().Retry();
            ////重试多次
            //Policy.Handle<Exception>().Retry(3, onRetry: (e, i) => { });
            ////一直重试，直到成功,非高并发 适合业务场景
            //Policy.Handle<Exception>().RetryForever();
            ////重试且等待
            //Policy.Handle<Exception>().WaitAndRetry(sleepDurations: new[]
            //{
            //    TimeSpan.FromSeconds(1),//第一次重试
            //    TimeSpan.FromSeconds(2),//第二次重试
            //    TimeSpan.FromSeconds(3),//第三次重试
            //});

            //Policy.Handle<Exception>().WaitAndRetry(3, i => TimeSpan.FromSeconds(Math.Pow(2, i)));
            ////每一个策略 最后一个参数都是一个委托，这个委托就是异常委托


            ////断路器 非常实用的策略，也是必备的
            ////连续两次指定（2）次数的故障后，就开启断路器，进入熔断状态1分钟
            //Policy.Handle<Exception>()
            //    .CircuitBreaker(2, TimeSpan.FromMinutes(1));
            ////断路器有三种状态 OPEN CLOSE HALF-OPEN
            //var barrier = Policy.Handle<Exception>()
            //   .CircuitBreaker(2, TimeSpan.FromMinutes(1),
            //   onBreak: (e, s) => { }, onReset: () => { });

            ////这个不是断路器模式的状态，这是Polly断路器策略里面的一种特殊状态
            ////手动的打开断路器 断路器：手动开启状态
            ////barrier.CircuitState== CircuitState.Isolated
            ////手动开启断路器 默认关闭
            //barrier.Isolate();
            ////开启断路器
            //barrier.Reset();

            ////高级断路器
            ////如果在故障采样持续时间内，发生故障的比例超过故障阈值则发生熔断
            ////前提是在此期间，通过断路器的操作的数量至少是最小吞吐量
            //Policy.Handle<Exception>()
            //    .AdvancedCircuitBreaker(0.5, //故障阈值 50%
            //    TimeSpan.FromSeconds(10), //故障采样时间 10秒
            //    8,                        //最小吞吐量 10秒内最少执行了8次操作
            //    TimeSpan.FromSeconds(30));//熔断时间 30秒
            ////half-open 半开启状态，断路器会尝试着释放（1次操作）尝试请求
            ////如果成功 就变成close 如果失败断路器打开（30秒）

            ////超时 服务调用,它没有挂就是慢! 慢本身就是一种故障 定义2秒
            ////超时本身就是异常
            //Policy.Timeout(3, (context, span, arg3, arg4) => { }).Execute(() => { });

            ////舱壁隔离 通过控制并发数量来管理负载 超过12的都被拒绝掉
            //Policy.Bulkhead(12,//请求次数
            //    100//等待次数
            //    ).Execute(() => { });

            //////缓存比较复杂 依赖其他库 集合redis
            ////Policy.Cache();
            //////回退策略

            //////策略包装 策略组合

            //////降级策略
            //var fallback = Policy.Handle<Exception>()
            //   .Fallback(() =>
            //   {
            //       Console.WriteLine("Polly Fallback!");
            //   });
            ////重试策略
            //var retry = Policy.Handle<Exception>().Retry(3, onRetry: (e, i) =>
            //{
            //     Console.WriteLine($"{i}");
            //});

            ////如果重试3次 仍然发生故障 就降级 (从右往左)
            //var policy = Policy.Wrap(fallback, retry);
            //policy.Execute(() =>
            //{
            //    Console.WriteLine("Policy begin");
            //    throw new Exception("Error");
            //});
            ////Policy里面所有的策略，除了缓存 其他都讲了        
            ////首先调用其中一个节点的服务,如果失败或者超时，则用轮询
            ////的方式进行重试调用
            try
            {
                var response = Policy.TimeoutAsync(1, TimeoutStrategy.Pessimistic).ExecuteAsync(async () => await GetResult());
            }
            catch (Exception ex) 
            {

            }
        }

        CancellationTokenSource tokenSource;

        #region 线程取消
        public void TaskRun()
        {
            //重新实例化cts.Token就会不一样，不然再次点击的时候会报错，提示线程已经执行完毕
            tokenSource = new CancellationTokenSource();
            Task.Run(() =>
            {
                bool a = true;
                while (a) 
                {
                    Thread.Sleep(10);
                    try
                    {
                        tokenSource.Token.ThrowIfCancellationRequested();
                        Console.WriteLine("线程被执行");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("线程被取消");
                        a=false;
                    }
                }
                Console.WriteLine("方法执行完成");
            }, tokenSource.Token);
            //如果放到这里有可能线程还没有开始执行就被取消了
            //tokenSource.Cancel(true);
        }

        /// <summary>
        /// 停止线程
        /// </summary>
        public void StopTaskRun()
        {
            Thread.Sleep(1000);
            tokenSource.Cancel(false);
        }
        #endregion

        #region Polly 策略
        public void Handle()
        {
            //定义故障 当发生了HttpRequestException异常就会触发策略
            Policy.Handle<HttpRequestException>();
            // Single exception type with condition
            Policy.Handle<Exception>(ex => ex.Message == "dddd");

            // Multiple exception types
            Policy.Handle<HttpRequestException>().Or<OperationCanceledException>();

            // Multiple exception types with condition
            Policy.Handle<Exception>(ex => ex.Message == "ddd").Or<ArgumentException>(ex => ex.ParamName == "example");

            //Task.Run()
            tokenSource = new CancellationTokenSource();
            //var token = new CancellationToken();
            // Inner exceptions of ordinary exceptions or AggregateException, with or without conditions
            // (HandleInner matches exceptions at both the top-level and inner exceptions)
            Policy.HandleInner<HttpRequestException>().OrInner<OperationCanceledException>(ex => ex.CancellationToken != tokenSource.Token);
          
        }

        public async Task GetTaskAsync()
        {
            // Handle return value with condition 
            Policy.HandleResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.NotFound);

            // Handle multiple return values 
            Policy.HandleResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.InternalServerError).OrResult(r => r.StatusCode == HttpStatusCode.BadGateway);

            // Handle primitive return values (implied use of .Equals())
            Policy.HandleResult<HttpStatusCode>(HttpStatusCode.InternalServerError).OrResult(HttpStatusCode.BadGateway);

            // Handle both exceptions and return values in one policy
            HttpStatusCode[] httpStatusCodesWorthRetrying = {
               HttpStatusCode.RequestTimeout, // 408
               HttpStatusCode.InternalServerError, // 500
               HttpStatusCode.BadGateway, // 502
               HttpStatusCode.ServiceUnavailable, // 503
               HttpStatusCode.GatewayTimeout // 504
            };
            Func<bool> methodCall = ()=> GetTask();
            Func<Task> method = () => GetTask1();
            //传递匿名无参方法() 并与想要执行方法关联(=>),可关联方法可任意参数，但必须有返回类型(无返回值的用Action<T>)。

              }

        public async Task<bool> GetResult() 
        {
            await Task.Run(() =>
            {
                Task.Delay(10000);
                return true;
            });
            return true;
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

        public void RetryPolly()
        {
            //重试策略
            var retryPolicy = Policy.Handle<HttpRequestException>()//指定要处理的异常
                .Or<Exception>()
                .OrResult<HttpResponseMessage>(res => { return res.StatusCode != System.Net.HttpStatusCode.OK; })//返回结果不是Ok重试
                .WaitAndRetryAsync(
                    //指定重试次数
                    sleepDurations: new[]
                    {
                        TimeSpan.FromMilliseconds(100),
                        TimeSpan.FromMilliseconds(200),
                        TimeSpan.FromMilliseconds(300)
                    },
                    //出异常会执行以下代码
                    onRetry: (exception, ts, context) =>
                    {
                        Console.WriteLine($"polly.retry：exMsg={ exception.Exception?.Message}, {ts.Minutes * 60 + ts.Seconds}");
                    });
            //超时策略
            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(30);//超时30秒重试
        }
        #endregion
    }
}

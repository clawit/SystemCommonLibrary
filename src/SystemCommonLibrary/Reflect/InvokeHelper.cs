using System;
using System.Threading;
using System.Threading.Tasks;

namespace SystemCommonLibrary.Reflect
{
    public static class InvokeHelper
    {
        public static void TryExecute(Action func, int millisecondsDelay = 1000, int maxAttempts = 3)
        {
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    func();
                    return; // 成功执行，返回
                }
                catch (Exception ex)
                {
                    if (attempt >= maxAttempts)
                    {
                        // 在最后一次尝试后仍然失败，抛出异常
                        throw new Exception($"尝试调用函数{maxAttempts}次后仍然失败", ex);
                    }
                    // 可选：在再次尝试前进行短暂等待
                    if (millisecondsDelay > 0)
                    {
                        Thread.Sleep(millisecondsDelay);
                    }
                }
            }
        }

        public static T TryExecute<T>(Func<T> func, int millisecondsDelay = 1000, int maxAttempts = 3)
        {
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    return func(); // 成功执行，返回结果
                }
                catch (Exception ex)
                {
                    if (attempt >= maxAttempts)
                    {
                        // 在最后一次尝试后仍然失败，抛出异常
                        throw new Exception($"{DateTime.Now}尝试调用函数{maxAttempts}次后仍然失败", ex);
                    }
                    // 可选：在再次尝试前进行短暂等待
                    if (millisecondsDelay > 0)
                    {
                        Thread.Sleep(millisecondsDelay);
                    }
                }
            }
            throw new InvalidOperationException("逻辑错误，不应到达这里");
        }

        public static async Task TryExecuteAsync(Func<Task> func, int millisecondsDelay = 1000, int maxAttempts = 3)
        {
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    await func();
                    return; // 成功执行，返回
                }
                catch (Exception ex)
                {
                    if (attempt >= maxAttempts)
                    {
                        // 在最后一次尝试后仍然失败，抛出异常
                        throw new Exception($"尝试调用函数{maxAttempts}次后仍然失败", ex);
                    }
                    // 可选：在再次尝试前进行短暂等待
                    if (millisecondsDelay > 0)
                    {
                        await Task.Delay(millisecondsDelay);
                    }
                }
            }
        }

        public static async Task<T> TryExecuteAsync<T>(Func<Task<T>> func, int millisecondsDelay = 1000, int maxAttempts = 3)
        {
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    return await func(); // 成功执行，返回结果
                }
                catch (Exception ex)
                {
                    if (attempt >= maxAttempts)
                    {
                        // 在最后一次尝试后仍然失败，抛出异常
                        throw new Exception($"{DateTime.Now}尝试调用函数{maxAttempts}次后仍然失败", ex);
                    }
                    // 可选：在再次尝试前进行短暂等待
                    if (millisecondsDelay > 0)
                    {
                        await Task.Delay(millisecondsDelay);
                    }
                }
            }
            throw new InvalidOperationException("逻辑错误，不应到达这里");
        }

    }
}

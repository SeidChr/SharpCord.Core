using Microsoft.Extensions.Logging;

namespace SharpCord.Core
{
    public static class ThreadingExtensions
    {
        private static void HandleUnawaitedTaskCompletion(Task task, ILogger logger)
        {
            task
                .GetAwaiter()
                .OnCompleted(() =>
                {
                    if (task.Exception != null)
                    {
                        logger.LogError(task.Exception, "Error in non-awaited Task");
                    }
                });
        }

        public static Task NoWait<T1, T2, T3>(Func<T1, T2, T3, Task> action, T1 o1, T2 o2, T3 o3, ILogger logger)
        {
            HandleUnawaitedTaskCompletion(action(o1, o2, o3), logger);
            return Task.CompletedTask;
        }

        public static Task NoWait<T1, T2>(Func<T1, T2, Task> action, T1 o1, T2 o2, ILogger logger)
        {
            HandleUnawaitedTaskCompletion(action(o1, o2), logger);
            return Task.CompletedTask;
        }

        public static Task NoWait<T1>(Func<T1, Task> action, T1 o1, ILogger logger)
        {
            HandleUnawaitedTaskCompletion(action(o1), logger);
            return Task.CompletedTask;
        }
    }
}
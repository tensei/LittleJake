using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace LittleJake.Extensions
{
    public static class TaskExtensions
    {
        public static T AsSync<T>(this Task<T> task, bool configureAwait)
        {
            try
            {
                return task.ConfigureAwait(configureAwait).GetAwaiter().GetResult();
            }
            catch (AggregateException ae)
            {
                var edi = ExceptionDispatchInfo.Capture(ae.InnerException);
                edi.Throw();
                return default;
            }
        }

        public static void AsSync(this Task task, bool configureAwait)
        {
            try
            {
                task.ConfigureAwait(configureAwait).GetAwaiter().GetResult();
            }
            catch (AggregateException ae)
            {
                var edi = ExceptionDispatchInfo.Capture(ae.InnerException);
                edi.Throw();
            }
        }
    }
}
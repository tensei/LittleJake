using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace LittleSteve.Extensions
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
                    return default(T);
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

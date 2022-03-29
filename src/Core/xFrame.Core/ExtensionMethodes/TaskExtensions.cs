using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace xFrame.Core.ExtensionMethodes
{
    public static class TaskExtensions
    {
#pragma warning disable AsyncFixer03 // Fire-and-forget async-void methods or delegates
        public static async void Await(this Task task, IErrorHandler errorHandler = null)

        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                errorHandler?.HandelError(ex);
            }
        }

        public static async void Await(this Task task, Action completedCallBack, IErrorHandler errorHandler = null)
        {
            try
            {
                await task;
                completedCallBack?.Invoke();
            }
            catch (Exception ex)
            {
                errorHandler?.HandelError(ex);
            }
        }

        public static async void Await<T>(this Task<T> task, Action<T> completedCallBack, IErrorHandler errorHandler = null)
        {
            try
            {
                var result = await task;
                completedCallBack?.Invoke(result);
            }
            catch (Exception ex)
            {
                errorHandler?.HandelError(ex);
            }
        }

#pragma warning restore AsyncFixer03 // Fire-and-forget async-void methods or delegates
    }
}

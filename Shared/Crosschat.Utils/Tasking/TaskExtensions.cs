using System;
using System.Threading.Tasks;

namespace Crosschat.Utils.Tasking
{
    public static class AsyncExtensions
    {
        public static async Task<T> WrapWithErrorIgnoring<T>(this Task<T> task, T valueOnError = default(T))
        {
            try
            {
                return await task;
            }
            catch (Exception exc)
            {
            }
            return valueOnError;
        }

        public static async Task WrapWithErrorIgnoring(this Task task)
        {
            try
            {
                await task;
            }
            catch (Exception exc)
            {
            }
        }
    }
}

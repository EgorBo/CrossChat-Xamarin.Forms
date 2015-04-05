using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Crosschat.Server.Application.DataTransferObjects.Requests;

namespace Crosschat.Server.Application.DataTransferObjects.Utils
{
    public class RequestsHandler
    {
        private readonly Dictionary<long, TaskCompletionSource<ResponseBase>> _tasks = new Dictionary<long, TaskCompletionSource<ResponseBase>>();

        public void AppendResponse(ResponseBase response)
        {
            if (response == null)
                return;

            lock (_tasks)
            {
                TaskCompletionSource<ResponseBase> task;
                if (_tasks.TryGetValue(response.Token, out task))
                {
                    _tasks.Remove(response.Token);
                    response.Error = CommonErrors.Success;
                    task.TrySetResult(response);
                }
            }
        }
        
        public async Task<TResponse> WaitForResponse<TResponse>(RequestBase request, Action action, int timeout = 100000) where TResponse : ResponseBase
        {
            var taskSource = new TaskCompletionSource<ResponseBase>();
            lock (_tasks)
            {
                _tasks[request.Token] = taskSource;
            }
            action();
            StartTimeoutCheck(timeout, request.Token, taskSource);
            var response = await taskSource.Task.ConfigureAwait(false);
            return response as TResponse;
        }

        private async void StartTimeoutCheck(int timeoutInMs, long token, TaskCompletionSource<ResponseBase> taskSource)
        {
            await Task.Delay(timeoutInMs).ConfigureAwait(false);
            if (taskSource.Task.IsCompleted || 
                taskSource.Task.IsFaulted || 
                taskSource.Task.IsCanceled)
                return;

            lock (_tasks)
            {
                if (_tasks.ContainsKey(token))
                {
                    _tasks.Remove(token);
                    taskSource.TrySetResult(new ResponseBase {Error = CommonErrors.Timeout});
                }
            }
        }
    }
}

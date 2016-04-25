using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrionLib.Subscribe.Renewing
{
    public class RecurringTask : IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private readonly Func<Task> _action;
        private readonly TimeSpan _interval;

        public static RecurringTask StartNew(TimeSpan interval, Func<Task> action)
        {
            var task = new RecurringTask(interval, action);

            task.Start();

            return task;
        }

        public RecurringTask(TimeSpan interval, Func<Task> action)
        {
            _action = action;
            _interval = interval;
        }

        public void Start()
        {
            var cancellationToken = _cancellationTokenSource.Token;

            Task.Factory.StartNew(async () =>
            {

                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        await _action();
                    }
                    catch (Exception e)
                    {
                        //add logging here
                    }

                    await Task.Delay(_interval, cancellationToken);
                }
                
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
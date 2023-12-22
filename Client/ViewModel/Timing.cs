using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutomateDesign.Client.ViewModel;

public static class Timing
{
    public static Action DebounceAsync(Func<Task> asyncAction, TimeSpan delay)
    {
        CancellationTokenSource? cancelTokenSource = null;

        return async () =>
        {
            cancelTokenSource?.Cancel();
            cancelTokenSource = new CancellationTokenSource();

            await Task.Delay(delay, cancelTokenSource.Token);

            await asyncAction();
        };
    }
}
using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace PokedexApp.Helpers
{
    public enum ContinueOnCapturedContextValues
    {
        Continue,
        No
    }

    public static class TaskHelper
    {
        #region Exceptions

        public static Action<Exception> TraceException { get; set; }

        private static void HandlerException(Exception ex, bool isSilent = false)
        {
            if (ex is AggregateException)
            {
                if (!isSilent)
                {
                    DoInApplicationThread(() =>
                    {
                        if (ex.InnerException != null)
                        {
                            ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                        }
                        else
                        {
                            throw ex;
                        }
                    });
                }
                else
                {
                    TraceException?.Invoke(ex);
                }
            }
            else if (ex != null)
            {
                if (!isSilent)
                {
                    DoInApplicationThread(() => throw ex);
                }
                else
                {
                    TraceException?.Invoke(ex);
                }
            }
        }

        #endregion Exceptions

        #region Run

        public static void DoInApplicationThread(Action action, DispatcherPriority dispatcherPriority = DispatcherPriority.Normal)
        {
            if (Application.Current == null)
            {
                action();
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(() => action(), dispatcherPriority);
            }
        }

        public static void RunWithoutAwaitIntoAnotherThread(
            Func<Task> function,
            bool isSilentException = false,
            Action continueWithAction = null,
            ContinueOnCapturedContextValues continueOnCapturedContext = ContinueOnCapturedContextValues.No)
        {
            var task = Task.Run(function).ContinueWith(t =>
            {
                if (t.IsFaulted && !t.IsCanceled && t.Exception != null)
                {
                    HandlerException(t.Exception, isSilentException);
                }

                if (continueWithAction != null)
                {
                    Application.Current.Dispatcher.BeginInvoke(() => continueWithAction());
                }
            }).ConfigureAwait(continueOnCapturedContext == ContinueOnCapturedContextValues.Continue);
        }

        public static void ExecuteWithoutAwait(
            this Task task,
            bool isSilentException = false,
            Action continueWithAction = null,
            ContinueOnCapturedContextValues continueOnCapturedContext = ContinueOnCapturedContextValues.No)
        {
            task.ContinueWith(t =>
            {
                if (t.IsFaulted && !t.IsCanceled && t.Exception != null)
                {
                    HandlerException(t.Exception, isSilentException);
                }

                if (continueWithAction != null)
                {
                    DoInApplicationThread(continueWithAction);
                }
            }).ConfigureAwait(continueOnCapturedContext == ContinueOnCapturedContextValues.Continue);
        }

        public static void ExecuteWithoutAwait(
            this Task task,
            Action<bool, Exception> continueWithAction,
            bool isSilentException = false,
            ContinueOnCapturedContextValues continueOnCapturedContext = ContinueOnCapturedContextValues.No)
        {
            task.ContinueWith(t =>
            {
                var success = true;
                if (t.IsFaulted && !t.IsCanceled && t.Exception != null)
                {
                    success = false;
                    HandlerException(t.Exception, isSilentException);
                }

                if (continueWithAction != null)
                {
                    DoInApplicationThread(() => continueWithAction(success, t.Exception));
                }
            }).ConfigureAwait(continueOnCapturedContext == ContinueOnCapturedContextValues.Continue);
        }

        #endregion Run

        #region Run Dispose

        public static async Task ExecuteAndCheckIfDisposed(
            this Task task,
            Func<bool> isDisposed,
            string objectName = null,
            bool checkResult = true)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                if (isDisposed())
                {
                    throw new ObjectDisposedException(objectName, ex);
                }
                else
                {
                    throw;
                }
            }

            if (checkResult && isDisposed())
            {
                throw new ObjectDisposedException(objectName);
            }
        }

        public static async Task<T> ExecuteAndCheckIfDisposed<T>(
            this Task<T> task,
            Func<bool> isDisposed,
            string objectName = null,
            bool checkResult = true)
        {
            var result = default(T);
            try
            {
                result = await task;
            }
            catch (Exception ex)
            {
                if (isDisposed())
                {
                    throw new ObjectDisposedException(objectName, ex);
                }
                else
                {
                    throw;
                }
            }

            return checkResult && isDisposed()
                ? throw new ObjectDisposedException(objectName)
                : result;
        }

        #endregion Run Dispose

        public static async Task<T> TimeoutAfter<T>(this Task<T> task, int delay)
        {
            await Task.WhenAny(task, Task.Delay(delay));
            return !task.IsCompleted
                ? throw new TimeoutException()
                : await task;
        }

        public static async Task TimeoutAfter(this Task task, int delay)
        {
            await Task.WhenAny(task, Task.Delay(delay));
            if (!task.IsCompleted)
            {
                throw new TimeoutException();
            }
        }

        public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            if (cancellationToken == CancellationToken.None)
            {
                return await task;
            }

            var tcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
            using (cancellationToken.Register(state => ((TaskCompletionSource<object>)state).TrySetResult(null), tcs))
            {
                var resultTask = await Task.WhenAny(task, tcs.Task);
                return resultTask == tcs.Task
                    ? throw new OperationCanceledException(cancellationToken)
                    : await task;
            }
        }

        public static async Task WithCancellation(this Task task, CancellationToken cancellationToken)
        {
            if (cancellationToken == CancellationToken.None)
            {
                await task;
            }
            else
            {
                var tcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);
                using (cancellationToken.Register(state => ((TaskCompletionSource<object>)state).TrySetResult(null), tcs))
                {
                    var resultTask = await Task.WhenAny(task, tcs.Task);
                    if (resultTask == tcs.Task)
                    {
                        throw new OperationCanceledException(cancellationToken);
                    }
                }
            }
        }
    }
}

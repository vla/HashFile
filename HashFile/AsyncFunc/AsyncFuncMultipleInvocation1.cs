using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;

namespace System
{
    public class AsyncFuncMultipleInvocation<T, TResult>
    {
        private Func<T, Func<bool>, Action<int>, TResult> _func;
        private SendOrPostCallback _onCompleted;
        private SendOrPostCallback _onProgressChanged;
        private HybridDictionary _userStateToLifetime;

        public AsyncFuncMultipleInvocation(Func<T, TResult> func)
            : this((arg, isCancelled, reportProgress) => func(arg)) {
        }

        public AsyncFuncMultipleInvocation(Func<T, Func<bool>, TResult> func)
            : this((arg, isCancelled, reportProgress) => func(arg, isCancelled)) {
        }

        public AsyncFuncMultipleInvocation(Func<T, Action<int>, TResult> func)
            : this((arg, isCancelled, reportProgress) => func(arg, reportProgress)) {
        }

        public AsyncFuncMultipleInvocation(Func<T, Func<bool>, Action<int>, TResult> func) {
            _func = func;
            _onCompleted = new SendOrPostCallback(CompletedCallback);
            _onProgressChanged = new SendOrPostCallback(ReportProgressCallback);
            _userStateToLifetime = new HybridDictionary();
        }

        public object Sender { get; set; }

        public event AsyncFuncCompletedEventHandler<TResult> Completed;

        public event ProgressChangedEventHandler ProgressChanged;

        public void InvokeAsync(T arg, object userState) {
            AsyncOperation asyncOp;
            lock (_userStateToLifetime.SyncRoot) {
                if (_userStateToLifetime.Contains(userState)) {
                    throw new ArgumentException("userState must be unique");
                }
                asyncOp = AsyncOperationManager.CreateOperation(userState);
                _userStateToLifetime[userState] = asyncOp;
            }
            WorkerDelegate worker = new WorkerDelegate(Worker);
            worker.BeginInvoke(arg, asyncOp, null, null);
        }

        public void Cancel(object userState) {
            if (_userStateToLifetime[userState] != null) {
                lock (_userStateToLifetime.SyncRoot) {
                    _userStateToLifetime.Remove(userState);
                }
            }
        }

        private void CompletedCallback(object operationState) {
            AsyncFuncCompletedEventArgs<TResult> e = (AsyncFuncCompletedEventArgs<TResult>)operationState;
            AsyncFuncCompletedEventHandler<TResult> snapshot = Completed;
            if (snapshot != null) {
                snapshot(Sender, e);
            }
        }

        private void ReportProgressCallback(object operationState) {
            ProgressChangedEventArgs e = (ProgressChangedEventArgs)operationState;
            ProgressChangedEventHandler snapshot = ProgressChanged;
            if (snapshot != null) {
                snapshot(Sender, e);
            }
        }

        private bool Cancelled(object userState) {
            return (_userStateToLifetime[userState] == null);
        }

        private void ReportProgressChanged(int progressPercentage, AsyncOperation asyncOp) {
            ProgressChangedEventArgs e = new ProgressChangedEventArgs(progressPercentage, asyncOp.UserSuppliedState);
            asyncOp.Post(_onProgressChanged, e);
        }

        private delegate void WorkerDelegate(T arg, AsyncOperation asyncOp);

        private void Worker(T arg, AsyncOperation asyncOp) {
            TResult result = default(TResult);
            Exception error = null;
            bool cancelled;
            if (!Cancelled(asyncOp.UserSuppliedState)) {
                try {
                    result = _func(arg, () => Cancelled(asyncOp.UserSuppliedState), (p) => ReportProgressChanged(p, asyncOp));
                } catch (Exception ex) {
                    error = ex;
                }
            }
            cancelled = Cancelled(asyncOp.UserSuppliedState);
            if (!cancelled) {
                lock (_userStateToLifetime.SyncRoot) {
                    _userStateToLifetime.Remove(asyncOp.UserSuppliedState);
                }
            }
            AsyncFuncCompletedEventArgs<TResult> e = new AsyncFuncCompletedEventArgs<TResult>(result, error, cancelled, asyncOp.UserSuppliedState);
            asyncOp.PostOperationCompleted(_onCompleted, e);
        }
    }
}
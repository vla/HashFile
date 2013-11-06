using System.ComponentModel;
using System.Threading;

namespace System
{
    public sealed class AsyncAction<T>
    {
        private Action<T, Func<bool>, Action<int>> _action;
        private SendOrPostCallback _onCompleted;
        private SendOrPostCallback _onProgressChanged;
        private bool _isBusy;
        private bool _isCancelled;
        private bool _isCancellable;
        private int _status;

        public AsyncAction(Action<T> action)
            : this((arg, isCancelled, reportProgress) => action(arg), false, false) {
        }

        public AsyncAction(Action<T, Func<bool>> action)
            : this((arg, isCancelled, reportProgress) => action(arg, isCancelled), true, false) {
        }

        public AsyncAction(Action<T, Action<int>> action)
            : this((arg, isCancelled, reportProgress) => action(arg, reportProgress), false, true) {
        }

        public AsyncAction(Action<T, Func<bool>, Action<int>> action)
            : this(action, true, true) {
        }

        /// <summary>
        /// Main constructor
        /// </summary>
        /// <remarks>
        /// Initializes all fields basing on parameters passed from public constructors
        /// </remarks>
        private AsyncAction(Action<T, Func<bool>, Action<int>> action, bool isCancellable, bool canReportProgress) {
            _action = action;
            _isCancellable = isCancellable;
            if (canReportProgress) {
                _onProgressChanged = new SendOrPostCallback(ReportProgressCallback);
            }
            _onCompleted = new SendOrPostCallback(CompletedCallback);
            _isBusy = false;
            _isCancelled = false;
        }

        public object Sender { get; set; }

        public event AsyncCompletedEventHandler Completed;

        public event ProgressChangedEventHandler ProgressChanged;

        public bool IsBusy { get { return _isBusy; } }

        public bool TryInvokeAsync(T arg) {
            return InvokeAsync(arg, true);
        }

        public void InvokeAsync(T arg) {
            InvokeAsync(arg, false);
        }

        internal bool InvokeAsync(T arg, bool isTry) {
            lock (this) {
                if (_isBusy) {
                    if (isTry) {
                        return false;
                    } else {
                        throw new InvalidOperationException("Operation is still executing");
                    }
                }
                _isBusy = true;
            }
            _isCancelled = false;
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(null);
            WorkerDelegate worker = new WorkerDelegate(Worker);
            worker.BeginInvoke(arg, asyncOp, null, null);
            return true;
        }

        public void Cancel() {
            if (!_isCancellable) {
                throw new InvalidOperationException("Delegate supplied in constructor doesn't handle cancelation");
            }
            _isCancelled = true;
        }

        private void CompletedCallback(object operationState) {
            AsyncCompletedEventArgs e = (AsyncCompletedEventArgs)operationState;
            AsyncCompletedEventHandler snapshot = Completed;
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

        private void ReportProgressChanged(int progressPercentage, AsyncOperation asyncOp) {
            ProgressChangedEventArgs e = new ProgressChangedEventArgs(progressPercentage, asyncOp.UserSuppliedState);
            asyncOp.Post(_onProgressChanged, e);
        }

        private delegate void WorkerDelegate(T arg, AsyncOperation asyncOp);

        private void Worker(T arg, AsyncOperation asyncOp) {
            if (Interlocked.CompareExchange(ref _status, 1, 0) == 1) {
                _isBusy = true;
                return;
            }

            Exception error = null;

            try {
                if (!_isCancelled) {
                    _action(arg, () => _isCancelled, (p) => ReportProgressChanged(p, asyncOp));
                }
            } catch (Exception ex) {
                error = ex;
            } finally {
                _isBusy = false;
                AsyncCompletedEventArgs e = new AsyncCompletedEventArgs(error, _isCancelled, null);
                asyncOp.PostOperationCompleted(_onCompleted, e);
                Interlocked.CompareExchange(ref _status, 0, 1);
            }
        }
    }
}
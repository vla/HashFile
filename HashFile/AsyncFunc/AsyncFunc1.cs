using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System {

    public sealed class AsyncFunc<T, TResult> {
        private Func<T, Func<bool>, Action<int>, TResult> _func;
        private SendOrPostCallback _onCompleted;
        private SendOrPostCallback _onProgressChanged;
        private bool _isBusy;
        private bool _isCancelled;
        private bool _isCancellable;

        private int _status;

        private Timer _timeoutTimer;

        public AsyncFunc ( Func<T, TResult> func )
            : this(( arg, isCancelled, reportProgress ) => func(arg), false, false) {
        }

        public AsyncFunc ( Func<T, Func<bool>, TResult> func )
            : this(( arg, isCancelled, reportProgress ) => func(arg, isCancelled), true, false) {
        }

        public AsyncFunc ( Func<T, Action<int>, TResult> func )
            : this(( arg, isCancelled, reportProgress ) => func(arg, reportProgress), false, true) {
        }

        public AsyncFunc ( Func<T, Func<bool>, Action<int>, TResult> func )
            : this(func, true, true) {
        }

        /// <summary>
        /// Main constructor
        /// </summary>
        /// <remarks>
        /// Initializes all fields basing on parameters passed from public constructors
        /// </remarks>
        private AsyncFunc ( Func<T, Func<bool>, Action<int>, TResult> func, bool isCancellable, bool canReportProgress ) {
            _func = func;
            _isCancellable = isCancellable;
            if ( canReportProgress ) {
                _onProgressChanged = new SendOrPostCallback(ReportProgressCallback);
            }
            _onCompleted = new SendOrPostCallback(CompletedCallback);
            _isBusy = false;
            _isCancelled = false;
        }

        public int TimeoutMilliseconds { get; set; }

        public object Sender { get; set; }

        public event AsyncFuncCompletedEventHandler<TResult> Completed;

        public event ProgressChangedEventHandler ProgressChanged;

        public bool IsBusy { get { return _isBusy; } }

        public bool TryInvokeAsync ( T arg ) {
            return InvokeAsyncCore(arg, true);
        }

        public void InvokeAsync ( T arg ) {
            InvokeAsyncCore(arg, false);
        }

        internal bool InvokeAsyncCore ( T arg, bool isTry ) {
            lock ( this ) {
                if ( _isBusy ) {
                    if ( isTry ) {
                        return false;
                    }
                    else {
                        throw new InvalidOperationException("Operation is still executing");
                    }
                }
                _isBusy = true;
            }
            _isCancelled = false;
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(null);
            WorkerDelegate worker = new WorkerDelegate(Worker);
            worker.BeginInvoke(arg, asyncOp, null, null);

            if ( this.TimeoutMilliseconds > 0 ) {
                _timeoutTimer = new Timer(TimeoutCallback, asyncOp, this.TimeoutMilliseconds, Timeout.Infinite);
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void DisposeTimer () {
            if ( _timeoutTimer != null ) {
                _timeoutTimer.Dispose();
                _timeoutTimer = null;
            }
        }

        // 超时的回调方法
        private void TimeoutCallback ( object state ) {
            DisposeTimer();

            Cancel();
        }

        public void Cancel () {
            if ( !_isCancellable ) {
                throw new InvalidOperationException("Delegate supplied in constructor doesn't handle cancelation");
            }
            _isCancelled = true;
        }

        private void CompletedCallback ( object operationState ) {
            AsyncFuncCompletedEventArgs<TResult> e = (AsyncFuncCompletedEventArgs<TResult>)operationState;
            AsyncFuncCompletedEventHandler<TResult> snapshot = Completed;
            if ( snapshot != null ) {
                snapshot(Sender, e);
            }
        }

        private void ReportProgressCallback ( object operationState ) {
            ProgressChangedEventArgs e = (ProgressChangedEventArgs)operationState;
            ProgressChangedEventHandler snapshot = ProgressChanged;
            if ( snapshot != null ) {
                snapshot(Sender, e);
            }
        }

        private void ReportProgressChanged ( int progressPercentage, AsyncOperation asyncOp ) {
            ProgressChangedEventArgs e = new ProgressChangedEventArgs(progressPercentage, asyncOp.UserSuppliedState);
            asyncOp.Post(_onProgressChanged, e);
        }

        private delegate void WorkerDelegate ( T arg, AsyncOperation asyncOp );

        private void Worker ( T arg, AsyncOperation asyncOp ) {

            // 进入这个方法表示异步调用已完成。
            // 设置标志变量，确保只调用一次，不管是正常完成的回调还是超时回调。
            if ( Interlocked.CompareExchange(ref _status, 1, 0) == 1 ) {
                DisposeTimer();
                _isBusy = true;
                return;
            }

            TResult result = default(TResult);
            Exception error = null;

            try {
                if ( !_isCancelled ) {

                    // Check if function can check for cancellation
                    Func<bool> isCancelled = null;
                    if ( _isCancellable ) {
                        isCancelled = () => _isCancelled;
                    }

                    // Check if function can report progress
                    Action<int> reportProgress = null;
                    if ( _onProgressChanged != null ) {
                        reportProgress = ( p ) => ReportProgressChanged(p, asyncOp);
                    }

                    // Invoke function synchronously
                    result = _func(arg, isCancelled, reportProgress);
                }
            }
            catch ( Exception ex ) {
                error = ex;
            }
            finally {
                DisposeTimer();
                _isBusy = false;
                AsyncFuncCompletedEventArgs<TResult> e = new AsyncFuncCompletedEventArgs<TResult>(result, error, _isCancelled, null);
                asyncOp.PostOperationCompleted(_onCompleted, e);
                Interlocked.CompareExchange(ref _status, 0, 1);
            }
        }
    }
}
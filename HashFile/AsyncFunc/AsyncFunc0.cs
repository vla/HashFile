using System.ComponentModel;
using System.Threading;

namespace System {

    public sealed class AsyncFunc<TResult> {
        private Func<Func<bool>, Action<int>, TResult> _func;
        private SendOrPostCallback _onCompleted;
        private SendOrPostCallback _onProgressChanged;
        private bool _isBusy;
        private bool _isCancelled;
        private bool _isCancellable;

        private int _status;

        public AsyncFunc ( Func<TResult> func )
            : this(( isCancelled, reportProgress ) => func(), false, false) {
        }

        public AsyncFunc ( Func<Func<bool>, TResult> func )
            : this(( isCancelled, reportProgress ) => func(isCancelled), true, false) {
        }

        public AsyncFunc ( Func<Action<int>, TResult> func )
            : this(( isCancelled, reportProgress ) => func(reportProgress), false, true) {
        }

        public AsyncFunc ( Func<Func<bool>, Action<int>, TResult> func )
            : this(func, true, true) {
        }

        /// <summary>
        /// Main constructor
        /// </summary>
        /// <remarks>
        /// Initializes all fields basing on parameters passed from public constructors
        /// </remarks>
        private AsyncFunc ( Func<Func<bool>, Action<int>, TResult> func, bool isCancellable, bool canReportProgress ) {
            _func = func;
            _isCancellable = isCancellable;
            if ( canReportProgress ) {
                _onProgressChanged = new SendOrPostCallback(ReportProgressCallback);
            }
            _onCompleted = new SendOrPostCallback(CompletedCallback);
            _isBusy = false;
            _isCancelled = false;
        }

        public object Sender { get; set; }

        public event AsyncFuncCompletedEventHandler<TResult> Completed;

        public event ProgressChangedEventHandler ProgressChanged;

        public bool IsBusy { get { return _isBusy; } }

        public bool TryInvokeAsync () {
            return InvokeAsyncCore(true);
        }

        public void InvokeAsync () {
            InvokeAsyncCore(false);
        }

        internal bool InvokeAsyncCore ( bool isTry ) {
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
            worker.BeginInvoke(asyncOp, null, null);
            return true;
        }

        public void Cancel () {
            if ( !_isCancellable ) {
                throw new InvalidOperationException("Delegate supplied in constructor doesn't handle cancelation");
            }
            _isCancelled = true; ;
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

        private delegate void WorkerDelegate ( AsyncOperation asyncOp );

        private void Worker ( AsyncOperation asyncOp ) {
            if ( Interlocked.CompareExchange(ref _status, 1, 0) == 1 ) {
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
                    result = _func(isCancelled, reportProgress);
                }
            }
            catch ( Exception ex ) {
                error = ex;
            }
            finally {
                _isBusy = false;
                AsyncFuncCompletedEventArgs<TResult> e = new AsyncFuncCompletedEventArgs<TResult>(result, error, _isCancelled, null);
                asyncOp.PostOperationCompleted(_onCompleted, e);
                Interlocked.CompareExchange(ref _status, 0, 1);
            }
        }
    }
}
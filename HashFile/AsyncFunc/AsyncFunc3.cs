using System.ComponentModel;

namespace System {

    public sealed class AsyncFunc<T1, T2, T3, TResult> {
        private AsyncFunc<Tuple<T1, T2, T3>, TResult> _func;

        public AsyncFunc ( Func<T1, T2, T3, TResult> func ) {
            _func = new AsyncFunc<Tuple<T1, T2, T3>, TResult>(
                ( tuple ) => func(tuple.Item1, tuple.Item2, tuple.Item3));
        }

        public AsyncFunc ( Func<T1, T2, T3, Func<bool>, TResult> func ) {
            _func = new AsyncFunc<Tuple<T1, T2, T3>, TResult>(
                ( tuple, isCancelled ) => func(tuple.Item1, tuple.Item2, tuple.Item3, isCancelled));
        }

        public AsyncFunc ( Func<T1, T2, T3, Action<int>, TResult> func ) {
            _func = new AsyncFunc<Tuple<T1, T2, T3>, TResult>(
                ( tuple, reportProgress ) => func(tuple.Item1, tuple.Item2, tuple.Item3, reportProgress));
        }

        public AsyncFunc ( Func<T1, T2, T3, Func<bool>, Action<int>, TResult> func ) {
            _func = new AsyncFunc<Tuple<T1, T2, T3>, TResult>(
                ( tuple, isCancelled, reportProgress ) => func(tuple.Item1, tuple.Item2, tuple.Item3, isCancelled, reportProgress));
        }

        public object Sender {
            get { return _func.Sender; }
            set { _func.Sender = value; }
        }

        public event AsyncFuncCompletedEventHandler<TResult> Completed {
            add { _func.Completed += value; }
            remove { _func.Completed -= value; }
        }

        public event ProgressChangedEventHandler ProgressChanged {
            add { _func.ProgressChanged += value; }
            remove { _func.ProgressChanged -= value; }
        }

        public bool IsBusy { get { return _func.IsBusy; } }

        public bool TryInvokeAsync ( T1 arg1, T2 arg2, T3 arg3 ) {
            return InvokeAsyncCore(arg1, arg2, arg3, true);
        }

        public void InvokeAsync ( T1 arg1, T2 arg2, T3 arg3 ) {
            InvokeAsyncCore(arg1, arg2, arg3, false);
        }

        internal bool InvokeAsyncCore ( T1 arg1, T2 arg2, T3 arg3, bool isTry ) {
            return _func.InvokeAsyncCore(new Tuple<T1, T2, T3>(arg1, arg2, arg3), isTry);
        }

        public void Cancel () {
            _func.Cancel();
        }
    }
}
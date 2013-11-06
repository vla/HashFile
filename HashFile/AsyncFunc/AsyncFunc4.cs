using System.ComponentModel;

namespace System
{
    public sealed class AsyncFunc<T1, T2, T3, T4, TResult>
    {
        private AsyncFunc<Tuple<T1, T2, T3,  T4>, TResult> _func;

        public AsyncFunc(Func<T1, T2, T3, T4, TResult> func) {
            _func = new AsyncFunc<Tuple<T1, T2, T3, T4>, TResult>(
                (tuple) => func(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
        }

        public AsyncFunc(Func<T1, T2, T3, T4, Func<bool>, TResult> func) {
            _func = new AsyncFunc<Tuple<T1, T2, T3, T4>, TResult>(
                (tuple, isCancelled) => func(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, isCancelled));
        }

        public AsyncFunc(Func<T1, T2, T3, T4, Action<int>, TResult> func) {
            _func = new AsyncFunc<Tuple<T1, T2, T3, T4>, TResult>(
                (tuple, reportProgress) => func(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, reportProgress));
        }

        public AsyncFunc(Func<T1, T2, T3, T4, Func<bool>, Action<int>, TResult> func) {
            _func = new AsyncFunc<Tuple<T1, T2, T3, T4>, TResult>(
                (tuple, isCancelled, reportProgress) => func(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, isCancelled, reportProgress));
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

        public bool TryInvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
            return InvokeAsyncCore(arg1, arg2, arg3, arg4, true);
        }

        public void InvokeAsync(T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
            InvokeAsyncCore(arg1, arg2, arg3, arg4, false);
        }

        internal bool InvokeAsyncCore(T1 arg1, T2 arg2, T3 arg3, T4 arg4, bool isTry) {
            return _func.InvokeAsyncCore(new Tuple<T1, T2, T3, T4>(arg1, arg2, arg3, arg4), isTry);
        }

        public void Cancel() {
            _func.Cancel();
        }
    }
}
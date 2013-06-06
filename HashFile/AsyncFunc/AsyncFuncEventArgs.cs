using System.ComponentModel;

namespace System {

    public class AsyncFuncCompletedEventArgs<T>: AsyncCompletedEventArgs {
        protected T _result;

        public AsyncFuncCompletedEventArgs ( T result, Exception exception, bool cancelled, object userState )
            : base(exception, cancelled, userState) {
            _result = result;
        }

        public T Result {
            get {
                RaiseExceptionIfNecessary();
                return _result;
            }
        }
    }

    public class AsyncFuncProgressChangedEventArgs<T>: ProgressChangedEventArgs {
        protected T _data;

        public AsyncFuncProgressChangedEventArgs ( T data, int progressPercentage, object userState )
            : base(progressPercentage, userState) {
            _data = data;
        }

        public T ProgressData {
            get {
                return _data;
            }
        }
    }

    public delegate void AsyncFuncCompletedEventHandler<T> ( object sender, AsyncFuncCompletedEventArgs<T> e );
}
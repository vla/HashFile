namespace System {
#if !NET4 && !NET35 && !NET45

    public delegate void Action<T1, T2> ( T1 arg1, T2 arg2 );

    public delegate void Action<T1, T2, T3> ( T1 arg1, T2 arg2, T3 arg3 );

#endif
}
/// <summary>
/// Global Event Center, Static class, manages global game events
/// </summary>
public static class GEventCenter
{
    private static EventCenter<GameEvent> _eventCenter = new EventCenter<GameEvent>();

    public static void Subscribe(GameEvent eventType, CallBack callback)
    {
        _eventCenter.Subscribe(eventType, callback);
    }

    public static void Subscribe<T>(GameEvent eventType, CallBack<T> callback)
    {
        _eventCenter.Subscribe(eventType, callback);
    }

    public static void Subscribe<T, X>(GameEvent eventType, CallBack<T, X> callback)
    {
        _eventCenter.Subscribe(eventType, callback);
    }

    public static void Subscribe<T, X, Y>(GameEvent eventType, CallBack<T, X, Y> callback)
    {
        _eventCenter.Subscribe(eventType, callback);
    }

    public static void Subscribe<T, X, Y, Z>(GameEvent eventType, CallBack<T, X, Y, Z> callback)
    {
        _eventCenter.Subscribe(eventType, callback);
    }

    public static void Subscribe<T, X, Y, Z, W>(GameEvent eventType, CallBack<T, X, Y, Z, W> callback)
    {
        _eventCenter.Subscribe(eventType, callback);
    }

    public static void Unsubscribe(GameEvent eventType, CallBack callback)
    {
        _eventCenter.Unsubscribe(eventType, callback);
    }

    public static void Unsubscribe<T>(GameEvent eventType, CallBack<T> callback)
    {
        _eventCenter.Unsubscribe(eventType, callback);
    }

    public static void Unsubscribe<T, X>(GameEvent eventType, CallBack<T, X> callback)
    {
        _eventCenter.Unsubscribe(eventType, callback);
    }

    public static void Unsubscribe<T, X, Y>(GameEvent eventType, CallBack<T, X, Y> callback)
    {
        _eventCenter.Unsubscribe(eventType, callback);
    }

    public static void Unsubscribe<T, X, Y, Z>(GameEvent eventType, CallBack<T, X, Y, Z> callback)
    {
        _eventCenter.Unsubscribe(eventType, callback);
    }

    public static void Unsubscribe<T, X, Y, Z, W>(GameEvent eventType, CallBack<T, X, Y, Z, W> callback)
    {
        _eventCenter.Unsubscribe(eventType, callback);
    }

    public static void Invoke(GameEvent eventType)
    {
        _eventCenter.Invoke(eventType);
    }

    public static void Invoke<T>(GameEvent eventType, T arg)
    {
        _eventCenter.Invoke(eventType, arg);
    }

    public static void Invoke<T, X>(GameEvent eventType, T arg1, X arg2)
    {
        _eventCenter.Invoke(eventType, arg1, arg2);
    }

    public static void Invoke<T, X, Y>(GameEvent eventType, T arg1, X arg2, Y arg3)
    {
        _eventCenter.Invoke(eventType, arg1, arg2, arg3);
    }

    public static void Invoke<T, X, Y, Z>(GameEvent eventType, T arg1, X arg2, Y arg3, Z arg4)
    {
        _eventCenter.Invoke(eventType, arg1, arg2, arg3, arg4);
    }

    public static void Invoke<T, X, Y, Z, W>(GameEvent eventType, T arg1, X arg2, Y arg3, Z arg4, W arg5)
    {
        _eventCenter.Invoke(eventType, arg1, arg2, arg3, arg4, arg5);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utility class to schedule delayed tasks
/// </summary>
public static class TimerUtility
{
    private class TimerRunner : MonoBehaviour { }

    private static TimerRunner _runner;
    private static Dictionary<Guid, Coroutine> _activeCoroutines = new();

    private static void Init()
    {
        if (_runner == null)
        {
            GameObject obj = new GameObject("TimerUtility");
            UnityEngine.Object.DontDestroyOnLoad(obj);
            _runner = obj.AddComponent<TimerRunner>();
        }
    }

    /// <summary>
    /// Run a task after delay and return a unique Guid for potential cancellation
    /// </summary>
    public static Guid Invoke(float delay, Action action)
    {
        Init();
        Guid id = Guid.NewGuid();
        Coroutine coroutine = _runner.StartCoroutine(RunTimer(id, delay, action));
        _activeCoroutines[id] = coroutine;
        return id;
    }

    /// <summary>
    /// Run a task with one parameter after delay and return a Guid for cancellation
    /// </summary>
    public static Guid Invoke<T>(float delay, Action<T> action, T param)
    {
        Init();
        Guid id = Guid.NewGuid();
        Coroutine coroutine = _runner.StartCoroutine(RunTimer(id, delay, () => action?.Invoke(param)));
        _activeCoroutines[id] = coroutine;
        return id;
    }

    /// <summary>
    /// Cancel a delayed task by its Guid
    /// </summary>
    public static void CancelInvoke(Guid id)
    {
        if (_activeCoroutines.TryGetValue(id, out Coroutine coroutine))
        {
            _runner.StopCoroutine(coroutine);
            _activeCoroutines.Remove(id);
        }
    }

    private static IEnumerator RunTimer(Guid id, float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
        _activeCoroutines.Remove(id);
    }
}

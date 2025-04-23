using System;
using System.Collections.Generic;

/// <summary>
/// Event processing center template
/// </summary>
public class EventCenter<TEvent> where TEvent : struct, IConvertible
{
    private Dictionary<TEvent, Delegate> m_EventTable = new Dictionary<TEvent, Delegate>();// Define a dictionary for storing event codes and delegates

    /// <summary>
    /// Simplify the program, instead of adding listeners
    /// </summary>
    private void OnSubscribing(TEvent eventType, Delegate callBack)
    {
        if (!m_EventTable.ContainsKey(eventType)) //Judge whether the event code is included in the event list, if not, add the event code to the event list
        {
            m_EventTable.Add(eventType, null); //Add this event code to the event table (the delegate is empty)
        }
        Delegate d = m_EventTable[eventType]; //If the event code is included in the event list, store the delegate corresponding to the event code as d
        if (d != null && d.GetType() != callBack.GetType())//Determine if the original delegate in the event code givent is not empty, and is inconsistent with the newly added delegate type (parameter type or quantity is different)
        {
            //Throw a Exception
            throw new Exception(string.Format("Add listener error: try to add different types of delegates for event {0}, the delegate type corresponding to the current event is {1}, and the delegate type to be added is {2}", eventType, d.GetType(), callBack.GetType()));
        }
    }

    /// <summary>
    /// Add a listener fonction/ No Parameters
    /// </summary>
    /// <param name="eventType"></eventCode>
    /// <param name="callBack"></DelegateToAdd>
    public void Subscribe(TEvent eventType, CallBack callBack)
    {
        OnSubscribing(eventType, callBack);
        //If there is no exception, associate the delegate with the original delegate and store it in the event code
        m_EventTable[eventType] = (CallBack)m_EventTable[eventType] + callBack;//Add them directly (because the new delegate is a callback type, so the original delegate is forced to callback)
    }

    /// <summary>
    ///Add a listener fonction / Single Parameter
    /// </summary>
    /// <param name="eventType"></eventCode>
    /// <param name="callBack"></DelegateToAdd>
    public void Subscribe<T>(TEvent eventType, CallBack<T> callBack)
    {
        OnSubscribing(eventType, callBack);
        //If there is no exception, associate the delegate with the original delegate and store it in the event code
        m_EventTable[eventType] = (CallBack<T>)m_EventTable[eventType] + callBack;//Add them directly (because the new delegate is a callback type, so the original delegate is forced to callback)
    }

    /// <summary>
    /// Add a listener fonction/ Double Parameters
    /// </summary>
    ///  <param name="eventType"></eventCode>
    /// <param name="callBack"></DelegateToAdd>
    public void Subscribe<T, X>(TEvent eventType, CallBack<T, X> callBack)
    {
        OnSubscribing(eventType, callBack);
        //If there is no exception, associate the delegate with the original delegate and store it in the event code
        m_EventTable[eventType] = (CallBack<T, X>)m_EventTable[eventType] + callBack;//Add them directly (because the new delegate is a callback type, so the original delegate is forced to callback)
    }

    /// <summary>
    /// Add a listener fonction / Three Parameters
    /// </summary>
    /// <param name="eventType"></eventCode>
    /// <param name="callBack"></DelegateToAdd>
    public void Subscribe<T, X, Y>(TEvent eventType, CallBack<T, X, Y> callBack)
    {
        OnSubscribing(eventType, callBack);
        //If there is no exception, associate the delegate with the original delegate and store it in the event code
        m_EventTable[eventType] = (CallBack<T, X, Y>)m_EventTable[eventType] + callBack;//Add them directly (because the new delegate is a callback type, so the original delegate is forced to callback)
    }

    /// <summary>
    /// Add a listener fonction / Four Parameters
    /// </summary>
    /// <param name="eventType"></eventCode>
    /// <param name="callBack"></DelegateToAdd>
    public void Subscribe<T, X, Y, Z>(TEvent eventType, CallBack<T, X, Y, Z> callBack)
    {
        OnSubscribing(eventType, callBack);
        //If there is no exception, associate the delegate with the original delegate and store it in the event code
        m_EventTable[eventType] = (CallBack<T, X, Y, Z>)m_EventTable[eventType] + callBack;//Add them directly (because the new delegate is a callback type, so the original delegate is forced to callback)
    }

    /// <summary>
    /// Add a listener fonction / Five Parameters
    /// </summary>
    /// <param name="eventType"></eventCode>
    /// <param name="callBack"></DelegateToAdd>
    public void Subscribe<T, X, Y, Z, W>(TEvent eventType, CallBack<T, X, Y, Z, W> callBack)
    {
        OnSubscribing(eventType, callBack);
        //If there is no exception, associate the delegate with the original delegate and store it in the event code
        m_EventTable[eventType] = (CallBack<T, X, Y, Z, W>)m_EventTable[eventType] + callBack;//Add them directly (because the new delegate is a callback type, so the original delegate is forced to callback)
    }

    /// <summary>
    ///  Simplify the procedure, instead of removing the listener
    /// </summary>
    /// <param name="eventType"></eventCode>
    /// <param name="callBack"></param>
    private void OnUnsubscribing(TEvent eventType, Delegate callBack)
    {
        if (m_EventTable.ContainsKey(eventType))//Judge whether the event code is included in the event list
        {
            Delegate d = m_EventTable[eventType];//If the event code is included in the event list, store the delegate corresponding to the event code as d
            if (d == null) //Judging whether d is empty, if it is empty, remove the event code from the event list and throw an exception
            {
                m_EventTable.Remove(eventType);//Remove the event code from the event list
                throw new Exception(string.Format("Remove listener error: event {0} has no corresponding delegate", eventType));
            }
            else if (d.GetType() != callBack.GetType())//If it is not empty, continue to judge whether the delegate type to be removed is consistent with the delegate type in the original event, or not, throw an exception
            {
                throw new Exception(string.Format("Remove listener error: The delegate type corresponding to the current event {0} is {1}, and the delegate type to be removed is {2}", eventType, d.GetType(), callBack.GetType()));
            }
        }
        else // If the event code is not in the event list
        {
            throw new Exception(String.Format("Remove listener error: the event code {0} to be removed does not exist", eventType));
        }
    }

    /// <summary>
    ///Remove listener fonction/No Parameters
    /// </summary>
    /// <param name="eventType"></eventCode>
    /// <param name="callBack"></DelegateToRemove>
    public void Unsubscribe(TEvent eventType, CallBack callBack)
    {
        OnUnsubscribing(eventType, callBack);
        //If there is no exception, remove the delegate from the event code
        m_EventTable[eventType] = (CallBack)m_EventTable[eventType] - callBack;
    }

    /// <summary>
    ///Remove listener fonction/Single Parameter
    /// </summary>
    /// <param name="eventType"></eventCode>
    /// <param name="callBack"></DelegateToRemove>
    public void Unsubscribe<T>(TEvent eventType, CallBack<T> callBack)
    {
        OnUnsubscribing(eventType, callBack);
        //If there is no exception, remove the delegate from the event code
        m_EventTable[eventType] = (CallBack<T>)m_EventTable[eventType] - callBack;
    }

    /// <summary>
    ///Remove listener fonction/Double Parameters
    /// </summary>
    /// <param name="eventType"></eventCode>
    /// <param name="callBack"></DelegateToRemove>
    public void Unsubscribe<T, X>(TEvent eventType, CallBack<T, X> callBack)
    {
        OnUnsubscribing(eventType, callBack);
        //If there is no exception, remove the delegate from the event code
        m_EventTable[eventType] = (CallBack<T, X>)m_EventTable[eventType] - callBack;
    }

    /// <summary>
    ///Remove listener fonction/Three Parameters
    /// </summary>
    /// <param name="eventType"></eventCode>
    /// <param name="callBack"></DelegateToRemove>
    public void Unsubscribe<T, X, Y>(TEvent eventType, CallBack<T, X, Y> callBack)
    {
        OnUnsubscribing(eventType, callBack);
        //If there is no exception, remove the delegate from the event code
        m_EventTable[eventType] = (CallBack<T, X, Y>)m_EventTable[eventType] - callBack;
    }

    /// <summary>
    ///Remove listener fonction/Four Parameters
    /// </summary>
    /// <param name="eventType"></eventCode>
    /// <param name="callBack"></DelegateToRemove>
    public void Unsubscribe<T, X, Y, Z>(TEvent eventType, CallBack<T, X, Y, Z> callBack)
    {
        OnUnsubscribing(eventType, callBack);
        //If there is no exception, remove the delegate from the event code
        m_EventTable[eventType] = (CallBack<T, X, Y, Z>)m_EventTable[eventType] - callBack;
    }

    /// <summary>
    ///Remove listener fonction/Five Parameters
    /// </summary>
    /// <param name="eventType"></eventCode>
    /// <param name="callBack"></DelegateToRemove>
    public void Unsubscribe<T, X, Y, Z, W>(TEvent eventType, CallBack<T, X, Y, Z, W> callBack)
    {
        OnUnsubscribing(eventType, callBack);
        //If there is no exception, remove the delegate from the event code
        m_EventTable[eventType] = (CallBack<T, X, Y, Z, W>)m_EventTable[eventType] - callBack;
    }

    /// <summary>
    ///SendToListener listener fonction/No Parameters
    /// </summary>
    public void Invoke(TEvent eventType)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(eventType, out d)) //Extract the delegate in the event code from the event list and store it in delegate d. The return value is bool type, to determine whether the event code exists in the event list
        {
            CallBack callBack = d as CallBack; //Change the delegate d to the callback type
            if (callBack != null)// Determine whether the delegate is empty
            {
                callBack();//If it isn't empty, broadcast the event (using delegate)
            }
            else // If it is empty, throw an exception
            {
                throw new Exception(string.Format("SendToListener event error: event {0} corresponds to a different type of delegate", eventType));
            }
        }
        else
        {
            UnityEngine.Debug.LogWarning(string.Format("SendToListener warning: the event {0} to be broadcast does not exist", eventType));
        }
    }

    /// <summary>
    /// SendToListener listener fonction/Single Parameter
    /// </summary>
    public void Invoke<T>(TEvent eventType, T arg)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(eventType, out d)) //Extract the delegate in the event code from the event list and store it in delegate d. The return value is bool type, to determine whether the event code exists in the event list
        {
            CallBack<T> callback = d as CallBack<T>; //Change the delegate d to the callback type
            if (callback != null)//Determine whether the delegate is empty
            {
                callback(arg);//If it isn't empty, broadcast the event (using delegate)
            }
            else //If it is empty, throw an exception
            {
                throw new Exception(string.Format("SendToListener event error: event {0} corresponds to a different type of delegate", eventType));
            }
        }
        else
        {
            UnityEngine.Debug.LogWarning(string.Format("SendToListener warning: the event {0} to be broadcast does not exist", eventType));
        }
    }
    #region Broadcast
    /// <summary>
    /// SendToListener listener fonction/Double Parameters
    /// </summary>
    public void Invoke<T, X>(TEvent eventType, T arg1, X arg2)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(eventType, out d)) //Extract the delegate in the event code from the event list and store it in delegate d. The return value is bool type, to determine whether the event code exists in the event list
        {
            CallBack<T, X> callBack = d as CallBack<T, X>; //Change the delegate d to the callback type
            if (callBack != null)//Determine whether the delegate is empty
            {
                callBack(arg1, arg2);//If it isn't empty, broadcast the event (using delegate)
            }
            else //If it is empty, throw an exception
            {
                throw new Exception(string.Format("SendToListener event error: event {0} corresponds to a different type of delegate", eventType));
            }
        }
        else
        {
            UnityEngine.Debug.LogWarning(string.Format("SendToListener warning: the event {0} to be broadcast does not exist", eventType));
        }
    }
    #endregion

    /// <summary>
    /// SendToListener listener fonction/Three Parameters
    /// </summary>
    public void Invoke<T, X, Y>(TEvent eventType, T arg1, X arg2, Y arg3)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(eventType, out d)) //Extract the delegate in the event code from the event list and store it in delegate d. The return value is bool type, to determine whether the event code exists in the event list
        {
            CallBack<T, X, Y> callBack = d as CallBack<T, X, Y>; //Change the delegate d to the callback type
            if (callBack != null)//Determine whether the delegate is empty
            {
                callBack(arg1, arg2, arg3);//If it isn't empty, broadcast the event (using delegate)
            }
            else //If it is empty, throw an exception
            {
                throw new Exception(string.Format("SendToListener event error: event {0} corresponds to a different type of delegate", eventType));
            }
        }
        else
        {
            UnityEngine.Debug.LogWarning(string.Format("SendToListener warning: the event {0} to be broadcast does not exist", eventType));
        }
    }

    /// <summary>
    /// SendToListener listener fonction/Four Parameters
    /// </summary>
    public void Invoke<T, X, Y, Z>(TEvent eventType, T arg1, X arg2, Y arg3, Z arg4)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(eventType, out d)) //Extract the delegate in the event code from the event list and store it in delegate d. The return value is bool type, to determine whether the event code exists in the event list
        {
            CallBack<T, X, Y, Z> callBack = d as CallBack<T, X, Y, Z>; //Change the delegate d to the callback type
            if (callBack != null)//Determine whether the delegate is empty
            {
                callBack(arg1, arg2, arg3, arg4);//If it isn't empty, broadcast the event (using delegate)
            }
            else //If it is empty, throw an exception
            {
                throw new Exception(string.Format("SendToListener event error: event {0} corresponds to a different type of delegate", eventType));
            }
        }
        else
        {
            UnityEngine.Debug.LogWarning(string.Format("SendToListener warning: the event {0} to be broadcast does not exist", eventType));
        }
    }

    /// <summary>
    /// SendToListener listener fonction/Five Parameters
    /// </summary>
    public void Invoke<T, X, Y, Z, W>(TEvent eventType, T arg1, X arg2, Y arg3, Z arg4, W arg5)
    {
        Delegate d;
        if (m_EventTable.TryGetValue(eventType, out d)) //Extract the delegate in the event code from the event list and store it in delegate d. The return value is bool type, to determine whether the event code exists in the event list
        {
            CallBack<T, X, Y, Z, W> callBack = d as CallBack<T, X, Y, Z, W>; //Change the delegate d to the callback type
            if (callBack != null)//Determine whether the delegate is empty
            {
                callBack(arg1, arg2, arg3, arg4, arg5);//If it isn't empty, broadcast the event (using delegate)
            }
            else //If it is empty, throw an exception
            {
                throw new Exception(string.Format("SendToListener event error: event {0} corresponds to a different type of delegate", eventType));
            }
        }
        else
        {
            UnityEngine.Debug.LogWarning(string.Format("SendToListener warning: the event {0} to be broadcast does not exist", eventType));
        }
    }
}

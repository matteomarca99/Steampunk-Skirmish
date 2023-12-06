using System;
using System.Collections.Generic;

public static class EventManager
{
    // Dizionario per tenere traccia degli eventi e dei relativi sottoscrittori
    private static Dictionary<EventType, Delegate> eventListeners = new Dictionary<EventType, Delegate>();

    // Metodo per iscriversi a un evento specifico
    public static void SubscribeToEvent<T>(EventType eventName, Action<T> listener)
    {
        if (!eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName] = listener;
        }
        else
        {
            eventListeners[eventName] = Delegate.Combine(eventListeners[eventName], listener);
        }
    }

    public static void SubscribeToEvent<T, U>(EventType eventName, Action<T, U> listener)
    {
        if (!eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName] = listener;
        }
        else
        {
            eventListeners[eventName] = Delegate.Combine(eventListeners[eventName], listener);
        }
    }

    // Metodo per iscriversi a un evento senza parametri
    public static void SubscribeToEvent(EventType eventName, Action listener)
    {
        if (!eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName] = listener;
        }
        else
        {
            eventListeners[eventName] = Delegate.Combine(eventListeners[eventName], listener);
        }
    }

    // Metodo per disiscriversi da un evento specifico
    public static void UnsubscribeFromEvent<T>(EventType eventName, Action<T> listener)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName] = Delegate.Remove(eventListeners[eventName], listener);
        }
    }

    public static void UnsubscribeFromEvent<T, U>(EventType eventName, Action<T, U> listener)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName] = Delegate.Remove(eventListeners[eventName], listener);
        }
    }

    // Metodo per disiscriversi da un evento senza parametri
    public static void UnsubscribeFromEvent(EventType eventName, Action listener)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            eventListeners[eventName] = Delegate.Remove(eventListeners[eventName], listener);
        }
    }

    // Metodo per triggerare un evento
    public static void TriggerEvent<T>(EventType eventName, T eventData)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            if (eventListeners[eventName] is Action<T> eventDelegate)
            {
                eventDelegate.Invoke(eventData);
            }
        }
    }

    public static void TriggerEvent<T, U>(EventType eventName, T param1, U param2)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            if (eventListeners[eventName] is Action<T, U> eventDelegate)
            {
                eventDelegate.Invoke(param1, param2);
            }
        }
    }

    // Metodo per triggerare un evento senza parametri
    public static void TriggerEvent(EventType eventName)
    {
        if (eventListeners.ContainsKey(eventName))
        {
            if (eventListeners[eventName] is Action eventDelegate)
            {
                eventDelegate.Invoke();
            }
        }
    }
}


using System.Collections.Generic;

public partial class GodotEvent
{

    private List<GodotAction> _listeners = new List<GodotAction>();
    
    public GodotEvent()
    {
        
    }

    public virtual void AddListener(GodotAction listener)
    {
        if (listener == null)
            return;

        lock (this)
        {
            _listeners.Add(listener);
        }
    }
    
    public virtual void RemoveListener(GodotAction listener)
    {
        lock (this)
        {
            _listeners.Add(listener);
        }
    }
    
    public virtual void RemoveAllListeners()
    {
        lock (this)
        {
            _listeners.Clear();
        }
    }

    public void Invoke()
    {
        for (int i = 0; i < _listeners.Count; i++)
        {
            _listeners[i].Invoke();
        }
    }
    
}

public partial class GodotEvent<T>
{

    private List<GodotAction<T>> _listeners = new List<GodotAction<T>>();
    
    public GodotEvent()
    {
        
    }

    public virtual void AddListener(GodotAction<T> listener)
    {
        if (listener == null)
            return;

        lock (this)
        {
            _listeners.Add(listener);
        }
    }
    
    public virtual void RemoveListener(GodotAction<T> listener)
    {
        lock (this)
        {
            _listeners.Add(listener);
        }
    }
    
    public virtual void RemoveAllListeners()
    {
        lock (this)
        {
            _listeners.Clear();
        }
    }

    public void Invoke(T arg)
    {
        for (int i = 0; i < _listeners.Count; i++)
        {
            _listeners[i].Invoke(arg);
        }
    }
    
}
using UnityEngine;

public abstract class ISubject
{
    public abstract void Notify();
    public abstract void Add(IObserver obs);
    public abstract void Remove(IObserver obs);
}

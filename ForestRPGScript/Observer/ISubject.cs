public interface ISubject
{
    public void Notify();
    public void Add(IObserver obs);
    public void Remove(IObserver obs);
}
namespace try_to_make_app;

public interface IObservable
{
   void AddObserver(IObserver o);
   void RemoveObser(IObserver o);
   void Notify();
} 
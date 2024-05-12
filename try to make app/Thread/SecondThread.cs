using System;
using System.Linq;
using System.Windows.Threading;

namespace try_to_make_app.Thread;

public class SecondThread
{
    private MainWindow _window;

    public SecondThread(MainWindow window)
    {
        _window = window;
    }

    public void Main()
    {
        while (true)
        {
            DateTime dateTime = _window.DayViewModel.today;
            Dispatcher.CurrentDispatcher.Invoke(() => { _window.DayViewModel.UpdateList(1); });
            if ( dateTime != DateTime.Today)
            {
                Dispatcher.CurrentDispatcher.Invoke(() => { _window.database.NewDay(); });
            }
            System.Threading.Thread.Sleep(1000);
        }
    }
    
}
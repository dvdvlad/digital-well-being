using System;
using System.Linq;
using System.Windows;
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
            if (_window.DayViewModel.today == dateTime)
            {
                Dispatcher.CurrentDispatcher.Invoke(() => { _window.database.NewDay(); });
                // Dispatcher.CurrentDispatcher.Invoke(() => _window.DayViewModel = _window.database.DayViewModels.Last() );
            }
            System.Threading.Thread.Sleep(1000);
        }
    }
    
}
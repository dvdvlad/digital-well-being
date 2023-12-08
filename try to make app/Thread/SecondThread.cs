using System;
using System.Dynamic;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using try_to_make_app.Database_things;
using try_to_make_app;
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
            Dispatcher.CurrentDispatcher.Invoke(() => { _window.DayViewModel.UpdateList(); });
            System.Threading.Thread.Sleep(10000);
        }
        
    }
}
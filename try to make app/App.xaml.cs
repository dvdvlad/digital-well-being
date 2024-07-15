using System;
using System.Linq;
using System.Threading;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using try_to_make_app.Database_things;
using Thread = System.Threading.Thread;

namespace try_to_make_app
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [STAThread]
        public static void Main()
        {
            App app = new App();
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Database.EnsureCreated();
            }
            SecondThread secondThread = new SecondThread();
            Thread SecondThread = new Thread(secondThread.MainTwo){IsBackground = true};
            SecondThread.Start();
            MainWindow window = new MainWindow();
            app.Run(window);
            
        }
    }
}
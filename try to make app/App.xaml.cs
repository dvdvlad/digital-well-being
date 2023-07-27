using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using try_to_make_app;
namespace try_to_make_app
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    
    public partial class App : Application
    {
        [STAThread]
        static void Main()
        {
            App app = new App();
            try_to_make_app.MainWindow window = new MainWindow();
            window.Title = "tetst";
            app.Run(window);
                
        }
    }
}
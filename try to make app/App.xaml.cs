using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ScottPlot;
using try_to_make_app;
using try_to_make_app.Thread;

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
            try_to_make_app.MainWindow window = new MainWindow();
            app.Run(window);
        }
    }
}
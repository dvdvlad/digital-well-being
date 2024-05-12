using System;
using System.Windows;

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
            MainWindow window = new MainWindow();app.Run(window);
        }
    }
}
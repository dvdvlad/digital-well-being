using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ScottPlot;
using ScottPlot.Plottable;
using try_to_make_app.Database_things;
using try_to_make_app.Thread;
using Color = System.Drawing.Color;

namespace try_to_make_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        object locker  = new ();
        private static string path = $"{Environment.CurrentDirectory}\\AppDatabase.json";
        Save save = new Save();
        public AppViewModel appViewModel = new AppViewModel();

        delegate void SaveDelegate();
        
        public MainWindow()
        {   
            InitializeComponent();
            lock (locker)
            {
                DataContext = appViewModel;
            }

            double[] valuesCirculeDiograma = { 1, 2, 3, 45 };
            CirculeDioagram.Plot.Frameless();
            PiePlot Chart  = CirculeDioagram.Plot.AddPie(valuesCirculeDiograma);
            Chart.Explode = true;
            Chart.DonutSize = .6;
            Chart.Size = 0.75;

            double[] valuesDiograma =  { 13,12,21,11,4,2,7};
            double[] Day_of_The_Week = { 1, 2,3,4,5,6,7};
            string[] Day_of_The_Week_String = { "Monday","Thuesday", "Wensday","Thursday","Friday","Saturday","Sunday"};
            Dioagram.Plot.AddBar(valuesDiograma,Day_of_The_Week);
            Dioagram.Plot.XTicks(Day_of_The_Week,Day_of_The_Week_String);
            
            SecondThread secondThread = new SecondThread();
            System.Threading.Thread SaveThread = new System.Threading.Thread(Save) { IsBackground = true };
            SaveThread.Start();
            

        }

        public void Save()
        {   
            while (true)
            {
                System.Threading.Thread.Sleep(10000);   
                this.Dispatcher.Invoke(() => {save.SaveDatabase(appViewModel);});
                

            }
        }

        private void SaveCallback()
        {
            SaveDelegate d = new SaveDelegate(SaveCallback);
            Dispatcher.Invoke(d);
        }
        
    }
}
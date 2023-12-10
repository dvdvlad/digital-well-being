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
using ScottPlot.Renderable;
using try_to_make_app.Database_things;
using try_to_make_app.Thread;
using Color = System.Drawing.Color;

namespace try_to_make_app
{
    public partial class MainWindow : Window
    {
        object locker  = new ();
        private static string path = $"{Environment.CurrentDirectory}\\AppDatabase.json";
        Save save = new Save();
        public DayViewModel DayViewModel = new DayViewModel();
        public Database database = Save.LoadDatabase();
        public MainWindow()
        {
            if (database.AppViewModels != null)
            {
                
            }
            
            InitializeComponent();
            
            DayViewModel.Apps = database.AppViewModels.LastOrDefault().Apps;
            DayViewModel.PropertyChanged += DayViewModelOnPropertyChanged;
            
            lock (locker)
            {
                DataContext = DayViewModel;
            }
            
            double[] valuesDiograma =  { 13,12,21,11,4,2,7};
            double[] Day_of_The_Week = { 1, 2,3,4,5,6,7};
            string[] Day_of_The_Week_String = { "Monday","Thuesday", "Wensday","Thursday","Friday","Saturday","Sunday"};
            Dioagram.Plot.AddBar(valuesDiograma,Day_of_The_Week);
            Dioagram.Plot.XTicks(Day_of_The_Week,Day_of_The_Week_String);
            Thread.SecondThread secondThread = new SecondThread(this);
            System.Threading.Thread SecondThread = new System.Threading.Thread(secondThread.Main) { IsBackground = true };
            SecondThread.Start();
            

        }
        
        private void DayViewModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() => { AppViewModelOnPropertyChangedMethod(); });
        }

        private void AppViewModelOnPropertyChangedMethod()
        {
            if (DayViewModel.Apps != null)
            {
                List<double> CirculeDay = new List<double>();
                List<string> CirculeLabels = new List<string>();
                WrapPanel.Children.Clear();
                foreach (var app in DayViewModel.Apps)
                {
                    Button button = new Button();
                    button.Content = app.Name;
                    Thickness thickness = new Thickness(5);
                    button.Margin = thickness;
                    WrapPanel.Children.Add(button);
                    CirculeDay.Add(app.WorkTimeToDay);
                    CirculeLabels.Add(app.Name);
                }

                double[] valuesCirculeDiogram =CirculeDay.ToArray();
                string[] labelsCirculeDiogram = CirculeLabels.ToArray();
                PiePlot Chart  = CirculeDioagram.Plot.AddPie(valuesCirculeDiogram);
                Chart.ShowLabels = true;
                Chart.SliceLabels = labelsCirculeDiogram;
                Chart.SliceFont.Size = 10;
                CirculeDioagram.Configuration.Pan = false;
                CirculeDioagram.Refresh();
                
            }
        }


        private void MainWindow_OnClosed(object? sender, EventArgs e)
        {
            Save.SaveDatabase(database);
        }
    }
}
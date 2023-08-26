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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        object locker  = new ();
        private static string path = $"{Environment.CurrentDirectory}\\AppDatabase.json";
        Save save = new Save();
        private AppViewModel appViewModel = new AppViewModel();

        delegate void SaveDelegate();
        
        public MainWindow()
        {   
            InitializeComponent();
            lock (locker)
            {
                DataContext = appViewModel;
            }
            appViewModel.Apps = save.LoadDatabase();
            appViewModel.Apps.CollectionChanged += AppsOnCollectionChanged;
            
            // CirculeDioagram.Plot.Frameless();

            double[] valuesDiograma =  { 13,12,21,11,4,2,7};
            double[] Day_of_The_Week = { 1, 2,3,4,5,6,7};
            string[] Day_of_The_Week_String = { "Monday","Thuesday", "Wensday","Thursday","Friday","Saturday","Sunday"};
            Dioagram.Plot.AddBar(valuesDiograma,Day_of_The_Week);
            Dioagram.Plot.XTicks(Day_of_The_Week,Day_of_The_Week_String);

            ObservableCollection<AppModel> appModelsCollection = save.LoadDatabase();
            CirculeDioagram.Refresh();
            SecondThread secondThread = new SecondThread();
            System.Threading.Thread SecondThread = new System.Threading.Thread(ThreadTwo) { IsBackground = true };
            SecondThread.Start();
            
        }
        public void ThreadTwo()
        {   
            while (true)
            {
                this.Dispatcher.Invoke(() => { appViewModel.UpdateList(); });
                this.Dispatcher.Invoke(() => {save.SaveDatabase(appViewModel);});
                appViewModel.PropertyChanged += AppViewModelOnPropertyChanged;
                appViewModel.Apps.CollectionChanged += AppsOnCollectionChanged;
                System.Threading.Thread.Sleep(10000);
            }
        }

        private void AppViewModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (appViewModel.Apps != null)
            {
                List<double> CirculeDay = new List<double>();
                List<string> CirculeLabels = new List<string>();
                WrapPanel.Children.Clear();
                foreach (var app in appViewModel.Apps)
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
                Chart.SliceLabels = labelsCirculeDiogram;
                Chart.ShowLabels = true;
                Chart.SliceFont.Size = 10;
                
                CirculeDioagram.Render();
                CirculeDioagram.Refresh();
                
            }
        }

        private void AppsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (appViewModel.Apps != null)
                {
                    List<double> CirculeDay = new List<double>();
                    List<string> CirculeLabels = new List<string>();
                    WrapPanel.Children.Clear();
                    foreach (var app in appViewModel.Apps)
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
                    Chart.SliceLabels = labelsCirculeDiogram;
                    Chart.ShowLabels = true;
                    Chart.SliceFont.Size = 13;
                
                    CirculeDioagram.Render();
                    CirculeDioagram.Refresh();
                
                }
            }
        }
    }
}
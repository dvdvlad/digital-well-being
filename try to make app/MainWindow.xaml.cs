using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ScottPlot.Plottable;
using try_to_make_app.Database_things;
using try_to_make_app.Thread;

namespace try_to_make_app
{
    public partial class MainWindow : Window
    {
        object locker = new();
        private static string path = $"{Environment.CurrentDirectory}\\AppDatabase.json";
        Save save = new Save();
        public DayViewModel DayViewModel = new DayViewModel();
        public Database database = Save.LoadDatabase();
        public MainWindow()
        {
            if (database.DayViewModels == null)
            {
                DayViewModel.UpdateList(0);
                database.DayViewModels.Add(DayViewModel);
            }
            database.DayViewModels.Last().UpdateList(0);
            InitializeComponent();

            DayViewModel.Apps = database.DayViewModels.LastOrDefault().Apps;
            DayViewModel.PropertyChanged += DayViewModelOnPropertyChanged;
            DayViewModel.Apps.CollectionChanged += AppsOnCollectionChanged;

            lock (locker)
            {
                DataContext = DayViewModel;
            }
            SecondThread secondThread = new SecondThread(this);
            System.Threading.Thread SecondThread = new System.Threading.Thread(secondThread.Main)
                { IsBackground = true };
            SecondThread.Start();
            

        }

        private void AppsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() => { AppViewModelOnPropertyChangedMethod(); });
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

                double[] valuesCirculeDiogram = CirculeDay.ToArray();
                string[] labelsCirculeDiogram = CirculeLabels.ToArray();
                PiePlot Chart = CirculeDioagram.Plot.AddPie(valuesCirculeDiogram);
                Chart.ShowLabels = true;
                Chart.SliceLabels = labelsCirculeDiogram;
                Chart.SliceFont.Size = 10;
                CirculeDioagram.Configuration.Pan = false;
                CirculeDioagram.Refresh();


                if (Dioagram != null)
                {
                    Dioagram.Plot.Clear();
                }
                var _dayViewModels = database.DayViewModels.ToArray();
                double[] valuesDiograma = new double[7]; // массив для хранения последних 7 элементов

                int startIndex = Math.Max(0, _dayViewModels.Length - 7); // начальный индекс для загрузки последних 7 элементов

                for (int i = 0; i < 7; i++)
                {
                    int sourceIndex = startIndex + i;
                    if (sourceIndex < _dayViewModels.Length && _dayViewModels[sourceIndex] != null)
                    {
                        valuesDiograma[i] = _dayViewModels[sourceIndex].UsadgeTimeToDay;
                    }
                    else
                    {
                        valuesDiograma[i] = 0; // если элемент равен null, установить значение по умолчанию
                    }
                }
                double[] Day_of_The_Week = { 1, 2, 3, 4, 5, 6, 7 };
                string[] Day_of_The_Week_String = { "Пн", "Вт", "Ср", "Чт", "Пт", "Суб", "Вс" };
                Dioagram.Plot.AddBar(valuesDiograma, Day_of_The_Week, color:Color.Blue);
                Dioagram.Plot.XTicks(Day_of_The_Week, Day_of_The_Week_String);
                
                Dioagram.Render();
                
            }
        }


        private void MainWindow_OnClosed(object? sender, EventArgs e)
        {
            Save.SaveDatabase(database);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
using ScottPlot;
using ScottPlot.Plottable;
using Color = System.Drawing.Color;

namespace try_to_make_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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

            List<Database_things.App> App = new List<Database_things.App>();
            
            
            List<string> Apps = new List<string>() { "Telegram","Edge", "warframe","vs code","vivaldi","sumblime","obsidian","wallpaper engin"};
            foreach (string App in Apps)
            {
                Button button = new Button();
                button.Content = App;
                button.Width = 100;
                button.Height = 20;
                WrapPanel.Children.Add(button);
            }
        }   
    }
}
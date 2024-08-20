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

namespace try_to_make_app
{
    public partial class MainWindow : Window
    {
        public List<double> PieValues { get; set; }
        public List<string> PieLabels {get; set;}
        public MainWindow()
        {
            InitializeComponent();
            PieValues = new List<double>() { 6.3, 3.1, 3.2 };
            PieLabels = new List<string>() { "test1", "test2", "test3"};
            DataContext = this;
        }
    }
}
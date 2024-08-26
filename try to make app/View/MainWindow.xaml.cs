﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ScottPlot.Plottable;
using try_to_make_app.Database_things;
using try_to_make_app.ViewModel;

namespace try_to_make_app
{
    public partial class MainWindow : Window
    {
        public MainWindow(DataWorker dataWorker)
        {
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(dataWorker);
            InitializeComponent();
            DataContext = mainWindowViewModel;
        }
    }
}
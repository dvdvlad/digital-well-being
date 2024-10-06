using System;
using System.Collections.Generic;
using System.Windows.Controls;
using try_to_make_app.ViewModel;

namespace try_to_make_app.View;

public partial class AppWindow : UserControl
{
    private AppWindowViewModel AppWindowViewModel; 
    public AppWindow(AppWindowViewModel appWindowViewModel)
    {
        InitializeComponent();
        this.DataContext = AppWindowViewModel;
    }
}
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using try_to_make_app.ViewModel;

namespace try_to_make_app.View;

public partial class AppWindow : UserControl
{
    private AppWindowViewModel AppWindowViewModel;

    public AppWindow(AppWindowViewModel appWindowViewModel)
    {
        AppWindowViewModel = appWindowViewModel;
        this.DataContext = AppWindowViewModel;
        InitializeComponent();
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (DataContext is AppWindowViewModel appWindowViewModel)
        {
            appWindowViewModel.TextCHTimer.Stop();
            appWindowViewModel.TextCHTimer.Start();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using try_to_make_app.Database_things;

namespace try_to_make_app.View;

public partial class AppsView : UserControl
{
    public static readonly DependencyProperty AppsProperty = DependencyProperty.Register(
        nameof(AppModels), typeof(ObservableCollection<AppModel>), typeof(AppsView),
        new FrameworkPropertyMetadata(new ObservableCollection<AppModel>(), FrameworkPropertyMetadataOptions.AffectsArrange, OnvaluesChanged));
    public ObservableCollection<AppModel> AppModels
    {
        get => (ObservableCollection<AppModel>)GetValue(AppsProperty);
        set => SetValue(AppsProperty, value);
    }

    public AppsView()
    {
        InitializeComponent();
    }

    private void DrawApps(ICollection<AppModel> appModels)
    { 
        Panel.Children.Clear();
        foreach (var app in appModels)
        {
            Panel.Children.Add(new Button{Content = app.Name});
        }
    }
    private static void OnvaluesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is ICollection<AppModel> apps)
        {
            Console.WriteLine(apps.Count);
            if (d is AppsView appsView)
            {
               appsView.DrawApps(apps); 
            } 
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using try_to_make_app.Database_things;
using try_to_make_app.ViewModel;

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
    private static void OnvaluesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {

    }
}
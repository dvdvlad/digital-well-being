using System.Collections.ObjectModel;
using System.Windows;
using try_to_make_app.Database_things;
using try_to_make_app.ViewModel;

namespace try_to_make_app.View;

public partial class AppsView
{
    public static readonly DependencyProperty AppsProperty = DependencyProperty.Register(
        nameof(AppModels), typeof(ObservableCollection<AppModel>), typeof(AppsView),
        new FrameworkPropertyMetadata(new ObservableCollection<AppModel>(),
            FrameworkPropertyMetadataOptions.AffectsArrange, OnvaluesChanged));

    public ObservableCollection<AppModel> AppModels
    {
        get => (ObservableCollection<AppModel>)GetValue(AppsProperty);
        set => SetValue(AppsProperty, value);
    }

    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        nameof(ButtonAppComand), typeof(RelayComand), typeof(AppsView),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange, OnvaluesChanged));



    public RelayComand ButtonAppComand
    {
        get => (RelayComand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }



    public AppsView()
    {
        InitializeComponent();
    }

    public AppsView(RelayComand appComand, ObservableCollection<AppModel> appModels)
    {
        AppModels = appModels;
        ButtonAppComand = appComand;

        InitializeComponent();
    }

    private static void OnvaluesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
    }
}
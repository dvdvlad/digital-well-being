using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Extensions.DependencyModel;
using try_to_make_app.ViewModel;

namespace try_to_make_app.View;

public partial class CustomButon : UserControl, INotifyPropertyChanged
{
    public static readonly DependencyProperty RotationAngelProperty = DependencyProperty.Register(
        nameof(RotationAngel), typeof(double), typeof(CustomButon),
        new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange, PropertyChangedCallback)
    );

    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        nameof(Command), typeof(ICommand), typeof(CustomButon),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange, PropertyChangedCallback)
    );


    private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var customButton = d as CustomButon;
        if (customButton != null && e.NewValue is double)
        {
        }
    }


    public double RotationAngel
    {
        get => (double)GetValue(RotationAngelProperty);
        set
        {
            SetValue(RotationAngelProperty, value);
            OnPropertyChanged("Angel");
        }
    }

    public ICommand Command
    {
        get => (RelayComand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public CustomButon()
    {
        InitializeComponent();
        
    }

    private void Button_OnClick(object sender, RoutedEventArgs e)
    {
        Console.WriteLine("исполняем команду");
        // if (Command != null && Command.CanExecute(null))
        // {
        //     Command.Execute(null); 
        // }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged(string prop = "")
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
    }
}
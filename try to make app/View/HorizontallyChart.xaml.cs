using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace try_to_make_app.View;

public partial class HorizontallyChart : UserControl
{
    public static readonly DependencyProperty ValuesProperty = DependencyProperty.Register(
        nameof(Values), typeof(List<double>), typeof(HorizontallyChart),
        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange, OnvaluesChanged));

    public static readonly DependencyProperty LabelsProperty =
        DependencyProperty.Register(
            nameof(Labels), typeof(List<string>), typeof(HorizontallyChart),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsArrange, OnvaluesChanged));

    private List<double> Values
    {
        get => (List<double>)GetValue(ValuesProperty);
        set => SetValue(ValuesProperty, value);
    }

    private List<string> Labels
    {
        get => (List<string>)GetValue(LabelsProperty);
        set => SetValue(LabelsProperty, value);
    }

    public HorizontallyChart()
    {
        InitializeComponent();
        List<string> label = new List<string> { "0", "2", "4", "6" };
        Labels = label;
    }

    void DrawChart()
    {
        Canvas.Children.Clear();
        Rectangle bckground =  DrawBackground();
        double lheight = (bckground.Height / Labels.Count);
        DrawLabels(lheight, bckground.Width,bckground.Height);
    }

    private Rectangle DrawBackground()
    {
        Rectangle BackgroundRectangle = new Rectangle();
        if (Canvas.ActualWidth >= 25 && Canvas.ActualHeight >= 25)
        {
            BackgroundRectangle.Width = Canvas.ActualWidth - 25;
            BackgroundRectangle.Height = Canvas.ActualHeight - 25;
        }

        BackgroundRectangle.RadiusX = 15.0;
        BackgroundRectangle.RadiusY = 15.0;
        BackgroundRectangle.Fill = Brushes.Black;
        BackgroundRectangle.Margin = new Thickness(10);
        Canvas.Children.Add(BackgroundRectangle);
        return BackgroundRectangle;
    }

    private void DrawLabels(double Lheight, double BGWidth,double BGHeight)
    {
        
        double LPreviousHeight = BGHeight * 0.20;;
        foreach (string label in Labels)
        {
            TextBlock Label = new TextBlock
            {
                Text = label,
                Foreground = Brushes.White,
                FontSize = 15,
            };
            Canvas.Children.Add(Label);
            Canvas.SetBottom(Label, LPreviousHeight);
            Line StaticLine = new Line();
            StaticLine.X1 = 15;
            if (!double.IsNaN(BGWidth))
            {
                StaticLine.X2 = BGWidth;
            }

            if (!double.IsNaN(LPreviousHeight))
            {
                StaticLine.Y1 = LPreviousHeight;
                StaticLine.Y2 = LPreviousHeight;
            }

            StaticLine.Stroke = Brushes.White;
            Canvas.Children.Add(StaticLine);
            LPreviousHeight += Lheight;
        }
    }

    private void MainGrid_OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        DrawChart();
    }

    private static void OnvaluesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is HorizontallyChart chart)
        {
            chart.DrawChart();
        }
    }
}
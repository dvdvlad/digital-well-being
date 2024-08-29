using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
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

    public List<double> Values
    {
        get
        {
            List<double> _data = (List<double>)GetValue(ValuesProperty);
            if (_data != null)
            {
                List<double> data = _data.Select(a => a).ToList();
                for (int i = 0; i < data.Count; i++)
                {
                    data[i] = data[i]/360.0;
                }

                return data;
            }

            return _data;
        }
        set => SetValue(ValuesProperty, value);
    }

    public List<string> Labels
    {
        get => (List<string>)GetValue(LabelsProperty);
        set => SetValue(LabelsProperty, value);
    }

    public HorizontallyChart()
    {
        InitializeComponent();
    }

    void DrawChart()
    {
        Canvas.Children.Clear();
        Rectangle bckground = DrawBackground();
        List<string> VertLabels = new List<string>();
        if (Values != null)
        {
            VertLabels = GenerateLabels(Values);
        }

        double lheight;
        double lwidth;
        if (!double.IsNaN(bckground.Width) && !double.IsNaN(bckground.Height))
        {
            lheight = (bckground.Height / VertLabels.Count);
            lwidth = (bckground.Width / 8);
        }
        else
        {
            lheight = 0;
            lwidth = 0;
        }

        DrawVertLabels(lwidth, bckground.Width, bckground.Height, VertLabels);
        DrawHorizLabels(lwidth, bckground.Width, bckground.Height);
        DrawColumns(lwidth, bckground.Height, bckground.Width, Values, VertLabels);
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

    private List<string> GenerateLabels(List<double> data)
    {
        List<string> labels = new List<string>();
        if (data.Count() > 0)
        {
            double startprocen = 0;
            for (int i = 0; startprocen <= 1; i++)
            {
                labels.Add(Math.Ceiling(data.Max() * startprocen).ToString());
                startprocen += 0.25;
            }
        }

        return labels;
    }

    private void DrawVertLabels(double LWidth, double BGWidth, double BGHeight, List<string> labels)
    {
        double LPreviousHeight = BGHeight * 0.10;
        double LHeight = BGHeight / labels.Count();
        for (int i = 0; i < labels.Count; i++)
        {
            TextBlock VerticalLabels = new TextBlock
            {
                Text = labels[i],
                Foreground = Brushes.White,
                FontSize = 15,
            };
            Canvas.Children.Add(VerticalLabels);
            Canvas.SetBottom(VerticalLabels, LPreviousHeight);
            System.Windows.Controls.Canvas.SetLeft(VerticalLabels, 10);
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
            Canvas.SetBottom(StaticLine, LPreviousHeight);
            LPreviousHeight += LHeight;
        }
    }

    private void DrawHorizLabels(double lWith, double BGWidth, double BGHeight)
    {
        List<string> HorizLabels = Labels;
        double LpreviousWidth = BGWidth * 0.15;
        if (HorizLabels == null)
        {
            return;
        }

        for (int i = 0; i < HorizLabels.Count; i++)
        {
            TextBlock HorzintalLabel = new TextBlock
            {
                Text = HorizLabels[i],
                Foreground = Brushes.White,
                FontSize = 15,
            };


            Canvas.Children.Add(HorzintalLabel);
            Canvas.SetBottom(HorzintalLabel, BGHeight * 0.05);
            Canvas.SetLeft(HorzintalLabel, LpreviousWidth);

            LpreviousWidth += lWith;
        }
    }

    private void DrawColumns(double CWidth, double BGHeight, double BGWidth, List<double> Data, List<string> VertLabels)
    {
        if (Data != null)
        {
            double CSettWith = BGWidth * 0.15;
            foreach (var data in Data)
            {
                double CHeight = BGHeight * 0.80 * (data / Data.Max());
                double ColumWidth = (BGWidth / Data.Count) - (BGWidth * 0.05);
                Rectangle Columns = new Rectangle
                {
                    Height = CHeight,
                    Width = ColumWidth,
                    Fill = Brushes.DarkSlateGray,
                    RadiusX = 10,
                    RadiusY = 10
                };
                Canvas.Children.Add(Columns);
                Canvas.SetLeft(Columns, CSettWith - (Columns.Width / 2.5));
                Canvas.SetBottom(Columns, BGHeight * 0.1);
                CSettWith += CWidth;
            }
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
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace try_to_make_app.View;

public partial class CirculeChart : UserControl
{
    public static readonly DependencyProperty ValuesProperty =
        DependencyProperty.Register(
            nameof(Values), typeof(List<double>), typeof(CirculeChart),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnValuesChanged));

    public static readonly DependencyProperty LabelsProperty =
        DependencyProperty.Register(
            nameof(Labels), typeof(List<string>), typeof(CirculeChart),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnValuesChanged));

    public List<double> Values
    {
        get => (List<double>)GetValue(ValuesProperty);
        set => SetValue(ValuesProperty, value);
    }

    public List<string> Labels
    {
        get => (List<string>)GetValue(LabelsProperty);
        set => SetValue(LabelsProperty, value);
    }

    public CirculeChart()
    {
        InitializeComponent();
        PieCanvas.SizeChanged += SizeChanged;
    }

    private void SizeChanged(object sender, SizeChangedEventArgs e)
    {
        DrawPieChart();
    }

    private static void OnValuesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is CirculeChart control)
        {
            control.DrawPieChart();
        }
    }

    private void DrawPieChart()
    {
        if (Values == null || Values.Count == 0)
        {
            return;
        }

        List<Brush> brushes = new List<Brush>
        {
            Brushes.Red,
            Brushes.Green,
            Brushes.Blue
        };
        double total = 0;
        foreach (var value in Values)
        {
            total += value;
        }

        double angle = 0;
        double centerX = PieCanvas.ActualWidth / 2;
        double centerY = PieCanvas.ActualHeight / 2;
        double radius = Math.Min(centerX, centerY);
        PieCanvas.Children.Clear();
        for (int i = 0; i < Values.Count; i++)
        {
            double sliceAngle = Values[i] / total * 360;
            PathFigure pathFigure = new PathFigure
            {
                StartPoint = new Point(centerX, centerY)
            };

            double x = centerX + radius * Math.Cos(angle * Math.PI / 180);
            double y = centerY + radius * Math.Sin(angle * Math.PI / 180);

            pathFigure.Segments.Add(new LineSegment(new Point(x, y), true));

            angle += sliceAngle;
            x = centerX + radius * Math.Cos(angle * Math.PI / 180);
            y = centerY + radius * Math.Sin(angle * Math.PI / 180);

            pathFigure.Segments.Add(new ArcSegment(new Point(x, y), new Size(radius, radius),
                sliceAngle, sliceAngle > 180, SweepDirection.Clockwise, true));
            pathFigure.Segments.Add(new LineSegment(new Point(centerX, centerY), true));

            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);

            Path path = new Path
            {
                Fill = brushes[i],
                Data = pathGeometry
            };

            PieCanvas.Children.Add(path);
            // Добавление меток
            double labelAngle = angle - sliceAngle / 2;
            double labelradius = radius + 20;
            double labelX = centerX + labelradius * Math.Cos(labelAngle * Math.PI / 180);
            double labelY = centerY + labelradius * Math.Sin(labelAngle * Math.PI / 180);
            double ActualFontSize = 10;
            if (PieCanvas.ActualHeight != null && PieCanvas.ActualHeight > 0)
            {
                ActualFontSize = PieCanvas.ActualHeight * 0.05;
            }

            TextBlock label = new TextBlock
            {
                Text = Labels[i],
                Foreground = Brushes.Black,
                FontSize = ActualFontSize
            };
            if (labelAngle < 180)
            {
                Canvas.SetLeft(label, labelX);
                Canvas.SetTop(label, labelY);
            }
            else
            {
                Canvas.SetLeft(label, labelX);
                Canvas.SetBottom(label, PieCanvas.ActualHeight - labelY + (PieCanvas.ActualHeight / 13));
            }

            PieCanvas.Children.Add(label);
        }
    }
}
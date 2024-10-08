using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
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
            Brushes.Blue,
            Brushes.Coral,
            Brushes.Aqua,
            Brushes.Fuchsia,
            Brushes.Yellow,
            Brushes.Red,
            Brushes.Green,
            Brushes.Blue,
            Brushes.Coral,
            Brushes.Aqua,
            Brushes.Fuchsia,
            Brushes.Yellow,
            
        };
        double total = 0;
        foreach (var value in Values)
        {
            total += value;
        }

        double angle = 0;
        double centerX = PieCanvas.ActualWidth / 2;
        double centerY = PieCanvas.ActualHeight / 2;
        double radius = Math.Max(Math.Min(centerX, centerY) - 40,0);
        double ActualFontSize = 10;
        if (PieCanvas.ActualHeight != null && PieCanvas.ActualHeight > 0)
        {
            ActualFontSize = PieCanvas.ActualHeight * 0.05;
        }

        PieCanvas.Children.Clear();
        for (int i = 0; i < Values.Count && i < Labels.Count; i++)
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
            double labelOffset = radius * 0.15;
            double labelAngle = angle - sliceAngle / 2;
            double labelRadius = radius + labelOffset; // Учитываем отступ как процент от радиуса
            double labelX = centerX + labelRadius * Math.Cos(labelAngle * Math.PI / 180);
            double labelY = centerY + labelRadius * Math.Sin(labelAngle * Math.PI / 180);

            TextBlock label = new TextBlock
            {
                Text = Labels[i],
                Foreground = Brushes.Black,
                FontSize = ActualFontSize,
                // RenderTransform = new TranslateTransform(-0.5, -0.5)
            };
            label.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            label.Arrange(new Rect(label.DesiredSize));
            double adjustedX = labelX - (label.ActualWidth / 2);
            double adjustedY = labelY - (label.ActualHeight / 2);
            if (adjustedX < 0)
            {
                adjustedX = 0;
            }
            else if (adjustedX + label.ActualWidth > PieCanvas.ActualWidth)
            {
                adjustedX = PieCanvas.ActualWidth - label.ActualWidth;
            }

            if (adjustedY < 0)
            {
                adjustedY = 0;
            }
            else if (adjustedY + label.ActualHeight > PieCanvas.ActualHeight)
            {
                adjustedY = PieCanvas.ActualHeight - label.ActualHeight;
            }

            Canvas.SetLeft(label, adjustedX);
            Canvas.SetTop(label, adjustedY);

            PieCanvas.Children.Add(label);
        }

        EllipseGeometry center = new EllipseGeometry(new Point(centerX, centerY), radius * 0.9, radius * 0.9);
        PathGeometry centerPathGeometry = new PathGeometry();
        centerPathGeometry.AddGeometry(center);
        Path centerPath = new Path
        {
            Fill = Brushes.LightSlateGray,
            Data = centerPathGeometry
        };
        PieCanvas.Children.Add(centerPath);
        double Hours = Math.Round(Values.Sum(),2);
        TextBlock DayTimeLabel = new TextBlock
        {
            Text = $"Сегодня:{Hours}ч",
            Foreground = Brushes.Black,
            FontSize = ActualFontSize * 1.5,
        };
        PieCanvas.Children.Add(DayTimeLabel);
        DayTimeLabel.UpdateLayout();
        Canvas.SetLeft(DayTimeLabel, centerX - (DayTimeLabel.ActualWidth / 2));
        Canvas.SetTop(DayTimeLabel, centerY - (DayTimeLabel.ActualHeight / 2));
    }
}
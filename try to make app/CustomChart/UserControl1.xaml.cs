using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace try_to_make_app.CustomChart
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
            double[] data = { 1.0, 2.0, 3.0, 5.0 };
            var sum = data.Sum();
            var fg = MainWindow.ActualWidthProperty.ToString();
            var angles = data.Select(d => d * 2.0 * Math.PI / sum);
            var radius = 80.0;
            var startAngle = 0.0;

            var centerPoint = new Point(radius, radius);
            var xyradius = new Size(radius, radius);

            foreach (var angle in angles)
            {
                var endAngle = startAngle + angle;

                var startPoint = centerPoint;
                startPoint.Offset(radius * Math.Cos(startAngle), radius * Math.Sin(startAngle));

                var endPoint = centerPoint;
                endPoint.Offset(radius * Math.Cos(endAngle), radius * Math.Sin(endAngle));

                var angleDeg = angle * 180.0 / Math.PI;

                Path p = new Path()
                {
                    Stroke = Brushes.Black,
                    Fill = Brushes.Red,
                    Data = new PathGeometry(
                        new PathFigure[]
                        {
                new PathFigure(
                    centerPoint,
                    new PathSegment[]
                    {
                        new LineSegment(startPoint, isStroked: true),
                        new ArcSegment(endPoint, xyradius,
                                       angleDeg, angleDeg > 180,
                                       SweepDirection.Clockwise, isStroked: true)
                    },
                    closed: true)
                        })
                };
                test.Children.Add(p);

                startAngle = endAngle;
            }
        }
    }
}

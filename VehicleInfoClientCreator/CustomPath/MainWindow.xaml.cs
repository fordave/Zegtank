using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CustomPath
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Brush brush = Brushes.Red;

        private const int _multi = 20;

        //private readonly Point _startPoint = new Point(100, 100);

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            var pathGeometry = new PathGeometry();
            var path = new Path() { Stroke = Brushes.Black, StrokeThickness = 1 ,Fill=Brushes.LightGreen};

            var _startPoint = new Point(new Random(DateTime.Now.Second).Next(100, 200), new Random(DateTime.Now.Second).Next(100, 200));

            var figure1 = new PathFigure() { IsClosed = true, IsFilled = true, StartPoint = _startPoint };

            var point1 = _startPoint + new Vector(1, 0) * _multi / 4 * 3;
            figure1.Segments.Add(new LineSegment(point1, isStroked: true));

            var point2 = point1 + new Vector(0, 1) * _multi / 4;
            figure1.Segments.Add(new LineSegment(point2, isStroked: true));

            var point3 = point2 + new Vector(0, 1) * _multi / 2;

            figure1.Segments.Add(new ArcSegment(point3, new Size(_multi / 4, _multi / 4), -90, true, SweepDirection.Clockwise, isStroked: true));

            var point4 = point3 + new Vector(0, 1) * _multi / 4;
            figure1.Segments.Add(new LineSegment(point4, isStroked: true));

            var point5 = point4 + new Vector(-1, 0) * _multi / 4 * 3;
            figure1.Segments.Add(new LineSegment(point5, isStroked: true));

            pathGeometry.Figures.Add(figure1);

            var point6 = _startPoint + new Vector(0.2, 0.5) * _multi;

            var figure2 = new PathFigure() { IsClosed = true, IsFilled = true, StartPoint = point6 };
            var point7 = point6 + new Vector(0.35, 0) * _multi;
            figure2.Segments.Add(new LineSegment(point7, isStroked: false));
            //figure2.Segments.Add(new ArcSegment(point7, new Size(5, 2), -90, true, SweepDirection.Clockwise, isStroked: true));
            pathGeometry.Figures.Add(figure2);

            var figure3 = new PathFigure { IsClosed = true, IsFilled = true, StartPoint = point7 };
            figure3.Segments.Add(new PolyLineSegment(new List<Point> { point7, point7 + new Vector(0, -0.15) * _multi, point7 + new Vector(0.3, 0) * _multi, point7 + new Vector(0, 0.15) * _multi }, isStroked: true));
            pathGeometry.Figures.Add(figure3);

            var rotatePoint = _startPoint + _multi * new Vector(1, 1) / 2;
            var rotateTransform = new RotateTransform(new Random(DateTime.Now.Second).Next(0, 360), rotatePoint.X, rotatePoint.Y);
            pathGeometry.Transform = rotateTransform;

            pathGeometry.Freeze();
            path.Data = pathGeometry;
            canvas.Children.Add(path);
            //Canvas.SetLeft(path, 100);
            //Canvas.SetTop(path, 100);
        }
    }
}

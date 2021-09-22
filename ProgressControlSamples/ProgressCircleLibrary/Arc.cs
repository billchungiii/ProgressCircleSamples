using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ProgressCircleLibrary
{
    internal class Arc : Shape
    {

        public static readonly DependencyProperty PercentProperty =
             DependencyProperty.Register(nameof(Percent), typeof(double), typeof(Arc), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, null, new CoerceValueCallback(OnPercentChanged)));

        private static object OnPercentChanged(DependencyObject d, object baseValue)
        {
            var value = (double)baseValue;
            if (value < 0) { value = 0; }
            if (value > 100) { value = 100; }
            return value;
        }

        public double Percent 
        {
            get => (double)GetValue(PercentProperty);
            set => SetValue(PercentProperty, value);
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                StreamGeometry geometry = new StreamGeometry();
                using (StreamGeometryContext context = geometry.Open())
                {

                    context.BeginFigure(new Point(RenderSize.Width / 2.0, StrokeThickness / 2.0), false, false);
                    var end = GetScreenCoordinate();
                    (double radiusX, double radiusY) = GetRadii();
                    context.ArcTo(end, new Size(radiusX > 0 ? radiusX : 0, radiusY > 0 ? radiusY : 0), 0.0, IsLargeArc(), SweepDirection.Clockwise, true, true);
                }

                return geometry;
            }
        }

        

        private bool IsLargeArc()
        {
            return Percent > 50;
        }

        private double EndAngleToPolarAngle()
        {
            var endAngle = 360.0 / 100.0 * Percent;
            if (endAngle > 359.9 ) { endAngle = 359.9; }
            var result = 90.0 - endAngle;            
            if (result < 0) { result += 360.0; }           
            return result;
        }

        private (double radiusX, double radiusY) GetRadii()
        {
            return ((RenderSize.Width - StrokeThickness) / 2.0, (RenderSize.Height - StrokeThickness) / 2.0);
        }

        private Point GetScreenCoordinate()
        {
            var angle = EndAngleToPolarAngle();

            double radian = angle * Math.PI / 180.0;
            (double radiusX, double radiusY) = GetRadii();
            var x = RenderSize.Width / 2.0 + radiusX * Math.Cos(radian);
            var y = (RenderSize.Height - (StrokeThickness / 2.0)) - (radiusY + radiusY * Math.Sin(radian));
            return new Point(x, y);
        }
    }
}

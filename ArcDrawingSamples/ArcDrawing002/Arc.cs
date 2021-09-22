using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ArcDrawing002
{
    public class Arc : Shape
    {
        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register(nameof(EndAngle), typeof(double), typeof(Arc), new PropertyMetadata(0.0, null, new CoerceValueCallback(OnEndAngleChanged)));

        private static object OnEndAngleChanged(DependencyObject d, object baseValue)
        {
            var value = (double)baseValue;
            if (value < 0) { value = 0; }
            if (value > 359.9) { value = 359.9; }
            return value;
        }


        public double EndAngle
        {
            get => (double)GetValue(EndAngleProperty);
            set => SetValue(EndAngleProperty, value);
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                StreamGeometry geometry = new StreamGeometry();
                using (StreamGeometryContext context = geometry.Open())
                {
                    context.BeginFigure(new Point(RenderSize.Width / 2, 0), false, false);
                    var end = GetScreenCoordinate();
                    context.ArcTo(end, new Size(RenderSize.Width / 2, RenderSize.Height / 2), 0.0, IsLargeArc(), SweepDirection.Clockwise, true, true);
                }

                return geometry;
            }
        }

        private bool IsLargeArc()
        {
            return EndAngle > 180;
        }

        private double EndAngleToPolarAngle()
        {
            var result = 90.0 - EndAngle;
            if (result < 0) { result += 360.0; }
            return result;
        }

        private Point GetCenter()
        {
            return new Point(RenderSize.Width / 2, RenderSize.Height / 2);
        }

        private Point GetScreenCoordinate()
        {
            var angle = EndAngleToPolarAngle();
            var center = GetCenter();
            double radian = angle * Math.PI / 180.0;
            var x = center.X + (RenderSize.Width / 2) * Math.Cos(radian);
            var y = RenderSize.Height - (center.Y + (RenderSize.Height / 2) * Math.Sin(radian));
            return new Point(x, y);
        }
    }
}

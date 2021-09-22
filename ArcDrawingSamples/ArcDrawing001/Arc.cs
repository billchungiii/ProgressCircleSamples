using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ArcDrawing001
{
    public class Arc : Shape
    {
        protected override Geometry DefiningGeometry
        {
            get
            {
                StreamGeometry geometry = new StreamGeometry();
                using (StreamGeometryContext context = geometry.Open())
                {
                    context.BeginFigure(new Point(50, 0), false, false);
                    context.ArcTo(new Point(100, 50), new Size(50, 50), 0.0, false, SweepDirection.Clockwise, true, true);
                }

                return geometry;
            }
        }
    }
}

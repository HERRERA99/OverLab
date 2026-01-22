using System.Windows;
using System.Windows.Media;

namespace OverLab.Wpf.Rendering;

public static class SplineHelper
{
    public static PathGeometry CreateSmoothCurve(
        IReadOnlyList<float> values,
        double width,
        double height)
    {
        if (values.Count < 2)
            return new PathGeometry();

        var points = new List<Point>();
        double stepX = width / (values.Count - 1);

        for (int i = 0; i < values.Count; i++)
        {
            points.Add(new Point(
                i * stepX,
                height - (values[i] * height)));
        }

        return CreateCatmullRomGeometry(points);
    }

    private static PathGeometry CreateCatmullRomGeometry(
        IReadOnlyList<Point> pts)
    {
        var figure = new PathFigure { StartPoint = pts[0] };

        for (int i = 0; i < pts.Count - 1; i++)
        {
            Point p0 = i == 0 ? pts[i] : pts[i - 1];
            Point p1 = pts[i];
            Point p2 = pts[i + 1];
            Point p3 = i + 2 < pts.Count ? pts[i + 2] : p2;

            var c1 = new Point(
                p1.X + (p2.X - p0.X) / 6,
                p1.Y + (p2.Y - p0.Y) / 6);

            var c2 = new Point(
                p2.X - (p3.X - p1.X) / 6,
                p2.Y - (p3.Y - p1.Y) / 6);

            figure.Segments.Add(
                new BezierSegment(c1, c2, p2, true));
        }

        return new PathGeometry { Figures = { figure } };
    }
}
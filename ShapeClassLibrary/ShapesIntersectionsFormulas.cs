using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeClassLibrary
{
    public class ShapesIntersectionsFormulas
    {
        public static List<Point> Intersect(Point point1, Point point2)
        {
            var intersectionPoints = new List<Point>();

            if (point1.X == point2.X && point1.Y == point2.Y)
            {
               intersectionPoints.Add(point1);
            }
            return intersectionPoints;
        }
        public static List<Point> Intersect(Point point, Circle circle)
        {
            var intersectionPoints = new List<Point>();

            if (IsPointInsideCircle(point,circle))
            {
                intersectionPoints.Add(point);
            }

            return intersectionPoints;
        }
        public static List<Point> Intersect(Point point, Rect rect)
        {
            var intersectionPoints = new List<Point>();

            if (point.X >= rect.point1.X && point.X <= rect.point2.X && point.Y >= rect.point1.Y && point.Y <= rect.point2.Y)
            {
                intersectionPoints.Add(point);
            }

            return intersectionPoints;
        }
        public static List<Point> Intersect(Point point, Line line)
        {
            var intersectionPoints = new List<Point>();

            var minX = Math.Min(line.point1.X, line.point2.X);
            var minY = Math.Min(line.point1.Y, line.point2.Y);
            var maxX = Math.Max(line.point1.X, line.point2.X);
            var maxY = Math.Max(line.point1.Y, line.point2.Y);

            var crossProduct = (point.Y - line.point1.Y) *
                (line.point2.X - line.point1.X) -
                (point.X - line.point1.X) *
                (line.point2.Y - line.point1.Y);
            if (crossProduct == 0 && point.X >= minX && point.Y >= minY && point.X <= maxX && point.Y <= maxY)
            {
                intersectionPoints.Add(point);
            }

            return intersectionPoints;
        }

        private static bool ClipLine(ref double x1, ref double y1, ref double x2, ref double y2, double clipX1, double clipY1, double clipX2, double clipY2, ref double t1, ref double t2)
        {
            var dx = x2 - x1;
            var dy = y2 - y1;

            double p, q, r;

            for (int edge = 0; edge < 4; edge++)
            {
                if (edge == 0)
                {
                    p = -dx;
                    q = -(clipX1 - x1);
                }
                else if (edge == 1)
                {
                    p = dx;
                    q = (clipX2 - x1);
                }
                else if (edge == 2)
                {
                    p = -dy;
                    q = -(clipY1 - y1);
                }
                else
                {
                    p = dy;
                    q = (clipY2 - y1);
                }

                r = q / p;

                if (p == 0 && q < 0)
                    return false;

                if (p < 0)
                {
                    if (r > t2)
                        return false;
                    else if (r > t1)
                        t1 = r;
                }
                else if (p > 0)
                {
                    if (r < t1)
                        return false;
                    else if (r < t2)
                        t2 = r;
                }
                else if (q < 0)
                    return false;
            }

            return true;
        }

        public static List<Point> Intersect(Rect rect, Line line)
        {
            var intersectionPoints = new List<Point>();

            var t1 = 0.0;
            var t2 = 1.0;

            var x1 = line.point1.X;
            var y1 = line.point1.Y;
            var x2 = line.point2.X;
            var y2 = line.point2.Y;

            var rectX1 = rect.point1.X;
            var rectX2 = rect.point2.X;
            var rectY1 = rect.point1.Y;
            var rectY2 = rect.point2.Y;

            if (ClipLine(ref x1, ref y1, ref x2, ref y2, rectX1, rectY1, rectX2, rectY2, ref t1, ref t2) &&
                ClipLine(ref x1, ref y1, ref x2, ref y2, rectX2, rectY1, rectX2, rectY2, ref t1, ref t2) &&
                ClipLine(ref x1, ref y1, ref x2, ref y2, rectX2, rectY2, rectX1, rectY2, ref t1, ref t2) &&
                ClipLine(ref x1, ref y1, ref x2, ref y2, rectX1, rectY2, rectX1, rectY1, ref t1, ref t2))
            {
                var intersectionPoint1X = x1 + t1 * (x2 - x1);
                var intersectionPoint1Y = y1 + t1 * (y2 - y1);

                var intersectionPoint2X = x1 + t2 * (x2 - x1);
                var intersectionPoint2Y = y1 + t2 * (y2 - y1);

                intersectionPoints.Add(new Point(intersectionPoint1X, intersectionPoint1Y));
                intersectionPoints.Add(new Point(intersectionPoint2X, intersectionPoint2Y));
            }
            return intersectionPoints;
        }

        public static List<Point> Intersect(Rect rect1, Rect rect2)
        {
            var intersectionPoints = new List<Point>();

            var xOverlap = Math.Max(0, Math.Min(rect1.point2.X, rect2.point2.X) - Math.Max(rect1.point1.X, rect2.point1.X));
            var yOverlap = Math.Max(0, Math.Min(rect1.point2.Y, rect2.point2.Y) - Math.Max(rect1.point1.Y, rect2.point1.Y));

            if (xOverlap > 0 && yOverlap > 0)
            {
                var intersectionPoint1X = Math.Max(rect1.point1.X, rect2.point1.X);
                var intersectionPoint1Y = Math.Max(rect1.point1.Y, rect2.point1.Y);

                var intersectionPoint2X = Math.Min(rect1.point2.X, rect2.point2.X);
                var intersectionPoint2Y = Math.Min(rect1.point2.Y, rect2.point2.Y);

                intersectionPoints.Add(new Point(intersectionPoint1X, intersectionPoint1Y));
                intersectionPoints.Add(new Point(intersectionPoint2X, intersectionPoint2Y));
            }

            return intersectionPoints;
        }
        public static List<Point> Intersect(Rect rect, Circle circle)
        {
            var intersectionPoints = new List<Point>();

            var nearestX = Math.Max(rect.point1.X, Math.Min(circle.point.X, rect.point2.X));
            var nearestY = Math.Max(rect.point1.Y, Math.Min(circle.point.Y, rect.point2.Y));
            if (IsPointInsideCircle(new Point(nearestX, nearestY), circle))
            {
                intersectionPoints.Add(new Point(nearestX, nearestY));
            }

            var side1 = new Line(new Point(rect.point1.X, rect.point1.Y), new Point(rect.point2.X, rect.point2.Y));
            var side2 = new Line(new Point(rect.point2.X, rect.point1.Y), new Point(rect.point2.X, rect.point2.Y));
            var side3 = new Line(new Point(rect.point2.X, rect.point2.Y), new Point(rect.point1.X, rect.point2.Y));
            var side4 = new Line(new Point(rect.point1.X, rect.point2.Y), new Point(rect.point1.X, rect.point1.Y));

            intersectionPoints.AddRange(Intersect(side1, circle));
            intersectionPoints.AddRange(Intersect(side2, circle));
            intersectionPoints.AddRange(Intersect(side3, circle));
            intersectionPoints.AddRange(Intersect(side4, circle));
            return intersectionPoints;
        }

        static bool IsPointInsideCircle(Point point, Circle circle)
        {
            var distance = Math.Sqrt(Math.Pow(point.X - circle.point.X, 2) + Math.Pow(point.Y - circle.point.Y, 2));
            return distance <= circle.radius;
        }

        public static List<Point> Intersect(Line line, Circle circle)
        {
            var intersectionPoints = new List<Point>();

            var A = Math.Pow(line.point2.X - line.point1.X, 2) + Math.Pow(line.point2.Y - line.point1.Y, 2);
            var B = 2 * ((line.point2.X - line.point1.X) * (line.point1.X - circle.point.X) + (line.point2.Y - line.point1.Y) * (line.point1.Y - circle.point.Y));
            var C = Math.Pow(line.point1.X, 2) + Math.Pow(line.point1.Y, 2) + Math.Pow(circle.point.X, 2) + Math.Pow(circle.point.Y, 2) - 2 * (line.point1.X * circle.point.X + line.point1.Y * circle.point.Y) - Math.Pow(circle.radius, 2);

            var discriminant = Math.Pow(B, 2) - 4 * A * C;

            if (discriminant >= 0)
            {
                var t1 = (-B + Math.Sqrt(discriminant)) / (2 * A);
                var t2 = (-B - Math.Sqrt(discriminant)) / (2 * A);

                var xIntersection1 = line.point1.X + t1 * (line.point2.X - line.point1.X);
                var yIntersection1 = line.point1.Y + t1 * (line.point2.Y - line.point1.Y);

                var xIntersection2 = line.point1.X + t2 * (line.point2.X - line.point1.X);
                var yIntersection2 = line.point1.Y + t2 * (line.point2.Y - line.point1.Y);

                var intersectionPoint1 = new Point(xIntersection1, yIntersection1);
                var intersectionPoint2 = new Point(xIntersection2, yIntersection2);

                var templateIntersectionList1 = Intersect(intersectionPoint1, line);
                var templateIntersectionList2 = Intersect(intersectionPoint2, line);

                if (templateIntersectionList1.Count > 0) intersectionPoints.Add(new Point(xIntersection1, yIntersection1));
                if (templateIntersectionList2.Count > 0) intersectionPoints.Add(new Point(xIntersection2, yIntersection2));
            }

            return intersectionPoints;
        }
        private static bool IsPointOnSegment(double px, double py, double x1, double y1, double x2, double y2)
        {
            return px >= Math.Min(x1, x2) && px <= Math.Max(x1, x2) &&
                   py >= Math.Min(y1, y2) && py <= Math.Max(y1, y2);
        }
        public static List<Point> Intersect(Line line1, Line line2)
        {
            var intersectionPoints = new List<Point>();

            var m1 = (line1.point2.Y - line1.point1.Y) / (line1.point2.X - line1.point1.X);
            var b1 = line1.point1.Y - m1 * line1.point1.X;

            var m2 = (line2.point2.Y - line2.point1.Y) / (line2.point2.X - line2.point1.X);
            var b2 = line2.point1.Y - m2 * line2.point1.X;

            if (m1 != m2)
            {
                var intersectionX = (b2 - b1) / (m1 - m2);
                var intersectionY = m1 * intersectionX + b1;

                if (IsPointOnSegment(intersectionX, intersectionY, line1.point1.X, line1.point1.Y, line1.point1.X, line1.point2.Y) && IsPointOnSegment(intersectionX, intersectionY, line2.point1.X, line2.point1.Y, line2.point2.X, line2.point2.Y))
                {
                    intersectionPoints.Add(new Point(intersectionX, intersectionY));
                }
            }
            return intersectionPoints;
        }
        public static List<Point> Intersect(Circle circle1, Circle circle2)
        {
            var intersectionPoints = new List<Point>();

            var distance = Math.Sqrt(Math.Pow(circle2.point.X - circle1.point.X, 2) + Math.Pow(circle2.point.Y - circle1.point.Y, 2));

            if (distance <= circle1.radius + circle2.radius && distance >= Math.Abs(circle1.radius - circle2.radius))
            {
                var a = (Math.Pow(circle1.radius, 2) - Math.Pow(circle2.radius, 2) + Math.Pow(distance, 2)) / (2 * distance);

                var p2x = circle1.point.X + a * (circle2.point.X - circle1.point.X) / distance;
                var p2y = circle1.point.Y + a * (circle2.point.Y - circle1.point.Y) / distance;

                var h = Math.Sqrt(Math.Pow(circle1.radius, 2) - Math.Pow(a, 2));

                var intersection1X = p2x + h * (circle2.point.Y - circle1.point.Y) / distance;
                var intersection1Y = p2y - h * (circle2.point.X - circle1.point.X) / distance;

                var intersection2X = p2x - h * (circle2.point.Y - circle1.point.Y) / distance;
                var intersection2Y = p2y + h * (circle2.point.X - circle1.point.X) / distance;

                intersectionPoints.Add(new Point(intersection1X, intersection1Y));
                intersectionPoints.Add(new Point(intersection2X, intersection2Y));
            }
            return intersectionPoints;
        }
    }
}

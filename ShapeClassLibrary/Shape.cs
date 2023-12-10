namespace ShapeClassLibrary
{
    public class Shape
    {
        public readonly List<(double, double)> points;
        public string shapeName { get; protected set; }
        public Shape(List<(double, double)> points) 
        {
            this.points = points;
        }
        public virtual void Draw()
        {
            var shapeInfo = shapeName + " at ";
            foreach(var point in points)
            {
                shapeInfo += point.ToString()+", ";
            }
            Console.Write(shapeInfo);
        }
        protected static List<(double, double)> GetIntersectionPoints(Point point1, Point point2)
        {
            var intersectionPoints = new List<(double, double)>();
            if(point1.x == point2.x && point1.y == point2.y) 
                intersectionPoints.Add((point1.x, point1.y));
            return intersectionPoints;
        }
        protected static List<(double, double)> GetIntersectionPoints(Point point,Circle circle)
        {
            var intersectionPoints = new List<(double, double)> ();
            var distance = Math.Sqrt(Math.Pow(point.x - circle.x, 2) + Math.Pow(point.y - circle.points[0].Item2, 2));
            if(distance <= circle.radius)
            {
                intersectionPoints.Add((point.x, point.y));
            }
            return intersectionPoints;
        }
        protected static List<(double, double)> GetIntersectionPoints(Point point, Rect rect)
        {
            var intersectionPoints = new List<(double, double)>();
            if(point.x >= rect.x1 &&
                point.x <= rect.x2 &&
                point.y >= rect.y1 &&
                point.y <= rect.y2)
            {
                intersectionPoints.Add((point.x, point.y));
            }
            return intersectionPoints;
        }
        protected static List<(double, double)> GetIntersectionPoints(Point point, Line line)
        {
            var intersectionPoints = new List<(double, double)>();
            var minX = Math.Min(line.x1, line.x2);
            var minY = Math.Min(line.y1, line.y2);
            var maxX = Math.Max(line.x1, line.x2);
            var maxY = Math.Max(line.y1, line.y2);

            var crossProduct = (point.y - line.y1) *
                (line.x2 - line.x1) -
                (point.x - line.x1) *
                (line.y2 - line.y1);
            if (crossProduct == 0 && point.x >= minX && point.y >= minY && point.x <= maxX && point.y <= maxY)
            {
                intersectionPoints.Add((point.x, point.y));
            }
            return intersectionPoints;
        }
        static bool ClipLine(ref double x1, ref double y1, ref double x2, ref double y2, double clipX1, double clipY1, double clipX2, double clipY2, ref double t1, ref double t2)
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
        protected static List<(double, double)> GetIntersectionPoints(Rect rect, Line line)
        {
            var intersectionPoints = new List<(double, double)>();
            
            var t1 = 0.0;
            var t2 = 1.0;

            var x1 = line.x1;
            var y1 = line.y1;
            var x2 = line.x2;
            var y2 = line.y2;

            var rectX1 = rect.x1;
            var rectX2 = rect.x2;
            var rectY1 = rect.y1;
            var rectY2 = rect.y2;

            if (ClipLine(ref x1, ref y1, ref x2, ref y2, rectX1, rectY1, rectX2, rectY2, ref t1, ref t2) &&
                ClipLine(ref x1, ref y1, ref x2, ref y2, rectX2, rectY1, rectX2, rectY2, ref t1, ref t2) &&
                ClipLine(ref x1, ref y1, ref x2, ref y2, rectX2, rectY2, rectX1, rectY2, ref t1, ref t2) &&
                ClipLine(ref x1, ref y1, ref x2, ref y2, rectX1, rectY2, rectX1, rectY1, ref t1, ref t2))
            {
                double intersectionPoint1X = x1 + t1 * (x2 - x1);
                double intersectionPoint1Y = y1 + t1 * (y2 - y1);

                double intersectionPoint2X = x1 + t2 * (x2 - x1);
                double intersectionPoint2Y = y1 + t2 * (y2 - y1);

                intersectionPoints.Add((intersectionPoint1X,intersectionPoint1Y));
                intersectionPoints.Add((intersectionPoint2X, intersectionPoint2Y));
            }
            return intersectionPoints;
        }
        protected static List<(double, double)> GetIntersectionPoints(Rect rect1, Rect rect2)
        {
            var intersectionPoints = new List<(double, double)>();
            var xOverlap = Math.Max(0, Math.Min(rect1.x2, rect2.x2) - Math.Max(rect1.x1, rect2.x1));
            var yOverlap = Math.Max(0, Math.Min(rect1.y2, rect2.y2) - Math.Max(rect1.y1, rect2.y1));

            if (xOverlap > 0 && yOverlap > 0)
            {
                var intersectionX1 = Math.Max(rect1.x1, rect2.x1);
                var intersectionY1 = Math.Max(rect1.y1, rect2.y1);

                var intersectionX2 = Math.Min(rect1.x2, rect2.x2);
                var intersectionY2 = Math.Min(rect1.y2, rect2.y2);

                intersectionPoints.Add((intersectionX1, intersectionY1));
                intersectionPoints.Add((intersectionX2, intersectionY2));
            }
            return intersectionPoints;
        }
        protected static List<(double, double)> GetIntersectionPoints(Rect rect, Circle circle)
        {
            var intersectionPoints = new List<(double, double)>();

            var nearestX = Math.Max(rect.x1, Math.Min(circle.x, rect.x2));
            var nearestY = Math.Max(rect.y1, Math.Min(circle.y, rect.y2));

            if (IsPointInsideCircle(nearestX, nearestY, circle.x, circle.y, circle.radius))
            {
                intersectionPoints.Add((nearestX, nearestY));
            }

            var listOfPoints1 = new List<(double, double)>();
            listOfPoints1.Add((rect.x1, rect.y1));
            listOfPoints1.Add((rect.x2, rect.y2));
            var listOfPoints2 = new List<(double, double)>();
            listOfPoints2.Add((rect.x2, rect.y1));
            listOfPoints2.Add((rect.x2, rect.y2));
            var listOfPoints3 = new List<(double, double)>();
            listOfPoints3.Add((rect.x2, rect.y2));
            listOfPoints3.Add((rect.x1, rect.y2));
            var listOfPoints4 = new List<(double, double)>();
            listOfPoints4.Add((rect.x1, rect.y2));
            listOfPoints4.Add((rect.x1, rect.y1));

            var side1 = new Line(listOfPoints1);
            var side2 = new Line(listOfPoints2);
            var side3 = new Line(listOfPoints3);
            var side4 = new Line(listOfPoints4);

            intersectionPoints.AddRange(GetIntersectionPoints(side1, circle));
            intersectionPoints.AddRange(GetIntersectionPoints(side2, circle));
            intersectionPoints.AddRange(GetIntersectionPoints(side3, circle));
            intersectionPoints.AddRange(GetIntersectionPoints(side4, circle));
            return intersectionPoints;
        }

        static bool IsPointInsideCircle(double x, double y, double circleCenterX, double circleCenterY, double circleRadius)
        {
            double distance = Math.Sqrt(Math.Pow(x - circleCenterX, 2) + Math.Pow(y - circleCenterY, 2));
            return distance <= circleRadius;
        }

        protected static List<(double, double)> GetIntersectionPoints(Line line, Circle circle)
        {
            var intersectionPoints = new List<(double, double)>();

            var A = Math.Pow(line.x2 - line.x1, 2) + Math.Pow(line.y2 - line.y1, 2);
            var B = 2 * ((line.x2 - line.x1) * (line.x1 - circle.x) + (line.y2 - line.y1) * (line.y1 - circle.y));
            var C = Math.Pow(line.x1, 2) + Math.Pow(line.y1, 2) + Math.Pow(circle.x, 2) + Math.Pow(circle.y, 2) - 2 * (line.x1 * circle.x + line.y1 * circle.y) - Math.Pow(circle.radius, 2);

            var discriminant = Math.Pow(B, 2) - 4 * A * C;

            if(discriminant >= 0)
            {
                var t1 = (-B + Math.Sqrt(discriminant)) / (2 * A);
                var t2 = (-B - Math.Sqrt(discriminant)) / (2 * A);

                var xIntersection1 = line.x1 + t1 * (line.x2 - line.x1);
                var yIntersection1 = line.y1 + t1 * (line.y2 - line.y1);

                var xIntersection2 = line.x1 + t2 * (line.x2 - line.x1);
                var yIntersection2 = line.y1 + t2 * (line.y2 - line.y1);

                var listPoints1 = new List<(double, double)>();
                listPoints1.Add((xIntersection1, yIntersection1));
                var listPoints2 = new List<(double, double)>();
                listPoints2.Add((xIntersection2, yIntersection2));
                var intersectionPoint1 = new Point(listPoints1);
                var intersectionPoint2 = new Point(listPoints2);

                var templateIntersectionList1 = GetIntersectionPoints(intersectionPoint1, line);
                var templateIntersectionList2 = GetIntersectionPoints(intersectionPoint2, line);

                if (templateIntersectionList1.Count > 0) intersectionPoints.Add((xIntersection1, yIntersection1));
                if (templateIntersectionList2.Count > 0) intersectionPoints.Add((xIntersection2, yIntersection2));
            }
            return intersectionPoints;
        }
        private static bool IsPointOnSegment(double px, double py, double x1, double y1, double x2, double y2)
        {
            return px >= Math.Min(x1, x2) && px <= Math.Max(x1, x2) &&
                   py >= Math.Min(y1, y2) && py <= Math.Max(y1, y2);
        }
        protected static List<(double, double)> GetIntersectionPoints(Line line1, Line line2)
        {
            var intersectionPoints = new List<(double, double)>();

            var m1 = (line1.y2 - line1.y1) / (line1.x2 - line1.x1);
            var b1 = line1.y1 - m1 * line1.x1;

            var m2 = (line2.y2 - line2.y1) / (line2.x2 - line2.x1);
            var b2 = line2.y1 - m2 * line2.x1;

            if (m1 != m2)
            {
                double intersectionX = (b2 - b1) / (m1 - m2);
                double intersectionY = m1 * intersectionX + b1;

                if (IsPointOnSegment(intersectionX, intersectionY, line1.x1, line1.y1, line1.x2, line1.y2) && IsPointOnSegment(intersectionX, intersectionY, line2.x1, line2.y1, line2.x2, line2.y2))
                {
                    intersectionPoints.Add((intersectionX, intersectionY));
                }
            }
            return intersectionPoints;
        }
        protected static List<(double, double)> GetIntersectionPoints(Circle circle1, Circle circle2)
        {
            var intersectionPoints = new List<(double, double)>();

            var distance = Math.Sqrt(Math.Pow(circle2.x- circle1.x, 2) + Math.Pow(circle2.y- circle1.y, 2));

            if (distance <= circle1.radius + circle2.radius && distance >= Math.Abs(circle1.radius - circle2.radius))
            {
                var a = (Math.Pow(circle1.radius, 2) - Math.Pow(circle2.radius, 2) + Math.Pow(distance, 2)) / (2 * distance);

                var p2x = circle1.x + a * (circle2.x - circle1.x) / distance;
                var p2y = circle1.y + a * (circle2.y - circle1.y) / distance;

                var h = Math.Sqrt(Math.Pow(circle1.radius, 2) - Math.Pow(a, 2));

                var intersection1X = p2x + h * (circle2.y - circle1.y) / distance;
                var intersection1Y = p2y - h * (circle2.x- circle1.x) / distance;

                var intersection2X = p2x - h * (circle2.y - circle1.y) / distance;
                var intersection2Y = p2y + h * (circle2.x - circle1.x) / distance;

                intersectionPoints.Add((intersection1X, intersection1Y));
                intersectionPoints.Add((intersection2X, intersection2Y));
            }
            return intersectionPoints;
        }
        public virtual void Intersect(Shape other)
        {

        }
    }
}

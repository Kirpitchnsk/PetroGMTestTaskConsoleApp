namespace ShapeClassLibrary
{
    public class Circle : IShape
    {
        public Point point;
        public readonly int radius;
        public Circle(Point point, int radius)
        {
            this.point = point;
            this.radius = radius;
        }
        public void Draw()
        {
            Console.WriteLine($"Circle at ({point.X}, {point.Y}), radius={radius}");
        }
        public void Intersect(IShape shape)
        {
            var intersectionPoints = new List<Point>();

            switch (shape)
            {
                case Point point:
                    intersectionPoints = ShapesIntersectionsFormulas.Intersect(point, this);
                    if (intersectionPoints.Count > 0)
                    {
                        var pointsInOneString = String.Empty;
                        foreach (var item in intersectionPoints)
                        {
                            pointsInOneString += "(" + item.X + "," + item.Y + ")";
                        }
                        Console.WriteLine($"The circle intersect points at {pointsInOneString}");
                    }
                    else
                    {
                        Console.WriteLine("The circle cannot inersect point");
                    }
                    break;
                case Line line:
                    intersectionPoints = ShapesIntersectionsFormulas.Intersect(line,this);
                    if (intersectionPoints.Count > 0)
                    {
                        Console.WriteLine($"The point intersect line at ({intersectionPoints[0].X}, {intersectionPoints[0].Y})");
                    }
                    else
                    {
                        Console.WriteLine("The point cannot inersect line");
                    }
                    break;
                case Circle circle:
                    intersectionPoints = ShapesIntersectionsFormulas.Intersect(this, circle);
                    if (intersectionPoints.Count > 0)
                    {
                        var pointsInOneString = String.Empty;
                        foreach (var item in intersectionPoints)
                        {
                            pointsInOneString += "(" + item.X + "," + item.Y + ")";
                        }
                        Console.WriteLine($"The line intersect points at {pointsInOneString}");
                    }
                    else
                    {
                        Console.WriteLine("The point 1 cannot inersect circle");
                    }
                    break;
                case Rect rect:
                    intersectionPoints = ShapesIntersectionsFormulas.Intersect(rect, this);
                    if (intersectionPoints.Count > 0)
                    {
                        var pointsInOneString = String.Empty;
                        foreach (var item in intersectionPoints)
                        {
                            pointsInOneString += "(" + item.X + "," + item.Y + ")";
                        }
                        Console.WriteLine($"The line intersect points at {pointsInOneString}");
                    }
                    else
                    {
                        Console.WriteLine("The point 1 cannot inersect rect");
                    }
                    break;
            }
        }
    }
}

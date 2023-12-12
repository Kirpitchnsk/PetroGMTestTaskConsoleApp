namespace ShapeClassLibrary
{
    public class Line : IShape
    {
        public readonly Point point1;
        public readonly Point point2;

        public Line(Point point1,Point point2)
        {
            this.point1 = point1;
            this.point2 = point2;
        }
        public void Draw()
        {
            Console.WriteLine($"line at ({point1.X}, {point1.Y}), ({point2.X}, {point2.Y})");
        }
        public void Intersect(IShape shape)
        {
            var intersectionPoints = new List<Point>();

            switch (shape)
            {
                case Point point:
                    intersectionPoints = ShapesIntersectionsFormulas.Intersect(point,this);
                    if (intersectionPoints.Count > 0)
                    {
                        var pointsInOneString = String.Empty;
                        foreach(var item in intersectionPoints)
                        {
                            pointsInOneString += "("+item.X+","+item.Y+")";
                        }
                        Console.WriteLine($"The line intersect points at {pointsInOneString}");
                    }
                    else
                    {
                        Console.WriteLine("The point 1 cannot inersect point 2");
                    }
                    break;
                case Line line:
                    intersectionPoints = ShapesIntersectionsFormulas.Intersect(this,line);
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
                    intersectionPoints = ShapesIntersectionsFormulas.Intersect(rect,this);
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

namespace ShapeClassLibrary
{
    public class Point:IShape
    {
        public readonly double X;
        public readonly double Y;
        public Point(double x,double y)
        {
            X = x; 
            Y = y;
        }
        public void Draw()
        {
            Console.WriteLine($"Point at {X}, {Y}");
        }
        public void Intersect(IShape shape)
        {
            var intersectionPoints = new List<Point>();

            switch (shape)
            {
                case Point point:
                    intersectionPoints = ShapesIntersectionsFormulas.Intersect(this, point);
                    if(intersectionPoints.Count > 0)
                    {
                        Console.WriteLine($"The point 1 intersect point 2 at ({intersectionPoints[0].X}, {intersectionPoints[0].Y})");
                    }
                    else
                    {
                        Console.WriteLine("The point 1 cannot inersect point 2");
                    }
                    break;
                case Line line:
                    intersectionPoints = ShapesIntersectionsFormulas.Intersect(this, line);
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
                        Console.WriteLine($"The point intersect circle at ({intersectionPoints[0].X}, {intersectionPoints[0].Y})");
                    }
                    else
                    {
                        Console.WriteLine("The point 1 cannot inersect circle");
                    }
                    break;
                case Rect rect:
                    intersectionPoints = ShapesIntersectionsFormulas.Intersect(this, rect);
                    if (intersectionPoints.Count > 0)
                    {
                        Console.WriteLine($"The point 1 intersect rect at ({intersectionPoints[0].X}, {intersectionPoints[0].Y})");
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

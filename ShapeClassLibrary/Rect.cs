namespace ShapeClassLibrary
{
    public class Rect:Shape
    {
        public readonly double x1;
        public readonly double y1;
        public readonly double x2;
        public readonly double y2;
        public Rect(List<(double, double)> points):base(points)
        {
            x1 = points[0].Item1;
            y1 = points[0].Item2;
            x2 = points[1].Item1;
            y2 = points[1].Item2;
            shapeName = "rect";
        }
        public override void Draw()
        {
            base.Draw();
            Console.WriteLine();
        }
        public override void Intersect(Shape other)
        {
            base.Intersect(other);

            var otherShapeName = other.shapeName;

            var intersectionPoints = new List<(double, double)>();

            switch (otherShapeName)
            {
                case "circle":
                    intersectionPoints = GetIntersectionPoints(this, (Circle)other);
                    break;
                case "line":
                    intersectionPoints = GetIntersectionPoints(this,(Line)other);
                    break;
                case "rect":
                    intersectionPoints = GetIntersectionPoints((Rect)other, this);
                    break;
                case "point":
                    intersectionPoints = GetIntersectionPoints((Point)other, this);
                    break;
            }
            if (intersectionPoints.Count > 0)
            {
                if (intersectionPoints.Count == 1)
                {
                    Console.Write($"The {shapeName} and the {otherShapeName} have intersection at {intersectionPoints[0]}");
                }
                else
                {
                    var counter = 0;
                    Console.Write($"The {shapeName} and the {otherShapeName} have intersections at");
                    foreach (var point in intersectionPoints)
                    {
                        if (counter != intersectionPoints.Count) Console.Write(" " + point + " and");
                        else Console.Write(" " + point);
                    }
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"The {shapeName} cannot intersect the {otherShapeName}");
            }
        }
    }
}

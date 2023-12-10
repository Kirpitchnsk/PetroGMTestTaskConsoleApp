namespace ShapeClassLibrary
{
    public class Circle : Shape
    {
        public readonly double x;
        public readonly double y;
        public readonly int radius;
        public Circle(List<(double, double)> points, int radius) : base(points)
        {
            x = points[0].Item1;
            y = points[0].Item2;
            this.radius = radius;
            shapeName = "circle";
        }
        public override void Draw()
        {
            base.Draw();
            Console.WriteLine("radius = " + radius);
        }
        public override void Intersect(Shape other)
        {
            base.Intersect(other);
           
            var otherShapeName = other.shapeName;

            var intersectionPoints = new List<(double, double)> ();

            switch(shapeName)
            {
                case "circle": intersectionPoints = GetIntersectionPoints(this,(Circle)other); 
                    break;
                case "line": intersectionPoints = GetIntersectionPoints((Line)other,this);
                    break;
                case "rect":
                    intersectionPoints = GetIntersectionPoints((Rect)other,this);
                    break;
                case "point":
                    intersectionPoints = GetIntersectionPoints((Point)other,this);
                    break;
            }
            if(intersectionPoints.Count > 0)
            {
                if(intersectionPoints.Count == 1)
                {
                    Console.Write($"The {shapeName} and the {otherShapeName} have intersection at {intersectionPoints[0]}");
                }
                else
                {
                    var counter = 0;
                    Console.Write($"The {shapeName} and the {otherShapeName} have intersections at");
                    foreach (var point in intersectionPoints)
                    {
                        if(counter != intersectionPoints.Count) Console.Write(" " + point + " and");
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

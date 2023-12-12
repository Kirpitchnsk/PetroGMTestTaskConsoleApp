namespace ShapeClassLibrary
{
    public interface IShape
    {
        public void Draw();
        public void Intersect(IShape shape);
    }
}
